using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Przuchodnia_Medyczna_Inz.Models
{
    public class PacjentChorobaPrzewlekla
    {
        public PacjentChorobaPrzewlekla()
        {
            this.ChorobyPrzewlekle = new HashSet<ChorobaPrzewlekla>();
        }
        [Key, Required]
        public int PacjentChorobaID { get; set; }
        public string Opis { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DataWpisu { get; set; }
        public virtual Pacjent Pacjent { get; set; } // foreignkey Pacjenta
        public virtual ICollection<ChorobaPrzewlekla> ChorobyPrzewlekle { get; set; }
        public virtual Wizyta Wizyta { get; set; }     // foreignkey Wizyty
    }
}
