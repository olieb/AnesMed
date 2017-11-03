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
        }
        [Display(Name = "Data zatrudnienia")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DataZatrudnienia { get; set; }
        public double Pensja { get; set; }
        public virtual ICollection<Wizyta> Wizyty { get; set; }
        public int StanowiskoID { get; set; }
        [ForeignKey("StanowiskoID")]
        public virtual Stanowisko Stanowisko { get; set; } // foreignkey Stanowiska
        public int SpecjalizacjaID { get; set; }
        [ForeignKey("SpecjalizacjaID")]
        public virtual Specjalizacja Specjalizacja { get; set; } //foreignkey Specjalizacji     
        public int ZatrudnienieID { get; set; }
        [ForeignKey("ZatrudnienieID")]
        public virtual Zatrudnienie Zatrudnienie { get; set; }      //foreignkey Zatrudnienia      
        public int GrafikID { get; set; }
        [ForeignKey("GrafikID")]
        public virtual Grafik Grafik { get; set; }         //foreignkey Grafiku
    }
}
