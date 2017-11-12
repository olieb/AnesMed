using Przuchodnia_Medyczna_Inz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Przuchodnia_Medyczna_Inz.ViewModel
{
    public class PracownikVM
    {
        public IEnumerable<Pracownik> Pracownicy { get; set; }
        public IEnumerable<Adres> Adresy { get; set; }
        public Pracownik Pracownik { get; set; }
        public Adres Adres { get; set; }
        public Stanowisko Stanowisko { get; set; }
        public Zatrudnienie Zatrudnienie { get; set; }
        public Specjalizacja Specjalizacja { get; set; }
        public Grafik Grafik { get; set; }

    }
}