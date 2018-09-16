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
    /// UserData 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    [System.Web.Script.Services.ScriptService]
    public class UserData : System.Web.Services.WebService
    {
        string strConn = "Server=tcp:boxchang.database.windows.net,1433;Initial Catalog=POS58MENU;Persist Security Info=False;User ID=box;Password=123qweasd!@#;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;MultipleActiveResultSets=True";
        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string UserCheck(string ename, string pass)
        {
            string sResult = "NG";
            List<Result> listResults = new List<Result>();
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                conn.Open();
                SqlCommand cmd;
                string sql;
                
                sql = "select ename,sid from Member where ename = @ename and password = @pass";
                cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add("ename", ename);
                cmd.Parameters.Add("pass", pass);
                SqlDataReader rd = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (rd.Read())
                {
                    Result result = new Result("OK", rd["ENAME"].ToString());
                    result.usid = rd["SID"].ToString();
                    listResults.Add(result);
                }

            }
            //Context.Response.ContentType = "application/json; charset=utf-8";
            //Context.Response.Write("{\"result\":\"xxx\"}");

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string jsonData = serializer.Serialize(listResults);
            Context.Response.ContentType = "application/json; charset=utf-8";
            //Context.Response.Write(jsonData);
            return jsonData;
        }

        [WebMethod]
        public void InsertUser(string ename, string phone, string pass, string email)
        {
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                int next_num = 1;
                conn.Open();
                SqlCommand cmd;
                string sql;
                sql = "select count(*)+1 next_num from Member where CONVERT(varchar, getdate(), 11) = '" + DateTime.Now.ToString("yy/MM/dd") + "'";
                cmd = new SqlCommand(sql, conn);
                SqlDataReader rd = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (rd.Read())
                {
                    next_num = Convert.ToInt32(rd["next_num"]);
                }
                
                string _sid = DateTime.Now.ToString("yyMMdd")+next_num.ToString("000");
                string _createDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                User user = new User(_sid, ename, phone, pass, email, _createDate);
                sql = "insert into Member(SID,EName,Phone,Password,Email,Money,CreateDate,UpdateDate,Enable) Values('{0}','{1}','{2}','{3}','{4}',{5},'{6}','{7}','1')";
                sql = string.Format(sql, user.sid, user.ename, user.phone, user.password, user.email, user.money, user.createdate, user.createdate);
                cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();

                conn.Close();
            }
        }
    }

}
