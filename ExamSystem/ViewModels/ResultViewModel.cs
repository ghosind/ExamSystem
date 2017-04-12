using ExamSystem.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamSystem.ViewModels
{
    public class ResultView
    {
        private static ExamSystemContext context = new ExamSystemContext();
        private static ILog log = LogManager.GetLogger(typeof(ResultView));

        public static IOrderedQueryable<Result> GetResultByUid(int uid)
        {
            var results = from result in context.Results
                          where result.uid == uid
                          orderby result.eid descending
                          select result;

            return results;
        }

        public static IOrderedQueryable<Result> GetResultByExam(int eid)
        {
            var results = from result in context.Results
                          where result.eid == eid
                          orderby result.uid descending
                          select result;

            return results;
        }

        public static Result GetResult(int eid, int uid)
        {
            var result = (from r in context.Results
                          where r.uid == uid && r.eid == eid
                          select r).FirstOrDefault();

            return result;
        }

        public static IOrderedQueryable<Result> GetResultByGroup(int gid)
        {
            var groups = from g in context.ExamsGroups
                         where g.gid == gid
                         select g.eid;
            var results = from result in context.Results
                          where groups.Contains(result.eid)
                          orderby result.uid descending
                          select result;
            return results;
        }

        public static bool AddResult(Result result)
        {
            context.Results.Add(result);

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

        public static IOrderedQueryable<Result> GetReviewByExam(int eid)
        {
            var results = from result in context.Results
                          where result.eid == eid && result.score == -1
                          orderby result.uid descending
                          select result;

            return results;
        }

        public static bool SubmitScore(Result result)
        {
            var oldResult = (from r in context.Results
                             where r.eid == result.eid && r.uid == result.uid
                             select r).FirstOrDefault();
            if (oldResult == null)
            {
                return false;
            }

            oldResult = result;
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