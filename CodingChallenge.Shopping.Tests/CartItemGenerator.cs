using System.Collections.Generic;
using CodingChallenge.Shopping;

namespace CodingChallenge.Shopping.Tests
{
    public static class CartItemGenerator
    {
        public static List<CartItem> MakeChristmasCart()
        {
            return new List<CartItem>
            {
                new CartItem {ProductName = "Lights", Category = "Christmas", Price = 5.99m, Quantity = 10},
                new CartItem {ProductName = "Tree", Category = "Christmas", Price = 169m, Quantity = 1},
                new CartItem {ProductName = "Ornaments", Category = "Christmas", Price = 8m, Quantity = 15},
            };
        }

        public static List<CartItem> MakeFoodCart()
        {
            return new List<CartItem>
            {
                new CartItem {ProductName = "Apple", Category = "Food", Price = 3.27m, Weight = 0.79m},
                new CartItem {ProductName = "Scallop", Category = "Food", Price = 18m, Weight = 1.5m},
                new CartItem {ProductName = "Salad", Category = "Food", Price = 6.99m, Quantity = 1},
                new CartItem {ProductName = "Ground Beef", Category = "Food", Price = 7.99m, Weight = 1.5m},
                new CartItem {ProductName = "Red Wine", Category = "Food", Price = 25.99m, Quantity = 1}
            };
        }
    }
}