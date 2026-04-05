using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace CodingChallenge.Shopping.Tests
{
    public class ChristmasPricingRuleTests
    {
        [Test]
        public void BeforeDecember_NoChristmasDiscount()
        {
            var cart = CartItemGenerator.MakeChristmasCart();
            var calculator = CalculatorFactory.Create();
            var checkoutDate = new DateTime(2020, 11, 30);

            var total = calculator.Calculate(cart, checkoutDate);

            decimal expected =
                10 * 5.99m +
                1 * 169m +
                15 * 8m;

            Assert.That(total, Is.EqualTo(expected));
        }

        [Test]
        public void EarlyDecember_20PercentDiscount()
        {
            var cart = CartItemGenerator.MakeChristmasCart();
            var calculator = CalculatorFactory.Create();
            var checkoutDate = new DateTime(2020, 12, 5);

            var total = calculator.Calculate(cart, checkoutDate);

            decimal expected =
                10 * ApplyDiscount(5.99m, 20m) +
                1 * ApplyDiscount(169m, 20m) +
                15 * ApplyDiscount(8m, 20m);

            Assert.That(total, Is.EqualTo(expected));
        }

        [Test]
        public void MidDecember_60PercentDiscount()
        {
            var cart = CartItemGenerator.MakeChristmasCart();
            var calculator = CalculatorFactory.Create();
            var checkoutDate = new DateTime(2020, 12, 20);

            var total = calculator.Calculate(cart, checkoutDate);

            decimal expected =
                10 * ApplyDiscount(5.99m, 60m) +
                1 * ApplyDiscount(169m, 60m) +
                15 * ApplyDiscount(8m, 60m);

            Assert.That(total, Is.EqualTo(expected));
        }

        [Test]
        public void LateDecember_90PercentDiscount()
        {
            var cart = CartItemGenerator.MakeChristmasCart();
            var calculator = CalculatorFactory.Create();
            var checkoutDate = new DateTime(2020, 12, 30);

            var total = calculator.Calculate(cart, checkoutDate);

            decimal expected =
                10 * ApplyDiscount(5.99m, 90m) +
                1 * ApplyDiscount(169m, 90m) +
                15 * ApplyDiscount(8m, 90m);

            Assert.That(total, Is.EqualTo(expected));
        }

        [Test]
        public void January_NoChristmasDiscount()
        {
            var cart = CartItemGenerator.MakeChristmasCart();
            var calculator = CalculatorFactory.Create();
            var checkoutDate = new DateTime(2021, 1, 5);

            var total = calculator.Calculate(cart, checkoutDate);

            decimal expected =
                10 * 5.99m +
                1 * 169m +
                15 * 8m;

            Assert.That(total, Is.EqualTo(expected));
        }

        // ------------------------------
        // Single-item tests (cleaner)
        // ------------------------------

        [Test]
        public void SingleItem_EarlyDecember_20PercentDiscount()
        {
            var item = new CartItem
            {
                ProductName = "Lights",
                Category = "Christmas",
                Price = 10m,
                Quantity = 1
            };

            var calculator = CalculatorFactory.Create();
            var total = calculator.Calculate(new List<CartItem> { item }, new DateTime(2020, 12, 5));

            Assert.That(total, Is.EqualTo(ApplyDiscount(10m, 20m)));
        }

        [Test]
        public void SingleItem_MidDecember_60PercentDiscount()
        {
            var item = new CartItem
            {
                ProductName = "Lights",
                Category = "Christmas",
                Price = 10m,
                Quantity = 1
            };

            var calculator = CalculatorFactory.Create();
            var total = calculator.Calculate(new List<CartItem> { item }, new DateTime(2020, 12, 20));

            Assert.That(total, Is.EqualTo(ApplyDiscount(10m, 60m)));
        }

        [Test]
        public void SingleItem_LateDecember_90PercentDiscount()
        {
            var item = new CartItem
            {
                ProductName = "Lights",
                Category = "Christmas",
                Price = 10m,
                Quantity = 1
            };

            var calculator = CalculatorFactory.Create();
            var total = calculator.Calculate(new List<CartItem> { item }, new DateTime(2020, 12, 30));

            Assert.That(total, Is.EqualTo(ApplyDiscount(10m, 90m)));
        }

        private decimal ApplyDiscount(decimal price, decimal percent)
        {
            return price - price * (percent / 100m);
        }
    }
}