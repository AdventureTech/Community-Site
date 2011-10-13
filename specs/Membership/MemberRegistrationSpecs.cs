using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;
using CommunitySite.Core.Data;
using CommunitySite.Core.Data.NHibernate;
using CommunitySite.Core.Domain;
using CommunitySite.Web.UI;
using CommunitySite.Web.UI.Controllers;
using FakeItEasy;
using Machine.Specifications;
using MvcContrib.TestHelper;

namespace CommunitySite.Specifications.Membership
{
    public class When_an_anonymous_user_wishes_to_register_for_membership
        : With_the_main_site_routes_registered
    {
        It should_navigate_to_the_member_registration_page =()=>
            "~/member/register".ShouldMapTo<MemberController>(ctrl=>ctrl.Register());
    }

    public class When_navigating_to_the_member_registration_page 
        : With_a_membership_controller_context
    {
        Because of = () => _result = _controller.Register();

        It should_load_the_member_registration_page = () =>
            _result.AssertViewRendered().ForView("Register");

        It should_load_an_empty_member_registration_form = () =>
            _result.AssertViewRendered().Model.ShouldBe(typeof(Member));
    }

    public class When_a_user_attempts_to_register_with_valid_and_complete_information
        : With_a_membership_controller_context
    {

        Establish context = () =>
            {
                A.CallTo(() => _repository.GetByUsername(A<string>.Ignored)).Returns(null);
                _controller.ModelState.Clear(); // THIS IS A VALID MODEL
               
            };

        Because of = () => _result = _controller.Register(_member);

        It should_create_the_membership_for_the_user = () =>
            A.CallTo(() => _repository.Save(_member)).MustHaveHappened();

        It should_redirect_the_user_to_the_home_page = () =>
            _result.AssertActionRedirect().ToAction<HomeController>(ctrl => ctrl.Index());
    }

    public class When_a_user_attempts_to_register_with_invalid_or_incomplete_information 
        : With_a_membership_controller_context
    {
        Establish context = () => _controller.ModelState.AddModelError("", "INVALID MODEL");

        Because of = () => _result = _controller.Register(_member);

        Behaves_like<FailedRegistrationBehavior> a_failed_registration;

        It should_refill_the_registration_form_with_the_invalid_information = () =>
            _result.AssertViewRendered().Model.ShouldBe(_member);

        It should_inform_the_user_that_the_registration_was_unsuccessful = () =>
            _controller.TempData["Error"].ShouldNotBeNull();
    }

    public class When_a_membership_is_created_for_a_user
    {
        Establish context = () =>
            {
                _member = new Member {ID = 1};
                _repository = A.Fake<Repository>();
                _memberRepository = new NHibernateMemberRepository(_repository);
            };

        Because of = () => _memberRepository.Save(_member);

        It should_save_that_user_information = () =>
            A.CallTo(() => _repository.Save(_member)).MustHaveHappened();

        static NHibernateMemberRepository _memberRepository;
        static Member _member;
        static Repository _repository;
    }

    public class When_a_user_attempts_to_register_with_a_username_that_is_already_taken
        : With_a_membership_controller_context
    {
        Establish context = () =>
            {
                _member = new Member {Username = "Jim"};
                A.CallTo(() => _repository.GetByUsername(A<string>.Ignored))
                    .Returns(new Member { ID = 1, Username = "Jim" });
            };

        Because of = () => _result = _controller.Register(_member);

        It should_search_the_membership_listing_for_a_member_with_the_requested_username = () =>
            A.CallTo(() => _repository.GetByUsername("Jim")).MustHaveHappened();

        Behaves_like<FailedRegistrationBehavior> a_failed_registration_attempt;

        It should_inform_the_user_that_the_username_is_taken = () =>
            _controller.ModelState["UserName"].Errors.ShouldNotBeEmpty();
    }

    public class When_searching_the_membership_listing_for_a_member_by_username_when_a_member_with_that_username_exists
    {
        Establish context = () =>
            {
                _repository = A.Fake<Repository>();
                A.CallTo(() => _repository.All<Member>())
                    .Returns(new List<Member>
                                 {
                                     new Member {Username = "John"},
                                     new Member {Username = "Bill"},
                                     new Member {Username = "Steve"},
                                     new Member {Username = "Dave"}
                                 }.AsQueryable());
                _memberRepository = new NHibernateMemberRepository(_repository);
            };

        Because of = () => _result = _memberRepository.GetByUsername("Steve");

        It should_search_the_membership_listing = () =>
            A.CallTo(() => _repository.All<Member>()).MustHaveHappened();

        It should_return_the_user_with_that_username = () =>
            _result.Username.ShouldEqual("Steve");

        static Repository _repository;
        static NHibernateMemberRepository _memberRepository;
        static Member _result;
    }

    public class When_searching_the_membership_listing_for_a_member_by_username_when_no_member_with_that_username_exists
    {
        Establish context = () =>
            {
                _repository = A.Fake<Repository>();
                A.CallTo(() => _repository.All<Member>())
                    .Returns(new List<Member>
                                 {
                                     new Member {Username = "John"},
                                     new Member {Username = "Bill"},
                                     new Member {Username = "Steve"},
                                     new Member {Username = "Dave"}
                                 }.AsQueryable());
                _memberRepository = new NHibernateMemberRepository(_repository);
            };

        Because of = () => _result = _memberRepository.GetByUsername("This user doesnt exist");

        It should_search_the_membership_listing = () =>
            A.CallTo(() => _repository.All<Member>()).MustHaveHappened();

        It should_return_nothing = () => _result.ShouldBeNull();

        static Repository _repository;
        static NHibernateMemberRepository _memberRepository;
        static Member _result;
    }

    [Behaviors]
    public class FailedRegistrationBehavior : With_a_membership_controller_context
    {
        It should_not_create_a_membership_for_the_user = () =>
           A.CallTo(() => _repository.Save(A<Member>.Ignored)).MustNotHaveHappened();

        It should_return_them_to_the_member_registration_page = () =>
            _result.AssertViewRendered().ForView("Register");

    }
}