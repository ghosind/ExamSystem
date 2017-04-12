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
    public class GroupController : Controller
    {
        private ILog log = LogManager.GetLogger(typeof(GroupController));
        private Pager<Group> pager = new Pager<Group>();
        private IOrderedQueryable<Group> groupsCache = null;

        // GET: Group
        public ActionResult Index(int id = 1)
        {
            if (!Permission.LoginedNeed(Request, Response, Session))
            {
                return null;
            }

            IOrderedQueryable<Group> groups = null;

            if (groupsCache != null)
            {
                groups = groupsCache;
            }
            else
            {
                groups = GroupView.GetAllGroups();
                groupsCache = groups;
            }

            int totalPageNumber = pager.GetPageNumber(groups);
            if (id > totalPageNumber || id < 1)
            {
                id = 1;
            }
            ViewBag.totalPageNumber = totalPageNumber;
            ViewBag.groups = pager.GetPage(groups, id);
            ViewBag.page = id;
            ViewBag.user = (User)Session["user"];

            return View();
        }

        public ActionResult Create()
        {
            if (!Permission.PremissionNeed(Request, Response, Session, UserRank.TEACHER))
            {
                return null;
            }

            return View();
        }

        [HttpPost]
        public JsonResult SubmitCreation()
        {
            if (!Permission.PremissionNeed(Request, Response, Session, UserRank.TEACHER))
            {
                return Json(0);
            }

            string groupName = Request["groupname"];
            bool allowJoin = Convert.ToBoolean(Request["allowjoin"]);
            bool allowquit = Convert.ToBoolean(Request["allowquit"]);
            User user = (User)Session["user"];

            Group group = new Group
            {
                group_name = groupName,
                owner_uid = user.uid,
                number = 0,
                allow_join = allowJoin,
                allow_quit = allowquit
            };

            if (!GroupView.NewGroup(group))
            {
                return Json(0);
            }

            groupsCache = GroupView.GetAllGroups();

            GroupMember member = new GroupMember
            {
                gid = group.gid,
                uid = user.uid,
                rank = (int)MemberRank.CREATOR
            };

            GroupView.AddMember(group.gid, member);

            return Json(group.gid);
        }

        [HttpPost]
        public JsonResult Join()
        {
            if (!Permission.LoginedNeed(Request, Response, Session))
            {
                return Json(false);
            }

            User user = (User)Session["user"];
            int gid = Convert.ToInt32(Request["gid"]);
            Group group = GroupView.GetGroupById(gid);
            if (group == null || group.allow_join == false)
            {
                Json(false);
            }
            if (GroupView.GetGroupMember(user.uid, gid) != null)
            {
                return Json(false);
            }
            GroupMember member = new GroupMember
            {
                uid = user.uid,
                gid = gid,
                rank = (int)MemberRank.MEMBER
            };

            return Json(GroupView.AddMember(gid, member));
        }

        public ActionResult Detail(int id = 0)
        {
            if (!Permission.LoginedNeed(Request, Response, Session))
            {
                return null;
            }

            User user = (User)Session["user"];
            GroupMember memberInfo = GroupView.GetGroupMember(user.uid, id);
            if (memberInfo == null)
            {
                Permission.BackToPrevPageOrIndex(Request, Response);
                return null;
            }

            ViewBag.memberInfo = memberInfo;
            ViewBag.group = GroupView.GetGroupById(id);
            ViewBag.members = GroupView.GetGroupMemberByGID(id).Take(5).ToList();

            return View();
        }

        public ActionResult Members(int id = 0)
        {
            if (!Permission.LoginedNeed(Request, Response, Session))
            {
                return null;
            }

            User user = (User)Session["user"];
            GroupMember memberInfo = GroupView.GetGroupMember(user.uid, id);
            if (memberInfo == null)
            {
                Permission.BackToPrevPageOrIndex(Request, Response);
                return null;
            }

            ViewBag.memberInfo = memberInfo;
            ViewBag.members = GroupView.GetGroupMemberByGID(id).ToList();

            return View();
        }

        [HttpPost]
        public JsonResult SubmitChange()
        {
            if (!Permission.LoginedNeed(Request, Response, Session))
            {
                return Json(false);
            }
            User user = (User)Session["user"];
            int gid = Convert.ToInt32(Request["gid"]);
            int uid = Convert.ToInt32(Request["uid"]);
            int rank = Convert.ToInt32(Request["rank"]);

            GroupMember opInfo = GroupView.GetGroupMember(user.uid, gid);
            GroupMember memberInfo = GroupView.GetGroupMember(uid, gid);
            if (opInfo == null || memberInfo == null || 
                rank >= 3 || rank < 0 ||
                opInfo.rank < (int) MemberRank.ADMINISTRATOR ||
                memberInfo.rank == (int) MemberRank.CREATOR)
            {
                return Json(false);
            }

            memberInfo.rank = rank;

            return Json(GroupView.SaveGroupMember(memberInfo));
        }

        [HttpPost]
        public JsonResult Dismiss()
        {
            if (!Permission.LoginedNeed(Request, Response, Session))
            {
                return Json(false);
            }

            User user = (User)Session["user"];
            int gid = Convert.ToInt32(Request["gid"]);
            GroupMember memberInfo = GroupView.GetGroupMember(user.uid, gid);

            if (memberInfo == null || memberInfo.rank != (int) MemberRank.CREATOR)
            {
                return Json(false);
            }

            groupsCache = GroupView.GetAllGroups();
            return Json(GroupView.DismissGroup(gid));
        }

        [HttpPost]
        public JsonResult Quit()
        {
            if (!Permission.LoginedNeed(Request, Response, Session))
            {
                return Json(false);
            }

            User user = (User)Session["user"];
            int gid = Convert.ToInt32(Request["gid"]);
            GroupMember memberInfo = GroupView.GetGroupMember(user.uid, gid);

            if (memberInfo == null || memberInfo.rank == (int)MemberRank.CREATOR)
            {
                return Json(false);
            }

            return Json(GroupView.QuitGroup(memberInfo));
        }

        public ActionResult AddMember(int id = 0)
        {
            if (!Permission.LoginedNeed(Request, Response, Session))
            {
                return null;
            }

            User user = (User)Session["user"];
            GroupMember memberInfo = GroupView.GetGroupMember(user.uid, id);
            if (memberInfo == null || memberInfo.rank < (int) MemberRank.ADMINISTRATOR)
            {
                Permission.BackToPrevPageOrIndex(Request, Response);
                return null;
            }
            ViewBag.memberInfo = memberInfo;

            return View();
        }

        [HttpPost]
        public JsonResult Add()
        {
            if (!Permission.LoginedNeed(Request, Response, Session))
            {
                return Json(false);
            }

            User user = (User)Session["user"];
            int gid = Convert.ToInt32(Request["gid"]);
            string username = Request["username"];

            GroupMember memberInfo = GroupView.GetGroupMember(user.uid, gid);
            if (memberInfo == null || memberInfo.rank < (int)MemberRank.ADMINISTRATOR)
            {
                return Json(false);
            }

            User newMember = UserView.GetUserByUsername(username);
            if (newMember == null)
            {
                return Json(false);
            }

            GroupMember newMemberInfo = new GroupMember
            {
                uid = newMember.uid,
                gid = gid,
                rank = 0
            };

            return Json(GroupView.AddMember(gid, newMemberInfo));
        }

        public ActionResult Exams(int id = 0)
        {
            if (!Permission.LoginedNeed(Request, Response, Session))
            {
                return null;
            }

            User user = (User)Session["user"];
            GroupMember memberInfo = GroupView.GetGroupMember(user.uid, id);
            if (memberInfo == null)
            {
                Permission.BackToPrevPageOrIndex(Request, Response);
                return null;
            }

            Pager<Exam> pager = new Pager<Exam>();

            ViewBag.memberInfo = memberInfo;
            ViewBag.exams = ExamView.GetAllExamByGroup(id).Where(e => e.end_date < DateTime.Now).ToList();

            return View();
        }
    }
}