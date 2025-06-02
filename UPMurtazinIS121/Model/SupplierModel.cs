using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace UPMurtazinIS121.Model
{
    public class SupplierModel(Suppliers supplier) : INotifyPropertyChanged, IDataErrorInfo
    {
        private readonly Suppliers _supplier = supplier;

        public string Name
        {
            get => _supplier.Name;
            set { if (_supplier.Name != value) { _supplier.Name = value; OnPropertyChanged(); } }
        }

        public string Type
        {
            get => _supplier.Type;
            set { if (_supplier.Type != value) { _supplier.Type = value; OnPropertyChanged(); } }
        }

        public string YuridAdres
        {
            get => _supplier.YuridAdres;
            set { if (_supplier.YuridAdres != value) { _supplier.YuridAdres = value; OnPropertyChanged(); } }
        }

        public double? INN
        {
            get => _supplier.INN;
            set { if (_supplier.INN != value) { _supplier.INN = value; OnPropertyChanged(); } }
        }

        public string FIO
        {
            get => _supplier.FIO;
            set { if (_supplier.FIO != value) { _supplier.FIO = value; OnPropertyChanged(); } }
        }

        public string Phone
        {
            get => _supplier.Phone;
            set { if (_supplier.Phone != value) { _supplier.Phone = value; OnPropertyChanged(); } }
        }

        public string Email
        {
            get => _supplier.Email;
            set { if (_supplier.Email != value) { _supplier.Email = value; OnPropertyChanged(); } }
        }

        public string Logo
        {
            get => _supplier.Logo;
            set { if (_supplier.Logo != value) { _supplier.Logo = value; OnPropertyChanged(); } }
        }

        public decimal? RatingReliability
        {
            get => _supplier.RatingReliability;
            set { if (_supplier.RatingReliability != value) { _supplier.RatingReliability = value; OnPropertyChanged(); } }
        }

        public Suppliers GetModel() => _supplier;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case nameof(INN):
                        var s = INN?.ToString() ?? "";
                        if (s.Length != 12) return "ИНН — ровно 12 цифр";
                        break;
                    case nameof(Phone):
                        // разрешаем +, цифры, скобки, дефис
                        if (!Regex.IsMatch(Phone ?? "",
                              @"^\+7\(\d{3}\)\d{3}-\d{2}-\d{2}$"))
                            return "Телефон: +7(000)000-00-00";
                        break;
                    case nameof(Email):
                        if (!Regex.IsMatch(Email ?? "",
                              @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                            return "Неверный e-mail";
                        break;
                    case nameof(RatingReliability):
                        if (RatingReliability < 0m || RatingReliability > 10m)
                            return "Рейтинг 0.00–10.00";
                        break;
                }
                return null;
            }
        }
        public string Error => null;
    }
}