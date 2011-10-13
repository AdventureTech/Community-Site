using CommunitySite.Core.Domain;
using CommunitySite.Web.UI.Controllers;
using FakeItEasy;
using Machine.Specifications;
using MvcContrib.TestHelper;

namespace CommunitySite.Specifications.Membership
{
    public class When_an_anonymous_user_wishes_to_log_in
        : With_the_main_site_routes_registered
    {
        It should_navigate_to_the_member_login_page = () =>
            "~/member/login".ShouldMapTo<MemberController>(ctrl => ctrl.Login());
    }

    public class When_navigating_to_the_member_login_page
        : With_a_membership_controller_context
    {
        Because of = () => _result = _controller.Login();
        
        It should_load_the_member_login_form =()=>
            _result.AssertViewRendered().ForView("Login");
    }

    public class When_logging_in_with_a_valid_username_and_password
        : With_a_membership_controller_context
    {
        Establish context = () =>
            {
                _validUsername = "VALIDUSERNAME";
                _validPassword = "VALIDPASSWORD";
                A.CallTo(() => _authService.Authenticate(_validUsername, _validPassword))
                    .Returns(true);
            };

        Because of = () => _result = _controller.Login(_validUsername, _validPassword);

        It should_authenticate_the_user_credentials = () =>
            A.CallTo(() => _authService.Authenticate(_validUsername, _validPassword)).MustHaveHappened();

        It should_log_the_member_in = () =>
            A.CallTo(() => _authService.SignIn(_validUsername)).MustHaveHappened();

        It should_take_the_user_to_the_home_page = () =>
            _result.AssertActionRedirect().ToAction<HomeController>(ctrl => ctrl.Index());

        static string _validUsername;
        static string _validPassword;
    }

    public class When_logging_in_with_invalid_username_or_password
        : With_a_membership_controller_context
    {
        Establish context = () =>
        {
            _invalidUsername = "INVALIDUSERNAME";
            _invalidPassword = "INVALIDPASSWORD";
            A.CallTo(() => _authService.Authenticate(_invalidUsername, _invalidPassword))
                .Returns(false);
        };

        Because of = () => _result = _controller.Login(_invalidUsername, _invalidPassword);

        It should_authenticate_the_user_credentials = () =>
            A.CallTo(() => _authService.Authenticate(_invalidUsername, _invalidPassword)).MustHaveHappened();

        It should_not_log_the_user_in = () =>
            A.CallTo(() => _authService.SignIn(A<string>.Ignored)).MustNotHaveHappened();

        It should_return_the_user_to_the_login_page = () =>
            _result.AssertViewRendered().ForView("Login");

        It should_inform_the_user_that_the_authentication_failed = () =>
            _controller.TempData["Error"].ShouldNotBeNull();

        static string _invalidUsername;
        static string _invalidPassword;
    }
}