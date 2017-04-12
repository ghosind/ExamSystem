namespace ExamSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Message")]
    public partial class Message
    {
        [Key]
        public int mid { get; set; }

        public int? sender { get; set; }

        public int? receiver { get; set; }

        [Required]
        [StringLength(128)]
        public string title { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string content { get; set; }

        public DateTime send_date { get; set; }

        public bool read { get; set; }

        public virtual User User { get; set; }

        public virtual User User1 { get; set; }
    }
}
