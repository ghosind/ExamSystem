﻿using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ExamSystem.Toolkit
{
    public class FileOperation
    {
        private static ILog log = LogManager.GetLogger(typeof(FileOperation));

        public static bool CreateFile(string filename, string content)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(filename))
                {
                    sw.Write(content);
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return false;
            }

            return true;
        }

        public static string ReadFile(string filename)
        {
            return File.ReadAllText(filename);
        }
    }
}