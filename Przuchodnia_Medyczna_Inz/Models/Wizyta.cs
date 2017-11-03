using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Przuchodnia_Medyczna_Inz.Models
{
    public enum Status
    {
        [Display(Name = "Zaplanowana")]
        Zaplanowana = 1,
        [Display(Name = "Anulowana")]
        Anulowana = 2,
        [Display(Name = "Odbyta")]
        Odbyta = 3
    };
    public class Wizyta
    {
        public Wizyta()
        {
            this.ChorobyPacjenta = new HashSet<PacjentChorobaPrzewlekla>();
        }

        [Key]
        public int WizytaID { get; set; }  // Primary key Wizyty    
        public string Status { get; set; }
        [DataType(DataType.MultilineText)]
        public string Diagnoza { get; set; }
        [DataType(DataType.MultilineText)]
        public string Uwagi { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime Data { get; set; }
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Time)]
        public DateTime Godzina { get; set; }
        public string PacjentID { get; set; }
        [ForeignKey("PacjentID")]
        public virtual Pacjent Pacjent { get; set; } // foreignkey Pacjenta
        public virtual Pracownik Pracownik { get; set; } //foreignkey Pracownika

        public virtual ICollection<PacjentChorobaPrzewlekla> ChorobyPacjenta { get; set; }
    }
}
