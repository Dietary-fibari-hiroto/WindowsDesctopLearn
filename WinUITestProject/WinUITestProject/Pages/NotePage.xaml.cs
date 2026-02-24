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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUITestProject.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NotePage : Page
    {
        public NotePage()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender,RoutedEventArgs args)
        {
            StatusText.Text = "☑保存しました(デモ)";
        }

        private void Clear_Click(object sender, RoutedEventArgs args)
        {
            EditorBox.Text = string.Empty;
            StatusText.Text = "クリアしました。";
        }

        private void Copy_Click(object sender, RoutedEventArgs args) {
            var dp = new DataPackage();
            dp.SetText(EditorBox.Text);
            Clipboard.SetContent(dp);
            StatusText.Text = "☑コピーしました。";

        }

        private void EditorBox_TextChanged(object sender, TextChangedEventArgs args)
        {
            var text = EditorBox.Text;
            CharCountText.Text = $"文字数: {text.Length}";
            LineCountText.Text = $"行数: {text.Split('\n').Length}";
        }

    }
}
