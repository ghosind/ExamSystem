using ExamSystem.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamSystem.ViewModels
{
    public class MessageView
    {
        private static ExamSystemContext context = new ExamSystemContext();
        private static ILog log = LogManager.GetLogger(typeof(NewsView));

        public static int GetNewMessageNumber(int id = 1)
        {
            var number = (from message in context.Messages
                          where message.receiver == id && message.read == false
                          select message).Count();
            return number;
        }

        public static IOrderedQueryable<Message> GetRecivedMessage(int id = 1)
        {
            var messages = from message in context.Messages
                          where message.receiver == id
                          orderby message.send_date descending
                          select message;
            return messages;
        }

        public static IOrderedQueryable<Message> GetSendMessage(int id = 1)
        {
            var messages = from message in context.Messages
                           where message.sender == id
                           orderby message.send_date descending
                           select message;
            return messages;
        }

        public static Message GetMessage(int id = 1)
        {
            var message = from m in context.Messages
                          where m.mid == id
                          select m;
            return message.FirstOrDefault();
        }

        public static void ReadMessage(int id = 1)
        {
            var message = (from m in context.Messages
                           where m.mid == id
                           select m).FirstOrDefault();
            message.read = true;
            try
            {
                context.SaveChanges();
            }
            catch (Exception e)
            {
                log.Error(e.Message);
            }
        }

        public static bool SendMessage(Message message)
        {
            context.Messages.Add(message);

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