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

namespace Przuchodnia_Medyczna_Inz.Controllers
{
    public class PracownikController : Controller
    {
        private PrzychodniaContext db = new PrzychodniaContext();

        // GET: /Pracownik/Lekarz
        public ActionResult Lekarz()
        {
            var model = new PracownikVM();
            model.Pracownicy = db.Pracownik.Where(x => x.Stanowisko.Nazwa == StanowiskoNazwa.Lekarz);
            ViewBag.Selected = StanowiskoNazwa.Lekarz;
      
            return View(model);
        }

        // GET: /Pracownik/Recepcja
        public ActionResult Recepcja()
        {
            var model = new PracownikVM();
            model.Pracownicy = db.Pracownik.Where(x => x.Stanowisko.Nazwa == StanowiskoNazwa.Recepcja);
            ViewBag.Selected = StanowiskoNazwa.Recepcja;
            return View(model);
        }

        // GET: /Pracownik/Administrator
        public ActionResult Administrator()
        {
            var model = new PracownikVM();
            model.Pracownicy = db.Pracownik.Where(x => x.Stanowisko.Nazwa == StanowiskoNazwa.Administrator);
            ViewBag.Selected = StanowiskoNazwa.Administrator;
            return View(model);
        }
        // GET: /Pracownik/Pielegniarka
        public ActionResult Pielegniarka()
        {
            var model = new PracownikVM();
            model.Pracownicy = db.Pracownik.Where(x => x.Stanowisko.Nazwa == StanowiskoNazwa.Pielegniarka);
            ViewBag.Selected = StanowiskoNazwa.Pielegniarka;
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

        // GET: /Pracownik/NowyAdministrator
        public ActionResult NowyAdministrator()
        {
            var stanowiska  = from StanowiskoNazwa s in Enum.GetValues(typeof(StanowiskoNazwa)) select new {Id = s, Name = s.ToString()};
            ViewData["stanowiska"] = new SelectList(stanowiska, "Id", "Name", StanowiskoNazwa.Administrator.ToString());
            return View();
        }
        // GET: /Pracownik/NowyLekarz
        public ActionResult NowyLekarz()
        {
            var stanowiska = from StanowiskoNazwa s in Enum.GetValues(typeof(StanowiskoNazwa)) select new { Id = s, Name = s.ToString() };
            var specjalizacje = db.Specjalizacja.ToList();
            ViewData["stanowiska"] = new SelectList(stanowiska, "Id", "Name", StanowiskoNazwa.Lekarz.ToString());
            ViewData["specjalizacja"] = new SelectList(specjalizacje, "Id", "Name", null);
            return View();

        }

        // GET: /Pracownik/NowaPielegniarka
        public ActionResult NowaPielegniarka()
        {
            var stanowiska = from StanowiskoNazwa s in Enum.GetValues(typeof(StanowiskoNazwa)) select new { Id = s, Name = s.ToString() };
            ViewData["stanowiska"] = new SelectList(stanowiska, "Id", "Name", StanowiskoNazwa.Pielegniarka.ToString());
            return View();

        }

        // GET: /Pracownik/NowyRecepcja
        public ActionResult NowyRecepcja()
        {
            var stanowiska = from StanowiskoNazwa s in Enum.GetValues(typeof(StanowiskoNazwa)) select new { Id = s, Name = s.ToString() };
            ViewData["stanowiska"] = new SelectList(stanowiska, "Id", "Name", StanowiskoNazwa.Recepcja.ToString());
            return View();

        }

        // POST: /Pracownik/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="OsobaID,AdresID,Imie,Nazwisko,Telefon,DataZatrudnienia,Pensja,StanowiskoID,SpecjalizacjaID,ZatrudnienieID,GrafikID")] PracownikVM pracownik)
        {
            if (ModelState.IsValid)
            {
                //db.Osoba.Add(pracownik);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.GrafikID = new SelectList(db.Grafik, "GrafikID", "GrafikID", pracownik.GrafikID);
            //ViewBag.SpecjalizacjaID = new SelectList(db.Specjalizacja, "SpecjalizacjaID", "PracownikID", pracownik.SpecjalizacjaID);
            //ViewBag.StanowiskoID = new SelectList(db.Stanowisko, "StanowiskoID", "PracownikID", pracownik.StanowiskoID);
            //ViewBag.ZatrudnienieID = new SelectList(db.Zatrudnienie, "ZatrudnienieID", "Uwagi", pracownik.ZatrudnienieID);
            return View(pracownik);
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
            ViewBag.GrafikID = new SelectList(db.Grafik, "GrafikID", "GrafikID", pracownik.GrafikID);
            ViewBag.SpecjalizacjaID = new SelectList(db.Specjalizacja, "SpecjalizacjaID", "PracownikID", pracownik.SpecjalizacjaID);
            ViewBag.StanowiskoID = new SelectList(db.Stanowisko, "StanowiskoID", "PracownikID", pracownik.StanowiskoID);
            ViewBag.ZatrudnienieID = new SelectList(db.Zatrudnienie, "ZatrudnienieID", "Uwagi", pracownik.ZatrudnienieID);
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
            ViewBag.GrafikID = new SelectList(db.Grafik, "GrafikID", "GrafikID", pracownik.GrafikID);
            ViewBag.SpecjalizacjaID = new SelectList(db.Specjalizacja, "SpecjalizacjaID", "PracownikID", pracownik.SpecjalizacjaID);
            ViewBag.StanowiskoID = new SelectList(db.Stanowisko, "StanowiskoID", "PracownikID", pracownik.StanowiskoID);
            ViewBag.ZatrudnienieID = new SelectList(db.Zatrudnienie, "ZatrudnienieID", "Uwagi", pracownik.ZatrudnienieID);
            return View(pracownik);
        }

        // GET: /Pracownik/Delete/5
        public ActionResult Delete(string id)
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

        // POST: /Pracownik/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Pracownik pracownik = db.Pracownik.Find(id);
            db.Osoba.Remove(pracownik);
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
