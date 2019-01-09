using System;
using System.Linq;
using System.Web.Mvc;
using ExamSystem.Models;
using ExamSystem.Toolkit;
using ExamSystem.ViewModels;
using log4net;

namespace ExamSystem.Controllers
{
    public class MessageController : Controller
    {
        private ILog log = LogManager.GetLogger(typeof(NewsController));
        private Pager<Message> pager = new Pager<Message>();

        // GET: Message
        public ActionResult Index(int id = 1)
        {
            if (!Permission.LoginedNeed(Request, Response, Session))
            {
                return null;
            }

            User user = (User)Session["user"];
            IOrderedQueryable<Message> messages = MessageView.GetRecivedMessage(user.uid);
            ViewBag.messages = pager.GetPage(messages, id);
            ViewBag.page = id;
            ViewBag.pageNumber = pager.GetPageNumber(messages);

            return View();
        }

        public ActionResult SendBox(int id = 1)
        {
            if (!Permission.LoginedNeed(Request, Response, Session))
            {
                return null;
            }

            User user = (User)Session["user"];
            IOrderedQueryable<Message> messages = MessageView.GetSendMessage(user.uid);
            ViewBag.messages = pager.GetPage(messages, id);
            ViewBag.page = id;
            ViewBag.pageNumber = pager.GetPageNumber(messages);

            return View();
        }

        public ActionResult Detail(int id = 1)
        {
            if (!Permission.LoginedNeed(Request, Response, Session))
            {
                return null;
            }

            User user = (User)Session["user"];

            log.Info("uid:" + user.uid + "访问消息id:" + id);

            Message message = MessageView.GetMessage(id);
            if (user.uid != message.sender && user.uid != message.receiver)
            {
                Response.Redirect("/Message");
            }

            bool isReceiver = false;
            if (user.uid == message.receiver)
            {
                if (message.read == false)
                {
                    MessageView.ReadMessage(id);
                }
                isReceiver = true;
            }

            ViewBag.message = message;
            ViewBag.isReceiver = isReceiver;

            return View();
        }

        public ActionResult New(int id = 0)
        {
            if (!Permission.LoginedNeed(Request, Response, Session))
            {
                return null;
            }

            User receiver = UserView.GetUserById(id);

            ViewBag.receiver = receiver;

            return View();
        }

        [HttpPost]
        public JsonResult Send()
        {

            string username = Request["username"];
            User receiver = UserView.GetUserByUsername(username);
            if (!Permission.LoginedNeed(Request, Response, Session) || receiver == null)
            {
                return Json(false);
            }

            string title = Request["title"];
            string content = Request["content"];
            
            User sender = (User) Session["user"];

            log.Info("uid:" + sender.uid + "发送消息至uid:" + receiver.uid);

            Message message = new Message
            {
                sender = sender.uid,
                receiver = receiver.uid,
                title = title,
                content = content,
                send_date = DateTime.Now,
                read = false
            };
            
            return Json(MessageView.SendMessage(message));
        }
    }
}