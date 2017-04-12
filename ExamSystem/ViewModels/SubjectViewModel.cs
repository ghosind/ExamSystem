using ExamSystem.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamSystem.ViewModels
{
    public class SubjectView
    {
        private static ExamSystemContext context = new ExamSystemContext();
        private static ILog log = LogManager.GetLogger(typeof(SubjectView));

        public static Subject GetSubjectById(int id)
        {
            var subject = (from s in context.Subjects
                           where s.sid == id
                           select s).FirstOrDefault();

            return subject;
        }

        public static IOrderedQueryable<Subject> GetAllSubject()
        {
            var subjects = from subject in context.Subjects
                           orderby subject.sid
                           select subject;

            return subjects;
        }

        public static bool DeleteSubject(int id)
        {
            var subject = (from s in context.Subjects
                           where s.sid == id
                           select s).FirstOrDefault();
            context.Subjects.Remove(subject);

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


        public static bool SaveSubject(Subject subject)
        {
            var oldSubject = (from s in context.Subjects
                              where s.sid == subject.sid
                              select s).FirstOrDefault();
            oldSubject = subject;
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

        public static bool AddSubject(Subject subject)
        {
            context.Subjects.Add(subject);

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