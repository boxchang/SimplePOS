using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POS58.lib
{
    public class Order
    {
        public int sid { get; set; }
        public string orderno { get; set; }
        public string userid { get; set; }
        public string enable { get; set; }
        public string printflag { get; set; }
        public string createdate { get; set; }
        public List<OrderDetail> orderdetail { get; set; }
        public string paytype { get; set; }
        public int total { get; set; }
        public string odesc { get; set; }
    }
}