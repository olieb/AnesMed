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
using Przuchodnia_Medyczna_Inz.ViewModel;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace Przuchodnia_Medyczna_Inz.Controllers
{
    public class PracownikController : Controller
    {
        private PrzychodniaContext db = new PrzychodniaContext();

        // GET: /Pracownik/Index
        public ActionResult Index(string stanowisko, string imieNazwisko = null, int? specjalizacjaId = null, string pesel = null)
        {
            var model = new PracownikIndexVM();

            model.Adresy = db.Adres;
            model.Specjalizacje = db.Specjalizacja;

            if (!String.IsNullOrEmpty(stanowisko))
            {
                model.Pracownicy = db.Pracownik.Where(x => x.Stanowisko.Nazwa.Equals(stanowisko)).OrderBy(p => p.Nazwisko).ToList();
            }

            if (!String.IsNullOrEmpty(imieNazwisko))
            {
                model.Pracownicy = model.Pracownicy.Where(s => s.ImieNazwisko.Contains(imieNazwisko)).ToList();
            }
            if(specjalizacjaId != null)
            {
                model.Pracownicy = model.Pracownicy.Where(s => s.Specjalizacja.SpecjalizacjaID == specjalizacjaId);
            }
            if (pesel != null)
            {
                model.Pracownicy = model.Pracownicy.Where(s => s.Pesel.ToString().Contains(pesel));
            }
            ViewBag.SpecjalizacjaID = new SelectList(db.Specjalizacja, "SpecjalizacjaID", "Nazwa");
            ViewBag.StanowiskoNazwa = stanowisko;
      
            return View(model);
        }


        // GET: /Pracownik/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pracownik pracownik = db.Pracownik.Find(id);
            if (pracownik == null)
            {
                return HttpNotFound();
            }
            return View(pracownik);
        }

        // GET: /Pracownik/Create
        public ActionResult Create(string stanowisko)
        {
            if (!String.IsNullOrEmpty(stanowisko))
            {
                ViewBag.StanowiskoNazwa = stanowisko;
            }
            ViewBag.StanowiskoID = new SelectList(db.Stanowisko, "StanowiskoID", "Nazwa", db.Stanowisko.Where(x => x.Nazwa.Equals(stanowisko)));
            ViewBag.SpecjalizacjaID = new SelectList(db.Specjalizacja, "SpecjalizacjaID", "Nazwa");
            ViewBag.PlacowkaMedycznaID = new SelectList(db.PlacowkaMedyczna, "PlacowkaMedycznaID", "Nazwa");

            return View();

        }
     
        // GET: /Pracownik/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pracownik pracownik = db.Pracownik.Find(id);
            if (pracownik == null)
            {
                return HttpNotFound();
            }
           // ViewBag.GrafikID = new SelectList(db.Grafik, "GrafikID", "GrafikID", pracownik.GrafikID);
            ViewBag.SpecjalizacjaID = new SelectList(db.Specjalizacja, "SpecjalizacjaID", "PracownikID", pracownik.SpecjalizacjaID);
           // ViewBag.ZatrudnienieID = new SelectList(db.Zatrudnienie, "ZatrudnienieID", "Uwagi", pracownik.ZatrudnienieID);
            return View(pracownik);
        }

        // POST: /Pracownik/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="OsobaID,AdresID,Imie,Nazwisko,Telefon,DataZatrudnienia,Pensja,StanowiskoID,SpecjalizacjaID,ZatrudnienieID,GrafikID")] Pracownik pracownik)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pracownik).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.GrafikID = new SelectList(db.Grafik, "GrafikID", "GrafikID", pracownik.GrafikID);
            //ViewBag.SpecjalizacjaID = new SelectList(db.Specjalizacja, "SpecjalizacjaID", "PracownikID", pracownik.SpecjalizacjaID);
            //ViewBag.StanowiskoID = new SelectList(db.Stanowisko, "StanowiskoID", "PracownikID", pracownik.StanowiskoID);
            //ViewBag.ZatrudnienieID = new SelectList(db.Zatrudnienie, "ZatrudnienieID", "Uwagi", pracownik.ZatrudnienieID);
            return View(pracownik);
        }

        // GET: /Pracownik/Delete/5
        public ActionResult Delete(string id, string stanowisko)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pracownik pracownik = db.Pracownik.Find(id);
            ViewBag.StanowiskoNazwa = stanowisko;
            if (pracownik == null)
            {
                return HttpNotFound();
            }
            return View(pracownik);
        }

        // POST: /Pracownik/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id, string stanowisko)
        {
            Pracownik pracownik = db.Pracownik.Find(id);
            db.Osoba.Remove(pracownik);
            db.SaveChanges();

            return RedirectToAction("Index", "Pracownik", new { stanowisko});
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
