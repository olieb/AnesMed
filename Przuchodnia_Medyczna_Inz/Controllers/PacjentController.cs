using Przuchodnia_Medyczna_Inz.DAL;
using Przuchodnia_Medyczna_Inz.Models;
using Przuchodnia_Medyczna_Inz.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Przuchodnia_Medyczna_Inz.Controllers
{
    public class PacjentController : Controller
    {
        private PrzychodniaContext db = new PrzychodniaContext();

        // GET: /Pacjent/
        public ActionResult Index(string imieNazwisko, long? pesel)
        {
            var model = new AdresyOsobVM();

            model.Pacjenci = (from p in db.Pacjent select p).OrderBy(p => p.Nazwisko);
            model.Adresy = from a in db.Adres select a;

            if (!String.IsNullOrEmpty(imieNazwisko))
            {
                model.Pacjenci = model.Pacjenci.Where(s => s.ImieNazwisko.Contains(imieNazwisko));
            }
            if (pesel != null)
            {
                model.Pacjenci = model.Pacjenci.Where(p => p.PESEL == pesel);
            }

            return View(model);
        }

        // GET: /Pacjent/Details/5
        public ActionResult Wizyty(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var pacjent = db.Pacjent.Find(id);

            if (pacjent == null)
            {
                return HttpNotFound();
            }
            return View(pacjent);
        }

        // GET: /Pacjent/Create
        public ActionResult Create()
        {
            ViewBag.AdresID = new SelectList(db.Adres, "AdresID", "Miejscowosc");
            return View();
        }

        // POST: /Pacjent/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AdresyOsobVM model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    Adres adres = new Adres();
                    Pacjent pacjent = new Pacjent();

                    pacjent.Imie = model.Pacjent.Imie;
                    pacjent.Nazwisko = model.Pacjent.Nazwisko;
                    pacjent.PESEL = model.Pacjent.PESEL;
                    pacjent.NIP = model.Pacjent.NIP;
                    pacjent.Telefon = model.Pacjent.Telefon;

                    adres.Ulica = model.Adres.Ulica;
                    adres.NrBudynku = model.Adres.NrBudynku;
                    adres.NrMieszkania = model.Adres.NrMieszkania;
                    adres.KodPocztowy = model.Adres.KodPocztowy;
                    adres.Miejscowosc = model.Adres.Miejscowosc;

                    pacjent.Adres = adres;

                    TempData["Message"] = "Dodano pacjenta " + pacjent.ImieNazwisko + " do listy pacjentów AnesMed.";

                    db.Osoba.Add(pacjent);
                    db.SaveChanges();
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation("Property: {0} Error: {1}",
                                    validationError.PropertyName,
                                    validationError.ErrorMessage);
                        }
                    }
                }

                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: /Pacjent/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Pacjent pacjent = db.Pacjent.Find(id);
            ViewBag.AdresId = from a in db.Osoba where a.Adres.AdresID.Equals(id) select a;

            if (pacjent == null)
            {
                return HttpNotFound();
            }

            return View(pacjent);
        }

        // POST: /Pacjent/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OsobaID,AdresID,Imie,Nazwisko,Telefon,PESEL,NIP")] Pacjent pacjent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pacjent).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Message"] = "Edycja 'danych osobowych' pacjenta " + pacjent.ImieNazwisko + " zakończona powodzeniem.";
                return RedirectToAction("Index");
            }

            return View(pacjent);
        }

        // GET: /Pacjent/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pacjent pacjent = db.Pacjent.Find(id);
            if (pacjent == null)
            {
                return HttpNotFound();
            }
            return View(pacjent);
        }

        // POST: /Pacjent/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pacjent pacjent = db.Pacjent.Find(id);
            db.Osoba.Remove(pacjent);
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