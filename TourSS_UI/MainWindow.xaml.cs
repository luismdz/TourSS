﻿using System;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public UsuarioModel Current { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            GridPrincipal.Children.Clear();
            GridPrincipal.Children.Add(new InicioUC());
        }

        public MainWindow(UsuarioModel u)
        {
            InitializeComponent();
            GridPrincipal.Children.Clear();
            GridPrincipal.Children.Add(new InicioUC());

            Current = u;
            lbUserName.Content = $"[{Current.Codigo}] {Current.Fullname}";
            lbUserRol.Content = Current.Rol;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }

        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = ListViewMenu.SelectedIndex;
            MoveCursorMenu(index);
          
            switch(index)
            {
                case 0:
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new InicioUC());
                    break;
                case 1:
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new ClientesUC());
                    break;
                case 2:
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new ReservacionUC());
                    break;
                case 3:
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new CatalogoUC());
                    break;
                case 4:
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new UsuariosUC());
                    break;
            }
        }

        private void MoveCursorMenu(int index)
        {
            TransitioningContentSlide.OnApplyTemplate();
            GridCursor.Margin = new Thickness(0, (100 + (60 * index)), 0, 0);
        }

        private void BtnCerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Seguro que desea cerrar la sesion?", "Cerrar Sesion", MessageBoxButton.YesNo);
           
            if (result == MessageBoxResult.Yes)
            {
                LoginWindow lw = new LoginWindow();
                App.Current.MainWindow = lw;
                Close();
                lw.Show();
            }
        }
    }
}
