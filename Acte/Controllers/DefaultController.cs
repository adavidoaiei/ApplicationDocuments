using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ActeAuto.Controllers
{
    public class DefaultController : Controller
    {
        //
        // GET: /Default/

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Models.User user)
        {
            if (ModelState.IsValid)
            {
                if (user.IsValid(user.UserName, user.Password))
                {
                    FormsAuthentication.SetAuthCookie(user.UserName, user.RememberMe);
                    return RedirectToAction("Completeaza", "Facade");
                }
                else
                {
                    ModelState.AddModelError("", "Datele de login sunt incorecte!");
                }
            }
            return View(user);
        }

    }
}
