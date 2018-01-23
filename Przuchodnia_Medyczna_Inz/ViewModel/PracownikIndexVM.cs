using Przuchodnia_Medyczna_Inz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Przuchodnia_Medyczna_Inz.ViewModel
{
    public class PracownikIndexVM
    {
        public IEnumerable<Pracownik> Pracownicy { get; set; }
        public IEnumerable<Adres> Adresy { get; set; }
        public IEnumerable<Specjalizacja> Specjalizacje { get; set; }
    }
}