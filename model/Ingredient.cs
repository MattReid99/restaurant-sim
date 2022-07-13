using System.Collections.Generics;

public class Ingredient {
  public string IngredientName;
  public IngredientType Type;
  public float Cost;
  // ingredients have a 0 appeal if customers don't factor them into their purchase decision
  public float Appeal;
  public int Calories;
  public int FreshDays;

  public static Dictionary<string, Ingredient> ALL_INGREDIENTS = new Dictionary
  {
    {"Beef Patty", new Ingredient { IngredientName = "Beef Patty",
      Type = IngredientType.PROTEIN, Cost = 1.1f, Appeal = .7f, Calories = 250, FreshDays = 7 } },
    {"White Bun", new Ingredient { IngredientName = "White Bun", Type = IngredientType.BUN,
      Cost = .3f, Appeal = .7f, Calories = 150, FreshDays = 10 } },
    {"Lettuce", new Ingredient { IngredientName = "Lettuce", Type = IngredientType.VEGETABLE,
      Cost = .1f, Appeal = .4f, Calories = 5, FreshDays = 5 } },
    {"Cheddar Cheese", new Ingredient { IngredientName = "Cheddar Cheese",
      Type = IngredientType.CHEESE, Cost = .15f, Appeal = .2f, Calories = 25, FreshDays = 14 } },
    {"Coke", new Ingredient { IngredientName = "Coke", Type = IngredientType.DRINK,
      Cost = .3f, Appeal = .7f, Calories = 150, FreshDays = 100 } },
    {"Pepsi", new Ingredient { IngredientName = "Pepsi", Type = IngredientType.DRINK,
      Cost = .1f, Appeal = .4f, Calories = 5, FreshDays = 100 } },
    {"Potatoes", new Ingredient { IngredientName = "Potatoes", Type = IngredientType.POTATOES,
      Cost = .35f, Appeal = .85f, Calories = 25, FreshDays = 20 },
    {"Frying Oil", new Ingredient { IngredientName = "Frying Oil", Type = IngredientType.FATTY,
      Cost = .001f, Appeal = 0f, Calories = 5, FreshDays = 100 } },
    {"Salt", new Ingredient { IngredientName = "Salt", Type = IngredientType.FATTY,
      Cost = .001f, Appeal = 0f, Calories = 0, FreshDays = 500 } }
  };
}
