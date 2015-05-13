using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CloudWebMVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            CloudModel.User user1 = Models.Profile.leUser;
            if (user1.Status == true)
                return RedirectToAction("../Home/Dashboard");
            return View();
        }

        public ActionResult Dashboard()
        {
            CloudModel.User user1 = Models.Profile.leUser;
            Models.User user = new Models.User();
            user.Username = user1.Username;

            if (user1.Status == false)
                return RedirectToAction("../Home/Index");

            using (var context = new CloudModel.LeModelContainer())
            {
                var leUser = context.Users.Find(Models.Profile.leUser.Id);
                int i = 0;
                user.FileNames = new string[leUser.Documents.Count];
                foreach (var doc in context.Documents)
                {
                    if (doc.User.Equals(leUser))
                        user.FileNames[i++] = doc.Name;
                }
                return View(user);
            }

        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public ActionResult SignUp(Models.User user)
        {
            ViewBag.Message = "Login page";
            CloudModel.User user1 = Models.Profile.leUser;
            if (user1.Status == true)
                return RedirectToAction("../Home/Dashboard");
            return View(user);
        }

        [HttpPost]
        public ActionResult SignUp(string username, string password)
        {
            return View("Index");
        }
    }
}