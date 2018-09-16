using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POS58.lib
{
    public class Result
    {
        public string result { get; set; }
        public string name { get; set; }
        public string usid { get; set; }
        public Result(string _result, string _name)
        {
            result = _result;
            name = _name;
        }
    }
}