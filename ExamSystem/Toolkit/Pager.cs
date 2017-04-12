using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamSystem.Toolkit
{
    public class Pager<T>
    {
        private static int numberPerPage = 10;
        private ILog log = LogManager.GetLogger(typeof(Pager<T>));
        
        public List<T> GetPage(IOrderedQueryable<T> elements, int page)
        {
            List<T> list = elements.Take(page * numberPerPage).ToList();

            try
            {
                list.RemoveRange(0, (page - 1) * numberPerPage);
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return null;
            }

            return list;
        }

        public int GetPageNumber(IOrderedQueryable<T> elements)
        {
            int number = elements.Count();

            return (number % numberPerPage == 0 ? number / numberPerPage : number / numberPerPage + 1);
        }
    }
}