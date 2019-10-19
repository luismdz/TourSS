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
using TourSSLibrary;

namespace TourSS_UI
{
    /// <summary>
    /// Interaction logic for ReservacionUC.xaml
    /// </summary>
    public partial class ReservacionUC : UserControl
    {
        private DataAccess da = new DataAccess();

        IList<ReservacionModel> Items = new List<ReservacionModel>();
        ReservacionModel reservacion;

        // Crear tabla temporal para agregar como parametro (Agregar a DataAccess)
        DataTable servicios = new DataTable("List");
        ClienteModel reservacionCliente;

        //// Listas para pasar como argumentos Tipo "Tabla" al Stored Procedure
        //List<long> serviciosID = new List<long>();
        //List<int> cantidades = new List<int>();
        //List<string> fechas = new List<string>();

        public UsuarioModel Usuario { get; set; }

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
            lbUser.Content = $"[{Usuario.Codigo}] {Usuario.Fullname}";

            comboxClienteR.ItemsSource = da.GetAll<ClienteModel>("Clientes");
            comboxServicioR.ItemsSource = da.GetAll<ServicioModel>("Servicios");

            servicios.Columns.Add("servicioID", typeof(long));
            servicios.Columns.Add("cantidad", typeof(int));

        }

        private static readonly Regex regex = new Regex("[^0-9]");

        private static bool IsTextAllowed(string text) => !regex.IsMatch(text);
        
        private void TxtClienteCodigo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string codigo = "TC-" + txtClienteCodigo.Text;
                if (txtClienteCodigo.Text != "")
                {
                    var clientesBuscados = new List<ClienteModel>();
                    clientesBuscados = da.BuscarClientes(new string[] { codigo, null, null });

                    if (clientesBuscados == null)
                    {
                        _ = MessageBox.Show($"Cliente [ {codigo} ] no existe", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                        lbClienteNombre.Content = "NO EXISTE";
                    }
                    else
                    {
                        _ = MessageBox.Show($"Cliente [ {codigo} ] ENCONTRADO", "SUCCESS", MessageBoxButton.OK, MessageBoxImage.Information);
                        lbClienteNombre.Content = $"[{clientesBuscados[0].Codigo}] {clientesBuscados[0].Fullname}";

                        //comboxClienteR.SelectedIndex = comboxClienteR.Items.Filter()
                        comboxClienteR.IsEnabled = false;
                    }
                }
                else
                    _ = MessageBox.Show("INGRESE CODIGO DE CLIENTE", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            comboxClienteR.SelectedIndex = -1;
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

        private void BtnAgregar_Click(object sender, RoutedEventArgs e)
        {
            ServicioModel item = (ServicioModel)comboxServicioR.SelectedItem;
            ClienteModel cliente = (ClienteModel)comboxClienteR.SelectedItem;

            if (item != null && cliente != null)
            {
                int qty = (int)qtyPicker.Value;
                decimal subtotal = item.Precio * qty;
                decimal itbis = subtotal * (decimal)0.18;
                decimal total = subtotal + itbis;

                if(qty >= 1)
                {
                    reservacion = new ReservacionModel();
                    reservacionCliente = new ClienteModel();

                    reservacion.Cliente = cliente;
                    reservacion.Servicio = item;
                    reservacion.Cantidad = qty;
                    reservacion.Subtotal = subtotal;
                    reservacion.Itbis = itbis;
                    reservacion.Fecha = dateReservacion.SelectedDate.Value.Date;
                    //reservacion.Total = total;

                    Items.Add(reservacion);    
                    dgReservacion.Items.Add(reservacion);
                    ActualizarPrecios(reservacion, null);

                    //serviciosID.Add(item.ID);
                    //cantidades.Add(qty);
                    //fechas.Add(reservacion.Fecha);

                    // Agregar ID del servicio actual a la Tabla Temporal
                    servicios.Rows.Add(item.ID, qtyPicker.Value);

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
                _ = MessageBox.Show("DEBE SELECCIONAR EL CLIENTE Y AL MENOS UN PRODUCTO", "ERROR", MessageBoxButton.OK);

        }

        /// <summary>
        /// Actualiza el GroupBox del detalle de los precios a medida que se agregan (i) o eliminan (d) servicios en el DataGrid
        /// </summary>
        /// <param name="i"></param>
        /// <param name="d"></param>
        private void ActualizarPrecios(ReservacionModel i, ReservacionModel d)
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
            reservacion.Cantidad < reservacion.Servicio.CuposDisponibles ? true : false; 
       

        private void BtnReservar_Click(object sender, RoutedEventArgs e)
        {
            if(esValida())
            {
                var usuarioID = Usuario.ID;
                var clienteID = reservacion.Cliente.ID;

                var output = da.Reservar(Items, clienteID, usuarioID);

                if (Convert.ToInt64(output) > 1000)
                {
                    MessageBox.Show($"RESERVACION #{output} CREADA.");
                    ClearDataGrid();
                }
                else
                    MessageBox.Show($"ERROR. NO SE PUDO CREAR RESERVACION", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
                MessageBox.Show($"ERROR. ALGUNOS DATOS ESTAN INCORRECTOS, FAVOR VALIDAR!", "DATOS NO VALIDOS", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void ClearDataGrid()
        {
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            var selected = dgReservacion.SelectedItem as ReservacionModel;
            ActualizarPrecios(null, selected);
            dgReservacion.Items.Remove(selected);
        }

        /// <summary>
        /// Establece la cantidad maxima de unidades de servicios quedan disponibles (cupos disponibles)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
    }
}
