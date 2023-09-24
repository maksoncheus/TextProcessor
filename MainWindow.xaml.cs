using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using System.Text.RegularExpressions;

namespace TextProcessor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new ViewModel();
        }

        private void OpenDialog(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new() {Filter = "Text files(*.txt) | *.txt", Multiselect = true};
            if (dialog.ShowDialog() == true)
            {
                (DataContext as ViewModel)?.SetFilesName(dialog.FileNames);
            }
        }
        private async void SaveFiles(object sender, RoutedEventArgs e)
        {
            string[] files = (DataContext as ViewModel)?.FilesName.ToArray();
            using (System.Windows.Forms.FolderBrowserDialog dialog = new() { Description = "Сохранить обработанные файлы" })
            {
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    foreach (string filePath in files)
                    {
                        string newFileName = $"{dialog.SelectedPath}\\{System.IO.Path.GetFileName(filePath)} +  edited {System.DateTime.Now.ToString().Replace(':','-')}.txt";
                        (DataContext as ViewModel)?.StartProcessingTask(filePath, newFileName);
                    }
                    if (DataContext is ViewModel { Tasks.Count: > 0 })
                    {
                        await Task.WhenAll((DataContext as ViewModel)?.Tasks);
                        MessageBox.Show("Все файлы обработаны");
                        (DataContext as ViewModel)?.ResetProperties();
                    }
                else MessageBox.Show("Путь не выбран!");
            }
        }
        private static readonly Regex _regex = new("[^0-9]");
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if(_regex.IsMatch(e.Text)) { e.Handled = true; return; }
        }
    }
}
