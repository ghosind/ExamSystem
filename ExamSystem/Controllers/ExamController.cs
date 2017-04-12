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
    public class ExamController : Controller
    {
        private ILog log = LogManager.GetLogger(typeof(NewsController));
        private Pager<Exam> pager = new Pager<Exam>();
        private IOrderedQueryable<Exam> examsCache = null;

        // GET: Exam
        public ActionResult Index(int id = 1)
        {
            IOrderedQueryable<Exam> exams = null;
            if (examsCache != null)
            {
                exams = examsCache;
            }
            else
            {
                exams = ExamView.GetAllExams();
                examsCache = exams;
            }

            int totalPageNumber = pager.GetPageNumber(exams);
            if (id > totalPageNumber || id < 1)
            {
                id = 1;
            }
            ViewBag.totalPageNumber = totalPageNumber;
            ViewBag.exams = pager.GetPage(exams, id);
            ViewBag.page = id;

            return View();
        }

        public ActionResult MyExams(int id = 1)
        {
            if (!Permission.LoginedNeed(Request, Response, Session))
            {
                return null;
            }

            User user = (User)Session["user"];
            Pager<Exam> pager = new Pager<Exam>();
            
            IOrderedQueryable<Exam> exams = ExamView.GetAllExamByUser(user.uid);
            ViewBag.page = id;
            ViewBag.exams = pager.GetPage(exams, id);
            ViewBag.pageNumber = pager.GetPageNumber(exams);

            return View();
        }

        public ActionResult Detail(int id = 0)
        {
            if (id == 0 || !Permission.LoginedNeed(Request, Response, Session))
            {
                return null;
            }

            ViewBag.exam = ExamView.GetExamById(id);
            ViewBag.groups = ExamView.GetAllGroupsByExam(id).ToList();
            
            return View();
        }

        public ActionResult New()
        {
            if (!Permission.PremissionNeed(Request, Response, Session, UserRank.TEACHER))
            {
                return null;
            }

            List<Subject> subjects = SubjectView.GetAllSubject().ToList();
            if (subjects == null)
            {
                Permission.BackToPrevPageOrIndex(Request, Response);
                return null;
            }

            ViewBag.subjects = subjects;
            ViewBag.groups = GroupView.GetAllGroups().ToList();

            return View();
        }

        [HttpPost]
        public JsonResult GetQuestions()
        {
            if (!Permission.PremissionNeed(Request, Response, Session, UserRank.TEACHER))
            {
                return Json(null);
            }

            int section = Convert.ToInt32(Request["section"]);
            int type = Convert.ToInt32(Request["type"]);
            
            switch (type)
            {
                case 0:
                    return Json(ExamView.GetChoiceQuestionBySection(section, 0));
                case 1:
                    return Json(ExamView.GetChoiceQuestionBySection(section, 1));
                case 2:
                    return Json(ExamView.GetFillinQuestionBySection(section));
                case 3:
                    return Json(ExamView.GetDiscussQuestionBySection(section));
                default:
                    break;
            }

            return Json(null);
        }

        [HttpPost]
        public JsonResult SaveExam()
        {
            if (!Permission.PremissionNeed(Request, Response, Session, UserRank.TEACHER))
            {
                return Json(0);
            }

            string title = Request["title"];
            int subject = Convert.ToInt32(Request["subject"]);
            int time = Convert.ToInt32(Request["time"]);
            DateTime start = Convert.ToDateTime(Request["start"]);
            DateTime end = Convert.ToDateTime(Request["end"]);
            bool ispublic = Convert.ToBoolean(Request["public"]);
            bool musttake = Convert.ToBoolean(Request["musttake"]);
            string[] groups = Request["groups"].Trim().Split(' ');

            string exam_file = Request["exam_file"];
            string result_file = Request["result_file"];
            exam_file = exam_file.Replace("[lt]", "<").Replace("[rt]", ">").Replace("[flt]", "&lt;").Replace("[frt]", "&rt;");
            result_file = result_file.Replace("[lt]", "<").Replace("[rt]", ">").Replace("[flt]", "&lt;").Replace("[frt]", "&rt;");

            string now = DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss");
            if (!FileOperation.CreateFile(Server.MapPath("/exams/") + now + ".xml", exam_file) || !FileOperation.CreateFile(Server.MapPath("/answer/") + now + ".xml", result_file))
            {
                return Json(0);
            }

            Exam exam = new Exam
            {
                title = title,
                subject = subject,
                time = time,
                start_date = start,
                end_date = end,
                exam_path = Server.MapPath("/exams/") + now + ".xml",
                answer_path = Server.MapPath("/answer/") + now + ".xml",
                must_take = musttake,
                _public = ispublic
            };

            if (!ExamView.AddExam(exam, groups))
            {
                return Json(0);
            }

            examsCache = ExamView.GetAllExams();
            return Json(exam.eid);
        }

        public ActionResult Attend(int id = 0)
        {
            if (id == 0 || !Permission.LoginedNeed(Request, Response, Session))
            {
                return null;
            }

            Exam exam = ExamView.GetExamById(id);
            if (exam == null || DateTime.Now < exam.start_date || DateTime.Now > exam.end_date)
            {
                Permission.BackToPrevPageOrIndex(Request, Response);
                return null;
            }

            User user = (User)Session["user"];
            List<int> userGroups = GroupView.GetGroupMemberByUID(user.uid).Select(gm => (int)gm.gid).ToList();
            IQueryable<int> examGroups = ExamView.GetAllGroupsByExam(exam.eid).Select(e => e.gid);
            List<int> gids = userGroups.Intersect(examGroups).ToList();

            if ((exam._public == false && gids.Count() == 0) || ResultView.GetResult(id, user.uid) != null)
            {
                Permission.BackToPrevPageOrIndex(Request, Response);
                return null;
            }

            ViewBag.exam = exam;
            ViewBag.questions = FileOperation.ReadFile(exam.exam_path);
            
            return View();
        }

        [HttpPost]
        public JsonResult Submit()
        {
            if (!Permission.LoginedNeed(Request, Response, Session))
            {
                return Json(false);
            }

            int eid = Convert.ToInt32(Request["eid"]);
            User user = (User)Session["user"];
            int number = Convert.ToInt32(Request["number"]);
            string content = "<exam>";

            for (var i = 1; i <= number; i++)
            {
                string answer = Request["q" + i];
                content += "<question><number>" + i + "</number><answer>" + answer + "</answer></question>";
            }
            content += "</exam>";

            string filename = Server.MapPath("/result/") + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + ".xml";
            if (!FileOperation.CreateFile(filename, content))
            {
                return Json(false);
            }

            Result result = new Result
            {
                uid = user.uid,
                eid = eid,
                answer = filename,
                reviewer = 1,
                score = -1
            };

            return Json(ResultView.AddResult(result));
        }

        public ActionResult Result(int id = 0)
        {
            if (id == 0 || !Permission.PremissionNeed(Request, Response, Session, UserRank.TEACHER))
            {
                return null;
            }

            ViewBag.exam = ExamView.GetExamById(id);
            ViewBag.results = ResultView.GetResultByExam(id).ToList();

            return View();
        }

        public ActionResult ReviewList(int id = 0)
        {
            if (id == 0 || !Permission.PremissionNeed(Request, Response, Session, UserRank.TEACHER))
            {
                return null;
            }

            ViewBag.exam = ExamView.GetExamById(id);
            ViewBag.reviews = ResultView.GetReviewByExam(id).ToList();

            return View();
        }

        public ActionResult Review()
        {
            if (!Permission.PremissionNeed(Request, Response, Session, UserRank.TEACHER))
            {
                return null;
            }

            int eid = Convert.ToInt32(Request["eid"]);
            int uid = Convert.ToInt32(Request["uid"]);
            Result result = ResultView.GetResult(eid, uid);
            
            if (result == null)
            {
                Permission.BackToPrevPageOrIndex(Request, Response);
                return null;
            }

            Exam exam = ExamView.GetExamById((int) result.eid);

            ViewBag.result = result;
            ViewBag.exam = exam;
            ViewBag.questions = FileOperation.ReadFile(exam.exam_path);
            ViewBag.answers = FileOperation.ReadFile(exam.answer_path);
            ViewBag.results = FileOperation.ReadFile(result.answer);
            ViewBag.uid = uid;

            return View();
        }

        [HttpPost]
        public JsonResult SubmitReview()
        {
            if (!Permission.PremissionNeed(Request, Response, Session, UserRank.TEACHER))
            {
                return Json(false);
            }

            User user = (User)Session["user"];
            int eid = Convert.ToInt32(Request["eid"]);
            int uid = Convert.ToInt32(Request["uid"]);
            int score = Convert.ToInt32(Request["score"]);

            Result result = ResultView.GetResult(eid, uid);
            result.score = score;
            result.reviewer = user.uid;

            return Json(ResultView.SubmitScore(result));
        }

        [HttpPost]
        public JsonResult SubmitDifficulty()
        {
            if (!Permission.PremissionNeed(Request, Response, Session, UserRank.TEACHER))
            {
                return Json(false);
            }
            
            int qid = Convert.ToInt32(Request["qid"]);
            double d = Convert.ToDouble(Request["d"]);

            Question question = ExamView.GetQuestionById(qid);
            question.difficulty = (question.difficulty * question.number + d) / (question.number + 1);
            question.number++;

            return Json(ExamView.SubmitDifficulty(question));
        }
    }
}