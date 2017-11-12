using Przuchodnia_Medyczna_Inz.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Przuchodnia_Medyczna_Inz.ViewModel
{
    public class AdresyOsobVM
    {
        public IEnumerable<Pacjent> Pacjenci { get; set; }
        public IEnumerable<Adres> Adresy { get; set; }
        //Osoba-Pacjent
        public Pacjent Pacjent { get; set; }
        //Adres
        public Adres Adres { get; set; }
    }
}