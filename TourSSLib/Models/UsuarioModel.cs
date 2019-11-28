using System;
using System.Collections.Generic;
using System.Text;
using TourSSLib.Models;

namespace TourSSLibrary
{
    public class UsuarioModel
    {
        public long ID { get; set; }
        public string Codigo { get; set; }
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Genero { get; set; }
        public string Correo { get; set; }
        public Boolean Estado { get; set; }
        public int RolID { get; set; }
        public long CredencialID { get; set; }
        public List<TelefonoModel> Telefonos { get; set; }
        public Rol Rol { get; set; }
        public Credencial Credencial { get; set; }

        public string Fullname { get => $"{Nombre} {Apellido}"; }
        public bool isAdmin { 
            get => Rol.Descripcion == "GERENTE" || Rol.Descripcion == "DBA" || Rol.Descripcion == "ADMINISTRADOR" ? true : false;
        }
    }
}
