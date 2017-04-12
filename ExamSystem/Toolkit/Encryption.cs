using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamSystem.Toolkit
{
    public class Encryption
    {
        public static string PasswordCookieEncryption(string ip, string password, DateTime time)
        {
            return Hash.Md5(ip + password + time);
        }
    }
}