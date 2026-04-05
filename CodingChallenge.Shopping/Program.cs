using System;
using System.Collections.Generic;
using System.Linq;

namespace CodingChallenge.Shopping
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = new Program();

            program.ChristmasShoppingAtTheGroceryStore();
            program.BuyingFood();
        }

        void ChristmasShoppingAtTheGroceryStore()
        {
            var carts = new List<CartItem>
            {
                new CartItem {ProductName = "Lights", Category = "Christmas", Price = 5.99m, Quantity = 10},
                new CartItem {ProductName = "Tree", Category = "Christmas", Price = 169m, Quantity = 1},
                new CartItem {ProductName = "Ornaments", Category = "Christmas", Price = 8m, Quantity = 15},
            };

            var calculator = CreateCalculator();
            var total = calculator.Calculate(carts, new DateTime(2020, 11, 30));
            Console.WriteLine(total);

            var totalAfterChristmas = calculator.Calculate(carts, new DateTime(2020, 12, 30));
            Console.WriteLine(totalAfterChristmas);
        }

        void BuyingFood()
        {
            var carts = new List<CartItem>
            {
                new CartItem {ProductName = "Apple", Category = "Food", Price = 3.27m, Weight = 0.79m},
                new CartItem {ProductName = "Scallop", Category = "Food", Price = 18m, Weight = 1.5m},
                new CartItem {ProductName = "Salad", Category = "Food", Price = 6.99m, Quantity = 1},
                new CartItem {ProductName = "Ground Beef", Category = "Food", Price = 7.99m, Weight = 1.5m},
                new CartItem {ProductName = "Red Wine", Category = "Food", Price = 25.99m, Quantity = 1}
            };

            var calculator = CreateCalculator();
            var total = calculator.Calculate(carts, new DateTime(2020, 11, 30));
            Console.WriteLine(total);

            var seniorHourTotal = calculator.Calculate(carts, new DateTime(2020, 11, 30, 7, 11, 0));
            Console.WriteLine(seniorHourTotal);
        }

        private GroceryStoreCheckoutCalculator CreateCalculator()
        {
            var rules = new List<ICartPricingRule>
            {
                new ChristmasPricingRule(),
                new FoodPricingRule(),
                new DefaultPricingRule()
                // future: new FirstResponderDiscountRule(), new JanuarySaleRule(), etc.
            };

            return new GroceryStoreCheckoutCalculator(rules);
        }
    }

    public interface ICartPricingRule
    {
        bool AppliesTo(CartItem item, DateTime checkoutDate);
        decimal CalculateTotal(CartItem item, DateTime checkoutDate);
    }

    public class GroceryStoreCheckoutCalculator
    {
        private readonly IReadOnlyList<ICartPricingRule> _rules;

        public GroceryStoreCheckoutCalculator(IEnumerable<ICartPricingRule> rules)
        {
            _rules = rules.ToList();
        }

        public decimal Calculate(List<CartItem> carts, DateTime checkOutDate)
        {
            decimal total = 0m;

            foreach (var item in carts)
            {
                if (string.IsNullOrWhiteSpace(item.Category))
                {
                    // In real life: log or throw
                    continue;
                }

                var rule = _rules.FirstOrDefault(r => r.AppliesTo(item, checkOutDate));

                if (rule == null)
                {
                    // Fallback if no rule matches
                    rule = new DefaultPricingRule();
                }

                total += rule.CalculateTotal(item, checkOutDate);
            }

            return total;
        }
    }

    public class ChristmasPricingRule : ICartPricingRule
    {
        public bool AppliesTo(CartItem item, DateTime checkoutDate)
        {
            return item.Category == "Christmas";
        }

        public decimal CalculateTotal(CartItem item, DateTime checkoutDate)
        {
            var quantity = GetEffectiveQuantity(item);

            if (checkoutDate.Month != 12)
            {
                return quantity * item.Price;
            }

            var discountPercent = GetChristmasDiscountPercentage(checkoutDate);
            var discountedPrice = ApplyPercentageDiscount(item.Price, discountPercent);

            return quantity * discountedPrice;
        }

        private decimal GetChristmasDiscountPercentage(DateTime date)
        {
            if (date.Day < 15)
                return 20m;

            if (date.Day <= 25)
                return 60m;

            return 90m;
        }

        private decimal GetEffectiveQuantity(CartItem item)
        {
            return item.Weight > 0 ? item.Weight : item.Quantity;
        }

        private decimal ApplyPercentageDiscount(decimal amount, decimal percentage)
        {
            return amount - amount * (percentage / 100m);
        }
    }

    public class FoodPricingRule : ICartPricingRule
    {
        public bool AppliesTo(CartItem item, DateTime checkoutDate)
        {
            return item.Category == "Food";
        }

        public decimal CalculateTotal(CartItem item, DateTime checkoutDate)
        {
            var quantity = GetEffectiveQuantity(item);
            var baseTotal = quantity * item.Price;

            if (IsSeniorHour(checkoutDate))
            {
                return ApplyPercentageDiscount(baseTotal, 10m);
            }

            return baseTotal;
        }

        private bool IsSeniorHour(DateTime checkOutDate)
        {
            var hour = checkOutDate.TimeOfDay.Hours;
            return hour > 6 && hour <= 8;
        }

        private decimal GetEffectiveQuantity(CartItem item)
        {
            return item.Weight > 0 ? item.Weight : item.Quantity;
        }

        private decimal ApplyPercentageDiscount(decimal amount, decimal percentage)
        {
            return amount - amount * (percentage / 100m);
        }
    }

    public class DefaultPricingRule : ICartPricingRule
    {
        public bool AppliesTo(CartItem item, DateTime checkoutDate)
        {
            // fallback rule – applies when nothing else does
            return true;
        }

        public decimal CalculateTotal(CartItem item, DateTime checkoutDate)
        {
            var quantity = item.Weight > 0 ? item.Weight : item.Quantity;
            return quantity * item.Price;
        }
    }

    public class CartItem
    {
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Category { get; set; }
        public decimal Weight { get; set; }
    }
}