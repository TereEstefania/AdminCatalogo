using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domains;
using DataAccess;
using System.Collections;
using System.Xml.Linq;
using System.Data.SqlTypes;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.CompilerServices;


namespace Business
{
    public class MerchandiseBusiness
    {
        //Listar artículos, Retornar una lista de artículos
        public List<Merchandise> GetMerchandises()
        {
            try
            {

              return  RegistryMerchandise("select A.Id, A.Codigo, A.Nombre, A.Descripcion,M.Descripcion as Marca, C.Descripcion as Categoria, A.ImagenUrl, A.Precio, A.IdMarca, A.IdCategoria from ARTICULOS A, CATEGORIAS C, MARCAS M \r\n  where A.IdMarca = M.Id and A.IdCategoria = C.Id;");

            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        //Crear un artículo, Recibe un nuevo objeto artículo
        public void Create(Merchandise NewMerchandise)
        {
            ConnectionDataBase data = new ConnectionDataBase();

            try
            {
                data.SetQuery("insert into ARTICULOS(Codigo,Nombre,Descripcion,IdMarca,IdCategoria,ImagenUrl,Precio)values(@Codigo, @Nombre, @Descripcion, @IdMarca, @IdCategoria, @ImagenUrl, @Precio)");
                data.SetParams("@Codigo", NewMerchandise.codigo);
                data.SetParams("@Nombre", NewMerchandise.nombre);
                data.SetParams("@Descripcion", NewMerchandise.descripcion);
                data.SetParams("@IdMarca", NewMerchandise.marca.id);
                data.SetParams("@IdCategoria", NewMerchandise.categoria.id);
                data.SetParams("@ImagenUrl", NewMerchandise.imagenUrl);
                data.SetParams("@Precio", NewMerchandise.precio);

                data.ExecuteRead();

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

        //Modificar un artículo, Recibe un objeto artículo ya existente
        public void Update(Merchandise merchandise)
        {
            ConnectionDataBase data = new ConnectionDataBase();

            try
            {
                data.SetQuery("update ARTICULOS set Codigo = @Codigo, Nombre = @Nombre, Descripcion = @Descripcion, IdMarca = @IdMarca, IdCategoria = @IdCategoria, ImagenUrl = @ImagenUrl, Precio = @Precio where Id = @Id");
                data.SetParams("@Codigo", merchandise.codigo);
                data.SetParams("@Nombre", merchandise.nombre);
                data.SetParams("@Descripcion", merchandise.descripcion);
                data.SetParams("@IdMarca", merchandise.marca.id);
                data.SetParams("@IdCategoria", merchandise.categoria.id);
                data.SetParams("@ImagenUrl", merchandise.imagenUrl);
                data.SetParams("@Precio", merchandise.precio);
                data.SetParams("@Id", merchandise.id);

                data.ExecuteRead();

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

        //Eliminar un artículo, Recibe el id del artículo a eliminar
        public void Delete(int id)
        { 
            try
            {
                ConnectionDataBase data = new ConnectionDataBase();
                data.SetQuery("delete from ARTICULOS where Id = @id");
                data.SetParams("@id", id);
                data.ExecuteRead();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Filtra por Categoria
        public List<Merchandise> FilterCategories(string category)
        {
             string auxQuery = "select A.Id, A.Codigo, A.Nombre, A.Descripcion,M.Descripcion as Marca, C.Descripcion as Categoria, A.ImagenUrl, A.Precio, A.IdMarca, A.IdCategoria from ARTICULOS A, CATEGORIAS C, MARCAS M where A.IdMarca = M.Id and A.IdCategoria = C.Id and ";
             auxQuery += "C.Descripcion = '" + category + "' ";

             return RegistryMerchandise(auxQuery);

        }

        //Filtra por Marca
        public List<Merchandise> FilterBrands(string brand)
        {
            string auxQuery = "select A.Id, A.Codigo, A.Nombre, A.Descripcion,M.Descripcion as Marca, C.Descripcion as Categoria, A.ImagenUrl, A.Precio, A.IdMarca, A.IdCategoria from ARTICULOS A, CATEGORIAS C, MARCAS M where A.IdMarca = M.Id and A.IdCategoria = C.Id and ";
            auxQuery += "M.Descripcion = '" + brand + "' ";

              return RegistryMerchandise(auxQuery);

        }
        
        //Filtra por Nombre
        public List<Merchandise> FilterByName(string text)
        {
            string auxQuery = "select A.Id, A.Codigo, A.Nombre, A.Descripcion,M.Descripcion as Marca, C.Descripcion as Categoria, A.ImagenUrl, A.Precio, A.IdMarca, A.IdCategoria from ARTICULOS A, CATEGORIAS C, MARCAS M where A.IdMarca = M.Id and A.IdCategoria = C.Id and ";
            auxQuery += "A.Nombre LIKE '%" + text + "%' ";

            return RegistryMerchandise(auxQuery);

        }

        //Filtra por todo
        public List<Merchandise> FilterAll(string text, string brand, string category)
        {

            string auxQuery = "select A.Id, A.Codigo, A.Nombre, A.Descripcion,M.Descripcion as Marca, C.Descripcion as Categoria, A.ImagenUrl, A.Precio, A.IdMarca, A.IdCategoria from ARTICULOS A, CATEGORIAS C, MARCAS M where A.IdMarca = M.Id and A.IdCategoria = C.Id and ";
            auxQuery += "M.Descripcion = '" + brand + "' and C.Descripcion = '" + category + "' and A.Nombre LIKE '%" + text + "%' ";

            return RegistryMerchandise(auxQuery);

        }

        //Método privado que recibe un query y retorna una lista de artículos
        private List<Merchandise> RegistryMerchandise(string query)
        {
            List<Merchandise> list = new List<Merchandise>();
            ConnectionDataBase data = new ConnectionDataBase();
            try
            {
                data.SetQuery(query);
                data.ExecuteRead();

                while (data.Reader.Read())
                {
                    Merchandise aux = new Merchandise();
                    aux.id = (int)data.Reader["Id"];
                    aux.codigo = (string)data.Reader["Codigo"];
                    aux.nombre = (string)data.Reader["Nombre"];
                    aux.descripcion = (string)data.Reader["Descripcion"];

                    if (!(data.Reader["ImagenUrl"] is DBNull))
                        aux.imagenUrl = (string)data.Reader["ImagenUrl"];

                    aux.marca = new Brands();
                    aux.marca.id = (int)data.Reader["IdMarca"];
                    aux.marca.descripcion = (string)data.Reader["Marca"];
                    aux.categoria = new Categories();
                    aux.categoria.id = (int)data.Reader["IdCategoria"];
                    aux.categoria.descripcion = (string)data.Reader["Categoria"];
                    aux.precio = (decimal)data.Reader["Precio"];

                    list.Add(aux);

                }
                return list;
            }
            catch
            {
                throw;
            }
            finally
            {
                data.CloseConnection();
            }

        }
    }
}
