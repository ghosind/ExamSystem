using ExamSystem.Models;
using ExamSystem.Toolkit;
using ExamSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExamSystem.Controllers
{
    public class ManageController : Controller
    {
        // GET: Manage
        public ActionResult Index(int id = 1)
        {
            Pager<User> pager = new Pager<User>();

            if (!Permission.PremissionNeed(Request, Response, Session, UserRank.ADMINISTATOR))
            {
                return null;
            }

            IOrderedQueryable<User> users = UserView.GetUserByKeyword("");
            ViewBag.page = id;
            ViewBag.users = pager.GetPage(users, id);
            ViewBag.pageNumber = pager.GetPageNumber(users);

            return View();
        }

        public ActionResult Subject(int id = 1)
        {
            Pager<Subject> pager = new Pager<Subject>();

            if (!Permission.PremissionNeed(Request, Response, Session, UserRank.ADMINISTATOR))
            {
                return null;
            }

            IOrderedQueryable<Subject> subjects = SubjectView.GetAllSubject();
            ViewBag.page = id;
            ViewBag.subjects = pager.GetPage(subjects, id);
            ViewBag.pageNumber = pager.GetPageNumber(subjects);

            return View();
        }

        [HttpPost]
        public JsonResult DeleteSubject()
        {
            if (!Permission.PremissionNeed(Request, Response, Session, UserRank.ADMINISTATOR))
            {
                return Json(false);
            }

            int id = Convert.ToInt32(Request["sid"]);
            return Json(SubjectView.DeleteSubject(id));
        }

        [HttpPost]
        public JsonResult SaveSubject()
        {
            if (!Permission.PremissionNeed(Request, Response, Session, UserRank.ADMINISTATOR))
            {
                return Json(false);
            }

            int id = Convert.ToInt32(Request["sid"]);
            string name = Request["name"];
            Subject subject = SubjectView.GetSubjectById(id);
            subject.subject_name = name;

            return Json(SubjectView.SaveSubject(subject));
        }

        public ActionResult AddSubject()
        {
            if (!Permission.PremissionNeed(Request, Response, Session, UserRank.ADMINISTATOR))
            {
                return null;
            }

            return View();
        }

        [HttpPost]
        public JsonResult NewSubject()
        {
            if (!Permission.PremissionNeed(Request, Response, Session, UserRank.ADMINISTATOR))
            {
                return Json(false);
            }
            
            string name = Request["name"];
            Subject subject = new Subject
            {
                subject_name = name
            };

            return Json(SubjectView.AddSubject(subject));
        }

        public ActionResult Section(int id = 1)
        {
            Pager<Section> pager = new Pager<Section>();

            if (!Permission.PremissionNeed(Request, Response, Session, UserRank.ADMINISTATOR))
            {
                return null;
            }

            IOrderedQueryable<Section> sections = SectionView.GetAllSection();
            ViewBag.page = id;
            ViewBag.sections = pager.GetPage(sections, id);
            ViewBag.pageNumber = pager.GetPageNumber(sections);
            ViewBag.subjects = SubjectView.GetAllSubject().ToList();

            return View();
        }

        [HttpPost]
        public JsonResult DeleteSection()
        {
            if (!Permission.PremissionNeed(Request, Response, Session, UserRank.ADMINISTATOR))
            {
                return Json(false);
            }

            int id = Convert.ToInt32(Request["kid"]);
            return Json(SectionView.DeleteSection(id));
        }

        [HttpPost]
        public JsonResult SaveSection()
        {
            if (!Permission.PremissionNeed(Request, Response, Session, UserRank.ADMINISTATOR))
            {
                return Json(false);
            }

            int kid = Convert.ToInt32(Request["kid"]);
            string name = Request["name"];
            int sid = Convert.ToInt32(Request["sid"]);
            Section section = SectionView.GetSectionById(kid);
            section.section_name = name;
            section.kid = kid;

            return Json(SectionView.SaveSection(section));
        }

        public ActionResult AddSection()
        {
            if (!Permission.PremissionNeed(Request, Response, Session, UserRank.ADMINISTATOR))
            {
                return null;
            }

            List<Subject> subjects = SubjectView.GetAllSubject().ToList();
            if (subjects.Count == 0)
            {
                Permission.BackToPrevPageOrIndex(Request, Response);
                return null;
            }
            ViewBag.subjects = subjects;

            return View();
        }

        [HttpPost]
        public JsonResult NewSection()
        {
            if (!Permission.PremissionNeed(Request, Response, Session, UserRank.ADMINISTATOR))
            {
                return Json(false);
            }

            string name = Request["name"];
            int sid = Convert.ToInt32(Request["sid"]);
            Section section = new Section
            {
                section_name = name,
                sid = sid
            };

            return Json(SectionView.AddSection(section));
        }

        public ActionResult Questions(int id = 1)
        {
            Pager<Question> pager = new Pager<Question>();

            if (!Permission.PremissionNeed(Request, Response, Session, UserRank.ADMINISTATOR))
            {
                return null;
            }

            IOrderedQueryable<Question> questions = ExamView.GetAllQuestion();
            ViewBag.page = id;
            ViewBag.questions = pager.GetPage(questions, id);
            ViewBag.pageNumber = pager.GetPageNumber(questions);
            ViewBag.subjects = SubjectView.GetAllSubject().ToList();

            return View();
        }

        [HttpPost]
        public JsonResult DeleteQuestion()
        {
            if (!Permission.PremissionNeed(Request, Response, Session, UserRank.ADMINISTATOR))
            {
                return Json(false);
            }

            int qid = Convert.ToInt32(Request["qid"]);
            Question question = ExamView.GetQuestionById(qid);

            return Json(ExamView.DeleteQuestion(question));
        }

        public ActionResult AddQuestion()
        {
            if (!Permission.PremissionNeed(Request, Response, Session, UserRank.ADMINISTATOR))
            {
                return null;
            }

            List<Subject> subjects = SubjectView.GetAllSubject().ToList();
            if (subjects.Count == 0)
            {
                Permission.BackToPrevPageOrIndex(Request, Response);
                return null;
            }

            ViewBag.subjects = subjects;

            return View();
        }

        [HttpPost]
        public JsonResult GetSectionsBySubject()
        {
            if (!Permission.PremissionNeed(Request, Response, Session, UserRank.TEACHER))
            {
                return Json(null);
            }

            int sid = Convert.ToInt32(Request["subject"]);

            return Json(SectionView.GetSectionsBySubject(sid));
        }

        [HttpPost]
        public JsonResult SubmitQuestion()
        {
            if (!Permission.PremissionNeed(Request, Response, Session, UserRank.ADMINISTATOR))
            {
                return Json(false);
            }

            User user = (User)Session["user"];
            string text = Request["text"];
            int section = Convert.ToInt32(Request["section"]);
            int type = Convert.ToInt32(Request["type"]);
            double difficulty = Convert.ToDouble(Request["difficulty"]);
            int number = Convert.ToInt32(Request["number"]);
            string choicea = Request["choicea"];
            string choiceb = Request["choiceb"];
            string choicec = Request["choicec"];
            string choiced = Request["choiced"];
            string answer = Request["answer"];
            bool result = false;

            Question question = new Question
            {
                type = type,
                kid = section,
                suggest_difficulty = difficulty,
                difficulty = 0,
                number = 0
            };

            switch (type)
            {
                case (int)QuestionType.SINGLECHOICE:
                    ChoiceQuestion sc = new ChoiceQuestion
                    {
                        content = text,
                        choice_num = number,
                        choice_1 = choicea,
                        choice_2 = choiceb,
                        choice_3 = choicec,
                        choice_4 = choiced,
                        answer = Convert.ToInt32(answer)
                    };
                    result = ExamView.AddChoiceQuestion(question, sc);
                    break;
                case (int)QuestionType.MULTICHOICE:
                    ChoiceQuestion mc = new ChoiceQuestion
                    {
                        content = text,
                        choice_num = number,
                        choice_1 = choicea,
                        choice_2 = choiceb,
                        choice_3 = choicec,
                        choice_4 = choiced,
                        answer = Convert.ToInt32(answer.Replace(",", ""))
                    };
                    result = ExamView.AddChoiceQuestion(question, mc);
                    break;
                case (int)QuestionType.FILLIN:
                    FillQuestion fq = new FillQuestion
                    {
                        content = text,
                        answer = answer
                    };
                    result = ExamView.AddFillinQuestion(question, fq);
                    break;
                case (int)QuestionType.DISCUSS:
                    DiscussQuestion dq = new DiscussQuestion
                    {
                        content = text,
                        answer = answer
                    };
                    result = ExamView.AddDiscussQuestion(question, dq);
                    break;
                default:
                    break;
            }

            return Json(result);
        }
    }
}