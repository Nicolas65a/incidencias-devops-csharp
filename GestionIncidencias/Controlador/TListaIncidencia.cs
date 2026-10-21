using GestionIncidencias.Entidades;
using System.Collections.Generic;

namespace GestionIncidencias.Controlador
{
    public class TListaIncidencia
    {
        public static List<Incidencia> listaIncidencias = new List<Incidencia>();

        public static void Agregar(Incidencia op)
        {
            listaIncidencias.Add(op);
        }

        public static void Modificar(int pos, Incidencia op)
        {
            listaIncidencias[pos] = op;
        }

        public static void Eliminar(int pos)
        {
            listaIncidencias.RemoveAt(pos);
        }

        public static Incidencia getIncidencia(int pos)
        {
            return listaIncidencias[pos];
        }

        public static int Buscar(int id)
        {
            for (int i = 0; i < listaIncidencias.Count; i++)
            {
                if (listaIncidencias[i].Id == id)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
