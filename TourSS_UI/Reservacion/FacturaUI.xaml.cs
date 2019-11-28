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
using System.Windows.Shapes;
using TourSSLib.Models;
using TourSSLibrary;

namespace TourSS_UI.Reservacion
{
    /// <summary>
    /// Interaction logic for Factura.xaml
    /// </summary>
    public partial class Factura : Window
    {
        public List<ReservacionDetalle> Detalles { get; set; } = new List<ReservacionDetalle>();
        private List<FacturaModel> Lineas = new List<FacturaModel>();
        private FacturaModel FacturaDetalle;
        public ClienteModel Cliente { get; set; }
        public UsuarioModel Usuario { get; set; }

        //private PrintDialog print = new PrintDialog();

        DataAccess da = new DataAccess();
        
        public Factura()
        {
            InitializeComponent();
        }

        public Factura(List<ReservacionDetalle> items, long clienteID, long usuarioID)
        {
            InitializeComponent();
            this.Owner = App.Current.MainWindow;
            this.Detalles = items;
            Cliente = da.Clientes.Where(x => x.ID == clienteID).FirstOrDefault();
            Usuario = da.Usuarios.Where(x => x.ID == usuarioID).FirstOrDefault();
            
            Fill();
        }

        public void Fill()
        {
            string numeroF = da.Query<string>("select max(reservacionID) + 1 as UltimoID from Reservaciones").SingleOrDefault();
            txtNuevaFactura.Text = $"Reservacion #{numeroF}";
            datePicker.SelectedDate = DateTime.Now.Date;
            lbUser.Content = Usuario.Fullname;
            lbCliente.Content = Cliente.Fullname;
            Lineas.Clear();

            int index = 1;
            decimal total = 0m;
            foreach (var rd in Detalles)
            {
                FacturaDetalle = new FacturaModel();
                FacturaDetalle.Linea = rd;
                FacturaDetalle.Numero = index;
                total += rd.Total;
                Lineas.Add(FacturaDetalle);
                index++;
            }
            lbTotal.Content = total.ToString("C2");
            dgNuevaFactura.ItemsSource = Lineas;
        }

        private void btnConfirmar_Click(object sender, RoutedEventArgs e)
        {
            if (Detalles != null)
            {
                var output = da.Reservar(Detalles, Cliente.ID, Usuario.ID);
                long nueva = 0;
                if (long.TryParse(output, out nueva))
                {
                    if(nueva > 1000)
                    {
                        MessageBox.Show($"RESERVACION #{output} CREADA.");

                        //if ((bool)print.ShowDialog().GetValueOrDefault())
                        //{
                        //    print.PrintVisual(this, "Factura");
                        //}
                    }
                }
                else
                    MessageBox.Show($"ERROR. NO SE PUDO CREAR RESERVACION", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                
                this.Close();
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
