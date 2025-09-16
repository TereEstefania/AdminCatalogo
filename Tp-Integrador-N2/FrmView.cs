using Domains;
using Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using System.Xml.Linq;


namespace Tp_Integrador_N2
{
    public partial class FrmView : Form
    {
        private Merchandise merchandise = null;

        public FrmView()
        {
            InitializeComponent();
        }
        
        public FrmView(Merchandise merchandise)
        {
            InitializeComponent();
            this.merchandise = merchandise;
        }

        private void FrmView_Load(object sender, EventArgs e)
        {
            CategoriesBusiness serveCategories = new CategoriesBusiness();
            BrandsBusiness serveBrands = new BrandsBusiness();
            try
            {

                if (merchandise != null)
                {
                    
                    lblCodeView.Text = merchandise.codigo.ToString();
                    lblNameView.Text = merchandise.nombre.ToString();
                    txtDescriptionView.Text = merchandise.descripcion.ToString();
                    txtImageUrlView.Text = merchandise.imagenUrl;
                    UpLoadImage(merchandise.imagenUrl);
                    lblBrandView.Text = merchandise.marca.ToString();
                    lblCategoriesView.Text = merchandise.categoria.ToString();
                    lblPriceView.Text = merchandise.precio.ToString("0.00");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void UpLoadImage(string image)
        {
            try
            {
                pbxImageView.Load(image);
            }
            catch (Exception)
            {
                pbxImageView.Load("https://winguweb.org/wp-content/uploads/2022/09/placeholder.png");
            }
        }

        private void btnBackView_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
