using GestorDeArchivosDeTexto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GestorDeArchivosDeTexto
{
    public class Conversor
    {
    //    public bool EscribirAlumnosAArchivo(List<Alumno> alumnos, string rutaCompleta, string extension)
    //    {
    //        try
    //        {
    //            string extension = Path.GetExtension(rutaCompleta).ToLowerInvariant();

    //            switch (extension)
    //            {
    //                case ".txt":
    //                    EscribirTxt(alumnos, rutaCompleta);
    //                    break;
    //                case ".csv":
    //                    EscribirCsv(alumnos, rutaCompleta);
    //                    break;
    //                case ".json":
    //                    EscribirJson(alumnos, rutaCompleta);
    //                    break;
    //                case ".xml":
    //                    EscribirXml(alumnos, rutaCompleta);
    //                    break;
    //                default:
    //                    Console.WriteLine($"[ERROR ESCRITURA]: Formato '{extension}' no soportado para escritura.");
    //                    return false;
    //            }

    //            return true; // Éxito en la escritura
    //        }
    //        catch (Exception ex)
    //        {
    //            // Manejo de errores de I/O (permisos, disco lleno, etc.)
    //            Console.WriteLine($"[ERROR FATAL DE ESCRITURA]: No se pudo guardar el archivo en {rutaCompleta}. Detalle: {ex.Message}");
    //            return false;
    //        }
    //    }
    }
    }

