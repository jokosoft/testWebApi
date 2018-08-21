using ModeloPedidos.Clases.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModeloPedidos.Clases.DAOs
{
    public class PedidosDAO
    {
        public PedidosDAO()
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
        public List<PedidoDTO> GetListaFiltrada(int paginaActual, int registrosPagina, string termminoBusqueda,
                                                string campoOrdenar, string orden, out int iTotalRegistros)
        {
            // nota: paginacion linq :: coleccion.Skip(pagAMostrar * numElementosPorPagina).Take(numElementosPorPagina);
            List<PedidoDTO> lista = new List<PedidoDTO>();

            try
            {
                using (var db = new PruebasEntities())
                {
                    var listaPedidos = from pe in db.Pedidos
                                       join res in db.Restaurantes
                                           on pe.FIdRestaurante equals res.Id_Restaurante
                                       join per in db.Personas
                                           on pe.FIdPersona equals per.id
                                       select new PedidoDTO
                                       {
                                           Id_pedido = pe.Id_pedido,
                                           Referencia = pe.Referencia,
                                           Fecha = pe.Fecha,
                                           FIdRestaurante = pe.FIdRestaurante,
                                           NombreRestaurante = res.Restaurante,
                                           FIdPersona = pe.FIdPersona,
                                           NombrePersona = per.nombre
                                       };
                    
                    // establece el filtrado de datos
                    if (!string.IsNullOrEmpty(termminoBusqueda))
                    {
                        listaPedidos = listaPedidos.Where(x => x.Id_pedido.ToString().Contains(termminoBusqueda) ||
                                                                    x.Referencia.Contains(termminoBusqueda) ||
                                                                    x.Fecha.ToString("dd/MM/yyyy").Contains(termminoBusqueda) ||
                                                                    x.NombrePersona.Contains(termminoBusqueda) ||
                                                                    x.NombreRestaurante.ToString().Contains(termminoBusqueda));
                    }

                    // obtiene el total de registros antes de paginar
                    iTotalRegistros = (listaPedidos != null) ? listaPedidos.Count() : 0;

                    // se establece la ordenación
                    listaPedidos = establecerOrdenacion(campoOrdenar, orden, listaPedidos);

                    // se controla si es búsqueda total o paginada
                    if (registrosPagina > 0)
                        listaPedidos = listaPedidos.Skip((paginaActual - 1) * registrosPagina).Take(registrosPagina);

                    lista = listaPedidos.ToList();
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
        /// <param name="listaPedidos"></param>
        /// <returns></returns>
        private static IQueryable<PedidoDTO> establecerOrdenacion(string campoOrdenar, string orden, IQueryable<PedidoDTO> listaPedidos)
        {
            if (!string.IsNullOrEmpty(campoOrdenar))
            {
                if (orden.ToLower().Equals("asc"))
                {
                    if (campoOrdenar.Equals("Referencia"))
                        listaPedidos = listaPedidos.OrderBy(x => x.Referencia);
                    else if (campoOrdenar.Equals("Fecha"))
                        listaPedidos = listaPedidos.OrderBy(x => x.Fecha);
                    else if (campoOrdenar.Equals("NombreRestaurante"))
                        listaPedidos = listaPedidos.OrderBy(x => x.NombreRestaurante);
                    else if (campoOrdenar.Equals("NombrePersona"))
                        listaPedidos = listaPedidos.OrderBy(x => x.NombrePersona);
                    else
                        listaPedidos = listaPedidos.OrderBy(s => s.Id_pedido);
                }
                else
                {
                    if (campoOrdenar.Equals("Referencia"))
                        listaPedidos = listaPedidos.OrderByDescending(x => x.Referencia);
                    else if (campoOrdenar.Equals("Fecha"))
                        listaPedidos = listaPedidos.OrderByDescending(x => x.Fecha);
                    else if (campoOrdenar.Equals("NombreRestaurante"))
                        listaPedidos = listaPedidos.OrderByDescending(x => x.NombreRestaurante);
                    else if (campoOrdenar.Equals("NombrePersona"))
                        listaPedidos = listaPedidos.OrderByDescending(x => x.NombrePersona);
                    else
                        listaPedidos = listaPedidos.OrderByDescending(s => s.Id_pedido);
                }
            }
            else
                listaPedidos = listaPedidos.OrderBy(s => s.Id_pedido);

            return listaPedidos;
        }
    }
}
