using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using POS58.lib;

namespace POS58.Mobile.Service
{
    /// <summary>
    /// ItemData 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class ItemData : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        public void getCookieItemData()
        {
            string strConn = "Server=tcp:boxchang.database.windows.net,1433;Initial Catalog=POS58MENU;Persist Security Info=False;User ID=box;Password=123qweasd!@#;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            List<Item> listItems = new List<Item>();
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                SqlCommand cmd = new SqlCommand("Select * from POSITEM where enable = 'T' and StoreSID = 1",conn);
                conn.Open();
                SqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    Item item = new Item();
                    item.sid = Convert.ToInt32(rd["SID"]);
                    item.item = rd["ITEM"].ToString();
                    item.photo = rd["PHOTO"].ToString();
                    item.price = Convert.ToInt32(rd["PRICE"]);
                    item.dprice = Convert.ToInt32(rd["DPRICE"]);
                    item.createdate = rd["CREATEDATE"].ToString();
                    item.num = 1;
                    listItems.Add(item);

                }
                JavaScriptSerializer js = new JavaScriptSerializer();
                Context.Response.Write(js.Serialize(listItems));
                conn.Close();
            }

        }

        [WebMethod]
        public void getPushPinItemData()
        {
            string strConn = "Server=tcp:boxchang.database.windows.net,1433;Initial Catalog=POS58MENU;Persist Security Info=False;User ID=box;Password=123qweasd!@#;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            List<Item> listItems = new List<Item>();
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                SqlCommand cmd = new SqlCommand("Select * from POSITEM where enable = 'T' and StoreSID = 2", conn);
                conn.Open();
                SqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    Item item = new Item();
                    item.sid = Convert.ToInt32(rd["SID"]);
                    item.item = rd["ITEM"].ToString();
                    item.photo = rd["PHOTO"].ToString();
                    item.price = Convert.ToInt32(rd["PRICE"]);
                    item.dprice = Convert.ToInt32(rd["DPRICE"]);
                    item.createdate = rd["CREATEDATE"].ToString();
                    item.num = 1;
                    listItems.Add(item);

                }
                JavaScriptSerializer js = new JavaScriptSerializer();
                Context.Response.Write(js.Serialize(listItems));
                conn.Close();
            }

        }
    }
}
