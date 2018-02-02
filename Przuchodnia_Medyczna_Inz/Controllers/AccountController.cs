using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Przuchodnia_Medyczna_Inz.Models;
using Przuchodnia_Medyczna_Inz.DAL;
using Przuchodnia_Medyczna_Inz.Helpers;

namespace Przuchodnia_Medyczna_Inz.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        public AccountController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new PrzychodniaContext())))
        {
        }

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindAsync(model.UserName, model.Password);
                if (user != null)
                {
                    await SignInAsync(user, model.RememberMe);
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/ProfilPacjenta
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ProfilPacjenta(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindAsync(model.UserName, model.Password);
                if (user != null)
                {
                    await SignInAsync(user, model.RememberMe);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Niepoprawny Login lub Hasło.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
              PrzychodniaContext db = new PrzychodniaContext();

               var currentUser = User.Identity.GetUserId();
              
              
                var user = new ApplicationUser() { UserName = model.UserName };

              model.AdresyOsob.Pacjent.OsobaID = user.Id;
              ModelState["AdresyOsob.Pacjent.OsobaID"].Errors.Clear();

            if (ModelState.IsValid)
            {
                 var pacjent = new Pacjent();
                 var address = new Adres()
                {
                    Ulica = model.AdresyOsob.Adres.Ulica,
                    NrBudynku = model.AdresyOsob.Adres.NrBudynku,
                    NrMieszkania = model.AdresyOsob.Adres.NrMieszkania,
                    KodPocztowy = model.AdresyOsob.Adres.KodPocztowy,
                    Miejscowosc = model.AdresyOsob.Adres.Miejscowosc,
                };
                 pacjent.OsobaID = user.Id;
                 pacjent.Imie = model.AdresyOsob.Pacjent.Imie;
                 pacjent.Nazwisko = model.AdresyOsob.Pacjent.Nazwisko;
                 pacjent.Pesel = model.AdresyOsob.Pacjent.Pesel;
                 pacjent.NIP = model.AdresyOsob.Pacjent.NIP;
                 pacjent.Telefon = model.AdresyOsob.Pacjent.Telefon;
                 pacjent.Adres = address;               

                var result = await UserManager.CreateAsync(user, model.Password);

               // db.Adres.Add(address);
                db.Osoba.Add(pacjent);
                db.SaveChanges();   

                if (result.Succeeded)
                {
                    ViewBag.Id = currentUser;
                    result = UserManager.AddToRole(user.Id, "Pacjent");
                    if (currentUser == null)
                    {
                        await SignInAsync(user, isPersistent: false);
                    }
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    AddErrors(result);
                }
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterPracownik(RegisterPracownikViewModel model, int placowkaMedycznaId, int? specjalizacjaId, int stanowiskoId)
        {
            PrzychodniaContext db = new PrzychodniaContext();
            var user = new ApplicationUser() { UserName = model.UserName };

            model.Pracownicy.Pracownik.OsobaID = user.Id;
            ModelState["Pracownicy.Pracownik.OsobaID"].Errors.Clear();

            if (ModelState.IsValid)
            {
                var pracownik = new Pracownik();
                var address = new Adres()
                {
                    Ulica = model.Pracownicy.Adres.Ulica,
                    NrBudynku = model.Pracownicy.Adres.NrBudynku,
                    NrMieszkania = model.Pracownicy.Adres.NrMieszkania,
                    KodPocztowy = model.Pracownicy.Adres.KodPocztowy,
                    Miejscowosc = model.Pracownicy.Adres.Miejscowosc,
                };

                pracownik.OsobaID = user.Id;
                pracownik.Imie = model.Pracownicy.Pracownik.Imie;
                pracownik.Nazwisko = model.Pracownicy.Pracownik.Nazwisko;
                pracownik.Pesel = model.Pracownicy.Pracownik.Pesel;
                pracownik.Pensja = model.Pracownicy.Pracownik.Pensja;
                pracownik.Telefon = model.Pracownicy.Pracownik.Telefon;
                pracownik.DataZatrudnienia = model.Pracownicy.Pracownik.DataZatrudnienia;
                pracownik.DataZwolnienia = model.Pracownicy.Pracownik.DataZwolnienia;

                pracownik.Adres = address;
                pracownik.SpecjalizacjaID = specjalizacjaId;
                pracownik.StanowiskoID = stanowiskoId;
                pracownik.PlacowkaID = placowkaMedycznaId;
               
                var result = await UserManager.CreateAsync(user, model.Password);

                db.Osoba.Add(pracownik);

                db.SaveChanges();
                ViewBag.Id = user.Id;
                if (result.Succeeded)
                {
                    var stanowisko = db.Stanowisko.Where(x => x.StanowiskoID == stanowiskoId).First();
                    ViewBag.StanowiskoNazwa = stanowisko.Nazwa;
                    if(stanowisko.Nazwa.Equals("Lekarz"))
                    {
                        result = UserManager.AddToRole(user.Id, "Lekarz");
                    }
                    if (stanowisko.Nazwa.Equals("Administrator"))
                    {
                        result = UserManager.AddToRole(user.Id, "Administrator");
                    }
                    if (stanowisko.Nazwa.Equals("Recepcjonista"))
                    {
                        result = UserManager.AddToRole(user.Id, "Recepcja");
                    }
                    //await SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Pracownik", new { stanowisko = @ViewBag.StanowiskoNazwa, akcja = PracownikActionMessage.Create, info = pracownik.ImieNazwisko });
                }
                else
                {
                    AddErrors(result);
                }
            }
            // If we got this far, something failed, redisplay form
            return RedirectToAction("Index", "Pracownik", new { stanowisko = @ViewBag.StanowiskoNazwa, akcja = PracownikActionMessage.Error });
        }
        //
        // POST: /Account/Disassociate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        {
            ManageMessageId? message = null;
            IdentityResult result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("Manage", new { Message = message });
        }

        // GET: /Account/ProfilPacjenta
        [AllowAnonymous]
        public ActionResult ProfilPacjenta(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

       
        //
        // GET: /Account/Manage
        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Twoje hasło zostało zmienione."
                : message == ManageMessageId.SetPasswordSuccess ? "Twoje hasło zostało ustawione."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "Wystąpił błąd. Skontaktuj sie z administratorem."
                : "";
            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(ManageUserViewModel model)
        {
            bool hasPassword = HasPassword();
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasPassword)
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }
            else
            {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        //[AllowAnonymous]
        //public ActionResult ExternalLoginFailure()
        //{
        //    return View();
        //}

        [ChildActionOnly]
        public ActionResult RemoveAccountList()
        {
            var linkedAccounts = UserManager.GetLogins(User.Identity.GetUserId());
            ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
            return (ActionResult)PartialView("_RemoveAccountPartial", linkedAccounts);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        //private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri) : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

        }
        #endregion
    }
}