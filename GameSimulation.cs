using System.Collections.Generics;
using UnityEngine;

public class GameSimulation {

  public static float MIN_PRICE_FACTOR = 1.25f;
  public static float MAX_PRICE_FACTOR = 7f;

  static int CUSTOMER_POOL_SIZE = 1000;
  static float BASE_POPULARITY = .008f;
  static int BASE_RENT = 650;
  static int RENT_PERIOD = 7;

  private Dictionary<FoodType, List<FoodItem>> menu;
  private Dictionary<string, SupplierTier> ingredientSuppliers;
  private Customer[] customers;

  public int Days;
  public float Balance;
  public float Popularity;
  public float Satisfaction;

  public GameSimulation() {
    menu = new Dictionary<FoodType, List<FoodItem>>();
    menu.Add(FoodType.SIDE, new List<FoodItem>());
    menu.Add(FoodType.DRINK, new List<FoodItem>());
    menu.Add(FoodType.BURGER, new List<FoodItem>());

    customers = new Customer[CUSTOMER_POOL_SIZE];
    for (int i=0; i<CUSTOMER_POOL_SIZE; i++)
      customers[i] = new Customer();
  }

  public bool PayRent() {
    this.Balance -= BASE_RENT;
    return Balance > 0;
  }

  public Results SimulateDay() {
    Days++;

    int numCustomers = (int)(Popularity * CUSTOMER_POOL_SIZE * Random.Range(0.95f, 1.05f));
    float totalRevenue = 0f;
    float totalValue = 0f;
    float totalSatisfaction = 0f;

    int rand;
    CustomerOrder order;
    for (int i=0; i<numCustomers; i++) {
      rand = Random.Range(0, customers.Count);
      if (CanFulfill(customers[rand].GetOrder()) {
        order = customers[rand].MakeOrder();
        totalRevenue += order.Cost;
        totalValue += order.Value;
        totalSatisfaction += order.Satisfaction;
      }
    }

    if (Days % RENT_PERIOD == 0 && !PayRent())
      LoseGame();
    return true;
  }

  void LoseGame() {
    
  }


  public bool AddFoodItem(FoodItem newItem) {
    if (allIngredients[newItem].Select(item => item.ItemName).Contains(newItem.ItemName)) return false;

    newItem.Quality = CalculateQuality(newItem);
    menu[newItem.Type].Add(newItem);
    return true;
  }

  public float CalculateQuality(FoodItem item) {
    return 0f;
  }

  public Results GetCumulativeStats() {
    return cumulativeResults;
  }


}
