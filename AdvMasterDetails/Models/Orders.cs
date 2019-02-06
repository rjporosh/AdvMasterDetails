using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdvMasterDetails.Models
{
    public class Orders
    {
        public int OrderId { get; set; }
        public  string OrderNo { get; set; }
        public  DateTime OrderDate { get; set; }
        public  string Description { get; set; }
    }
}