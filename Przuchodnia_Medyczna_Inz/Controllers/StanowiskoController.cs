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
    
    public class StanowiskoController : Controller
    {
        private PrzychodniaContext db = new PrzychodniaContext();

        // GET: /Stanowisko/
        public ActionResult Index()
        {
            return View(db.Stanowisko.ToList());
        }

        // GET: /Stanowisko/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stanowisko stanowisko = db.Stanowisko.Find(id);
            if (stanowisko == null)
            {
                return HttpNotFound();
            }

            if (!Request.IsAjaxRequest())
            {
                return View(stanowisko);
            }
            else
            {
                return PartialView("_Details", stanowisko);

            }
        }

        // GET: /Stanowisko/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Stanowisko/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(string nazwa, string obowiazki)
        {
            if (ModelState.IsValid)
            {
                var stanowisko = new Stanowisko();
                stanowisko.Nazwa = nazwa;
                stanowisko.Obowiazki = obowiazki;

                db.Stanowisko.Add(stanowisko);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
        }

        // GET: /Stanowisko/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stanowisko stanowisko = db.Stanowisko.Find(id);
            if (stanowisko == null)
            {
                return HttpNotFound();
            }
            return View(stanowisko);
        }

        // POST: /Stanowisko/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="StanowiskoID,Nazwa,Obowiazki")] Stanowisko stanowisko)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stanowisko).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(stanowisko);
        }

        // GET: /Stanowisko/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stanowisko stanowisko = db.Stanowisko.Find(id);
            if (stanowisko == null)
            {
                return HttpNotFound();
            }
            return PartialView("_Delete", stanowisko);
        }

        // POST: /Stanowisko/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Stanowisko stanowisko = db.Stanowisko.Find(id);
            db.Stanowisko.Remove(stanowisko);
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
