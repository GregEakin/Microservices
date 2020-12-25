// Copyright © 2020-2020. All Rights Reserved.
// 
// SUBSYSTEM: ShoppingCart
// FILE:  Money.cs
// AUTHOR:  Greg Eakin

namespace ShoppingCartSvc.Models
{
    public class Money
    {
        public string Currency { get; set; }
        public int Amount { get; set; }
        public Money() { }
        public Money(string currency, int amount)
        {
            Currency = currency;
            Amount = amount;
        }
    }
}