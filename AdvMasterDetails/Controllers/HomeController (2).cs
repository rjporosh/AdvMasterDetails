using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.Mvc;
using AdvMasterDetails.Models;

namespace AdvMasterDetails.Controllers
{
    public class HomeController : Controller
    {
        MyDatabaseEntities db = new MyDatabaseEntities();
        public ActionResult Index()
        {
            return View();
        }

     
        public JsonResult GetProductCategories()
        {
            List<Category> categories;
            using (MyDatabaseEntities db = new MyDatabaseEntities())
            {
                categories = db.Categories.OrderBy(a => a.CategortyName).ToList();
            }
            return new JsonResult { Data = categories, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetProducts(int categoryID)
        {
            List<Product> products = new List<Product>();
            using (MyDatabaseEntities db = new MyDatabaseEntities())
            {
                products = db.Products.Where(a => a.CategoryID.Equals(categoryID)).OrderBy(a => a.ProductName).ToList();
            }
            return new JsonResult { Data = products, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult ShowAll()
        {
            var result = (from c in db.OrderMasters
                select new Orders()
                {
                    OrderId = c.OrderID,
                    OrderNo = c.OrderNo,
                    OrderDate = c.OrderDate,
                    Description = c.Description
                }).ToList();
            return View(result);
        }
        [HttpPost]
        public JsonResult Save(OrderMaster order)
        {
            bool status = false;
            DateTime dateOrg;
            var isValidDate = DateTime.TryParseExact(order.OrderDateString, "mm-dd-yyyy", null, System.Globalization.DateTimeStyles.None, out dateOrg);
            if (isValidDate)
            {
                order.OrderDate = dateOrg;
            }

            var isValidModel = TryUpdateModel(order);
            if (isValidModel)
            {
                using (MyDatabaseEntities db = new MyDatabaseEntities())
                {
                    db.OrderMasters.Add(order);
                    db.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status } };
        }

        public ActionResult Edit(OrderMaster id)
        {
             var result = false;
            try
            {
                OrderDetail od = new OrderDetail();
                if (id.OrderID > 0)
                {
                    OrderMaster om = db.OrderMasters.SingleOrDefault(x => x.OrderID == id.OrderID);
                    om.OrderID = id.OrderID;
                    om.OrderNo = id.OrderNo;
                    om.OrderDate = id.OrderDate;
                    om.OrderDetails = id.OrderDetails;
                    db.SaveChanges();
                    result = true;
                }
                //else
                //{
                //    od.OrderID=om.OrderID = id.OrderId;
                //    om.OrderNo = id.OrderNo;
                //    om.OrderDate = id.OrderDate;
                //    om.OrderDetails = id.Description;
                //    db.OrderMasters.Add(om);
                //    db.OrderDetails.Add(od);
                //    db.SaveChanges();
                //    result = true;
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //return View("ShowAll");
            return Json(result, JsonRequestBehavior.AllowGet);
            throw new NotImplementedException();
          
            
        }

        public ActionResult Delete(int id)
        {
            bool result = false;
            OrderMaster orderid = db.OrderMasters.SingleOrDefault(x => x.OrderID == id);
            OrderDetail orderdetail = db.OrderDetails.SingleOrDefault(x => x.OrderID == id);
            if (orderid != null)
            {
                db.OrderMasters.Remove(orderid);
                db.OrderDetails.Remove(orderdetail);
                db.SaveChanges();
                result = true;
            }
            
            
            return Json(result, JsonRequestBehavior.AllowGet);
            //return View("ShowAll");
            throw new NotImplementedException();
        }


    }
}
