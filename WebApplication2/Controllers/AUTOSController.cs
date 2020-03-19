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
    public class AUTOSController : Controller
    {
        private testEntities2 db = new testEntities2();

        // GET: AUTOS
        public ActionResult Index()
        {
            var aUTOS = db.AUTOS.Include(a => a.MODELO);

            ViewBag.ID_MARCA = new SelectList(db.MARCA, "ID_MARCA", "DESCRIPCION");
            var query = (from n in db.AUTOS
                         select n.ANO
                        ).Distinct()
                        .OrderBy(ANO => ANO);


            ViewBag.ID_AUTO = new SelectList(query);
            return View(aUTOS.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(int ID_MARCA, string patente, int ID_AUTO)
        {
            var query = (from n in db.AUTOS
                         select n.ANO
                        ).Distinct()
                        .OrderBy(ANO => ANO);
            var aUTOS = db.AUTOS.Include(a => a.MODELO);
            ViewBag.ID_AUTO = new SelectList(query);
            ViewBag.ID_MARCA = new SelectList(db.MARCA, "ID_MARCA", "DESCRIPCION");


            if(patente == "")
            {
                var autitos = aUTOS.Where(x => x.MODELO.MARCA.ID_MARCA == ID_MARCA);
                return View(autitos.Where(y=>y.ANO == ID_AUTO).ToList());
            }
            else
            {
                if (patente.Length < 3)
                {

                    ViewBag.ID_MODELO = (db.MODELO, "patente", "PATENTE");
                    ModelState.AddModelError("patente", "Tiene que ingresar como mínimo 3 caracteres ");
                    return View(aUTOS);
                }
                else
                {
                    var autitos = aUTOS.Where(x => x.MODELO.MARCA.ID_MARCA == ID_MARCA);
                    var autitos2 = autitos.Where(y => y.ANO == ID_AUTO);
                    return View(autitos2.Where(y => y.PATENTE.Contains(patente)).ToList());
                }
                    
            }
            
        }

        // GET: AUTOS/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AUTOS aUTOS = db.AUTOS.Find(id);
            if (aUTOS == null)
            {
                return HttpNotFound();
            }
            return View(aUTOS);
        }

        // GET: AUTOS/Create
        public ActionResult Create()
        {
            ViewBag.ID_MODELO = new SelectList(db.MODELO, "ID_MODELO", "DESCRIPCION_MODELO");
            return View();
        }

        // POST: AUTOS/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_AUTO,ID_MODELO,PATENTE,ANO,COLOR,OBSERVACION")] AUTOS aUTOS)
        {
            if (ModelState.IsValid)
            {
                if (aUTOS.ANO < 1990)
                {
                    aUTOS.OBSERVACION = "ANTIGUO";
                }
                else
                {
                    aUTOS.OBSERVACION = "SIN OBSERVACIONES";
                }
                var patejemplo = db.AUTOS.Where(x => x.PATENTE == aUTOS.PATENTE);
                if (patejemplo == null || patejemplo.Count() == 0)
                {           
                    
                    int datefecha = DateTime.Now.Year;
                    Console.WriteLine(datefecha);
                    if (aUTOS.ANO > (datefecha + 2))
                    {
                        
                        ViewBag.ID_MODELO = new SelectList(db.MODELO, "ID_MODELO", "DESCRIPCION_MODELO", aUTOS.ID_MODELO);
                        ModelState.AddModelError("ANO", "El año no puede ser superior a " + (datefecha + 2));
                        return View(aUTOS);
                    }
                    else
                    {
                        db.AUTOS.Add(aUTOS);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                else
                {

                    int datefecha = DateTime.Now.Year;
                    Console.WriteLine(datefecha);
                    if (aUTOS.ANO > (datefecha + 2))
                    {

                        ViewBag.ID_MODELO = new SelectList(db.MODELO, "ID_MODELO", "DESCRIPCION_MODELO", aUTOS.ID_MODELO);
                        ModelState.AddModelError("ANO", "El año no puede ser superior a " + (datefecha + 2));
                        ModelState.AddModelError("PATENTE", "Patente ya registrada");
                        return View(aUTOS);
                    }
                    else
                    {
                        ViewBag.ID_MODELO = new SelectList(db.MODELO, "ID_MODELO", "DESCRIPCION_MODELO", aUTOS.ID_MODELO);
                        ModelState.AddModelError("PATENTE", "Patente ya registrada");
                    }
                    
            
                }

                
                
                
            }

           
            return View(aUTOS);
        }

        // GET: AUTOS/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AUTOS aUTOS = db.AUTOS.Find(id);
            if (aUTOS == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID_MODELO = new SelectList(db.MODELO, "ID_MODELO", "DESCRIPCION_MODELO", aUTOS.ID_MODELO);
            return View(aUTOS);
        }

        // POST: AUTOS/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_AUTO,ID_MODELO,PATENTE,ANO,COLOR,OBSERVACION")] AUTOS aUTOS)
        {
            if (ModelState.IsValid)
            {
                if (aUTOS.ANO < 1990)
                {
                    aUTOS.OBSERVACION = "ANTIGUO";
                }
                else
                {
                    aUTOS.OBSERVACION = "SIN OBSERVACIONES";
                }
                var patejemplo = db.AUTOS.Where(x => x.PATENTE == aUTOS.PATENTE);
                if (patejemplo.Count() < 1)
                {

                    int datefecha = DateTime.Now.Year;
                    Console.WriteLine(datefecha);
                    if (aUTOS.ANO > (datefecha + 2))
                    {

                        ViewBag.ID_MODELO = new SelectList(db.MODELO, "ID_MODELO", "DESCRIPCION_MODELO", aUTOS.ID_MODELO);
                        ModelState.AddModelError("ANO", "El año no puede ser superior a " + (datefecha + 2));
                        return View(aUTOS);
                    }
                    else
                    {
                        db.Entry(aUTOS).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                else
                {

                    int datefecha = DateTime.Now.Year;
                    Console.WriteLine(datefecha);
                    if (aUTOS.ANO > (datefecha + 2))
                    {

                        ViewBag.ID_MODELO = new SelectList(db.MODELO, "ID_MODELO", "DESCRIPCION_MODELO", aUTOS.ID_MODELO);
                        ModelState.AddModelError("ANO", "El año no puede ser superior a " + (datefecha + 2));
                        ModelState.AddModelError("PATENTE", "Patente ya registrada");
                        return View(aUTOS);
                    }
                    else
                    {
                        ViewBag.ID_MODELO = new SelectList(db.MODELO, "ID_MODELO", "DESCRIPCION_MODELO", aUTOS.ID_MODELO);
                        ModelState.AddModelError("PATENTE", "Patente ya registrada");
                        return View(aUTOS);
                    }


                }
                
                return RedirectToAction("Index");
            }
            ViewBag.ID_MODELO = new SelectList(db.MODELO, "ID_MODELO", "DESCRIPCION_MODELO", aUTOS.ID_MODELO);
            return View(aUTOS);
        }

        // GET: AUTOS/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AUTOS aUTOS = db.AUTOS.Find(id);
            if (aUTOS == null)
            {
                return HttpNotFound();
            }
            return View(aUTOS);
        }

        // POST: AUTOS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AUTOS aUTOS = db.AUTOS.Find(id);
            db.AUTOS.Remove(aUTOS);
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
