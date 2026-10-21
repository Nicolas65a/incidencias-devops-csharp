using System;

namespace GestionIncidencias.Entidades
{
    public class Incidencia
    {
        private int id;
        private string titulo;
        private string descripcion;
        private string responsable;
        private string prioridad;
        private string estado;
        private DateTime fechaRegistro;
        private DateTime? fechaCierre;

        public Incidencia()
        {

        }

        public Incidencia(int id, string titulo, string descripcion, string responsable, string prioridad, DateTime fechaRegistro)
        {
            this.id = id;
            this.titulo = titulo;
            this.descripcion = descripcion;
            this.responsable = responsable;
            this.prioridad = prioridad;
            this.fechaRegistro = fechaRegistro;
            this.estado = "Pendiente";
            this.fechaCierre = null;
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Titulo
        {
            get { return titulo; }
            set { titulo = value; }
        }

        public string Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; }
        }

        public string Responsable
        {
            get { return responsable; }
            set { responsable = value; }
        }

        public string Prioridad
        {
            get { return prioridad; }
            set { prioridad = value; }
        }

        public string Estado
        {
            get { return estado; }
            set { estado = value; }
        }

        public DateTime FechaRegistro
        {
            get { return fechaRegistro; }
            set { fechaRegistro = value; }
        }

        public DateTime? FechaCierre
        {
            get { return fechaCierre; }
            set { fechaCierre = value; }
        }

        public string ValidarDatos()
        {
            if (Id <= 0)
            {
                return "El código debe ser positivo.";
            }
            if (Titulo == null || Titulo.Trim() == "")
            {
                return "Escriba el título de la incidencia.";
            }
            if (Descripcion == null || Descripcion.Trim() == "")
            {
                return "Describa el problema encontrado.";
            }
            if (Responsable == null || Responsable.Trim() == "")
            {
                return "Escriba el nombre del responsable.";
            }
            if (Prioridad != "Alta" && Prioridad != "Media" && Prioridad != "Baja")
            {
                return "Seleccione una prioridad válida.";
            }
            if (Estado != "Pendiente" && Estado != "En atención" && Estado != "Resuelta")
            {
                return "El estado de la incidencia no es válido.";
            }
            if (Estado == "Resuelta" && (!FechaCierre.HasValue || FechaCierre.Value < FechaRegistro))
            {
                return "La fecha de cierre no es válida.";
            }
            if (Estado != "Resuelta" && FechaCierre.HasValue)
            {
                return "Una incidencia abierta no debe tener fecha de cierre.";
            }
            return "";
        }

        public string IniciarAtencion()
        {
            if (Estado != "Pendiente")
            {
                return "Solo se puede atender una incidencia pendiente.";
            }
            Estado = "En atención";
            return "";
        }

        public string Reabrir()
        {
            return "La reapertura todavía no está implementada.";
        }

        public string Resolver(DateTime fecha)
        {
            if (Estado != "En atención")
            {
                return "Primero debe iniciar la atención de la incidencia.";
            }
            if (fecha < FechaRegistro)
            {
                return "La fecha de cierre no puede ser anterior al registro.";
            }
            Estado = "Resuelta";
            FechaCierre = fecha;
            return "";
        }
    }
}
