using GestionIncidencias.Entidades;
using System;
using System.Windows.Forms;

namespace GestionIncidencias.Formularios
{
    public partial class frmAdmin : Form
    {
        private int codigo;
        private Incidencia anterior;

        public frmAdmin()
        {
            InitializeComponent();
            this.codigo = 0;
            this.anterior = null;
            cmbPrioridad.SelectedIndex = 1;
        }

        public void setCodigo(int codigo)
        {
            this.codigo = codigo;
            txtCodigo.Text = codigo.ToString("D3");
        }

        public void setDatos(Incidencia op)
        {
            this.anterior = op;
            setCodigo(op.Id);
            txtTitulo.Text = op.Titulo;
            txtDescripcion.Text = op.Descripcion;
            txtResponsable.Text = op.Responsable;
            cmbPrioridad.SelectedItem = op.Prioridad;
            lblEdicion.Text = "Modificar incidencia " + txtCodigo.Text;
            this.Text = "Modificar incidencia";
        }

        public Incidencia CrearObjeto()
        {
            Incidencia op = new Incidencia(codigo, txtTitulo.Text.Trim(), txtDescripcion.Text.Trim(), txtResponsable.Text.Trim(), cmbPrioridad.Text, DateTime.Now);
            if (anterior != null)
            {
                op.Estado = anterior.Estado;
                op.FechaRegistro = anterior.FechaRegistro;
                op.FechaCierre = anterior.FechaCierre;
            }
            return op;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            Incidencia op = CrearObjeto();
            string mensaje = op.ValidarDatos();
            if (mensaje != "")
            {
                MessageBox.Show(mensaje);
                return;
            }
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
