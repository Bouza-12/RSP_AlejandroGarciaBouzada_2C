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
        //atributo
        private static string path;

        //constructor
        static FileManager() 
        {
            FileManager.path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "SP_07122023_AlejandroGarciaBozuada");
            FileManager.ValidaExistenciaDeDirectorio();
        }

        //métodos
        /// <summary>
        /// Guarda un archivo o apendea un texto
        /// </summary>
        /// <param name="data">texto a guardar</param>
        /// <param name="nombreArchivo">nombre del archivo donde guardar</param>
        /// <param name="append">true para appendear o false para sobreescribir</param>
        /// <exception cref="FileManagerException">problemas al guardar el archivo</exception>
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
        /// <summary>
        /// Serializa un objeto genérico a Json
        /// </summary>
        /// <typeparam name="T">Objeto genético a serializar solo por tipo de referencia</typeparam>
        /// <param name="elemento">objeto</param>
        /// <param name="nombreArchivo">nombre del archivo que guarda</param>
        /// <returns></returns>
        /// <exception cref="FileManagerException"></exception>
        public static bool Serializar<T>(T elemento,  string nombreArchivo) where T: class
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

        /// <summary>
        /// Valida la existancia del directorio, si no la crea
        /// </summary>
        /// <exception cref="FileManagerException"></exception>
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
