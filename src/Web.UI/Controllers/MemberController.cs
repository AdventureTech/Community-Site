using System.Web.Mvc;
using CommunitySite.Core.Data;
using CommunitySite.Core.Domain;

namespace CommunitySite.Web.UI.Controllers
{
    public class MemberController : Controller
    {
        readonly MemberRepository _memberRepository;

        public MemberController(MemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public ActionResult Register()
        {
            return View("Register", new Member());
        }

        [HttpPost]
        public ActionResult Register(Member member)
        {
            if (_memberRepository.GetByUsername(member.Username) != null)
            {
                ModelState.AddModelError("UserName", "Username already exists");
            }

            if(ModelState.IsValid)
            {
                _memberRepository.Save(member);
                return RedirectToAction("Index", "Home");
            }
            TempData["Error"] = "You messed that up.";
            return View("Register", member);
        }
    }
}
