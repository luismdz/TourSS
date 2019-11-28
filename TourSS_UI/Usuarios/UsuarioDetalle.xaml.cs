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

namespace TourSS_UI.Usuarios
{
    /// <summary>
    /// Interaction logic for UsuarioDetalle.xaml
    /// </summary>
    public partial class UsuarioDetalle : Window
    {
        public UsuarioModel Usuario { get; set; }
        public List<string> Generos { get; set; }

        private DataAccess da = new DataAccess();

        public UsuarioDetalle()
        {
            InitializeComponent();
            this.Owner = App.Current.MainWindow;
        }

        public UsuarioDetalle(UsuarioModel usuario, bool asEdit = false)
        {
            InitializeComponent();
            this.Owner = App.Current.MainWindow;

            this.Usuario = usuario;
            Generos = da.Query<string>("select distinct empGenero from Usuarios");

            Fill();

            if (asEdit)
                Enable();
        }

        /// <summary>
        /// Llena los campos con la informacion de cliente seleccionado
        /// </summary>
        private void Fill()
        {
            txtNombre.Content = Usuario.Nombre.ToUpper();
            txtApellido.Content = Usuario.Apellido.ToUpper();
            lbCodigo.Content = Usuario.Codigo.ToUpper();
            lbCedula.Content = Usuario.Cedula;
            lbCorreo.Content = Usuario.Correo.ToUpper();
            lbRol.Content = Usuario.Rol.Descripcion.ToUpper();

            if (Usuario.Telefonos != null)
                lbCelular.Content = Usuario.Telefonos[0].Numero;

            if (Usuario.Telefonos.Count > 1)
                lbTelResidencia.Content = Usuario.Telefonos[1].Numero;
            else
                lbTelResidencia.Content = "";

            lbGenero.Content = Usuario.Genero == "M" ? "MASCULINO" : Usuario.Genero == "F" ? "FEMENINO" : "INDEFINIDO";
            cbxGenero.ItemsSource = Generos;
            cbxGenero.SelectedItem = Generos.Where(x => x == Usuario.Genero.ToString());
        }

        private void Enable()
        {
            txtNombre.Visibility = Visibility.Collapsed;
            txtApellido.Visibility = Visibility.Collapsed;
            lbGenero.Visibility = Visibility.Collapsed;
            lbCorreo.Visibility = Visibility.Collapsed;
            btnHistorial.IsEnabled = false;

            txtNombreE.Visibility = Visibility.Visible;
            txtApellidoE.Visibility = Visibility.Visible;
            txtCorreoE.Visibility = Visibility.Visible;
            cbxGenero.Visibility = Visibility.Visible;
            btnSave.IsEnabled = true;
        }

        private bool ValidarDatos()
        {
            return !string.IsNullOrEmpty(txtNombreE.Text) && !string.IsNullOrEmpty(txtApellidoE.Text)
                && !string.IsNullOrEmpty(txtCorreoE.Text) && cbxGenero.SelectedItem != null ? true : false;
        }

        private void BtnClose_DetalleUsuario_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if(ValidarDatos())
            {
                var usuario = new UsuarioModel()
                {
                    ID = Usuario.ID,
                    Codigo = Usuario.Codigo,
                    Cedula = Usuario.Cedula,
                    Nombre = txtNombreE.Text,
                    Apellido = txtApellidoE.Text,
                    Genero = cbxGenero.SelectedItem.ToString(),
                    Correo = txtCorreoE.Text
                };

                var output = da.Edit(usuario);
                MessageBox.Show($"Usuario {Usuario.Codigo} actualizado.");
            }
            else
                MessageBox.Show($"TODOS LOS CAMPOS DEBEN ESTAR LLENOS", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);


        }

    }
}
