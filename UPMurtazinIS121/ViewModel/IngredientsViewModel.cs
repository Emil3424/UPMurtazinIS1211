using Microsoft.Win32;
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
using Excel = Microsoft.Office.Interop.Excel;

namespace UPMurtazinIS121.ViewModel
{
    public class IngredientsViewModel : INotifyPropertyChanged
    {
        private readonly CoffeeDBMurtazinEntities2 _context;
        private IngredientModel _selectedIngredient;

        public ObservableCollection<IngredientModel> IngredientsList { get; } = [];
        private string _selectedFilterType;
        public ObservableCollection<string> IngredientTypes { get; } = [];
        public ObservableCollection<IngredientModel> FilteredIngredientsList { get; } = [];

        public ObservableCollection<string> MeasurementUnits { get; } =
        [
            "шт", "кг", "г", "л"
        ];
        public string SelectedFilterType
        {
            get => _selectedFilterType;
            set
            {
                _selectedFilterType = value;
                OnPropertyChanged();
                ApplyFilter();
            }
        }
        public IngredientModel SelectedIngredient
        {
            get => _selectedIngredient;
            set
            {
                _selectedIngredient = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand AddNewCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand ExportToExcelCommand { get; }

        public IngredientsViewModel()
        {
            _context = new CoffeeDBMurtazinEntities2();
            _context.Configuration.ProxyCreationEnabled = false;
            LoadData();

            SaveCommand = new RelayCommand(SaveChanges);
            AddNewCommand = new RelayCommand(AddNewIngredient);
            DeleteCommand = new RelayCommand(DeleteIngredient);
            ExportToExcelCommand = new RelayCommand(_ => ExportToExcel());

            SortAscCommand = new RelayCommand(_ => SortIngredients(true));
            SortDescCommand = new RelayCommand(_ => SortIngredients(false));
        }

        private void LoadData()
        {
            try
            {
                _context.Ingredients.Load();
                IngredientsList.Clear();
                IngredientTypes.Clear();
                IngredientTypes.Add("Все типы");

                foreach (var ingredient in _context.Ingredients.Local)
                {
                    var vm = new IngredientModel(ingredient);
                    IngredientsList.Add(vm);

                    if (!string.IsNullOrEmpty(vm.TypeIngredients))
                    {
                        if (!IngredientTypes.Contains(vm.TypeIngredients))
                            IngredientTypes.Add(vm.TypeIngredients);
                    }
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
            FilteredIngredientsList.Clear();

            if (string.IsNullOrEmpty(SelectedFilterType))
            {
                foreach (var item in IngredientsList)
                    FilteredIngredientsList.Add(item);
            }
            else if (SelectedFilterType == "Все типы")
            {
                foreach (var item in IngredientsList)
                    FilteredIngredientsList.Add(item);
            }
            else
            {
                foreach (var item in IngredientsList.Where(i => i.TypeIngredients == SelectedFilterType))
                    FilteredIngredientsList.Add(item);
            }
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

        private void AddNewIngredient(object parameter)
        {
            var newIngredient = new Ingredients
            {
                IngredientsName = "Новый ингредиент",
                TypeIngredients = "Тип",
                UnitOfMeasurement = "кг",
                KolichSklad = 0,
                MinimKolich = 0,
                KolichUpakovka = 1,
                CostForOne = 0,
                ExpirationDate = DateTime.Now.AddMonths(6)
            };
            var wrapper = new IngredientModel(newIngredient);
            IngredientsList.Add(wrapper);
            ApplyFilter(); // <---- Добавить
            SelectedIngredient = wrapper;
            _context.Ingredients.Add(newIngredient);
        }

        private void DeleteIngredient(object parameter)
        {
            if (SelectedIngredient == null) return;
            if (MessageBox.Show($"Удалить {SelectedIngredient.IngredientsName}?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question) != MessageBoxResult.Yes) return;
            try
            {
                var model = SelectedIngredient.GetModel();
                _context.Ingredients.Remove(model);
                IngredientsList.Remove(SelectedIngredient); // Удаляем из основной коллекции
                ApplyFilter(); // <---- Добавить
                _context.SaveChanges();
                SelectedIngredient = IngredientsList.FirstOrDefault();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public ICommand SortAscCommand { get; }
        public ICommand SortDescCommand { get; }
        private void SortIngredients(bool ascending)
        {
            var sorted = ascending
                ? [.. FilteredIngredientsList.OrderBy(s => s.IngredientsName)]
                : FilteredIngredientsList.OrderByDescending(s => s.IngredientsName).ToList();

            FilteredIngredientsList.Clear();
            foreach (var s in sorted)
                FilteredIngredientsList.Add(s);
        }
        private void ExportToExcel()
        {
            try
            {
                var dlg = new SaveFileDialog
                {
                    Filter = "Excel Workbook|*.xlsx",
                    FileName = "Ingredients.xlsx"
                };
                if (dlg.ShowDialog() != true)
                    return;

                string path = dlg.FileName;

                // Запускаем Excel
                var excelApp = new Excel.Application
                {
                    Visible = true
                };
                var workbook = excelApp.Workbooks.Add();
                var sheet = (Excel._Worksheet)workbook.Sheets[1];

                // Заголовки столбцов
                sheet.Cells[1, 1] = "Название";
                sheet.Cells[1, 2] = "Тип";
                sheet.Cells[1, 3] = "Ед. измерения";
                sheet.Cells[1, 4] = "В наличии";
                sheet.Cells[1, 5] = "Мин. кол-во";
                sheet.Cells[1, 6] = "Цена за ед.";
                sheet.Cells[1, 7] = "Мин. партия";

                for (int i = 0; i < FilteredIngredientsList.Count; i++)
                {
                    var ing = FilteredIngredientsList[i];
                    int row = i + 2;
                    sheet.Cells[row, 1] = ing.IngredientsName;
                    sheet.Cells[row, 2] = ing.TypeIngredients;
                    sheet.Cells[row, 3] = ing.UnitOfMeasurement;
                    sheet.Cells[row, 4] = ing.KolichSklad;
                    sheet.Cells[row, 5] = ing.MinimKolich;
                    sheet.Cells[row, 6] = ing.CostForOne;
                    sheet.Cells[row, 7] = ing.MinOrderCost;
                }

                workbook.SaveAs(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось экспортировать в Excel: {ex.Message}",
                                "Ошибка экспорта",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}