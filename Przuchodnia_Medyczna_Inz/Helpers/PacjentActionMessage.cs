using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Przuchodnia_Medyczna_Inz.Helpers
{
    public enum PacjentActionMessage
    {
        [Description("Poprawnie dodano pacjenta ")]
        Create,
        [Description("Poprawnie edytowano dane pacjenta ")]
        Edit,
        [Description("Poprawnie usunięto pacjenta ")]
        Delete,
        [Description("Wystąpił nieoczekiwany błąd, operacja nie powiodła się. Spróbuj jeszcze raz lub skontakuj sie z Administratorem jeżeli problem będzie sie powtarzał.")]
        Error,
        Empty
    }
}