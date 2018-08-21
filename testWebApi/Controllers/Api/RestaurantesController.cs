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
    public class RestaurantesController : ApiController
    {
        private PruebasEntities db = new PruebasEntities();

        // GET: api/Restaurantes
        public IHttpActionResult GetRestaurantes(string current, string rowCount, string searchPhrase, string id)
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
            RestaurantesDAO item = new RestaurantesDAO();
            List<RestauranteDTO> lista = new List<RestauranteDTO>();

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


        // GET: api/Restaurantes/5
        [ResponseType(typeof(Restaurantes))]
        public IHttpActionResult GetRestaurantes(int id)
        {
            Restaurantes restaurantes = db.Restaurantes.Find(id);
            if (restaurantes == null)
            {
                return NotFound();
            }

            return Ok(restaurantes);
        }

        // PUT: api/Restaurantes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRestaurantes(int id, Restaurantes restaurantes)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != restaurantes.Id_Restaurante)
            {
                return BadRequest();
            }

            db.Entry(restaurantes).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RestaurantesExists(id))
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

        // POST: api/Restaurantes
        [ResponseType(typeof(Restaurantes))]
        public IHttpActionResult PostRestaurantes(Restaurantes restaurantes)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Restaurantes.Add(restaurantes);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = restaurantes.Id_Restaurante }, restaurantes);
        }

        // DELETE: api/Restaurantes/5
        [ResponseType(typeof(Restaurantes))]
        public IHttpActionResult DeleteRestaurantes(int id)
        {
            Restaurantes restaurantes = db.Restaurantes.Find(id);
            if (restaurantes == null)
            {
                return NotFound();
            }

            db.Restaurantes.Remove(restaurantes);
            db.SaveChanges();

            return Ok(restaurantes);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RestaurantesExists(int id)
        {
            return db.Restaurantes.Count(e => e.Id_Restaurante == id) > 0;
        }
    }
}