using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using TourSSLib.Models;
using TourSSLibrary;

namespace TourSS_UI
{
    /// <summary>
    /// Interaction logic for AgregarClienteUC.xaml
    /// </summary>
    public partial class AgregarClienteUC : UserControl
    {
        private static readonly Regex regex = new Regex("[^0-9]");
        private static bool IsTextAllowed(string text) => !regex.IsMatch(text);

        private DataAccess da = new DataAccess();

        public AgregarClienteUC()
        {
            InitializeComponent();
        }

        private void Btn_CancelarNuevoCliente_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult message = MessageBox.Show("CANCELAR CREACION DE CLIENTE?", "Cancelar creacion de cliente", MessageBoxButton.YesNo);
            if (message == MessageBoxResult.Yes)
            {
                MainWindow mw = (MainWindow)Window.GetWindow(this);
                mw.GridPrincipal.Children.Clear();
                mw.GridPrincipal.Children.Add(new ClientesUC());
                mw.ListViewMenu.IsEnabled = true;
            }

        }

        private void txt_CedulaCliente_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private bool ValidarDatos()
        {
            if (!string.IsNullOrEmpty(txt_CedulaCliente.Text) && !string.IsNullOrEmpty(txt_NombreCliente.Text)
                && !string.IsNullOrEmpty(txt_ApellidoCliente.Text) && txt_CelularCliente.IsMaskCompleted
                && !string.IsNullOrEmpty(txt_CorreoCliente.Text))
            {
                if (txt_CorreoCliente.Text.Contains("@"))
                {
                    if (!da.TelefonosClientes.Any(x => x.Numero == txt_CelularCliente.Text))
                        return true;
                    else
                        MessageBox.Show($"NUMERO DE CELULAR COINCIDE CON UNO REGISTRADO", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                return false;
            }
            else
                return false;
        }

        private void Btn_AgregarNuevoCliente_Click(object sender, RoutedEventArgs e)
        {
            if(ValidarDatos())
            {
                var cliente = new ClienteModel()
                {
                    Cedula = txt_CedulaCliente.Text,
                    Nombre = txt_NombreCliente.Text,
                    Apellido = txt_ApellidoCliente.Text,
                    Genero = btnMasculino.IsChecked == true ? "M" : "F",
                    Correo = txt_CorreoCliente.Text,
                    Telefonos = new List<TelefonoModel>() { new TelefonoModel { Numero = txt_CelularCliente.Text } }
                };
                
                if (txt_TelefonoCliente.IsMaskCompleted)
                {
                    if(!da.TelefonosClientes.Any(x => x.Numero == txt_TelefonoCliente.Text))
                        cliente.Telefonos.Add(new TelefonoModel { Numero = txt_TelefonoCliente.Text });
                    else
                        MessageBox.Show($"NUMERO DE TELEFONO COINCIDE CON UNO REGISTRADO", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                long output = da.Insert(cliente);
                if(output > 0)
                {
                    MessageBox.Show($"NUEVO CLIENTE [TC-{output}] REGISTRADO EXITOSAMENTE!");

                    MainWindow mw = (MainWindow)Window.GetWindow(this);
                    mw.GridPrincipal.Children.Clear();
                    mw.GridPrincipal.Children.Add(new ClientesUC());
                    mw.ListViewMenu.IsEnabled = true;
                }

            }
            else
                MessageBox.Show($"TODOS LOS CAMPOS DEBEN ESTAR LLENOS", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void txt_CelularCliente_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void txt_TelefonoCliente_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }
    }
}
