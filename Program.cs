internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Taller de algoritmos modulares");
        Console.WriteLine("--------------------------------------------------");
        Console.WriteLine("1) Control de IPS");
        Console.WriteLine("2) Inventario de Cafetería");
        Console.WriteLine("3) Control de Parqueadero");
        Console.Write("Seleccione una opción: ");

        switch (Console.ReadLine())
        {
            case "1":
                Console.WriteLine("Control de IPS");
                Ips.Ejecutar();
                break;
            case "2":
                Console.WriteLine("Inventario Cafetería");
                Cafeteria.Ejecutar();
                break;
            case "3":
                Console.WriteLine("Control de Parqueadero");
                Parqueadero.Ejecutar();
                break;
            default:
                Console.WriteLine("Opción no válida");
                break;
        }
    }
}
