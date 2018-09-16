using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace POS58
{
    /// <summary>
    /// PrintRecept 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class PrintRecept : System.Web.Services.WebService
    {
        SqlConnection conn = new SqlConnection("Server=tcp:boxchang.database.windows.net,1433;Initial Catalog=POS58MENU;Persist Security Info=False;User ID=box;Password=123qweasd!@#;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        public DataSet GetDate()
        {
            SqlDataAdapter CustDataAdapter = new SqlDataAdapter("SELECT * FROM POSOrder o,POSOrderDetail d where PrintFlag='F' AND o.OrderNo = d.OrderNo ORDER BY o.OrderNo", conn);
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
            SqlCommand cmd = new SqlCommand("UPDATE POSOrder SET PrintFlag = 'T' WHERE OrderNo = @OrderNo", conn);
            cmd.Parameters.AddWithValue("OrderNo", OrderNo);

            try
            {
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
