﻿using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using BrockAllen.MembershipReboot;
using Elevate.Accounts.Web.Areas.UserAccount.Models;

namespace Elevate.Accounts.Web.Areas.UserAccount.Controllers
{
    [Authorize]
    public class ChangeUsernameController : Controller
    {
        UserAccountService userAccountService;
        AuthenticationService authSvc;

        public ChangeUsernameController(AuthenticationService authSvc)
        {
            this.userAccountService = authSvc.UserAccountService;
            this.authSvc = authSvc;
        }

        public ActionResult Index()
        {
            return View("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ChangeUsernameInputModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    this.userAccountService.ChangeUsername(User.GetUserID(), model.NewUsername);
                    this.authSvc.SignIn(User.GetUserID());
                    return RedirectToAction("Success");
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            return View("Index", model);
        }

        public ActionResult Success()
        {
            return View("Success", (object)User.Identity.Name);
        }
    }
}
