using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Przuchodnia_Medyczna_Inz.Models
{
    public class Adres
    {
        [ForeignKey("Osoba")]
        public string AdresID { get; set; }
        public int? PlacowkaID { get; set; }
        public string Miejscowosc { get; set; }
        public string Ulica { get; set; }
        [Display(Name = "Budynek nr.")]
        public int NrBudynku { get; set; }
        [Display(Name = "Lokal nr.")]
        public int? NrMieszkania { get; set; }
        [RegularExpression(@"^\d{2}-\d{3}$", ErrorMessage = "Kod pocztowy ma niepoprawny format!")]
        public string KodPocztowy { get; set; }
        [ForeignKey("PlacowkaID")]
        public virtual PlacowkaMedyczna PlacowkaMedyczna { get; set; }
        public virtual Osoba Osoba { get; set; }

        public string FullAdres
        {
            get
            {
                return Ulica + ' ' + NrBudynku + '/' + NrMieszkania + ", " + KodPocztowy + ' ' + Miejscowosc;
            }
        }
    }
}