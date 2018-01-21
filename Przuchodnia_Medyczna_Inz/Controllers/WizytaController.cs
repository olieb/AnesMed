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
        public ActionResult Index(string startDate, string endDate)
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

            return View("TerminCreate");
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTermin(Wizyta wizyta, string Id)
        {
           wizyta.PracownikID = Id; 
       
            if (ModelState.IsValid)
            {
                try
                {
                    wizyta.Status = Status.Wolna;
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
            return View("TerminCreate", wizyta);
        }
        
        public ActionResult VisitList(string id, string startDate, string endDate)
        {

            if (!String.IsNullOrEmpty(id))
            {
                var wizyty = db.Wizyta.Where(x => x.PracownikID.Equals(id) && x.Status == Status.Wolna).ToList();

                if (!String.IsNullOrEmpty(startDate) || !String.IsNullOrEmpty(endDate))
                {
                    var start = DateTime.Parse(startDate);
                    var end = DateTime.Parse(endDate);
                    wizyty = db.Wizyta.Where(x => x.Data >= start && x.Data <= end).ToList();
                }

                //int pageSize = 1;
                //int pageNumber = 1;

                //PagedList<Wizyta> model = new PagedList<Wizyta>(wizyty.OrderBy(x => x.Godzina), page, pageSize);

                return PartialView("_VisitList", wizyty.ToList());
            }
            else
            {
                return HttpNotFound();
            }
        }
        // GET: /Wizyta/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Wizyta/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]        
        //public ActionResult Create([Bind(Include = "WizytaID,Diagnoza,Uwagi,Data,Godzina,OsobaID,PacjentID")] Wizyta wizyta, string OsobaID, string PacjentImie, string PacjentNazwisko)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            if (OsobaID == null)
        //            {
        //                wizyta.Pacjent.Imie = PacjentImie;
        //                wizyta.Pacjent.Nazwisko = PacjentNazwisko;
        //            }
        //            else
        //            {
        //                wizyta.PacjentID = OsobaID;
        //            }

        //            wizyta.Status = Status.Zaplanowana.ToString();
        //            db.Wizyta.Add(wizyta);
                    
        //            db.SaveChanges();
        //            TempData["Success"] = "Pomyślnie zarezerwowano wizytę.";

        //            return RedirectToAction("Index");
        //        }
        //        catch (DbEntityValidationException dbEx)
        //        {
        //            foreach (var validationErrors in dbEx.EntityValidationErrors)
        //            {
        //                foreach (var validationError in validationErrors.ValidationErrors)
        //                {
        //                    Trace.TraceInformation("Property: {0} Error: {1}",
        //                            validationError.PropertyName,
        //                            validationError.ErrorMessage);
        //                }
        //            }
        //        }
        //    }
        //    return View(wizyta);
        //}

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
            return View(wizyta);
        }

        // POST: /Wizyta/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "WizytaID,Diagnoza,Uwagi,Data,Godzina")] Wizyta wizyta)
        {
            if (ModelState.IsValid)
            {
                db.Entry(wizyta).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(wizyta);
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
            db.Wizyta.Remove(wizyta);
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