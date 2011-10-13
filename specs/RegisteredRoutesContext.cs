using System.Web.Routing;
using CommunitySite.Web.UI;
using Machine.Specifications;

namespace CommunitySite.Specifications
{
    public class With_the_main_site_routes_registered
    {
        Establish context = () =>
        {
            RouteTable.Routes.Clear();
            MvcApplication.RegisterRoutes(RouteTable.Routes);
        };
    }
}