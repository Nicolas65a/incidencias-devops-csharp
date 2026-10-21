using GestionIncidencias.Entidades;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace GestionIncidencias.Controlador
{
    public class ArchivoIncidencia
    {
        public static List<Incidencia> Cargar(string ruta)
        {
            if (!File.Exists(ruta))
            {
                return new List<Incidencia>();
            }
            XmlSerializer serializador = new XmlSerializer(typeof(List<Incidencia>));
            List<Incidencia> datos;
            using (FileStream archivo = new FileStream(ruta, FileMode.Open, FileAccess.Read))
            {
                datos = (List<Incidencia>)serializador.Deserialize(archivo);
            }
            List<int> codigos = new List<int>();
            foreach (Incidencia op in datos)
            {
                if (op == null || op.ValidarDatos() != "" || codigos.Contains(op.Id))
                {
                    throw new InvalidDataException("El archivo contiene incidencias inválidas o códigos repetidos.");
                }
                codigos.Add(op.Id);
            }
            return datos;
        }

        public static void Guardar(string ruta, List<Incidencia> datos)
        {
            string carpeta = Path.GetDirectoryName(Path.GetFullPath(ruta));
            Directory.CreateDirectory(carpeta);
            string temporal = ruta + ".tmp";
            XmlSerializer serializador = new XmlSerializer(typeof(List<Incidencia>));
            using (FileStream archivo = new FileStream(temporal, FileMode.Create, FileAccess.Write))
            {
                serializador.Serialize(archivo, datos);
            }
            if (File.Exists(ruta))
            {
                File.Replace(temporal, ruta, null);
            }
            else
            {
                File.Move(temporal, ruta);
            }
        }
    }
}
