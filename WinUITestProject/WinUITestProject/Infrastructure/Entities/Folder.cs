using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WinUITestProject.Infrastructure.Entities
{
    public class Folder
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Parent { get; set; }

        public List<FolderNote>? FolderNotes { get; set; }
    }
}
