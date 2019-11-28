using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TourSS_UI.Reservacion;
using TourSSLib.Models;
using TourSSLibrary;

namespace TourSS_UI
{
    /// <summary>
    /// Interaction logic for ReservacionUC.xaml
    /// </summary>
    public partial class ReservacionUC : UserControl
    {
        private DataAccess da = new DataAccess();
        private List<ReservacionDetalle> Items = new List<ReservacionDetalle>();
        private IList<ClienteModel> clientes;
        private IList<ServicioModel> servicios;
        private ReservacionModel reservacion;
        private ReservacionDetalle reservacionDetalle;
        private UsuarioModel Usuario;

        private static readonly Regex regex = new Regex("[^0-9]");
        private static bool IsTextAllowed(string text) => !regex.IsMatch(text);

        public ReservacionUC()
        {
            InitializeComponent();
            
            FillData();
        }

        private void FillData()
        {
            dateReservacion.SelectedDate = DateTime.Now.Date;

            MainWindow mw = (MainWindow)Application.Current.MainWindow;
            Usuario = mw.Current;
            servicios = da.Servicios;
            clientes = da.Clientes;

            lbUser.Content = $"[{Usuario.Codigo}] {Usuario.Fullname}";
            comboxClienteR.ItemsSource = clientes;
            comboxServicioR.ItemsSource = servicios;
        }
        
        private void TxtClienteCodigo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string codigo = "TC-" + txtClienteCodigo.Text;
                if (!string.IsNullOrEmpty(txtClienteCodigo.Text))
                {
                    ClienteModel cliente = clientes.Where(c => c.Codigo == codigo).SingleOrDefault();

                    if (cliente == null)
                    {
                        _ = MessageBox.Show($"Cliente [ {codigo} ] no existe", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                        lbClienteNombre.Content = "NO EXISTE";
                    }
                    else
                    {
                        _ = MessageBox.Show($"Cliente [ {codigo} ] ENCONTRADO", "SUCCESS", MessageBoxButton.OK, MessageBoxImage.Information);
                        lbClienteNombre.Content = $"[{cliente.Codigo}] {cliente.Fullname}";

                        comboxClienteR.ItemsSource = clientes.Where(c => c.ID == cliente.ID);
                        comboxClienteR.SelectedIndex = 0;
                    }
                }
                else
                {
                    _ = MessageBox.Show("INGRESE CODIGO DE CLIENTE", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    comboxClienteR.SelectedIndex = -1;
                }

                txtClienteCodigo.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
        }

        private void ComboxClienteR_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var seleccion = comboxClienteR.SelectedItem as ClienteModel;

            if (seleccion != null)
            {
                lbClienteNombre.Content = $"[{seleccion.Codigo}] {seleccion.Fullname}";
                txtClienteCodigo.Text = seleccion.Codigo;
            }
        }

        private bool RevisarFechas(ServicioModel i)
        {
            bool result = true;

            foreach(var s in Items)
            {
                result = i.fechaDesde > s.Servicio.fechaHasta ? true : false;
            }
            return result;
        }

        private void BtnAgregar_Click(object sender, RoutedEventArgs e)
        {
            ServicioModel item = (ServicioModel)comboxServicioR.SelectedItem;
            ClienteModel cliente = (ClienteModel)comboxClienteR.SelectedItem;

            if (item != null && cliente != null)
            {
                if(RevisarFechas(item))
                {
                    int qty = (int)qtyPicker.Value;
                    decimal subtotal = item.Precio * qty;
                    decimal itbis = subtotal * (decimal)0.18;
                    //decimal total = subtotal + itbis;

                    if (qty >= 1)
                    {
                        reservacion = new ReservacionModel();
                        reservacionDetalle = new ReservacionDetalle();

                        reservacion.ClienteID = cliente.ID;
                        reservacion.Fecha = dateReservacion.SelectedDate.Value.Date;

                        reservacionDetalle.Servicio = item;
                        reservacionDetalle.Cantidad = qty;
                        reservacionDetalle.Subtotal = subtotal;
                        reservacionDetalle.Itbis = itbis;

                        Items.Add(reservacionDetalle);
                        dgReservacion.Items.Add(reservacionDetalle);
                        ActualizarPrecios(reservacionDetalle, null);

                        //item.CuposDisponibles -= qty;
                        qtyPicker.Value = 1;
                        comboxClienteR.IsEnabled = false;
                        txtClienteCodigo.IsEnabled = false;
                        comboxServicioR.SelectedIndex = -1;
                    }
                    else
                        _ = MessageBox.Show("LA CANTIDAD SELECCIONADA NO ES VALIDA", "ERROR", MessageBoxButton.OK);
                }
                else
                    _ = MessageBox.Show("FECHA DEL SERVICIO CHOCA CON OTRO SERVICIO SELECCIONADO", "ERROR", MessageBoxButton.OK);
            }
            else
                _ = MessageBox.Show("DEBE SELECCIONAR EL CLIENTE Y AL MENOS UN PRODUCTO", "ERROR", MessageBoxButton.OK);

        }

        /// <summary>
        /// Actualiza el GroupBox del detalle de los precios a medida que se agregan (i) o eliminan (d) servicios en el DataGrid
        /// </summary>
        /// <param name="i"></param>
        /// <param name="d"></param>
        private void ActualizarPrecios(ReservacionDetalle i, ReservacionDetalle d)
        {
            decimal subtotal = decimal.Parse((string)lbSubtotal.Content,
                NumberStyles.AllowCurrencySymbol | NumberStyles.Number);

            decimal itbis = decimal.Parse((string)lbItbis.Content,
                NumberStyles.AllowCurrencySymbol | NumberStyles.Number);

            decimal total = decimal.Parse((string)lbTotal.Content,
                NumberStyles.AllowCurrencySymbol | NumberStyles.Number);

            if (i != null)
            {
                subtotal += i.Subtotal;
                itbis += i.Itbis;
                total += i.Total;
            }
            else
            {
                subtotal -= d.Subtotal;
                itbis -= d.Itbis;
                total -= d.Total;
            }
            
            lbSubtotal.Content = subtotal.ToString("C2");
            lbItbis.Content = itbis.ToString("C2");
            lbTotal.Content = total.ToString("C2");
        }

        private bool esValida() =>
            Usuario != null && reservacion != null && Items != null &&
            reservacionDetalle.Cantidad < reservacionDetalle.Servicio.CuposDisponibles ? true : false; 
       

        private void BtnReservar_Click(object sender, RoutedEventArgs e)
        {
            if(esValida())
            {
                var usuarioID = Usuario.ID;
                var clienteID = reservacion.ClienteID;

                var factura = new Factura(Items, clienteID, usuarioID);
                factura.ShowDialog();
                ClearDataGrid();
            }
            else
                MessageBox.Show($"ERROR. ALGUNOS DATOS ESTAN INCORRECTOS, FAVOR VALIDAR!", "DATOS NO VALIDOS", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void ClearDataGrid()
        {
            Items.Clear();
            dgReservacion.Items.Clear();
            dgReservacion.Items.Refresh();
            comboxServicioR.SelectedIndex = -1;
            comboxClienteR.SelectedIndex = -1;
            txtClienteCodigo.Text = "";
            lbClienteNombre.Content = "";

            lbSubtotal.Content = "$0.0";
            lbItbis.Content = "$0.0";
            lbTotal.Content = "$0.0";

            comboxClienteR.IsEnabled = true;
            txtClienteCodigo.IsEnabled = true;
        }

        /// <summary>
        /// Limpia los datos del datagrid y groupbox
        /// </summary>
        private void BtnLimpiarGrid_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Seguro que desea limpiar todos los campos?", "BORRAR TODO", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                ClearDataGrid();
            }
            
        }

        /// <summary>
        /// Elimina registros del datagrid
        /// </summary>
        private void DgBtnBorrar_Click(object sender, RoutedEventArgs e)
        {
            var selected = dgReservacion.SelectedItem as ReservacionDetalle;
            ActualizarPrecios(null, selected);
            dgReservacion.Items.Remove(selected);
            Items.Remove(selected);
        }

        /// <summary>
        /// Establece la cantidad maxima de unidades de servicios quedan disponibles (cupos disponibles)
        /// </summary>
        private void QtyPicker_GotFocus(object sender, RoutedEventArgs e)
        {
            var item = comboxServicioR.SelectedItem as ServicioModel;

            if (item != null)
            {
                int max = item.CuposDisponibles;
                qtyPicker.Maximum = max;
            }
           
        }

        private void TxtClienteCodigo_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void comboxClienteR_DropDownOpened(object sender, EventArgs e)
        {
           comboxClienteR.ItemsSource = clientes;
        }

        private void txtClienteCodigo_GotFocus(object sender, RoutedEventArgs e)
        {
            txtClienteCodigo.Text = "";
        }
    }
}
