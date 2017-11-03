using Przuchodnia_Medyczna_Inz.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Przuchodnia_Medyczna_Inz.ViewModel
{
    public class AdresyOsobVM
    {
        public IEnumerable<Pacjent> Pacjenci { get; set; }
        public IEnumerable<Adres> Adresy { get; set; }
        //Osoba-Pacjent
        public string OsobaID { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Niepoprawny format numeru PESEL!")] // walidacja numeru PESEL
        public long PESEL { get; set; }
        [RegularExpression(@"^((\d{3}[-]\d{3}[-]\d{2}[-]\d{2})|(\d{3}[-]\d{2}[-]\d{2}[-]\d{3}))$", ErrorMessage = "Niepoprawny format numeru NIP!")]
        public string NIP { get; set; }
        public string Telefon { get; set; }
        public string AdresID { get; set; }
        //Adres
        public string Miejscowosc { get; set; }
        public string Ulica { get; set; }
        [Display(Name = "Budynek nr.")]
        public int NrBudynku { get; set; }
        [Display(Name = "Lokal nr.")]
        public int NrMieszkania { get; set; }
        public string KodPocztowy { get; set; }

    }
}