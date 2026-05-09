/*1. Sistema de control de turnos para una IPS
Una IPS de atención prioritaria necesita organizar diariamente la atención
de pacientes.
El sistema debe permitir registrar durante el día hasta 30 pacientes,
almacenando:
● Número de documento
● Nombre completo
● Edad
● Tipo de atención (Urgencias, Consulta General, Prioritaria)
● Nivel de prioridad (1 a 5)
El programa debe permitir:
● Registrar pacientes
● Mostrar listado general
● Buscar pacientes por documento
● Mostrar cuántos pacientes hay por tipo de atención
● Identificar el paciente con mayor prioridad
Importante:
No todos los pacientes pueden ser atendidos inmediatamente, por lo que
debe existir validación de cupos disponibles.*/
internal class Ips
{
    private const int CapacidadMaxima = 30;

    public static void Ejecutar()
    {
        string[] documentos = new string[CapacidadMaxima];
        string[] nombres = new string[CapacidadMaxima];
        int[] edades = new int[CapacidadMaxima];
        string[] tiposAtencion = new string[CapacidadMaxima];
        int[] prioridades = new int[CapacidadMaxima];
        int cantidadPacientes = 0;
        bool continuar = true;

        while (continuar)
        {
            Console.WriteLine();
            Console.WriteLine("Sistema de control de turnos para IPS");
            Console.WriteLine("-------------------------------------");
            Console.WriteLine("1) Registrar paciente");
            Console.WriteLine("2) Mostrar listado general");
            Console.WriteLine("3) Buscar paciente por documento");
            Console.WriteLine("4) Mostrar cantidad por tipo de atención");
            Console.WriteLine("5) Identificar paciente con mayor prioridad");
            Console.WriteLine("6) Salir");
            Console.Write("Seleccione una opción: ");

            switch (Console.ReadLine())
            {
                case "1":
                    RegistrarPaciente(documentos, nombres, edades, tiposAtencion, prioridades, ref cantidadPacientes);
                    break;
                case "2":
                    MostrarListado(documentos, nombres, edades, tiposAtencion, prioridades, cantidadPacientes);
                    break;
                case "3":
                    BuscarPorDocumento(documentos, nombres, edades, tiposAtencion, prioridades, cantidadPacientes);
                    break;
                case "4":
                    MostrarCantidadPorTipo(tiposAtencion, cantidadPacientes);
                    break;
                case "5":
                    MostrarPacienteMayorPrioridad(documentos, nombres, edades, tiposAtencion, prioridades, cantidadPacientes);
                    break;
                case "6":
                    continuar = false;
                    Console.WriteLine("Saliendo del control de IPS...");
                    break;
                default:
                    Console.WriteLine("Opción no válida.");
                    break;
            }
        }
    }

    private static void RegistrarPaciente(string[] documentos, string[] nombres, int[] edades, string[] tiposAtencion, int[] prioridades, ref int cantidadPacientes)
    {
        if (cantidadPacientes >= CapacidadMaxima)
        {
            Console.WriteLine("No hay cupos disponibles. Ya se registraron los 30 pacientes del día.");
            return;
        }

        Console.Write("Número de documento: ");
        string documento = LeerTextoObligatorio();

        if (BuscarIndicePorDocumento(documentos, cantidadPacientes, documento) != -1)
        {
            Console.WriteLine("Ya existe un paciente registrado con ese documento.");
            return;
        }

        Console.Write("Nombre completo: ");
        string nombre = LeerTextoObligatorio();

        int edad = LeerEnteroEnRango("Edad: ", 0, 120);
        string tipoAtencion = LeerTipoAtencion();
        int prioridad = LeerEnteroEnRango("Nivel de prioridad (1 a 5): ", 1, 5);

        documentos[cantidadPacientes] = documento;
        nombres[cantidadPacientes] = nombre;
        edades[cantidadPacientes] = edad;
        tiposAtencion[cantidadPacientes] = tipoAtencion;
        prioridades[cantidadPacientes] = prioridad;
        cantidadPacientes++;

        Console.WriteLine("Paciente registrado correctamente.");
        Console.WriteLine($"Cupos disponibles: {CapacidadMaxima - cantidadPacientes}");
    }

    private static void MostrarListado(string[] documentos, string[] nombres, int[] edades, string[] tiposAtencion,  int[] prioridades, int cantidadPacientes)
    {
        if (!HayPacientesRegistrados(cantidadPacientes))
        {
            return;
        }

        Console.WriteLine();
        Console.WriteLine("Listado general de pacientes");
        Console.WriteLine("----------------------------");

        for (int i = 0; i < cantidadPacientes; i++)
        {
            MostrarPaciente(i, documentos, nombres, edades, tiposAtencion, prioridades);
        }
    }

    private static void BuscarPorDocumento(string[] documentos, string[] nombres, int[] edades, string[] tiposAtencion, int[] prioridades, int cantidadPacientes)
    {
        if (!HayPacientesRegistrados(cantidadPacientes))
        {
            return;
        }

        Console.Write("Documento a buscar: ");
        string documento = LeerTextoObligatorio();
        int indice = BuscarIndicePorDocumento(documentos, cantidadPacientes, documento);

        if (indice == -1)
        {
            Console.WriteLine("No se encontró un paciente con ese documento.");
            return;
        }

        Console.WriteLine("Paciente encontrado:");
        MostrarPaciente(indice, documentos, nombres, edades, tiposAtencion, prioridades);
    }

    private static void MostrarCantidadPorTipo(string[] tiposAtencion, int cantidadPacientes)
    {
        if (!HayPacientesRegistrados(cantidadPacientes))
        {
            return;
        }

        int urgencias = 0;
        int consultaGeneral = 0;
        int prioritaria = 0;

        for (int i = 0; i < cantidadPacientes; i++)
        {
            switch (tiposAtencion[i])
            {
                case "Urgencias":
                    urgencias++;
                    break;
                case "Consulta General":
                    consultaGeneral++;
                    break;
                case "Prioritaria":
                    prioritaria++;
                    break;
            }
        }

        Console.WriteLine();
        Console.WriteLine("Pacientes por tipo de atención");
        Console.WriteLine("------------------------------");
        Console.WriteLine($"Urgencias: {urgencias}");
        Console.WriteLine($"Consulta General: {consultaGeneral}");
        Console.WriteLine($"Prioritaria: {prioritaria}");
    }

    private static void MostrarPacienteMayorPrioridad(string[] documentos, string[] nombres, int[] edades, string[] tiposAtencion, int[] prioridades, int cantidadPacientes)
    {
        int indiceMayorPrioridad = 0;
        
        if (!HayPacientesRegistrados(cantidadPacientes))
        {
            return;
        }

        for (int i = 1; i < cantidadPacientes; i++)
        {
            if (prioridades[i] > prioridades[indiceMayorPrioridad])
            {
                indiceMayorPrioridad = i;
            }
        }

        Console.WriteLine();
        Console.WriteLine("Paciente con mayor prioridad");
        Console.WriteLine("----------------------------");
        MostrarPaciente(indiceMayorPrioridad, documentos, nombres, edades, tiposAtencion, prioridades);
    }

    private static string LeerTipoAtencion()
    {
        while (true)
        {
            Console.WriteLine("Tipo de atención:");
            Console.WriteLine("1) Urgencias");
            Console.WriteLine("2) Consulta General");
            Console.WriteLine("3) Prioritaria");
            Console.Write("Seleccione una opción: ");

            switch (Console.ReadLine())
            {
                case "1":
                    return "Urgencias";
                case "2":
                    return "Consulta General";
                case "3":
                    return "Prioritaria";
                default:
                    Console.WriteLine("Tipo de atención no válido.");
                    break;
            }
        }
    }

    private static int LeerEnteroEnRango(string mensaje, int minimo, int maximo)
    {
        while (true)
        {
            Console.Write(mensaje);
            string? entrada = Console.ReadLine();

            if (int.TryParse(entrada, out int valor) && valor >= minimo && valor <= maximo)
            {
                return valor;
            }

            Console.WriteLine($"Ingrese un número entre {minimo} y {maximo}.");
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

    private static int BuscarIndicePorDocumento(string[] documentos, int cantidadPacientes, string documento)
    {
        for (int i = 0; i < cantidadPacientes; i++)
        {
            if (documentos[i].Equals(documento, StringComparison.OrdinalIgnoreCase))
            {
                return i;
            }
        }

        return -1;
    }

    private static bool HayPacientesRegistrados(int cantidadPacientes)
    {
        if (cantidadPacientes > 0)
        {
            return true;
        }

        Console.WriteLine("No hay pacientes registrados.");
        return false;
    }

    private static void MostrarPaciente(int indice, string[] documentos, string[] nombres, int[] edades, string[] tiposAtencion, int[] prioridades)
    {
        Console.WriteLine($"{indice + 1}. Documento: {documentos[indice]}");
        Console.WriteLine($"Nombre: {nombres[indice]}");
        Console.WriteLine($"Edad: {edades[indice]}");
        Console.WriteLine($"Tipo de atención: {tiposAtencion[indice]}");
        Console.WriteLine($"Prioridad: {prioridades[indice]}");
    }
}
