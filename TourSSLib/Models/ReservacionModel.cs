using System;
using System.Collections.Generic;
using System.Text;

namespace TourSSLibrary
{
    public class ReservacionModel
    {
        public UsuarioModel Usuario { get; set; }
        public ClienteModel Cliente { get; set; }
        public string Fecha { get; set; }

        /// <summary>
        /// Lista de Servicios en la Reservacion
        /// Obtiene Precio Unidad de cada Servicio
        /// </summary>
        public List<ServicioModel> Servicios { get; set; }

        public ServicioModel Servicio { get; set; }
        //public decimal PrecioUnidad { get; set; }
        public int Cantidad { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Itbis { get; set; }
        public decimal Total { get => Subtotal + Itbis; }

        //public string SubtotalFormatted { get => Subtotal.ToString("C2"); }
        //public string ItbisFormatted { get => Itbis.ToString("C2"); }
        //public string TotalFormatted { get => Total.ToString("C2"); }

        public ReservacionModel()
        {
            Usuario = new UsuarioModel();
            Cliente = new ClienteModel();
            Servicio = new ServicioModel();

            // Lista de Servicios al ser Consultado
            Servicios = new List<ServicioModel>();
        }

    }
}
