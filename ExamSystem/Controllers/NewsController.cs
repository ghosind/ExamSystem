using System;
using System.Linq;
using System.Web.Mvc;
using ExamSystem.Models;
using ExamSystem.Toolkit;
using ExamSystem.ViewModels;
using log4net;

namespace ExamSystem.Controllers
{
    public class NewsController : Controller
    {
        private ILog log = LogManager.GetLogger(typeof(NewsController));
        private static Pager<News> pager = new Pager<News>();
        private static IOrderedQueryable<News> newsCache = null;

        // GET: News
        public ActionResult Index(int id = 1)
        {
            IOrderedQueryable<News> news = null;
            if (newsCache != null)
            {
                news = newsCache;
            } else
            {
                news = NewsView.GetAllNews();
                newsCache = news;
            }

            int totalPageNumber = pager.GetPageNumber(news);
            if (id < 1 || id > totalPageNumber)
            {
                id = 1;
            }

            ViewBag.news = pager.GetPage(news, id);
            ViewBag.totalPageNumber = totalPageNumber;
            ViewBag.page = id;

            return View();
        }

        public ActionResult Detail(int id = 1)
        {
            News news = NewsView.GetNewsById(id);
            if (news == null)
            {
                Response.Redirect("/Index");
                return null;
            }

            ViewBag.news = news;
            return View();
        }

        public ActionResult New()
        {
            log.Info(Request.UserHostAddress + "访问新闻发布页面");
            if (!Permission.PremissionNeed(Request, Response, Session, UserRank.TEACHER))
            {
                return null;
            }
            return View();
        }

        public void Post()
        {
            if (!Permission.PremissionNeed(Request, Response, Session, UserRank.TEACHER))
            {
                return ;
            }

            User user = (User) Session["user"];
            string title = Request.Form["title"];
            string content = Request.Form["content"];

            if (title.Length != 0 && content.Length != 0)
            {
                log.Info("uid:" + user.uid + "发布新闻：" + title);
                News news = new News
                {
                    publisher = user.uid,
                    title = title,
                    content = content,
                    date = DateTime.Now
                };
                NewsView.PostNewNews(news);
            }

            newsCache = NewsView.GetAllNews();
            Response.Redirect("/News");
        }

        public ActionResult Edit(int id = 1)
        {
            if (!Permission.PremissionNeed(Request, Response, Session, UserRank.ADMINISTATOR))
            {
                return null;
            }

            log.Info(Request.UserHostAddress + "访问新闻编辑页面，nid:" + id);
            News news = NewsView.GetNewsById(id);
            if (news == null)
            {
                Response.Redirect("/Index");
                return null;
            }
            ViewBag.news = news;

            return View();
        }

        public void Save(int id)
        {
            if (!Permission.PremissionNeed(Request, Response, Session, UserRank.TEACHER))
            {
                return ;
            }

            User user = (User)Session["user"];
            string title = Request.Form["title"];
            string content = Request.Form["content"];

            if (title.Length != 0 && content.Length != 0)
            {
                log.Info("uid:" + user.uid + "编辑新闻id：" + id);
                NewsView.SaveNews(id, title, content);
            }
            
            Response.Redirect("/News");
        }

        [HttpPost]
        public JsonResult Delete()
        {
            User user = (User) Session["user"];
            int nid = Convert.ToInt32(Request["id"]);

            log.Info("uid:" + user.uid + "进行删除新闻操作，新闻id:" + nid);

            if (user.rank != (int) UserRank.ADMINISTATOR)
            {
                return Json(false);
            }

            bool result = NewsView.DeleteNews(nid);
            newsCache = NewsView.GetAllNews();

            return Json(result);
        }
    }
}