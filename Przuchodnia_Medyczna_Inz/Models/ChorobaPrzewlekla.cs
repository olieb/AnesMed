using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Przuchodnia_Medyczna_Inz.Models
{
    public class ChorobaPrzewlekla
    {
        public ChorobaPrzewlekla()
        {

        }

        [Key, Required]
        public int ChorobaPrzewleklaID { get; set; }
        [Required]
        [Display(Name = "Nazwa choroby")]
        [StringLength(30, ErrorMessage = "Za długa nazwa choroby.")]
        public string Nazwa { get; set; }
        [StringLength(255, ErrorMessage = "Pole \"opis\" jest zbyt długie. ")]
        public string Opis { get; set; }
        public virtual PacjentChorobaPrzewlekla PacjenciChorobyPrzewlekle { get; set; }
    }
}