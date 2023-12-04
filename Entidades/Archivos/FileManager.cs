using Entidades.Exceptions;
using Entidades.Interfaces;
using Entidades.Modelos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Entidades.Files
{
    
    public static class FileManager
    {
        private static string path;

        static FileManager() 
        {
            path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "SP_07122023_AlejandroGarciaBozuada");
            FileManager.ValidaExistenciaDeDirectorio();
        }

        public static void Guardar(string data, string nombreArchivo, bool append)
        {
            FileManager.ValidaExistenciaDeDirectorio();
            string rutaArchivo = Path.Combine(path, nombreArchivo);
            using (StreamWriter sw = new StreamWriter(rutaArchivo, append))
            {
                sw.WriteLine(data);
            }
        }
        public static bool Serializar<T>(T elemento,  string nombreArchivo)
        {
            string rutaCompleta = Path.Combine(path, nombreArchivo);
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.WriteIndented = true;
            string objetoSerializado = JsonSerializer.Serialize(elemento, options);
            FileManager.Guardar(objetoSerializado, nombreArchivo, true);
            return true;
        }
        private static void ValidaExistenciaDeDirectorio()
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

            }
            catch
            {
                throw new FileManagerException("Error al crear el directorio");
            }
        }
    }
}
