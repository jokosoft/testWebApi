using ModeloPedidos.Clases.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModeloPedidos.Clases.DAOs
{
    public class RestaurantesDAO
    {
        public RestaurantesDAO()
        {

        }

        public List<Restaurantes> Get()
        {
            List<Restaurantes> lista = new List<Restaurantes>();

            try
            {
                using (var db = new PruebasEntities())
                {
                    lista = db.Restaurantes
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
        /// Obtiene la lista de Restaurantes filtrada por un témino de búsqueda
        /// ordenada por un campo en el sentido indicado
        /// y paginada según los datos pasados
        /// </summary>
        /// <param name="current">Pagina que se quiere mostrar</param>
        /// <param name="rowCount">filas que se quieren mostrar en cada página</param>
        /// <param name="searchPhrase">Término de búsqueda</param>
        /// <param name="campoOrdenar">Nombre del campo por qu e quieren ordenar los resultados</param>
        /// <param name="orden">sentido de la ordenación</param>
        /// <returns></returns>
        public List<RestauranteDTO> GetListaFiltrada(int paginaActual, int registrosPagina, string termminoBusqueda,
                                                string campoOrdenar, string orden, out int iTotalRegistros)
        {
            // nota: paginacion linq :: coleccion.Skip(pagAMostrar * numElementosPorPagina).Take(numElementosPorPagina);
            List<RestauranteDTO> lista = new List<RestauranteDTO>();

            try
            {
                using (var db = new PruebasEntities())
                {
                    var listaRestaurantes = from res in db.Restaurantes
                                        select new RestauranteDTO
                                        {
                                            Id_Restaurante = res.Id_Restaurante,
                                            Restaurante = res.Restaurante
                                        };

                    // establece el filtrado de datos
                    if (!string.IsNullOrEmpty(termminoBusqueda))
                    {
                        listaRestaurantes = listaRestaurantes.Where(x => x.Id_Restaurante.ToString().Contains(termminoBusqueda) ||
                                                                    x.Restaurante.Contains(termminoBusqueda) );
                    }

                    // obtiene el total de registros antes de paginar
                    iTotalRegistros = (listaRestaurantes != null) ? listaRestaurantes.Count() : 0;

                    // se establece la ordenación
                    listaRestaurantes = establecerOrdenacion(campoOrdenar, orden, listaRestaurantes);

                    // se controla si es búsqueda total o paginada
                    if (registrosPagina > 0)
                        listaRestaurantes = listaRestaurantes.Skip((paginaActual - 1) * registrosPagina).Take(registrosPagina);

                    lista = listaRestaurantes.ToList();
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
        /// <param name="listaRestaurantes"></param>
        /// <returns></returns>
        private static IQueryable<RestauranteDTO> establecerOrdenacion(string campoOrdenar, string orden, IQueryable<RestauranteDTO> listaRestaurantes)
        {
            if (!string.IsNullOrEmpty(campoOrdenar))
            {
                if (orden.ToLower().Equals("asc"))
                {
                    if (campoOrdenar.Equals("Restaurante"))
                        listaRestaurantes = listaRestaurantes.OrderBy(x => x.Restaurante);
                    else
                        listaRestaurantes = listaRestaurantes.OrderBy(s => s.Id_Restaurante);
                }
                else
                {
                    if (campoOrdenar.Equals("Restaurante"))
                        listaRestaurantes = listaRestaurantes.OrderByDescending(x => x.Restaurante);
                    else
                        listaRestaurantes = listaRestaurantes.OrderByDescending(s => s.Id_Restaurante);
                }
            }
            else
                listaRestaurantes = listaRestaurantes.OrderBy(s => s.Id_Restaurante);

            return listaRestaurantes;
        }
    }
}
