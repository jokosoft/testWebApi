using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ModeloPedidos.Clases;

namespace testWebApi.Controllers
{
    public class RestaurantesController : Controller
    {
        private PruebasEntities db = new PruebasEntities();

        // GET: Restaurantes
        public ActionResult Index()
        {
            return View(db.Restaurantes.ToList());
        }

        // GET: Restaurantes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Restaurantes restaurantes = db.Restaurantes.Find(id);
            if (restaurantes == null)
            {
                return HttpNotFound();
            }
            return View(restaurantes);
        }

        // GET: Restaurantes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Restaurantes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Restaurante,Restaurante")] Restaurantes restaurantes)
        {
            if (ModelState.IsValid)
            {
                db.Restaurantes.Add(restaurantes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(restaurantes);
        }

        // GET: Restaurantes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Restaurantes restaurantes = db.Restaurantes.Find(id);
            if (restaurantes == null)
            {
                return HttpNotFound();
            }
            return View(restaurantes);
        }

        // POST: Restaurantes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Restaurante,Restaurante")] Restaurantes restaurantes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(restaurantes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(restaurantes);
        }

        // GET: Restaurantes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Restaurantes restaurantes = db.Restaurantes.Find(id);
            if (restaurantes == null)
            {
                return HttpNotFound();
            }
            return View(restaurantes);
        }

        // POST: Restaurantes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Restaurantes restaurantes = db.Restaurantes.Find(id);
            db.Restaurantes.Remove(restaurantes);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
