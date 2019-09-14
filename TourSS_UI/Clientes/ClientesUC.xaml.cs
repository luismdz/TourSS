using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TourSSLibrary;

namespace TourSS_UI
{
    /// <summary>
    /// Interaction logic for UCClientes.xaml
    /// </summary>
    public partial class ClientesUC : UserControl
    {
        private DataAccess da = new DataAccess();
        //PrintDialog print = new PrintDialog();
      
        public IList<ClienteModel> Clientes { get; set; }
        
        public ClientesUC()
        {
            InitializeComponent();
            DataContext = this;

            FillDataGrid();
        }

        private void FillDataGrid()
        {
            //Clientes = da.GetAllClientes();
            Clientes = da.GetAll<ClienteModel>("Clientes");
            ClientesDataGrid.ItemsSource = Clientes;
          
        }
        private void BtnAgregarCliente_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = (MainWindow)Window.GetWindow(this);
            mw.GridPrincipal.Children.Clear();
            mw.GridPrincipal.Children.Add(new AgregarClienteUC());
            mw.ListViewMenu.IsEnabled = false;
        }

        //private void BtnBuscarCliente_Click(object sender, RoutedEventArgs e)
        //{
        //    var clientesBuscados = new List<ClienteModel>();

        //    if (txtBuscarCliente.Text == "")
        //    {
        //        ClientesDataGrid.ItemsSource = da.GetAllClientes();
        //    }
        //    else
        //    {
        //        string nombre = txtBuscarCliente.Text;
        //        clientesBuscados = da.BuscarClientesNombre(nombre);

        //        ClientesDataGrid.ItemsSource = clientesBuscados;
        //    }
        //}

        private void TxtBuscarCliente_TextChanged(object sender, TextChangedEventArgs e)
        {
            var clientesBuscados = new List<ClienteModel>();

            txtBuscarCedula.Text = "";

            if (txtBuscarCliente.Text == "")
            {
                ClientesDataGrid.ItemsSource = da.GetAll<ClienteModel>("Clientes");
            }
            else
            {
                string nombre = txtBuscarCliente.Text;
                clientesBuscados = da.BuscarClientes(new string[] { null, nombre, null });

                if (clientesBuscados != null)
                    ClientesDataGrid.ItemsSource = clientesBuscados;
            }
        }

        private void TxtBuscarCedula_KeyDown(object sender, KeyEventArgs e)
        { 
            txtBuscarCliente.Text = "";
            
            if (e.Key == Key.Enter)
            {
                if (!txtBuscarCedula.IsMaskCompleted)
                {
                    ClientesDataGrid.ItemsSource = da.GetAll<ClienteModel>("Clientes");
                }
                else
                {
                    var clientesBuscados = new List<ClienteModel>();
                    string cedula = txtBuscarCedula.Text;
                    clientesBuscados = da.BuscarClientes(new string[] { null, null, cedula });

                    if (clientesBuscados != null)
                    {
                        ClientesDataGrid.ItemsSource = clientesBuscados;
                        txtBuscarCedula.Text = "";
                    }    
                }
            }
        }

        /// <summary>
        /// Permite copiar de celda
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClientesDataGrid_CopyingRowClipboardContent(object sender, DataGridRowClipboardEventArgs e)
        {
            var currenCell = e.ClipboardRowContent[ClientesDataGrid.CurrentCell.Column.DisplayIndex];
            e.ClipboardRowContent.Clear();
            e.ClipboardRowContent.Add(currenCell);
        }


        //private void BtnPrint_Click(object sender, RoutedEventArgs e)
        //{
        //    if ((bool)print.ShowDialog().GetValueOrDefault())
        //    {
        //        Size size = new Size(print.PrintableAreaWidth, print.PrintableAreaHeight);
        //        ClientesDataGrid.Measure(size);
        //        ClientesDataGrid.Arrange(new Rect(5, 5, size.Width, size.Height));
        //        print.PrintVisual(ClientesDataGrid, "Print PDF");
        //    }
        //}
    }
}
