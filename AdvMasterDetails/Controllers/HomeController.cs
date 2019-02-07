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

        public ActionResult Edit(int id)
        {
            throw new NotImplementedException();
        }

        public ActionResult Delete(OrderMaster id)
        {
            // List<OrderMaster> order = new List<OrderMaster>();
            using (MyDatabaseEntities db = new MyDatabaseEntities())
            {
                // order=db.OrderMasters.Where(a => a.OrderID.Equals(id)).OrderBy(a => a.OrderDetails).ToList();
                db.OrderMasters.Remove(id);
                db.SaveChanges();
                //ShowAll();
            }
                throw new NotImplementedException();
        }
    }
}
