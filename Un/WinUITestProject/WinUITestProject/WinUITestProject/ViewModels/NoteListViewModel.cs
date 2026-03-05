using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinUITestProject.Core.Entities;
using WinUITestProject.Core.Interfaces.Services;

namespace WinUITestProject.ViewModels
{
    public class NoteListViewModel:ViewModelBase
    {
        private readonly INoteService _noteService;

        private string _statusText = "";
        private bool _isLoading;

        public ObservableCollection<Note> Notes { get; } = []; 


        public string StatusText
        {
            get => _statusText;
            set => SetProperty(ref _statusText, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public NoteListViewModel()
        {
            _noteService = App.Services.GetRequiredService<INoteService>();
        }

        public async Task LoadNotesAsync()
        {
            IsLoading = true;
            StatusText = "読み込み中...";

            var result = await _noteService.GetNotesAsync();

            Notes.Clear();

            if (result.IsSuccess && result.Value is not null)
            {
                foreach (var note in result.Value)
                {
                    Notes.Add(note);
                }
                StatusText = $"{Notes.Count}件のメモ";
            }
            else
            {
                StatusText = $"{Notes.Count}";
            }

            IsLoading = false;
        }

        public async Task<bool> DeleteNoteAsync(Note note)
        {
            var result = await _noteService.DeleteNoteAsync(note.Id.Value);

            if (result.IsSuccess) {
                Notes.Remove(note);
                StatusText = $"{Notes.Count}件のメモ";
                return true;
            }
            StatusText = $"⚠ {result.Error}";
            return false;
        }

    }
    
}
