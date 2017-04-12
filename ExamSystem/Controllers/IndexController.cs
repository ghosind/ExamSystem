using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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