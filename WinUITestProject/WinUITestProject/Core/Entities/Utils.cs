using System;

namespace WinUITestProject.Core.Entities
{
    public abstract class TimestampedEntity
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
