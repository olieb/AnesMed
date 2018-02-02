using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Przuchodnia_Medyczna_Inz.Helpers
{
    public enum ActionMessage
    {
        [Description("Zarezerwowano wizytę u lekarza: ")]
        Created,
        [Description("Anulowano wizytę do lekarza:  ")]
        Deleted,
        [Description("Edytowano termin wizyty ")]
        Edited,
        [Description("Wystąpił nieoczekiwany błąd, operacja nie powiodła się. Spróbuj jeszcze raz lub skontakuj sie z Administratorem jeżeli problem będzie sie powtarzał. ")]
        Error,
        Empty
    }
}