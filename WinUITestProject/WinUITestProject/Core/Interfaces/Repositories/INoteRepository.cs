using System.Collections.Generic;
using System.Threading.Tasks;
using WinUITestProject.Infrastructure.Entities;

namespace WinUITestProject.Core.Interfaces.Repositories;

public interface INoteRepository
{
    Task AddAsync(Note note);
    Task<List<Note>> GetAllAsync();
    Task<Note?> GetAsync(NoteId id);
    Task UpdateContentAsync(NoteId id,string content);
}