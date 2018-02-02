using PagedList;
using Przuchodnia_Medyczna_Inz.DAL;
using Przuchodnia_Medyczna_Inz.Helpers;
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
        public ActionResult Index(string imieNazwisko, string pesel, int page = 1, PacjentActionMessage akcja = PacjentActionMessage.Empty, string info = null)
        {
            var pacjenci= new AdresyOsobVM();

            pacjenci.Pacjenci = (from p in db.Pacjent select p).OrderBy(p => p.Nazwisko).ToList();
            pacjenci.Adresy = from a in db.Adres select a;

            if (!String.IsNullOrEmpty(imieNazwisko))
            {
                pacjenci.Pacjenci = pacjenci.Pacjenci.Where(s => s.ImieNazwisko.Contains(imieNazwisko)).ToList();
            }
            if (!String.IsNullOrEmpty(pesel))
            {
                pacjenci.Pacjenci = pacjenci.Pacjenci.Where(p => p.Pesel.ToString().Contains(pesel)).ToList();
            }

            if (akcja != PacjentActionMessage.Empty)
            {
                ViewBag.info = info;
                ViewBag.Akcja = akcja;
            }

            int pageSize = 10;
            int pageNumber = 1;

            PagedList<Pacjent> model = new PagedList<Pacjent>(pacjenci.Pacjenci.OrderBy(x => x.Nazwisko), page, pageSize);

            return View(model);
        }
        // GET: /Pacjent/Details/5
        public ActionResult Wizyty(string id)
        {
            if (String.IsNullOrWhiteSpace(id))
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
                    pacjent.Pesel = model.Pacjent.Pesel;
                    pacjent.NIP = model.Pacjent.NIP;
                    pacjent.Telefon = model.Pacjent.Telefon;

                    adres.Ulica = model.Adres.Ulica;
                    adres.NrBudynku = model.Adres.NrBudynku;
                    adres.NrMieszkania = model.Adres.NrMieszkania;
                    adres.KodPocztowy = model.Adres.KodPocztowy;
                    adres.Miejscowosc = model.Adres.Miejscowosc;

                    pacjent.Adres = adres;

                    db.Osoba.Add(pacjent);
                    db.SaveChanges();
                    return RedirectToAction("Index", new { akcja = PacjentActionMessage.Create, info = pacjent.ImieNazwisko });
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", new { akcja = PacjentActionMessage.Error, info = ex.ToString() });
                }
            }
            return View(model);
        }
        // GET: /Pacjent/Edit/5
        public ActionResult Edit(string id)
        {
            if (String.IsNullOrWhiteSpace(id))
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
        public ActionResult Edit([Bind(Include = "OsobaID,AdresID,Imie,Nazwisko,Telefon,Pesel,NIP")] Pacjent pacjent)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(pacjent).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", new { akcja = PacjentActionMessage.Edit, info = pacjent.ImieNazwisko });
                }
                catch(Exception ex)
                {
                    return RedirectToAction("Index", new { akcja = PacjentActionMessage.Error, info = ex.ToString() });
                }
            }

            return View(pacjent);
        }

        // GET: /Pacjent/Delete/5
        public ActionResult Delete(string id)
        {
            if (String.IsNullOrWhiteSpace(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pacjent pacjent = db.Pacjent.Find(id);
            if (pacjent == null)
            {
                return HttpNotFound();
            }
            return PartialView("_Delete",pacjent);
        }

        // POST: /Pacjent/Delete/5
        [HttpPost, ActionName("_Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
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