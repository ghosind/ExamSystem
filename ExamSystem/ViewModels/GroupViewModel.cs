using System;
using System.Linq;
using ExamSystem.Models;
using log4net;

namespace ExamSystem.ViewModels
{
    public class GroupView
    {
        private static ExamSystemContext context = new ExamSystemContext();
        private static ILog log = LogManager.GetLogger(typeof(GroupView));

        public static IOrderedQueryable<Group> GetAllGroups()
        {
            var groups = from g in context.Groups
                         orderby g.gid descending
                         select g;

            return groups;
        }

        public static bool NewGroup(Group group)
        {
            context.Groups.Add(group);

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

        public static bool AddMember(int gid, GroupMember member)
        {
            var group = (from g in context.Groups
                         where g.gid == gid
                         select g).FirstOrDefault();
            if (group == null)
            {
                return false;
            }

            group.number++;
            context.GroupMembers.Add(member);

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

        public static Group GetGroupById(int gid)
        {
            var group = (from g in context.Groups
                         where g.gid == gid
                         select g).FirstOrDefault();
            return group;
        }

        public static GroupMember GetGroupMember(int uid, int gid)
        {
            var member = (from gm in context.GroupMembers
                          where gm.uid == uid && gm.gid == gid
                          select gm).FirstOrDefault();
            return member;
        }

        public static IOrderedQueryable<GroupMember> GetGroupMemberByUID(int uid)
        {
            var member = from gm in context.GroupMembers
                         where gm.uid == uid
                         orderby gm.gid
                         select gm;

            return member;
        }

        public static IOrderedQueryable<GroupMember> GetGroupMemberByGID(int gid)
        {
            var member = from gm in context.GroupMembers
                         where gm.gid == gid
                         orderby gm.uid
                         select gm;
            return member;
        }

        public static bool QuitGroup(GroupMember gm)
        {
            var group = (from g in context.Groups
                         where g.gid == gm.gid
                         select g).FirstOrDefault();
            if (group == null || group.allow_quit == false)
            {
                return false;
            }

            group.number--;
            context.GroupMembers.Remove(gm);

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

        public static bool DismissGroup(int gid)
        {
            var group = (from g in context.Groups
                         where g.gid == gid
                         select g).FirstOrDefault();
            context.Groups.Remove(group);

            var members = from gm in context.GroupMembers
                          where gm.gid == gid
                          select gm;
            context.GroupMembers.RemoveRange(members);

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

        public static bool SaveGroupMember(GroupMember member)
        {
            var oldMember = (from gm in context.GroupMembers
                             where gm.gid == member.gid && gm.uid == member.uid
                             select gm).FirstOrDefault();

            if (oldMember == null)
            {
                return false;
            }

            oldMember = member;

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

        public static IOrderedQueryable<Group> GetGroupByKeywork(string Keyword)
        {
            var groups = (from g in context.Groups
                          select g).Where(g => g.group_name.Contains(Keyword)).OrderBy(g => g.gid);
            return groups;
        }
    }
}