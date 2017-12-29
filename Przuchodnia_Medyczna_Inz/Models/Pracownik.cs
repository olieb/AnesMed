using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Przuchodnia_Medyczna_Inz.Models
{
    public class Pracownik : Osoba
    {
        public Pracownik()
        {
            this.Wizyty = new HashSet<Wizyta>();
            this.Zatrudnienie = new HashSet<Zatrudnienie>();
        }
        public double Pensja { get; set; }
        public int SpecjalizacjaID { get; set; }
        public int StanowiskoID { get; set; }
        public virtual ICollection<Wizyta> Wizyty { get; set; }
        [ForeignKey("StanowiskoID")]
        public Stanowisko Stanowisko { get; set; }
        [ForeignKey("SpecjalizacjaID")]
        public virtual Specjalizacja Specjalizacja { get; set; } //foreignkey Specjalizacji     
        public virtual ICollection<Zatrudnienie> Zatrudnienie { get; set; }      //foreignkey Zatrudnienia      
        public virtual ICollection<Grafik> Grafik { get; set; }         //foreignkey Grafiku
    }
}
