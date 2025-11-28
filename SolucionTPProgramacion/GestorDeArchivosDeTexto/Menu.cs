using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorDeArchivosDeTexto
{
    public class Menu
    {
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

        }

        static void ConvertirEntreFormatos()
        {

        }

        static void CrearReporte()
        {

        }


        public static void Pause()
        {
            Console.WriteLine("\n");
            Console.WriteLine("Oprima una tecla para continuar");
            Console.ReadKey();
        }
    }
}
