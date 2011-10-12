using System.Web.Mvc;
using System.Web.Routing;
using CommunitySite.Core.Data;
using CommunitySite.Web.UI;
using CommunitySite.Web.UI.Controllers;
using FakeItEasy;
using Machine.Specifications;
using MvcContrib.TestHelper;

namespace CommunitySite.Specifications
{
    public class When_a_user_wishes_to_view_the_home_page
    {
        Establish context = () =>
            {
                RouteTable.Routes.Clear();
                MvcApplication.RegisterRoutes(RouteTable.Routes);
            };

        It should_navigate_to_the_home_page_of_the_application = () =>
            "~/".ShouldMapTo<HomeController>(ctrl => ctrl.Index());
    }

    public class When_navigating_to_the_home_page_of_the_application
    {
        Establish context = () =>
            {
                _controller = new HomeController();
            };

        Because of = () => _result = _controller.Index();

        It should_load_the_home_page = () =>
            _result.AssertViewRendered().ForView("Index");

        static HomeController _controller;
        static ActionResult _result;
    }
}