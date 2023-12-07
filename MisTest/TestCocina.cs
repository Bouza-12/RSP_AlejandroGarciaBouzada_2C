using Entidades.Exceptions;
using Entidades.Files;
using Entidades.Modelos;

namespace MisTest
{
    [TestClass]
    public class TestCocina
    {
        [TestMethod]
        [ExpectedException(typeof(FileManagerException))]
        public void AlGuardarUnArchivo_ConNombreInvalido_TengoUnaExcepcion()
        {
            //arrange
            Cocinero<Hamburguesa> cocinero = new Cocinero<Hamburguesa>("John");
            
            //act
            //Uso 2 caracteres no permitido para crear una carpeta
            FileManager.Guardar(cocinero.ToString(), "cocinero*/<>exe",false);
        }

        [TestMethod]

        public void AlInstanciarUnCocinero_SeEspera_PedidosCero()
        {
            //arrange
            Cocinero<Hamburguesa> cocinero = new Cocinero<Hamburguesa>("John");

            //act

            //assert
            Assert.AreEqual(0, cocinero.CantPedidosFinalizados);
        }
    }
}