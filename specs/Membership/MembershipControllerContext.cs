using System.Web.Mvc;
using CommunitySite.Core.Data;
using CommunitySite.Core.Domain;
using CommunitySite.Web.UI.Controllers;
using FakeItEasy;
using Machine.Specifications;

namespace CommunitySite.Specifications.Membership
{
    public class With_a_membership_controller_context   
    {
        Establish context = () =>
        {
            _member = new Member { ID = 1 };  
            _repository = A.Fake<MemberRepository>();
            _controller = new MemberController(_repository);
        };

        protected static MemberController _controller;
        protected static MemberRepository _repository;
        protected static Member _member;
        protected static ActionResult _result;
    }
}