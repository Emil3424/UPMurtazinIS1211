using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace UPMurtazinIS121.Model
{
    public class IngredientModel(Ingredients ingredient) : INotifyPropertyChanged
    {
        private readonly Ingredients _ingredient = ingredient;

        public int IngredientID
        {
            get => _ingredient.IngredientID;
            set
            {
                if (_ingredient.IngredientID != value)
                {
                    _ingredient.IngredientID = value;
                    OnPropertyChanged();
                }
            }
        }

        public string IngredientsName
        {
            get => _ingredient.IngredientsName;
            set
            {
                if (_ingredient.IngredientsName != value)
                {
                    _ingredient.IngredientsName = value;
                    OnPropertyChanged();
                }
            }
        }

        public string TypeIngredients
        {
            get => _ingredient.TypeIngredients;
            set
            {
                if (_ingredient.TypeIngredients != value)
                {
                    _ingredient.TypeIngredients = value;
                    OnPropertyChanged();
                }
            }
        }

        public string UnitOfMeasurement
        {
            get => _ingredient.UnitOfMeasurement;
            set
            {
                if (_ingredient.UnitOfMeasurement != value)
                {
                    _ingredient.UnitOfMeasurement = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal? KolichSklad
        {
            get => _ingredient.KolichSklad;
            set
            {
                if (_ingredient.KolichSklad != value)
                {
                    _ingredient.KolichSklad = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(MinOrderCost));
                }
            }
        }

        public int? MinimKolich
        {
            get => _ingredient.MinimKolich;
            set
            {
                if (_ingredient.MinimKolich != value)
                {
                    _ingredient.MinimKolich = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(MinOrderCost));
                }
            }
        }

        public int? KolichUpakovka
        {
            get => _ingredient.KolichUpakovka;
            set
            {
                if (_ingredient.KolichUpakovka != value)
                {
                    _ingredient.KolichUpakovka = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(MinOrderCost));
                }
            }
        }

        public int? CostForOne
        {
            get => _ingredient.CostForOne;
            set
            {
                if (_ingredient.CostForOne != value)
                {
                    _ingredient.CostForOne = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(MinOrderCost));
                }
            }
        }

        public string Description
        {
            get => _ingredient.Description;
            set
            {
                if (_ingredient.Description != value)
                {
                    _ingredient.Description = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime? ExpirationDate
        {
            get => _ingredient.ExpirationDate;
            set
            {
                if (_ingredient.ExpirationDate != value)
                {
                    _ingredient.ExpirationDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal MinOrderCost
        {
            get
            {
                if (KolichSklad >= MinimKolich ||
                    !KolichSklad.HasValue ||
                    !MinimKolich.HasValue ||
                    !KolichUpakovka.HasValue ||
                    !CostForOne.HasValue)
                {
                    return 0.00m;
                }

                decimal deficit = (decimal)(MinimKolich - KolichSklad);
                int packages = (int)System.Math.Ceiling(deficit / (decimal)KolichUpakovka);
                return packages * (decimal)KolichUpakovka * (decimal)CostForOne;
            }
        }

        public Ingredients GetModel() => _ingredient;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case nameof(IngredientsName):
                        if (string.IsNullOrWhiteSpace(IngredientsName))
                            return "Название обязательно";
                        break;
                    case nameof(KolichSklad):
                        if (KolichSklad < 0) return "Не отрицательное";
                        break;
                    case nameof(MinimKolich):
                        if (MinimKolich < 0) return "Не отрицательное";
                        break;
                    case nameof(CostForOne):
                        if (CostForOne < 0) return "Не отрицательное";
                        break;
                }
                return null;
            }
        }
    }
}