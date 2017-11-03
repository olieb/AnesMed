using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Przuchodnia_Medyczna_Inz.Models
{
    public class PlacowkaMedyczna
    {
        public PlacowkaMedyczna()
        {
            this.Zatrudnienia = new HashSet<Zatrudnienie>();
        }
        [Key, Required]
        [Column(Order = 0)]
        public int PlacowkaMedycznaID { get; set; }
        public string AdresID { get; set; }
        public string Nazwa { get; set; }
        [Display(Name = "Numer telefonu:")]
        public string Telefon { get; set; }
        [Display(Name = "Godziny otwarcia:")]
        public string GodzinyOtwarcia { get; set; }
        [ForeignKey("AdresID")]
        public virtual Adres Adres { get; set; } //foreignkey Adresu
        [InverseProperty("PlacowkaMedyczna")]
        public virtual ICollection<Adres> Adresy { get; set; }
        public virtual ICollection<Zatrudnienie> Zatrudnienia { get; set; }
    }
}
