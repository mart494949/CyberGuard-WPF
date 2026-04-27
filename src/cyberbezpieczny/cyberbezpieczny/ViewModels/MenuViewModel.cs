using cyberbezpieczny.Core;
using System.Windows.Input;

namespace cyberbezpieczny.ViewModels
{
    public class MenuViewModel : ObservableObject
    {
        // To jest nasz "pilot" do zmiany ekranów
        private MainViewModel _mainViewModel;

        // Zmienna sterująca widocznością Intro (Overlay)
        private bool _isIntroVisible = true;
        public bool IsIntroVisible
        {
            get { return _isIntroVisible; }
            set { _isIntroVisible = value; OnPropertyChanged(); }
        }

        public ICommand CloseIntroCommand { get; }
        public ICommand VerifyCommand { get; }
        public ICommand FirewallCommand { get; }
        public ICommand ExitCommand { get; }

        // KONSTRUKTOR - Tu musimy przyjąć MainViewModel
        public MenuViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;

            // 1. Zamknięcie Intro
            CloseIntroCommand = new RelayCommand(() =>
            {
                IsIntroVisible = false;
            });

            // 2. Otwarcie Gry w Phishing
            VerifyCommand = new RelayCommand(() =>
            {
                _mainViewModel.ShowGame();
            });

            // 3. Otwarcie Gry Firewall
            FirewallCommand = new RelayCommand(() =>
            {
                _mainViewModel.ShowFirewall();
            });

            // 4. Wyjście
            ExitCommand = new RelayCommand(() =>
            {
                System.Windows.Application.Current.Shutdown();
            });
        }
    }
}