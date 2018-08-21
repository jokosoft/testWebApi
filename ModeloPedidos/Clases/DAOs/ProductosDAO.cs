using ModeloPedidos.Clases.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModeloPedidos.Clases.DAOs
{
    public class ProductosDAO
    {
        public ProductosDAO()
        {

        }

        public List<Productos> Get()
        {
            List<Productos> lista = new List<Productos>();

            try
            {
                using (var db = new PruebasEntities())
                {
                    lista = db.Productos
                        .Include("Familia")
                        .ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return lista;
        }

        /// <summary>
        /// Obtiene la lista de productos filtrada por un témino de búsqueda
        /// ordenada por un campo en el sentido indicado
        /// y paginada según los datos pasados
        /// </summary>
        /// <param name="current"></param>
        /// <param name="rowCount"></param>
        /// <param name="searchPhrase"></param>
        /// <param name="campoOrdenar"></param>
        /// <param name="orden"></param>
        /// <returns></returns>
        public List<ProductoDTO> GetListaFiltrada(int paginaActual, int registrosPagina, string termminoBusqueda,
                                                string campoOrdenar, string orden, out int iTotalRegistros)
        {
            // nota: paginacion linq :: coleccion.Skip(pagAMostrar * numElementosPorPagina).Take(numElementosPorPagina);
            List<ProductoDTO> lista = new List<ProductoDTO>();

            try
            {
                using (var db = new PruebasEntities())
                {
                    var listaProductos = from prod in db.Productos
                                         join fam in db.Familia
                                           on prod.FidFamilia equals fam.Id
                                         select new ProductoDTO
                                         {
                                             Id_prod = prod.Id_prod,
                                             Nombre_prod = prod.Nombre_prod,
                                             Precio = prod.Precio,
                                             FidFamilia = prod.FidFamilia,
                                             NombreFamilia = fam.Nombre
                                         };

                    // establece el filtrado de datos
                    if(!string.IsNullOrEmpty(termminoBusqueda))
                    {
                        listaProductos = listaProductos.Where(x => x.Id_prod.ToString().Contains(termminoBusqueda) || 
                                                                    x.Nombre_prod.Contains(termminoBusqueda) ||
                                                                    x.NombreFamilia.Contains(termminoBusqueda) ||
                                                                    x.Precio.ToString().Contains(termminoBusqueda));
                    }

                    // obtiene el total de registros antes de paginar
                    iTotalRegistros = (listaProductos != null) ? listaProductos.Count() : 0;

                    // se establece la ordenación
                    listaProductos = establecerOrdenacion(campoOrdenar, orden, listaProductos);

                    // se controla si es búsqueda total o paginada
                    if (registrosPagina > 0)
                        listaProductos = listaProductos.Skip((paginaActual - 1) * registrosPagina).Take(registrosPagina);

                    lista = listaProductos.ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return lista;
        }

        /// <summary>
        /// establece la ordenacion en funcion de los datos pasados
        /// </summary>
        /// <param name="campoOrdenar"></param>
        /// <param name="orden"></param>
        /// <param name="listaProductos"></param>
        /// <returns></returns>
        private static IQueryable<ProductoDTO> establecerOrdenacion(string campoOrdenar, string orden, IQueryable<ProductoDTO> listaProductos)
        {
            if (!string.IsNullOrEmpty(campoOrdenar))
            {
                if (orden.ToLower().Equals("asc"))
                {
                    if (campoOrdenar.Equals("Nombre_prod"))
                        listaProductos = listaProductos.OrderBy(x => x.Nombre_prod);
                    else if (campoOrdenar.Equals("NombreFamilia"))
                        listaProductos = listaProductos.OrderBy(x => x.NombreFamilia);
                    else if (campoOrdenar.Equals("Precio"))
                        listaProductos = listaProductos.OrderBy(x => x.Precio);
                    else
                        listaProductos = listaProductos.OrderBy(s => s.Id_prod);
                }
                else
                {
                    if (campoOrdenar.Equals("Nombre_prod"))
                        listaProductos = listaProductos.OrderByDescending(x => x.Nombre_prod);
                    else if (campoOrdenar.Equals("NombreFamilia"))
                        listaProductos = listaProductos.OrderByDescending(x => x.NombreFamilia);
                    else if (campoOrdenar.Equals("Precio"))
                        listaProductos = listaProductos.OrderByDescending(x => x.Precio);
                    else
                        listaProductos = listaProductos.OrderByDescending(s => s.Id_prod);
                }
            }
            else
                listaProductos = listaProductos.OrderBy(s => s.Id_prod);

            return listaProductos;
        }
    }
}
