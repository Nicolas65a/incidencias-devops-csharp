using GestionIncidencias.Entidades;
using GestionIncidencias.Controlador;
using System;
using System.IO;
using System.Collections.Generic;

namespace Pruebas
{
    internal class Program
    {
        private static int aprobadas;
        private static int fallidas;

        private static Incidencia Crear()
        {
            return new Incidencia(1, "Falla al guardar", "La aplicación no guarda el registro.", "Ana", "Alta", new DateTime(2026, 10, 1));
        }

        private static void Comprobar(string nombre, bool condicion)
        {
            if (condicion)
            {
                aprobadas++;
                Console.WriteLine("OK: " + nombre);
            }
            else
            {
                fallidas++;
                Console.WriteLine("ERROR: " + nombre);
            }
        }

        private static int Main()
        {
            aprobadas = 0;
            fallidas = 0;
            Incidencia op = Crear();
            Comprobar("Registro válido", op.ValidarDatos() == "");
            Comprobar("Estado inicial pendiente", op.Estado == "Pendiente" && !op.FechaCierre.HasValue);
            op.Id = 0;
            Comprobar("Código no positivo", op.ValidarDatos() != "");
            op = Crear();
            op.Titulo = "  ";
            Comprobar("Título vacío", op.ValidarDatos() != "");
            op = Crear();
            op.Descripcion = null;
            Comprobar("Descripción nula", op.ValidarDatos() != "");
            op = Crear();
            op.Responsable = "";
            Comprobar("Responsable obligatorio", op.ValidarDatos() != "");
            op = Crear();
            op.Prioridad = "Urgente";
            Comprobar("Prioridad inválida", op.ValidarDatos() != "");
            op = Crear();
            op.Estado = "Otro";
            Comprobar("Estado inválido", op.ValidarDatos() != "");
            op = Crear();
            Comprobar("No resolver pendiente", op.Resolver(new DateTime(2026, 10, 2)) != "" && op.Estado == "Pendiente" && !op.FechaCierre.HasValue);
            Comprobar("Iniciar atención", op.IniciarAtencion() == "" && op.Estado == "En atención");
            Comprobar("No iniciar atención dos veces", op.IniciarAtencion() != "");
            Comprobar("No cerrar antes del registro", op.Resolver(new DateTime(2026, 9, 1)) != "" && op.Estado == "En atención" && !op.FechaCierre.HasValue);
            DateTime cierre = new DateTime(2026, 10, 2);
            Comprobar("Resolver en fecha válida", op.Resolver(cierre) == "" && op.Estado == "Resuelta" && op.FechaCierre == cierre);
            Comprobar("No volver a resolver", op.Resolver(cierre.AddDays(1)) != "" && op.FechaCierre == cierre);
            Comprobar("No atender resuelta", op.IniciarAtencion() != "" && op.Estado == "Resuelta");
            Comprobar("Reabrir una resuelta", op.Reabrir() == "" && op.Estado == "Pendiente" && !op.FechaCierre.HasValue);
            op = Crear();
            Comprobar("No reabrir pendiente", op.Reabrir() != "" && op.Estado == "Pendiente");
            op = Crear();
            op.IniciarAtencion();
            Comprobar("No reabrir en atención", op.Reabrir() != "" && op.Estado == "En atención");
            op = Crear();
            op.FechaCierre = cierre;
            Comprobar("Abierta sin fecha de cierre", op.ValidarDatos() != "");
            op.Estado = "Resuelta";
            op.FechaCierre = null;
            Comprobar("Resuelta requiere fecha", op.ValidarDatos() != "");
            TListaIncidencia.listaIncidencias.Clear();
            op = Crear();
            TListaIncidencia.Agregar(op);
            Comprobar("Agregar y buscar", TListaIncidencia.Buscar(1) == 0 && TListaIncidencia.listaIncidencias.Count == 1);
            Comprobar("Buscar ausente", TListaIncidencia.Buscar(55) == -1);
            Incidencia editada = new Incidencia(1, "Título corregido", op.Descripcion, op.Responsable, op.Prioridad, op.FechaRegistro);
            TListaIncidencia.Modificar(0, editada);
            Comprobar("Modificar", TListaIncidencia.getIncidencia(0).Titulo == "Título corregido");
            TListaIncidencia.Eliminar(0);
            Comprobar("Eliminar", TListaIncidencia.listaIncidencias.Count == 0);
            string carpeta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PruebasIncidencias_" + Guid.NewGuid().ToString("N"));
            string ruta = Path.Combine(carpeta, "datos.xml");
            try
            {
                Comprobar("Archivo nuevo vacío", ArchivoIncidencia.Cargar(ruta).Count == 0);
                List<Incidencia> datos = new List<Incidencia>();
                datos.Add(Crear());
                ArchivoIncidencia.Guardar(ruta, datos);
                List<Incidencia> cargadas = ArchivoIncidencia.Cargar(ruta);
                Comprobar("Guardar y recuperar XML", cargadas.Count == 1 && cargadas[0].Titulo == datos[0].Titulo && cargadas[0].Estado == "Pendiente");
                datos[0].IniciarAtencion();
                datos[0].Resolver(cierre);
                ArchivoIncidencia.Guardar(ruta, datos);
                cargadas = ArchivoIncidencia.Cargar(ruta);
                Comprobar("Actualizar archivo y conservar cierre", cargadas[0].Estado == "Resuelta" && cargadas[0].FechaCierre == cierre);
                datos.Add(Crear());
                ArchivoIncidencia.Guardar(ruta, datos);
                bool duplicado = false;
                try
                {
                    ArchivoIncidencia.Cargar(ruta);
                }
                catch (InvalidDataException)
                {
                    duplicado = true;
                }
                Comprobar("Rechazar códigos duplicados al cargar", duplicado);
                File.WriteAllText(ruta, "<incorrecto>");
                bool danado = false;
                try
                {
                    ArchivoIncidencia.Cargar(ruta);
                }
                catch (InvalidOperationException)
                {
                    danado = true;
                }
                Comprobar("Detectar XML dañado", danado);
            }
            catch (Exception ex)
            {
                Comprobar("Persistencia: " + ex.Message, false);
            }
            finally
            {
                if (Directory.Exists(carpeta))
                {
                    Directory.Delete(carpeta, true);
                }
            }
            Console.WriteLine("Aprobadas: " + aprobadas + ". Fallidas: " + fallidas + ".");
            if (fallidas > 0)
            {
                return 1;
            }
            return 0;
        }
    }
}
