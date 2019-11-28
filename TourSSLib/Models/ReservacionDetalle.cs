using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourSSLibrary;

namespace TourSSLib.Models
{
    public class ReservacionDetalle
    {
        public long ReservacionID { get; set; }
        public long ServicioID { get; set; }
        public ServicioModel Servicio { get; set; } = new ServicioModel();
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Itbis { get; set; }
        public decimal Total { get => Subtotal + Itbis; }
        //public string SubtotalFormatted { get => Subtotal.ToString("C2"); }
        //public string ItbisFormatted { get => Itbis.ToString("C2"); }
        //public string TotalFormatted { get => Total.ToString("C2"); }
        //public DateTime FechaDesde { get; set; }
        //public DateTime FechaHasta { get; set; }
    }
}
