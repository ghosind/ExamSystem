using ExamSystem.Models;
using ExamSystem.Toolkit;
using ExamSystem.ViewModels;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExamSystem.Controllers
{
    public class SearchController : Controller
    {
        private ILog log = LogManager.GetLogger(typeof(NewsController));

        // GET: Search
        public void Index()
        {
            string type = Request.QueryString["type"];
            string content = Request.QueryString["content"];
            string url = "";

            switch (type)
            {
                case "2":
                    url = "/Search/Exams";
                    break;
                case "3":
                    url = "/Search/Groups";
                    break;
                case "4":
                    url = "/Search/Users";
                    break;
                case "1":
                default:
                    url = "/Search/News";
                    break;
            }

            url += "?content=" + content;
            Response.Redirect(url);
        }

        public ActionResult News(int id = 1)
        {
            Pager<News> pager = new Pager<News>();

            string keyword = Request.QueryString["content"];
            IOrderedQueryable<News> news = NewsView.GetNewsByKeyword(keyword);
            ViewBag.page = id;
            ViewBag.news = pager.GetPage(news, id);
            ViewBag.pageNumber = pager.GetPageNumber(news);
            ViewBag.keyword = keyword;

            return View();
        }

        public ActionResult Exams(int id = 1)
        {
            Pager<Exam> pager = new Pager<Exam>();

            string keyword = Request.QueryString["content"];
            IOrderedQueryable<Exam> exams = ExamView.GetExamsByKeyword(keyword);
            ViewBag.page = id;
            ViewBag.exams = pager.GetPage(exams, id);
            ViewBag.pageNumber = pager.GetPageNumber(exams);
            ViewBag.keyword = keyword;

            return View();
        }

        public ActionResult Groups(int id = 1)
        {
            Pager<Group> pager = new Pager<Group>();

            string keyword = Request.QueryString["content"];
            IOrderedQueryable<Group> groups = GroupView.GetGroupByKeywork(keyword);
            ViewBag.page = id;
            ViewBag.groups = pager.GetPage(groups, id);
            ViewBag.pageNumber = pager.GetPageNumber(groups);
            ViewBag.keyword = keyword;

            return View();
        }

        public ActionResult Users(int id = 1)
        {
            Pager<User> pager = new Pager<User>();

            string keyword = Request.QueryString["content"];
            IOrderedQueryable<User> users = UserView.GetUserByKeyword(keyword);
            ViewBag.page = id;
            ViewBag.users = pager.GetPage(users, id);
            ViewBag.pageNumber = pager.GetPageNumber(users);
            ViewBag.keyword = keyword;

            return View();
        }
    }
}