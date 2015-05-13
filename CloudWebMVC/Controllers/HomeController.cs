using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CloudWebMVC.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {         
        }
        public ActionResult Index()
        {
            if (Session["leUser"] == null)
            {
                var user = new CloudModel.User();
                user.Status = false;
                user.Username = "";
                user.Password = "";
                Session["leUser"] = user;
            }
            CloudModel.User user1 = (CloudModel.User) Session["leUser"];
            if (user1.Status == true)
                return RedirectToAction("../Home/Dashboard");
            return View();
        }

        public ActionResult Dashboard()
        {
            CloudModel.User user1 = (CloudModel.User)Session["leUser"];
            Models.User user = new Models.User();
            user.Username = user1.Username;

            if (user1.Status == false)
                return RedirectToAction("../Home/Index");

            using (var context = new CloudModel.LeModelContainer())
            {
                var leUser = context.Users.Find(user1.Id);
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
            CloudModel.User user1 = (CloudModel.User)Session["leUser"];
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