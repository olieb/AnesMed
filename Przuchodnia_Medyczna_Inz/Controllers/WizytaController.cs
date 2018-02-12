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
using Przuchodnia_Medyczna_Inz.Helpers;

namespace Przuchodnia_Medyczna_Inz.Controllers
{
    public class WizytaController : Controller
    {
        private PrzychodniaContext db = new PrzychodniaContext();

        // Post: /Wizyta/
       [Authorize]
        public ActionResult Index(string startDate, string status, string endDate, string lekarz, ActionMessage akcja = ActionMessage.Empty, string info = null, string data = null)
        {
            List<Wizyta> wizyty;
            var userId = User.Identity.GetUserId();

            if(User.IsInRole("Pacjent") && !String.IsNullOrEmpty(status)){
                var stat = (Status)Enum.Parse(typeof(Status), status);
                wizyty = db.Wizyta.Where(x => x.Pacjent.OsobaID.Equals(userId) && x.Status == stat).ToList();
            }
            else if (User.IsInRole("Pacjent"))
            {
                wizyty = db.Wizyta.Where(x => x.Pacjent.OsobaID.Equals(userId) && x.Status == Status.Zaplanowana).ToList();
            }
            else
            {
                wizyty = db.Wizyta.Where(x =>x.PacjentID != null).ToList();
            }
            var liczbaWizyt = wizyty.Count();
            DateTime start;
            DateTime end;

            if (!String.IsNullOrEmpty(startDate))
            {
                start = DateTime.Parse(startDate);
                wizyty = wizyty.Where(x => x.Data >= start).ToList();
            }
            if (!String.IsNullOrEmpty(endDate))
            {
                end = DateTime.Parse(endDate);
                wizyty = wizyty.Where(x => x.Data <= end).ToList();
            }            
            TempData["startDate"] = startDate;
            TempData["endDate"] = endDate;
            ViewBag.Wizyty = liczbaWizyt;

            if (akcja != ActionMessage.Empty)
            {
                ViewBag.data = data;
                ViewBag.info = info;
                ViewBag.Akcja = akcja;
            }

            return View(wizyty);
        }

        //POST
        [HttpPost]
        [Authorize(Roles = "Lekarz")]
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
                catch (Exception ex)
                {
                    return RedirectToAction("Terminy", "Pracownik", new { akcja = PracownikActionMessage.Error, info = ex.ToString()});
                }
        }

        [HttpPost]
        [Authorize(Roles = "Lekarz")]
        public ActionResult Diagnoza(string diagnoza, string Id)
        {
            var wizytaId = Int32.Parse(Id);
            
                Wizyta wizyta = db.Wizyta.Where(x => x.WizytaID == wizytaId).First();

                wizyta.Status = Status.Odbyta;
                wizyta.Diagnoza = diagnoza;
                db.SaveChanges();
                return RedirectToAction("MojeWizyty", "Pracownik");
        }
        public ActionResult VisitList(string id, string startDate, string endDate)
        {
            var start = DateTime.ParseExact(startDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var end = DateTime.ParseExact(endDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var wizyty = db.Wizyta.Where(x => x.Data >= start && x.Data <= end && x.PracownikID.Equals(id) && x.Status == Status.Wolna).ToList();

            return PartialView("_VisitList", wizyty.ToList());
        }
        // GET: /Wizyta/Create
        public ActionResult Create()
        {
            return View();
        }

         [Authorize(Roles = "Recepcja, Pacjent")]
        public ActionResult Rezerwacja(int id)
        {
            var userId = User.Identity.GetUserId();
            var pacjent = db.Pacjent.Where(x => x.OsobaID.Equals(userId));

            Wizyta wizyta = db.Wizyta.Find(id);
            try
            {
                if (wizyta != null)
                {
                    wizyta.PacjentID = userId;
                    wizyta.Status = Status.Zaplanowana;
                    db.SaveChanges();

                    return RedirectToAction("Index", new { akcja = ActionMessage.Created, info = wizyta.Pracownik.ImieNazwisko, data = wizyta.Data });
                }
                else
                {
                    return HttpNotFound();
                }
            }
            catch(Exception ex)
            {
                return RedirectToAction("Index", new { akcja = ActionMessage.Error });
                ex.ToString();
            }
        }

        public ActionResult GetLekarz(string lekarz)
        {
            List<SelectListItem> listaLekarzy = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(lekarz))
            {
                var lekarze = db.Pracownik.Where(x => x.Stanowiska.Nazwa.Equals("Lekarz") && x.Specjalizacja.Nazwa.Equals(lekarz)).ToList();
                lekarze.ForEach(x => 
                {
                    listaLekarzy.Add(new SelectListItem { Text = x.ImieNazwisko, Value = x.OsobaID });
                });
            }
            return Json(listaLekarzy, JsonRequestBehavior.AllowGet);
        }
        
        // GET: /Wizyta/Edit/5
         [Authorize(Roles = "Lekarz")]
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
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(wizyta).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Terminy", "Pracownik", new { akcja = ActionMessage.Edited, info = wizyta.Pracownik.ImieNazwisko });
            }
            catch(Exception ex)
            {
                return RedirectToAction("Terminy","Pracownik", new { akcja = ActionMessage.Error });
                ex.ToString();
            }
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

            return PartialView("_Delete", wizyta);
        }

        // POST: /Wizyta/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id, string pacjentId)
        {
            try
            {
                
                Wizyta wizyta = db.Wizyta.Find(id);

                if (wizyta.Status == Status.Odbyta || wizyta.Data < DateTime.Today)
                {
                    wizyta.Status = Status.Archiwum;
                    db.SaveChanges();

                    if (String.IsNullOrEmpty(pacjentId))
                    {
                        return RedirectToAction("Index", new { akcja = "Wizyta została usunięta z twojej listy"});
                    }

                }
                else
                {
                    wizyta.PacjentID = null;
                    wizyta.Status = Status.Wolna;
                    db.SaveChanges();

                    if (String.IsNullOrEmpty(pacjentId))
                    {
                        return RedirectToAction("Index", new { akcja = ActionMessage.Deleted, info = wizyta.Pracownik.ImieNazwisko, data = wizyta.Data });
                    }
                }
                return RedirectToAction("Wizyty", "Pacjent", new { id = pacjentId});
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", new { akcja = ActionMessage.Error});
                ex.ToString();
            }
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