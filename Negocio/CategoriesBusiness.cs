using DataAccess;
using Domains;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Negocio
{
    public class CategoriesBusiness
    {
        public List<Categories> GetCategories()
        {
            List<Categories> list = new List<Categories>();
            ConnectionDataBase data = new ConnectionDataBase();

            try
            {
                data.SetQuery("SELECT Id, Descripcion from CATEGORIAS");
                data.ExecuteRead();

                while (data.Reader.Read())
                {
                    Categories aux = new Categories();
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
