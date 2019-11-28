using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TourSSLibrary;
using DataTable = System.Data.DataTable;
using ClosedXML.Excel;

namespace TourSS_UI.Reportes
{
    /// <summary>
    /// Interaction logic for ReportesUC.xaml
    /// </summary>
    public partial class ReportesUC : UserControl
    {
        private readonly DataAccess da = new DataAccess();
        private readonly List<ReporteModel> registros = new List<ReporteModel>();
        readonly IList<ClienteModel> clientes = new List<ClienteModel>();
        readonly IList<ServicioModel> servicios = new List<ServicioModel>();
        //private int actual = 1;

        public enum TipoDeReporte
        {
            Fechas = 1,
            Clientes = 2,
            //Mensual = 3,
            //Anual = 4
        }

        public enum Meses
        {
            Enero = 1,
            Febrero = 2,
            Marzo = 3,
            Abril = 4,
            Mayo,
            Junio,
            Julio,
            Agosto,
            Septiembre,
            Octubre,
            Noviembre,
            Diciembre
        }


        public ReportesUC()
        {
           InitializeComponent();

           // From database
           registros = da.GenerarReporte(1);
           clientes = da.Clientes;
           servicios = da.Servicios;
            
           // FrontEnd Stuff
           cmbxTipoReporte.ItemsSource = Enum.GetValues(typeof(TipoDeReporte));
           cmbxTipoReporte.SelectedIndex = 0;
           cbxMes.ItemsSource = Enum.GetValues(typeof(Meses));

           dgReportes.ItemsSource = registros.OrderBy(x => x.ReservacionID);
            SetTotal(registros); 
        }

        private void SetTotal(List<ReporteModel> reportes)
        {
            decimal total = 0.0m;
            foreach (var r in reportes) 
            { 
                total += r.Total; 
            }
            lbTotal.Content = total.ToString("C2");
        }

        private void CambiarReporte(TipoDeReporte e)
        {
            switch (e)
            {
                case TipoDeReporte.Fechas:
                    stkFechas.Visibility = Visibility;
                    stkMensual.Visibility = Visibility.Collapsed;
                    dgReportes.Visibility = Visibility.Visible;
                    dgReporteCliente.Visibility = Visibility.Collapsed;

                    //actual = 1;
                    break;

                //case TipoDeReporte.Mensual:
                //    stkFechas.Visibility = Visibility.Collapsed;
                //    stkMensual.Visibility = Visibility.Visible;
                //    break;

                case TipoDeReporte.Clientes:
                    dgReportes.Visibility = Visibility.Collapsed;
                    dgReporteCliente.Visibility = Visibility.Visible;

                    var registros = da.GenerarReporte((int)TipoDeReporte.Clientes);
                    dgReporteCliente.ItemsSource = registros;
                    SetTotal(registros);
                    //actual = 2;

                    break;
                default:
                    break;
            }            
        }


        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            if (dtpDesde.SelectedDate != null && dtpHasta.SelectedDate != null)
            {
                decimal total = 0;
                var fechaD = dtpDesde.SelectedDate.Value.Date;
                var fechaH = dtpHasta.SelectedDate.Value.Date;

                var seleccion = registros.
                    Where(x => x.Reservacion.Fecha >= fechaD && x.Reservacion.Fecha <= fechaH).
                    OrderBy(x => x.ReservacionID);

                dgReportes.ItemsSource = seleccion;
                
                foreach(var r in seleccion)
                {
                    total += r.Total;
                }

                lbTotal.Content = total.ToString("C2");
                //lbTotal.Content = decimal.ToOACurrency(total);
            }
            else if (dtpDesde.SelectedDate != null)
            {
                decimal total = 0;
                var fechaD = dtpDesde.SelectedDate.Value.Date;

                var seleccion = registros.
                    Where(x => x.Reservacion.Fecha >= fechaD).
                    OrderBy(x => x.ReservacionID);
                dgReportes.ItemsSource = seleccion;

                foreach (var r in seleccion)
                {
                    total += r.Total;
                }

                lbTotal.Content = total.ToString("C2");
                //lbTotal.Content = decimal.ToOACurrency(total);
            }
        }

        private void cmbxTipoReporte_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cmbxTipoReporte.SelectedItem != null)
            {
                var selected = (TipoDeReporte)cmbxTipoReporte.SelectedItem;
                CambiarReporte(selected);
            }
        }

        //private void btnExport_Click(object sender, RoutedEventArgs e)
        //{
            
        //}


    }
}
