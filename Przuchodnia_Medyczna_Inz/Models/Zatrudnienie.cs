using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Przuchodnia_Medyczna_Inz.Models
{
    public class Zatrudnienie
    {
        public Zatrudnienie()
        {
            this.Pracownicy = new HashSet<Pracownik>();
        }
        [Column(Order = 0)]
        public int ZatrudnienieID { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DataZatrudnienia { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DataZwolnienia { get; set; }
        public string Uwagi { get; set; }
        public virtual PlacowkaMedyczna Placowka { get; set; }  //foreignkey Placownika
        public virtual ICollection<Pracownik> Pracownicy { get; set; }

    }
}
