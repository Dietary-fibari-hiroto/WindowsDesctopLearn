

namespace WinUITestProject.Infrastructure.Entities
{
    public class NoteTag
    {
        public NoteId NoteId { get; set; }
        public Note? Note { get; set; }
        public int TagId { get; set; }
        public Tag? Tag { get; set; }
    }
}
