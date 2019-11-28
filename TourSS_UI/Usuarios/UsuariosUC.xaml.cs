using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TourSS_UI.Usuarios;
using TourSSLibrary;

namespace TourSS_UI
{
    /// <summary>
    /// Interaction logic for UsuariosUC.xaml
    /// </summary>
    public partial class UsuariosUC : UserControl
    {
        readonly DataAccess da = new DataAccess();
        private IList<UsuarioModel> Usuarios;

        public UsuariosUC()
        {
            InitializeComponent();
            Usuarios = da.Usuarios;

            dgUsuarios.ItemsSource = Usuarios;
        }

        private void txtBuscarUsuario_TextChanged(object sender, TextChangedEventArgs e)
        {
            string txt = txtBuscarUsuario.Text;
            dgUsuarios.ItemsSource = Usuarios.Where(x => x.Fullname.ToUpper().Contains(txt)).ToList();
        }

        private void dgBtnDetalle_Click(object sender, RoutedEventArgs e)
        {
            var selected = dgUsuarios.SelectedItem as UsuarioModel;
            var detalle = new UsuarioDetalle(selected);
            detalle.ShowDialog();
        }

        private void dgBtnEditar_Click(object sender, RoutedEventArgs e)
        {
            var selected = dgUsuarios.SelectedItem as UsuarioModel;
            var detalle = new UsuarioDetalle(selected, true);
            detalle.ShowDialog();
        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = (MainWindow)Window.GetWindow(this);
            mw.GridPrincipal.Children.Clear();
            mw.GridPrincipal.Children.Add(new AgregarUsuario());
            mw.ListViewMenu.IsEnabled = false;
        }
    }
}
