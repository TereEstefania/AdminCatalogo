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
    public partial class FormCatalog : Form
    {
        private List<Merchandise> listMerchandise;

        public FormCatalog()
        {
            InitializeComponent();
        }

        //Carga el formulario
        private void FormCatalogo_Load(object sender, EventArgs e)
        {
            UpLoad();
        }

        //Carga el datagridview con los artículos
        private void UpLoad()
        {
            MerchandiseBusiness serve = new MerchandiseBusiness();
 
            try
            {
                listMerchandise = serve.GetMerchandises();
                
                DgvCatalog.DataSource = listMerchandise;

                UpLoadImage(listMerchandise[0].imagenUrl);

                VisibleColum("imagenUrl");
                VisibleColum("id");

                UpLoadComboBox();
                txtNameFilter.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        // Muestra la imagen del artículo al ser seleccionado
        private void DgvCatalog_SelectionChanged(object sender, EventArgs e)
        {
            if(DgvCatalog.CurrentRow != null)
            {
                Merchandise selected =(Merchandise)DgvCatalog.CurrentRow.DataBoundItem;
                UpLoadImage(selected.imagenUrl);
            }

        }

        //Carga los combobox de categorías y marcas
        private void UpLoadComboBox()
        {
            
            CategoriesBusiness serveCategories = new CategoriesBusiness();
            BrandsBusiness serveBrands = new BrandsBusiness();

            var categories = serveCategories.GetCategories();
            categories.Insert(0, new Categories { id = 0, descripcion = "--Seleccione--" });
            cboCategories.DataSource = categories;
            cboCategories.ValueMember = "id";
            cboCategories.DisplayMember = "descripcion";
            cboCategories.SelectedIndex = 0;


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
        
        //Oculta las columnas no deseadas
        private void VisibleColum(string column)
        {
           DgvCatalog.Columns[column].Visible = false;
        }
        
        //Botones que abre los formularios de crear un artículo
        private void btnCreate_Click(object sender, EventArgs e)
        {
            FmrMerchandise create = new FmrMerchandise();
            create.ShowDialog();
            UpLoad();
        }
        
        //Botón que abre el formulario de modificar un artículo
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Merchandise selected;
            selected = (Merchandise)DgvCatalog.CurrentRow.DataBoundItem;

            FmrMerchandise discharge = new FmrMerchandise(selected);
            discharge.ShowDialog();
            UpLoad();
        }
        
        //Botón que elimina un artículo
        private void btnDelete_Click(object sender, EventArgs e)
        {
                delete();
        }
        
        //Función que elimina un artículo
        private void delete()
        {
            MerchandiseBusiness service = new MerchandiseBusiness();
            Merchandise selected;
            try
            {
                DialogResult result = MessageBox.Show("¿Deseas eliminar este artículo?", "Eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    selected = (Merchandise)DgvCatalog.CurrentRow.DataBoundItem;

                    service.Delete(selected.id);

                    MessageBox.Show("Artículo eliminado");

                    UpLoad();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        //Abre el formulario de ver artículo al hacer doble click en el articulo seleccionado en el datagridview
        private void DgvCatalog_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Merchandise selected;
            selected = (Merchandise)DgvCatalog.CurrentRow.DataBoundItem;
            FrmView discharge = new FrmView(selected);
            discharge.ShowDialog();
            
            UpLoad();
        }

        //Botón que filtra los artículos según los parámetros seleccionados
        private void btnSearch_Click(object sender, EventArgs e)
        {  
            MerchandiseBusiness serve = new MerchandiseBusiness();

            try
            {
                // Filtro por todo
                if ((cboCategories.SelectedIndex > 0) && (cboBrands.SelectedIndex > 0) && !(string.IsNullOrWhiteSpace(txtNameFilter.Text)))
                {
                    DgvCatalog.DataSource = serve.FilterAll(txtNameFilter.Text, cboBrands.Text,cboCategories.Text);
                    cboCategories.SelectedIndex = 0;
                    cboBrands.SelectedIndex = 0;
                    txtNameFilter.Clear();
                    return;
                }

                // Filtro por categoría
                if (cboCategories.SelectedValue != null && cboCategories.SelectedIndex > 0)
                {
                    DgvCatalog.DataSource = serve.FilterCategories(cboCategories.Text);
                    cboCategories.SelectedIndex = 0;
                    return;
                }

                // Filtro por marca
                if (cboBrands.SelectedValue != null && cboBrands.SelectedIndex > 0)
                {
                    DgvCatalog.DataSource = serve.FilterBrands(cboBrands.Text);
                    cboBrands.SelectedIndex = 0;
                    return;
                }

                // Filtro por nombre
                if (!string.IsNullOrWhiteSpace(txtNameFilter.Text))
                {
                    DgvCatalog.DataSource = serve.FilterByName(txtNameFilter.Text.Trim());
                    txtNameFilter.Clear();
                    return;
                }

                //Filtra por codigo de artículo


                MessageBox.Show("Seleccione una categoría, una marca o ingrese un nombre para filtrar.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
       
        }

        //Botón que recarga el datagridview
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            UpLoad();
        }


    }
}   
