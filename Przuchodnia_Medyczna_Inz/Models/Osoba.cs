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
        public string AdresID { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public string Telefon { get; set; }
        [ForeignKey("AdresID")]
        public virtual Adres Adres { get; set; }
        public string ImieNazwisko
        {
            get { return Imie + ' ' + Nazwisko; }

        }
       
    }
}