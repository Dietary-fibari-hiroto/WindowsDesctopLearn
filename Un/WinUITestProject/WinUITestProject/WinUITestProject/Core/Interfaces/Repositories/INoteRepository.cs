using System.Collections.Generic;
using System.Threading.Tasks;
using WinUITestProject.Core.Entities;

namespace WinUITestProject.Core.Interfaces.Repositories;

public interface INoteRepository
{
    Task<List<Note>> GetAllAsync();
    Task<Note?> GetAsync(NoteId id);
    Task<Note> AddAsync(Note note);
   
    Task UpdateAsync(NoteId id,string title,string content);

    Task DeleteAsync(NoteId id);
}