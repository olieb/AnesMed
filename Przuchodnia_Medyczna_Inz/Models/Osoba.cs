using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Przuchodnia_Medyczna_Inz.Models
{
    public abstract class Osoba
    {
        [Key, Required]
        public string OsobaID { get; set; }
        [Required]
        [StringLength(15)]
        [RegularExpression("^[A-ZĄĆĘŁŃÓŚŹŻ][a-ząćęłńóśźż]{2,}$", ErrorMessage = "Podane imie jest niepoprawne. Sprawdź czy zaczyna sie z wielkiej litery lub nie zawiera liczb")]
        public string Imie { get; set; }
        [Required]
        [StringLength(25)]
        [RegularExpression("^[A-ZĄĆĘŁŃÓŚŹŻ][a-ząćęłńóśźż]{2,}$", ErrorMessage = "Podane nazwisko jest niepoprawne. Sprawdź czy zaczyna sie z wielkiej litery lub nie zawiera liczb")]
        public string Nazwisko { get; set; }
        [DataType(DataType.PhoneNumber)]
        [DisplayFormat(DataFormatString = "{0:###-###-####}", ApplyFormatInEditMode = true)]
        [RegularExpression("^[0-9]+-?[0-9]+-?[0-9]+-?[0-9]+-?[0-9]*$", ErrorMessage = "Numer telefonu jest nie poprawny")]
        public string Telefon { get; set; }
        [Required]
        [RegularExpression("^[0-9]{11}$", ErrorMessage = "PESEL musi się skladać z 11 liczb np. 87041309234")]
        public long Pesel { get; set; }
        public virtual Adres Adres { get; set; }
        public string ImieNazwisko
        {
            get { return Imie + ' ' + Nazwisko; }
        }

    }
}