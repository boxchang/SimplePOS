using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POS58.lib
{
    public class OrderDetail
    {
        public int sid { get; set; }
        public string orderno { get; set; }
        public string itemsid { get; set; }
        public string item { get; set; }
        public string itemqty { get; set; }
        public int price { get; set; }
        public int dprice { get; set; }
        public int total { get; set; }
        public string createdate { get; set; }
    }
}