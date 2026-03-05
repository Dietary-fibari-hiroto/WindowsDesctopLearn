using System.Collections.Generic;
using System.Threading.Tasks;
using WinUITestProject.Core.Entities;

namespace WinUITestProject.Core.Interfaces.Services;

public interface INoteService
{
    Task<Result<List<Note>>> GetNotesAsync();
    Task<Result<Note>> GetNoteAsync(int id);
    Task<Result<Note>> CreateNoteAsync(string title,string content);

    Task<Result> UpdateNoteAsync(int id, string? title, string content);

    Task<Result> DeleteNoteAsync(int id);
}