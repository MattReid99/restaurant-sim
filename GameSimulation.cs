using System;
using System.Collections.Generics;

public class GameSimulation {

  public static float MIN_PRICE_FACTOR = 1.25f;
  public static float MAX_PRICE_FACTOR = 7f;

  static int CUSTOMER_POOL_SIZE = 1000;
  static float BASE_POPULARITY = .008f;
  static int BASE_RENT = 650;
  static int RENT_PERIOD = 7;


  private Dictionary<FoodType, List<FoodItem>> menu;
  private Dictionary<string, SupplierTier> ingredientSuppliers;
  private Dictionary<string, int[]> ingredientQuantityAndFreshness;
  private Customer[] customers;


  public int Days;
  public float Balance;
  public float Popularity;
  public float Satisfaction;
  public bool HasLost;

  public Results CumulativeResults;
  public List<Results> DailyResults;

  private Random rand;

  public GameSimulation() {
    rand = new Random();
    menu = new Dictionary<FoodType, List<FoodItem>>();
    menu.Add(FoodType.SIDE, new List<FoodItem>());
    menu.Add(FoodType.DRINK, new List<FoodItem>());
    menu.Add(FoodType.BURGER, new List<FoodItem>());

    ingredientSuppliers = new Dictionary<string, SupplierTier>();
    ingredientQuantityAndFreshness = new Dictionary<string, int[]>();
    foreach (string ingredientName in Ingredient.ALL_INGREDIENTS.Keys()) {
      ingredientSuppliers.Add(ingredientName, SupplierTier.LOWEST);
      ingredientQuantityAndFreshness.Add(ingredientName, new int[2]);
    }

    customers = new Customer[CUSTOMER_POOL_SIZE];
    for (int i=0; i<CUSTOMER_POOL_SIZE; i++)
      customers[i] = new Customer();
    HasLost = false;
    DailyResults = new List<Results>();
    CumulativeResults = new Results();
  }

  bool PayRent() {
    this.Balance -= BASE_RENT;
    return Balance > 0;
  }

  public Results SimulateDay() {
    if (HasLost) {
      throw new Exception("Game has ended!");
    }

    int numCustomers = (int)(Popularity * CUSTOMER_POOL_SIZE * rand.Next(0.95f, 1.05f));
    float totalRevenue = 0f;
    float totalValue = 0f;
    float totalSatisfaction = 0f;
    int customersServed = 0;
    int customersRejected = 0;

    int randCustomer;
    CustomerOrder order;
    for (int i=0; i<numCustomers; i++) {
      randCustomer = rand.Next(0, customers.Count);
      if (CanFulfill(customers[randCustomer].GetOrder()) {
        order = customers[randCustomer].MakeOrder();
        totalRevenue += order.Cost;
        totalValue += order.Value;
        totalSatisfaction += order.Satisfaction;
        customersServed++;
      } else customersRejected++;
    }

    if (Days % RENT_PERIOD == 0 && !PayRent())
      HasLost = true;

    Results results = new Results
    {
      TotalRevenue = totalRevenue,
      CustomersServed = customersServed,
      CustomersRejected = customersRejected,
      AverageSatisfaction = totalSatisfaction / customersServed,
      Days = 1
    };

    // Update Popularity, Balance, Days, Satisfaction
    Days++;
    Balance += results.TotalRevenue;
    Satisfaction = ((Satisfaction * (Days - 1)) +
      (results.AverageSatisfaction)) / Days;
    // double in popularity if average satisfaction is maxed out
    Popularity += (Popularity * (results.AverageSatisfaction - .5);
    Popularity = Mathf.Max(Mathf.Min(1.0f, Popularity), BASE_POPULARITY);
    CheckForSpoiledIngredients();

    CumulativeResults.Append(results);
    DailyResults.Add(results);
    return results;
  }

  public bool AddFoodItem(FoodItem newItem) {
    if (allIngredients[newItem].Select(item => item.ItemName).Contains(newItem.ItemName))
      return false;

    newItem.Quality = CalculateQuality(newItem);
    menu[newItem.Type].Add(newItem);
    return true;
  }

  public float CalculateQuality(FoodItem item) {
    // each game generate a new seed to determine how well different ingredients mesh
    float quality = 0f;
    foreach (string ingred in item.Ingredients) {
        quality += Ingredient.ALL_INGREDIENTS[ingred].Appeal;
    }

    return quality /= item.Ingredients.Length;
  }

  // Returns a list containing names of all spoiled ingredients
  public List<string> CheckForSpoiledIngredients() {
    List<string> spoiled = new List<string>();

    foreach (string ingred in ingredientQuantityAndFreshness.Keys()) {
      if (ingred[1].Days < 0) {
        ingred[0] = 0;
        spoiled.Add(ingred);
      }
    }
    return spoiled;
  }

  public bool OrderIngredient(string ingredName, int quantity) {
    Ingredient ingred = Ingredient.ALL_INGREDIENTS[ingredName];
    float cost = ingred.Cost * quantity;
    if (Balance < cost)
      return false;

    ingredientQuantityAndFreshness[ingredName][0] += quantity;
    int prevFreshness = ingredientQuantityAndFreshness[ingredName][1];
    int prevQty = ingredientQuantityAndFreshness[ingredName][0];
    ingredientQuantityAndFreshness[ingredName][1] = ((prevQty * prevFreshness)
      + (ingred.FreshDays * quantity)) / (prevQty + quantity);

    Balance -= cost;
    return true;
  }

  public List<FoodItem> GetMenuItemsForCategory(FoodType foodType) {
    return menu[foodType];
  }

  public SupplierTier GetSupplierForIngredient(string ingredientName) {
    return ingredientSuppliers[ingredientName];
  }

  public GetQuantityAndFreshnessForIngredient(string ingredientName) {
    return ingredientQuantityAndFreshness[ingredientName];
  }
}
