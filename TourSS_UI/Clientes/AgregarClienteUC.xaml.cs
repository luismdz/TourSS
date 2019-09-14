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

namespace TourSS_UI
{
    /// <summary>
    /// Interaction logic for AgregarClienteUC.xaml
    /// </summary>
    public partial class AgregarClienteUC : UserControl
    {
        public AgregarClienteUC()
        {
            InitializeComponent();
        }

        private void Btn_CancelarNuevoCliente_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult message = MessageBox.Show("CANCELAR CREACION DE CLIENTE?", "Cancelar creacion de cliente", MessageBoxButton.YesNo);
            if(message == MessageBoxResult.Yes)
            {
                MainWindow mw = (MainWindow)Window.GetWindow(this);
                mw.GridPrincipal.Children.Clear();
                mw.GridPrincipal.Children.Add(new ClientesUC());
                mw.ListViewMenu.IsEnabled = true;
            }
            
        }

    }
}
