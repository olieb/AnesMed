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
    public class PlacowkaController : Controller
    {
        private PrzychodniaContext db = new PrzychodniaContext();

        // GET: /Placowka/
        public ActionResult Index()
        {
            var placowkamedyczna = db.PlacowkaMedyczna.Include(p => p.Adres);
            return View(placowkamedyczna.ToList());
        }

        [HttpPost]
        public ActionResult Create(string nazwa, string kom, string tel, string otwarte, string ulica, int budynek, string kod, string miejscowosc )
        {
            if (ModelState.IsValid)
            {
                var placowka = new PlacowkaMedyczna()
                {
                    Nazwa = nazwa,
                    Komorka = kom,
                    Telefon  = tel,
                    GodzinyOtwarcia = otwarte,
                    Adres = new Adres()
                        {
                            Ulica = ulica,
                            NrBudynku = budynek,
                            KodPocztowy = kod,
                            Miejscowosc = miejscowosc
                        }
                };
                db.PlacowkaMedyczna.Add(placowka);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        // GET: /Placowka/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlacowkaMedyczna placowkamedyczna = db.PlacowkaMedyczna.Find(id);
            if (placowkamedyczna == null)
            {
                return HttpNotFound();
            }
            return PartialView("_Edit", placowkamedyczna);
        }

        // POST: /Placowka/Edit/5
        [HttpPost]
        public ActionResult Edit(PlacowkaMedyczna placowka)
        {
            if (ModelState.IsValid)
            {
                db.Entry(placowka).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            TempData["error"] = "Wystapił nie oczekiwany błąd. Edycja nie powiodła sie.";
            return RedirectToAction("Index");
        }

        // GET: /Placowka/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlacowkaMedyczna placowkamedyczna = db.PlacowkaMedyczna.Find(id);
            if (placowkamedyczna == null)
            {
                return HttpNotFound();
            }
            return PartialView("_Delete",placowkamedyczna);
        }

        // POST: /Placowka/Delete/5
        [HttpPost, ActionName("Delete")]       
        public ActionResult DeleteConfirmed(int id)
        {
            PlacowkaMedyczna placowkamedyczna = db.PlacowkaMedyczna.Find(id);
            db.PlacowkaMedyczna.Remove(placowkamedyczna);
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
