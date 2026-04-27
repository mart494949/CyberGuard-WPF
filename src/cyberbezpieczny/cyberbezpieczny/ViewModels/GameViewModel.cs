using cyberbezpieczny.Core;
using cyberbezpieczny.Models;
using cyberbezpieczny.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.IO;

namespace cyberbezpieczny.ViewModels
{
    public class GameViewModel : ObservableObject
    {
        // --- DANE ---
        public ObservableCollection<EmailTask> Emails { get; set; }

        private EmailTask _currentEmail;
        public EmailTask CurrentEmail
        {
            get => _currentEmail;
            set { _currentEmail = value; OnPropertyChanged(); }
        }

        private int _score;
        public int Score
        {
            get => _score;
            set { _score = value; OnPropertyChanged(); }
        }

        // KOMENDY
        public ICommand MarkAsPhishingCommand { get; }
        public ICommand MarkAsSafeCommand { get; }
        public Action BackToMenuRequested { get; set; }
        public ICommand GoBackCommand { get; }

        public GameViewModel()
        {
            Emails = new ObservableCollection<EmailTask>();
            Score = 0;

            // 1. Wczytuje dane i parsujemy tekst
            LoadData();

            CurrentEmail = Emails.FirstOrDefault();

            // 2. Obsługa przycisków
            MarkAsPhishingCommand = new RelayCommand(() => CheckAnswer(true));
            MarkAsSafeCommand = new RelayCommand(() => CheckAnswer(false));
            GoBackCommand = new RelayCommand(() => BackToMenuRequested?.Invoke());
        }

        private void LoadData()
        {
            FileManager fileManager = new FileManager();

            // 1. Buduje ścieżkę do folderu Assets tam
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string path = Path.Combine(baseDir, "Assets", "phishing.json");

            // DEBUG:
            // MessageBox.Show($"Szukam pliku tutaj:\n{path}", "Debug Ścieżki");

            // 2. Sprawdzenie czy plik istnieje
            if (!File.Exists(path))
            {
                //Czasem plik ląduje w głównym folderze, a nie w Assets
                string alternativePath = Path.Combine(baseDir, "phishing.json");
                if (File.Exists(alternativePath))
                {
                    path = alternativePath;
                }
                else
                {
                    // Jeśli nigdzie go nie ma -> BŁĄD
                    Emails.Add(new EmailTask
                    {
                        Sender = "SYSTEM ERROR",
                        Subject = "Brak pliku",
                        Content = $"Nie znaleziono pliku pod adresem:\n{path}\n\nUpewnij się, że w Visual Studio we właściwościach pliku 'phishing.json' ustawiłeś opcję 'Kopiuj do katalogu wyjściowego' na 'Kopiuj zawsze'.",
                        IsPhishing = false,
                        Explanation = "Błąd konfiguracji projektu."
                    });
                    return; // Przerywamy wczytywanie
                }
            }

            // 3. Wczytywanie (jeśli plik istnieje)
            Module loadedModule = fileManager.LoadModule(path);

            if (loadedModule != null && loadedModule.Challenges != null)
            {
                foreach (var challenge in loadedModule.Challenges)
                {
                    // Parsowanie formatu tekstowego
                    string raw = challenge.QuestionText;
                    string sender = "Nieznany";
                    string subject = "Brak tematu";
                    string content = raw;

                    var lines = raw.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                    var senderLine = lines.FirstOrDefault(l => l.Trim().StartsWith("OD:", StringComparison.OrdinalIgnoreCase));
                    if (senderLine != null)
                    {
                        sender = senderLine.Replace("OD:", "", StringComparison.OrdinalIgnoreCase).Trim();
                        lines.Remove(senderLine);
                    }

                    var subjectLine = lines.FirstOrDefault(l => l.Trim().StartsWith("TEMAT:", StringComparison.OrdinalIgnoreCase));
                    if (subjectLine != null)
                    {
                        subject = subjectLine.Replace("TEMAT:", "", StringComparison.OrdinalIgnoreCase).Trim();
                        lines.Remove(subjectLine);
                    }

                    content = string.Join("\n", lines).Trim();

                    Emails.Add(new EmailTask
                    {
                        Sender = sender,
                        Subject = subject,
                        Content = content,
                        IsPhishing = !challenge.IsSafe,
                        Explanation = challenge.Explanation
                    });
                }
            }
        }

        private void CheckAnswer(bool playerThinkIsPhishing)
        {
            if (CurrentEmail == null) return;

            if (playerThinkIsPhishing == CurrentEmail.IsPhishing)
            {
                Score += 100;
                MessageBox.Show($"✅ Dobra robota!\n\n{CurrentEmail.Explanation}", "Sukces");
            }
            else
            {
                Score -= 50;
                MessageBox.Show($"❌ Błąd!\n\n{CurrentEmail.Explanation}", "Pomyłka");
            }

            var emailToRemove = CurrentEmail;
            var nextEmail = Emails.FirstOrDefault(e => e != emailToRemove);
            Emails.Remove(emailToRemove);

            if (nextEmail != null)
            {
                CurrentEmail = nextEmail;
            }
            else
            {
                MessageBox.Show($"Koniec zadań! Twój wynik: {Score}", "Gratulacje");
                BackToMenuRequested?.Invoke();
            }
        }
    }
}