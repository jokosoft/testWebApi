using ModeloPedidos.Clases.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModeloPedidos.Clases.DAOs
{
    public class PersonasDAO
    {
        public PersonasDAO()
        {

        }

        public List<Personas> Get()
        {
            List<Personas> lista = new List<Personas>();

            try
            {
                using (var db = new PruebasEntities())
                {
                    lista = db.Personas
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
        public List<PersonaDTO> GetListaFiltrada(int paginaActual, int registrosPagina, string termminoBusqueda,
                                                string campoOrdenar, string orden, out int iTotalRegistros)
        {
            // nota: paginacion linq :: coleccion.Skip(pagAMostrar * numElementosPorPagina).Take(numElementosPorPagina);
            List<PersonaDTO> lista = new List<PersonaDTO>();

            try
            {
                using (var db = new PruebasEntities())
                {
                    var listaPersonas = from per in db.Personas
                                        select new PersonaDTO
                                        {
                                            id = per.id,
                                            nombre = per.nombre,
                                            edad = per.edad
                                        };

                    // establece el filtrado de datos
                    if (!string.IsNullOrEmpty(termminoBusqueda))
                    {
                        listaPersonas = listaPersonas.Where(x => x.id.ToString().Contains(termminoBusqueda) ||
                                                                    x.nombre.Contains(termminoBusqueda) ||
                                                                    x.edad.ToString().Contains(termminoBusqueda));
                    }

                    // obtiene el total de registros antes de paginar
                    iTotalRegistros = (listaPersonas != null) ? listaPersonas.Count() : 0;

                    // se establece la ordenación
                    listaPersonas = establecerOrdenacion(campoOrdenar, orden, listaPersonas);

                    // se controla si es búsqueda total o paginada
                    if (registrosPagina > 0)
                        listaPersonas = listaPersonas.Skip((paginaActual - 1) * registrosPagina).Take(registrosPagina);

                    lista = listaPersonas.ToList();
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
        /// <param name="listaPersonas"></param>
        /// <returns></returns>
        private static IQueryable<PersonaDTO> establecerOrdenacion(string campoOrdenar, string orden, IQueryable<PersonaDTO> listaPersonas)
        {
            if (!string.IsNullOrEmpty(campoOrdenar))
            {
                if (orden.ToLower().Equals("asc"))
                {
                    if (campoOrdenar.Equals("nombre"))
                        listaPersonas = listaPersonas.OrderBy(x => x.nombre);
                    else if (campoOrdenar.Equals("edad"))
                        listaPersonas = listaPersonas.OrderBy(x => x.edad);
                    else
                        listaPersonas = listaPersonas.OrderBy(s => s.id);
                }
                else
                {
                    if (campoOrdenar.Equals("nombre"))
                        listaPersonas = listaPersonas.OrderByDescending(x => x.nombre);
                    else if (campoOrdenar.Equals("edad"))
                        listaPersonas = listaPersonas.OrderByDescending(x => x.edad);
                    else
                        listaPersonas = listaPersonas.OrderByDescending(s => s.id);
                }
            }
            else
                listaPersonas = listaPersonas.OrderBy(s => s.id);

            return listaPersonas;
        }
    }
}
