using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TourSSLib.Models;
using TourSSLibrary;

namespace TourSS_UI
{
    /// <summary>
    /// Interaction logic for AgregarCredenciales.xaml
    /// </summary>
    public partial class AgregarCredenciales : Window
    {
        DataAccess da = new DataAccess();
        public UsuarioModel Usuario { get; set; }
        public List<Credencial> Credenciales { get; set; } = new List<Credencial>();
        
        private bool isAdmin = false;

        public AgregarCredenciales()
        {
            InitializeComponent();
        }

        public AgregarCredenciales(UsuarioModel u, bool solicitar = false)
        {
            InitializeComponent();
            this.Owner = App.Current.MainWindow;
            Credenciales = (List<Credencial>)da.Credenciales;

            Usuario = u;

            if (solicitar)
                ConfirmarAdmin();
        }

        public bool GetIsAdmin() => isAdmin;

        private void ConfirmarAdmin()
        {
            txtConfirmarPass.Visibility = Visibility.Collapsed;
            txtPass.Visibility = Visibility.Collapsed;
            txtPass2.Visibility = Visibility.Visible;
            btnAgregar.Visibility = Visibility.Collapsed;
            btnConfirmar.Visibility = Visibility.Visible;
            key.Visibility = Visibility.Collapsed;
        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrEmpty(txtUser.Text) && !string.IsNullOrEmpty(txtPass.Password) && !string.IsNullOrEmpty(txtConfirmarPass.Password))
            {
                if (!da.Credenciales.Any(x => x.Username == txtUser.Text))
                {
                    if (txtPass.Password == txtConfirmarPass.Password)
                    {
                        var credencial = new Credencial()
                        {
                            Username = txtUser.Text,
                            Password = txtConfirmarPass.Password.Trim(),
                            UsuarioID = Usuario.ID
                        };

                        var output = da.Insert(credencial);
                        if (output > 0)
                        {
                            MessageBox.Show($"NUEVO USUARIO REGISTRADO EXITOSAMENTE!");
                            this.Close();
                        }
                    }
                    else
                        MessageBox.Show("CONTRASEÑAS NO COINCIDEN", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                    MessageBox.Show("NOMBRE DE USUARIO YA REGISTRADO", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
                MessageBox.Show("CAMPOS NO PUEDEN ESTAR EN BLANCO", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("SEGURO QUE DESEA CANCELAR EL REGISTRO?", "CANCELAR", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }

        private void btnConfirmar_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtUser.Text) && !string.IsNullOrEmpty(txtPass2.Password))
            {
                if (Credenciales.Any(x => x.UsuarioID == Usuario.ID))
                {
                    var credencial = Credenciales.Where(x => x.UsuarioID == Usuario.ID).SingleOrDefault();
                    var usuario = da.Usuarios.Where(x => x.ID == credencial.UsuarioID).SingleOrDefault();

                    if (credencial.Username == txtUser.Text && credencial.Password.Trim() == txtPass2.Password && usuario.isAdmin == true)
                    {
                        isAdmin = true;
                        this.Close();
                    }
                    else
                        MessageBox.Show("USUARIO INGRESADO NO TIENE PERMISOS PARA REALIZAR ESTA ACCION", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                    MessageBox.Show("ERROR AL VERIFICAR USUARIO", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
                MessageBox.Show("CAMPOS NO PUEDEN ESTAR EN BLANCO", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void StackPanel_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
                this.DragMove();
        }

        private void txtPass2_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(e.Key == System.Windows.Input.Key.Enter)
            {
                if (!string.IsNullOrEmpty(txtUser.Text) && !string.IsNullOrEmpty(txtPass2.Password))
                {
                    if (Credenciales.Any(x => x.UsuarioID == Usuario.ID))
                    {
                        var credencial = Credenciales.Where(x => x.UsuarioID == Usuario.ID).SingleOrDefault();
                        var usuario = da.Usuarios.Where(x => x.ID == credencial.UsuarioID).SingleOrDefault();

                        if (credencial.Username == txtUser.Text && credencial.Password.Trim() == txtPass2.Password && usuario.isAdmin == true)
                        {
                            isAdmin = true;
                            this.Close();
                        }
                        else
                            MessageBox.Show("USUARIO INGRESADO NO TIENE PERMISOS PARA REALIZAR ESTA ACCION", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                        MessageBox.Show("ERROR AL VERIFICAR USUARIO", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                    MessageBox.Show("CAMPOS NO PUEDEN ESTAR EN BLANCO", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
