using System;
using System.Linq;
using ExamSystem.Models;
using log4net;

namespace ExamSystem.ViewModels
{
    public class ExamView
    {
        private static ExamSystemContext context = new ExamSystemContext();
        private static ILog log = LogManager.GetLogger(typeof(ExamView));

        public static IOrderedQueryable<Exam> GetAllExams()
        {
            var exams = from exam in context.Exams
                       orderby exam.eid descending
                       select exam;

            return exams;
        }

        public static IOrderedQueryable<Exam> GetExamsByKeyword(string keyword)
        {
            var exams = context.Exams.Where(exam => exam.title.Contains(keyword)).OrderBy(exam => exam.eid);
            return exams;
        }

        public static Exam GetExamById(int id)
        {
            var exam = (from e in context.Exams
                        where e.eid == id
                        select e).FirstOrDefault();
            return exam;
        }

        public static IOrderedQueryable<ExamsGroup> GetAllGroupsByExam(int id)
        {
            var groups = from g in context.ExamsGroups
                         where g.eid == id
                         orderby g.gid descending
                         select g;
            return groups;
        }

        public static IOrderedQueryable<Exam> GetAllExamByGroup(int id)
        {
            var exams = from exam in context.Exams
                        where (from e in context.ExamsGroups
                               where e.gid == id || e.gid == 0
                               select e.eid).Contains(exam.eid)
                        orderby exam.eid
                        select exam;
            return exams;
        }

        public static IOrderedQueryable<Exam> GetAllExamByUser(int uid)
        {
            var exams = from exam in context.Exams
                        where (from e in context.ExamsGroups
                               where (from g in context.GroupMembers
                                      where g.uid == uid
                                      select g.gid).Contains(e.gid) || e.gid == 0
                               select e.eid).Contains(exam.eid)
                        orderby exam.eid
                        select exam;
            return exams;
        }

        public static IOrderedQueryable<Question> GetAllQuestion()
        {
            var questions = from question in context.Questions
                            orderby question.qid
                            select question;
            return questions;
        }

        public static ChoiceQuestion GetChoiceQuestionByQid(int qid)
        {
            var question = from q in context.ChoiceQuestions
                           where q.qid == qid
                           select q;
            return question.FirstOrDefault();
        }

        public static FillQuestion GetFillQuestionByQid(int qid)
        {
            var question = from q in context.FillQuestions
                           where q.qid == qid
                           select q;
            return question.FirstOrDefault();
        }

        public static DiscussQuestion GetDiscussQuestionByQid(int qid)
        {
            var question = from q in context.DiscussQuestions
                           where q.qid == qid
                           select q;
            return question.FirstOrDefault();
        }

        public static Question GetQuestionById(int id)
        {
            var question = from q in context.Questions
                           where q.qid == id
                           select q;
            return question.FirstOrDefault();
        }

        public static bool AddChoiceQuestion(Question question, ChoiceQuestion detail)
        {
            context.Questions.Add(question);

            try
            {
                context.SaveChanges();
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return false;
            }

            detail.qid = question.qid;
            context.ChoiceQuestions.Add(detail);

            try
            {
                context.SaveChanges();
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return false;
            }
            return true;
        }

        public static bool AddFillinQuestion(Question question, FillQuestion detail)
        {
            context.Questions.Add(question);

            try
            {
                context.SaveChanges();
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return false;
            }

            detail.qid = question.qid;
            context.FillQuestions.Add(detail);

            try
            {
                context.SaveChanges();
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return false;
            }

            return true;
        }

        public static bool AddDiscussQuestion(Question question, DiscussQuestion detail)
        {
            context.Questions.Add(question);

            try
            {
                context.SaveChanges();
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return false;
            }

            detail.qid = question.qid;
            context.DiscussQuestions.Add(detail);

            try
            {
                context.SaveChanges();
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return false;
            }
            return true;
        }

        public static bool DeleteQuestion(Question question)
        {
            switch (question.type)
            {
                case (int)QuestionType.SINGLECHOICE:
                case (int)QuestionType.MULTICHOICE:
                    ChoiceQuestion cdetail = GetChoiceQuestionByQid(question.qid);
                    context.ChoiceQuestions.Remove(cdetail);
                    break;
                case (int)QuestionType.FILLIN:
                    FillQuestion fdetail = GetFillQuestionByQid(question.qid);
                    context.FillQuestions.Remove(fdetail);
                    break;
                case (int)QuestionType.DISCUSS:
                    DiscussQuestion ddetail = GetDiscussQuestionByQid(question.qid);
                    context.DiscussQuestions.Remove(ddetail);
                    break;
                default:
                    return false;
            }

            context.Questions.Remove(question);

            try
            {
                context.SaveChanges();
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return false;
            }
            return true;
        }

        public static IOrderedQueryable GetChoiceQuestionBySection(int section, int type)
        {
            var questions = (from question in context.ChoiceQuestions
                            where (from q in context.Questions
                                   where q.type == type && q.kid == section
                                   select q.qid).Contains((int) question.qid)
                            select new
                            {
                                qid = question.qid,
                                content = question.content,
                                number = question.choice_num,
                                a = question.choice_1,
                                b = question.choice_2,
                                c = question.choice_3,
                                d = question.choice_4,
                                answer = question.answer
                            }).OrderBy(q => q.qid);
            return questions;
        }

        public static IOrderedQueryable GetFillinQuestionBySection(int section)
        {
            var questions = (from question in context.FillQuestions
                             where (from q in context.Questions
                                    where q.kid == section
                                    select q.qid).Contains((int)question.qid)
                             select new
                             {
                                 qid = question.qid,
                                 content = question.content,
                                 answer = question.answer
                             }).OrderBy(q => q.qid);
            return questions;
        }

        public static IOrderedQueryable GetDiscussQuestionBySection(int section)
        {
            var questions = (from question in context.DiscussQuestions
                             where (from q in context.Questions
                                    where q.kid == section
                                    select q.qid).Contains((int)question.qid)
                             select new
                             {
                                 qid = question.qid,
                                 content = question.content,
                                 answer = question.answer
                             }).OrderBy(q => q.qid);
            return questions;
        }

        public static bool AddExam(Exam exam, string[] gids)
        {
            context.Exams.Add(exam);

            try
            {
                context.SaveChanges();
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return false;
            }

            foreach (var gid in gids)
            {
                if (gid != "")
                {
                    ExamsGroup eg = new ExamsGroup
                    {
                        eid = exam.eid,
                        gid = Convert.ToInt32(gid)
                    };
                    context.ExamsGroups.Add(eg);
                }
            }

            try
            {
                context.SaveChanges();
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return false;
            }

            return true;
        }

        public static bool SubmitDifficulty(Question question)
        {
            var oldQuestion = (from q in context.Questions
                               where q.qid == question.qid
                               select q).FirstOrDefault();
            if (oldQuestion == null)
            {
                return false;
            }

            oldQuestion = question;
            try
            {
                context.SaveChanges();
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return false;
            }
            return true;
        }
    }
}