namespace ExamSystem.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class FillQuestion
    {
        public int? qid { get; set; }

        [Key]
        [Column(Order = 0, TypeName = "text")]
        public string content { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "text")]
        public string answer { get; set; }

        public virtual Question Question { get; set; }
    }
}
