using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using WinUITestProject.Core;
using WinUITestProject.Core.Entities;
using WinUITestProject.Core.Interfaces.Repositories;
using WinUITestProject.Core.Interfaces.Services;

namespace WinUITestProject.Application.Services;

public class NoteService : INoteService
{
    private readonly INoteRepository _repo;
    public NoteService(INoteRepository repo)
    {
        _repo = repo;
    }

    public async Task<Result<List<Note>>> GetNotesAsync() {
        try
        {
            var notes = await _repo.GetAllAsync();
            return Result<List<Note>>.Success(notes);
        }
        catch (Exception ex)
        {
            return Result<List<Note>>.Fail($"メモの取得に失敗しました: {ex.Message}");
        }
    }

    public async Task<Result<Note>> GetNoteAsync(int id) {
        try
        {
            NoteId noteId = new(id);
            var note = await _repo.GetAsync(noteId);

            return note is not null ? Result<Note>.Success(note) : Result<Note>.Fail("メモが見つかりませんでした。");
        }
        catch (Exception ex) {
            return Result<Note>.Fail($"メモの取得に失敗しました。:{ex.Message}");
        }
    }


    public async Task<Result<Note>> CreateNoteAsync(string title, string content)
    {
        try {
            if (string.IsNullOrEmpty(title)) return Result<Note>.Fail("タイトルが設定されていません。");

            var note = new Note
            {
                Title = title,
                Content = content,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            Note newNote = await _repo.AddAsync(note);
            return Result<Note>.Success(newNote);

        } catch (DbUpdateException ex) {
            Debug.WriteLine("例外発生でございm素。"+ex);
            return Result<Note>.Fail("保存処理に失敗しました。データを確認してください。");
        } catch (Exception ex) {
            return Result<Note>.Fail($"予期しないエラー：{ex.Message}");
        }
    }

    public async Task<Result> UpdateNoteAsync(int id, string? title, string content) {
        try
        {
            NoteId noteId = new NoteId(id);
            if (string.IsNullOrEmpty(title)) return Result.Fail("タイトルが設定されていません。");

            await _repo.UpdateAsync(noteId, title, content);
            return Result.Success();
        }
        catch (DbUpdateConcurrencyException)
        {
            return Result.Fail("ほかの操作と競合しました。もう一度お試しください。");
        }
        catch (KeyNotFoundException)
        {
            return Result.Fail("更新対象のノートが見つかりませんでした。");
        }
        catch (Exception ex)
        {
            return Result.Fail($"例外が発生しました。:{ex.Message}");
        }
    }

    public async Task<Result> DeleteNoteAsync(int id)
    {
        try
        {
            NoteId noteId = new NoteId(id);
            await _repo.DeleteAsync(noteId);

            return Result.Success();
        }
        catch (KeyNotFoundException)
        {
            return Result.Fail("削除対象のノートが見つかりませんでした。");
        }
        catch (Exception ex) {
            return Result.Fail($"例外が発生しました。:{ex.Message}");
        }
    }


}