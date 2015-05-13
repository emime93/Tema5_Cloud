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
                var user = new CloudModel.User { Username = username, Password = password, Status = true};
                if (Models.Profile.userBL.SignUp(user))
                {
                    Session["leUser"] = user;
                    Models.User user1 = new Models.User();
                    user1.Username = username;                 
                    return RedirectToAction("../Home/Dashboard", user);
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
            if (username.Trim().Equals("") || password.Trim().Equals(""))
                return RedirectToAction("../Home/Index");

            var user = (CloudModel.User)Session["leUser"];
            user.Username = username;
            user.Password = password;
            
            if ((user = Models.Profile.userBL.SignIn(user)) != null)
            {
                Session["leUser"] = user;
                return RedirectToAction("../Home/Dashboard");               
            }

            return RedirectToAction("../Home/Index");
        }

        [HttpPost]
        public ActionResult SignOut(Models.User user)
        {
            CloudModel.User user1 = new CloudModel.User();
            user1.Username = user.Username;
            var user2 = (CloudModel.User)Session["leUser"];
            user2.Status = false;            
            if (Models.Profile.userBL.SignOut(user1))
                return RedirectToAction("../Home/Index");
            else return RedirectToAction("../Home/Dashboard");
        }
    }
}