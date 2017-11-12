using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Przuchodnia_Medyczna_Inz.Models;
using Przuchodnia_Medyczna_Inz.DAL;

namespace Przuchodnia_Medyczna_Inz.Controllers
{
    public class SpecjalizacjaController : Controller
    {
        private PrzychodniaContext db = new PrzychodniaContext();

        // GET: /Specjalizacja/
        public ActionResult Index()
        {
            var specjalizacja = db.Specjalizacja.ToList();
            return View(specjalizacja);
        }
      
        // GET: /Specjalizacja/Details/5
        public ActionResult _Details(int? id)
        {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Specjalizacja specjalizacja = db.Specjalizacja.Find(id);
                if (specjalizacja == null)
                {
                    return HttpNotFound();
                }
               
                if (!Request.IsAjaxRequest())
                {
                    return View(specjalizacja);
                }
                else
                {
                    return PartialView("_Details", specjalizacja);
                
                }
            
        }

        // POST: /Specjalizacja/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(string nazwa, string opis)
        {
            if (ModelState.IsValid)
            {
                var spec = new Specjalizacja();
                spec.Nazwa = nazwa;
                spec.Opis = opis;

                db.Specjalizacja.Add(spec);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
        }

        // GET: /Specjalizacja/Delete/5
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Specjalizacja specjalizacja = db.Specjalizacja.Find(id);
            if (specjalizacja == null)
            {
                return HttpNotFound();
            }
            return PartialView("_Delete", specjalizacja);
        }

        // POST: /Specjalizacja/Delete/5
        [HttpPost]    
        public ActionResult Delete(int id)
        {
            Specjalizacja specjalizacja = db.Specjalizacja.Find(id);
            db.Specjalizacja.Remove(specjalizacja);
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
