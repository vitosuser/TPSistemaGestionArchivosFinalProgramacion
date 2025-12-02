using GestorDeArchivosDeTexto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

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

            // pedimos el nombre
            string nombre = SolicitarEntrada("Ingrese el nombre del archivo (sin extensión): ");

            // pedimos el formato
            Console.WriteLine("\nSeleccione el formato:");
            Console.WriteLine("1. TXT");
            Console.WriteLine("2. CSV");
            Console.WriteLine("3. JSON");
            Console.WriteLine("4. XML");
            string opc = SolicitarEntrada("Opción: ", s => "1234".Contains(s) && s.Length == 1, "Opción inválida.");

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

            // pedimos cantidad y cargar lista
            string cantStr = SolicitarEntrada("\n¿Cuántos alumnos desea registrar?: ", EsSoloNumeros, "Debe ser un número.");
            int cantidad = int.Parse(cantStr);

            List<Alumno> nuevosAlumnos = new List<Alumno>();

            for (int i = 0; i < cantidad; i++)
            {
                Console.WriteLine($"\n--- Cargando Alumno {i + 1}/{cantidad} ---");

                string legajo = SolicitarEntrada("Legajo: ", EsSoloNumeros, "Solo números.");
                string apellido = SolicitarEntrada("Apellido: ");
                string name = SolicitarEntrada("Nombre: ");
                string documento = SolicitarEntrada("Documento: ", EsSoloNumeros, "Solo números.");
                string mail = SolicitarEntrada("Email: ", EsEmailValido, "Formato inválido (ej: a@b.com).");
                string tel = SolicitarEntrada("Teléfono: ", EsSoloNumeros, "Solo números.");

                nuevosAlumnos.Add(new Alumno(legajo, apellido, name, documento, mail, tel));
            }

            // 4. Guardar
            string rutaCompleta = Path.Combine(Environment.CurrentDirectory, nombre + extension);
            gestor.GuardarArchivo(rutaCompleta, extension, nuevosAlumnos);

            Pause();
        }

        static void LeerArchivoExistente()
        {
            Console.WriteLine("\n--------------------------LEER ARCHIVO-------------------------- \n");

            // pedir el nombre con extension
            string archivo = SolicitarEntrada("Ingrese el nombre completo (ej: alumnos.json): ");

            // validar el nombre
            if (!validadorNombreArchivo(archivo)) { Pause(); return; }

            // validar existencia
            if (!gestor.ValidarExistencia(archivo))
            {
                Console.WriteLine($"ERROR: El archivo \"{archivo}\" no existe.");
                Pause();
                return;
            }

            // preparamos los datos para el Gestor
            string ruta = Path.Combine(Environment.CurrentDirectory, archivo);
            string extension = Path.GetExtension(archivo).ToLower();

            List<Alumno> lista = gestor.LeerAlumnosDesdeArchivo(extension, ruta);

            if (lista.Count == 0)
            {
                Console.WriteLine("El archivo está vacío o no se pudieron leer datos.");
            }
            else
            {
                MostrarTabla(lista);
            }

            Pause();
        }

        static void ModificarArchivo()
        {
            Console.WriteLine("\n--------------------------MODIFICAR ARCHIVO-------------------------- \n");

            // solicitamos el arhchivo
            string archivo = SolicitarEntrada("Ingrese el nombre completo del archivo a modificar (ej: alumnos.json): ");

            if (!validadorNombreArchivo(archivo)) { Pause(); return; }
            if (!gestor.ValidarExistencia(archivo))
            {
                Console.WriteLine($"ERROR: El archivo \"{archivo}\" no existe.");
                Pause();
                return;
            }

            string ruta = Path.Combine(Environment.CurrentDirectory, archivo);
            string extension = Path.GetExtension(archivo).ToLower();

            List<Alumno> listaMemoria = gestor.LeerAlumnosDesdeArchivo(extension, ruta);

            if (listaMemoria.Count == 0) Console.WriteLine("El archivo está vacío. Puede agregar alumnos nuevos.");
            else Console.WriteLine($"\n✅ Se cargaron {listaMemoria.Count} alumnos en memoria.");

            // SUBMENU DE MODIFICACIÓN
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

                string opc = SolicitarEntrada("Opción: ");

                switch (opc)
                {
                    case "1":
                        Console.WriteLine("\n--- Nuevo Alumno ---");
                        string leg = SolicitarEntrada("Legajo: ", EsSoloNumeros, "Solo números.");

                        if (listaMemoria.Any(a => a.Legajo == leg))
                        {
                            Console.WriteLine("ERROR: Ya existe un alumno con ese Legajo.");
                            Pause();
                            break;
                        }

                        listaMemoria.Add(new Alumno(
                            leg,
                            SolicitarEntrada("Apellido: "),
                            SolicitarEntrada("Nombre: "),
                            SolicitarEntrada("Documento: ", EsSoloNumeros, "Solo números."),
                            SolicitarEntrada("Email: ", EsEmailValido, "Email inválido."),
                            SolicitarEntrada("Teléfono: ", EsSoloNumeros, "Solo números.")
                        ));
                        Console.WriteLine("Alumno agregado.");
                        Pause();
                        break;

                    case "2":
                        string legBusq = SolicitarEntrada("Ingrese el Legajo a editar: ");
                        Alumno alu = listaMemoria.FirstOrDefault(x => x.Legajo == legBusq);

                        if (alu != null)
                        {
                            Console.WriteLine("\n(Deje vacío y presione Enter para mantener el valor actual)");

                            alu.Apellido = SolicitarEntradaOpcional($"Apellido ({alu.Apellido}): ", alu.Apellido);
                            alu.Nombre = SolicitarEntradaOpcional($"Nombre ({alu.Nombre}): ", alu.Nombre);
                            alu.Documento = SolicitarEntradaOpcional($"Documento ({alu.Documento}): ", alu.Documento, EsSoloNumeros);
                            alu.Email = SolicitarEntradaOpcional($"Email ({alu.Email}): ", alu.Email, EsEmailValido);
                            alu.Telefono = SolicitarEntradaOpcional($"Teléfono ({alu.Telefono}): ", alu.Telefono, EsSoloNumeros);

                            Console.WriteLine("✅ Registro actualizado en memoria.");
                        }
                        else Console.WriteLine("❌ No se encontró ese Legajo.");

                        Pause();
                        break;

                    case "3":
                        string legDel = SolicitarEntrada("Ingrese el Legajo a eliminar: ");
                        Alumno aluDel = listaMemoria.FirstOrDefault(x => x.Legajo == legDel);

                        if (aluDel != null)
                        {
                            Console.WriteLine($"¿Eliminar a {aluDel.Apellido}, {aluDel.Nombre}? (S/N)");
                            if (Console.ReadLine().ToUpper() == "S")
                            {
                                listaMemoria.Remove(aluDel);
                                Console.WriteLine("✅ Eliminado.");
                            }
                        }
                        else Console.WriteLine("❌ No se encontró ese Legajo.");
                        Pause();
                        break;

                    case "4":
                        gestor.CrearBackup(ruta);
                        gestor.GuardarArchivo(ruta, extension, listaMemoria);
                        guardarYSalir = true;
                        Pause();
                        break;

                    case "5":
                        Console.WriteLine("\nCambios descartados.");
                        cancelar = true;
                        Pause();
                        break;

                    default:
                        Console.WriteLine("Opción inválida");
                        Pause();
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

        static bool EsEmailValido(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            try
            {
                return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            }
            catch
            {
                return false;
            }
        }

        static bool EsSoloNumeros(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor)) return false;
            // Verifica que toda la cadena sean dígitos del 0 al 9
            return Regex.IsMatch(valor, @"^\d+$");
        }

        // === MÉTODOS AUXILIARES PARA LIMPIAR CÓDIGO ===
        static string SolicitarEntrada(string mensaje, Func<string, bool> validador = null, string error = "Valor inválido.")
        {
            string input;
            do
            {
                Console.Write(mensaje);
                input = Console.ReadLine();

                // Si hay validador y falla, o si es vacío (y no hay validador específico), mostramos error
                if ((validador != null && !validador(input)) || (validador == null && string.IsNullOrWhiteSpace(input)))
                {
                    Console.WriteLine("ERROR: " + error);
                    input = null; // Forzamos repetir el bucle
                }
            } while (input == null);

            return input;
        }
        static string SolicitarEntradaOpcional(string mensaje, string valorActual, Func<string, bool> validador = null, string error = "Valor inválido.")
        {
            string input;
            do
            {
                Console.Write(mensaje);
                input = Console.ReadLine();

                // Si deja vacío, retornamos el valor original (significa "no cambiar")
                if (string.IsNullOrWhiteSpace(input)) return valorActual;

                // Si escribió algo, validamos
                if (validador != null && !validador(input))
                {
                    Console.WriteLine("ERROR: " + error);
                    input = null; // Forzamos repetir el bucle
                }
            } while (input == null);

            return input;
        }

        static void MostrarTabla(List<Alumno> lista)
        {
            Console.WriteLine("\n");
            Console.WriteLine("=".PadRight(110, '='));
            Console.WriteLine($"| {"Legajo".PadRight(10)} | {"Apellido".PadRight(15)} | {"Nombre".PadRight(20)} | {"Documento".PadRight(12)} | {"Email".PadRight(30)} |");
            Console.WriteLine("=".PadRight(110, '='));

            int contador = 0;
            foreach (var alu in lista)
            {
                Console.WriteLine($"| {alu.Legajo.PadRight(10)} | {alu.Apellido.PadRight(15)} | {alu.Nombre.PadRight(20)} | {alu.Documento.PadRight(12)} | {alu.Email.PadRight(30)} |");
                contador++;

                if (contador % 20 == 0)
                {
                    Console.WriteLine("=".PadRight(110, '='));
                    Console.WriteLine($"--- Mostrando {contador}/{lista.Count}. Tecla para seguir ---");
                    Console.ReadKey();
                    Console.WriteLine();
                }
            }
            Console.WriteLine("=".PadRight(110, '='));
            Console.WriteLine($"Total: {lista.Count}");
        }

        public static void Pause()
        {
            Console.WriteLine("\n");
            Console.WriteLine("Oprima una tecla para continuar");
            Console.ReadKey();
        }
    }
}
