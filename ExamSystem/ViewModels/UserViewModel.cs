using System;
using System.Linq;
using ExamSystem.Models;
using log4net;

namespace ExamSystem.ViewModels
{
    public class UserView
    {
        private static ExamSystemContext context = new ExamSystemContext();
        private static ILog log = LogManager.GetLogger(typeof(UserView));

        public static bool UserNameValidate(string username)
        {
            var existNumer = (from user in context.Users
                            where user.username == username
                            select user).Count();
            return existNumer == 0;
        }

        public static User Login(string username, string password)
        {
            var user = from u in context.Users
                       where u.username == username && u.password == password
                       select u;
            return user.FirstOrDefault();
        }

        public static void LoginSuccess(int uid, DateTime time)
        {
            var user = from u in context.Users
                       where u.uid == uid
                       select u;
            user.First().login_date = time;
            context.SaveChangesAsync();
        }

        public static User GetUserById(int uid)
        {
            var user = from u in context.Users
                       where u.uid == uid
                       select u;
            return user.FirstOrDefault();
        }

        public static User GetUserByUsername(string username)
        {
            var user = from u in context.Users
                       where u.username == username
                       select u;
            return user.FirstOrDefault();
        }

        public static bool Register(string username, string password, string name, DateTime time)
        {
            User user = new User
            {
                username = username,
                password = password,
                name = name,
                rank = 1,
                login_date = time
            };

            context.Users.Add(user);

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

        public static bool SaveUser(User user)
        {
            var oldUser = (from u in context.Users
                           where u.uid == user.uid
                           select u).FirstOrDefault();
            
            oldUser = user;
            
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

        public static IOrderedQueryable<User> GetUserByKeyword(string keyword)
        {
            var users = context.Users.Where(u => u.name.Contains(keyword)).OrderBy(u => u.uid);
            return users;
        }

        public static bool ForgetPassword(ForgetPassword fp)
        {
            var old = (from ofp in context.ForgetPasswords
                       where ofp.uid == fp.uid
                       select ofp).FirstOrDefault();
            if (old != null)
            {
                old = fp;
            }
            else
            {
                context.ForgetPasswords.Add(fp);
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

        public static int GetUIDByCode(string code)
        {
            var fp = (from f in context.ForgetPasswords
                      where f.code == code
                      select f).FirstOrDefault();
            if (fp != null)
            {
                return (int) fp.uid;
            }
            else
            {
                return 0;
            }
        }

        public static bool DeleteForgetPassword(string code)
        {
            var fp = (from f in context.ForgetPasswords
                      where f.code == code
                      select f).FirstOrDefault();

            context.ForgetPasswords.Remove(fp);

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