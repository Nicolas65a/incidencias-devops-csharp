using GestionIncidencias.Controlador;
using GestionIncidencias.Entidades;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace GestionIncidencias.Formularios
{
    public partial class frmPrincipal : Form
    {
        private string rutaDatos;
        private bool archivoDisponible;

        public frmPrincipal()
        {
            InitializeComponent();
            this.rutaDatos = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GestionIncidencias", "incidencias.xml");
            this.archivoDisponible = true;
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            cmbEstadoConsulta.SelectedIndex = 0;
            try
            {
                TListaIncidencia.listaIncidencias = ArchivoIncidencia.Cargar(rutaDatos);
            }
            catch (Exception ex)
            {
                archivoDisponible = false;
                btnNuevo.Enabled = false;
                btnModificar.Enabled = false;
                btnEliminar.Enabled = false;
                btnAtender.Enabled = false;
                btnResolver.Enabled = false;
                btnReabrir.Enabled = false;
                MessageBox.Show("No se pudo leer el archivo. Se bloqueó la edición para conservar sus datos. " + ex.Message, "Datos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            Listar();
        }

        private void Listar()
        {
            dgvIncidencias.DataSource = null;
            dgvIncidencias.DataSource = TListaIncidencia.listaIncidencias.ToList();
            dgvIncidencias.ClearSelection();
            lblCantidad.Text = "Incidencias registradas: " + TListaIncidencia.listaIncidencias.Count;
            dgvConsulta.DataSource = null;
            lblResumen.Text = "Seleccione los filtros y pulse Consultar.";
        }

        private int CodigoSeleccionado()
        {
            if (dgvIncidencias.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione una incidencia en la tabla.");
                return 0;
            }
            Incidencia op = (Incidencia)dgvIncidencias.SelectedRows[0].DataBoundItem;
            return op.Id;
        }

        private bool GuardarDatos(List<Incidencia> datos)
        {
            if (!archivoDisponible)
            {
                return false;
            }
            try
            {
                ArchivoIncidencia.Guardar(rutaDatos, datos);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se guardaron los cambios. " + ex.Message, "Archivo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            int codigo = 1;
            foreach (Incidencia actual in TListaIncidencia.listaIncidencias)
            {
                if (actual.Id >= codigo)
                {
                    codigo = actual.Id + 1;
                }
            }
            using (frmAdmin frm = new frmAdmin())
            {
                frm.setCodigo(codigo);
                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    GuardarIncidencia(frm.CrearObjeto());
                }
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            int codigo = CodigoSeleccionado();
            int pos = TListaIncidencia.Buscar(codigo);
            if (pos == -1)
            {
                return;
            }
            using (frmAdmin frm = new frmAdmin())
            {
                frm.setDatos(TListaIncidencia.getIncidencia(pos));
                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    GuardarIncidencia(frm.CrearObjeto());
                }
            }
        }

        private void GuardarIncidencia(Incidencia op)
        {
            int pos = TListaIncidencia.Buscar(op.Id);
            List<Incidencia> datos = TListaIncidencia.listaIncidencias.ToList();
            if (pos >= 0)
            {
                datos[pos] = op;
            }
            else
            {
                datos.Add(op);
            }
            if (!GuardarDatos(datos))
            {
                return;
            }
            if (pos >= 0)
            {
                TListaIncidencia.Modificar(pos, op);
            }
            else
            {
                TListaIncidencia.Agregar(op);
            }
            Listar();
            MessageBox.Show("Incidencia guardada correctamente.");
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            int codigo = CodigoSeleccionado();
            int pos = TListaIncidencia.Buscar(codigo);
            if (pos == -1)
            {
                return;
            }
            DialogResult respuesta = MessageBox.Show("¿Desea eliminar la incidencia " + codigo.ToString("D3") + "?", "Eliminar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (respuesta != DialogResult.Yes)
            {
                return;
            }
            List<Incidencia> datos = TListaIncidencia.listaIncidencias.ToList();
            datos.RemoveAt(pos);
            if (!GuardarDatos(datos))
            {
                return;
            }
            TListaIncidencia.Eliminar(pos);
            Listar();
        }

        private void btnAtender_Click(object sender, EventArgs e)
        {
            CambiarEstado(false);
        }

        private void btnResolver_Click(object sender, EventArgs e)
        {
            CambiarEstado(true);
        }

        private void btnReabrir_Click(object sender, EventArgs e)
        {
            int codigo = CodigoSeleccionado();
            int pos = TListaIncidencia.Buscar(codigo);
            if (pos == -1)
            {
                return;
            }
            Incidencia anterior = TListaIncidencia.getIncidencia(pos);
            Incidencia op = new Incidencia(anterior.Id, anterior.Titulo, anterior.Descripcion, anterior.Responsable, anterior.Prioridad, anterior.FechaRegistro);
            op.Estado = anterior.Estado;
            op.FechaCierre = anterior.FechaCierre;
            string mensaje = op.Reabrir();
            if (mensaje != "")
            {
                MessageBox.Show(mensaje);
                return;
            }
            List<Incidencia> datos = TListaIncidencia.listaIncidencias.ToList();
            datos[pos] = op;
            if (!GuardarDatos(datos))
            {
                return;
            }
            TListaIncidencia.Modificar(pos, op);
            Listar();
            MessageBox.Show("Incidencia reabierta. Su estado es Pendiente.");
        }

        private void CambiarEstado(bool resolver)
        {
            int codigo = CodigoSeleccionado();
            int pos = TListaIncidencia.Buscar(codigo);
            if (pos == -1)
            {
                return;
            }
            Incidencia anterior = TListaIncidencia.getIncidencia(pos);
            Incidencia op = new Incidencia(anterior.Id, anterior.Titulo, anterior.Descripcion, anterior.Responsable, anterior.Prioridad, anterior.FechaRegistro);
            op.Estado = anterior.Estado;
            op.FechaCierre = anterior.FechaCierre;
            string mensaje;
            if (resolver)
            {
                mensaje = op.Resolver(DateTime.Now);
            }
            else
            {
                mensaje = op.IniciarAtencion();
            }
            if (mensaje != "")
            {
                MessageBox.Show(mensaje);
                return;
            }
            List<Incidencia> datos = TListaIncidencia.listaIncidencias.ToList();
            datos[pos] = op;
            if (!GuardarDatos(datos))
            {
                return;
            }
            TListaIncidencia.Modificar(pos, op);
            Listar();
            MessageBox.Show("Estado actualizado: " + op.Estado + ".");
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            string estado = cmbEstadoConsulta.Text;
            string responsable = txtBuscarResponsable.Text.Trim();
            var sql = from op in TListaIncidencia.listaIncidencias
                      where (estado == "Todos" || op.Estado == estado)
                      && (responsable == "" || op.Responsable.IndexOf(responsable, StringComparison.OrdinalIgnoreCase) >= 0)
                      orderby op.Id
                      select op;
            List<Incidencia> resultado = sql.ToList();
            dgvConsulta.DataSource = null;
            dgvConsulta.DataSource = resultado;
            lblResumen.Text = "Resultados: " + resultado.Count + "    Pendientes: " + resultado.Count(op => op.Estado == "Pendiente") + "    En atención: " + resultado.Count(op => op.Estado == "En atención") + "    Resueltas: " + resultado.Count(op => op.Estado == "Resuelta");
        }

        private void btnResumen_Click(object sender, EventArgs e)
        {
            var sql = from op in TListaIncidencia.listaIncidencias
                      group op by op.Responsable into grupo
                      orderby grupo.Key
                      select new
                      {
                          Responsable = grupo.Key,
                          Total = grupo.Count(),
                          Pendientes = grupo.Count(op => op.Estado == "Pendiente"),
                          EnAtencion = grupo.Count(op => op.Estado == "En atención"),
                          Resueltas = grupo.Count(op => op.Estado == "Resuelta")
                      };
            dgvConsulta.DataSource = null;
            dgvConsulta.DataSource = sql.ToList();
            lblResumen.Text = "Resumen de todas las incidencias por responsable.";
        }

        private void dgvConsulta_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewColumn columna in dgvConsulta.Columns)
            {
                if (columna.DataPropertyName == "Descripcion")
                {
                    columna.Visible = false;
                }
                if (columna.DataPropertyName == "Id")
                {
                    columna.HeaderText = "Código";
                    columna.FillWeight = 55;
                    columna.DefaultCellStyle.Format = "D3";
                }
                if (columna.DataPropertyName == "Titulo")
                {
                    columna.HeaderText = "Título";
                    columna.FillWeight = 180;
                }
                if (columna.DataPropertyName == "FechaRegistro")
                {
                    columna.HeaderText = "Registro";
                    columna.DefaultCellStyle.Format = "dd/MM/yyyy";
                }
                if (columna.DataPropertyName == "FechaCierre")
                {
                    columna.HeaderText = "Cierre";
                    columna.DefaultCellStyle.Format = "dd/MM/yyyy";
                }
                if (columna.DataPropertyName == "EnAtencion")
                {
                    columna.HeaderText = "En atención";
                }
            }
        }
    }
}
