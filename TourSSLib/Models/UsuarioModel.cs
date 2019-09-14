using System;
using System.Collections.Generic;
using System.Text;

namespace TourSSLibrary
{
    public class UsuarioModel
    {
        public long ID { get; set; }
        public string Codigo { get; set; }
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public char Genero { get; set; }
        public string Correo { get; set; }
        public Boolean Estado { get; set; }
        public string Rol { get; set; }
        public string Fullname { get => $"{Nombre} {Apellido}"; }
        public List<string> Telefonos { get; set; }

    }
}
