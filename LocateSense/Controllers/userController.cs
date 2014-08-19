using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LocateSense.Models;
using LocateSense.Business;

namespace LocateSense.Controllers
{
    public class userController : Controller
    {
        public userController()
        {
            db = new LocateSenseContext();
        }

        LocateSenseContext db;

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// get the user details
        /// </summary>
        /// <param name="GUID">user id</param>
        /// <returns></returns>
        public JsonResult getUser(string GUID)
        {
            user user = db.users.Where(x => x.guid == GUID).SingleOrDefault();
            if (user == null)
            {
                return Json(new {message = "No User"}, JsonRequestBehavior.AllowGet);
            }

            user.isLive = true;
            user.isLoggedOn = true;
            user.lastLoggedOn = DateTime.Now;
            db.SaveChanges();
            user.password = "Sensitive data not available";
            
            return Json(user, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// login and return user guid
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public JsonResult UserLogin(string email, string password)
        {
            user user = db.users.Where(x => x.email == email && x.password == password).SingleOrDefault();
            if (user == null)
            {
                return Json(new { message = "No User" }, JsonRequestBehavior.AllowGet);
            }

            user.guid =  Guid.NewGuid().ToString();
            user.isLive = true;
            user.isLoggedOn = true;
            user.lastLoggedOn = DateTime.Now;
            db.SaveChanges();

            return Json(user, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// request new user account
        /// </summary>
        /// <param name="name">name</param>
        /// <param name="level">0 is consumer, 1 is retailer</param>
        /// <param name="email">email</param>
        /// <param name="password">password</param>
        /// <param name="telephone">telephone</param>
        /// <returns>user object</returns>
        public JsonResult RequestUserAccount(string name, int level, string email, string password, string telephone)
        {
            try
            {

                if (Business.Utilities.IsValidEmail(email) == false)
                    return Json(new { message = "Invalid email address" }, JsonRequestBehavior.AllowGet);

                var userOnSystem = db.users.Where(x => x.email == email).Count();
                if (userOnSystem != 0)
                {
                    return Json(new { message = "User already on system." }, JsonRequestBehavior.AllowGet);
                }
                Business.Utilities.sendNewRegistrationEmail(name, email);
                var user = new user();
                user.name = name;
                user.email = email;
                user.telephone = telephone;
                user.password = password;
                user.isLoggedOn = false;
                user.lastLoggedOn = DateTime.Now;
                user.isLive = true;
                user.guid = Guid.NewGuid().ToString();
                user.level = level;
                db.users.Add(user);
                db.SaveChanges();
                return Json(user, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json (new {test = ex.Message.ToString()}, JsonRequestBehavior.AllowGet);

            }
            return Json(new{ed= "edward"}, JsonRequestBehavior.AllowGet);

            //return Json( new { message = "New account created, password send to:" + email }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Logs out and creates new user id
        /// </summary>
        /// <param name="email"></param>
        /// <returns>Logged out message</returns>
        public JsonResult UserLogout(string email)
        {
            user user = db.users.Where(x => x.email == email).SingleOrDefault();
            if (user == null)
            {
                return Json(new { message = "Email not valid on system" }, JsonRequestBehavior.AllowGet);
            }

            user.isLive = true;
            user.isLoggedOn = false;
            user.guid = Guid.NewGuid().ToString();
            db.SaveChanges();

            return Json(new { message = "Logged Out" }, JsonRequestBehavior.AllowGet);

        }

    }
}
