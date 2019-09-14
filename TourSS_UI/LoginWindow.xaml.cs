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

namespace TourSS_UI
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private DataAccess da = new DataAccess();

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void BtnInicioSesion_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            UsuarioModel usuario = new UsuarioModel();
            bool esValido;
 
            (usuario, esValido) = da.GetUserByLogin(username, password);

            if (esValido && usuario != null)
            {
                var mw = new MainWindow(usuario);
                App.Current.MainWindow = mw;
                Close();
                mw.Show();
            }
            else
            {
                MessageBox.Show("Usuario o Contraseña incorrecta", "USUARIO NO VALIDO", MessageBoxButton.OK);
                txtUsername.Text = "";
                txtPassword.Password = "";
            }
        }


        private void TxtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string username = txtUsername.Text;
                string password = txtPassword.Password;

                UsuarioModel usuario = new UsuarioModel();
                bool esValido;

                (usuario, esValido) = da.GetUserByLogin(username, password);

                if (esValido && usuario != null)
                {
                    var mw = new MainWindow(usuario);
                    App.Current.MainWindow = mw;
                    Close();
                    mw.Show();
                }
                else
                {
                    MessageBox.Show("Usuario o Contraseña incorrecta", "USUARIO NO VALIDO", MessageBoxButton.OK);
                    txtUsername.Text = "";
                    txtPassword.Password = "";
                }

            }
        }

        //private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    DragMove();
        //}

        //private void BtnClose_Click(object sender, RoutedEventArgs e)
        //{
        //    Application.Current.Shutdown();
        //}
    }
}
