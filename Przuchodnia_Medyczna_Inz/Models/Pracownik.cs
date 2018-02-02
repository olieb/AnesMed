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

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public double Pensja { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Data zatrudnienia")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataZatrudnienia { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Data zwolnienia")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<DateTime> DataZwolnienia { get; set; }
        public int? SpecjalizacjaID { get; set; }
        public int StanowiskoID { get; set; }
        public int PlacowkaID { get; set; }
        
        public virtual ICollection<Wizyta> Wizyty { get; set; }
        [ForeignKey("StanowiskoID")]
        public virtual Stanowisko Stanowiska { get; set; }
        [ForeignKey("SpecjalizacjaID")]
        public virtual Specjalizacja Specjalizacja { get; set; } //foreignkey Specjalizacji
        [ForeignKey("PlacowkaID")]
        public virtual PlacowkaMedyczna Placowka { get; set; }
    }
}
