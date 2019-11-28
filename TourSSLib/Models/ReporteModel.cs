using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using TourSSLib.Models;

namespace TourSSLibrary
{
    public class ReporteModel
    {
        public long ReservacionID { get; set; }
        public long ClienteID { get; set; }
        public long ServicioID { get; set; }
        public long UsuarioID { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Itbis { get; set; }
        public decimal Total { get; set; }
        public ReservacionModel Reservacion { get; set; } = new ReservacionModel();
        public ClienteModel Cliente { get; set; } = new ClienteModel();
        public ServicioModel Servicio { get; set; } = new ServicioModel();
        public UsuarioModel Usuario { get; set; } = new UsuarioModel();
        public List<ReservacionDetalle> Detalles { get; set; } = new List<ReservacionDetalle>();
    }
}
