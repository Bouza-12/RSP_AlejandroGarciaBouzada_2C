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
            FileManager.path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "SP_07122023_AlejandroGarciaBozuada");
            FileManager.ValidaExistenciaDeDirectorio();
        }

        public static void Guardar(string data, string nombreArchivo, bool append)
        {
            try
            {
                string rutaArchivo = Path.Combine(path, nombreArchivo);
                using (StreamWriter sw = new StreamWriter(rutaArchivo, append))
                {
                    sw.WriteLine(data);
                }
            }catch(Exception e)
            {
                throw new FileManagerException("Error al guardar el archivo", e);
            }
        }
        public static bool Serializar<T>(T elemento,  string nombreArchivo)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions();
                options.WriteIndented = true;
                string objetoSerializado = JsonSerializer.Serialize(elemento, typeof(T), options);
                FileManager.Guardar(objetoSerializado, nombreArchivo, false);
                return true;
            }
            catch(Exception e)
            {
                throw new FileManagerException("Error al serializar el json", e);
            }
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
