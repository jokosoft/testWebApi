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
    public class FamiliasController : ApiController
    {
        private PruebasEntities db = new PruebasEntities();

        // GET: api/Familias
        public IHttpActionResult GetFamilias(string current, string rowCount, string searchPhrase, string id)
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
            FamiliasDAO item = new FamiliasDAO();
            List<FamiliaDTO> lista = new List<FamiliaDTO>();

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