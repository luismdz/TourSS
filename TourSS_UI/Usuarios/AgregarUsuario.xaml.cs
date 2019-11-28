using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TourSSLib.Models;
using TourSSLibrary;

namespace TourSS_UI.Usuarios
{
    /// <summary>
    /// Interaction logic for AgregarUsuario.xaml
    /// </summary>
    public partial class AgregarUsuario : UserControl
    {
        private static readonly Regex regex = new Regex("[^0-9]");
        private static bool IsTextAllowed(string text) => !regex.IsMatch(text);

        private DataAccess da = new DataAccess();

        public AgregarUsuario()
        {
            InitializeComponent();
            cbxRoles.ItemsSource = da.Roles;
        }

        private bool ValidarDatos()
        {
            if (!string.IsNullOrEmpty(txt_CedulaUsuario.Text) && !string.IsNullOrEmpty(txt_NombreUsuario.Text)
                && !string.IsNullOrEmpty(txt_ApellidoUsuario.Text) && txt_CelularUsuario.IsMaskCompleted
                && cbxRoles.SelectedItem != null && !string.IsNullOrEmpty(txt_CorreoUsuario.Text))
            {
                if (txt_CorreoUsuario.Text.Contains("@"))
                {
                    if (!da.TelefonosUsuarios.Any(x => x.Numero == txt_CelularUsuario.Text))
                        return true;
                    else
                        MessageBox.Show($"NUMERO DE CELULAR COINCIDE CON UNO REGISTRADO", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                return false;
            }
            else
                return false;
        }

        private void txt_TelefonoUsuario_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void txt_CelularUsuario_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void txt_CedulaUsuario_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void Btn_AgregarNuevo_Click(object sender, RoutedEventArgs e)
        {
            if (ValidarDatos())
            {
                var usuario = new UsuarioModel()
                {
                    Cedula = txt_CedulaUsuario.Text,
                    Nombre = txt_NombreUsuario.Text,
                    Apellido = txt_ApellidoUsuario.Text,
                    Genero = btnMasculino.IsChecked == true ? "M" : "F",
                    Correo = txt_CorreoUsuario.Text,
                    Rol = cbxRoles.SelectedItem as Rol,
                    Telefonos = new List<TelefonoModel>() { new TelefonoModel { Numero = txt_CelularUsuario.Text } }
                };

                if (txt_TelefonoUsuario.IsMaskCompleted)
                {
                    if(!da.TelefonosUsuarios.Any(x => x.Numero == txt_TelefonoUsuario.Text))
                        usuario.Telefonos.Add(new TelefonoModel { Numero = txt_TelefonoUsuario.Text });
                    else
                        MessageBox.Show($"NUMERO DE TELEFONO COINCIDE CON UNO REGISTRADO", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                long output = da.Insert(usuario);
                if (output > 0)
                {
                    usuario.ID = output;

                    if(!da.Credenciales.Any(x => x.UsuarioID == usuario.ID))
                    {
                        var agregarCredencial = new AgregarCredenciales(usuario);
                        agregarCredencial.ShowDialog();

                        MainWindow mw = (MainWindow)Window.GetWindow(this);
                        mw.GridPrincipal.Children.Clear();
                        mw.GridPrincipal.Children.Add(new UsuariosUC());
                        mw.ListViewMenu.IsEnabled = true;
                    }
                    else
                        MessageBox.Show($"USUARIO YA TIENE CREDENCIALES EN EL SISTEMA", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);

                }
                else
                    MessageBox.Show($"HA OCURRIDO UN ERROR", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            else
                MessageBox.Show($"TODOS LOS CAMPOS DEBEN ESTAR LLENOS", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void Btn_CancelarNuevo_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult message = MessageBox.Show("CANCELAR CREACION DE USUARIO?", "Cancelar", MessageBoxButton.YesNo);
            if (message == MessageBoxResult.Yes)
            {
                MainWindow mw = (MainWindow)Window.GetWindow(this);
                mw.GridPrincipal.Children.Clear();
                mw.GridPrincipal.Children.Add(new UsuariosUC());
                mw.ListViewMenu.IsEnabled = true;
            }
        }

   
    }
}
