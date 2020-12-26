// Copyright © 2020-2020. All Rights Reserved.
// 
// SUBSYSTEM: ShoppingCart
// FILE:  Money.cs
// AUTHOR:  Greg Eakin

namespace ShoppingCartSvc.Carts
{
    public class Money
    {
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public Money() { }
        public Money(string currency, decimal amount)
        {
            Currency = currency;
            Amount = amount;
        }
    }
}