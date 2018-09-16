using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POS58
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string strConn = "Server=tcp:boxchang.database.windows.net,1433;Initial Catalog=POS58MENU;Persist Security Info=False;User ID=box;Password=123qweasd!@#;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            //建立連接
            SqlConnection myConn = new SqlConnection(strConn);
            //打開連接
            myConn.Open();
            string sOrderNo = DateTime.Now.ToString("yyyyMMdd hhmmss");
            int iQty = Convert.ToInt32(ddlQty.SelectedValue);
            int iTotal = iQty * 30;
            SqlCommand cmd = new SqlCommand("insert into POSOrder Values('" + sOrderNo + "','Sausage'," + iQty + ",30," + iTotal + ",getdate(),'F')", myConn);
            cmd.ExecuteNonQuery();
            myConn.Close();
        }
    }
}