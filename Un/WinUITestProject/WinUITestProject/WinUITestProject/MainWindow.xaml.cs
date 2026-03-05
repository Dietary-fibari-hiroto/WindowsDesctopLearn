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
using WinUITestProject.Pages;
using WinUITestProject.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUITestProject
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private int _tabCounter = 1;
        public MainWindow()
        {
            InitializeComponent();

            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(this);//Win32の世界のIDみたいなもの
            var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hwnd);//上記をWinUIの世界で使えるIDに変換している
            var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);
            appWindow.Resize(new Windows.Graphics.SizeInt32(1200, 800));

            Title = "TabSidebar Demo - WinUI3";

        }


        private void SidebarNav_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                SidebarStatusText.Text = "サイドバー:設定";
                AddOrSwitchToTab("設定", new SettingsPage(), Symbol.Setting);
                return;
            }

            if (args.SelectedItem is NavigationViewItem item)
            {
                var tag = item.Tag?.ToString() ?? "Unknown";
                SidebarStatusText.Text = $"サイドバー:{item.Content}";
                switch (tag)
                {
                    case "Home":
                        MainTabView.SelectedIndex = 0;
                        break;
                    case "Dashboard":
                        AddOrSwitchToTab("ダッシュボード", new DashboardPage(), Symbol.ViewAll);
                        break;
                    case "Editor":
                        AddNewNoteTab();
                        break;
                    case "Files":
                        AddOrSwitchToTab("ファイル一覧", new FilesPage(), Symbol.Document);
                        break;
                    case "Terminal":
                        AddOrSwitchToTab("ターミナル", new TerminalPage(), Symbol.OtherUser);
                        break;
                    case "Help":
                        AddOrSwitchToTab("ヘルプ", new HelpPage(), Symbol.Help);
                        break;
                    case "NoteList":
                        var listPage = CreateNoteListPage();
                        AddOrSwitchToTab("メモ一覧", listPage, Symbol.AllApps);
                        break;


                }
            }
        }

        private void MainTabView_AddTabButtonClick(TabView sender, object args)
        {
            AddNewNoteTab();
        }


        private void MainTabView_TabCloseRequested(TabView sender, TabViewTabCloseRequestedEventArgs args)
        {
            sender.TabItems.Remove(args.Tab);
        }


        private void AddNewNoteTab()
        {
            _tabCounter++;
            var tab = new TabViewItem
            {
                Header = $"メモ {_tabCounter}",
                IconSource = new SymbolIconSource { Symbol = Symbol.Edit },
                Content = new NotePage()
            };
            MainTabView.TabItems.Add(tab);
            MainTabView.SelectedItem = tab;
        }

        //tab作るかスイッチするか
        private void AddOrSwitchToTab(string header, UIElement content, Symbol icon)
        {
            foreach (var item in MainTabView.TabItems)
            {
                if (item is TabViewItem existing && existing.Header?.ToString() == header)
                {
                    MainTabView.SelectedItem = existing;
                    return;
                }
            }

            var tab = new TabViewItem
            {
                Header = header,
                IconSource = new SymbolIconSource { Symbol = icon },
                Content = content
            };
            MainTabView.TabItems.Add(tab);
            MainTabView.SelectedItem = tab;
        }

        private NoteListPage CreateNoteListPage()
        {
            var listPage = new NoteListPage();

            // ── ノートカードがクリックされた ──
            // NoteListPage の NoteOpened イベントを購読
            // → 既存のノートをタブで開く
            listPage.NoteOpened += (note) =>
            {
                OpenNoteTab(note);
            };

            // ── 新規メモボタンが押された ──
            // NoteListPage の NewNoteRequested イベントを購読
            // → 新しいメモタブを作成
            listPage.NewNoteRequested += () =>
            {
                AddNewNoteTab();
            };

            return listPage;
        }

        private void OpenNoteTab(Note note)
        {
            // 既にこのノートのタブが開いてないか確認
            foreach (var item in MainTabView.TabItems)
            {
                if (item is TabViewItem existing
                    && existing.Content is NotePage existingPage
                    && existingPage.ViewModel.NoteId == note.Id.Value)
                {
                    // 既に開いてる → そのタブに切り替え
                    MainTabView.SelectedItem = existing;
                    return;
                }
            }

            // 新しいタブとして開く
            var notePage = new NotePage(note.Id.Value);
            var header = string.IsNullOrWhiteSpace(note.Title) ? "無題のメモ" : note.Title;

            var tab = new TabViewItem
            {
                Header = header,
                IconSource = new SymbolIconSource { Symbol = Symbol.Edit },
                Content = notePage
            };

            // タイトル変更をタブヘッダーに反映
            notePage.ViewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(NoteViewModel.Title))
                {
                    tab.Header = string.IsNullOrWhiteSpace(notePage.ViewModel.Title)
                        ? "無題のメモ"
                        : notePage.ViewModel.Title;
                }
            };

            MainTabView.TabItems.Add(tab);
            MainTabView.SelectedItem = tab;
        }


    }
}
