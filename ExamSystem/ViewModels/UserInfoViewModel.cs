using ExamSystem.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamSystem.ViewModels
{
    public class UserInfoView
    {
        private static ExamSystemContext context = new ExamSystemContext();
        private static ILog log = LogManager.GetLogger(typeof(UserInfoView));
        
        public static bool EmailValidate(string email)
        {
            var existNumber = (from user in context.UserInfoes
                               where user.email == email
                               select user).Count();
            return existNumber == 0;
        }

        public static bool NewUserInfo(string username, string email, DateTime time)
        {
            var uid = (from u in context.Users
                       where u.username == username
                       select u).FirstOrDefault().uid;
            UserInfo info = new UserInfo
            {
                uid = uid,
                sex = "男",
                birthday = DateTime.Parse("1970-01-01"),
                email = email,
                email_valid = false,
                reg_date = time
            };

            context.UserInfoes.Add(info);
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

        public static UserInfo GetUserInfo(int id)
        {
            var userInfo = (from uf in context.UserInfoes
                            where uf.uid == id
                            select uf).FirstOrDefault();
            return userInfo;
        }

        public static bool SaveUserInfo(UserInfo userInfo)
        {
            var oldUserInfo = (from uf in context.UserInfoes
                               where uf.uid == userInfo.uid
                               select uf).FirstOrDefault();
            
            oldUserInfo = userInfo;

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

        public static bool DeleteUserInfo(UserInfo userInfo)
        {
            context.UserInfoes.Remove(userInfo);
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