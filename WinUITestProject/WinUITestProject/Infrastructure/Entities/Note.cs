using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WinUITestProject.Infrastructure.Entities
{
    public class Note: TimestampedEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public NoteId Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Content { get; set; } 
        public int? TabOrder { get; set; }
        public bool IsPinned { get; set; } = false;
        


        public List<NoteTag>? NoteTags { get; set; }
        public List<FolderNote>? FolderNotes { get; set; }
        
    }
    //型つくた
    public readonly record struct NoteId(int Value);
}
