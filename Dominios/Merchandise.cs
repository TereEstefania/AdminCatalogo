using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains
{
    public class Merchandise
    {
        public int id { get; set; }
        public string codigo { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public Brands marca { get; set; }
        public Categories categoria { get; set; }
        public string imagenUrl { get; set; }
        public decimal precio { get; set; }
    }
}
