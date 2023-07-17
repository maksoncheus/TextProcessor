using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;

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
        private TextProcess _process;
        private bool _isRemovingWords;
        private bool _isRemovingPunctuation;
        private ObservableCollection<string>? filesName;
        private string? _countCharsToRemove;
        public List<Task> Tasks { get; set; }
        public ObservableCollection<string>? FilesName
        { get { return filesName; } set { filesName = value; OnPropertyChanged(nameof(FilesName)); } }
        public bool IsRemovingWords { get { return _isRemovingWords; } set { _isRemovingWords = value; OnPropertyChanged(nameof(IsRemovingWords)); } }
        public bool IsRemovingPunctuation { get { return _isRemovingPunctuation; } set { _isRemovingPunctuation = value; OnPropertyChanged(nameof(IsRemovingPunctuation)); } }
        public string? CountCharsToRemove { get { return _countCharsToRemove; } set { _countCharsToRemove = value; OnPropertyChanged(nameof(CountCharsToRemove)); } }

        public ViewModel()
        {
            _process = new TextProcess();
            FilesName = new();
            Tasks = new();
        }
        public void SetFilesName(string[] filesname) => FilesName = new ObservableCollection<string>(filesname);
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
            string error;
            Tasks.Add(Task.Run(
                () =>
                    { 
                        _process.ProcessFile(a, b, IsRemovingWords, IsRemovingPunctuation, CountCharsToRemove, out error);
                        if (!string.IsNullOrEmpty(error))
                            MessageBox.Show(error);
                    }
                )
            );
        }
    }
}
