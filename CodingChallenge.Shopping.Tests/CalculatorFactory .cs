using System.Collections.Generic;

namespace CodingChallenge.Shopping.Tests
{
    public static class CalculatorFactory
    {
        public static GroceryStoreCheckoutCalculator Create()
        {
            var rules = new List<ICartPricingRule>
            {
                new ChristmasPricingRule(),
                new FoodPricingRule(),
                new DefaultPricingRule()
            };

            return new GroceryStoreCheckoutCalculator(rules);
        }
    }
}