using System.Web.Mvc;
using CommunitySite.Core.Data;
using CommunitySite.Core.Domain;
using CommunitySite.Core.Services.Authentication;

namespace CommunitySite.Web.UI.Controllers
{
    public class MemberController : Controller
    {
        readonly MemberRepository _memberRepository;
        readonly AuthenticationService _authService;

        public MemberController(MemberRepository memberRepository, AuthenticationService authService)
        {
            _memberRepository = memberRepository;
            _authService = authService;
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

        public ActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            if(_authService.Authenticate(username, password))
            {
                _authService.SignIn(username);
                return RedirectToAction("Index", "Home");                
            }
            TempData["Error"] = "Login attempt failed.";
            return View("Login");
        }
    }
}
