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
using TourSSLibrary;

namespace TourSS_UI
{
    /// <summary>
    /// Interaction logic for UsuariosUC.xaml
    /// </summary>
    public partial class UsuariosUC : UserControl
    {
        DataAccess da = new DataAccess();
        public UsuariosUC()
        {
            InitializeComponent();
            dgUsuarios.ItemsSource = da.GetAllUsuarios();
        }
    }
}
