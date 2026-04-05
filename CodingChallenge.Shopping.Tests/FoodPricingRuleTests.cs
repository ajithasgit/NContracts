using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace CodingChallenge.Shopping.Tests
{
    public class FoodPricingRuleTests
    {
        [Test]
        public void Food_Items_Normal_Hours_No_Discount()
        {
            var cart = CartItemGenerator.MakeFoodCart();
            var calculator = CalculatorFactory.Create();
            var checkoutDate = new DateTime(2020, 11, 30, 10, 0, 0);

            var total = calculator.Calculate(cart, checkoutDate);

            decimal expected =
                0.79m * 3.27m +
                1.5m * 18m +
                1 * 6.99m +
                1.5m * 7.99m +
                1 * 25.99m;

            Assert.That(total, Is.EqualTo(expected));
        }

        [Test]
        public void Food_Items_Senior_Hours_Get_10Percent_Discount()
        {
            var cart = CartItemGenerator.MakeFoodCart();
            var calculator = CalculatorFactory.Create();
            var checkoutDate = new DateTime(2020, 11, 30, 7, 30, 0);

            var total = calculator.Calculate(cart, checkoutDate);

            decimal baseTotal =
                0.79m * 3.27m +
                1.5m * 18m +
                1 * 6.99m +
                1.5m * 7.99m +
                1 * 25.99m;

            decimal expected = ApplyDiscount(baseTotal, 10m);

            Assert.That(total, Is.EqualTo(expected));
        }

        [Test]
        public void Food_Item_With_Weight_Uses_Weight_For_Pricing()
        {
            var item = new CartItem
            {
                ProductName = "Test Weight Item",
                Category = "Food",
                Price = 10m,
                Weight = 2.5m
            };

            var calculator = CalculatorFactory.Create();
            var total = calculator.Calculate(new List<CartItem> { item }, new DateTime(2020, 11, 30));

            Assert.That(total, Is.EqualTo(25m));
        }

        [Test]
        public void Food_Item_With_Quantity_Uses_Quantity_For_Pricing()
        {
            var item = new CartItem
            {
                ProductName = "Test Quantity Item",
                Category = "Food",
                Price = 10m,
                Quantity = 3
            };

            var calculator = CalculatorFactory.Create();
            var total = calculator.Calculate(new List<CartItem> { item }, new DateTime(2020, 11, 30));

            Assert.That(total, Is.EqualTo(30m));
        }

        [Test]
        public void Senior_Discount_Applies_At_7AM()
        {
            var cart = CartItemGenerator.MakeFoodCart();
            var calculator = CalculatorFactory.Create();
            var checkoutDate = new DateTime(2020, 11, 30, 7, 0, 0);

            var total = calculator.Calculate(cart, checkoutDate);

            decimal baseTotal =
                0.79m * 3.27m +
                1.5m * 18m +
                1 * 6.99m +
                1.5m * 7.99m +
                1 * 25.99m;

            decimal expected = ApplyDiscount(baseTotal, 10m);

            Assert.That(total, Is.EqualTo(expected));
        }

        [Test]
        public void Senior_Discount_Does_Not_Apply_At_6AM()
        {
            var cart = CartItemGenerator.MakeFoodCart();
            var calculator = CalculatorFactory.Create();
            var checkoutDate = new DateTime(2020, 11, 30, 6, 0, 0);

            var total = calculator.Calculate(cart, checkoutDate);

            decimal expected =
                0.79m * 3.27m +
                1.5m * 18m +
                1 * 6.99m +
                1.5m * 7.99m +
                1 * 25.99m;

            Assert.That(total, Is.EqualTo(expected));
        }

        private decimal ApplyDiscount(decimal amount, decimal percent)
        {
            return amount - amount * (percent / 100m);
        }
    }
}