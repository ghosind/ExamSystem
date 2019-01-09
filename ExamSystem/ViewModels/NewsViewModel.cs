using System;
using System.Linq;
using ExamSystem.Models;
using log4net;

namespace ExamSystem.ViewModels
{
    public class NewsView
    {
        private static ExamSystemContext context = new ExamSystemContext();
        private static ILog log = LogManager.GetLogger(typeof(NewsView));

        public static IOrderedQueryable<News> GetAllNews()
        {
            var news = from n in context.News
                       orderby n.date descending
                       select n;
            
            return news;
        }

        public static int GetNewsNumber()
        {
            var number = context.News.Count();

            return number;
        }

        public static News GetNewsById(int nid)
        {
            var news = from n in context.News
                       where n.nid == nid
                       select n;
            return news.FirstOrDefault();
        }
        
        public static bool PostNewNews(News news)
        {
            context.News.Add(news);
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

        public static bool SaveNews(int nid, string title, string content)
        {
            var news = (from n in context.News
                       where n.nid == nid
                       select n).FirstOrDefault();

            try
            {
                news.title = title;
                news.content = content;
                context.SaveChanges();
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return false;
            }
            return true;
        }

        public static bool DeleteNews(int nid)
        {
            var news = (from n in context.News
                        where n.nid == nid
                        select n).FirstOrDefault();
            context.News.Remove(news);
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

        public static IOrderedQueryable<News> GetNewsByKeyword(string keyword)
        {
            var news = context.News.Where(n => n.title.Contains(keyword)).OrderByDescending(n => n.date);
            return news;
        }
    }
}