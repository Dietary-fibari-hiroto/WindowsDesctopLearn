using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Linq;
using System.Threading.Tasks;
using WinUITestProject.Components;
using WinUITestProject.Core.Entities;
using WinUITestProject.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUITestProject.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NoteListPage : Page
    {

        public NoteListViewModel ViewModel { get; }

        public event Action<Note>? NoteOpened;
        public event Action? NewNoteRequested;
        public NoteListPage()
        {
            InitializeComponent();
            ViewModel = new NoteListViewModel();
            this.Loaded += NoteListPage_Loaded;
        }

        private async void NoteListPage_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadNotesAsync();
            RenderNoteCards();
            UpdateVisibility();
        }

        // ── カード一覧の描画 ──

        private void RenderNoteCards(string? searchQuery = null)
        {
            // ItemsRepeater に手動でカードを配置
            var panel = new StackPanel { Spacing = 8 };

            var notes = ViewModel.Notes.AsEnumerable();

            // 検索フィルタ
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                notes = notes.Where(n =>
                    (n.Title?.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (n.Content?.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ?? false)
                );
            }

            // ピン止めを上に
            notes = notes
                .OrderByDescending(n => n.IsPinned)
                .ThenByDescending(n => n.UpdatedAt);

            foreach (var note in notes)
            {
                var card = new NoteCard();
                card.SetNote(note);

                // ── ここがポイント: カードのイベントを購読 ──
                // カードクリック → このページの NoteOpened イベントを発火
                card.NoteSelected += (selectedNote) =>
                {
                    NoteOpened?.Invoke(selectedNote);
                };

                // 削除要求 → 確認ダイアログ後に削除
                card.DeleteRequested += async (targetNote) =>
                {
                    var dialog = new ContentDialog
                    {
                        Title = "削除確認",
                        Content = $"「{targetNote.Title}」を削除しますか？",
                        PrimaryButtonText = "削除",
                        CloseButtonText = "キャンセル",
                        DefaultButton = ContentDialogButton.Close,
                        XamlRoot = this.XamlRoot
                    };

                    if (await dialog.ShowAsync() == ContentDialogResult.Primary)
                    {
                        var success = await ViewModel.DeleteNoteAsync(targetNote);
                        if (success)
                        {
                            RenderNoteCards(SearchBox.Text);
                            UpdateVisibility();
                        }
                    }
                };

                panel.Children.Add(card);
            }

            // ItemsRepeater の代わりに ScrollViewer の中身を差し替え
            var scrollViewer = FindName("NoteRepeater") as ScrollViewer;

            // シンプルにItemsRepeaterの親を使う
            NoteRepeater.Visibility = Visibility.Collapsed;

            // 既存のカードパネルを探して差し替え
            var grid = (Grid)this.Content;
            var existingPanel = grid.Children
                .OfType<ScrollViewer>()
                .FirstOrDefault();

            if (existingPanel is not null)
            {
                existingPanel.Content = panel;
            }

            StatusTextBlock.Text = ViewModel.StatusText;
        }

        private void UpdateVisibility()
        {
            var hasNotes = ViewModel.Notes.Count > 0;
            EmptyState.Visibility = hasNotes ? Visibility.Collapsed : Visibility.Visible;
        }

        // ── UIイベント ──

        private void NewNote_Click(object sender, RoutedEventArgs e)
        {
            // 親に「新規メモ作って」と通知
            NewNoteRequested?.Invoke();
        }

        private async void Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadingBar.Visibility = Visibility.Visible;
            await ViewModel.LoadNotesAsync();
            RenderNoteCards(SearchBox.Text);
            UpdateVisibility();
            LoadingBar.Visibility = Visibility.Collapsed;
        }

        private void SearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                RenderNoteCards(sender.Text);
            }
        }

        /// <summary>
        /// 外部からリフレッシュを要求するためのpublicメソッド
        /// （例: 他のタブでメモが更新された後に一覧を再取得）
        /// </summary>
        public async Task RefreshAsync()
        {
            await ViewModel.LoadNotesAsync();
            RenderNoteCards(SearchBox.Text);
            UpdateVisibility();
        }
    }
}