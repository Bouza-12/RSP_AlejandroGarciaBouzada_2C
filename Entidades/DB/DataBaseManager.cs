using System.Data.SqlClient;
using Entidades.Excepciones;
using Entidades.Exceptions;
using Entidades.Interfaces;

namespace Entidades.DataBase
{
    public static class DataBaseManager
    {
        //Atributos
        private static SqlConnection connection;
        private static string stringConnection;

        //Constructor
        static DataBaseManager()
        {
            DataBaseManager.stringConnection = "Server=DESKTOP-3LVM66M;Database=20230622SP;Trusted_Connection=True;";
        }

        //Métodos

        /// <summary>
        /// Consigue la imagen de una haburguesa de una base de datos
        /// </summary>
        /// <param name="tipo">nombre de la imagen para traer</param>
        /// <returns>string con la dirección de la imagen</returns>
        /// <exception cref="DataBaseManagerException">Error al Traer la Imagen</exception>
        public static string GetImagenComida(string tipo)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(stringConnection))
                {
                    string query = "SELECT * FROM comidas WHERE tipo_comida= @tipo";
                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("tipo", tipo);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        return reader.GetString(2);
                    }
                    else
                    {
                        throw new ComidaInvalidaExeption("No encontro Consulta");
                    }

                }
            }
            catch (ComidaInvalidaExeption ex)
            {
                throw new DataBaseManagerException("Error al leer de la Base de Datos", ex);
            }
        }
        /// <summary>
        /// Guarda los tickets de las haburguesas generadas
        /// </summary>
        /// <typeparam name="T">tipo de objeto debe 
        /// implementar IComestible y tener un constructor sin Parámetros</typeparam>
        /// <param name="nombreEmpleado">nombre del cocinero</param>
        /// <param name="comida">objeto genérico comida</param>
        /// <returns>true si pudo guardar, false si no</returns>
        /// <exception cref="DataBaseManagerException">No pudo conectarse a la base de datos</exception>
        public static bool GuardarTicket<T>(string nombreEmpleado, T comida) where T : IComestible, new()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(stringConnection))
                {
                    string query = "INSERT INTO tickets (empleado, ticket)" +
                        "values(@empleado, @ticket)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("empleado", nombreEmpleado);
                    command.Parameters.AddWithValue("ticket", comida);
                    connection.Open();
                    command.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new DataBaseManagerException("Error al escribir en la Base de Datos", ex);
            }
        }
    }
}
