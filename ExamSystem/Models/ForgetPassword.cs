namespace ExamSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ForgetPassword")]
    public partial class ForgetPassword
    {
        public int? uid { get; set; }

        [Key]
        public string code { get; set; }

        public virtual User User { get; set; }
    }
}
