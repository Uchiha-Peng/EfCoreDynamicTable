

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EfCoreDynamicTable.Entities
{
    public class Table
    {
        [Key]
        [Column(TypeName = "char(36)")]
        public Guid Id { get; set; }

        [Required]
        [StringLength(36)]
        public string TableName { get; set; }

        public DateTime CreateAt { get; set; }
    }
}
