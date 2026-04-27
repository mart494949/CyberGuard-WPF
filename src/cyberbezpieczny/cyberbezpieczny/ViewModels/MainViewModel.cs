using cyberbezpieczny.Core;

namespace cyberbezpieczny.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set { _currentView = value; OnPropertyChanged(); }
        }

        public MainViewModel()
        {
            ShowMenu();
        }

        public void ShowMenu()
        {
            // Przekazujemy "this" (czyli ten MainViewModel) do konstruktora Menu
            MenuViewModel menu = new MenuViewModel(this);
            CurrentView = menu;
        }

        public void ShowGame()
        {
            GameViewModel game = new GameViewModel();
            // To jest bardzo ważne - to sprawia, że przycisk "Wróć" w grze działa
            game.BackToMenuRequested = ShowMenu;
            CurrentView = game;
        }

        // Zmieniamy na PUBLIC
        public void ShowFirewall()
        {
            FirewallViewModel firewall = new FirewallViewModel();
            // To sprawia, że przycisk "Wróć" w drugiej grze działa
            firewall.BackToMenuRequested = ShowMenu;
            CurrentView = firewall;
        }
    }
}