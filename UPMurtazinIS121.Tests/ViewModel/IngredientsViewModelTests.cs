using UPMurtazinIS121.Model;
using UPMurtazinIS121.ViewModel;
using Xunit;

namespace UPMurtazinIS121.Tests.ViewModel
{
    public class IngredientsViewModelTests
    {
        // 1) Тест “вывода” ингредиентов, не null
        [Fact]
        public void IngredientsList_Initially_IsNotNull()
        {
            var vm = new IngredientsViewModel();

            Assert.NotNull(vm.IngredientsList);
        }

        // 2) Тест добавления ингредиента: добавляем и проверяем рост Count
        [Fact]
        public void IngredientsList_Add_NewIngredient_IncreasesCount()
        {
            var vm = new IngredientsViewModel();
            int before = vm.IngredientsList.Count;

            var dummy = new IngredientModel(new Ingredients
            {
                IngredientsName = "Тестовый",
                TypeIngredients = "T",
                UnitOfMeasurement = "шт"
            });

            vm.IngredientsList.Add(dummy);

            Assert.Equal(before + 1, vm.IngredientsList.Count);
            Assert.Contains(dummy, vm.IngredientsList);
        }

        // 3) Тест удаления ингредиента: удаляем и проверяем уменьшение Count
        [Fact]
        public void IngredientsList_Remove_ExistingIngredient_DecreasesCount()
        {
            var vm = new IngredientsViewModel();
            var dummy = new IngredientModel(new Ingredients
            {
                IngredientsName = "Удалить",
                TypeIngredients = "D",
                UnitOfMeasurement = "шт"
            });
            vm.IngredientsList.Add(dummy);
            int before = vm.IngredientsList.Count;

            bool removed = vm.IngredientsList.Remove(dummy);

            Assert.True(removed, "Элемент должен быть удалён");
            Assert.Equal(before - 1, vm.IngredientsList.Count);
            Assert.DoesNotContain(dummy, vm.IngredientsList);
        }
    }
}