namespace ExamSystem.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class ChoiceQuestion
    {
        public int? qid { get; set; }

        [Column(TypeName = "text")]
        public string content { get; set; }

        [Key]
        [Column(Order = 0)]
        public int choice_num { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "text")]
        public string choice_1 { get; set; }

        [Key]
        [Column(Order = 2, TypeName = "text")]
        public string choice_2 { get; set; }

        [Column(TypeName = "text")]
        public string choice_3 { get; set; }

        [Column(TypeName = "text")]
        public string choice_4 { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int answer { get; set; }

        public virtual Question Question { get; set; }
    }
}
