using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Meals.API.Domains
{
    public enum MealType
    {
        Breakfast,
        Launch,
        Dinner,
        Snack
    }

    public class Meal : IEquatable<Meal>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public uint Id { get; set; }
        public MealType Type { get; set; }
        public DateTime DateTime { get; set; }

        [NotMapped]
        public List<MealFood> Foods { get; set; }

        #region Constructor

        public Meal()
        {
            Foods = new List<MealFood>();
        }

        #endregion

        #region Overridden Methods

        public override bool Equals(object obj)
        {
            if (obj == null || !ReferenceEquals(obj, this))
            {
                return false;
            }

            return Equals(obj as Meal);
        }

        public bool Equals([AllowNull] Meal meal)
        {
            if (meal != null)
            {
                bool result = true;
                result &= Id == meal.Id;
                result &= Type == meal.Type;
                result &= DateTime == meal.DateTime;

                for (int i = 0; i < Foods.Count; i++)
                {
                    result &= Foods[i].Equals(meal.Foods[i]);
                }

                return result;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (Id, Type, DateTime, Foods).GetHashCode();
        }

        #endregion
    }
}
