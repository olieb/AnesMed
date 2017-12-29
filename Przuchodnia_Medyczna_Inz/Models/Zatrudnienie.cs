using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Przuchodnia_Medyczna_Inz.Models
{
    public class Zatrudnienie
    {
        [Required, Key]
        [Column(Order = 0)]
        public int ZatrudnienieID { get; set; }
        [Required]
        public string OsobaID { get; set; }
        [Required]
        public int PlacowkaID { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Data Zatrudnienia")]
        public DateTime DataZatrudnienia { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DataZwolnienia { get; set; }
        [DisplayName("Infomracje dodatkowe")]
        public string DodatkoweInformacje { get; set; }
        [ForeignKey("PlacowkaID")]
        public virtual PlacowkaMedyczna Placowka { get; set; }  //foreignkey Placownika
        [ForeignKey("OsobaID")]
        public virtual Pracownik Pracownik { get; set; }

    }
}
