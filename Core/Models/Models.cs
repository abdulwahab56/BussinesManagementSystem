using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BetManagement.Core.Models
{
    public class Team
    {
        public int TeamId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string NickName { get; set; } = string.Empty;
        public bool IsSelected { get; set; }
    }

    public class BetItem : INotifyPropertyChanged
    {
        private int _sNo;
        private string _customerName = string.Empty;
        private decimal _rate;
        private decimal _betAmount;
        private string _betType = "Normal";
        private string _team = string.Empty;
        private string _backLay = "B";
        private decimal _totalAmount;

        public int SNo { get => _sNo; set { _sNo = value; OnPropertyChanged(); } }
        public string CustomerName { get => _customerName; set { _customerName = value; OnPropertyChanged(); } }
        public decimal Rate { get => _rate; set { _rate = value; RecalculateTotal(); OnPropertyChanged(); } }
        public decimal BetAmount { get => _betAmount; set { _betAmount = value; RecalculateTotal(); OnPropertyChanged(); } }
        public string BetType { get => _betType; set { _betType = value; OnPropertyChanged(); } }
        public string Team { get => _team; set { _team = value; OnPropertyChanged(); } }
        public string BackLay { get => _backLay; set { _backLay = value; OnPropertyChanged(); } }
        public decimal TotalAmount { get => _totalAmount; set { _totalAmount = value; OnPropertyChanged(); } }

        private void RecalculateTotal()
        {
            TotalAmount = Rate * BetAmount;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public class TeamExposure
    {
        public string TeamName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public bool IsLoss => Amount < 0;
    }
}
