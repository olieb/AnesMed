using Microsoft.AspNet.Identity;
using PagedList;
using Przuchodnia_Medyczna_Inz.DAL;
using Przuchodnia_Medyczna_Inz.Helpers;
using Przuchodnia_Medyczna_Inz.Models;
using Przuchodnia_Medyczna_Inz.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Przuchodnia_Medyczna_Inz.Controllers
{
    public class PracownikController : Controller
    {
        private PrzychodniaContext db = new PrzychodniaContext();

        // GET: /Pracownik/Index
        [Authorize]
        public ActionResult Index(string stanowisko, string imieNazwisko, int? specjalizacjaId, string pesel, int page = 1, PracownikActionMessage akcja = PracownikActionMessage.Empty, string info = null, string data = null)
        {
            List<Pracownik> pracownicy = db.Pracownik.ToList(); 
            if (!String.IsNullOrEmpty(stanowisko))
            {
                pracownicy = db.Pracownik.Where(x => x.Stanowiska.Nazwa.Equals(stanowisko)).OrderBy(p => p.Nazwisko).ToList();
            }

            if (!String.IsNullOrEmpty(imieNazwisko))
            {
                pracownicy = db.Pracownik.Where(s => s.Imie.Contains(imieNazwisko) || s.Nazwisko.Contains(imieNazwisko)).ToList();
            }

            if(specjalizacjaId != null)
            {
                pracownicy = db.Pracownik.Where(s => s.Specjalizacja.SpecjalizacjaID == specjalizacjaId).ToList();
            }

            if (!String.IsNullOrEmpty(pesel))
            {
                var longPesel = Convert.ToInt64(pesel);
                pracownicy = db.Pracownik.Where(s => s.Pesel == longPesel).ToList();
            }

            if (akcja != PracownikActionMessage.Empty)
            {
                ViewBag.data = data;
                ViewBag.info = info;
                ViewBag.Akcja = akcja;
            }
            ViewBag.SpecjalizacjaID = new SelectList(db.Specjalizacja, "SpecjalizacjaID", "Nazwa");
            ViewBag.StanowiskoNazwa = stanowisko;  
           
            int pageSize = 10;
            int pageNumber = 1;
            
            PagedList<Pracownik> model = new PagedList<Pracownik>(pracownicy.Where(x =>x.DataZwolnienia == null).OrderBy(x => x.Nazwisko), page, pageSize);

            return View(model);
        }
        
        // GET: /Pracownik/Terminy/id
       
        public ActionResult Terminy(string id , string akcja)
        {

            if (String.IsNullOrEmpty(id))
            {
                return RedirectToAction("MojeWizyty");
            }

            Pracownik pracownik = db.Pracownik.Find(id);
            ViewBag.StanowiskoNazwa = db.Stanowisko.Where(x => x.StanowiskoID == pracownik.StanowiskoID).FirstOrDefault().Nazwa;
            ViewBag.LekarzID = id;
            ViewBag.Akcja = akcja;

            if (pracownik == null)
            {
                return HttpNotFound();
            }
            return View(pracownik);
        }

        [Authorize(Roles = "Lekarz")]
        public ActionResult MojeWizyty(){
          
           var userId = User.Identity.GetUserId();
           var test = User.Identity.IsAuthenticated;
           var test1 = User.IsInRole("Pracownik");

            if (String.IsNullOrEmpty(userId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

           var pracownik = db.Pracownik.Find(userId);
           ViewBag.StanowiskoNazwa = db.Stanowisko.Where(x => x.StanowiskoID == pracownik.StanowiskoID).FirstOrDefault().Nazwa;
           ViewBag.LekarzID = userId;

           if (pracownik == null)
           {
               return HttpNotFound();
           }
           return View("Terminy", pracownik);
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
            ViewBag.StanowiskoNazwa = db.Stanowisko.Where(x => x.StanowiskoID == pracownik.StanowiskoID).FirstOrDefault().Nazwa;
            ViewBag.StanowiskoID = new SelectList(db.Stanowisko, "StanowiskoID", "Nazwa");
            ViewBag.SpecjalizacjaID = new SelectList(db.Specjalizacja, "SpecjalizacjaID", "Nazwa", pracownik.SpecjalizacjaID);
            return View(pracownik);
        }

        // POST: /Pracownik/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Pracownik model, DateTime date, string stanowisko)
        {           
            if (ModelState.IsValid)
            {
                try
                {
                    //model.StanowiskoID = stanId;
                    model.DataZatrudnienia = date;
                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", new { stanowisko = stanowisko, akcja = PracownikActionMessage.Edit, info = model.ImieNazwisko });
                }
                catch (Exception ex)
                {
                    
                }
            }
           
            return View(model);
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
            return PartialView("_Delete", pracownik);
        }
        // POST: /Pracownik/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id, string stanowisko)
        {
            Pracownik pracownik = db.Pracownik.Find(id);
            var wizyty = db.Wizyta.Where(x => x.PracownikID.Equals(id)).ToList();
            pracownik.DataZwolnienia = DateTime.Now;
            db.SaveChanges();       

            return RedirectToAction("Index", "Pracownik", new { stanowisko , akcja = PracownikActionMessage.Delete, info = pracownik.ImieNazwisko});
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
