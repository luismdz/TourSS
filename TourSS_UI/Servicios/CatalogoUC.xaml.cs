using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
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
        private int opc = 0;

        public CatalogoUC()
        {
            InitializeComponent();
            Fill();
        }

        private void Fill()
        {
            ubicaciones = da.Ubicaciones;
            servicios = da.Servicios;

            dgServicios.ItemsSource = servicios.OrderBy(x => x.Descripcion);
            comboxUbicacion.ItemsSource = ubicaciones;
            //comboxUbicacion.SelectedIndex = 0;
            DatePickerFrom.DisplayDateStart = DateTime.Now;
            DatePickerTo.DisplayDateStart = DateTime.Now;
        }

        private static readonly Regex regex = new Regex("[^0-9]");
        private static bool IsTextAllowed(string text) => !regex.IsMatch(text);


        private void comboxUbicacion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var ubicacion = comboxUbicacion.SelectedItem as UbicacionModel;

            if (ubicacion != null)
                dgServicios.ItemsSource = servicios.Where(x => x.UbicacionID == ubicacion.ID);
        }

        private void btnClearFilters_Click(object sender, RoutedEventArgs e)
        {
            DatePickerFrom.SelectedDate = null;
            DatePickerTo.SelectedDate = null;
            comboxUbicacion.ItemsSource = null;

            dgServicios.ItemsSource = servicios;
            comboxUbicacion.ItemsSource = ubicaciones;
        }

        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            if(DatePickerFrom.SelectedDate != null || DatePickerTo.SelectedDate != null || comboxUbicacion.SelectedItem != null)
            {
                DateTime? fechaD;
                DateTime? fechaH;
                UbicacionModel ubicacion;

                if (DatePickerFrom.SelectedDate != null && DatePickerTo.SelectedDate != null && comboxUbicacion.SelectedItem == null)
                {
                    fechaD = DatePickerFrom.SelectedDate.Value.Date;
                    fechaH = DatePickerTo.SelectedDate.Value.Date;
                    dgServicios.ItemsSource = servicios.Where(s => s.fechaDesde >= fechaD && s.fechaHasta <= fechaH);
                }
                    
                else if (DatePickerFrom.SelectedDate != null && DatePickerTo.SelectedDate == null && comboxUbicacion.SelectedItem == null)
                {
                    fechaD = DatePickerFrom.SelectedDate.Value.Date;
                    dgServicios.ItemsSource = servicios.Where(s => s.fechaDesde >= fechaD);
                }
                    
                else if (DatePickerFrom.SelectedDate == null && DatePickerTo.SelectedDate != null && comboxUbicacion.SelectedItem == null)
                {
                    fechaH = DatePickerTo.SelectedDate.Value.Date;
                    dgServicios.ItemsSource = servicios.Where(s => s.fechaHasta <= fechaH);
                }       

                else if (DatePickerFrom.SelectedDate != null && DatePickerTo.SelectedDate != null && comboxUbicacion.SelectedItem != null)
                {
                    fechaD = DatePickerFrom.SelectedDate.Value.Date;
                    fechaH = DatePickerTo.SelectedDate.Value.Date;
                    ubicacion = comboxUbicacion.SelectedItem as UbicacionModel;
                    dgServicios.ItemsSource = servicios.Where(s => s.UbicacionID == ubicacion.ID && s.fechaDesde >= fechaD && s.fechaHasta <= fechaH);
                }
                 
                else
                    dgServicios.ItemsSource = servicios;
            }  
        }

        private void dgBtnDetalle_Click(object sender, RoutedEventArgs e)
        {
            if(dgServicios.SelectedItem != null)
            {
                var selection = dgServicios.SelectedItem as ServicioModel;

                lbDescripcion.Content = servicios.Where(x => x.ID == selection.ID).Select(x => x.Descripcion).SingleOrDefault();
                lbPrecio.Content = servicios.Where(x => x.ID == selection.ID).Select(x => x.Precio.ToString("C2")).SingleOrDefault();
                lbCupos.Content = servicios.Where(x => x.ID == selection.ID).Select(x => x.CuposDisponibles.ToString()).SingleOrDefault();
                cbxDetallePais.ItemsSource = ubicaciones.Where(x => x.ID == selection.UbicacionID);
                cbxDetallePais.SelectedIndex = 0;

                var fechaDesde = servicios.Where(x => x.ID == selection.ID).Select(x => x.fechaDesde).SingleOrDefault();
                dateDetailsFrom.SelectedDate = fechaDesde;
                dateDetailsTo.SelectedDate = servicios.Where(x => x.ID == selection.ID).Select(x => x.fechaHasta).SingleOrDefault();
            }
        }

        private void Enable()
        {
            lbDescripcion.Visibility = Visibility.Collapsed;
            txtDescripcion.Visibility = Visibility.Visible;

            lbPrecio.Visibility = Visibility.Collapsed;
            txtPrecio.Visibility = Visibility.Visible;
            
            lbCupos.Visibility = Visibility.Collapsed;
            qtyPicker.Visibility = Visibility.Visible;
            
            cbxDetallePais.ItemsSource = ubicaciones;
            dateDetailsFrom.IsEnabled = true;
            dateDetailsTo.IsEnabled = true;
            
            dateDetailsFrom.SelectedDate = DateTime.Now.Date;
            dateDetailsTo.SelectedDate = DateTime.Now.Date;
            
            btnGuardar.IsEnabled = true;
            btnEditar.IsEnabled = false;

            MainWindow mw = (MainWindow)Window.GetWindow(this);
            mw.ListViewMenu.IsEnabled = false;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            opc = 1;

            MainWindow mw = (MainWindow)Application.Current.MainWindow;
            var usuario = mw.Current;

            var agregarCredencial = new AgregarCredenciales(usuario, true);
            agregarCredencial.ShowDialog();

            if(agregarCredencial.GetIsAdmin())
            {
                Enable();
            }
        }

        private bool ValidarDatos()
        {
            return !string.IsNullOrEmpty(txtDescripcion.Text) && !string.IsNullOrEmpty(txtPrecio.Text)
                && cbxDetallePais.SelectedItem != null && dateDetailsFrom.SelectedDate.Value != null
                && dateDetailsTo.SelectedDate.Value != null ? true : false;
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {

            if (opc == 1)
            {
                if (ValidarDatos())
                {
                    var ubicacion = cbxDetallePais.SelectedItem as UbicacionModel;

                    var servicio = new ServicioModel()
                    {
                        Descripcion = txtDescripcion.Text,
                        Precio = Convert.ToDecimal(txtPrecio.Text),
                        CuposDisponibles = (int)qtyPicker.Value,
                        fechaDesde = dateDetailsFrom.SelectedDate.Value,
                        fechaHasta = dateDetailsTo.SelectedDate.Value,
                        UbicacionID = ubicacion.ID
                    };

                    var output = da.Insert(servicio);
                    if (output > 0)
                        MessageBox.Show("NUEVO SERVICIO REGISTRADO EXITOSAMENTE!", "REGISTRADO", MessageBoxButton.OK, MessageBoxImage.Information);
                    else
                        MessageBox.Show("HA OCURRIDO UN ERROR", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else if (opc == 2)
            {
                if (ValidarDatos())
                {
                    var selection = dgServicios.SelectedItem as ServicioModel;
                    var ubicacion = cbxDetallePais.SelectedItem as UbicacionModel;

                    var servicio = new ServicioModel()
                    {
                        ID = selection.ID,
                        Descripcion = txtDescripcion.Text,
                        Precio = decimal.Parse((string)txtPrecio.Text, NumberStyles.AllowCurrencySymbol | NumberStyles.Number),
                        CuposDisponibles = (int)qtyPicker.Value,
                        fechaDesde = dateDetailsFrom.SelectedDate.Value,
                        fechaHasta = dateDetailsTo.SelectedDate.Value,
                        UbicacionID = ubicacion.ID
                    };

                    var output = da.Edit(servicio);

                    if (output > 0)
                        MessageBox.Show("SERVICIO ACTUALIZADO", "ACTUALIZADO", MessageBoxButton.OK, MessageBoxImage.Information);
                    else
                        MessageBox.Show("HA OCURRIDO UN ERROR", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

             MainWindow mw = (MainWindow)Window.GetWindow(this);
             mw.GridPrincipal.Children.Clear();
             mw.GridPrincipal.Children.Add(new CatalogoUC());
             mw.ListViewMenu.IsEnabled = true;
        }

        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            if(dgServicios.SelectedItem  == null)
                MessageBox.Show("SELECCIONE UN REGISTRO", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                MainWindow mw = (MainWindow)Application.Current.MainWindow;
                var usuario = mw.Current;

                var agregarCredencial = new AgregarCredenciales(usuario, true);
                agregarCredencial.ShowDialog();

                if (agregarCredencial.GetIsAdmin())
                {
                    opc = 2;

                    lbDescripcion.Visibility = Visibility.Collapsed;
                    txtDescripcion.Visibility = Visibility.Visible;

                    lbPrecio.Visibility = Visibility.Collapsed;
                    txtPrecio.Visibility = Visibility.Visible;

                    lbCupos.Visibility = Visibility.Collapsed;
                    qtyPicker.Visibility = Visibility.Visible;

                    cbxDetallePais.ItemsSource = ubicaciones;
                    dateDetailsFrom.IsEnabled = true;
                    dateDetailsTo.IsEnabled = true;

                    txtDescripcion.Text = lbDescripcion.Content.ToString();
                    txtPrecio.Text = lbPrecio.Content.ToString();
                    cbxDetallePais.ItemsSource = ubicaciones;
                    qtyPicker.Value = Convert.ToInt32(lbCupos.Content);
                    btnGuardar.IsEnabled = true;

                    dateDetailsFrom.IsEnabled = true;
                    dateDetailsTo.IsEnabled = true;

                    mw = (MainWindow)Window.GetWindow(this);
                    mw.ListViewMenu.IsEnabled = false;
                }
                
            }                       
        }

        private void txtPrecio_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }
    }
}
