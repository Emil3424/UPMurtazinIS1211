﻿//------------------------------------------------------------------------------
// <auto-generated>
//    Этот код был создан из шаблона.
//
//    Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//    Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UPMurtazinIS121.Model
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class CoffeeDBMurtazinEntities2 : DbContext
    {
        public CoffeeDBMurtazinEntities2()
            : base("name=CoffeeDBMurtazinEntities2")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Employees> Employees { get; set; }
        public DbSet<Ingredients> Ingredients { get; set; }
        public DbSet<IngredientSupplier> IngredientSupplier { get; set; }
        public DbSet<InventoryAlerts> InventoryAlerts { get; set; }
        public DbSet<InventoryTransactions> InventoryTransactions { get; set; }
        public DbSet<MenuCategories> MenuCategories { get; set; }
        public DbSet<MenuItems> MenuItems { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<OrderStatuses> OrderStatuses { get; set; }
        public DbSet<PositionsEmployee> PositionsEmployee { get; set; }
        public DbSet<Recipes> Recipes { get; set; }
        public DbSet<Suppliers> Suppliers { get; set; }
        public DbSet<sysdiagrams> sysdiagrams { get; set; }
        public DbSet<TransactionType> TransactionType { get; set; }
        public DbSet<TypeIngredients> TypeIngredients { get; set; }
        public DbSet<TypeSupplier> TypeSupplier { get; set; }
        public DbSet<UnitOfMeasurement> UnitOfMeasurement { get; set; }
        public DbSet<WorkSchedules> WorkSchedules { get; set; }
    }
}
