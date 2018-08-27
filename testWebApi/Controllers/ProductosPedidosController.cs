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
    public class ProductosPedidosController : Controller
    {
        private PruebasEntities db = new PruebasEntities();

        [ChildActionOnly]
        public ActionResult _listaProductosPedido(int? idPedido)
        {
            var lista = db.ProductosPedidos.Where(x => x.FIdPedido == idPedido).ToList();
            return PartialView("_listaProductosPedido", lista);
        }
        
        // GET: ProductosPedidos
        public ActionResult Index()
        {
            var productosPedidos = db.ProductosPedidos.Include(p => p.Productos);
            return View(productosPedidos.ToList());
        }

        // GET: ProductosPedidos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductosPedidos productosPedidos = db.ProductosPedidos.Find(id);
            if (productosPedidos == null)
            {
                return HttpNotFound();
            }
            return View(productosPedidos);
        }

        // GET: ProductosPedidos/Create
        public ActionResult Create()
        {
            ViewBag.FIdProducto = new SelectList(db.Productos, "Id_prod", "Nombre_prod");
            return View();
        }

        // POST: ProductosPedidos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_productosPedido,FIdPedido,FIdProducto,Cantidad")] ProductosPedidos productosPedidos)
        {
            if (ModelState.IsValid)
            {
                db.ProductosPedidos.Add(productosPedidos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FIdProducto = new SelectList(db.Productos, "Id_prod", "Nombre_prod", productosPedidos.FIdProducto);
            return View(productosPedidos);
        }

        // GET: ProductosPedidos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductosPedidos productosPedidos = db.ProductosPedidos.Find(id);
            if (productosPedidos == null)
            {
                return HttpNotFound();
            }
            ViewBag.FIdProducto = new SelectList(db.Productos, "Id_prod", "Nombre_prod", productosPedidos.FIdProducto);
            return View(productosPedidos);
        }

        // POST: ProductosPedidos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_productosPedido,FIdPedido,FIdProducto,Cantidad")] ProductosPedidos productosPedidos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productosPedidos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FIdProducto = new SelectList(db.Productos, "Id_prod", "Nombre_prod", productosPedidos.FIdProducto);
            return View(productosPedidos);
        }

        // GET: ProductosPedidos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductosPedidos productosPedidos = db.ProductosPedidos.Find(id);
            if (productosPedidos == null)
            {
                return HttpNotFound();
            }
            return View(productosPedidos);
        }

        // POST: ProductosPedidos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductosPedidos productosPedidos = db.ProductosPedidos.Find(id);
            db.ProductosPedidos.Remove(productosPedidos);
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
