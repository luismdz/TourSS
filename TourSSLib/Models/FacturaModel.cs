using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourSSLibrary;

namespace TourSSLib.Models
{
    public class FacturaModel
    {
        public ReservacionDetalle Linea { get; set; }
        public int Numero { get; set; }
        public string Descripcion { get => $"{Linea.Cantidad} X {Linea.Servicio.Descripcion}"; }
        public decimal Total { get => Linea.Total; }
    }
}
