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

        }

        static void LeerArchivoExistente()
        {

        }

        static void ModificarArchivo()
        {

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

                    if(extOrigen == ".txt")
                    {
                        Console.WriteLine("\n FORMATOS POSIBLES DE CONVERSION: ");
                        Console.WriteLine("\t -CSV ");
                    }
                    else if(extOrigen == ".csv")
                    {
                        Console.WriteLine("\n FORMATOS POSIBLES DE CONVERSION: ");
                        Console.WriteLine("\t -TXT ");
                    }
                    else if(extOrigen == ".csv")
                    {
                        Console.WriteLine("\n FORMATOS POSIBLES DE CONVERSION: ");
                        Console.WriteLine("\t -TXT ");
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
        

        static void Pause()
        {
            Console.WriteLine("\n");
            Console.WriteLine("Oprima una tecla para continuar");
            Console.ReadKey();
        }
    }
}
