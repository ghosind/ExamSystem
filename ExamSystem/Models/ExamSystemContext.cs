namespace ExamSystem.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ExamSystemContext : DbContext
    {
        public ExamSystemContext()
            : base("name=ExamSystemContext")
        {
        }

        public virtual DbSet<Exam> Exams { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<Section> Sections { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<ChoiceQuestion> ChoiceQuestions { get; set; }
        public virtual DbSet<DiscussQuestion> DiscussQuestions { get; set; }
        public virtual DbSet<ExamsGroup> ExamsGroups { get; set; }
        public virtual DbSet<FillQuestion> FillQuestions { get; set; }
        public virtual DbSet<ForgetPassword> ForgetPasswords { get; set; }
        public virtual DbSet<GroupMember> GroupMembers { get; set; }
        public virtual DbSet<Result> Results { get; set; }
        public virtual DbSet<UserInfo> UserInfoes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Exam>()
                .Property(e => e.title)
                .IsUnicode(false);

            modelBuilder.Entity<Exam>()
                .Property(e => e.exam_path)
                .IsUnicode(false);

            modelBuilder.Entity<Exam>()
                .Property(e => e.answer_path)
                .IsUnicode(false);

            modelBuilder.Entity<Group>()
                .Property(e => e.group_name)
                .IsUnicode(false);

            modelBuilder.Entity<Message>()
                .Property(e => e.title)
                .IsUnicode(false);

            modelBuilder.Entity<Message>()
                .Property(e => e.content)
                .IsUnicode(false);

            modelBuilder.Entity<News>()
                .Property(e => e.title)
                .IsUnicode(false);

            modelBuilder.Entity<News>()
                .Property(e => e.content)
                .IsUnicode(false);

            modelBuilder.Entity<Section>()
                .Property(e => e.section_name)
                .IsUnicode(false);

            modelBuilder.Entity<Subject>()
                .Property(e => e.subject_name)
                .IsUnicode(false);

            modelBuilder.Entity<Subject>()
                .HasMany(e => e.Exams)
                .WithOptional(e => e.Subject1)
                .HasForeignKey(e => e.subject);

            modelBuilder.Entity<User>()
                .Property(e => e.username)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.password)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Groups)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.owner_uid);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Messages)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.receiver);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Messages1)
                .WithOptional(e => e.User1)
                .HasForeignKey(e => e.sender);

            modelBuilder.Entity<User>()
                .HasMany(e => e.News)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.publisher);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Results)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.reviewer);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Results1)
                .WithOptional(e => e.User1)
                .HasForeignKey(e => e.uid);

            modelBuilder.Entity<ChoiceQuestion>()
                .Property(e => e.content)
                .IsUnicode(false);

            modelBuilder.Entity<ChoiceQuestion>()
                .Property(e => e.choice_1)
                .IsUnicode(false);

            modelBuilder.Entity<ChoiceQuestion>()
                .Property(e => e.choice_2)
                .IsUnicode(false);

            modelBuilder.Entity<ChoiceQuestion>()
                .Property(e => e.choice_3)
                .IsUnicode(false);

            modelBuilder.Entity<ChoiceQuestion>()
                .Property(e => e.choice_4)
                .IsUnicode(false);

            modelBuilder.Entity<DiscussQuestion>()
                .Property(e => e.content)
                .IsUnicode(false);

            modelBuilder.Entity<DiscussQuestion>()
                .Property(e => e.answer)
                .IsUnicode(false);

            modelBuilder.Entity<FillQuestion>()
                .Property(e => e.content)
                .IsUnicode(false);

            modelBuilder.Entity<ForgetPassword>()
                .Property(e => e.code)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Result>()
                .Property(e => e.answer)
                .IsUnicode(false);

            modelBuilder.Entity<UserInfo>()
                .Property(e => e.sex)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<UserInfo>()
                .Property(e => e.telephone)
                .IsUnicode(false);

            modelBuilder.Entity<UserInfo>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<UserInfo>()
                .Property(e => e.address)
                .IsUnicode(false);

            modelBuilder.Entity<UserInfo>()
                .Property(e => e.description)
                .IsUnicode(false);
        }
    }
}
