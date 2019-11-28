using System;
using System.Collections.Generic;
using System.Text;
using TourSSLib.Models;

namespace TourSSLibrary
{
    public class ReservacionModel
    {
        public long ReservacionID { get; set; }
        public long UsuarioID { get; set; }
        public long ClienteID { get; set; }
        public DateTime Fecha { get; set; }
        public List<ReservacionDetalle> ReservacionDetalle { get; set; } = new List<ReservacionDetalle>();
    }
}
