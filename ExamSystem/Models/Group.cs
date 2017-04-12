namespace ExamSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Group")]
    public partial class Group
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Group()
        {
            GroupMembers = new HashSet<GroupMember>();
        }

        [Key]
        public int gid { get; set; }

        [Required]
        [StringLength(64)]
        public string group_name { get; set; }

        public int? owner_uid { get; set; }

        public int number { get; set; }

        public bool allow_join { get; set; }

        public bool allow_quit { get; set; }

        public virtual User User { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GroupMember> GroupMembers { get; set; }
    }
}
