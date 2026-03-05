using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WinUITestProject.Core.Entities;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUITestProject.Components
{
    public sealed partial class NoteCard : UserControl
    {
        public event Action<Note>? NoteSelected;

        public event Action<Note>? DeleteRequested;

        public event Action<Note>? PinToggled;

        private Note? _note;

        public NoteCard()
        {
            this.InitializeComponent();
        }


        public void SetNote(Note note) {
            _note = note;

            TitleText.Text = string.IsNullOrWhiteSpace(note.Title) ? "無題のメモ" : note.Title;

            PreviewText.Text = string.IsNullOrWhiteSpace(note.Content)
                ? "(内容無し)"
                : note.Content.Length > 100
                ? note.Content[..100] + "..."
                : note.Content;

            DateText.Text = note.UpdatedAt.ToString("yyyy/MM/dd HH:mm");
            PinIcon.Glyph = note.IsPinned ? "\uE841" : "\uE718";

        }

        //UIイベント
        private void CardButton_Click(object sender, RoutedEventArgs e) {
            if (_note is not null) NoteSelected?.Invoke(_note);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e) {
            if (_note is not null) DeleteRequested?.Invoke(_note);
        }

        private void PinButton_Click(object sender, RoutedEventArgs e) {
            if (_note is not null) PinToggled?.Invoke(_note);
        }

        //ホバーエフェクト
        private void Border_PointerEntered(object sender, PointerRoutedEventArgs e) {
            if (sender is Border border) border.Background = (Brush)Microsoft.UI.Xaml.Application.Current.Resources["BorderBrush"];
        }

        private void Border_PointerExited(object sender, PointerRoutedEventArgs e) {
            if (sender is Border border) border.Background = (Brush)Microsoft.UI.Xaml.Application.Current.Resources["AccentStrongBrush"];
        }




    }

}
 