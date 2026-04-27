using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using cyberbezpieczny.Core;
using cyberbezpieczny.Models;
using System.IO;

namespace cyberbezpieczny.ViewModels
{
    public class FirewallViewModel : ObservableObject
    {
        public Action BackToMenuRequested { get; set; }

        // Lista pakietów widoczna na ekranie
        public ObservableCollection<Packet> Packets { get; set; } = new ObservableCollection<Packet>();

        private DispatcherTimer _gameTimer;
        private DispatcherTimer _spawnerTimer;
        private Random _random = new Random();

        private int _score;
        public int Score { get => _score; set { _score = value; OnPropertyChanged(); } }

        private int _lives = 100;
        public int Lives { get => _lives; set { _lives = value; OnPropertyChanged(); } }

        public ICommand RemovePacketCommand { get; }

        private double _gameSpeed = 3.0;
        private bool _showGameOver;
        public bool ShowGameOver
        {
            get { return _showGameOver; }
            set
            {
                _showGameOver = value;
                OnPropertyChanged(nameof(ShowGameOver));
            }
        }

        // 1. Lista porad
        private readonly string[] _securityTips = new string[]
        {
    "Hasło '123456' łamane jest w ułamku sekundy. Używaj minimum 12 znaków!",
    "Banki nigdy nie proszą o hasło w e-mailu. To Phishing.",
    "Nie korzystaj z bankowości na darmowym Wi-Fi w kawiarni.",
    "Aktualizacje systemu łatają dziury, przez które wchodzą wirusy.",
    "Zawsze blokuj ekran (Win+L), gdy odchodzisz od komputera.",
    "Zielona kłódka (HTTPS) nie gwarantuje, że strona jest uczciwa.",
    "Nie otwieraj załączników .exe, .bat lub .vbs od nieznajomych."
        };

        // 2. Zmienna przechowująca tekst do wyświetlenia
        private string _tipText = "";
        public string TipText
        {
            get { return _tipText; }
            set
            {
                _tipText = value;
                OnPropertyChanged(nameof(TipText));
            }
        }

        public FirewallViewModel()
        {
            RemovePacketCommand = new RelayCommand<Packet>(OnPacketClicked);
            StartGame();
        }

        private void StartGame()
        {
            _gameTimer = new DispatcherTimer();
            _gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            _gameTimer.Tick += GameLoop;
            _gameTimer.Start();

            _spawnerTimer = new DispatcherTimer();
            _spawnerTimer.Interval = TimeSpan.FromSeconds(1);
            _spawnerTimer.Tick += SpawnPacket;
            _spawnerTimer.Start();
        }

        private void SpawnPacket(object sender, EventArgs e)
        {
            bool isVirus = _random.NextDouble() > 0.3;

            // Pobieramy katalog, w którym uruchomiona jest gra (np. bin/Debug)
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;

            var packet = new Packet
            {
                X = _random.Next(50, 400),
                Y = -50,
                IsMalicious = isVirus,

                // Sklejamy pełną ścieżkę do pliku na dysku
                ImagePath = Path.Combine(baseDir, "Assets", isVirus ? "phishing.png" : "phishing1.png")
            };

            Packets.Add(packet);
        }

        private void GameLoop(object sender, EventArgs e)
        {
            for (int i = Packets.Count - 1; i >= 0; i--)
            {
                var packet = Packets[i];
                packet.Y += _gameSpeed; // Prędkość spadania

                if (packet.Y > 550) 
                {
                    if (packet.IsMalicious) Lives -= 10; 
                    Packets.RemoveAt(i);
                }
                else
                {
                    // Odświeżenie pozycji
                    int index = Packets.IndexOf(packet);
                    Packets.RemoveAt(index);
                    Packets.Insert(index, packet);
                }
            }
            CheckGameOver();
        }

        private void OnPacketClicked(Packet packet)
        {
            if (packet == null) return;

            if (packet.IsMalicious)
            {
                Score += 10;
                if (Score % 50 == 0 && _gameSpeed < 10.0)
                {
                    _gameSpeed += 0.5; 
                }
            } 
            else Lives -= 10;

            Packets.Remove(packet);
            CheckGameOver();
        }
        public void CloseGame()
        {
            BackToMenuRequested?.Invoke();
        }
        private void CheckGameOver()
        {
            if (Lives <= 0)
            {
                _gameTimer.Stop();
                _spawnerTimer.Stop();
                Random losuj = new Random();
                int indeks = losuj.Next(_securityTips.Length);
                TipText = "PORADA BEZPIECZEŃSTWA:\n" + _securityTips[indeks];
                ShowGameOver = true;

            }
        }
    }
}