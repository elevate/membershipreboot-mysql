using System;
using System.Security.Claims;
using System.Web.Mvc;
using BrockAllen.MembershipReboot;

namespace Elevate.Accounts.Web.Areas.UserAccount.Controllers
{
    [RouteArea("UserAccount")]
    public class AccountHomeController : Controller
    {
        readonly UserAccountService _userAccountService;
        readonly AuthenticationService _authSvc;

        public AccountHomeController(UserAccountService userAccountService, AuthenticationService authSvc)
        {
            _userAccountService = userAccountService;
            _authSvc = authSvc;
        }

        //[Route("useraccount/home")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Index(string gender)
        {
            if (String.IsNullOrWhiteSpace(gender))
            {
                _userAccountService.RemoveClaim(User.GetUserID(), ClaimTypes.Gender);
            }
            else
            {
                // if you only want one of these claim types, uncomment the next line
                //account.RemoveClaim(ClaimTypes.Gender);
                _userAccountService.AddClaim(User.GetUserID(), ClaimTypes.Gender, gender);
            }

            // since we've changed the claims, we need to re-issue the cookie that
            // contains the claims.
            _authSvc.SignIn(User.GetUserID());

            return RedirectToAction("Index");
        }

    }
}
