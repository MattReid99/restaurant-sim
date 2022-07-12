using UnityEngine;

public class Customer {
  private string like;
  private string dislike;
  private string favoriteBurger;
  private float prevSatisfaction;
  private CustomerType customerType;

  static Dictionary<FoodType, float> FOOD_TYPE_WEIGHTS = new Dictionary<FootType, float>
  {
    {FoodType.BURGER, 1.55},
    {FoodType.DRINK, .65},
    {FoodType.SIDE, .8f}
  };

  public Customer(string[] ingredientNames) {
    customerType = (CustomerType) Random.Range(0, 5);

    bool hasLike = (Random.Range(0f, 1f) <= .3f);
    bool hasDislike = (Random.Range(0f, 1f) <= .3f);

    like = (hasLike) ? ingredientNames[Random.Range(0, ingredientNames.Length)] : null;
    dislike = (hasDislike) ? ingredientNames[Random.Range(0, ingredientNames.Length)] : null;

    while (like == dislike && (hasLike && hasDislike)) {
      dislike = ingredientNames[Random.Range(0, ingredientNames.Length)];
    }
  }

  public CustomerOrder MakeOrder(List<FoodItem> order,
    Dictionary<FoodType, float> pricing) {
      float satisfaction = 0f;
      float price = 0f;
      foreach (FoodItem item in order) {
        if (item.Ingredients.Contains(Like)) satisfaction += .1f;
        if (item.Ingredients.Contains(Dislike)) satisfaction -= .2f;
        satisfaction += (item.Quality * FOOD_TYPE_WEIGHTS[item.Type]);
        price += (item.Cost * pricing[item.Type]);
      }

      return new CustomerOrder() {
          Satisfaction = satisfaction,
          //todo
          Value = 1.0f,
          Cost = price;
      };
  }

  public List<FoodItem> GetOrder(Dictionary<FoodType, List<FoodItem>> menu) {
    List<FoodItem> orderItems = new List<FoodItem>();
    foreach (FoodType foodType in menu.Keys()) {
      if (menu[foodType].Count > 0) {
        List<FoodItem> filtered = (Dislike != null) ? menu[FoodType].Where(!item.Ingredients.Contains(Dislike)).ToList()
          : menu[FoodType];
        if (filtered.Count > 0) {
          List<FoodItem> likes = filtered.Where(item => item.Ingredients.Contains(Like)).ToList();
          if (likes.Count > 0) orderItems.Add(likes[Random.Range(0, likes.Count)]);
          else orderItems.Add(filtered[Random.Range(0, filtered.Count)]);
        } else
          orderItems.Add(menu[foodType][Random.Range(0, menu[foodType].Count)]);
      }
    }
    return orderItems;
  }
}

public enum CustomerType {
  GRUBBER,
  VALUE,
  HEALTHY,
  ADVENTUROUS,
  NORMAL
}

public struct CustomerOrder {
  public float Cost;
  public float Value;
  public float Satisfaction;
}
