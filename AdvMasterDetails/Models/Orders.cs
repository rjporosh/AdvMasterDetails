﻿using System;
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

        public List<OrderDetail> OrderDetails { get; set; }

        public int OrderDetailsId { get; set; }
        public string ProductId { get; set; }
        public double Rate { get; set; }
        public double Quantity { get; set; }

    }
}