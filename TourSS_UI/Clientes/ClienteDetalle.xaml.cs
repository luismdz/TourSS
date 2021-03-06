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
        public List<string> Generos { get; set; }

        private DataAccess da = new DataAccess();

        public ClienteDetalle()
        {
            InitializeComponent();
            this.Owner = App.Current.MainWindow;
        }

        public ClienteDetalle(ClienteModel cliente, bool asEdit = false)
        {
            InitializeComponent();
            this.Owner = App.Current.MainWindow;
            
            this.Cliente = cliente;
            Generos = da.Query<string>("select distinct clienteGenero from Clientes");

            Fill();
            
            if (asEdit)
                Enable();
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

            if(Cliente.Telefonos != null)
                lbCelular.Content = Cliente.Telefonos[0].Numero;

            if (Cliente.Telefonos.Count > 1)
                lbTelResidencia.Content = Cliente.Telefonos[1].Numero;
            else
                lbTelResidencia.Content = "";

            lbGenero.Content = Cliente.Genero == "M" ? "MASCULINO" : Cliente.Genero == "F" ? "FEMENINO" : "INDEFINIDO";
            cbxGenero.ItemsSource = Generos;
            cbxGenero.SelectedItem = Generos.Where(x => x == Cliente.Genero.ToString());
        }

        private void Enable()
        {
            txtNombre.Visibility = Visibility.Collapsed;
            txtApellido.Visibility = Visibility.Collapsed;
            lbGenero.Visibility = Visibility.Collapsed;
            lbCorreo.Visibility = Visibility.Collapsed;
            btnHistorial.IsEnabled = false;

            txtNombreE.Visibility = Visibility.Visible;
            txtNombreE.Text = txtNombre.Content.ToString();

            txtApellidoE.Visibility = Visibility.Visible;
            txtApellidoE.Text = txtApellido.Content.ToString();

            txtCorreoE.Visibility = Visibility.Visible;
            txtCorreoE.Text = lbCorreo.Content.ToString();

            cbxGenero.Visibility = Visibility.Visible;

            btnSave.IsEnabled = true;
        }

        private void BtnClose_DetalleCliente_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private bool ValidarDatos()
        {
            return !string.IsNullOrEmpty(txtNombreE.Text) && !string.IsNullOrEmpty(txtApellidoE.Text)
                && !string.IsNullOrEmpty(txtCorreoE.Text) && cbxGenero.SelectedItem != null ? true : false;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if(ValidarDatos())
            {
                var cliente = new ClienteModel()
                {
                    ID = Cliente.ID,
                    Codigo = Cliente.Codigo,
                    Cedula = Cliente.Cedula,
                    Nombre = txtNombreE.Text,
                    Apellido = txtApellidoE.Text,
                    Genero = cbxGenero.SelectedItem.ToString(),
                    Correo = txtCorreoE.Text
                };

                var output = da.Edit(cliente);
                MessageBox.Show($"Cliente {Cliente.Codigo} actualizado.");
            }
            else
                MessageBox.Show($"TODOS LOS CAMPOS DEBEN ESTAR LLENOS", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
