using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdvMasterDetails.Models;

namespace AdvMasterDetails.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

     
        public JsonResult GetProductCategories()
        {
            List<Category> categories;
            using (MyDatabaseEntities dc = new MyDatabaseEntities())
            {
                categories = dc.Categories.OrderBy(a => a.CategortyName).ToList();
            }
            return new JsonResult { Data = categories, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetProducts(int categoryID)
        {
            List<Product> products = new List<Product>();
            using (MyDatabaseEntities dc = new MyDatabaseEntities())
            {
                products = dc.Products.Where(a => a.CategoryID.Equals(categoryID)).OrderBy(a => a.ProductName).ToList();
            }
            return new JsonResult { Data = products, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult ShowAll()
        {
            //String connnectionString = "data source=(LocalDB)\\MSSQLLocalDB;attachdbfilename=|DataDirectory|\\MyDatabase.mdf;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework";
            //SqlConnection con = new SqlConnection(connnectionString);
            //if (con.State == ConnectionState.Closed)
            //{
            //    con.Open();
            //}
            //String sql = "SELECT * FROM OrderMaster";
            //SqlCommand cmd = new SqlCommand(sql, con);

            //var model = new List<Orders>();
            //using (SqlConnection conn = new SqlConnection(connnectionString))
            //{
            //    conn.Open();
            //    SqlDataReader rdr = cmd.ExecuteReader();
            //    while (rdr.Read())
            //    {
            //        var order = new Orders();
            //        order.OrderId = (int) rdr["OrderId"];
            //        order.OrderNo = (string) rdr["OrderNo"];
            //        order.OrderDate = (DateTime) rdr["OrderDate"];
            //        order.Description = rdr["Description"].ToString();


            //        model.Add(order);
            //    }
            //    conn.Close();
            //}
            //con.Close();
            //return View(model);
            MyDatabaseEntities db = new MyDatabaseEntities();
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
            //var isValidDate = DateTime.TryParseExact(order.OrderDate.ToString(), "mm-dd-yyyy", null,
            //    System.Globalization.DateTimeStyles.None, out dateOrg);
            var isValidDate = DateTime.TryParseExact(order.OrderDateString, "mm-dd-yyyy", null, System.Globalization.DateTimeStyles.None, out dateOrg);
            if (isValidDate)
            {
                order.OrderDate = dateOrg;
            }

            var isValidModel = TryUpdateModel(order);
            if (isValidModel)
            {
                using (MyDatabaseEntities dc = new MyDatabaseEntities())
                {
                    dc.OrderMasters.Add(order);
                    dc.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status } };
        }

        public ActionResult Edit(int id)
        {
            throw new NotImplementedException();
        }
    }
}
