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
        [Display(Name = "Archiwum")]
        Archiwum = 2,
        [Display(Name = "Odbyta")]
        Odbyta = 3,
        [Display(Name = "Wolna")]
        Wolna = 4
    };
    public class Wizyta
    {
        public Wizyta()
        {
            this.ChorobyPacjenta = new HashSet<PacjentChorobaPrzewlekla>();
        }

        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WizytaID { get; set; }  // Primary key Wizyty    
        public Status Status { get; set; }
        [DataType(DataType.MultilineText)]
        public string Diagnoza { get; set; }
        [DataType(DataType.MultilineText)]
        public string Uwagi { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Required]
        public DateTime Data { get; set; }
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Time)]
        [Required]
        public DateTime Godzina { get; set; }
        public string PacjentID { get; set; }
        [ForeignKey("PacjentID")]
        public virtual Pacjent Pacjent { get; set; } // foreignkey Pacjenta

        public string PracownikID { get; set; }
        [ForeignKey("PracownikID")]
        public virtual Pracownik Pracownik { get; set; } //foreignkey Pracownika

        public virtual ICollection<PacjentChorobaPrzewlekla> ChorobyPacjenta { get; set; }
    }
}
