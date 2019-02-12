using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdvMasterDetails.Models
{
    public class Details
    {
        public int OrderDetailsId { get; set; }
        public string ProductId { get; set; }
        public double Rate { get; set; }
        public double Quantity { get; set; }
        public int OrderId { get; set; }
    }
}