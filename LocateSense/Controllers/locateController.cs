using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LocateSense.Models;

namespace LocateSense.Controllers
{
    public class locateController : Controller
    {
        //
        // GET: /locate/


        /// <summary>
        /// Add beacon visit and increment visit counter
        /// </summary>
        /// <param name="userID">user GUID</param>
        /// <param name="UUID">beacon UUID</param>
        /// <returns>status message</returns>
        public ActionResult addBeaconVisit(string userID, string UUID)
        {
            var productBeacon = db.products.Where(x => x.UUID == UUID);
            if(productBeacon == null)
            {
                return Json(new { message = "Not valid beacon UUID" }, JsonRequestBehavior.AllowGet);
            }

            var User = db.users.Where(x => x.guid == userID);
            if(User == null)
            {
                return Json(new { message = "Not valid  user" }, JsonRequestBehavior.AllowGet);
            }

            var productBeaconVist = productBeacon.SingleOrDefault();
            productBeaconVist.numberOfVisits ++;
            //adds a user to a beacon!!
            db.locate.Add(new locate { beaconId = productBeacon.SingleOrDefault().ID, userId = User.SingleOrDefault().ID, vistDateTime = DateTime.Now });
            db.SaveChanges();

            return Json(new { message = "beacon visit sucessfully logged" }, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// Get the users visits
        /// </summary>
        /// <param name="userId">user guid</param>
        /// <param name="numberOfDays">nmuber of days</param>
        /// <returns></returns>
        public ActionResult getLastUserBeaconVists(string userGUID, int numberOfDays)
        {

            //return products user visted in last number day sorted by most recent
            var User = db.users.Where(x => x.guid == userGUID);
            if (User == null)
            {
                return Json(new { message = "Not valid  user" }, JsonRequestBehavior.AllowGet);
            }

            var locates = db.locate.Where(x => x.userId == User.SingleOrDefault().ID && x.vistDateTime > DateTime.Now.AddDays(-1 * numberOfDays));

            if (locates == null)
            {
                return Json(new { message = "No beacon visits" }, JsonRequestBehavior.AllowGet);
            }

            return Json(locates, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Index()
        {
            return View();
        }

        LocateSenseContext db;

        public locateController()
        {
            db = new LocateSenseContext();
        }

    }
}
