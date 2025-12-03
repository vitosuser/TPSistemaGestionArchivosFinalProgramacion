using GestorDeArchivosDeTexto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorDeArchivosDeTexto
{
    public class GeneradorReportes
    {
        public string GenerarReporte(List<Alumno> alumnos)
        {
            var sb = new StringBuilder();
            string separadorPrincipal = new string('=', 80);
            string separadorSecundario = new string('-', 80);
            string tab = "  "; // 2 espacios para la indentacion

            // ENCABEZADO PRINCIPAL 
            sb.AppendLine(separadorPrincipal);
            sb.AppendLine("".PadRight(25) + "REPORTE DE ALUMNOS POR APELLIDO");
            sb.AppendLine($"{"".PadRight(25)}Fecha: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
            sb.AppendLine(separadorPrincipal);

            // Ordeno por apellido
            // La comparación OrdinalIgnoreCase me asegura que la agrupacion y el orden no sean afectados por mayusculas o minusculas
            var alumnosOrdenados = alumnos.OrderBy(a => a.Apellido, StringComparer.OrdinalIgnoreCase).ToList();

            // Agrupo por Apellido (aca hago el corte de control)
            var gruposPorApellido = alumnosOrdenados.GroupBy(a => a.Apellido);

            int totalAlumnos = 0; //contador del total de alumnos

            //  Estructura del reporte

            foreach (var grupo in gruposPorApellido)
            {
                string apellidoActual = grupo.Key.ToUpper();
                int subtotal = 0; //contador del grupo

                // Titulo del grupo 
                sb.AppendLine($"APELLIDO: {apellidoActual}");
                sb.AppendLine(separadorSecundario);

                // Detalle de cada alumno en el grupo
                foreach (var alumno in grupo)
                {
                    sb.AppendLine($"{tab}Legajo: {alumno.Legajo.PadRight(10)}");
                    sb.AppendLine($"{tab}Documento: {alumno.Documento}");
                    sb.AppendLine($"{tab}Nombre: {alumno.Nombre}");
                    sb.AppendLine($"{tab}Email: {alumno.Email}");
                    sb.AppendLine($"{tab}Teléfono: {alumno.Telefono}");
                    sb.AppendLine(new string(' ', 80)); // Espacio vertical para separar registros

                    subtotal++;
                }

                // Subtotal del grupo
                sb.AppendLine(separadorSecundario);
                sb.AppendLine($"{tab}→ Subtotal {apellidoActual}: {subtotal} alumnos");
                sb.AppendLine();

                totalAlumnos = totalAlumnos + subtotal;
            }

            // Resumen 

            int totalApellidosDiferentes = gruposPorApellido.Count();

            sb.AppendLine(separadorPrincipal);
            sb.AppendLine("".PadRight(28) + "RESUMEN GENERAL");
            sb.AppendLine(separadorPrincipal);

            sb.AppendLine($"Total de Apellidos diferentes: {totalApellidosDiferentes}");
            sb.AppendLine($"Total de Alumnos registrados: {totalAlumnos}");
            sb.AppendLine(separadorPrincipal);

            return sb.ToString(); //por si se quiere guardar en un .txt
        }
    }
}

