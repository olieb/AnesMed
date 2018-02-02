using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Przuchodnia_Medyczna_Inz.Helpers
{
    public enum PracownikActionMessage
    {
        [Description("Poprawnie dodano pracownika ")]
        Create,
        [Description("Edycja danych powiodla się")]
        Edit,
        [Description("Zwolniono pracownika ")]
        Delete,
        Empty,
        [Description("Wystąpił nieoczekiwany błąd, operacja nie powiodła się. Spróbuj jeszcze raz lub skontakuj sie z Administratorem jeżeli problem będzie sie powtarzał. ")]
        Error
    }
}