using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorDeArchivosDeTexto.Models
{
    public class Alumno
    {
        public string Legajo { get; set; }
        public string Apellido { get; set; }
        public string Nombre { get; set; }
        public string Documento { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }


        public Alumno(string legajo, string apellido, string nombre, string documento, string email, string telefono)
        {
            Legajo = legajo;
            Apellido = apellido;
            Nombre = nombre;
            Documento = documento;
            Email = email;
            Telefono = telefono;
        }

        // Constructor vacio(necesario para la serializacion de JSON, XML y la inicialización con { } en la lectura de csv)
        public Alumno()
        {

        }
    }
}
