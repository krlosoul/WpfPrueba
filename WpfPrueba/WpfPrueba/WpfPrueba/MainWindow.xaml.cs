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
using WpfPrueba.Modelo;

namespace WpfPrueba
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window{
        public MainWindow(){
            InitializeComponent();
        }

        List<Clientes> Cliente = null;
        List<Productos> Productos = null;
        List<Facturas> Factura = new List<Facturas>();
        
        private void Content_Loaded(object sender, RoutedEventArgs e)
        {
            txtFecha.Text = DateTime.Now.Day+"-"+DateTime.Now.Month+"-"+DateTime.Now.Year;
            
            txtNombre.IsEnabled = false;
            txtDireccion.IsEnabled = false;
            txtTelefono.IsEnabled = false;
            txtFecha.IsEnabled = false;

            GetClientes();
            GetProductos();
        }

        private void GetClientes() {
            Cliente = new Clientes().GetDatos();

            cbxCedula.Items.Clear();
            cbxCedula.ItemsSource = Cliente;
            cbxCedula.DisplayMemberPath = "Cedula";
        }
        private void GetInfoCliente() {
            Clientes _cliente = (Clientes)cbxCedula.SelectedItem;
            txtNombre.Text = _cliente.Nombre + " " + _cliente.Apellido;
            txtDireccion.Text = _cliente.Direccion;
            txtTelefono.Text = _cliente.Telefono;            
        }
        private void GetProductos(){
            Productos = new Productos().GetDatos();

            cbxProducto.ItemsSource = null;
            cbxProducto.Items.Clear();
            cbxProducto.ItemsSource = Productos;
            cbxProducto.DisplayMemberPath = "Codigo";
        }
        private void GetCantProducto() {
            Productos _poducto = (Productos)cbxProducto.SelectedItem;
            if (_poducto.Cantidad <= 0){
                MessageBox.Show("Producto agotado.");
                txtCantidad.IsEnabled = false;
            }
            else {
                txtCantidad.IsEnabled = true;
            }
        }
        
        private void Limpiar() {
            cbxCedula.Text = String.Empty;
            cbxCedula.SelectedItem = -1;

            txtNombre.Text = String.Empty;
            txtDireccion.Text = String.Empty;
            txtTelefono.Text = String.Empty;

            cbxProducto.Text = String.Empty;
            cbxProducto.SelectedItem = -1;

            txtCantidad.Text = String.Empty;

            Factura.Clear();

            dgvDetalle.ItemsSource = null;
            dgvDetalle.Items.Clear();
        }

        private void cbxCedula_SelectionChanged(object sender, SelectionChangedEventArgs e){
            if (cbxCedula.SelectedIndex>=0){
                GetInfoCliente();
            }
        }
        private void cbxProducto_SelectionChanged(object sender, SelectionChangedEventArgs e){
            if(cbxProducto.SelectedIndex>=0){
                GetCantProducto();
            }
        }

        private bool validarAgregar()
        {
            if (cbxProducto.Text == "Seleccione un producto")
            {
                MessageBox.Show("Debe ingresar un producto.");
                return false;
            }
            if (string.IsNullOrEmpty(txtCantidad.Text))
            {
                MessageBox.Show("Debe digitar la cantidad.");
                return false;
            }
            return true;
        }
        private bool validarGuardar()
        {
            if (cbxCedula.Text == "Seleccione una cedula")
            {
                MessageBox.Show("Debe ingresar la cedula.");
                return false;
            }
            if (!validarAgregar())
            {
                return false;
            }
            return true;
        }

        private void Agregar_Click(object sender, RoutedEventArgs e)
        {
            if (validarAgregar() == true)
            {
                Facturas _factura = new Facturas();
                _factura.Codigo = ((Productos)cbxProducto.SelectedItem).Codigo;
                _factura.Nombre = ((Productos)cbxProducto.SelectedItem).Nombre;
                _factura.Cantidad = Convert.ToInt32(txtCantidad.Text);
                _factura.Unidad = ((Productos)cbxProducto.SelectedItem).Valor;
                _factura.Total = _factura.Cantidad * _factura.Unidad;

                Productos _producto = (Productos)cbxProducto.SelectedItem;

                if (_producto.Cantidad - _factura.Cantidad >= 0){

                    var IsExist = (from c in Factura where c.Codigo == _factura.Codigo select c).Any();

                    if (!IsExist){
                        Factura.Add(_factura);
                        dgvDetalle.ItemsSource = null;
                        dgvDetalle.ItemsSource = Factura;
                    }
                    else {
                        MessageBox.Show("Este producto ya fue agregado.");
                    }
                } else {
                    MessageBox.Show("La cantidad Excede la del inventario.");
                    txtCantidad.Focus();
                }
            } 
        }
        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            if (validarGuardar() == true) { 

                int IdCabecera = new Facturas().GetIdCabecera(((Clientes)cbxCedula.SelectedItem).Cedula);
                
                foreach (var item in dgvDetalle.Items){
                    DataGridRow row = (DataGridRow)dgvDetalle.ItemContainerGenerator.ContainerFromItem(item);

                    new Facturas().SetDetalle(IdCabecera,
                        ((TextBlock)dgvDetalle.Columns[0].GetCellContent(row)).Text,
                        Convert.ToInt32(((TextBlock)dgvDetalle.Columns[2].GetCellContent(row)).Text),
                        Convert.ToInt32(((TextBlock)dgvDetalle.Columns[3].GetCellContent(row)).Text),
                        Convert.ToInt32(((TextBlock)dgvDetalle.Columns[4].GetCellContent(row)).Text));
                }
                Limpiar();
                GetProductos();
                MessageBox.Show("Datos Guardados");

            }
        }

    }
}
