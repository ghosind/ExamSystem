using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Web.Mvc;
using ExamSystem.Toolkit;
using System.Web.Configuration;
using log4net;

namespace ExamSystem.Controllers
{
    public class InstallController : Controller
    {
        ILog log = LogManager.GetLogger(typeof(InstallController));
        // GET: Install
        public ActionResult Index()
        {
            log.Info(Request.UserHostAddress + "访问系统初始化页面。");
            if (ConfigurationManager.ConnectionStrings["ExamSystemContext"].ConnectionString.Length != 0)
            {
                Response.Redirect("/Index");
            }
            return View();
        }

        public void Install()
        {
            log.Info(Request.UserHostAddress + "执行系统初始化。");
            // concatenate connection string
            string connectionString = "data source = {server}; initial catalog = {dbname};integrated security = {type};" +
                "user id = {dbusername}; password = {dbpassword}; MultipleActiveResultSets = True; App = EntityFramework";
            string[] keywords = { "server", "dbname", "type", "dbusername", "dbpassword" };
            foreach (string key in keywords)
            {
                connectionString = connectionString.Replace("{" + key + "}", Request.Form[key]);
            }

            try
            {
                // get sql file path and create table into database.
                string path = HttpContext.Server.MapPath("/install/ExamSystem.sql");
                FileInfo file = new FileInfo(path);
                string dbInstallString = file.OpenText().ReadToEnd();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(dbInstallString, connection);
                    command.Parameters.AddWithValue("@username", Request.Form["username"]);
                    command.Parameters.AddWithValue("@password", Hash.SHA512(Request.Form["password"]));
                    command.Parameters.AddWithValue("@nickname", Request.Form["nickname"]);
                    command.Parameters.AddWithValue("@email", Request.Form["email"]);

                    command.Connection.Open();
                    command.ExecuteNonQuery();
                    command.Connection.Close();
                }
            }
            catch (Exception e)
            {
                log.Fatal(e.Message);
                Response.Redirect("/Install/Index?error=1");
                return;
            }

            // write connection string into Web.config
            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            ConnectionStringsSection connectionSettings = (ConnectionStringsSection)config.GetSection("connectionStrings");
            connectionSettings.ConnectionStrings["ExamSystemContext"].ConnectionString = connectionString;
            config.Save();
            log.Info("已完成初始化，管理员" + Request.Form["username"]);

            Response.Redirect("/Index");
        }
    }
}