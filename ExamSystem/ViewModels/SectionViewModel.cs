using System;
using System.Linq;
using ExamSystem.Models;
using log4net;

namespace ExamSystem.ViewModels
{
    public class SectionView
    {
        private static ExamSystemContext context = new ExamSystemContext();
        private static ILog log = LogManager.GetLogger(typeof(SectionView));

        public static Section GetSectionById(int id)
        {
            var section = (from s in context.Sections
                           where s.kid == id
                           select s).FirstOrDefault();

            return section;
        }

        public static IOrderedQueryable<Section> GetAllSection()
        {
            var sections = from section in context.Sections
                           orderby section.kid
                           select section;

            return sections;
        }

        public static bool DeleteSection(int id)
        {
            var section = (from s in context.Sections
                           where s.kid == id
                           select s).FirstOrDefault();
            context.Sections.Remove(section);

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


        public static bool SaveSection(Section section)
        {
            var oldsection = (from s in context.Sections
                              where s.kid == section.kid
                              select s).FirstOrDefault();
            oldsection = section;
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

        public static bool AddSection(Section section)
        {
            context.Sections.Add(section);

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

        public static IOrderedQueryable GetSectionsBySubject(int sid)
        {
            var sections = (from section in context.Sections
                            where section.sid == sid
                            select new {
                                id = section.kid,
                                name = section.section_name
                            }).OrderBy(section => section.id);
            return sections;
        }
    }
}