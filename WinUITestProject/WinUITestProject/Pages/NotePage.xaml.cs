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
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WinUITestProject.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUITestProject.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NotePage : Page
    {
        public NoteViewModel ViewModel { get; }
        public NotePage()
        {
            InitializeComponent();
            ViewModel = new NoteViewModel();
            _ = ViewModel.CreateNewAsync();
        }

        public NotePage(int noteId) {
            InitializeComponent();
            ViewModel = new NoteViewModel();
            _ = ViewModel.LoadAsync(noteId);
        }

        private async void Save_Click(object sender,RoutedEventArgs args)=>
            await ViewModel.ForceSaveAsync();

        


        private void Copy_Click(object sender, RoutedEventArgs args) {
            var dp = new DataPackage();
            dp.SetText(ViewModel.Content);
            Clipboard.SetContent(dp);
            ViewModel.StatusText = "☑コピーしました。";

        }


        private void Clear_Click(object sender, RoutedEventArgs args) {
            ViewModel.Content = "";
            ViewModel.StatusText = "クリアしました。";
        }


    }
}
