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
            this.Pracownicy = new HashSet<Pracownik>();
        }
        [Key, Required]
        [Column(Order = 0)]
        public int PlacowkaMedycznaID { get; set; }
        [Required]
        [StringLength(15)]
        public string Nazwa { get; set; }
        [Required]
        [Display(Name = "Kom.")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression("^[0-9]+-?[0-9]+-?[0-9]+-?[0-9]+-?[0-9]*$", ErrorMessage = "Numer telefonu jest nie poprawny")]
        public string Komorka { get; set; }
        [Display(Name="Tel. ")]
        public string Telefon { get; set; }
        [Display(Name = "Godziny otwarcia")]
        public string GodzinyOtwarcia { get; set; }
        public virtual Adres Adres { get; set; } //foreignkey Adresu
        public virtual ICollection<Pracownik> Pracownicy { get; set; }
    }
}
