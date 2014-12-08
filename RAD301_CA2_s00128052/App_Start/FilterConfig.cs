using System.Web;
using System.Web.Mvc;

namespace RAD301_CA2_s00128052
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}