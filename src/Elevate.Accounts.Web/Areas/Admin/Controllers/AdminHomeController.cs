using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using BrockAllen.MembershipReboot;

namespace Elevate.Accounts.Web.Areas.Admin.Controllers
{
    public class AdminHomeController : Controller
    {
        readonly IUserAccountQuery query;
        readonly UserAccountService userAccountService;

        public AdminHomeController(IUserAccountQuery query, UserAccountService userAccountService)
        {
            this.userAccountService = userAccountService;
            this.query = query;
        }

        public ActionResult Index(string filter)
        {
            int count;
            var accounts = query.Query(userAccountService.Configuration.DefaultTenant, filter, 0, 1000, out count);

            return View("Index", accounts.ToArray());
        }

        public ActionResult Detail(Guid id)
        {
            var account = userAccountService.GetByID(id);
            return View("Detail", account);
        }

        [HttpPost]
        public ActionResult Reopen(Guid id)
        {
            try
            {
                userAccountService.ReopenAccount(id);
                return RedirectToAction("Detail", new { id });
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return Detail(id);
        }
    }
}
