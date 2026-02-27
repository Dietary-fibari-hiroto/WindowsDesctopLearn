using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WinUITestProject.Infrastructure.Entities
{
    public class Tag
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int id { get; set; }

        public string Name { get; set; } = null!;
        public string Color { get; set; } = "ffffff";


        public List<NoteTag>? NoteTags { get; set; }

    }
}
