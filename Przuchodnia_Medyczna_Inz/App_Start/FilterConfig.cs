using System.Web;
using System.Web.Mvc;

namespace Przuchodnia_Medyczna_Inz
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
