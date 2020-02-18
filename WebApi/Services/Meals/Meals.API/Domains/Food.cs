using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Meals.API.Domains
{
    public class Food : IEquatable<Food>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public uint Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [NotMapped]
        public List<MealFood> MealFoods { get; set; }

        public Food()
        {
            MealFoods = new List<MealFood>();
        }

        #region Overridden Methods

        public override bool Equals(object obj)
        {
            if (obj == null || !ReferenceEquals(obj, this))
            {
                return false;
            }

            return Equals(obj as Food);
        }

        public bool Equals([AllowNull] Food food)
        {
            if (food != null)
            {
                bool result = true;
                result &= (Id == food.Id);
                result &= string.Compare(Name, food.Name, StringComparison.Ordinal) == 0;
                result &= string.Compare(Description, food.Description, StringComparison.Ordinal) == 0;

                return result;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (Id, Name, Description).GetHashCode();
        }

        #endregion
    }
}
