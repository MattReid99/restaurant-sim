    using System;
    using System.Collections.Generic;

    class TestRunner
    {
        static int MAX_DAYS = 300;

        static List<FoodItem> basicFoodItems = new List<FoodItem>
        {
            { new FoodItem("Coke", new Ingredient[] { Ingredient.ALL_INGREDIENTS["Coke"] }, FoodType.DRINK) },
            { new FoodItem("Pepsi", new Ingredient[] { Ingredient.ALL_INGREDIENTS["Pepsi"] }, FoodType.DRINK) },
            { new FoodItem("French Fries", new Ingredient[] { Ingredient.ALL_INGREDIENTS["Potatoes"],
                Ingredient.ALL_INGREDIENTS["Frying Oil"], Ingredient.ALL_INGREDIENTS["Salt"] }, FoodType.SIDE) },
            { new FoodItem("Basic Burger", new Ingredient[] { Ingredient.ALL_INGREDIENTS["White Bun"],
                Ingredient.ALL_INGREDIENTS["Beef Patty"] }, FoodType.BURGER) }
        };

        static List<FoodItem> advancedFoodItems = new List<FoodItem>
        {
            { new FoodItem("Shroom Burger", new Ingredient[] { Ingredient.ALL_INGREDIENTS["White Bun"],
                Ingredient.ALL_INGREDIENTS["Beef Patty"], Ingredient.ALL_INGREDIENTS["Mushrooms"], 
                Ingredient.ALL_INGREDIENTS["BBQ Sauce"] }, FoodType.BURGER) },
            { new FoodItem("Pepsi", new Ingredient[] { Ingredient.ALL_INGREDIENTS["Pepsi"] }, FoodType.DRINK) },
            { new FoodItem("French Fries", new Ingredient[] { Ingredient.ALL_INGREDIENTS["Potatoes"],
                Ingredient.ALL_INGREDIENTS["Frying Oil"], Ingredient.ALL_INGREDIENTS["Salt"] }, FoodType.SIDE) },
            { new FoodItem("Basic Burger", new Ingredient[] { Ingredient.ALL_INGREDIENTS["White Bun"],
                Ingredient.ALL_INGREDIENTS["Beef Patty"] }, FoodType.BURGER) }
        };

        static Dictionary<FoodType, float> minPricing = new Dictionary<FoodType, float>
        {
            { FoodType.SIDE, GameSimulation.MIN_PRICE_FACTOR },
            { FoodType.BURGER, GameSimulation.MIN_PRICE_FACTOR },
            { FoodType.DRINK, GameSimulation.MIN_PRICE_FACTOR },
        };

        static float avgPrice = ((GameSimulation.MIN_PRICE_FACTOR + GameSimulation.MAX_PRICE_FACTOR) / 2);
        static Dictionary<FoodType, float> avgPricing = new Dictionary<FoodType, float>
        {
            { FoodType.SIDE, avgPrice },
            { FoodType.BURGER, avgPrice },
            { FoodType.DRINK, avgPrice },
        };
        
        static Dictionary<FoodType, float> maxPricing = new Dictionary<FoodType, float>
        {
            { FoodType.SIDE, GameSimulation.MAX_PRICE_FACTOR },
            { FoodType.BURGER, GameSimulation.MAX_PRICE_FACTOR },
            { FoodType.DRINK, GameSimulation.MAX_PRICE_FACTOR },
        };

        static void Main(string[] args)
        {
            Dictionary<string, Results> results = new Dictionary<string, Results>();
            results.Add("Basic Menu - Min Pricing", ExecuteSimulation(basicFoodItems, minPricing));
            results.Add("Basic Menu - Avg Pricing", ExecuteSimulation(basicFoodItems, avgPricing));
            results.Add("Basic Menu - Max Pricing", ExecuteSimulation(basicFoodItems, maxPricing));

            basicFoodItems.AddRange(advancedFoodItems);
            results.Add("Advanced Menu - Min Pricing", ExecuteSimulation(basicFoodItems, minPricing));
            results.Add("Advanced Menu - Avg Pricing", ExecuteSimulation(basicFoodItems, avgPricing));
            results.Add("Advanced Menu - Max Pricing", ExecuteSimulation(basicFoodItems, maxPricing));
           

            foreach (string simName in results.Keys) {
                Console.WriteLine($"======= {simName} =======");
                Console.WriteLine(results[simName]);
                Console.WriteLine("\n\n");
            }
        }


        public static Results ExecuteSimulation(List<FoodItem> menu, Dictionary<FoodType, float> pricing) {
            GameSimulation gameSimulation = new GameSimulation();
            foreach (FoodItem item in menu)
                gameSimulation.AddFoodItem(item);

            foreach (FoodType foodType in pricing.Keys)
                gameSimulation.SetPricingFactorForType(pricing[foodType], foodType);
 
            Dictionary<string, int[]> inventory;
            while (!gameSimulation.HasLost && gameSimulation.Days < MAX_DAYS) {
                    inventory = gameSimulation.CheckInventory();
                    foreach (string ingred in inventory.Keys) {
                        if (inventory[ingred][0] <= 10)
                            gameSimulation.OrderIngredient(ingred, 75);
                    }
                    gameSimulation.SimulateDay();
            }
            return gameSimulation.CumulativeResults;
        }
    }