namespace ExamSystem.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ExamsGroup")]
    public partial class ExamsGroup
    {
        public int? eid { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int gid { get; set; }

        public virtual Exam Exam { get; set; }
    }
}
