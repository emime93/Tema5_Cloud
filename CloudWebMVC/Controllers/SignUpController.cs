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
                var user = new CloudModel.User { Username = username, Password = password };
                if (Models.Profile.userBL.SignUp(user))
                {
                    Models.User user1 = new Models.User();
                    user1.Username = username;                 
                    return RedirectToAction("../Home/Dashboard", user1);
                }
                else
                {
                    Models.User user1 = new Models.User();
                    user1.Username = username;
                    user1.ErrorMessage = "There is already a user with the username " + user1.Username;
                    return RedirectToAction("../Home/SignUp", user1);
                }               
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

        [HttpPost]
        public ActionResult SignOut(Models.User user)
        {
            CloudModel.User user1 = new CloudModel.User();
            user1.Username = user.Username;
            if (Models.Profile.userBL.SignOut(user1))
                return RedirectToAction("../Home/Index");
            else return RedirectToAction("../Home/Dashboard");
        }
    }
}