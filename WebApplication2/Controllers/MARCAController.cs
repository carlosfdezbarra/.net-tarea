using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class MARCAController : Controller
    {
        private testEntities2 db = new testEntities2();

        // GET: MARCA
        public ActionResult Index()
        {
            return View(db.MARCA.ToList());
        }

        // GET: MARCA/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MARCA mARCA = db.MARCA.Find(id);
            if (mARCA == null)
            {
                return HttpNotFound();
            }
            return View(mARCA);
        }

        // GET: MARCA/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MARCA/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_MARCA,DESCRIPCION")] MARCA mARCA)
        {
            if (ModelState.IsValid)
            {

                var patejemplo = db.MARCA.Where(x => x.DESCRIPCION == mARCA.DESCRIPCION);

                if (patejemplo == null || patejemplo.Count() == 0)
                {
                    db.MARCA.Add(mARCA);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.ID_MARCA = new SelectList(db.MARCA, "ID_MARCA", "DESCRIPCION", mARCA.ID_MARCA);
                    ModelState.AddModelError("DESCRIPCION", "Marca ya registrada");
                    return View(mARCA);
                }
                
            }

            return View(mARCA);
        }

        // GET: MARCA/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MARCA mARCA = db.MARCA.Find(id);
            if (mARCA == null)
            {
                return HttpNotFound();
            }
            return View(mARCA);
        }

        // POST: MARCA/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_MARCA,DESCRIPCION")] MARCA mARCA)
        {
            if (ModelState.IsValid)
            {

                var patejemplo = db.MARCA.Where(x => x.DESCRIPCION == mARCA.DESCRIPCION);

                if (patejemplo.Count() < 1)
                {
                    db.Entry(mARCA).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.ID_MARCA = new SelectList(db.MARCA, "ID_MARCA", "DESCRIPCION", mARCA.ID_MARCA);
                    ModelState.AddModelError("DESCRIPCION", "Marca ya registrada");
                    return View(mARCA);
                }
                
            }
            return View(mARCA);
        }

        // GET: MARCA/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MARCA mARCA = db.MARCA.Find(id);
            if (mARCA == null)
            {
                return HttpNotFound();
            }
            return View(mARCA);
        }

        // POST: MARCA/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MARCA mARCA = db.MARCA.Find(id);
            db.MARCA.Remove(mARCA);
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
