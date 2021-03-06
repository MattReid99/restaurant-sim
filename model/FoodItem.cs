using System.Linq;

public class FoodItem {
  public string ItemName;
  public FoodType Type;
  public string[] Ingredients;
  public float Quality;
  public float Cost;

  public FoodItem(string itemName, Ingredient[] ingredients, FoodType foodType) {
    this.ItemName = itemName;
    this.Ingredients = ingredients.Select(ingred => ingred.IngredientName).ToArray();
  }

  public void UpdateCost(float pricingFactor) {
    this.Cost = 0f;
    foreach (string ingred in Ingredients) {
      Cost += (Ingredient.ALL_INGREDIENTS[ingred].Cost * pricingFactor);
    }
  } 
}
