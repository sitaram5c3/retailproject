﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LocateSense.Models;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace LocateSense.Controllers
{
    public class productController : Controller
    {
        public void getImage()
        {
      
            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
            CloudConfigurationManager.GetSetting("StorageConnectionString"));

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve a reference to a container. 
            CloudBlobContainer container = blobClient.GetContainerReference("image");

            foreach (IListBlobItem item in container.ListBlobs(null, true))
            {
                System.Diagnostics.Debug.Print(item.Uri.AbsoluteUri);
            }

            CloudBlockBlob blockBlob = container.GetBlockBlobReference(DateTime.Now.ToFileTimeUtc().ToString());

            // Create or overwrite the "myblob" blob with contents from a local file.
            using (var fileStream = System.IO.File.OpenRead(@"C:\data\locatesense\website\locatesensewebsite\images\marker.png"))
            {
                blockBlob.UploadFromStream(fileStream);
            } 



        }

        /// <summary>
        /// Save the installation image 
        /// </summary>
        /// <param name="UserGUID">User ID</param>
        /// <param name="UUID">Product UUID</param>
        /// <param name="file">Image file</param>
        /// <returns>Absolute URL of image</returns>
        public ActionResult SaveInstallationImage(string userGUID, string UUID, HttpPostedFileBase file)
        {
            var user = db.users.Where(x => x.guid == userGUID);

            if (user.Count() == 0) return Json(new { message = "No user" }, JsonRequestBehavior.AllowGet);
            if (user.SingleOrDefault().level != 1) return Json(new { message = "User is not retailer" }, JsonRequestBehavior.AllowGet);

            //var productBeacon = db.products.Where(x => x.UUID == UUID).SingleOrDefault();
            var product = db.products.Where(x => x.UUID == UUID);// && x.productOwner == user.SingleOrDefault().ID);

            if (product == null)
            {
                return Json(new { message = "No product for UUID" }, JsonRequestBehavior.AllowGet);
            }

            if (file != null && file.ContentLength > 0)
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference("image");
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(DateTime.Now.ToFileTimeUtc().ToString() + "-" + file.FileName);
                blockBlob.UploadFromStream(file.InputStream);
                product.SingleOrDefault().imageInstallationURL = blockBlob.Uri.AbsoluteUri;
                db.SaveChanges();
                return Json(new { message = blockBlob.Uri.AbsoluteUri }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { message = "Not uploaded" }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Save the product image 
        /// </summary>
        /// <param name="UserGUID">User ID</param>
        /// <param name="UUID">Product UUID</param>
        /// <param name="file">Image file</param>
        /// <returns>Absolute URL of image</returns>
       // [HttpPost]
        public ActionResult SaveProductImage(string userGUID, string UUID, HttpPostedFileBase file)
        {
            var user = db.users.Where(x => x.guid == userGUID);

            if (user.Count() == 0) return Json(new { message = "No user" }, JsonRequestBehavior.AllowGet);
            if (user.SingleOrDefault().level != 1) return Json(new { message = "User is not retailer" }, JsonRequestBehavior.AllowGet);


  //          if (user == null) return Json(new { message = "No user" }, JsonRequestBehavior.AllowGet);
  //          if (user.level != 1) return Json(new { message = "User is not retailer" }, JsonRequestBehavior.AllowGet);
         //   var userID = user.SingleOrDefault().ID;
          // var product = db.products.Where(x => x.productOwner == userID);
            var product = db.products.Where(x => x.UUID == UUID);
            if (product == null)
            {
                return Json(new { message = "No product for UUID" }, JsonRequestBehavior.AllowGet);
            }

            if (file != null && file.ContentLength > 0)
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference("image");
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(DateTime.Now.ToFileTimeUtc().ToString() + "-" + file.FileName);
                blockBlob.UploadFromStream(file.InputStream);
                product.SingleOrDefault().imageURL = blockBlob.Uri.AbsoluteUri;
                db.SaveChanges();
                return Json(new { message = blockBlob.Uri.AbsoluteUri }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { message = "Not uploaded" }, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// gets the products for a user
        /// </summary>
        /// <param name="UUID">user GUID</param>
        /// <returns></returns>
        public JsonResult getRetailerProducts(string userGUID)
        {
            var user = db.users.Where(x => x.guid == userGUID).SingleOrDefault();
            if (user == null) return Json(new { message = "No user" }, JsonRequestBehavior.AllowGet);
            if (user.level != 1) return Json(new { message = "User is not retailer" }, JsonRequestBehavior.AllowGet);

            var product = db.products.Where(x => x.productOwner == user.ID);
           if (product.Count() == 0)
            {
                return Json(new { message = "No products" }, JsonRequestBehavior.AllowGet);
            }

            return Json(product, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// search for the products in categories
        /// </summary>
        /// <param name="Category">all categories can be multilevel, delimited by "\"</param>
        /// <returns></returns>
        public JsonResult getProductsInCategory(string Category)
        {

            var product = db.products.Where(x => x.category.Contains(Category) );
            if (product.Count() == 0)
            {
                return Json(new { message = "No products in category " + Category }, JsonRequestBehavior.AllowGet);
            }

            return Json(product, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// gets the product from a beacon UUID
        /// </summary>
        /// <param name="UUID">beacon UUID</param>
        /// <returns></returns>
        public JsonResult getProduct(string UUID)
        {
            var product = db.products.Where(x => x.UUID == UUID);
            if (product == null)
            {
                return Json(new { message = "No product" }, JsonRequestBehavior.AllowGet);
            }
            else if (product.Count() == 1)
            {
                return Json(product.SingleOrDefault(), JsonRequestBehavior.AllowGet);
            }
            return Json(new { message = "System Error - multiple products with identical UUID" }, JsonRequestBehavior.AllowGet);
            
        }

        /// <summary>
        /// Debug only delete the beacon from the system
        /// </summary>
        /// <param name="UUID">beacon UUID</param>
        /// <returns></returns>
        public JsonResult removeBeaconTEST_ONLY(string UUID)
        {

                var productBeacon = db.products.Where(x => x.UUID == UUID);
                if (productBeacon == null) Json(new { message = "Not valid UUID" }, JsonRequestBehavior.AllowGet);
                
                try
                {
                    var deleteOffers = db.offers.Where(x => x.productId == productBeacon.SingleOrDefault().ID);
                    foreach (var offerToDelete in deleteOffers)
                    {
                        db.offers.Remove(offerToDelete);
                    }

                    var deleteLocate = db.locate.Where(x => x.beaconId == productBeacon.SingleOrDefault().ID);
                    foreach (var locateToDelete in deleteLocate)
                    {
                        db.locate.Remove(locateToDelete);
                    }
                }
                catch (Exception ex) { }
                try
                {
                    db.products.Remove(productBeacon.SingleOrDefault());
                    db.SaveChanges();
                }
                catch (Exception ex) { }
                

            return Json(new { message = "deleted UUID beacon and its associated offers from system" }, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// Retailer can add a product to beacon here
        /// </summary>
        /// <param name="userId">user GUID</param>
        /// <param name="UUID">UUID beacon ID</param>
        /// <param name="Product">product model</param>
        /// <returns>Product object</returns>
        //public JsonResult addNewBeacon( string UserGUID, 
        //                                string manufacturer, 
        //                                string productName, 
        //                                string imageURL, 
        //                                string imageInstallationURL, 
        //                                int? availableStock, 
        //                                int? numberOfVisits, 
        //                                decimal? price, 
        //                                string UUID,
        //                                string category   )
        //{
        //    var user = db.users.Where(x => x.guid == UserGUID);
        //    if (user.Count() == 0) return Json(new { message = "No user" }, JsonRequestBehavior.AllowGet);
        //    if (user.SingleOrDefault().level != 1) return Json(new { message = "User is not retailer" }, JsonRequestBehavior.AllowGet);
        //    var productBeacon = db.products.Where(x => x.UUID == UUID).SingleOrDefault();
            
        //    if (productBeacon != null)
        //    {
        //        return Json(new { message = "This beacon already on system" }, JsonRequestBehavior.AllowGet);
        //    }
        //    product Product = new product();
        //    Product.productOwner = user.SingleOrDefault().ID;
        //    Product.UUID = UUID;

        //    Product.manufacturer =  manufacturer;
        //    Product.productName  = productName;       
        //    if(availableStock != null) Product.availableStock = (int)availableStock;
        //    if(numberOfVisits != null) Product.numberOfVisits = (int)numberOfVisits;
        //    if (price != null) Product.price = (decimal)price;
        //    Product.category = @"\";
        //    db.products.Add(((product)Product));
        //    db.SaveChanges();
        //    //if new product can add
        //    return Json(Product, JsonRequestBehavior.AllowGet);
        //}


        /// <summary>
        /// Retailer can add a product to beacon here
        /// </summary>
        public JsonResult addNewBeacon(string UserGUID,
                                        string manufacturer,
                                        string productName,
                                        HttpPostedFileBase imageFile,
                                        int? availableStock,
                                        int? numberOfVisits,
                                        decimal? price,
                                        string UUID,
                                        string category,
                                        string imageURL, 
                                        string imageInstallationURL)
        {

            var user = db.users.Where(x => x.guid == UserGUID);
            if (user.Count() == 0) return Json(new { message = "No user" }, JsonRequestBehavior.AllowGet);
            if (user.SingleOrDefault().level != 1) return Json(new { message = "User is not retailer" }, JsonRequestBehavior.AllowGet);
            var productBeacon = db.products.Where(x => x.UUID == UUID).SingleOrDefault();

            if (productBeacon != null)
            {
                return Json(new { message = "This beacon already on system" }, JsonRequestBehavior.AllowGet);
            }
            product Product = new product();
            Product.productOwner = user.SingleOrDefault().ID;
            Product.UUID = UUID;
            if (imageFile != null && imageFile.ContentLength > 0)
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference("image");
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(DateTime.Now.ToFileTimeUtc().ToString() + "-" + imageFile.FileName);
                blockBlob.UploadFromStream(imageFile.InputStream);
                Product.imageInstallationURL = blockBlob.Uri.AbsoluteUri;
            }

            Product.manufacturer = manufacturer;
            Product.productName = productName;
            if (availableStock != null) Product.availableStock = (int)availableStock;
            if (numberOfVisits != null) Product.numberOfVisits = (int)numberOfVisits;

            if (!String.IsNullOrEmpty(imageURL))
            {
                Product.imageURL = imageURL;
            }
            if (!String.IsNullOrEmpty(imageInstallationURL))
            {
                Product.imageInstallationURL = imageInstallationURL;
            } 
            
            if (price != null) Product.price = (decimal)price;
            Product.category = @"\";
            db.products.Add(((product)Product));
            db.SaveChanges();
            //if new product can add
            return Json(Product, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// updates the product
        /// </summary>
        /// <param name="UserId">user GUID</param>
        /// <param name="Product">product updates, null parameter are not updated on system</param>
        /// <returns></returns>
        public JsonResult upDateProduct(string UserGUID,
                                        string manufacturer,
                                        string productName,
                                        string imageURL,
                                        string imageInstallationURL,
                                        int? availableStock,
                                        decimal? price,
                                        string UUID,
                                        string category)
        {
            var user = db.users.Where(x => x.guid == UserGUID);

            if (user.Count() == 0) return Json(new { message = "No user" }, JsonRequestBehavior.AllowGet);
            if (user.SingleOrDefault().level != 1) return Json(new { message = "User is not retailer" }, JsonRequestBehavior.AllowGet);
            var ProductDB = db.products.Where(x => x.UUID == UUID);
          
          //  var ProductDB = db.products.Where(x => x.UUID == UUID && x.productOwner == user.ID);
            if (ProductDB.Count() == 0)
            {
                if (db.products.Where(x => x.UUID == UUID).SingleOrDefault() != null)
                {
                    return Json(new { message = "No product with this UUID" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { message = "This is not your product" }, JsonRequestBehavior.AllowGet);
                }
            }

            string catStr = @"\";


            if(category != null)
            {
                var categorySplit = category.ToString().Split('\\');

                foreach (var catItem in categorySplit)
                {
                    if (catItem.Length >= 1)
                    {
                        catStr = catStr + catItem + @"\";
                    }
                }
            }

            if (category != null) ProductDB.SingleOrDefault().category = catStr;
            if (availableStock != null) ProductDB.SingleOrDefault().availableStock = (int)availableStock;
            if (imageInstallationURL != null) ProductDB.SingleOrDefault().imageInstallationURL = imageInstallationURL.ToString();
            if (imageURL != null)  ProductDB.SingleOrDefault().imageURL = imageURL.ToString();
            if (manufacturer != null)  ProductDB.SingleOrDefault().manufacturer = manufacturer;
            if (price != null)  ProductDB.SingleOrDefault().price = (decimal)price;
            if (productName != null) ProductDB.SingleOrDefault().productName = productName.ToString();
            db.SaveChanges();

            //if new product can add
            return Json(ProductDB.SingleOrDefault(), JsonRequestBehavior.AllowGet);


        }

        public productController()
        {
            db = new LocateSenseContext();
        }

        LocateSenseContext db;

        public ActionResult Index()
        {
          //  getImage();
            return View();
        }




    }
}
