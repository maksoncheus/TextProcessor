using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Threading;
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
            foreach (string filePath in files)
            {
                SaveFileDialog dialog = new() { DefaultExt = ".txt", AddExtension = true, Title = $"Сохранить обработанный файл {System.IO.Path.GetFileName(filePath)}"};
                while (dialog.ShowDialog() != true) { MessageBox.Show("Пожалуйста, укажите место сохранения файла"); }
                (DataContext as ViewModel)?.StartProcessingTask(filePath, dialog.FileName);
            }
            if ((DataContext as ViewModel)?.Tasks.Count > 0)
            {
                await Task.WhenAll((DataContext as ViewModel)?.Tasks);
                MessageBox.Show("Все файлы обработаны");
                (DataContext as ViewModel)?.ResetProperties();
            }
            else MessageBox.Show("Файлы не выбраны!");
        }
        private static readonly Regex _regex = new("[^0-9]");
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if(_regex.IsMatch(e.Text)) { e.Handled = true; return; }
        }
    }
}
