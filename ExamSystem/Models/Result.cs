namespace ExamSystem.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Result")]
    public partial class Result
    {
        public int? uid { get; set; }

        [Key]
        [Column(Order = 1)]
        public int? eid { get; set; }

        [Key]
        [Column(Order = 0)]
        public string answer { get; set; }

        public int? reviewer { get; set; }

        public int score { get; set; }

        public virtual Exam Exam { get; set; }

        public virtual User User { get; set; }
        
        public virtual User User1 { get; set; }
    }
}
