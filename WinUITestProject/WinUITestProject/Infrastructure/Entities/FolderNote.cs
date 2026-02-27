

namespace WinUITestProject.Infrastructure.Entities
{
    public class FolderNote
    {
        public int FolderId { get; set; }
        public Folder? Folder { get; set; }
        public NoteId NoteId { get; set; }
        public Note? Note { get; set; }
    }
}
