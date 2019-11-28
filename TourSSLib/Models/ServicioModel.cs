using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using TourSSLib.Models;

namespace TourSSLibrary
{
    public class ServicioModel
    {

        public long ID { get; set; } 
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int CuposDisponibles { get; set; }
        public DateTime fechaDesde { get; set; }
        public DateTime fechaHasta { get; set; }
        public long UbicacionID { get; set; }

        //public string PrecioFormatted { get => Precio.ToString("C2"); }
        public string RangoFecha { get => $"{fechaDesde.ToString("dd/M/yyyy", CultureInfo.InvariantCulture)} - " +
                $"{fechaHasta.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)}"; }
    }
}
