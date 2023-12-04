using Entidades.Enumerados;


namespace Entidades.MetodosDeExtension
{
    public static class IngredientesExtension
    {
        public static double CalcularCostoIngredientes(this List<EIngrediente> ingredientes, int costoInicial)
        {
            double suma = costoInicial;
            foreach (var e in ingredientes)
            {
                double d = (int)e / 100d;
                suma += costoInicial * d;
            }
            return suma;
        }

        public static List<EIngrediente> IngredientesAleatorios(this Random random)
        {
            List<EIngrediente> ingredientes = new List<EIngrediente>()
            {
                EIngrediente.QUESO,
                EIngrediente.PANCETA,
                EIngrediente.ADHERESO,
                EIngrediente.HUEVO,
                EIngrediente.JAMON,
            };
            random = new Random();
            int n = random.Next(1,ingredientes.Count +1);
            return ingredientes.Take(n).ToList();
        }

    }
}
