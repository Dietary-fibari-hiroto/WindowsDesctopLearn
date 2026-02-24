using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WinUITestProject.Infrastructure.Entities
{
    public class Note: TimestampedEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Content { get; set; } 
        public int? TabOrder { get; set; }
        public bool IsPinned { get; set; } = false;
        
        
    }
}
