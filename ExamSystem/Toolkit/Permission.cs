using ExamSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamSystem.Toolkit
{
    public class Permission
    {
        public static void BackToPrevPageOrIndex(HttpRequestBase request, HttpResponseBase response)
        {
            Uri uri = request.UrlReferrer;
            string url = "/Index";
            if (uri != null)
            {
                url = uri.ToString();
            }
            response.Redirect(url);
        }

        public static bool PremissionNeed(HttpRequestBase request,HttpResponseBase response, HttpSessionStateBase session, UserRank rank)
        {
            User user = (User) session["user"];
            if (user == null || user.rank < (int) rank)
            {
                BackToPrevPageOrIndex(request, response);
                return false;
            }
            return true;
        }

        public static bool NotLoginNeed(HttpRequestBase request, HttpResponseBase response, HttpSessionStateBase session)
        {
            User user = (User) session["user"];
            if (user != null)
            {
                BackToPrevPageOrIndex(request, response);
                return false;
            }
            return true;
        }

        public static bool LoginedNeed(HttpRequestBase request, HttpResponseBase response, HttpSessionStateBase session)
        {
            User user = (User) session["user"];
            if (user == null)
            {
                BackToPrevPageOrIndex(request, response);
                return false;
            }
            return true;
        }
    }
}