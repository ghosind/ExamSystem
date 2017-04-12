using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ExamSystem.Toolkit
{
    public class Hash
    {
        public static string SHA512(string content)
        {
            string salt = "es_password";
            byte[] bytes = Encoding.UTF8.GetBytes(content + salt);
            string hash = "";
            using (SHA512 shaM = new SHA512Managed())
            {
                var hashByte = shaM.ComputeHash(bytes);
                foreach (byte x in hashByte)
                {
                    hash += String.Format("{0:x2}", x);
                }
            }

            return hash;
        }

        public static string Md5(string content)
        {
            string salt = "es_password";
            byte[] bytes = Encoding.UTF8.GetBytes(content + salt);
            string hash = "";
            using (MD5 md5Hash = MD5.Create())
            {
                var hashByte = md5Hash.ComputeHash(bytes);
                foreach (byte x in hashByte)
                {
                    hash += String.Format("{0:x2}", x);
                }
            }
                
            return hash;
        }
    }
}