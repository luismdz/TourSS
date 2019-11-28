using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourSSLib.Models
{
    public class TelefonoModel
    {
        public long ID { get; set; }
        public int clienteID { get; set; }
        public int usuarioID { get; set; }
        public string Numero { get; set; }
        public char Estado { get; set; }
    }
}
