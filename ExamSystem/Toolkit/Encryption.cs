using System;

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