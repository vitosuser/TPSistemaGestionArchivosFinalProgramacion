using GestorDeArchivosDeTexto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorDeArchivosDeTexto
{
    public class Menu
    {
        static GestorArchivos gestor = new GestorArchivos();
        public static void menuPrincipal()
        {
            bool salir = false;

            while (!salir)
            {
                Console.Clear();
                Console.WriteLine("╔════════════════════════════════════════════════════════╗");
                Console.WriteLine("║               GESTOR DE ARCHIVOS DE TEXTO              ║");
                Console.WriteLine("╠════════════════════════════════════════════════════════╣");
                Console.WriteLine("║ 1. Crear Nuevo Archivo                                 ║");
                Console.WriteLine("║ 2. Leer archivo existente                              ║");
                Console.WriteLine("║ 3. Modificar archivo existente                         ║");
                Console.WriteLine("║ 4. Eliminar archivo                                    ║");
                Console.WriteLine("║ 5. Convertir entre formatos                            ║");
                Console.WriteLine("║ 6. Crear Reporte con Corte de control de un nivel      ║");
                Console.WriteLine("║ 0. Salir                                               ║");
                Console.WriteLine("╚════════════════════════════════════════════════════════╝");
                Console.Write("\nSeleccione una opción: ");

                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        NuevoArchivo();
                        break;

                    case "2":
                        LeerArchivoExistente();
                        break;

                    case "3":
                        ModificarArchivo();
                        break;

                    case "4":
                        EliminarArchivo();
                        break;

                    case "5":
                        ConvertirEntreFormatos();
                        break;

                    case "6":
                        CrearReporte();
                        break;

                    case "0":
                        salir = true;
                        break;

                    default:
                        Console.WriteLine("Valor no valido. Ingrese un numero del 1 al 5");
                        Pause();
                        break;

                }
            }
        }

        static void NuevoArchivo()
        {
            Console.WriteLine("\n--------------------------CREAR NUEVO ARCHIVO-------------------------- \n");

            // 1. pedimos el nombre
            Console.Write("Ingrese el nombre del archivo (sin extensión): ");
            string nombre = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(nombre))
            {
                Console.WriteLine("ERROR: El nombre no puede estar vacío.");
                Pause();
                return;
            }

            // 2. pedimos el formato
            Console.WriteLine("\nSeleccione el formato:");
            Console.WriteLine("1. TXT");
            Console.WriteLine("2. CSV");
            Console.WriteLine("3. JSON");
            Console.WriteLine("4. XML");
            Console.Write("Opción: ");
            string opc = Console.ReadLine();

            string extension = "";
            switch (opc)
            {
                case "1": extension = ".txt"; break;
                case "2": extension = ".csv"; break;
                case "3": extension = ".json"; break;
                case "4": extension = ".xml"; break;
                default:
                    Console.WriteLine("Opción inválida.");
                    Pause();
                    return;
            }

            // 3. pedimos cantidad y cargar lista
            Console.Write("\n¿Cuántos alumnos desea registrar?: ");
            if (!int.TryParse(Console.ReadLine(), out int cantidad) || cantidad <= 0)
            {
                Console.WriteLine("Cantidad inválida.");
                Pause();
                return;
            }

            List<Alumno> nuevosAlumnos = new List<Alumno>();

            for (int i = 0; i < cantidad; i++)
            {
                Console.WriteLine($"\n--- Cargando Alumno {i + 1}/{cantidad} ---");
                // usamos el constructor
                Console.Write("Legajo: "); string leg = Console.ReadLine();
                Console.Write("Apellido: "); string ape = Console.ReadLine();
                Console.Write("Nombre: "); string nom = Console.ReadLine();
                Console.Write("Documento: "); string doc = Console.ReadLine();
                Console.Write("Email: "); string mail = Console.ReadLine();
                Console.Write("Teléfono: "); string tel = Console.ReadLine();

                nuevosAlumnos.Add(new Alumno(leg, ape, nom, doc, mail, tel));
            }

            // 4. llamamos a la funcion del gestor
            string rutaCompleta = Path.Combine(Environment.CurrentDirectory, nombre + extension);
            gestor.GuardarArchivo(rutaCompleta, extension, nuevosAlumnos);

            Pause();
        }

        static void LeerArchivoExistente()
        {
            Console.WriteLine("\n--------------------------LEER ARCHIVO-------------------------- \n");

            // 1. pedir el nombre con extension
            Console.WriteLine("Ingrese el nombre completo del archivo (con extensión):");
            Console.WriteLine("Ejemplo: alumnos.json");
            string archivo = Console.ReadLine();

            // validar que se ingrese un nombre
            if (string.IsNullOrWhiteSpace(archivo))
            {
                Console.WriteLine("ERROR: Debe ingresar un nombre.");
                Pause();
                return;
            }

            // validamos con el metodo de vito
            if (!validadorNombreArchivo(archivo))
            {
                Pause();
                return;
            }

            // validamos con el gestor
            if (!gestor.ValidarExistencia(archivo))
            {
                Console.WriteLine($"ERROR: El archivo \"{archivo}\" no existe.");
                Pause();
                return;
            }

            // preparamos los datos que pide el metodo de LeerAlumnosDesdeArchivo
            string ruta = Path.Combine(Environment.CurrentDirectory, archivo);
            string extension = Path.GetExtension(archivo).ToLower();

            List<Alumno> lista = gestor.LeerAlumnosDesdeArchivo(extension, ruta);

            if (lista.Count == 0)
            {
                Console.WriteLine("El archivo está vacío o no se pudieron leer datos correctamente.");
                Pause();
                return;
            }

            Console.WriteLine("\n");
            // encabezado de la tabla
            Console.WriteLine("=".PadRight(110, '='));
            Console.WriteLine($"| {"Legajo".PadRight(10)} | {"Apellido".PadRight(15)} | {"Nombre".PadRight(20)} | {"Documento".PadRight(12)} | {"Email".PadRight(30)} |");
            Console.WriteLine("=".PadRight(110, '='));

            int contador = 0;
            foreach (var alu in lista)
            {
                Console.Write("| ");
                // usamos PadRight para que las columnas queden alineadas
                Console.Write($"{alu.Legajo.PadRight(10)} | ");
                Console.Write($"{alu.Apellido.PadRight(15)} | ");
                Console.Write($"{alu.Nombre.PadRight(20)} | ");
                Console.Write($"{alu.Documento.PadRight(12)} | ");
                Console.Write($"{alu.Email.PadRight(30)} |");
                Console.WriteLine();

                contador++;

                // paginación cada 20 registros
                if (contador % 20 == 0)
                {
                    Console.WriteLine("=".PadRight(110, '='));
                    Console.WriteLine($"--- Mostrando {contador} de {lista.Count}. Presione una tecla para continuar ---");
                    Console.ReadKey();
                    Console.WriteLine("\n");
                    // repetimos el encabezado para que no se pierda
                    Console.WriteLine($"| {"Legajo".PadRight(10)} | {"Apellido".PadRight(15)} | {"Nombre".PadRight(20)} | {"Documento".PadRight(12)} | {"Email".PadRight(30)} |");
                    Console.WriteLine("=".PadRight(110, '='));
                }
            }
            Console.WriteLine("=".PadRight(110, '='));
            Console.WriteLine($"Total de alumnos: {lista.Count}");

            Pause();
        }

        static void ModificarArchivo()
        {
            Console.WriteLine("\n--------------------------MODIFICAR ARCHIVO-------------------------- \n");

            // solicitamos el arhchivo
            Console.WriteLine("Ingrese el nombre completo del archivo a modificar (ej: alumnos.json):");
            string archivo = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(archivo) || !validadorNombreArchivo(archivo)) { Pause(); return; }
            if (!gestor.ValidarExistencia(archivo))
            {
                Console.WriteLine($"ERROR: El archivo \"{archivo}\" no existe.");
                Pause();
                return;
            }

            // cargamos en memoria
            string ruta = Path.Combine(Environment.CurrentDirectory, archivo);
            string extension = Path.GetExtension(archivo).ToLower();

            List<Alumno> listaMemoria = gestor.LeerAlumnosDesdeArchivo(extension, ruta);

            if (listaMemoria.Count == 0)
            {
                Console.WriteLine("El archivo está vacío o no se puede leer. Agregue alumnos primero.");
                // No retornamos, permitimos que agregue nuevos si quiere
            }
            else
            {
                Console.WriteLine($"\n Se cargaron {listaMemoria.Count} alumnos en memoria.");
            }

            // 3. SUB-MENÚ DE MODIFICACIÓN
            bool guardarYSalir = false;
            bool cancelar = false;

            while (!guardarYSalir && !cancelar)
            {
                Console.Clear();
                Console.WriteLine($"EDITANDO: {archivo} ({listaMemoria.Count} registros en memoria)");
                Console.WriteLine("=================================");
                Console.WriteLine("1. Agregar nuevo alumno");
                Console.WriteLine("2. Modificar alumno existente (por Legajo)");
                Console.WriteLine("3. Eliminar alumno (por Legajo)");
                Console.WriteLine("4. GUARDAR CAMBIOS Y SALIR");
                Console.WriteLine("5. CANCELAR (Salir sin guardar)");
                Console.WriteLine("=================================");
                Console.Write("Opción: ");
                string opc = Console.ReadLine();

                switch (opc)
                {
                    case "1":
                        Console.WriteLine("\n--- Nuevo Alumno ---");
                        Console.Write("Legajo: "); string l = Console.ReadLine();
                        // validar que el legajo no exista ya en la lista
                        if (listaMemoria.Any(a => a.Legajo == l))
                        {
                            Console.WriteLine("ERROR: Ya existe un alumno con ese Legajo en la lista.");
                            Pause();
                            break;
                        }
                        Console.Write("Apellido: "); string a = Console.ReadLine();
                        Console.Write("Nombre: "); string n = Console.ReadLine();
                        Console.Write("Documento: "); string d = Console.ReadLine();
                        Console.Write("Email: "); string e = Console.ReadLine();
                        Console.Write("Teléfono: "); string t = Console.ReadLine();

                        listaMemoria.Add(new Alumno(l, a, n, d, e, t));
                        Console.WriteLine("Alumno agregado a la lista temporal.");
                        Pause();
                        break;

                    case "2": // MODIFICAR
                        Console.Write("\nIngrese el Legajo del alumno a editar: ");
                        string legBusq = Console.ReadLine();

                        // buscamos el objeto en la lista
                        Alumno aluEdit = listaMemoria.FirstOrDefault(x => x.Legajo == legBusq);

                        if (aluEdit != null)
                        {
                            Console.WriteLine("\n--- Deje vacío y presione Enter para mantener el valor actual ---");

                            Console.Write($"Apellido ({aluEdit.Apellido}): ");
                            string input = Console.ReadLine();
                            if (!string.IsNullOrWhiteSpace(input)) aluEdit.Apellido = input;

                            Console.Write($"Nombre ({aluEdit.Nombre}): ");
                            input = Console.ReadLine();
                            if (!string.IsNullOrWhiteSpace(input)) aluEdit.Nombre = input;

                            Console.Write($"Documento ({aluEdit.Documento}): ");
                            input = Console.ReadLine();
                            if (!string.IsNullOrWhiteSpace(input)) aluEdit.Documento = input;

                            Console.Write($"Email ({aluEdit.Email}): ");
                            input = Console.ReadLine();
                            if (!string.IsNullOrWhiteSpace(input)) aluEdit.Email = input;

                            Console.Write($"Teléfono ({aluEdit.Telefono}): ");
                            input = Console.ReadLine();
                            if (!string.IsNullOrWhiteSpace(input)) aluEdit.Telefono = input;

                            Console.WriteLine("Registro actualizado en memoria.");
                        }
                        else
                        {
                            Console.WriteLine("No se encontró ningún alumno con ese Legajo.");
                        }
                        Pause();
                        break;

                    case "3": // ELIMINAR
                        Console.Write("\nIngrese el Legajo del alumno a eliminar: ");
                        string legDel = Console.ReadLine();
                        Alumno aluDel = listaMemoria.FirstOrDefault(x => x.Legajo == legDel);

                        if (aluDel != null)
                        {
                            Console.WriteLine($"¿Seguro desea eliminar a {aluDel.Apellido}, {aluDel.Nombre}? (S/N)");
                            if (Console.ReadLine().ToUpper() == "S")
                            {
                                listaMemoria.Remove(aluDel);
                                Console.WriteLine("Alumno eliminado de la lista temporal.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("No se encontró ese Legajo.");
                        }
                        Pause();
                        break;

                    case "4": // GUARDAR
                        Console.WriteLine("\nGuardando cambios...");

                        // A. Crear Backup
                        gestor.CrearBackup(ruta);

                        // B. Sobrescribir archivo original
                        gestor.GuardarArchivo(ruta, extension, listaMemoria);

                        guardarYSalir = true;
                        Pause();
                        break;

                    case "5": // CANCELAR
                        Console.WriteLine("\nCambios descartados. Volviendo al menú principal...");
                        cancelar = true;
                        Pause();
                        break;

                    default:
                        Console.WriteLine("Opción inválida");
                        break;
                }
            }
        }

        static void EliminarArchivo()
        {
            Console.WriteLine("\n--------------------------ELIMINAR ARCHIVO-------------------------- \n");
            Console.WriteLine("Ingrese el nombre completo del archivo (con extension): ");
            Console.WriteLine("Ejemplo: archivo.txt");
            string archivo = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(archivo))
            {
                if (validadorNombreArchivo(archivo))
                {
                    bool existe = gestor.ValidarExistencia(archivo);

                    if (existe)
                    {
                        gestor.EliminarArchivo(archivo);
                    }
                    else
                    {
                        Console.WriteLine($"ERROR: El archivo \"{archivo}\" no existe");
                    }
                }
               
            }
            else
            {
                Console.WriteLine("ERROR: Debe ingresar un nombre de archivo");
            }

            Pause();
        }

        static void ConvertirEntreFormatos()
        {
            Console.WriteLine("\n--------------------------CONVERTIR ENTRE FORMATOS-------------------------- \n");
            Console.WriteLine("Ingrese el nombre completo del archivo (con extension): ");
            Console.WriteLine("Ejemplo: archivo.txt");

            string archivo = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(archivo))
            {
                if (validadorNombreArchivo(archivo))
                {
                    bool existe = gestor.ValidarExistencia(archivo);

                    if (!existe)
                    {
                        Console.WriteLine($"ERROR: El archivo \"{archivo}\" no existe");
                        Pause();
                        return;
                    }

                    string rutaOrigen = Path.IsPathRooted(archivo) ? archivo : Path.Combine(Environment.CurrentDirectory, archivo);
                    
                    string extOrigen = Path.GetExtension(rutaOrigen).ToLowerInvariant();

                    var formatos = new List<string> { ".txt", ".csv", ".json", ".xml" };

                    if (!formatos.Contains(extOrigen))
                    {
                        Console.WriteLine($"ERROR: Extensión de origen '{extOrigen}' no soportada.");
                        Pause();
                        return;
                    }

                    string formatoDestino;

                    if(extOrigen == ".txt")
                    {
                        Console.WriteLine($"\n Formato actual: {extOrigen}");
                        Console.WriteLine("\n Seleccione formato de destino: ");
                        Console.WriteLine("\t 1. CSV ");
                        formatoDestino = Console.ReadLine();

                        if (formatoDestino == "1")
                        {

                        }
                        else
                        {
                            Console.WriteLine($"ERROR: Debe seleccionar un formato de destino valido.");
                            Pause();
                            return;
                        }
                    }
                    else if(extOrigen == ".csv")
                    {
                        Console.WriteLine($"\n Formato actual: {extOrigen}");
                        Console.WriteLine("\n Seleccione formato de destino: ");
                        Console.WriteLine("\t 1. TXT ");
                        Console.WriteLine("\t 2. JSON ");
                        formatoDestino = Console.ReadLine();

                        if (formatoDestino == "1" || formatoDestino == "2")
                        {
                            gestor.LeerAlumnosDesdeArchivo(extOrigen, rutaOrigen);
                        }
                        else
                        {
                            Console.WriteLine($"ERROR: Debe seleccionar un formato de destino valido.");
                            Pause();
                            return;
                        }

                    }
                    else if(extOrigen == ".json")
                    {
                        Console.WriteLine($"\n Formato actual: {extOrigen}");
                        Console.WriteLine("\n Seleccione formato de destino: ");
                        Console.WriteLine("\t 1. CSV ");
                        Console.WriteLine("\t 2. XML ");
                        formatoDestino = Console.ReadLine();

                        if (formatoDestino == "1")
                        {

                        }
                        else if (formatoDestino == "2")
                        {

                        }
                        else
                        {
                            Console.WriteLine($"ERROR: Debe seleccionar un formato de destino valido.");
                            Pause();
                            return;
                        }

                    }
                    else if(extOrigen == ".xml")
                    {
                        Console.WriteLine($"\n Formato actual: {extOrigen}");
                        Console.WriteLine("\n Seleccione formato de destino: ");
                        Console.WriteLine("\t 1. JSON ");
                        formatoDestino = Console.ReadLine();

                        if(formatoDestino == "1")
                        {
                            
                        }
                        else
                        {
                            Console.WriteLine($"ERROR: Debe seleccionar un formato de destino valido.");
                            Pause();
                            return;
                        }

                    }

                }

            }
            else
            {
                Console.WriteLine("ERROR: Debe ingresar un nombre de archivo");
            }

            Pause();
        }

        static void CrearReporte()
        {

        }

        static bool validadorNombreArchivo(string nombre)
        {
            // Validar que tenga extensión (un punto y algo mas)

            if (!nombre.Contains('.') || nombre.StartsWith('.') || nombre.EndsWith('.'))
            {
                Console.WriteLine("ERROR: Debe ingresar un nombre de archivo con extension. Ej: datos.txt");

                return false;
            }

            // Validar extension permitida

            string extension = Path.GetExtension(nombre).ToLower(); //obtengo la extension

            string[] extensionesValidas = { ".txt", ".csv", ".json", ".xml" }; //determino las extensiones validas segun la lista de requerimientos

            if (!extensionesValidas.Contains(extension))
            {
                Console.WriteLine($"ERROR: La extension '{extension}' no es válida. Extensiones permitidas: TXT, CSV, JSON, XML.");
                return false;
            }


            return true; // Todo OK
        }
        

        public static void Pause()
        {
            Console.WriteLine("\n");
            Console.WriteLine("Oprima una tecla para continuar");
            Console.ReadKey();
        }
    }
}
