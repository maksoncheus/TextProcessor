using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace TextProcessor
{
    internal class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        private static readonly char[] punctuationMarks = {',','.','…', '?','!', ';', ':', '-', '—', '=', '-', '`', '~', '≈', '|', '║', '(', ')', '{','}','[',']','<','>','/','\\', '"', '«', '»', '⟶', '⟵', '+' };
        private ObservableCollection<string>? filesName;
        public ObservableCollection<string>? FilesName
        { get { return filesName; } set { filesName = value; OnPropertyChanged(nameof(FilesName)); } }
        private static bool _isRemovingWords;
        public bool IsRemovingWords { get { return _isRemovingWords; } set { _isRemovingWords = value; OnPropertyChanged(nameof(IsRemovingWords)); } }
        private static bool _isRemovingPunctuation;
        public bool IsRemovingPunctuation { get { return _isRemovingPunctuation; } set { _isRemovingPunctuation = value; OnPropertyChanged(nameof(IsRemovingPunctuation)); } }
        private static string? _countCharsToRemove;
        public string? CountCharsToRemove { get { return _countCharsToRemove; } set { _countCharsToRemove = value; OnPropertyChanged(nameof(CountCharsToRemove)); } }

        public ViewModel()
        {
            FilesName = new();
        }
        public void SetFilesName(string[] filesname) => FilesName = new ObservableCollection<string>(filesname);
        public static void ProcessFile(string inputFile, string outputFile)
        {
            string? line;
            StreamReader reader = new(inputFile);
            StreamWriter writer = new(outputFile);
            line = reader.ReadLine();
            while (line != null)
            {
                string editedLine = "";
                string[] words = line.Split(' ');
                foreach (string word in words)
                {
                    string wordToPlace = word.Trim(punctuationMarks);
                    editedLine += (_isRemovingWords && !String.IsNullOrEmpty(_countCharsToRemove) && wordToPlace.Length > 0 && wordToPlace.Length < Convert.ToInt32(_countCharsToRemove))
                        ? "" 
                        : _isRemovingPunctuation ? $"{wordToPlace} " : $"{word} ";
                }
                if(editedLine.Contains(' ')) editedLine = editedLine.Remove(editedLine.LastIndexOf(' '));
                if (editedLine.Length > 0)
                    writer.WriteLine(editedLine);
                line = reader.ReadLine();
            }
            reader.Close();
            writer.Close();
        }
        public void ResetProperties()
        {
            FilesName = new();
            IsRemovingWords = false;
            IsRemovingPunctuation = false;
            CountCharsToRemove = new string("");
        }
    }
}
