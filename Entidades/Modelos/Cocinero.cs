using Entidades.Exceptions;
using Entidades.Files;
using Entidades.Interfaces;

namespace Entidades.Modelos
{

    public delegate void DelegadoDemoraAtencion(double tiempo);
    public delegate void DelegadoPedidoEnCurso(IComestible hamburguesa);
    public class Cocinero<T> where T : IComestible, new()
    {
        //Atributos
        private int cantPedidosFinalizados;
        private string nombre;
        private double demoraPreparacionTotal;
        private CancellationTokenSource cancellation;
        private T pedidoEnPreparacion;
        private Task tarea;

        //RSP
        private Mozo<T> mozo;
        private Queue<T> pedidos;

        //Eventos
        public event DelegadoDemoraAtencion OnDemora;
        public event DelegadoPedidoEnCurso OnPedido;

        //Constructor
        public Cocinero(string nombre)
        {
            this.nombre = nombre;
            this.mozo = new Mozo<T>();
            this.pedidos = new Queue<T>();
            this.mozo.OnPedido += this.TomarNuevoPedido;
        }


        //Propiedades
        //No hacer nada
        public bool HabilitarCocina
        {
            get
            {
                return this.tarea is not null && (this.tarea.Status == TaskStatus.Running ||
                    this.tarea.Status == TaskStatus.WaitingToRun ||
                    this.tarea.Status == TaskStatus.WaitingForActivation);
            }
            set
            {
                if (value && !this.HabilitarCocina)
                {
                    this.cancellation = new CancellationTokenSource();
                    this.mozo.EmpezarATrabajar = true;
                    this.EmpezarACocinar();
                }
                else
                {
                    this.cancellation.Cancel();
                    this.mozo.EmpezarATrabajar = !this.mozo.EmpezarATrabajar;
                }
            }
        }

        //no hacer nada
        public double TiempoMedioDePreparacion { get => this.cantPedidosFinalizados == 0 ? 0 : this.demoraPreparacionTotal / this.cantPedidosFinalizados; }
        public string Nombre { get => nombre; }
        public int CantPedidosFinalizados { get => cantPedidosFinalizados; }

        //Métodos
        /// <summary>
        /// Crea un hilo secundario para ejecutar los eventos
        /// </summary>
        private void EmpezarACocinar()
        {
            CancellationToken token = this.cancellation.Token;
            this.tarea = Task.Run(() =>
            {
                while (!this.cancellation.IsCancellationRequested)
                {
                    if (this.pedidos.Count > 0)
                    {
                        this.pedidoEnPreparacion = this.pedidos.Dequeue();
                        this.OnPedido.Invoke(this.pedidoEnPreparacion);
                        this.EsperarProximoIngreso();
                        this.cantPedidosFinalizados++;
                        try
                        {
                            DataBase.DataBaseManager.GuardarTicket(this.nombre, this.pedidoEnPreparacion);
                        }
                        catch (DataBaseManagerException ex)
                        {
                            FileManager.Guardar(ex.Message, "log.txt", true);
                        }
                    }

                }
            },token);
         
        }

        private void TomarNuevoPedido(T menu)
        {

            if (this.OnPedido is not null)
            {
                this.pedidos.Enqueue(menu);
            }
        }

        /// <summary>
        /// Crea un nuevo menu y comienza el evento que cuenta cuanto demora
        /// envía el evento al receptor
        /// </summary>
        //private void NotificarNuevoIngreso()
        //{
        //    if (this.OnIngreso is not null)
        //    {
        //        this.menu = new T();
        //        this.menu.IniciarPreparacion();
        //        this.OnIngreso.Invoke(this.menu);
        //    }
        //}
        /// <summary>
        /// Comienza un contador de segundos y el evento OnDemora envía el mensaje
        /// </summary>
        private void EsperarProximoIngreso()
        {
            int tiempoEspera = 0;

            while (this.OnDemora is not null && !this.pedidoEnPreparacion.Estado && !this.cancellation.IsCancellationRequested)
            {
                tiempoEspera++;
                Thread.Sleep(1000);
                this.OnDemora.Invoke(tiempoEspera);
            }
            this.demoraPreparacionTotal += tiempoEspera;
        }
    }
}
