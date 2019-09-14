using System;
using System.Collections.Generic;
using System.Text;

namespace TourSSLibrary
{
    public class PagosModel
    {
        public ReservacionModel Reservacion { get; set; }
        public decimal Monto { get; set; }
        public string Tipo { get; set; }
        public DateTime Fecha { get; set; }
        public string Concepto { get; set; }
    }
}
