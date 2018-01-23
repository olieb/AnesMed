using Przuchodnia_Medyczna_Inz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Przuchodnia_Medyczna_Inz.ViewModel
{
    public class PracownikCreateVM
    {
        public Pracownik Pracownik { get; set; }
        public Adres Adres { get; set; }
    }
}