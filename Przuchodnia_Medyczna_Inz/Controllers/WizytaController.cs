using Przuchodnia_Medyczna_Inz.DAL;
using Przuchodnia_Medyczna_Inz.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.Validation;
using System.Diagnostics;
using PagedList;

namespace Przuchodnia_Medyczna_Inz.Controllers
{
    [Authorize]
    public class WizytaController : Controller
    {
        private PrzychodniaContext db = new PrzychodniaContext();

        // Post: /Wizyta/
        public ActionResult Index(string startDate, string endDate, string lekarz)
        {
            var userId = User.Identity.GetUserId();
            var wizyty = db.Wizyta.Where(x => x.Pacjent.OsobaID.Equals(userId));
            var liczbaWizyt = wizyty.Count();
            DateTime start;
            DateTime end;

            if (!String.IsNullOrEmpty(startDate))
            {
                start = DateTime.Parse(startDate);
                wizyty = wizyty.Where(x => x.Data >= start);
            }
            if (!String.IsNullOrEmpty(endDate))
            {
                end = DateTime.Parse(endDate);
                wizyty = wizyty.Where(x => x.Data <= end);
            }            
            TempData["startDate"] = startDate;
            TempData["endDate"] = endDate;
            ViewBag.Wizyty = liczbaWizyt;

            return View(wizyty.ToList());
        }

        // GET: /Wizyta/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Wizyta wizyta = db.Wizyta.Find(id);
            if (wizyta == null)
            {
                return HttpNotFound();
            }
            return View(wizyta);
        }

        //GET: /Wityta/CreateTermin
        public ActionResult CreateTermin(string id)
        {
            //var lekarz = db.Pracownik.Where(x => x.OsobaID.Equals(id));
            return View("_TerminCreate");
        }

        //POST
        [HttpPost]
        public ActionResult CreateTermin(DateTime data, DateTime godzina, string uwagi, string Id)
        {
            Wizyta wizyta = new Wizyta();

                try
                {
                    wizyta.Status = Status.Wolna;
                    wizyta.PracownikID = Id;
                    wizyta.Data = data;
                    wizyta.Godzina = godzina;
                    wizyta.Uwagi = uwagi;
                    db.Wizyta.Add(wizyta);
                    db.SaveChanges();
                    return RedirectToAction("Terminy", "Pracownik", new { id = Id });
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }
        }

        public ActionResult VisitList(string id, string startDate, string endDate)
        {
            var start = DateTime.Parse(startDate);
            var end = DateTime.Parse(endDate);
            var wizyty = db.Wizyta.Where(x => x.Data >= start && x.Data <= end && x.PracownikID.Equals(id) && x.Status == Status.Wolna).ToList();

            return PartialView("_VisitList", wizyty.ToList());
        }
        // GET: /Wizyta/Create
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Rezerwacja(int id)
        {
            var userId = User.Identity.GetUserId();
            var pacjent = db.Pacjent.Where(x => x.OsobaID.Equals(userId));

            Wizyta wizyta = db.Wizyta.Find(id);

            if (wizyta == null)
            {
                return HttpNotFound();
            }
            else
            {
                wizyta.PacjentID = userId;
                wizyta.Status = Status.Zaplanowana;
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public ActionResult GetLekarz(string lekarz)
        {
            List<SelectListItem> listaLekarzy = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(lekarz))
            {
                var lekarze = db.Pracownik.Where(x => x.Stanowisko.Nazwa.Equals("Lekarz") && x.Specjalizacja.Nazwa.Equals(lekarz)).ToList();
                lekarze.ForEach(x => 
                {
                    listaLekarzy.Add(new SelectListItem { Text = x.ImieNazwisko, Value = x.OsobaID });
                });
            }
            return Json(listaLekarzy, JsonRequestBehavior.AllowGet);
        }



        // GET: /Wizyta/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Wizyta wizyta = db.Wizyta.Find(id);

            if (wizyta == null)
            {
                return HttpNotFound();
            }         
           return PartialView("_NewDiagnoza", wizyta);
        }

        // POST: /Wizyta/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit(Wizyta wizyta)
        {
            if (ModelState.IsValid)
            {
                db.Entry(wizyta).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Terminy", "Pracownik", null);

        }

        // GET: /Wizyta/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Wizyta wizyta = db.Wizyta.Find(id);

            if (wizyta == null)
            {
                return HttpNotFound();
            }
            return View(wizyta);
        }

        // POST: /Wizyta/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Wizyta wizyta = db.Wizyta.Find(id);
            wizyta.PacjentID = null;
            wizyta.Status = Status.Wolna;
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