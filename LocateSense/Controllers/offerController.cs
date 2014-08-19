using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LocateSense.Models;

namespace LocateSense.Controllers
{
    public class offerController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        LocateSenseContext db;

        public offerController()
        {
            db = new LocateSenseContext();
        }

        /// <summary>
        /// Adds product offers to a product
        /// </summary>
        /// <param name="beaconUUID"></param>
        /// <param name="startDateTime"></param>
        /// <param name="endDateTime"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="strapLine"></param>
        /// <param name="price"></param>
        /// <param name="productId"></param>
        /// <returns>offer object</returns>
        public ActionResult addProductOffer( string beaconUUID,
                                             DateTime? startDateTime,
                                             DateTime? endDateTime ,
                                             string title ,
                                             string description,
                                             string strapLine,
                                             decimal? price,
                                             int productId)
        {
            //adds a offer to a product!!
            product Product = db.products.Where(x => x.UUID == beaconUUID).SingleOrDefault();
            if (Product == null) return Json(new { message = "Not Valid beacon UUID" }, JsonRequestBehavior.AllowGet);
            productId = Product.ID;
            offer offerDB = new offer();
            if (price != null) offerDB.price = price;           
            if (startDateTime != null) offerDB.startDateTime = startDateTime;
            if (strapLine != null) offerDB.strapLine = strapLine;
            if (title != null) offerDB.title = title;
            if (endDateTime != null) offerDB.endDateTime = endDateTime;
            if (description != null) offerDB.description = description;

             db.offers.Add(offerDB);
             db.SaveChanges();

             return Json( offerDB, JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// Updates current offers on system
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="offerID"></param>
        /// <param name="startDateTime"></param>
        /// <param name="endDateTime"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="strapLine"></param>
        /// <param name="price"></param>
        /// <param name="productId"></param>
        /// <returns>offer updated message</returns>
        public ActionResult upDateProductOffer( string userId,
                                                 int offerID,
                                                 DateTime? startDateTime,
                                                 DateTime? endDateTime,
                                                 string title,
                                                 string description,
                                                 string strapLine,
                                                 decimal? price,
                                                 int productId     )
        {
            //adds a offer to a product!!
            var User = db.users.Where(x => x.guid == userId);
            if (User == null) return Json(new { message = "Not Valid user" }, JsonRequestBehavior.AllowGet);

            var OfferDb = (from of in db.offers
                          join pr in db.products on of.productId equals pr.ID
                          where pr.productOwner == User.SingleOrDefault().ID && of.ID == offerID
                          select of);
            if (OfferDb == null)
            {
                return Json(new { message = "Not valid offer to update" }, JsonRequestBehavior.AllowGet);
            }

            offer offerUpdate = db.offers.Where(x => x.ID == OfferDb.SingleOrDefault().ID).SingleOrDefault();

            if (price != null) offerUpdate.price = price;
            if (startDateTime != null) offerUpdate.startDateTime = startDateTime;
            if (strapLine != null) offerUpdate.strapLine = strapLine.ToString();
            if (title != null) offerUpdate.title = title.ToString();
            if (endDateTime != null) offerUpdate.endDateTime = endDateTime;
            if (description != null) offerUpdate.description = description.ToString();

            db.SaveChanges();

            return Json(new { message = "Offer sucessfully updated"}, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// return products sorted by most recent offers
        /// </summary>
        /// <param name="productId">pass in product ID NOT UUID</param>
        /// <returns></returns>
        public ActionResult getProductOffers(int productId)
        {

            var Offers = (from of in db.offers
                          join pr in db.products on of.productId equals pr.ID
                          where  pr.ID == productId
                          select of);

            if (Offers == null)
            {
                return Json(new { message = "No offers for this product" }, JsonRequestBehavior.AllowGet);
            }

            return Json(Offers.ToList(), JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Disables an offer
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="offerId">offer id</param>
        /// <returns></returns>
        public ActionResult disableOffer(string userId, int offerId)
        {
            user User = db.users.Where(x => x.guid == userId).SingleOrDefault();
            if (User == null) return Json(new { message = "Incorrect user" }, JsonRequestBehavior.AllowGet);

            var offerDisable = (from of in db.offers
                        join pr in db.products on of.productId equals pr.ID
                        where pr.productOwner == User.ID && of.ID == offerId
                        select of);

            if (offerDisable == null) return Json(new { message = "Incorrect offerId" }, JsonRequestBehavior.AllowGet);

            offer OfferDB = db.offers.Where(x => x.ID == offerId).SingleOrDefault();
            OfferDB.endDateTime = DateTime.Now;
            db.SaveChanges();

            return Json(new { message = "Offer disabled" }, JsonRequestBehavior.AllowGet);

        }



    }
}
