using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using BetManagement.Core.Models;

namespace BetManagement.Desktop.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        // Match Header State
        private int _matchNo = 354;
        private string _teamsDisplay = "WID VS ZIM";
        private DateTime _matchDate = DateTime.Now;
        private string _matchType = "One Day";
        private int _matchDay = 1;
        private string _teamA = "WID";
        private string _teamB = "ZIM";

        // Bet Entry Form State
        private string _selectedCustomer = "John Doe";
        private decimal _inputRate = 1.85m;
        private decimal _inputAmount = 1000m;
        private string _selectedBetType = "Normal";
        private string _selectedTeam = "WID";

        // Exposure Summaries
        private decimal _teamAExposure;
        private decimal _teamBExposure;

        public MainViewModel()
        {
            Bets = new ObservableCollection<BetItem>();
            CustomerList = new ObservableCollection<string> { "John Doe", "Alex Smith", "Irha Waseem", "Client 102" };
            TeamList = new ObservableCollection<string> { "WID", "ZIM" };
            MatchTypeList = new ObservableCollection<string> { "One Day", "T20", "Test Match" };
            BetTypeList = new ObservableCollection<string> { "Normal", "Session" };

            // Commands
            SaveBetCommand = new RelayCommand(_ => AddBet());
            NextMatchCommand = new RelayCommand(_ => MatchNo++);
            PrevMatchCommand = new RelayCommand(_ => { if (MatchNo > 1) MatchNo--; });

            // Seed Sample Bet
            AddSampleData();
        }

        public ObservableCollection<BetItem> Bets { get; }
        public ObservableCollection<string> CustomerList { get; }
        public ObservableCollection<string> TeamList { get; }
        public ObservableCollection<string> MatchTypeList { get; }
        public ObservableCollection<string> BetTypeList { get; }

        public int MatchNo { get => _matchNo; set { _matchNo = value; OnPropertyChanged(); } }
        public string TeamsDisplay { get => _teamsDisplay; set { _teamsDisplay = value; OnPropertyChanged(); } }
        public DateTime MatchDate { get => _matchDate; set { _matchDate = value; OnPropertyChanged(); } }
        public string MatchType { get => _matchType; set { _matchType = value; OnPropertyChanged(); } }
        public int MatchDay { get => _matchDay; set { _matchDay = value; OnPropertyChanged(); } }

        public string SelectedCustomer { get => _selectedCustomer; set { _selectedCustomer = value; OnPropertyChanged(); } }
        public decimal InputRate { get => _inputRate; set { _inputRate = value; OnPropertyChanged(); } }
        public decimal InputAmount { get => _inputAmount; set { _inputAmount = value; OnPropertyChanged(); } }
        public string SelectedBetType { get => _selectedBetType; set { _selectedBetType = value; OnPropertyChanged(); } }
        public string SelectedTeam { get => _selectedTeam; set { _selectedTeam = value; OnPropertyChanged(); } }

        public decimal TeamAExposure { get => _teamAExposure; set { _teamAExposure = value; OnPropertyChanged(); } }
        public decimal TeamBExposure { get => _teamBExposure; set { _teamBExposure = value; OnPropertyChanged(); } }

        public ICommand SaveBetCommand { get; }
        public ICommand NextMatchCommand { get; }
        public ICommand PrevMatchCommand { get; }

        private void AddBet()
        {
            if (string.IsNullOrWhiteSpace(SelectedCustomer) || InputAmount <= 0) return;

            var bet = new BetItem
            {
                SNo = Bets.Count + 1,
                CustomerName = SelectedCustomer,
                Rate = InputRate,
                BetAmount = InputAmount,
                BetType = SelectedBetType,
                Team = SelectedTeam,
                BackLay = "B"
            };

            Bets.Add(bet);
            RecalculateExposure();
            
            // Reset input field
            InputAmount = 0;
        }

        private void RecalculateExposure()
        {
            decimal teamANet = 0;
            decimal teamBNet = 0;

            foreach (var b in Bets)
            {
                if (b.Team == _teamA)
                {
                    teamANet += (b.TotalAmount - b.BetAmount);
                    teamBNet -= b.BetAmount;
                }
                else if (b.Team == _teamB)
                {
                    teamBNet += (b.TotalAmount - b.BetAmount);
                    teamANet -= b.BetAmount;
                }
            }

            TeamAExposure = teamANet;
            TeamBExposure = teamBNet;
        }

        private void AddSampleData()
        {
            Bets.Add(new BetItem { SNo = 1, CustomerName = "Irha Waseem", Rate = 1.50m, BetAmount = 5000, BetType = "Normal", Team = "WID", BackLay = "B" });
            Bets.Add(new BetItem { SNo = 2, CustomerName = "Alex Smith", Rate = 2.10m, BetAmount = 2000, BetType = "Normal", Team = "ZIM", BackLay = "B" });
            RecalculateExposure();
        }
    }

    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public class RelayCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Func<object?, bool>? _canExecute;

        public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter) => _canExecute == null || _canExecute(parameter);
        public void Execute(object? parameter) => _execute(parameter);
        public event EventHandler? CanExecuteChanged { add { } remove { } }
    }
}