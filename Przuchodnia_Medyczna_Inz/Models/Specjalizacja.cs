using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Przuchodnia_Medyczna_Inz.Models
{
    public class Specjalizacja
    {
        [Key, Required]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SpecjalizacjaID { get; set; }
        public string PracownikID { get; set; }
        public string Nazwa { get; set; }
        [ForeignKey("PracownikID")]
        public virtual Pracownik Pracownik { get; set; }//foreignkey Pracownika
        [InverseProperty("Specjalizacja")]
        public virtual ICollection<Pracownik> Pracownicy { get; set; }
    }
}
