using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Meals.API.Domains
{
    public enum MeasureUnit
    {
        gram,
        liter
    }

    public class MealFood : IEquatable<MealFood>
    {
        public uint Id { get; set; }

        public double Quantity { get; set; }
        public MeasureUnit Unit { get; set; }

        // Foreign Key mapping to Food entity (one-to-many)
        [NotMapped]
        public uint FoodId { get; set; }
        [NotMapped]
        public Food Food { get; set; }

        // Foreign Key mapping to Meal entity (one-to-many)
        [NotMapped]
        public uint MealId { get; set; }
        [NotMapped]
        public Meal Meal { get; set; }

        #region Overridden Methods

        public override bool Equals(object obj)
        {
            if (obj == null || !ReferenceEquals(obj, this))
            {
                return false;
            }

            return Equals(obj as MealFood);
        }

        public bool Equals([AllowNull] MealFood mealFood)
        {
            if (mealFood != null)
            {
                bool result = true;
                result &= Id == mealFood.Id;
                result &= (Quantity == mealFood.Quantity);
                result &= (Unit == mealFood.Unit);
                result &= (FoodId == mealFood.FoodId);
                result &= ((Food == null && mealFood.Food == null) || Food.Equals(mealFood.Food));
                result &= (MealId == mealFood.MealId);
                result &= ((Meal == null && mealFood.Meal == null) || Meal.Equals(mealFood.Meal));

                return result;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (Id, Quantity, Unit, FoodId, MealId, Meal).GetHashCode();
        }

        #endregion
    }
}
