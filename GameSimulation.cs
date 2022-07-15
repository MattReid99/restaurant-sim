using System;
using System.Collections.Generic;
using System.Linq;

public class GameSimulation {

  public static float MIN_PRICE_FACTOR = 1.25f;
  public static float MAX_PRICE_FACTOR = 7f;

  static int CUSTOMER_POOL_SIZE = 1000;
  static float BASE_POPULARITY = .008f;
  static int BASE_RENT = 400;
  static int RENT_PERIOD = 7;

  static int START_BALANCE = 2500;


  private Dictionary<FoodType, List<FoodItem>> menu;
  private Dictionary<string, SupplierTier> ingredientSuppliers;
  private Dictionary<string, int[]> ingredientQuantityAndFreshness;
  private Customer[] customers;
  private Dictionary<FoodType, float> pricing;


  public int Days;
  public float Balance;
  public float Popularity;
  public float Satisfaction;
  public bool HasLost;

  public Results CumulativeResults;
  public List<Results> DailyResults;

  private Random rand;

  public GameSimulation() {
    Balance = START_BALANCE;
    Popularity = BASE_POPULARITY;

    rand = new Random();
    menu = new Dictionary<FoodType, List<FoodItem>>();
    menu.Add(FoodType.SIDE, new List<FoodItem>());
    menu.Add(FoodType.DRINK, new List<FoodItem>());
    menu.Add(FoodType.BURGER, new List<FoodItem>());

    pricing = new Dictionary<FoodType, float>();
    pricing.Add(FoodType.SIDE, MIN_PRICE_FACTOR);
    pricing.Add(FoodType.DRINK, MIN_PRICE_FACTOR);
    pricing.Add(FoodType.BURGER, MIN_PRICE_FACTOR);   

    ingredientSuppliers = new Dictionary<string, SupplierTier>();
    ingredientQuantityAndFreshness = new Dictionary<string, int[]>();
    foreach (string ingredientName in Ingredient.ALL_INGREDIENTS.Keys) {
      ingredientSuppliers.Add(ingredientName, SupplierTier.LOWEST);
      ingredientQuantityAndFreshness.Add(ingredientName, new int[2]);
    }

    customers = new Customer[CUSTOMER_POOL_SIZE];
    var keys = Ingredient.ALL_INGREDIENTS.Keys.ToArray();
    for (int i=0; i<CUSTOMER_POOL_SIZE; i++)
      customers[i] = new Customer(rand, keys);
    HasLost = false;
    DailyResults = new List<Results>();
    CumulativeResults = new Results();

    UpdateCosts();
  }

  bool PayRent() {
    this.Balance -= BASE_RENT;
    return Balance > 0;
  }

  public Results SimulateDay() {
    if (HasLost) {
      throw new Exception("Game has ended!");
    }

    int numCustomers = (int)(Popularity * CUSTOMER_POOL_SIZE * ExtensionMethods.GetRandomNumber(0.95, 1.05));
    float totalRevenue = 0f;
    float totalValue = 0f;
    float totalSatisfaction = 0f;
    int customersServed = 0;
    int customersRejected = 0;

    int randCustomer;
    CustomerOrder order;
    List<FoodItem> orderedItems;

    for (int i=0; i<numCustomers; i++) {
      randCustomer = rand.Next(0, customers.Length);
      orderedItems = customers[randCustomer].GetOrder(rand, menu);
      if (CanFulfill(orderedItems)) {
        order = customers[randCustomer].MakeOrder(orderedItems, pricing);
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
      AverageSatisfaction = totalSatisfaction / (customersServed + 1),
      Popularity = Popularity,
      Days = 1
    };

    // Update Popularity, Balance, Days, Satisfaction
    Days++;
    Balance += results.TotalRevenue;
    Satisfaction = ((Satisfaction * (Days - 1)) +
      (results.AverageSatisfaction)) / Days;
    // double in popularity if average satisfaction is maxed out
    Popularity *= (1.0f + ((.5f * (results.AverageSatisfaction - .35f)) / (float)Math.Sqrt(Days)));
    //Popularity = Math.Max(Math.Min(1.0f, Popularity), BASE_POPULARITY);
    CheckForSpoiledIngredients();

    CumulativeResults.Append(results);
    DailyResults.Add(results);
    return results;
  }

  bool CanFulfill(List<FoodItem> items) {

    foreach (FoodItem item in items) { 
      foreach (string ingredientName in item.Ingredients)
        if (ingredientQuantityAndFreshness[ingredientName][0] > 0)
          ingredientQuantityAndFreshness[ingredientName][0]--;
        else return false;
    }
    return true;
   }

   void UpdateCosts() {
     foreach (FoodType foodType in menu.Keys)
      UpdateCosts(foodType);
   }

   void UpdateCosts(FoodType foodType) {
     for (int i=0; i<menu[foodType].Count; i++)
      menu[foodType][0].UpdateCost(pricing[foodType]);
   }

  public bool AddFoodItem(FoodItem newItem) {
    if (menu[newItem.Type].Select(item => item.ItemName).Contains(newItem.ItemName))
      return false;

    newItem.Quality = CalculateQuality(newItem);
    menu[newItem.Type].Add(newItem);
    newItem.UpdateCost(pricing[newItem.Type]);
    return true;
  }

  public float CalculateQuality(FoodItem item) {
    // each game generate a new seed to determine how well different ingredients mesh
    float quality = 0f;
    foreach (string ingred in item.Ingredients) {
        quality += Ingredient.ALL_INGREDIENTS[ingred].Appeal;
    }

    return quality / item.Ingredients.Length;
  }

  // Returns a list containing names of all spoiled ingredients
  public List<string> CheckForSpoiledIngredients() {
    List<string> spoiled = new List<string>();

    foreach (string ingred in ingredientQuantityAndFreshness.Keys) {
      if (ingredientQuantityAndFreshness[ingred][1] < 0) {
        ingredientQuantityAndFreshness[ingred][0] = 0;
        ingredientQuantityAndFreshness[ingred][1] = 0;
        spoiled.Add(ingred);
      }
    }
    return spoiled;
  }

  public Dictionary<string, int[]> CheckInventory() {
    return ingredientQuantityAndFreshness;
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

  public bool SetPricingFactorForType(float newPrice, FoodType foodType) {
    if (newPrice < MIN_PRICE_FACTOR || newPrice > MAX_PRICE_FACTOR) return false;

    pricing[foodType] = newPrice;
    UpdateCosts(foodType);
    return true;
  }

  public SupplierTier GetSupplierForIngredient(string ingredientName) {
    return ingredientSuppliers[ingredientName];
  }

  public int[] GetQuantityAndFreshnessForIngredient(string ingredientName) {
    return ingredientQuantityAndFreshness[ingredientName];
  }
}
