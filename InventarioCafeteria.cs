/*2. Sistema de control de inventario para una cafetería
universitaria
La cafetería del campus necesita controlar sus productos más vendidos.
El sistema maneja hasta 20 productos:
● Código
● Nombre
● Precio unitario
● Cantidad disponible
● Categoría (Bebidas, Panadería, Snacks)
El programa debe permitir:
● Registrar productos
● Realizar ventas
● Actualizar inventario
● Mostrar productos agotados
● Calcular el total vendido del día
Debe existir validación para evitar vender productos sin stock.*/
internal class Cafeteria
{
    private const int CapacidadMaxima = 20;

    public static void Ejecutar()
    {
        string[] codigos = new string[CapacidadMaxima];
        string[] nombres = new string[CapacidadMaxima];
        decimal[] precios = new decimal[CapacidadMaxima];
        int[] cantidades = new int[CapacidadMaxima];
        string[] categorias = new string[CapacidadMaxima];
        int cantidadProductos = 0;
        decimal totalVendidoDia = 0;
        bool continuar = true;

        while (continuar)
        {
            Console.WriteLine();
            Console.WriteLine("Sistema de inventario para cafetería");
            Console.WriteLine("------------------------------------");
            Console.WriteLine("1) Registrar producto");
            Console.WriteLine("2) Realizar venta");
            Console.WriteLine("3) Actualizar inventario");
            Console.WriteLine("4) Mostrar productos agotados");
            Console.WriteLine("5) Calcular total vendido del día");
            Console.WriteLine("6) Mostrar listado de productos");
            Console.WriteLine("7) Salir");
            Console.Write("Seleccione una opción: ");

            switch (Console.ReadLine())
            {
                case "1":
                    RegistrarProducto(codigos, nombres, precios, cantidades, categorias, ref cantidadProductos);
                    break;
                case "2":
                    RealizarVenta(codigos, nombres, precios, cantidades, cantidadProductos, ref totalVendidoDia);
                    break;
                case "3":
                    ActualizarInventario(codigos, nombres, cantidades, cantidadProductos);
                    break;
                case "4":
                    MostrarProductosAgotados(codigos, nombres, precios, cantidades, categorias, cantidadProductos);
                    break;
                case "5":
                    Console.WriteLine($"Total vendido del día: {totalVendidoDia}");
                    break;
                case "6":
                    MostrarListado(codigos, nombres, precios, cantidades, categorias, cantidadProductos);
                    break;
                case "7":
                    continuar = false;
                    Console.WriteLine("Saliendo del inventario de cafetería...");
                    break;
                default:
                    Console.WriteLine("Opción no válida.");
                    break;
            }
        }
    }

    private static void RegistrarProducto(string[] codigos, string[] nombres, decimal[] precios, int[] cantidades, string[] categorias, ref int cantidadProductos)
    {
        if (cantidadProductos >= CapacidadMaxima)
        {
            Console.WriteLine("No hay cupos disponibles. Ya se registraron los 20 productos.");
            return;
        }

        Console.Write("Código del producto: ");
        string codigo = LeerTextoObligatorio();

        if (BuscarIndicePorCodigo(codigos, cantidadProductos, codigo) != -1)
        {
            Console.WriteLine("Ya existe un producto registrado con ese código.");
            return;
        }

        Console.Write("Nombre del producto: ");
        string nombre = LeerTextoObligatorio();

        decimal precio = LeerDecimalEnRango("Precio unitario: ", 0.01m, decimal.MaxValue);
        int cantidad = LeerEnteroEnRango("Cantidad disponible: ", 0, int.MaxValue);
        string categoria = LeerCategoria();

        codigos[cantidadProductos] = codigo;
        nombres[cantidadProductos] = nombre;
        precios[cantidadProductos] = precio;
        cantidades[cantidadProductos] = cantidad;
        categorias[cantidadProductos] = categoria;
        cantidadProductos++;

        Console.WriteLine("Producto registrado correctamente.");
        Console.WriteLine($"Cupos disponibles: {CapacidadMaxima - cantidadProductos}");
    }

    private static void RealizarVenta(string[] codigos, string[] nombres, decimal[] precios, int[] cantidades, int cantidadProductos, ref decimal totalVendidoDia)
    {
        if (!HayProductosRegistrados(cantidadProductos))
        {
            return;
        }

        Console.Write("Código del producto a vender: ");
        string codigo = LeerTextoObligatorio();
        int indice = BuscarIndicePorCodigo(codigos, cantidadProductos, codigo);

        if (indice == -1)
        {
            Console.WriteLine("No se encontró un producto con ese código.");
            return;
        }

        if (cantidades[indice] == 0)
        {
            Console.WriteLine("No se puede vender este producto porque está agotado.");
            return;
        }

        Console.WriteLine($"Producto: {nombres[indice]}");
        Console.WriteLine($"Stock disponible: {cantidades[indice]}");
        int cantidadVendida = LeerEnteroEnRango("Cantidad a vender: ", 1, cantidades[indice]);
        decimal totalVenta = cantidadVendida * precios[indice];

        cantidades[indice] -= cantidadVendida;
        totalVendidoDia += totalVenta;

        Console.WriteLine($"Venta registrada por {totalVenta}.");
        Console.WriteLine($"Stock restante: {cantidades[indice]}");
    }

    private static void ActualizarInventario(
        string[] codigos,
        string[] nombres,
        int[] cantidades,
        int cantidadProductos)
    {
        if (!HayProductosRegistrados(cantidadProductos))
        {
            return;
        }

        Console.Write("Código del producto a actualizar: ");
        string codigo = LeerTextoObligatorio();
        int indice = BuscarIndicePorCodigo(codigos, cantidadProductos, codigo);

        if (indice == -1)
        {
            Console.WriteLine("No se encontró un producto con ese código.");
            return;
        }

        Console.WriteLine($"Producto: {nombres[indice]}");
        Console.WriteLine($"Cantidad actual: {cantidades[indice]}");
        cantidades[indice] = LeerEnteroEnRango("Nueva cantidad disponible: ", 0, int.MaxValue);

        Console.WriteLine("Inventario actualizado correctamente.");
    }

    private static void MostrarProductosAgotados(string[] codigos, string[] nombres, decimal[] precios, int[] cantidades, string[] categorias, int cantidadProductos)
    {
        if (!HayProductosRegistrados(cantidadProductos))
        {
            return;
        }
        bool hayAgotados = false;
        Console.WriteLine();
        Console.WriteLine("Productos agotados");
        Console.WriteLine("------------------");

        for (int i = 0; i < cantidadProductos; i++)
        {
            if (cantidades[i] == 0)
            {
                MostrarProducto(i, codigos, nombres, precios, cantidades, categorias);
                hayAgotados = true;
            }
        }
        if (!hayAgotados)
        {
            Console.WriteLine("No hay productos agotados.");
        }
    }
    private static void MostrarListado(string[] codigos, string[] nombres, decimal[] precios, int[] cantidades, string[] categorias, int cantidadProductos)
    {
        if (!HayProductosRegistrados(cantidadProductos))
        {
            return;
        }
        Console.WriteLine();
        Console.WriteLine("Listado general de productos");
        Console.WriteLine("----------------------------");

        for (int i = 0; i < cantidadProductos; i++)
        {
            MostrarProducto(i, codigos, nombres, precios, cantidades, categorias);
        }
    }
    private static string LeerCategoria()
    {
        while (true)
        {
            Console.WriteLine("Categoría:");
            Console.WriteLine("1) Bebidas");
            Console.WriteLine("2) Panadería");
            Console.WriteLine("3) Snacks");
            Console.Write("Seleccione una opción: ");

            switch (Console.ReadLine())
            {
                case "1":
                    return "Bebidas";
                case "2":
                    return "Panadería";
                case "3":
                    return "Snacks";
                default:
                    Console.WriteLine("Categoría no válida.");
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

    private static decimal LeerDecimalEnRango(string mensaje, decimal minimo, decimal maximo)
    {
        while (true)
        {
            Console.Write(mensaje);
            string? entrada = Console.ReadLine();

            if (decimal.TryParse(entrada, out decimal valor) && valor >= minimo && valor <= maximo)
            {
                return valor;
            }

            Console.WriteLine($"Ingrese un valor mayor o igual a {minimo}.");
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

    private static int BuscarIndicePorCodigo(string[] codigos, int cantidadProductos, string codigo)
    {
        for (int i = 0; i < cantidadProductos; i++)
        {
            if (codigos[i].Equals(codigo, StringComparison.OrdinalIgnoreCase))
            {
                return i;
            }
        }

        return -1;
    }

    private static bool HayProductosRegistrados(int cantidadProductos)
    {
        if (cantidadProductos > 0)
        {
            return true;
        }

        Console.WriteLine("No hay productos registrados.");
        return false;
    }

    private static void MostrarProducto(int indice, string[] codigos, string[] nombres, decimal[] precios, int[] cantidades, string[] categorias)
    {
        Console.WriteLine($"{indice + 1}. Código: {codigos[indice]}");
        Console.WriteLine($"Nombre: {nombres[indice]}");
        Console.WriteLine($"Precio unitario: $ {precios[indice]}");
        Console.WriteLine($"Cantidad disponible: {cantidades[indice]}");
        Console.WriteLine($"Categoría: {categorias[indice]}");
    }
}
