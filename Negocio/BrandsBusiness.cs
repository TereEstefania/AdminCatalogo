using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using Domains;

namespace Negocio
{
    public class BrandsBusiness
    {
        public List<Brands> GetBrands()
        {
            List<Brands> list = new List<Brands>();
            ConnectionDataBase data = new ConnectionDataBase();

            try
            {
                data.SetQuery("SELECT Id, Descripcion from MARCAS");
                data.ExecuteRead();

                while (data.Reader.Read())
                {
                    Brands aux = new Brands();
                    aux.id = (int)data.Reader["Id"];
                    aux.descripcion = (string)data.Reader["Descripcion"];

                    list.Add(aux);
                }

                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                data.CloseConnection();
            }

        }
    }
}
