using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TourSSLib.Models;
using TourSSLibrary;

namespace TourSS_UI
{
    /// <summary>
    /// Interaction logic for CatalogoUC.xaml
    /// </summary>
    public partial class CatalogoUC : UserControl
    {
        DataAccess da = new DataAccess();
        IEnumerable<UbicacionModel> ubicaciones;
        IEnumerable<ServicioModel> servicios;

        public CatalogoUC()
        {
            InitializeComponent();
            Fill();
        }

        private void Fill()
        {
            ubicaciones = da.GetAll<UbicacionModel>("Paises");
            servicios = da.GetAll<ServicioModel>("Servicios");

            dgServicios.ItemsSource = servicios;
            comboxUbicacion.ItemsSource = ubicaciones;
            //comboxUbicacion.SelectedIndex = 0;
            DatePickerFrom.DisplayDateStart = DateTime.Now;
            DatePickerTo.DisplayDateStart = DateTime.Now;

       
        }

        private void comboxUbicacion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var ubicacion = comboxUbicacion.SelectedItem as UbicacionModel;
            if (ubicacion != null)
                //dgServicios.ItemsSource = da.BuscarServicios(ubicacion.ID, null, null, null);
                dgServicios.ItemsSource = servicios.Where(x => x.UbicacionID == ubicacion.ID);
        }

        //private void DatePickerFrom_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    DateTime fecha = DatePickerFrom.SelectedDate.Value.Date;
        //    dgServicios.ItemsSource = da.BuscarServicios(null, null, fecha, null);
        //}

        //private void DatePickerTo_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    DateTime fecha = DatePickerFrom.SelectedDate.Value.Date;
        //    dgServicios.ItemsSource = da.BuscarServicios(null, null, null, fecha);
        //}

        private void btnClearFilters_Click(object sender, RoutedEventArgs e)
        {
            DatePickerFrom.SelectedDate = null;
            DatePickerTo.SelectedDate = null;
            comboxUbicacion.ItemsSource = null;

            dgServicios.ItemsSource = da.GetAll<ServicioModel>("Servicios");
            comboxUbicacion.ItemsSource = da.GetAll<UbicacionModel>("Paises");
        }

        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            if(DatePickerFrom.SelectedDate != null && DatePickerTo.SelectedDate != null && comboxUbicacion.SelectedItem != null)
            {
                var fechaD = DatePickerFrom.SelectedDate.Value.Date;
                var fechaH = DatePickerTo.SelectedDate.Value.Date;
                var ubicacion = comboxUbicacion.SelectedItem as UbicacionModel;

                if (fechaD != null && fechaH != null && ubicacion == null)
                    dgServicios.ItemsSource = da.BuscarServicios(null, null, fechaD, fechaH);

                else if (fechaD != null && fechaH != null && ubicacion != null)
                    dgServicios.ItemsSource = da.BuscarServicios(ubicacion.ID, null, fechaD, fechaH);
                else
                    dgServicios.ItemsSource = da.GetAll<ServicioModel>("Servicios");
            }
           
        }

        private void dgBtnDetalle_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
