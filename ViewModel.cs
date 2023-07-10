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
        public List<Task> Tasks { get; set; }
        private static readonly char[] punctuationMarks = {',','.','…', '?','!', ';', ':', '-', '—', '=', '-', '`', '~', '≈', '|', '║', '(', ')', '{','}','[',']','<','>','/','\\', '"', '«', '»', '⟶', '⟵', '+' };
        private ObservableCollection<string>? filesName;
        public ObservableCollection<string>? FilesName
        { get { return filesName; } set { filesName = value; OnPropertyChanged(nameof(FilesName)); } }
        private bool _isRemovingWords;
        public bool IsRemovingWords { get { return _isRemovingWords; } set { _isRemovingWords = value; OnPropertyChanged(nameof(IsRemovingWords)); } }
        private bool _isRemovingPunctuation;
        public bool IsRemovingPunctuation { get { return _isRemovingPunctuation; } set { _isRemovingPunctuation = value; OnPropertyChanged(nameof(IsRemovingPunctuation)); } }
        private string? _countCharsToRemove;
        public string? CountCharsToRemove { get { return _countCharsToRemove; } set { _countCharsToRemove = value; OnPropertyChanged(nameof(CountCharsToRemove)); } }

        public ViewModel()
        {
            FilesName = new();
            Tasks = new();
        }
        public void SetFilesName(string[] filesname) => FilesName = new ObservableCollection<string>(filesname);
        public void ProcessFile(string inputFile, string outputFile, bool RemoveWords, bool RemovePunctuation, string CountToRemove)
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
                    editedLine += (RemoveWords && !String.IsNullOrEmpty(CountToRemove) && wordToPlace.Length > 0 && wordToPlace.Length < Convert.ToInt32(CountToRemove))
                        ? "" 
                        : RemovePunctuation ? $"{wordToPlace} " : $"{word} ";
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
            Tasks.Clear();
            FilesName = new();
            IsRemovingWords = false;
            IsRemovingPunctuation = false;
            CountCharsToRemove = new string("");
        }
        public void StartProcessingTask(string a, string b)
        {
            Tasks.Add( Task.Run( () => ProcessFile(a, b, IsRemovingWords, IsRemovingPunctuation, CountCharsToRemove) ) );
        }
    }
}
