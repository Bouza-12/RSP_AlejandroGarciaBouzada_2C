using System.Data.SqlClient;
using Entidades.Excepciones;
using Entidades.Exceptions;
using Entidades.Interfaces;

namespace Entidades.DataBase
{
    public static class DataBaseManager
    {
        private static SqlConnection connection;
        private static string stringConnection;

        static DataBaseManager()
        {
            DataBaseManager.stringConnection = "Server=DESKTOP-3LVM66M;Database=20230622SP;Trusted_Connection=True;";
        }
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
                throw new DataBaseManagerException("Error al leer", ex);
            }
        }
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
                throw new DataBaseManagerException("Error al escribir", ex);
            }
        }
    }
}
