namespace ExamSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Exam
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Exam()
        {
            ExamsGroups = new HashSet<ExamsGroup>();
            Results = new HashSet<Result>();
        }

        [Key]
        public int eid { get; set; }

        [Required]
        [StringLength(128)]
        public string title { get; set; }

        public int? subject { get; set; }

        public int time { get; set; }

        public DateTime start_date { get; set; }

        public DateTime end_date { get; set; }

        [Required]
        [StringLength(128)]
        public string exam_path { get; set; }

        [Required]
        [StringLength(128)]
        public string answer_path { get; set; }

        public bool must_take { get; set; }

        [Column("public")]
        public bool _public { get; set; }

        public virtual Subject Subject1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExamsGroup> ExamsGroups { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Result> Results { get; set; }
    }
}
