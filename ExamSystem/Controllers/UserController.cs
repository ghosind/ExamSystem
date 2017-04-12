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
    public class UserController : Controller
    {
        private static ILog log = LogManager.GetLogger(typeof(UserController)); 

        public ActionResult Login()
        {
            if (!Permission.NotLoginNeed(Request, Response, Session))
            {
                return null;
            }
            return View();
        }

        public ActionResult Register()
        {
            if (!Permission.NotLoginNeed(Request, Response, Session))
            {
                return null;
            }
            return View();
        }

        [HttpPost]
        public JsonResult Logout()
        {
            log.Info("uid: " + ((User)Session["user"]).uid + "已注销");
            Session["user"] = null;

            // delete user cookies
            HttpCookie uidCookie = new HttpCookie("uid", "");
            HttpCookie passwordCookie = new HttpCookie("password", "");

            uidCookie.Expires = passwordCookie.Expires = DateTime.Now.AddMinutes(-1);
            Response.Cookies.Add(uidCookie);
            Response.Cookies.Add(passwordCookie);

            return Json(true);
        }

        [HttpPost]
        public JsonResult LoginWithJson()
        {
            string username = Request["username"];
            string password = Hash.SHA512(Request["password"]);
            string remeber = Request["remember"];

            log.Info(Request.UserHostAddress + "尝试登录" + username);

            User user = UserView.Login(username, password);
            if (user == null || user.rank == (int) UserRank.BLOCKED)
            {
                log.Info(Request.UserHostAddress + "尝试登录" + username + "失败");
                return Json(false);
            }
            else
            {
                log.Info(Request.UserHostAddress + "已登录uid: " + user.uid);
                DateTime time = DateTime.Now;
                UserView.LoginSuccess(user.uid, time);
                Session["user"] = user;

                if (remeber == "on")
                {
                    // store info into cookie to remember user.
                    HttpCookie uidCookie = new HttpCookie("uid", user.uid.ToString());
                    HttpCookie passwordCookie = new HttpCookie("password",
                        Encryption.PasswordCookieEncryption(Request.UserHostAddress, user.password, time));

                    uidCookie.Expires = passwordCookie.Expires = DateTime.Now.AddDays(7);
                    Response.Cookies.Add(uidCookie);
                    Response.Cookies.Add(passwordCookie);
                }

                return Json(true);
            }
        }

        [HttpPost]
        public JsonResult RegisterWithJson()
        {
            string username = Request["username"];
            string email = Request["email"];
            string password = Hash.SHA512(Request["password"]);
            string name = Request["nickname"];
            DateTime time = DateTime.Now;

            log.Info(Request.UserHostAddress + "尝试注册" + username);

            bool result = UserView.Register(username, password, name, time) && UserInfoView.NewUserInfo(username, email, time);

            log.Info(Request.UserHostAddress + "注册" + username + (result?"成功":"失败"));

            return Json(result);
        }

        [HttpPost]
        public JsonResult UserNameValidate()
        {
            string username = Request["username"];
            bool hasExist = UserView.UserNameValidate(username);
            return Json(hasExist);
        }

        [HttpPost]
        public JsonResult EmailValidate()
        {
            string email = Request["email"];
            bool hasExist = UserInfoView.EmailValidate(email);
            return Json(hasExist);
        }

        public static User LoginWithCookie(HttpRequestBase request, HttpSessionStateBase session)
        {
            int uid = Convert.ToInt32(request.Cookies.Get("uid").Value);
            string password = request.Cookies.Get("password").Value;
            User user = UserView.GetUserById(uid);

            if (password == Encryption.PasswordCookieEncryption(request.UserHostAddress, user.password, user.login_date)) {
                log.Info("uid: " + user.uid + "以Cookie方式登录");
                session["user"] = user;
                return user;
            }
            return null;
        }

        public new ActionResult Profile(int id = 0)
        {
            if (!Permission.LoginedNeed(Request, Response, Session))
            {
                return null;
            }

            User loginedUser = (User)Session["user"];
            if (id == 0)
            {
                id = loginedUser.uid;
            }

            User user = UserView.GetUserById(id);
            UserInfo info = UserInfoView.GetUserInfo(id);
            if (user == null || info == null)
            {
                Response.Redirect("/Index");
                return null;
            }

            ViewBag.loginedUser = loginedUser;
            ViewBag.user = user;
            ViewBag.info = info;

            return View();
        }

        public ActionResult Edit(int id = 0)
        {
            if (!Permission.LoginedNeed(Request, Response, Session))
            {
                return null;
            }

            User loginedUser = (User)Session["user"];
            if (id == 0)
            {
                id = loginedUser.uid;
            }

            if (loginedUser.uid != id && loginedUser.rank != (int) UserRank.ADMINISTATOR)
            {
                Response.Redirect("/Index");
                return null;
            }

            User user = UserView.GetUserById(id);
            UserInfo info = UserInfoView.GetUserInfo(id);
            if (user == null || info == null)
            {
                Response.Redirect("/Index");
                return null;
            }

            ViewBag.loginedUser = loginedUser;
            ViewBag.user = user;
            ViewBag.info = info;

            return View();
        }

        [HttpPost]
        public JsonResult SaveProfile()
        {
            if (!Permission.LoginedNeed(Request, Response, Session))
            {
                return Json(false);
            }

            User loginedUser = (User)Session["user"];
            int uid = Convert.ToInt32(Request["id"]);

            log.Info("uid:" + loginedUser.uid + "正在保存用户uid:" + uid + "个人资料");

            User user = UserView.GetUserById(uid);
            UserInfo info = UserInfoView.GetUserInfo(uid);

            if (loginedUser.rank == (int) UserRank.ADMINISTATOR)
            {
                user.rank = Convert.ToInt32(Request["rank"]);
                info.email = Request["email"];
            }
            user.name = Request["nickname"];

            info.sex = Request["sex"];
            info.telephone = Request["telephone"];
            info.address = Request["address"];
            info.description = Request["description"];
            info.birthday = Convert.ToDateTime(Request["birthday"]);

            return Json(UserView.SaveUser(user) && UserInfoView.SaveUserInfo(info));
        }

        public ActionResult Setting()
        {
            if (!Permission.LoginedNeed(Request, Response, Session))
            {
                return null;
            }

            User user = (User)Session["user"];
            ViewBag.info = UserInfoView.GetUserInfo(user.uid);
            ViewBag.user = user;

            return View();
        }

        [HttpPost]
        public JsonResult ChangeEmail()
        {
            if (!Permission.LoginedNeed(Request, Response, Session))
            {
                return Json(false);
            }

            string email = Request["email"];
            User user = (User)Session["user"];
            UserInfo info = UserInfoView.GetUserInfo(user.uid);
            info.email = email;

            return Json(UserInfoView.SaveUserInfo(info));
        }

        [HttpPost]
        public JsonResult PasswordValidate()
        {
            if (!Permission.LoginedNeed(Request, Response, Session))
            {
                return Json(false);
            }

            User user = (User)Session["user"];
            string password = Request["password"];
            if (Hash.SHA512(password) == user.password)
            {
                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }

        [HttpPost]
        public JsonResult ChangePassword()
        {
            if (!Permission.LoginedNeed(Request, Response, Session))
            {
                return Json(false);
            }

            string password = Hash.SHA512(Request["password"]);
            User user = (User)Session["user"];
            user.password = password;

            return Json(UserView.SaveUser(user));
        }

        public ActionResult ForgetPassword()
        {
            if (!Permission.NotLoginNeed(Request, Response, Session))
            {
                return null;
            }

            return View();
        }

        [HttpPost]
        public JsonResult SubmitForget()
        {
            if (!Permission.NotLoginNeed(Request, Response, Session))
            {
                return null;
            }

            string username = Request["username"];
            string email = Request["email"];
            User user = UserView.GetUserByUsername(username);
            UserInfo info = UserInfoView.GetUserInfo(user.uid);

            if (email != info.email)
            {
                return Json(false);
            }

            ForgetPassword fp = new ForgetPassword
            {
                uid = user.uid,
                code = Hash.Md5(user.login_date.ToString() + user.uid + new Random().Next() + DateTime.Now)
            };

            return Json(UserView.ForgetPassword(fp));
        }

        public ActionResult Recover()
        {
            if (!Permission.NotLoginNeed(Request, Response, Session))
            {
                return null;
            }

            string code = Request.QueryString["code"];
            User user = UserView.GetUserById(UserView.GetUIDByCode(code));

            if (user == null)
            {
                Response.Redirect("/Index");
                return null;
            }

            ViewBag.user = user;
            ViewBag.code = code;

            return View();
        }

        [HttpPost]
        public JsonResult ResetPassword()
        {
            if (!Permission.NotLoginNeed(Request, Response, Session))
            {
                return Json(false);
            }

            string code = Request["code"];
            int uid = Convert.ToInt32(Request["uid"]);
            string password = Hash.SHA512(Request["password"]);

            User user = UserView.GetUserById(UserView.GetUIDByCode(code));
            if (user.uid != uid)
            {
                return Json(false);
            }

            user.password = password;

            return Json(UserView.SaveUser(user) && UserView.DeleteForgetPassword(code));
        }

        public ActionResult Result(int id = 1)
        {
            if (!Permission.LoginedNeed(Request, Response, Session))
            {
                return Json(false);
            }

            Pager<Result> pager = new Pager<Result>();

            User user = (User)Session["user"];
            IOrderedQueryable<Result> results = ResultView.GetResultByUid(user.uid);
            ViewBag.page = id;
            ViewBag.results = pager.GetPage(results, id);
            ViewBag.pageNumber = pager.GetPageNumber(results);

            return View();
        }
    }
}