using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using POS58.lib;

namespace POS58.Mobile.Service
{
    /// <summary>
    /// OrderData 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)] 
    [System.Web.Script.Services.ScriptService]
    public class OrderData : System.Web.Services.WebService
    {
        string strConn = "Server=tcp:boxchang.database.windows.net,1433;Initial Catalog=POS58MENU;Persist Security Info=False;User ID=box;Password=123qweasd!@#;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;MultipleActiveResultSets=True";
        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        public void InsertOrder(string order,string usid,string desc)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Item> ItemList = serializer.Deserialize(order, typeof(List<Item>)) as List<Item>;
            string sOrderNo = DateTime.Now.ToString("yyyyMMddHHmmss");
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                
                conn.Open();
                SqlCommand cmd;
                SqlCommand cmd2;
                string order_sql;
                string sql;
                int total = 0;

                foreach(Item item in ItemList)
                {
                    sql = "insert into POSOrderDetail(OrderNo,ItemSID,ItemQty,Price,Total,CreateDate) Values('{0}','{1}',{2},{3},{4},'{5}')";
                    sql = string.Format(sql, sOrderNo, item.sid, item.num, item.dprice, item.num * item.dprice, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                    cmd = new SqlCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                    total += item.num * item.dprice;
                }

                order_sql = "insert into POSOrder(OrderNo,UserId,CreateDate,Enable,PrintFlag,Total,PayType,ODESC) Values('{0}','{1}','{2}','{3}','{4}',{5},'{6}',N'{7}')";
                order_sql = string.Format(order_sql, sOrderNo, usid, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), "T", "F",total,"1",desc);
                cmd2 = new SqlCommand(order_sql, conn);
                cmd2.ExecuteNonQuery();

                conn.Close();
            }
        }

        [WebMethod]
        public string getAllOrder(string usid)
        {
            
            List<Order> listOrders = new List<Order>();
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                SqlCommand cmd = new SqlCommand("Select * from POSOrder where enable = 'T' and UserId = @usid order by createDate desc", conn);
                conn.Open();
                cmd.Parameters.Add("usid", usid);
                SqlDataReader rd = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (rd.Read())
                {
                    List<OrderDetail> listDetails = new List<OrderDetail>();
                    Order order = new Order();
                    order.sid = Convert.ToInt32(rd["SID"]);
                    order.orderno = rd["OrderNo"].ToString();
                    order.userid = rd["UserId"].ToString();
                    order.printflag = rd["PrintFlag"].ToString();
                    order.createdate = rd["CreateDate"].ToString();
                    order.paytype = rd["PayType"].ToString();
                    order.total = Convert.ToInt32(rd["Total"]);
                    order.odesc = rd["ODESC"].ToString();
                    string sql2 = @"Select * from POSOrderDetail d,POSITEM i 
                                    where OrderNo = '{0}' 
                                    and d.itemsid = i.sid
                                    order by d.createDate desc";
                    sql2 = string.Format(sql2, order.orderno);
                    SqlCommand cmd2 = new SqlCommand(sql2, conn);
                    SqlDataReader rd2 = cmd2.ExecuteReader(CommandBehavior.CloseConnection);
                    while (rd2.Read())
                    {
                        OrderDetail orderdetail = new OrderDetail();
                        orderdetail.sid = Convert.ToInt32(rd2["SID"]);
                        orderdetail.orderno = rd2["OrderNo"].ToString();
                        orderdetail.item = rd2["ITEM"].ToString();
                        orderdetail.itemqty = rd2["ItemQty"].ToString();
                        orderdetail.dprice = Convert.ToInt32(rd2["DPRICE"]);
                        orderdetail.price = Convert.ToInt32(rd2["PRICE"]);
                        orderdetail.total = Convert.ToInt32(rd2["TOTAL"]);
                        orderdetail.createdate = rd2["CreateDate"].ToString();
                        listDetails.Add(orderdetail);
                    }
                    order.orderdetail = listDetails;

                    listOrders.Add(order);

                }
                conn.Close();
                JavaScriptSerializer js = new JavaScriptSerializer();
                return js.Serialize(listOrders);
            }
        }

        [WebMethod]
        public void getOrderItem(string OrderNo)
        {

        }

        [WebMethod]
        public DataSet getPrintOrderData()
        {
            SqlConnection conn = new SqlConnection(strConn);
            string sql = @"SELECT m.EName,o.OrderNo,UserId,o.Total,i.Item,d.ItemQty,i.dPrice,d.total as itotal,o.createDate,o.ODESC
                            FROM POSOrder o,POSOrderDetail d,POSITem i,Member m
                            where PrintFlag='F' 
                            AND d.ItemSID = i.SID
                            AND o.UserId = m.SID
                            AND CONVERT(varchar, getdate(), 11) = '" + DateTime.Now.ToString("yy/MM/dd") + @"'
                            AND o.OrderNo = d.OrderNo ORDER BY o.OrderNo";
            SqlDataAdapter CustDataAdapter = new SqlDataAdapter(sql, conn);
            DataSet CustDataSet = new DataSet();
            conn.Open();
            CustDataAdapter.Fill(CustDataSet);
            conn.Close();
            return CustDataSet;
        }

        [WebMethod]
        public void UpdatePrintFlag(string OrderNo)
        {
            int rowsAffected = -1;
            SqlConnection conn = new SqlConnection(strConn);
            try
            {
                
                SqlCommand cmd = new SqlCommand("UPDATE POSOrder SET PrintFlag = 'T' WHERE OrderNo = @OrderNo", conn);
                cmd.Parameters.AddWithValue("OrderNo", OrderNo);
                conn.Open();
                rowsAffected = cmd.ExecuteNonQuery();
            }
            finally
            {
                if (conn != null) { conn.Close(); }
            }
        }
    }
}
