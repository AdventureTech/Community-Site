using System.Web.Mvc;

namespace CommunitySite.Web.UI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View("Index");
        }
    }
}