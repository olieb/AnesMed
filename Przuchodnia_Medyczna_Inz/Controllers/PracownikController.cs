using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
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
        public ActionResult Index(string stanowisko, string imieNazwisko, int? specjalizacjaId, string pesel, int page = 1)
        {
            List<Pracownik> pracownicy = db.Pracownik.ToList();

            if (!String.IsNullOrEmpty(stanowisko))
            {
                pracownicy = db.Pracownik.Where(x => x.Stanowisko.Nazwa.Equals(stanowisko)).OrderBy(p => p.Nazwisko).ToList();
            }

            if (!String.IsNullOrEmpty(imieNazwisko))
            {
                pracownicy = db.Pracownik.Where(s => s.ImieNazwisko.Contains(imieNazwisko)).ToList();
            }

            if(specjalizacjaId != null)
            {
                pracownicy = db.Pracownik.Where(s => s.Specjalizacja.SpecjalizacjaID == specjalizacjaId).ToList();
            }

            if (!String.IsNullOrEmpty(pesel))
            {
                pracownicy = db.Pracownik.Where(s => s.Pesel.ToString().Contains(pesel)).ToList();
            }

            ViewBag.SpecjalizacjaID = new SelectList(db.Specjalizacja, "SpecjalizacjaID", "Nazwa");
            ViewBag.StanowiskoNazwa = stanowisko;  
           
            int pageSize = 1;
            int pageNumber = 1;
            
            PagedList<Pracownik> model = new PagedList<Pracownik>(pracownicy.OrderBy(x => x.Nazwisko), page, pageSize);

            return View(model);
        }

        // GET: /Pracownik/Terminy/5
        public ActionResult Terminy(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pracownik pracownik = db.Pracownik.Find(id);
            pracownik.Wizyty = db.Wizyta.Where(x => x.Pracownik.OsobaID.Equals(id) && x.Status == Status.Wolna).ToList();


            ViewBag.StanowiskoNazwa = db.Stanowisko.Where(x => x.StanowiskoID == pracownik.StanowiskoID).FirstOrDefault().Nazwa;
            
            if (pracownik == null)
            {
                return HttpNotFound();
            }
            return View(pracownik);
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

            ViewBag.SpecjalizacjaID = new SelectList(db.Specjalizacja, "SpecjalizacjaID", "PracownikID", pracownik.SpecjalizacjaID);
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

            //ViewBag.SpecjalizacjaID = new SelectList(db.Specjalizacja, "SpecjalizacjaID", "PracownikID", pracownik.SpecjalizacjaID);
            //ViewBag.StanowiskoID = new SelectList(db.Stanowisko, "StanowiskoID", "PracownikID", pracownik.StanowiskoID);

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
