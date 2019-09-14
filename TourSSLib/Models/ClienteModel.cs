using System;
using System.Collections.Generic;
using System.Text;

namespace TourSSLibrary
{
    public class ClienteModel
    {        
        public int ID  { get; set; }
        public string Codigo { get; set; }
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public char Genero { get; set; }
        public string Correo { get; set; }
        public List<string> Telefonos { get; set; }

        public string Fullname { get => $"{Nombre} {Apellido}"; }

        public string ClienteInfo { get => $"{Fullname} [{Codigo}]"; }
        //public string Informacion { get => $"{Fullname} | {Cedula} ( {Codigo} )"; }


    }
}
