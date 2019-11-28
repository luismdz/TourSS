using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourSSLibrary;

namespace TourSSLib.Models
{
    public class Credencial
    {
        public long credencialID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public long UsuarioID { get; set; }
                                          
    }
}
