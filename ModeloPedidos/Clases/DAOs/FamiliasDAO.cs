using ModeloPedidos.Clases.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModeloPedidos.Clases.DAOs
{
    public class FamiliasDAO
    {
        public FamiliasDAO()
        {

        }

        public List<Familia> Get()
        {
            List<Familia> lista = new List<Familia>();

            try
            {
                using (var db = new PruebasEntities())
                {
                    lista = db.Familia
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
        /// <param name="current">Pagina que se quiere mostrar</param>
        /// <param name="rowCount">filas que se quieren mostrar en cada página</param>
        /// <param name="searchPhrase">Término de búsqueda</param>
        /// <param name="campoOrdenar">Nombre del campo por qu e quieren ordenar los resultados</param>
        /// <param name="orden">sentido de la ordenación</param>
        /// <returns></returns>
        public List<FamiliaDTO> GetListaFiltrada(int paginaActual, int registrosPagina, string termminoBusqueda,
                                                string campoOrdenar, string orden, out int iTotalRegistros)
        {
            // nota: paginacion linq :: coleccion.Skip(pagAMostrar * numElementosPorPagina).Take(numElementosPorPagina);
            List<FamiliaDTO> lista = new List<FamiliaDTO>();

            try
            {
                using (var db = new PruebasEntities())
                {
                    var listaFamilias = from fam in db.Familia
                                         select new FamiliaDTO
                                         {
                                             Id = fam.Id,
                                             Nombre = fam.Nombre,
                                             Descripcion = fam.Descripcion
                                         };

                    // establece el filtrado de datos
                    if (!string.IsNullOrEmpty(termminoBusqueda))
                    {
                        listaFamilias = listaFamilias.Where(x => x.Id.ToString().Contains(termminoBusqueda) ||
                                                                    x.Nombre.Contains(termminoBusqueda) ||
                                                                    x.Descripcion.Contains(termminoBusqueda) );
                    }

                    // obtiene el total de registros antes de paginar
                    iTotalRegistros = (listaFamilias != null) ? listaFamilias.Count() : 0;

                    // se establece la ordenación
                    listaFamilias = establecerOrdenacion(campoOrdenar, orden, listaFamilias);

                    // se controla si es búsqueda total o paginada
                    if (registrosPagina > 0)
                        listaFamilias = listaFamilias.Skip((paginaActual - 1) * registrosPagina).Take(registrosPagina);

                    lista = listaFamilias.ToList();
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
        /// <param name="listaFamilias"></param>
        /// <returns></returns>
        private static IQueryable<FamiliaDTO> establecerOrdenacion(string campoOrdenar, string orden, IQueryable<FamiliaDTO> listaFamilias)
        {
            if (!string.IsNullOrEmpty(campoOrdenar))
            {
                if (orden.ToLower().Equals("asc"))
                {
                    if (campoOrdenar.Equals("Nombre"))
                        listaFamilias = listaFamilias.OrderBy(x => x.Nombre);
                    else if (campoOrdenar.Equals("Descripcion"))
                        listaFamilias = listaFamilias.OrderBy(x => x.Descripcion);
                    else
                        listaFamilias = listaFamilias.OrderBy(s => s.Id);
                }
                else
                {
                    if (campoOrdenar.Equals("Nombre"))
                        listaFamilias = listaFamilias.OrderByDescending(x => x.Nombre);
                    else if (campoOrdenar.Equals("Descripcion"))
                        listaFamilias = listaFamilias.OrderByDescending(x => x.Descripcion);
                    else
                        listaFamilias = listaFamilias.OrderByDescending(s => s.Id);
                }
            }
            else
                listaFamilias = listaFamilias.OrderBy(s => s.Id);

            return listaFamilias;
        }
    }
}
