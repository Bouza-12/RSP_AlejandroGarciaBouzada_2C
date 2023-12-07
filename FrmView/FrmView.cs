using Entidades.Exceptions;
using Entidades.Files;
using Entidades.Interfaces;
using Entidades.Modelos;


namespace FrmView
{
    public partial class FrmView : Form
    {
        private IComestible comida;
        Cocinero<Hamburguesa> hamburguesero;

        public FrmView()
        {
            InitializeComponent();
            this.hamburguesero = new Cocinero<Hamburguesa>("Ramon");
            //Alumno - agregar manejadores al cocinero
            this.hamburguesero.OnDemora += this.MostrarConteo;
            this.hamburguesero.OnPedido += this.MostrarComida;
        }


        //Alumno: Realizar los cambios necesarios sobre MostrarComida de manera que se refleje
        //en el formulario los datos de la comida
        private void MostrarComida(IComestible comida)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(() => MostrarComida(comida));
            }
            else
            {
                this.comida = comida;
                this.pcbComida.Load(comida.Imagen);
                this.rchElaborando.Text = comida.ToString();
            }
        }



        //Alumno: Realizar los cambios necesarios sobre MostrarConteo de manera que se refleje
        //en el fomrulario el tiempo transucurrido
        private void MostrarConteo(double tiempo)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(() => MostrarConteo(tiempo));
            }
            else
            {
                this.lblTiempo.Text = $"{tiempo} segundos";
                this.lblTmp.Text = $"{this.hamburguesero.TiempoMedioDePreparacion.ToString("00.0")} segundos";
            }
        }

        /// <summary>
        /// Actualiza el texto de la lista de pedidos completados
        /// </summary>
        /// <param name="comida"></param>
        //private void ActualizarAtendidos(IComestible comida)
        //{
        //    this.rchFinalizados.Text += "\n" + comida.Ticket;
        //}
        /// <summary>
        /// Crea un cocinero e inicia la primera orden
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAbrir_Click(object sender, EventArgs e)
        {
            if (!this.hamburguesero.HabilitarCocina)
            {
                this.hamburguesero.HabilitarCocina = true;
                this.btnAbrir.Image = Properties.Resources.close_icon;
            }
            else
            {
                this.hamburguesero.HabilitarCocina = false;
                this.btnAbrir.Image = Properties.Resources.open_icon;
            }

        }
        /// <summary>
        /// Finaliza el menu actual y lo muestra en atendidos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            if (this.comida is not null)
            {
                comida.FinalizarPreparacion(this.hamburguesero.Nombre);
                this.rchFinalizados.Text += "\n" + comida.Ticket;
                this.comida = null;
            }
            else
            {
                MessageBox.Show("El Cocinero no posee comidas", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }
        /// <summary>
        /// Cierra la cocina, no se aceptan más pedidos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmView_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Alumno: Serializar el cocinero antes de cerrar el formulario
            try
            {
                FileManager.Serializar(this.hamburguesero, "cocinero.json");
            }
            catch (FileManagerException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                FileManager.Guardar(ex.Message, "logs.txt", true);
            }
        }
    }
}