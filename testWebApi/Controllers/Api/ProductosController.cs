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
    public class ProductosController : ApiController
    {
        private PruebasEntities db = new PruebasEntities();

        // GET: api/Productos
        public IHttpActionResult GetProductos(string current, string rowCount, string searchPhrase, string id)
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

            if(!string.IsNullOrEmpty(sortValues.Value))
            {
                campoOrdenar = sortValues.Key.TrimStart("sort.".ToArray());
                orden = sortValues.Value;
            }                      

            // se transforman los valores de paginación
            Int32.TryParse(current, out iCurrent);
            Int32.TryParse(rowCount, out iRowCount);

            // obtiene la lista de productos filtrada
            ProductosDAO item = new ProductosDAO();
            List<ProductoDTO> lista = new List<ProductoDTO>();

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

        // GET: api/Productos/5
        [ResponseType(typeof(Productos))]
        public IHttpActionResult GetProductos(int id)
        {
            Productos productos = db.Productos.Find(id);
            if (productos == null)
            {
                return NotFound();
            }

            return Ok(productos);
        }

        // PUT: api/Productos/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProductos(int id, Productos productos)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != productos.Id_prod)
            {
                return BadRequest();
            }

            db.Entry(productos).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductosExists(id))
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

        // POST: api/Productos
        [ResponseType(typeof(Productos))]
        public IHttpActionResult PostProductos(Productos productos)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Productos.Add(productos);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = productos.Id_prod }, productos);
        }

        // DELETE: api/Productos/5
        [ResponseType(typeof(Productos))]
        public IHttpActionResult DeleteProductos(int id)
        {
            Productos productos = db.Productos.Find(id);
            if (productos == null)
            {
                return NotFound();
            }

            db.Productos.Remove(productos);
            db.SaveChanges();

            return Ok(productos);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductosExists(int id)
        {
            return db.Productos.Count(e => e.Id_prod == id) > 0;
        }
    }
}