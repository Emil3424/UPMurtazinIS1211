using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using UPMurtazinIS121.Model;
using UPMurtazinIS121.ViewModel;
using Xunit;

namespace CoffeeTest
{
    public class IngredientManagerTests
    {
        [Fact]
        public void AddNewIngredient_ShouldAddToContext()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<CoffeeDBMurtazinEntities2>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new CoffeeDBMurtazinEntities2(options);
            var viewModel = new IngredientsViewModel(context);

            // Act
            viewModel.AddNewIngredient();

            // Assert
            Assert.Single(context.Ingredients);
            Assert.Equal("Новый ингредиент", context.Ingredients.First().IngredientsName);
        }

        [Fact]
        public void DeleteIngredient_ShouldRemoveFromContext()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<CoffeeDBMurtazinEntities2>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new CoffeeDBMurtazinEntities2(options);
            context.Ingredients.Add(new Ingredients { IngredientsName = "Тест" });
            context.SaveChanges();

            var viewModel = new IngredientsViewModel(context);
            viewModel.SelectedIngredient = viewModel.IngredientsList.First();

            // Act
            viewModel.DeleteIngredient();

            // Assert
            Assert.Empty(context.Ingredients);
        }

        [Fact]
        public void LoadData_ShouldPopulateList()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<CoffeeDBMurtazinEntities2>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new CoffeeDBMurtazinEntities2(options);
            context.Ingredients.AddRange(
                new Ingredients { IngredientsName = "Кофе" },
                new Ingredients { IngredientsName = "Молоко" }
            );
            context.SaveChanges();

            var viewModel = new IngredientsViewModel(context);

            // Act
            viewModel.LoadData();

            // Assert
            Assert.Equal(2, viewModel.IngredientsList.Count);
            Assert.Contains(viewModel.IngredientsList, i => i.IngredientsName == "Кофе");
        }
    }
}