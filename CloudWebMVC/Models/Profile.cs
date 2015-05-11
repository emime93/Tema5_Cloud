using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CloudBusinessLayer;

namespace CloudWebMVC.Models
{
    public class Profile
    {
        public static CloudModel.User leUser = new CloudModel.User();
        public static UserBL userBL = new UserBL();
    }
}