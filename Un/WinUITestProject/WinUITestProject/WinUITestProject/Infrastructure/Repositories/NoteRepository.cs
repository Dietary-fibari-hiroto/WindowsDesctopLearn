using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WinUITestProject.Core.Entities;
using WinUITestProject.Core.Interfaces.Repositories;
using WinUITestProject.Infrastructure.Data;

namespace WinUITestProject.Infrastructure.Repositories;


public class NoteRepository : INoteRepository
{



    public async Task<List<Note>> GetAllAsync() {
        using var db = App.CreateDbContext();
        return await db.Notes.OrderByDescending(n => n.CreatedAt).ToListAsync();
    }

    public async Task<Note?> GetAsync(NoteId id) {
        using var db = App.CreateDbContext();
        return await db.Notes.FindAsync(id); 
    }

    public async Task<Note> AddAsync(Note note)
    {
        using var db = App.CreateDbContext();
        db.Notes.Add(note);
        await db.SaveChangesAsync();
        return note;
    }

    public async Task UpdateAsync(NoteId id,string title,string content) {
        using var db = App.CreateDbContext();

        var note = await db.Notes.FindAsync(id);
        if (note == null) throw new KeyNotFoundException($"Note Id={id} が見つかりません。");
        if (title != null) note.Title = title;
        if (content != null) note.Content = content;

        await db.SaveChangesAsync();
        
    }


    public async Task DeleteAsync(NoteId id)
    {
        using var db = App.CreateDbContext();

        var note = await db.Notes.FindAsync(id);
        if (note == null) throw new KeyNotFoundException($"Note Id={id} が見つかりませんでした。");

        db.Notes.Remove(note);

        await db.SaveChangesAsync();

    }
}