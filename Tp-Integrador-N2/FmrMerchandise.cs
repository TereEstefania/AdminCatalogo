using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Business;
using Domains;
using Negocio;


namespace Tp_Integrador_N2
{
    public partial class FmrMerchandise : Form
    {
        private Merchandise merchandise = null;
        
        public FmrMerchandise()
        {
            InitializeComponent();
            
        }
        
        //Sobrecarga del constructor para modificar un artículo
        public FmrMerchandise(Merchandise merchandise)
        {
            InitializeComponent();
            this.merchandise = merchandise;
            Text = "Modificar Articulo";
            lblCreate.Text = "MODIFICAR";
        }
        
        //Carga del formulario
        private void FmrMerchandise_Load(object sender, EventArgs e)
        {
            try
            {
                UpLoadComboBox();

                if (merchandise != null)
                {
                    txtCode.Text = merchandise.codigo.ToString();
                    txtDescription.Text = merchandise.descripcion.ToString();
                    txtName.Text = merchandise.nombre.ToString();
                    txtImage.Text = merchandise.imagenUrl;
                    UpLoadImage(merchandise.imagenUrl);
                    cboBrands.SelectedValue = merchandise.marca.id;
                    cboCategory.SelectedValue = merchandise.categoria.id;
                    txtPrice.Text = merchandise.precio.ToString("0.00");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
    
        }

        //Carga los combobox categorias y marcas
        private void UpLoadComboBox()
        {

            CategoriesBusiness serveCategories = new CategoriesBusiness();
            BrandsBusiness serveBrands = new BrandsBusiness();

            var categories = serveCategories.GetCategories();
            categories.Insert(0, new Categories { id = 0, descripcion = "--Seleccione--" });
            cboCategory.DataSource = categories;
            cboCategory.ValueMember = "id";
            cboCategory.DisplayMember = "descripcion";
            cboCategory.SelectedIndex = 0;


            var brands = serveBrands.GetBrands();
            brands.Insert(0, new Brands { id = 0, descripcion = "--Seleccione--" });
            cboBrands.DataSource = brands;
            cboBrands.ValueMember = "id";
            cboBrands.DisplayMember = "descripcion";
            cboBrands.SelectedIndex = 0;

        }

        //Carga la imagen en el picturebox
        private void UpLoadImage(string image)
        {
            try
            {
                pbxImage.Load(image);
            }
            catch
            {
                pbxImage.Image = Properties.Resources.Placeholder_view_vector_svg;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //boton para cargar la imagen desde un archivo
        }

        //Boton aceptar, carga la lista desde los datos ingresados por el ususario ene le formulario y luego llama a la función create o update según corresponda
        private void btnAccept_Click(object sender, EventArgs e)
        {
            MerchandiseBusiness business = new MerchandiseBusiness();

            try
            {
                if (merchandise == null)
                    merchandise = new Merchandise();

                merchandise.codigo = txtCode.Text;
                merchandise.nombre = txtName.Text;
                merchandise.descripcion = txtDescription.Text;
                merchandise.imagenUrl = txtImage.Text;
                merchandise.marca = (Brands)cboBrands.SelectedItem;
                merchandise.categoria = (Categories)cboCategory.SelectedItem;
                merchandise.precio = decimal.Parse(txtPrice.Text);

                if (merchandise.id != 0)
                {
                    //Update() modifica el artículo
                    business.Update(merchandise);
                    MessageBox.Show("Articulo Modificado");
                }
                 else
                 {
                    //Create() agrega un nuevo artículo
                    business.Create(merchandise);
                    MessageBox.Show("Agregado exitosamente");
                    
                 }

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        //Boton cancelar, cierra el formulario
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }


    }
}
