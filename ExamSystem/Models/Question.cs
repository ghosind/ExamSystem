namespace ExamSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Question
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Question()
        {
            ChoiceQuestions = new HashSet<ChoiceQuestion>();
            DiscussQuestions = new HashSet<DiscussQuestion>();
            FillQuestions = new HashSet<FillQuestion>();
        }

        [Key]
        public int qid { get; set; }

        public int type { get; set; }

        public int? kid { get; set; }

        public double suggest_difficulty { get; set; }

        public double difficulty { get; set; }

        public int number { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChoiceQuestion> ChoiceQuestions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DiscussQuestion> DiscussQuestions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FillQuestion> FillQuestions { get; set; }

        public virtual Section Section { get; set; }
    }

    public enum QuestionType
    {
        SINGLECHOICE = 0,
        MULTICHOICE = 1,
        FILLIN = 2,
        DISCUSS = 3
    }
}
