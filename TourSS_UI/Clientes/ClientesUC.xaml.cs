using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TourSS_UI.Clientes;
using TourSSLibrary;

namespace TourSS_UI
{
    /// <summary>
    /// Interaction logic for UCClientes.xaml
    /// </summary>
    public partial class ClientesUC : UserControl
    {
        private readonly DataAccess da = new DataAccess();
        //PrintDialog print = new PrintDialog();

        private IList<ClienteModel> Clientes = new List<ClienteModel>();
        
        public ClientesUC()
        {
            InitializeComponent();
            DataContext = this;

            FillDataGrid();
        }

        private void FillDataGrid()
        {
            Clientes = da.Clientes;
            ClientesDataGrid.ItemsSource = Clientes;   
        }

        private void BtnAgregarCliente_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = (MainWindow)Window.GetWindow(this);
            mw.GridPrincipal.Children.Clear();
            mw.GridPrincipal.Children.Add(new AgregarClienteUC());
            mw.ListViewMenu.IsEnabled = false;
        }

        private void TxtBuscarCliente_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtBuscarCedula.Text = "";
            string txt = txtBuscarCliente.Text;
            ClientesDataGrid.ItemsSource = Clientes.Where(x => x.Fullname.ToUpper().Contains(txt)).ToList();
        }

        private void TxtBuscarCedula_KeyDown(object sender, KeyEventArgs e)
        { 
            txtBuscarCliente.Text = "";
            
            if (e.Key == Key.Enter)
            {
                if (!txtBuscarCedula.IsMaskCompleted)
                {
                    ClientesDataGrid.ItemsSource = Clientes;
                }
                else
                {
                    string cedula = txtBuscarCedula.Text;
                    var clientesBuscados = Clientes.Where(c => c.Cedula == cedula).ToList();
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
        private void ClientesDataGrid_CopyingRowClipboardContent(object sender, DataGridRowClipboardEventArgs e)
        {
            var currenCell = e.ClipboardRowContent[ClientesDataGrid.CurrentCell.Column.DisplayIndex];
            e.ClipboardRowContent.Clear();
            e.ClipboardRowContent.Add(currenCell);
        }

        private void DgBtnDetalle_Click(object sender, RoutedEventArgs e)
        {
            var selected = ClientesDataGrid.SelectedItem as ClienteModel;
            var detalle = new ClienteDetalle(selected);
            detalle.ShowDialog();
        }

        private void DgBtnEditar_Click(object sender, RoutedEventArgs e)
        {
            var selected = ClientesDataGrid.SelectedItem as ClienteModel;
            var detalle = new ClienteDetalle(selected, true);
            detalle.ShowDialog();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            FillDataGrid();
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
