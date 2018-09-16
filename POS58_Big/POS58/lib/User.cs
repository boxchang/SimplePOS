using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POS58.lib
{
    public class User
    {
        public string sid { get; set; }
        public string ename { get; set; }
        public string phone { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public int money { get; set; }
        public string createdate { get; set; }
        public DateTime updatedate { get; set; }
        public string enable { get; set; }

        public User(string _sid, string _ename, string _phone, string _pass, string _email, string _createdate)
        {
            sid = _sid;
            ename = _ename;
            phone = _phone;
            password = _pass;
            email = _email;
            createdate = _createdate;
        }
    }
}