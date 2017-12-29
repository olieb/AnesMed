using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Przuchodnia_Medyczna_Inz.Models
{
    public class Stanowisko
    {
        [Required, Key]
        public int StanowiskoID { get; set; }
        [StringLength(20, ErrorMessage = "Nazwa musi mieć dlugość, między {2} a {1} znaków.", MinimumLength = 5)]
        public string Nazwa { get; set; }
        public string Obowiazki { get; set; }
        public ICollection<Pracownik> Pracownicy { get; set; }

    }
}
