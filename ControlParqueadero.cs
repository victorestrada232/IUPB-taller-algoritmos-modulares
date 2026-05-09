/*4. Sistema de control de parqueadero para conjunto
residencial
Un conjunto residencial necesita organizar el ingreso de vehículos
visitantes.
Se deben registrar hasta 40 ingresos diarios:
● Placa
● Torre visitada
● Apartamento
● Hora de ingreso
● Tipo de vehículo (Carro, Moto, Bicicleta)
El sistema debe permitir:
● Registrar ingresos
● Consultar salida de vehículo
● Calcular tiempo de permanencia
● Mostrar cantidad de ingresos por tipo
● Identificar el vehículo con mayor permanencia
Debe validarse que una misma placa no ingrese dos veces sin haber
salido.*/
internal class Parqueadero
{
    private const int CapacidadMaxima = 40;

    public static void Ejecutar()
    {
        string[] placas = new string[CapacidadMaxima];
        string[] torres = new string[CapacidadMaxima];
        string[] apartamentos = new string[CapacidadMaxima];
        TimeSpan[] horasIngreso = new TimeSpan[CapacidadMaxima];
        string[] tiposVehiculo = new string[CapacidadMaxima];
        bool[] salidasRegistradas = new bool[CapacidadMaxima];
        TimeSpan[] horasSalida = new TimeSpan[CapacidadMaxima];
        int cantidadIngresos = 0;
        bool continuar = true;

        while (continuar)
        {
            Console.WriteLine();
            Console.WriteLine("Sistema de control de parqueadero");
            Console.WriteLine("---------------------------------");
            Console.WriteLine("1) Registrar ingreso");
            Console.WriteLine("2) Consultar salida de vehículo");
            Console.WriteLine("3) Calcular tiempo de permanencia");
            Console.WriteLine("4) Mostrar cantidad de ingresos por tipo");
            Console.WriteLine("5) Identificar vehículo con mayor permanencia");
            Console.WriteLine("6) Mostrar listado de ingresos");
            Console.WriteLine("7) Salir");
            Console.Write("Seleccione una opción: ");

            switch (Console.ReadLine())
            {
                case "1":
                    RegistrarIngreso(placas, torres, apartamentos, horasIngreso, tiposVehiculo, salidasRegistradas, ref cantidadIngresos);
                    break;
                case "2":
                    RegistrarSalida(placas, horasIngreso, salidasRegistradas, horasSalida, cantidadIngresos);
                    break;
                case "3":
                    CalcularTiempoPermanencia(placas, horasIngreso, salidasRegistradas, horasSalida, cantidadIngresos);
                    break;
                case "4":
                    MostrarCantidadPorTipo(tiposVehiculo, cantidadIngresos);
                    break;
                case "5":
                    IdentificarMayorPermanencia(placas, horasIngreso, salidasRegistradas, horasSalida, cantidadIngresos);
                    break;
                case "6":
                    MostrarListado(placas, torres, apartamentos, horasIngreso, tiposVehiculo, salidasRegistradas, horasSalida, cantidadIngresos);
                    break;
                case "7":
                    continuar = false;
                    Console.WriteLine("Saliendo del control de parqueadero...");
                    break;
                default:
                    Console.WriteLine("Opción no válida.");
                    break;
            }
        }
    }

    private static void RegistrarIngreso(string[] placas, string[] torres, string[] apartamentos, TimeSpan[] horasIngreso, string[] tiposVehiculo, bool[] salidasRegistradas, ref int cantidadIngresos)
    {
        if (cantidadIngresos >= CapacidadMaxima)
        {
            Console.WriteLine("No hay cupos disponibles. Ya se registraron los 40 ingresos diarios.");
            return;
        }

        Console.Write("Placa del vehículo: ");
        string placa = LeerTextoObligatorio().ToUpper();

        if (BuscarVehiculoActivoPorPlaca(placas, salidasRegistradas, cantidadIngresos, placa) != -1)
        {
            Console.WriteLine("Esta placa ya tiene un ingreso activo. Debe registrar su salida antes de ingresar nuevamente.");
            return;
        }

        Console.Write("Torre visitada: ");
        string torre = LeerTextoObligatorio();

        Console.Write("Apartamento: ");
        string apartamento = LeerTextoObligatorio();

        TimeSpan horaIngreso = LeerHora("Hora de ingreso (HH:mm): ");
        string tipoVehiculo = LeerTipoVehiculo();

        placas[cantidadIngresos] = placa;
        torres[cantidadIngresos] = torre;
        apartamentos[cantidadIngresos] = apartamento;
        horasIngreso[cantidadIngresos] = horaIngreso;
        tiposVehiculo[cantidadIngresos] = tipoVehiculo;
        salidasRegistradas[cantidadIngresos] = false;
        cantidadIngresos++;

        Console.WriteLine("Ingreso registrado correctamente.");
        Console.WriteLine($"Cupos de registro disponibles: {CapacidadMaxima - cantidadIngresos}");
    }

    private static void RegistrarSalida(string[] placas, TimeSpan[] horasIngreso, bool[] salidasRegistradas, TimeSpan[] horasSalida, int cantidadIngresos)
    {
        if (!HayIngresosRegistrados(cantidadIngresos))
        {
            return;
        }

        Console.Write("Placa del vehículo que sale: ");
        string placa = LeerTextoObligatorio().ToUpper();
        int indice = BuscarVehiculoActivoPorPlaca(placas, salidasRegistradas, cantidadIngresos, placa);

        if (indice == -1)
        {
            Console.WriteLine("No se encontró un ingreso activo para esa placa.");
            return;
        }

        TimeSpan horaSalida = LeerHoraPosterior("Hora de salida (HH:mm): ", horasIngreso[indice]);
        salidasRegistradas[indice] = true;
        horasSalida[indice] = horaSalida;

        Console.WriteLine("Salida registrada correctamente.");
        Console.WriteLine($"Tiempo de permanencia: {FormatearTiempo(horaSalida - horasIngreso[indice])}");
    }

    private static void CalcularTiempoPermanencia(string[] placas, TimeSpan[] horasIngreso, bool[] salidasRegistradas, TimeSpan[] horasSalida, int cantidadIngresos)
    {
        if (!HayIngresosRegistrados(cantidadIngresos))
        {
            return;
        }

        Console.Write("Placa a consultar: ");
        string placa = LeerTextoObligatorio().ToUpper();
        int indice = BuscarUltimoIndicePorPlaca(placas, cantidadIngresos, placa);

        if (indice == -1)
        {
            Console.WriteLine("No se encontró un ingreso para esa placa.");
            return;
        }

        TimeSpan horaFinal = horasSalida[indice];

        if (!salidasRegistradas[indice])
        {
            horaFinal = LeerHoraPosterior("El vehículo sigue dentro. Ingrese hora actual (HH:mm): ", horasIngreso[indice]);
        }

        Console.WriteLine($"Tiempo de permanencia: {FormatearTiempo(horaFinal - horasIngreso[indice])}");
    }

    private static void MostrarCantidadPorTipo(string[] tiposVehiculo, int cantidadIngresos)
    {
        if (!HayIngresosRegistrados(cantidadIngresos))
        {
            return;
        }

        int carros = 0;
        int motos = 0;
        int bicicletas = 0;

        for (int i = 0; i < cantidadIngresos; i++)
        {
            switch (tiposVehiculo[i])
            {
                case "Carro":
                    carros++;
                    break;
                case "Moto":
                    motos++;
                    break;
                case "Bicicleta":
                    bicicletas++;
                    break;
            }
        }

        Console.WriteLine();
        Console.WriteLine("Ingresos por tipo de vehículo");
        Console.WriteLine("-----------------------------");
        Console.WriteLine($"Carros: {carros}");
        Console.WriteLine($"Motos: {motos}");
        Console.WriteLine($"Bicicletas: {bicicletas}");
    }

    private static void IdentificarMayorPermanencia(
        string[] placas,
        TimeSpan[] horasIngreso,
        bool[] salidasRegistradas,
        TimeSpan[] horasSalida,
        int cantidadIngresos)
    {
        if (!HayIngresosRegistrados(cantidadIngresos))
        {
            return;
        }

        TimeSpan horaReferencia = LeerHora("Hora de referencia para vehículos activos (HH:mm): ");
        int indiceMayor = -1;
        TimeSpan mayorPermanencia = TimeSpan.Zero;

        for (int i = 0; i < cantidadIngresos; i++)
        {
            TimeSpan horaFinal = salidasRegistradas[i] ? horasSalida[i] : horaReferencia;

            if (horaFinal < horasIngreso[i])
            {
                continue;
            }

            TimeSpan permanencia = horaFinal - horasIngreso[i];

            if (indiceMayor == -1 || permanencia > mayorPermanencia)
            {
                indiceMayor = i;
                mayorPermanencia = permanencia;
            }
        }

        if (indiceMayor == -1)
        {
            Console.WriteLine("No hay registros válidos para calcular la permanencia con esa hora de referencia.");
            return;
        }

        Console.WriteLine();
        Console.WriteLine("Vehículo con mayor permanencia");
        Console.WriteLine("------------------------------");
        Console.WriteLine($"Placa: {placas[indiceMayor]}");
        Console.WriteLine($"Tiempo de permanencia: {FormatearTiempo(mayorPermanencia)}");
    }

    private static void MostrarListado(
        string[] placas,
        string[] torres,
        string[] apartamentos,
        TimeSpan[] horasIngreso,
        string[] tiposVehiculo,
        bool[] salidasRegistradas,
        TimeSpan[] horasSalida,
        int cantidadIngresos)
    {
        if (!HayIngresosRegistrados(cantidadIngresos))
        {
            return;
        }

        Console.WriteLine();
        Console.WriteLine("Listado de ingresos");
        Console.WriteLine("-------------------");

        for (int i = 0; i < cantidadIngresos; i++)
        {
            Console.WriteLine($"{i + 1}. Placa: {placas[i]}");
            Console.WriteLine($"Torre visitada: {torres[i]}");
            Console.WriteLine($"Apartamento: {apartamentos[i]}");
            Console.WriteLine($"Hora de ingreso: {horasIngreso[i]:hh\\:mm}");
            Console.WriteLine($"Tipo de vehículo: {tiposVehiculo[i]}");
            Console.WriteLine($"Estado: {(salidasRegistradas[i] ? "Salió" : "Activo")}");

            if (salidasRegistradas[i])
            {
                Console.WriteLine($"Hora de salida: {horasSalida[i]:hh\\:mm}");
                Console.WriteLine($"Permanencia: {FormatearTiempo(horasSalida[i] - horasIngreso[i])}");
            }
        }
    }

    private static string LeerTipoVehiculo()
    {
        while (true)
        {
            Console.WriteLine("Tipo de vehículo:");
            Console.WriteLine("1) Carro");
            Console.WriteLine("2) Moto");
            Console.WriteLine("3) Bicicleta");
            Console.Write("Seleccione una opción: ");

            switch (Console.ReadLine())
            {
                case "1":
                    return "Carro";
                case "2":
                    return "Moto";
                case "3":
                    return "Bicicleta";
                default:
                    Console.WriteLine("Tipo de vehículo no válido.");
                    break;
            }
        }
    }

    private static TimeSpan LeerHora(string mensaje)
    {
        while (true)
        {
            Console.Write(mensaje);
            string? entrada = Console.ReadLine();

            if (TimeSpan.TryParse(entrada, out TimeSpan hora) && hora >= TimeSpan.Zero && hora < TimeSpan.FromDays(1))
            {
                return hora;
            }

            Console.WriteLine("Ingrese una hora válida en formato HH:mm.");
        }
    }

    private static TimeSpan LeerHoraPosterior(string mensaje, TimeSpan horaIngreso)
    {
        while (true)
        {
            TimeSpan hora = LeerHora(mensaje);

            if (hora >= horaIngreso)
            {
                return hora;
            }

            Console.WriteLine("La hora debe ser igual o posterior a la hora de ingreso.");
        }
    }

    private static string LeerTextoObligatorio()
    {
        while (true)
        {
            string? entrada = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(entrada))
            {
                return entrada.Trim();
            }

            Console.Write("El dato es obligatorio. Intente nuevamente: ");
        }
    }

    private static int BuscarVehiculoActivoPorPlaca(string[] placas, bool[] salidasRegistradas, int cantidadIngresos, string placa)
    {
        for (int i = cantidadIngresos - 1; i >= 0; i--)
        {
            if (!salidasRegistradas[i] && placas[i].Equals(placa, StringComparison.OrdinalIgnoreCase))
            {
                return i;
            }
        }

        return -1;
    }

    private static int BuscarUltimoIndicePorPlaca(string[] placas, int cantidadIngresos, string placa)
    {
        for (int i = cantidadIngresos - 1; i >= 0; i--)
        {
            if (placas[i].Equals(placa, StringComparison.OrdinalIgnoreCase))
            {
                return i;
            }
        }

        return -1;
    }

    private static bool HayIngresosRegistrados(int cantidadIngresos)
    {
        if (cantidadIngresos > 0)
        {
            return true;
        }

        Console.WriteLine("No hay ingresos registrados.");
        return false;
    }

    private static string FormatearTiempo(TimeSpan tiempo)
    {
        return $"{(int)tiempo.TotalHours} horas y {tiempo.Minutes} minutos";
    }
}
