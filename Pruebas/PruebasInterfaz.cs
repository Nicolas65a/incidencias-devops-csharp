using GestionIncidencias.Controlador;
using GestionIncidencias.Entidades;
using GestionIncidencias.Formularios;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Pruebas
{
    internal class PruebasInterfaz
    {
        private delegate bool VisitarVentana(IntPtr ventana, IntPtr dato);

        [DllImport("kernel32.dll")]
        private static extern uint GetCurrentThreadId();
        [DllImport("user32.dll")]
        private static extern bool EnumThreadWindows(uint hilo, VisitarVentana visitar, IntPtr dato);
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetClassName(IntPtr ventana, StringBuilder nombre, int capacidad);
        [DllImport("user32.dll")]
        private static extern bool EndDialog(IntPtr ventana, IntPtr resultado);

        private static void Ejecutar(Form frm, string metodo, params object[] argumentos)
        {
            MethodInfo accion = frm.GetType().GetMethod(metodo, BindingFlags.Instance | BindingFlags.NonPublic);
            accion.Invoke(frm, argumentos);
        }

        private static T Control<T>(Form frm, string nombre) where T : Control
        {
            return (T)frm.Controls.Find(nombre, true)[0];
        }

        private static void Ruta(frmPrincipal frm, string ruta)
        {
            FieldInfo campo = typeof(frmPrincipal).GetField("rutaDatos", BindingFlags.Instance | BindingFlags.NonPublic);
            campo.SetValue(frm, ruta);
        }

        private static bool ConMensaje(Action accion, int respuesta)
        {
            bool encontrado = false;
            uint hilo = GetCurrentThreadId();
            using (Timer reloj = new Timer())
            {
                reloj.Interval = 50;
                reloj.Tick += delegate
                {
                    EnumThreadWindows(hilo, delegate(IntPtr ventana, IntPtr dato)
                    {
                        StringBuilder clase = new StringBuilder(80);
                        GetClassName(ventana, clase, clase.Capacity);
                        if (clase.ToString() == "#32770")
                        {
                            encontrado = true;
                            EndDialog(ventana, new IntPtr(respuesta));
                        }
                        return true;
                    }, IntPtr.Zero);
                };
                reloj.Start();
                accion();
                reloj.Stop();
            }
            return encontrado;
        }

        private static void ConEditor(Action accion, string titulo)
        {
            using (Timer reloj = new Timer())
            {
                reloj.Interval = 50;
                reloj.Tick += delegate
                {
                    frmAdmin editor = Application.OpenForms.OfType<frmAdmin>().FirstOrDefault();
                    if (editor != null)
                    {
                        reloj.Stop();
                        Control<TextBox>(editor, "txtTitulo").Text = titulo;
                        Control<TextBox>(editor, "txtDescripcion").Text = "Detalle valido";
                        Control<TextBox>(editor, "txtResponsable").Text = "Ana";
                        Control<Button>(editor, "btnGuardar").PerformClick();
                    }
                };
                reloj.Start();
                accion();
                reloj.Stop();
            }
        }

        private static void Clic(Form frm, string nombre)
        {
            Ejecutar(frm, nombre + "_Click", frm, EventArgs.Empty);
        }

        private static Incidencia Crear(int codigo, string responsable)
        {
            return new Incidencia(codigo, "Problema " + codigo, "Detalle del problema", responsable, "Alta", new DateTime(2026, 10, 1));
        }

        private static frmPrincipal Abrir(string ruta)
        {
            frmPrincipal frm = new frmPrincipal();
            Ruta(frm, ruta);
            frm.Show();
            Application.DoEvents();
            return frm;
        }

        private static List<Incidencia> Resultado(frmPrincipal frm)
        {
            return (List<Incidencia>)Control<DataGridView>(frm, "dgvConsulta").DataSource;
        }

        private static void Seleccionar(frmPrincipal frm)
        {
            DataGridView tabla = Control<DataGridView>(frm, "dgvIncidencias");
            tabla.ClearSelection();
            tabla.Rows[0].Selected = true;
        }

        public static void Ejecutar(Action<string, bool> comprobar)
        {
            string carpeta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "IncidenciasInterfaz_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(carpeta);
            List<Incidencia> anteriores = TListaIncidencia.listaIncidencias;
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Consultas(carpeta, comprobar);
                Administracion(carpeta, comprobar);
                Errores(carpeta, comprobar);
            }
            catch (Exception ex)
            {
                comprobar("Interfaz: " + ex.ToString(), false);
            }
            finally
            {
                TListaIncidencia.listaIncidencias = anteriores;
                Directory.Delete(carpeta, true);
            }
        }

        private static void Consultas(string carpeta, Action<string, bool> comprobar)
        {
            string ruta = Path.Combine(carpeta, "consultas.xml");
            Incidencia primera = Crear(1, "Ana");
            Incidencia segunda = Crear(2, "Ana");
            segunda.IniciarAtencion();
            segunda.Resolver(new DateTime(2026, 10, 2));
            Incidencia tercera = Crear(3, "Luis");
            tercera.IniciarAtencion();
            ArchivoIncidencia.Guardar(ruta, new List<Incidencia> { tercera, primera, segunda });
            using (frmPrincipal frm = Abrir(ruta))
            {
                Clic(frm, "btnConsultar");
                comprobar("CA10 Sin filtros muestra todos en orden", Resultado(frm).Select(op => op.Id).SequenceEqual(new int[] { 1, 2, 3 }));
                Control<ComboBox>(frm, "cmbEstadoConsulta").SelectedItem = "Pendiente";
                Clic(frm, "btnConsultar");
                comprobar("CA10 Filtro por estado", Resultado(frm).Count == 1 && Resultado(frm)[0].Id == 1);
                Control<ComboBox>(frm, "cmbEstadoConsulta").SelectedItem = "Todos";
                Control<TextBox>(frm, "txtBuscarResponsable").Text = " aN ";
                Clic(frm, "btnConsultar");
                comprobar("CA10 Nombre parcial sin distinguir mayusculas", Resultado(frm).Count == 2);
                Control<ComboBox>(frm, "cmbEstadoConsulta").SelectedItem = "Resuelta";
                Clic(frm, "btnConsultar");
                comprobar("CA10 Combina ambos filtros", Resultado(frm).Count == 1 && Resultado(frm)[0].Id == 2);
                Control<TextBox>(frm, "txtBuscarResponsable").Text = "No existe";
                Clic(frm, "btnConsultar");
                comprobar("CA10 Consulta sin coincidencias", Resultado(frm).Count == 0);
                Clic(frm, "btnResumen");
                IList resumen = (IList)Control<DataGridView>(frm, "dgvConsulta").DataSource;
                object ana = resumen[0];
                object luis = resumen[1];
                comprobar("CA11 Resumen de todos los estados y responsables", resumen.Count == 2
                    && Valor(ana, "Responsable").Equals("Ana") && Valor(ana, "Total").Equals(2)
                    && Valor(ana, "Pendientes").Equals(1) && Valor(ana, "Resueltas").Equals(1)
                    && Valor(ana, "EnAtencion").Equals(0) && Valor(luis, "Total").Equals(1)
                    && Valor(luis, "EnAtencion").Equals(1));
                comprobar("CA15 Tabla principal de solo lectura", Control<DataGridView>(frm, "dgvIncidencias").ReadOnly);
            }
        }

        private static object Valor(object fila, string propiedad)
        {
            return fila.GetType().GetProperty(propiedad).GetValue(fila, null);
        }

        private static void Administracion(string carpeta, Action<string, bool> comprobar)
        {
            Incidencia original = Crear(7, "Ana");
            original.IniciarAtencion();
            original.Resolver(new DateTime(2026, 10, 2));
            using (frmAdmin frm = new frmAdmin())
            {
                frm.setDatos(original);
                frm.Show();
                Application.DoEvents();
                Control<TextBox>(frm, "txtTitulo").Text = "Titulo corregido";
                Incidencia editada = frm.CrearObjeto();
                comprobar("CA03 Modificar conserva codigo estado y fechas", editada.Id == original.Id
                    && editada.Estado == original.Estado && editada.FechaRegistro == original.FechaRegistro
                    && editada.FechaCierre == original.FechaCierre && editada.Titulo == "Titulo corregido");
                Clic(frm, "btnCancelar");
                comprobar("CA15 Cancelar no modifica el objeto original", frm.DialogResult == DialogResult.Cancel && original.Titulo == "Problema 7");
            }
            using (frmAdmin frm = new frmAdmin())
            {
                frm.setCodigo(8);
                frm.Show();
                Application.DoEvents();
                bool mensaje = ConMensaje(delegate
                {
                    Clic(frm, "btnGuardar");
                }, 1);
                comprobar("CA02 Guardar campos vacios mantiene abierto el editor", mensaje && frm.Visible && frm.DialogResult != DialogResult.OK);
                Control<TextBox>(frm, "txtTitulo").Text = "Nuevo problema";
                Control<TextBox>(frm, "txtDescripcion").Text = "Descripcion valida";
                Control<TextBox>(frm, "txtResponsable").Text = "Luis";
                Clic(frm, "btnGuardar");
                comprobar("CA01 CA15 Guardar editor devuelve registro pendiente", frm.DialogResult == DialogResult.OK
                    && frm.CrearObjeto().Id == 8 && frm.CrearObjeto().Estado == "Pendiente");
            }
            string ruta = Path.Combine(carpeta, "administracion.xml");
            ArchivoIncidencia.Guardar(ruta, new List<Incidencia> { original });
            using (frmPrincipal frm = Abrir(ruta))
            {
                ConMensaje(delegate
                {
                    ConEditor(delegate
                    {
                        Clic(frm, "btnNuevo");
                    }, "Nuevo desde boton");
                }, 1);
                comprobar("CA01 Nuevo calcula un codigo unico desde la tabla", TListaIncidencia.listaIncidencias.Count == 2
                    && TListaIncidencia.Buscar(8) >= 0 && TListaIncidencia.getIncidencia(TListaIncidencia.Buscar(8)).Estado == "Pendiente");
                Seleccionar(frm);
                ConMensaje(delegate
                {
                    ConEditor(delegate
                    {
                        Clic(frm, "btnModificar");
                    }, "Modificado desde boton");
                }, 1);
                Incidencia modificada = TListaIncidencia.getIncidencia(0);
                comprobar("CA03 Modificar desde boton conserva fechas al guardar", modificada.Id == 7
                    && modificada.Titulo == "Modificado desde boton" && modificada.Estado == "Resuelta"
                    && modificada.FechaRegistro == original.FechaRegistro && modificada.FechaCierre == original.FechaCierre);
                TListaIncidencia.listaIncidencias.RemoveAt(1);
                ArchivoIncidencia.Guardar(ruta, TListaIncidencia.listaIncidencias);
                Ejecutar(frm, "Listar");
                Seleccionar(frm);
                bool mensaje = ConMensaje(delegate
                {
                    Clic(frm, "btnEliminar");
                }, 7);
                comprobar("CA03 Rechazar confirmacion conserva registro y archivo", mensaje && TListaIncidencia.listaIncidencias.Count == 1 && ArchivoIncidencia.Cargar(ruta).Count == 1);
                Seleccionar(frm);
                ConMensaje(delegate
                {
                    Clic(frm, "btnEliminar");
                }, 6);
                comprobar("CA03 Confirmar eliminacion actualiza lista y archivo", TListaIncidencia.listaIncidencias.Count == 0 && ArchivoIncidencia.Cargar(ruta).Count == 0);
                ConMensaje(delegate
                {
                    Ejecutar(frm, "GuardarIncidencia", Crear(9, "Luis"));
                }, 1);
            }
            using (frmPrincipal frm = Abrir(ruta))
            {
                comprobar("CA04 Reabrir formulario recupera lo guardado", TListaIncidencia.listaIncidencias.Count == 1 && TListaIncidencia.getIncidencia(0).Id == 9);
            }
        }

        private static void Errores(string carpeta, Action<string, bool> comprobar)
        {
            string ruta = Path.Combine(carpeta, "errores.xml");
            ArchivoIncidencia.Guardar(ruta, new List<Incidencia> { Crear(1, "Ana") });
            string bloqueo = Path.Combine(carpeta, "bloqueo");
            File.WriteAllText(bloqueo, "Este archivo impide crear una carpeta con el mismo nombre.");
            using (frmPrincipal frm = Abrir(ruta))
            {
                Ruta(frm, Path.Combine(bloqueo, "datos.xml"));
                ConMensaje(delegate
                {
                    Ejecutar(frm, "GuardarIncidencia", Crear(2, "Luis"));
                }, 1);
                comprobar("CA12 Fallo al agregar conserva lista y archivo", TListaIncidencia.listaIncidencias.Count == 1 && ArchivoIncidencia.Cargar(ruta).Count == 1);
                Incidencia cambio = Crear(1, "Luis");
                ConMensaje(delegate
                {
                    Ejecutar(frm, "GuardarIncidencia", cambio);
                }, 1);
                comprobar("CA12 Fallo al modificar conserva datos anteriores", TListaIncidencia.getIncidencia(0).Responsable == "Ana" && ArchivoIncidencia.Cargar(ruta)[0].Responsable == "Ana");
                Seleccionar(frm);
                ConMensaje(delegate
                {
                    Clic(frm, "btnAtender");
                }, 1);
                comprobar("CA12 Fallo al cambiar estado conserva estado y fecha", TListaIncidencia.getIncidencia(0).Estado == "Pendiente" && !TListaIncidencia.getIncidencia(0).FechaCierre.HasValue);
                Seleccionar(frm);
                ConMensaje(delegate
                {
                    Clic(frm, "btnEliminar");
                }, 6);
                comprobar("CA12 Fallo al eliminar conserva registro", TListaIncidencia.listaIncidencias.Count == 1 && ArchivoIncidencia.Cargar(ruta).Count == 1);
            }
            string danado = Path.Combine(carpeta, "danado.xml");
            File.WriteAllText(danado, "<incorrecto>");
            using (frmPrincipal frm = new frmPrincipal())
            {
                Ruta(frm, danado);
                bool mensaje = ConMensaje(delegate
                {
                    frm.Show(); Application.DoEvents();
                }, 1);
                string[] botones = { "btnNuevo", "btnModificar", "btnEliminar", "btnAtender", "btnResolver", "btnReabrir" };
                bool bloqueados = true;
                foreach (string nombre in botones)
                {
                    if (Control<Button>(frm, nombre).Enabled)
                    {
                        bloqueados = false;
                    }
                }
                comprobar("CA13 Archivo danado bloquea todas las acciones de edicion", mensaje && bloqueados && File.ReadAllText(danado) == "<incorrecto>");
            }
        }
    }
}
