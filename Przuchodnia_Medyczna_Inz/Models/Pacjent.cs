using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Przuchodnia_Medyczna_Inz.Models
{
    public class Pacjent : Osoba
    {
        public Pacjent()
        {
            this.Wizyty = new HashSet<Wizyta>();
            this.ChorobyPacjenta = new HashSet<PacjentChorobaPrzewlekla>();
        }
        [Required]
        [RegularExpression(@"^((\d{3}[-]\d{3}[-]\d{2}[-]\d{2})|(\d{3}[-]\d{2}[-]\d{2}[-]\d{3}))$", ErrorMessage = "Niepoprawny format numeru NIP!")]
        public string NIP { get; set; }
        public string Informacje { get; set; }
        public virtual ICollection<Wizyta> Wizyty { get; set; }
        public virtual ICollection<PacjentChorobaPrzewlekla> ChorobyPacjenta { get; set; }
    }
}
