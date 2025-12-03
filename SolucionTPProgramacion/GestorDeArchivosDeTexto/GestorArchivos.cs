using GestorDeArchivosDeTexto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GestorDeArchivosDeTexto
{
    public class GestorArchivos
    {

        public void CrearBackup(string rutaOriginal)
        {
            try
            {
                if (File.Exists(rutaOriginal))
                {
                    string rutaBackup = rutaOriginal + ".bak";
                    // true = sobrescribir si ya existe un backup viejo
                    File.Copy(rutaOriginal, rutaBackup, true);
                    Console.WriteLine($"Backup creado: {Path.GetFileName(rutaBackup)}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: No se pudo crear el backup. {ex.Message}");
            }
        }
        public void GuardarArchivo(string rutaCompleta, string extension, List<Alumno> alumnos)
        {
            try
            {
                switch (extension)
                {
                    case ".txt": GuardarTxt(rutaCompleta, alumnos); break;
                    case ".csv": GuardarCsv(rutaCompleta, alumnos); break;
                    case ".json": GuardarJson(rutaCompleta, alumnos); break;
                    case ".xml": GuardarXml(rutaCompleta, alumnos); break;
                }
                Console.WriteLine($"\n Archivo guardado correctamente en: {rutaCompleta}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n Error al guardar el archivo: {ex.Message}");
            }
        }

        public void LeerArchivo()
        {
            // metodo innecesario, utilizamos el metodo de vito
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

        public List<Alumno> LeerAlumnosDesdeArchivo(string extension, string rutaCompleta)
        {

            // crear la lista donde se van a cargar los datos
            List<Alumno> listaAlumnos = new List<Alumno>();

            try
            {
                switch (extension)
                {
                    case ".txt":

                        // leo TXT (separado por |)
                        listaAlumnos = LeerTxt(rutaCompleta);
                        break;

                    case ".csv":

                        // leo CSV (separado por , con encabezado)
                        listaAlumnos = LeerCsv(rutaCompleta);
                        break;

                    case ".json":

                        // leo JSON (deserializacion)
                        listaAlumnos = LeerJson(rutaCompleta);
                        break;

                    case ".xml":

                        // leo XML (deserializacion)
                        listaAlumnos = LeerXml(rutaCompleta);
                        break;

                    //default:
                       // Console.WriteLine($"ERROR: Formato '{extension}' no soportado.");
                      //  return new List<Alumno>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR DE PARSEO en {extension}: El archivo no tiene el formato correcto o está dañado. Detalle: {ex.Message}");
                
                //devuelve lista vacia, sirve para evitar errores 
                return new List<Alumno>(); 
            }

            // Devuelvo la lista cargada
            return listaAlumnos;
        }

        // METODOS DE LECTURA POR FORMATO 

        private static List<Alumno> LeerTxt(string ruta)
        {
            List<Alumno> alumnos = new List<Alumno>();
            string[] lineas = File.ReadAllLines(ruta);

            foreach (string linea in lineas.Where(l => !string.IsNullOrWhiteSpace(l)))
            {
                //spliteo dividiendo cuando aparece | (pipe)

                string[] campos = linea.Split('|');

                if (campos.Length == 6)
                {
                    //aca uso el constructor vacio de Alumno
                    alumnos.Add(new Alumno
                    {
                        Legajo = campos[0].Trim(),
                        Apellido = campos[1].Trim(),
                        Nombre = campos[2].Trim(),
                        Documento = campos[3].Trim(),
                        Email = campos[4].Trim(),
                        Telefono = campos[5].Trim()
                    });
                }
            }
            return alumnos;
        }

        private static List<Alumno> LeerCsv(string ruta)
        {
            List<Alumno> alumnos = new List<Alumno>();
            string[] lineas = File.ReadAllLines(ruta);

            // Empiezo desde la linea 1 (i = 1) para omitir el encabezado (que esta en la linea 0)

            for (int i = 1; i < lineas.Length; i++)
            {
                string linea = lineas[i];

                //skipeo lineas vacias
                if (string.IsNullOrWhiteSpace(linea)) continue;

                string[] campos = linea.Split(',');

                if (campos.Length == 6)
                {
                    alumnos.Add(new Alumno
                    {
                        Legajo = campos[0].Trim(),
                        Apellido = campos[1].Trim(),
                        Nombre = campos[2].Trim(),
                        Documento = campos[3].Trim(),
                        Email = campos[4].Trim(),
                        Telefono = campos[5].Trim()
                    });
                }
            }
            return alumnos;
        }

        private static List<Alumno> LeerJson(string ruta)
        {
            string jsonString = File.ReadAllText(ruta);
            // Deserializa la cadena JSON directamente a la lista alumno

            // Uso la libreria System.Text.Json
            return JsonSerializer.Deserialize<List<Alumno>>(jsonString);
        }

        private static List<Alumno> LeerXml(string ruta)
        {
            // Necesita el atributo XmlRootAttribute("Alumnos") para reconocer la etiqueta raíz
            XmlSerializer serializer = new XmlSerializer(typeof(List<Alumno>), new XmlRootAttribute("Alumnos"));

            using (StreamReader reader = new StreamReader(ruta))
            {
                // Deserializa desde el Stream del archivo
                return (List<Alumno>)serializer.Deserialize(reader);
            }
        }


        // MÉTODOS DE ESCRITURA (Agregados para guardar archivos) 
        private void GuardarTxt(string ruta, List<Alumno> alumnos)
        {
            using (StreamWriter writer = new StreamWriter(ruta, false))
            {
                foreach (var alu in alumnos)
                {
                    // Formato: Legajo|Apellido|Nombre|Documento|Email|Telefono
                    writer.WriteLine($"{alu.Legajo}|{alu.Apellido}|{alu.Nombre}|{alu.Documento}|{alu.Email}|{alu.Telefono}");
                }
            }
        }

        private void GuardarCsv(string ruta, List<Alumno> alumnos)
        {
            using (StreamWriter writer = new StreamWriter(ruta, false))
            {
                // Escribir Encabezado CSV
                writer.WriteLine("Legajo,Apellido,Nombre,NumeroDocumento,Email,Telefono");

                foreach (var alu in alumnos)
                {
                    // Formato separado por comas
                    writer.WriteLine($"{alu.Legajo},{alu.Apellido},{alu.Nombre},{alu.Documento},{alu.Email},{alu.Telefono}");
                }
            }
        }

        private void GuardarJson(string ruta, List<Alumno> alumnos)
        {
            var opciones = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(alumnos, opciones);
            File.WriteAllText(ruta, jsonString);
        }

        private void GuardarXml(string ruta, List<Alumno> alumnos)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Alumno>), new XmlRootAttribute("Alumnos"));

            using (StreamWriter writer = new StreamWriter(ruta))
            {
                serializer.Serialize(writer, alumnos);
            }
        }

    }
}
