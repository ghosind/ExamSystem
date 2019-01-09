namespace ExamSystem.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("UserInfo")]
    public partial class UserInfo
    {
        public int? uid { get; set; }

        [Key]
        [Column(Order = 0)]
        [StringLength(2)]
        public string sex { get; set; }

        [Key]
        [Column(Order = 1)]
        public DateTime birthday { get; set; }

        [StringLength(11)]
        public string telephone { get; set; }

        [Key]
        [Column(Order = 2)]
        public string email { get; set; }

        [Key]
        [Column(Order = 3)]
        public bool email_valid { get; set; }

        [StringLength(128)]
        public string address { get; set; }

        [StringLength(256)]
        public string description { get; set; }

        [Key]
        [Column(Order = 4)]
        public DateTime reg_date { get; set; }

        public virtual User User { get; set; }
    }
}
