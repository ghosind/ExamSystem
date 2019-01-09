namespace ExamSystem.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class News
    {
        [Key]
        public int nid { get; set; }

        public int? publisher { get; set; }

        [Required]
        [StringLength(256)]
        public string title { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string content { get; set; }

        public DateTime date { get; set; }

        public virtual User User { get; set; }
    }
}
