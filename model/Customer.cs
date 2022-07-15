using System;
using System.Collections.Generic;
using System.Linq;

public class Customer {
  private string like;
  private string dislike;
  private string favoriteBurger;
  private float prevSatisfaction;
  private CustomerType customerType;

  static Dictionary<FoodType, float> FOOD_TYPE_WEIGHTS = new Dictionary<FoodType, float>
  {
    {FoodType.BURGER, 1.55f},
    {FoodType.DRINK, .65f},
    {FoodType.SIDE, .8f}
  };

  public Customer(Random rand, string[] ingredientNames) {
    customerType = (CustomerType) rand.Next(0, 5);

    bool hasLike = (ExtensionMethods.GetRandomNumber(0.0, 1.0) <= .3);
    bool hasDislike = (ExtensionMethods.GetRandomNumber(0.0, 1.0) <= .3);

    like = (hasLike) ? ingredientNames[rand.Next(0, ingredientNames.Length)] : null;
    dislike = (hasDislike) ? ingredientNames[rand.Next(0, ingredientNames.Length)] : null;

    while (like == dislike && (hasLike && hasDislike)) {
      dislike = ingredientNames[rand.Next(0, ingredientNames.Length)];
    }
  }

  public CustomerOrder MakeOrder(List<FoodItem> order,
    Dictionary<FoodType, float> pricing) {
      float satisfaction = 0f;
      float price = 0f;
      foreach (FoodItem item in order) {
        if (Array.IndexOf(item.Ingredients, like) != -1) satisfaction += .1f;
        if (Array.IndexOf(item.Ingredients, dislike) != -1) satisfaction -= .2f;
        satisfaction += (item.Quality * FOOD_TYPE_WEIGHTS[item.Type]);
        satisfaction -= ((pricing[item.Type] - ((GameSimulation.MIN_PRICE_FACTOR 
          + GameSimulation.MAX_PRICE_FACTOR) / 2)) * (.05f * FOOD_TYPE_WEIGHTS[item.Type]));
        price += (item.Cost * pricing[item.Type]);
      }

      return new CustomerOrder() {
          Satisfaction = satisfaction,
          //todo
          Value = 1.0f,
          Cost = price
      };
  }

  public List<FoodItem> GetOrder(Random rand, Dictionary<FoodType, List<FoodItem>> menu) {
    List<FoodItem> orderItems = new List<FoodItem>();
    foreach (FoodType foodType in menu.Keys) {
      if (menu[foodType].Count > 0) {
        List<FoodItem> filtered = (dislike != null) ? menu[foodType].Where(item => !item.Ingredients.Contains(dislike)).ToList()
          : menu[foodType];
        if (filtered.Count > 0) {
          List<FoodItem> likes = filtered.Where(item => item.Ingredients.Contains(like)).ToList();
          if (likes.Count > 0) orderItems.Add(likes[rand.Next(0, likes.Count)]);
          else orderItems.Add(filtered[rand.Next(0, filtered.Count)]);
        } else
          orderItems.Add(menu[foodType][rand.Next(0, menu[foodType].Count)]);
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
