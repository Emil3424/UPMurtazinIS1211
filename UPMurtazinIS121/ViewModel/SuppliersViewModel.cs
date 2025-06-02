using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using UPMurtazinIS121.Model;
using UPMurtazinIS121.Validations;

namespace UPMurtazinIS121.ViewModel
{
    public class SuppliersViewModel : INotifyPropertyChanged
    {
        private readonly CoffeeDBMurtazinEntities2 _context = new();
        private SupplierModel _selectedSupplier;
        private string _selectedFilterType;

        public ObservableCollection<SupplierModel> SuppliersList { get; } = [];
        public ObservableCollection<string> SupplierTypes { get; } = [];
        public ObservableCollection<SupplierModel> FilteredSuppliersList { get; } = [];
        public ICommand SortAscCommand { get; }
        public ICommand SortDescCommand { get; }
        public string SelectedFilterType
        {
            get => _selectedFilterType;
            set { _selectedFilterType = value; OnPropertyChanged(); ApplyFilter(); }
        }

        public SupplierModel SelectedSupplier
        {
            get => _selectedSupplier;
            set
            {
                _selectedSupplier = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand AddNewCommand { get; }
        public ICommand DeleteCommand { get; }


        public SuppliersViewModel()
        {
            _context.Configuration.ProxyCreationEnabled = false;
            LoadData();

            SaveCommand = new RelayCommand(SaveChanges);
            AddNewCommand = new RelayCommand(AddNewSupplier);
            DeleteCommand = new RelayCommand(DeleteSupplier);

            SortAscCommand = new RelayCommand(_ => SortSuppliers(true));
            SortDescCommand = new RelayCommand(_ => SortSuppliers(false));
        }
        private void SortSuppliers(bool ascending)
        {
            var sorted = ascending
                ? [.. FilteredSuppliersList.OrderBy(s => s.Name)]
                : FilteredSuppliersList.OrderByDescending(s => s.Name).ToList();

            FilteredSuppliersList.Clear();
            foreach (var s in sorted)
                FilteredSuppliersList.Add(s);
        }
        private void LoadData()
        {
            try
            {
                _context.Suppliers.Load();
                SuppliersList.Clear();
                SupplierTypes.Clear();
                SupplierTypes.Add("Все типы");

                foreach (var supplier in _context.Suppliers.Local)
                {
                    var vm = new SupplierModel(supplier);
                    SuppliersList.Add(vm);

                    if (!string.IsNullOrEmpty(vm.Type) && !SupplierTypes.Contains(vm.Type))
                        SupplierTypes.Add(vm.Type);
                }

                ApplyFilter();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки: {ex.Message}", " Ошибка");
            }
        }

        private void ApplyFilter()
        {
            FilteredSuppliersList.Clear();

            var items = SelectedFilterType switch
            {
                null or "Все типы" => SuppliersList,
                _ => SuppliersList.Where(i => i.Type == SelectedFilterType)
            };

            foreach (var item in items)
                FilteredSuppliersList.Add(item);
        }

        public bool HasChanges()
        {
            return _context.ChangeTracker.Entries()
                .Any(e => e.State != EntityState.Unchanged);
        }

        private void SaveChanges(object parameter)
        {
            try
            {
                if (HasChanges())
                {
                    _context.SaveChanges();
                    MessageBox.Show("Изменения сохранены!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddNewSupplier(object parameter)
        {
            var newSupplier = new Suppliers
            {
                Name = "Новый поставщик",
                Type = "Производитель",
                YuridAdres = "Адрес",
                INN = 0,
                FIO = "ФИО",
                Phone = "Телефон",
                Email = "email@example.com"
            };

            var wrapper = new SupplierModel(newSupplier);
            SuppliersList.Add(wrapper);
            SelectedSupplier = wrapper;
            _context.Suppliers.Add(newSupplier);

            if (!SupplierTypes.Contains(wrapper.Type))
                SupplierTypes.Add(wrapper.Type);
        }

        private void DeleteSupplier(object parameter)
        {
            if (SelectedSupplier == null) return;

            if (MessageBox.Show($"Удалить {SelectedSupplier.Name}?", "Подтверждение",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes) return;

            try
            {
                _context.Suppliers.Remove(SelectedSupplier.GetModel());
                SuppliersList.Remove(SelectedSupplier);
                _context.SaveChanges();
                SelectedSupplier = SuppliersList.FirstOrDefault();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}