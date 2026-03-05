using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WinUITestProject.Core;
using WinUITestProject.Core.Interfaces.Services;

namespace WinUITestProject.ViewModels;

public class NoteViewModel : ViewModelBase
{
    private readonly INoteService _noteService;
    private CancellationTokenSource? _saveCts;

    private int _noteId;
    private string _title = "";
    private string _content = "";
    private string _statusText = "";
    private bool _isSaving;

    public int NoteId
    {
        get => _noteId;
        set => SetProperty(ref _noteId, value);
    }

    public string Title
    {
        get => _title;
        set
        {
            if (SetProperty(ref _title, value))
            {
                ScheduleAutoSave();
            }
        }
    }


    public string Content
    {
        get => _content;
        set
        {
            if (SetProperty(ref _content, value))
            {
                OnPropertyChanged(nameof(CharCount));
                OnPropertyChanged(nameof(LineCount));
                ScheduleAutoSave();
            }
        }
    }

    public string StatusText
    {
        get => _statusText;
        set => SetProperty(ref _statusText, value);
    }

    public bool IsSaving {
        get => _isSaving;
        private set => SetProperty(ref _isSaving, value);
    }

    public int CharCount => _content.Length;
    public int LineCount => _content.Split('\n').Length;

    //コンストラクタ
    public NoteViewModel()
    {
        _noteService = App.Services.GetRequiredService<INoteService>();
    }

    public async Task LoadAsync(int noteId) {
        var result = await _noteService.GetNoteAsync(noteId);

        if (result.IsSuccess && result.Value is not null)
        {
            NoteId = result.Value.Id.Value;
            _title = result.Value.Title;

            _content = result.Value.Content ?? "";

            OnPropertyChanged(nameof(Title));
            OnPropertyChanged(nameof(Content));
            OnPropertyChanged(nameof(CharCount));
            OnPropertyChanged(nameof(LineCount));
            StatusText = $"✓ 読み込み済み ({result.Value.UpdatedAt:HH:mm})";
        }
        else
        {
            StatusText = $"⚠ {result.Error}";
        }

    }

    public async Task CreateNewAsync()
    {
        var result = await _noteService.CreateNoteAsync("無題のメモ", "");

        if (result.IsSuccess && result.Value is not null)
        {
            NoteId = result.Value.Id.Value;
            _title = result.Value.Title;
            OnPropertyChanged(nameof(Title));
            StatusText = "新規作成";
        }
        else
        {
            StatusText = $"⚠ {result.Error}";
        }
    }


    //自動保存(でバウンス)
    private void ScheduleAutoSave()
    {
        if (NoteId == 0) return;

        _saveCts?.Cancel();
        _saveCts = new CancellationTokenSource();
        _ = AutoSaveAsync(_saveCts.Token);
    }

    private async Task AutoSaveAsync(CancellationToken ct) {
        StatusText = "変更あり...";
        try {
            await Task.Delay(600, ct);
            IsSaving = true;
            var result = await _noteService.UpdateNoteAsync(NoteId,null, Content);
            if (ct.IsCancellationRequested) return;
            StatusText = result.IsSuccess
                ? $"☑自動保存({DateTime.Now:HH:mm:ss})" : $"⚠{result.Error}";
        } catch (OperationCanceledException) { } finally { IsSaving = false; }
    }

    public async Task ForceSaveAsync() {
        _saveCts?.Cancel();
        IsSaving = true;

        var result = await _noteService.UpdateNoteAsync(NoteId, null, Content);

        IsSaving = false;
        StatusText = result.IsSuccess
            ? $"✓ 保存完了 ({DateTime.Now:HH:mm:ss})"
            : $"⚠ {result.Error}";
    }


    public async Task<Result> DeleteAsync()
    {
        return await _noteService.DeleteNoteAsync(NoteId);
    }
}