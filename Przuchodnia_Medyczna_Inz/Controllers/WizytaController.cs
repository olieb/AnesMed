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

        // GET: /Wizyta/Create
        public ActionResult Create()
        {
            var userId = User.Identity.GetUserId();
            var pacjent = db.Osoba.OfType<Pacjent>().Where(x => x.OsobaID.Equals(userId));
           
            ViewBag.PacjentID = pacjent.First().OsobaID;
            ViewBag.OsobaID = new SelectList(db.Osoba.OfType<Pacjent>(), "OsobaID", "ImieNazwisko");
           
            return View();
        }

        // POST: /Wizyta/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "WizytaID,Diagnoza,Uwagi,Data,Godzina,OsobaID,PacjentID")] Wizyta wizyta, string OsobaID, string PacjentID)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (OsobaID == null)
                    {
                        wizyta.PacjentID = PacjentID;
                    }
                    else
                    {
                        wizyta.PacjentID = OsobaID;
                    }

                    wizyta.Status = Status.Zaplanowana.ToString();
                    db.Wizyta.Add(wizyta);
                    
                    db.SaveChanges();
                    TempData["Success"] = "Pomyślnie zarezerwowano wizytę.";

                    return RedirectToAction("Index");
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
            }
            return View(wizyta);
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