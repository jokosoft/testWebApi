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
    public class PersonasController : ApiController
    {
        private PruebasEntities db = new PruebasEntities();

        // GET: api/Personas
        public IHttpActionResult GetPersonas(string current, string rowCount, string searchPhrase, string id)
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
            PersonasDAO item = new PersonasDAO();
            List<PersonaDTO> lista = new List<PersonaDTO>();

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


        // GET: api/Personas/5
        [ResponseType(typeof(Personas))]
        public IHttpActionResult GetPersonas(int id)
        {
            Personas personas = db.Personas.Find(id);
            if (personas == null)
            {
                return NotFound();
            }

            return Ok(personas);
        }

        // PUT: api/Personas/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPersonas(int id, Personas personas)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != personas.id)
            {
                return BadRequest();
            }

            db.Entry(personas).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonasExists(id))
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

        // POST: api/Personas
        [ResponseType(typeof(Personas))]
        public IHttpActionResult PostPersonas(Personas personas)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Personas.Add(personas);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = personas.id }, personas);
        }

        // DELETE: api/Personas/5
        [ResponseType(typeof(Personas))]
        public IHttpActionResult DeletePersonas(int id)
        {
            Personas personas = db.Personas.Find(id);
            if (personas == null)
            {
                return NotFound();
            }

            db.Personas.Remove(personas);
            db.SaveChanges();

            return Ok(personas);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PersonasExists(int id)
        {
            return db.Personas.Count(e => e.id == id) > 0;
        }
    }
}