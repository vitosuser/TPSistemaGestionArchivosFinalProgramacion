using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorDeArchivosDeTexto
{
    public class GestorArchivos
    {
        public void CrearArchivo()
        {

        }

        public void LeerArchivo()
        {

        }

        public void ModificarArchivo()
        {

        }

        public void EliminarArchivo(string nombreArchivo)
        { 
            //Obtengo la ruta del archivo 

            string ruta = Path.Combine(Environment.CurrentDirectory, nombreArchivo);

            try
            {
                var info = new FileInfo(ruta);
                Console.WriteLine("\n--- Información del archivo ---");
                Console.WriteLine($"Nombre: {info.Name}");
                Console.WriteLine($"Ruta: {info.FullName}");
                Console.WriteLine($"Tamaño: {info.Length / 1024.0:F2} KB");
                Console.WriteLine($"Creación: {info.CreationTime}");
                Console.WriteLine($"Última modificación: {info.LastWriteTime}");
                Console.WriteLine($"Atributos: {info.Attributes}");
                Console.WriteLine("\nEscriba CONFIRMAR para eliminar el archivo:");

                string respuesta = Console.ReadLine();

                if (respuesta.Trim().ToUpperInvariant() != "CONFIRMAR")
                {
                   Console.WriteLine("Operación cancelada por el usuario");
                   return;

                }

                File.Delete(ruta);
                Console.WriteLine("Archivo eliminado correctamente.");
            }
            catch (UnauthorizedAccessException uex)
            {

                Console.WriteLine("ERROR: Permisos insuficientes para eliminar el archivo. " + uex.Message);

            }

            catch (IOException ioex)
            {

                Console.WriteLine("ERROR de E/S al intentar eliminar el archivo: " + ioex.Message);

            }

            catch (Exception ex)
            {

                Console.WriteLine("ERROR inesperado al eliminar el archivo: " + ex.Message);

            }
        }

        public bool ValidarExistencia(string nombreArchivo)
        {
            string ruta = Path.Combine(Environment.CurrentDirectory, nombreArchivo);

            return File.Exists(ruta);
        } 
    }
}
