using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WinUITestProject.Core.Interfaces.Repositories;
using WinUITestProject.Infrastructure.Data;
using WinUITestProject.Infrastructure.Entities;

namespace WinUITestProject.Infrastructure.Repositories;


public class NoteRepository : INoteRepository
{

    public async Task AddAsync(Note note)
    {
        using var db = App.CreateDbContext();
        db.Notes.Add(note);
        await db.SaveChangesAsync();
    }

    public async Task<List<Note>> GetAllAsync() {
        using var db = App.CreateDbContext();
        return await db.Notes.OrderByDescending(n => n.CreatedAt).ToListAsync();
    }

    public async Task<Note?> GetAsync(NoteId id) {
        using var db = App.CreateDbContext();
        return await db.Notes.FindAsync(id); 
    }
    public async Task UpdateContentAsync(NoteId id,string content) {
        using var db = App.CreateDbContext();
        await db.Notes
             .Where(n => n.Id == id)
             .ExecuteUpdateAsync(setters => setters
                 .SetProperty(x => x.Content, content)
                 );
    }
}