using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ModeloPedidos.Clases;
using ModeloPedidos.Clases.DAOs;
using ModeloPedidos.Clases.DTOs;

namespace testWebApi.Controllers.Api
{
    public class PedidosController : ApiController
    {
        private PruebasEntities db = new PruebasEntities();

        // GET: api/Pedidos
        public IHttpActionResult GetPedidos(string current, string rowCount, string searchPhrase, string id)
        {
            /*
             * Formato de la peticion que realiza por ajax el bootgrid
             current=1 & rowCount=10 & sort[sender]=asc & searchPhrase= & id=b0df282a-0d67-40e5-8558-c9e93b7befed

             */

            int iCurrent = 1;
            int iRowCount = 10;
            int iTotalRegistros = 0;
            string campoOrdenar = string.Empty;
            string orden = string.Empty;

            // obtiene de los parametros del query string el campo y el sentido de ordenacion
            var vars = Request.GetQueryNameValuePairs();

            KeyValuePair<string, string> sortValues = vars.FirstOrDefault(x => x.Key.Contains("sort"));

            if (!string.IsNullOrEmpty(sortValues.Value))
            {
                campoOrdenar = sortValues.Key.TrimStart("sort.".ToArray());
                orden = sortValues.Value;
            }

            // se transforman los valores de paginación
            Int32.TryParse(current, out iCurrent);
            Int32.TryParse(rowCount, out iRowCount);

            // obtiene la lista de familias filtrada
            PedidosDAO item = new PedidosDAO();
            List<PedidoDTO> lista = new List<PedidoDTO>();

            lista = item.GetListaFiltrada(iCurrent, iRowCount, searchPhrase, campoOrdenar, orden, out iTotalRegistros);

            // se completa el objeto de respuesta
            DataBootGrid bootGrid = new DataBootGrid()
            {
                current = iCurrent,
                rowsCount = iRowCount,
                rows = lista.ToArray(),
                total = iTotalRegistros
            };

            return Ok(bootGrid);
        }


        // GET: api/Pedidos/5
        [ResponseType(typeof(Pedidos))]
        public IHttpActionResult GetPedidos(int id)
        {
            Pedidos pedidos = db.Pedidos.Find(id);
            if (pedidos == null)
            {
                return NotFound();
            }

            return Ok(pedidos);
        }

        // PUT: api/Pedidos/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPedidos(int id, Pedidos pedidos)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pedidos.Id_pedido)
            {
                return BadRequest();
            }

            db.Entry(pedidos).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PedidosExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Pedidos
        [ResponseType(typeof(Pedidos))]
        public IHttpActionResult PostPedidos(Pedidos pedidos)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Pedidos.Add(pedidos);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = pedidos.Id_pedido }, pedidos);
        }

        // DELETE: api/Pedidos/5
        [ResponseType(typeof(Pedidos))]
        public IHttpActionResult DeletePedidos(int id)
        {
            Pedidos pedidos = db.Pedidos.Find(id);
            if (pedidos == null)
            {
                return NotFound();
            }

            db.Pedidos.Remove(pedidos);
            db.SaveChanges();

            return Ok(pedidos);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PedidosExists(int id)
        {
            return db.Pedidos.Count(e => e.Id_pedido == id) > 0;
        }
    }
}