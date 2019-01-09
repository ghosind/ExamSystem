namespace ExamSystem.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("GroupMember")]
    public partial class GroupMember
    {
        [Key]
        [Column(Order = 1)]
        public int? gid { get; set; }

        [Key]
        [Column(Order = 2)]
        public int? uid { get; set; }
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int rank { get; set; }
        
        public virtual Group Group { get; set; }
        
        public virtual User User { get; set; }
    }

    public enum MemberRank
    {
        MEMBER = 0,
        ADMINISTRATOR = 1,
        CREATOR = 2
    }
}
