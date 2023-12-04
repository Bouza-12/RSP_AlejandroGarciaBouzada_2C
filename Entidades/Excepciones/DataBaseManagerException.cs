using Entidades.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Exceptions
{
    public class DataBaseManagerException : Exception
    {
        public DataBaseManagerException(string? message) : base(message)
        {
            FileManager.Guardar(message, "log.txt", true);
        }
        public DataBaseManagerException(string? message, Exception? innerException) : base(message, innerException)
        {
            FileManager.Guardar(message, "log.txt", true);
        }
    }
}
