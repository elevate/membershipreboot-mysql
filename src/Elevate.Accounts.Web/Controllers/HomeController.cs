using System.Web.Mvc;

namespace Elevate.Accounts.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("Index", "AccountHome", new { area = "UserAccount" });
        }

    }
}
