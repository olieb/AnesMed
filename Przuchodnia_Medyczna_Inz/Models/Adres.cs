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
        [Key, Required]
        public int AdresID { get; set; }
        [Required]
        [StringLength(25)]
        public string Miejscowosc { get; set; }
        [StringLength(25)]
        [Required]
        public string Ulica { get; set; }
        [Display(Name = "Budynek nr.")]
        [RegularExpression(@"^[0-9\.\-\+]+$", ErrorMessage="Niepoprawny numer Budynku")]
        public int NrBudynku { get; set; }
        [Display(Name = "Lokal nr.")]
        [RegularExpression(@"^[0-9\.\-\+]+$", ErrorMessage = "Niepoprawny numer Budynku")]
        public int? NrMieszkania { get; set; }
        [RegularExpression(@"^\d{2}-\d{3}$", ErrorMessage = "Kod pocztowy ma niepoprawny format!")]
        public string KodPocztowy { get; set; }
        public virtual PlacowkaMedyczna PlacowkaMedyczna { get; set; }
        public ICollection<Osoba> Osoby { get; set; }

        public string FullAdres
        {
            get
            {
                if (NrMieszkania.HasValue)
                {
                    return Ulica+' '+NrBudynku+'/'+NrMieszkania+ ", "+KodPocztowy+' '+Miejscowosc;
                }
                else
                {
                    return Ulica+ ' '+NrBudynku+", "+KodPocztowy+' '+Miejscowosc;
                }
            }
        }
    }
}