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

namespace testWebApi.Controllers.Api
{
    public class PersonasController : ApiController
    {
        private PruebasEntities db = new PruebasEntities();

        // GET: api/Personas
        public IQueryable<Personas> GetPersonas()
        {
            return db.Personas;
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