using Entidades.Enumerados;
using Entidades.Exceptions;
using Entidades.Files;
using Entidades.Interfaces;
using Entidades.MetodosDeExtension;
using System.Text;
using Entidades.DataBase;

namespace Entidades.Modelos
{
    public class Hamburguesa : IComestible
    {
        //Atributos
        private static int costoBase;
        private bool esDoble;
        private double costo;
        private bool estado;
        private string imagen;
        List<EIngrediente> ingredientes;
        Random random;
        static Hamburguesa() => Hamburguesa.costoBase = 1500;

        //Constructor
        public Hamburguesa() : this(false) { }
        public Hamburguesa(bool esDoble)
        {
            this.esDoble = esDoble;
            this.random = new Random();
        }

        //Propiedades
        public string Ticket => $"{this}\nTotal a pagar:{this.costo}";

        #region Implementar Interfaz
        public bool Estado
        {
            get { return this.estado; }
        }

        public string Imagen { get { return this.imagen; } }

        //Métodos
        /// <summary>
        /// Asigna ingredientes aleatorios a la instancia
        /// </summary>
        private void AgregarIngredientes()
        {
            this.ingredientes = this.random.IngredientesAleatorios();
        }

        /// <summary>
        /// Cambia el estado de la instancia
        /// Calcula el costo del menu
        /// </summary>
        /// <param name="cocinero"></param>
        public void FinalizarPreparacion(string cocinero)
        {
            this.costo = ingredientes.CalcularCostoIngredientes(Hamburguesa.costoBase);
            this.estado = !this.estado;
        }
        /// <summary>
        /// Asigna una imagen e ingredientes aleatorios  
        /// </summary>
        public void IniciarPreparacion()
        {
            if (!this.estado)
            {
                int i = random.Next(1, 9);
                try
                {
                    this.imagen = DataBaseManager.GetImagenComida($"Hamburguesa_{i}");
                }
                catch (DataBaseManagerException ex)
                {
                    FileManager.Guardar(ex.Message, "log.txt", true);
                }

                this.AgregarIngredientes();
            }
        }
        #endregion
        /// <summary>
        /// Estilo para mostrar los datos de la instancia
        /// </summary>
        /// <returns></returns>
        private string MostrarDatos()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"Hamburguesa {(this.esDoble ? "Doble" : "Simple")}");
            stringBuilder.AppendLine("Ingredientes: ");
            this.ingredientes.ForEach(i => stringBuilder.AppendLine(i.ToString()));
            return stringBuilder.ToString();

        }
        /// <summary>
        /// Sobre escritura del método ToString para mostrar los datos deseados
        /// </summary>
        /// <returns>Datos de la instancia</returns>
        public override string ToString() => this.MostrarDatos();
    }
}