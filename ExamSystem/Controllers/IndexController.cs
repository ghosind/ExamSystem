using System.Web.Mvc;
using log4net;

namespace ExamSystem.Controllers
{
    public class IndexController : Controller
    {
        private ILog log = LogManager.GetLogger(typeof(IndexController));
        // GET: Index
        public ActionResult Index()
        {
            log.Info(Request.UserHostAddress + "访问主页。");
            return View();
        }
    }
}