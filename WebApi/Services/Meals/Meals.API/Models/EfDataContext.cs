using System;
using System.IO;
using Meals.API.Domains;
using Microsoft.EntityFrameworkCore;

namespace Meals.API.Models
{
    public class EfDataContext : DbContext
    {
        private const string DatabaseName = "dbEFMeals.db";

        #region Constructors

        public EfDataContext()
        {
            Init();
        }

        public EfDataContext(DbContextOptions options)
            : base(options)
        {
            Init();
        }

        #endregion

        #region Public Propeties

        public DbSet<Meal> Meals { get; set; }
        public DbSet<Food> Foods { get; set; }

        #endregion

        #region Public Methods

        public override void Dispose()
        {
            base.Dispose();

            Meals = null;
            Foods = null;

            GC.SuppressFinalize(this);
        }

        #endregion

        #region Overridden Methods

        /// <summary>
        /// Mapping of foreign keys
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
                return;

            // relationship between Meal and MealFoods
            modelBuilder.Entity<Meal>()
                        .HasMany(s => s.Foods)
                        .WithOne(m => m.Meal)
                        .HasForeignKey(k => k.MealId)
                        .OnDelete(DeleteBehavior.Cascade);

            // relationship between MealFoods and Foods
            modelBuilder.Entity<MealFood>()
                        .HasOne(s => s.Food)
                        .WithMany(m => m.MealFoods)
                        .HasForeignKey(k => k.FoodId)
                        .OnDelete(DeleteBehavior.Cascade);
        }

        #endregion

        #region Private Methods

        private void Init()
        {
            Database.EnsureCreated();
        }

        #endregion

        #region OnConfiguring

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder != null && !optionsBuilder.IsConfigured)
            {
                // create data directory to store database file if not exist
                var dataDirPath = Path.Combine(Directory.GetCurrentDirectory(), "Data");
                Directory.CreateDirectory(dataDirPath);

                // Specify that we will use sqlite and the path of the database here
                optionsBuilder.UseSqlite($"Filename={Path.Combine(dataDirPath, DatabaseName)}");
            }
        }

        #endregion
    }
}
