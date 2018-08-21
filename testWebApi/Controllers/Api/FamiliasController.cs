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
    public class FamiliasController : ApiController
    {
        private PruebasEntities db = new PruebasEntities();

        // GET: api/Familias
        public IQueryable<Familia> GetFamilia()
        {
            return db.Familia;
        }

        // GET: api/Familias/5
        [ResponseType(typeof(Familia))]
        public IHttpActionResult GetFamilia(int id)
        {
            Familia familia = db.Familia.Find(id);
            if (familia == null)
            {
                return NotFound();
            }

            return Ok(familia);
        }

        // PUT: api/Familias/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutFamilia(int id, Familia familia)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != familia.Id)
            {
                return BadRequest();
            }

            db.Entry(familia).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FamiliaExists(id))
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

        // POST: api/Familias
        [ResponseType(typeof(Familia))]
        public IHttpActionResult PostFamilia(Familia familia)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Familia.Add(familia);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = familia.Id }, familia);
        }

        // DELETE: api/Familias/5
        [ResponseType(typeof(Familia))]
        public IHttpActionResult DeleteFamilia(int id)
        {
            Familia familia = db.Familia.Find(id);
            if (familia == null)
            {
                return NotFound();
            }

            db.Familia.Remove(familia);
            db.SaveChanges();

            return Ok(familia);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FamiliaExists(int id)
        {
            return db.Familia.Count(e => e.Id == id) > 0;
        }
    }
}