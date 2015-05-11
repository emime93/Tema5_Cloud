using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CloudWebMVC.Controllers
{
    public class SignUpController : Controller
    {
        // GET: SignUp
        [HttpPost]
        public ActionResult SignUp(string username, string password)
        {
            if (username.Trim().Equals("") || password.Trim().Equals(""))
                return RedirectToAction("../Home/SignUp");
            else
            {
                using (var context = new CloudModel.LeModelContainer())
                {
                    foreach (var user1 in context.Users)
                    {
                        if (user1.Username.Equals(username))
                            return RedirectToAction("../Home/SignUp");
                    }

                    var user = new CloudModel.User { Username = username, Password = password };
                    context.Users.Add(user);
                }
                return RedirectToAction("../Home/Index");
            }
        }

        [HttpPost]
        public ActionResult SignIn(string username, string password)
        {
            Models.Profile.leUser.Username = username;
            Models.Profile.leUser.Password = password;
            Models.Profile.leUser.Status = true;

            if (Models.Profile.userBL.SignIn(Models.Profile.leUser))
            {
                Models.User user = new Models.User();
                user.Username = username;
                return RedirectToAction("../Home/Dashboard", user);
            }

            return RedirectToAction("../Home/Index");
        }
    }
}