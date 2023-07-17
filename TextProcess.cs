using System;
using System.IO;
namespace TextProcessor
{
    /// <summary>
    /// Модель приложения
    /// </summary>
    class TextProcess
    {

        private static readonly char[] punctuationMarks = { ',', '.', '…', '?', '!', ';', ':', '-', '—', '=', '-', '`', '~', '≈', '|', '║', '(', ')', '{', '}', '[', ']', '<', '>', '/', '\\', '"', '«', '»', '⟶', '⟵', '+' };
        public void ProcessFile(string inputFile, string outputFile, bool RemoveWords, bool RemovePunctuation, string CountToRemove, out string error)
        {
            error = string.Empty;
            string? line;
            try
            {
                using (StreamReader reader = new(inputFile))
                using (StreamWriter writer = new(outputFile))
                {
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
                        if (editedLine.Contains(' ')) editedLine = editedLine.Remove(editedLine.LastIndexOf(' '));
                        if (editedLine.Length > 0)
                            writer.WriteLine(editedLine);
                        line = reader.ReadLine();
                    }
                }
            }
            catch (IOException IOEx)
            {
                if ((IOEx.HResult & 0x0000FFFF) == 32)
                    error = $"Вы не можете читать и записывать в один и тот же файл одновременно! Файл {inputFile} не будет обработан";
                else
                    error = $"Данная операция невозможна! Файл:{inputFile}. Текст ошибки :{IOEx.Message}";
            }
            catch (Exception ex)
            {
                error = $"При обработке файла {inputFile} возникла ошибка: {ex.Message}";
            }
        }
    }
}
