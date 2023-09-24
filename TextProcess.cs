using System;
using System.IO;
using System.Text;

namespace TextProcessor
{
    /// <summary>
    /// Модель приложения
    /// </summary>
    class TextProcess
    {
        private static readonly char[] punctuationMarks = { ',', '.', '…', '?', '!', ';', ':', '-', '—', '=', '-', '`', '~', '≈', '|', '║', '(', ')', '{', '}', '[', ']', '<', '>', '/', '\\', '"', '«', '»', '⟶', '⟵', '+' };
        public void ProcessFile(string inputPath, string outputPath, bool RemoveWords, bool RemovePunctuation, string? CountToRemove, out string error)
        {
            error = string.Empty;
            string? line;
            StringBuilder stringBuilder = new();
            try
            {
                using (StreamReader reader = new(inputPath))
                using (StreamWriter writer = new(outputPath))
                {
                    stringBuilder.AppendLine(reader.ReadLine());
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
            catch (IOException IOEx) when ((IOEx.HResult & 0x0000FFFF) == 32)
            {
                error = $"Вы не можете читать и записывать в один и тот же файл одновременно! Файл {inputPath} не будет обработан";
            }
            catch (FileNotFoundException)
            {
                error = $"Запрашиваемый файл {inputPath} не найден.";
            }
            catch (IOException IOEx)
            {
                error = $"Данная операция невозможна! Файл:{inputPath}. Текст ошибки :{IOEx.Message}";
            }
            catch (Exception ex)
            {
                error = $"При обработке файла {inputPath} возникла ошибка: {ex.Message}";
            }
        }
    }
}
