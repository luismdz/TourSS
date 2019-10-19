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
using TourSSLibrary;

namespace TourSS_UI.Clientes
{
    /// <summary>
    /// Interaction logic for ClienteDetalle.xaml
    /// </summary>
    public partial class ClienteDetalle : Window
    {
        public ClienteModel Cliente { get; set; }

        public ClienteDetalle()
        {
            InitializeComponent();
            this.Owner = App.Current.MainWindow;
        }

        public ClienteDetalle(ClienteModel cliente)
        {
            InitializeComponent();
            this.Owner = App.Current.MainWindow;
            this.Cliente = cliente;
            Fill();
        }

        /// <summary>
        /// Llena los campos con la informacion de cliente seleccionado
        /// </summary>
        private void Fill()
        {
            txtNombre.Content = Cliente.Nombre.ToUpper();
            txtApellido.Content = Cliente.Apellido.ToUpper();
            lbCodigo.Content = Cliente.Codigo.ToUpper();
            lbCedula.Content = Cliente.Cedula;
            lbCorreo.Content = Cliente.Correo.ToUpper();
            lbCelular.Content = Cliente.Telefonos[0];

            if (Cliente.Telefonos.Count > 1)
                lbTelResidencia.Content = Cliente.Telefonos[1];
            else
                lbTelResidencia.Content = "(000) 000-0000";

            lbGenero.Content = Cliente.Genero == 'M' ? "MASCULINO" : Cliente.Genero == 'F' ? "FEMENINO" : "INDEFINIDO";  
        }

        private void BtnClose_DetalleCliente_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
