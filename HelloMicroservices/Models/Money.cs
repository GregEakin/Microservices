// Copyright © 2020-2020. All Rights Reserved.
// 
// SUBSYSTEM: HelloMicroservices
// FILE:  Money.cs
// AUTHOR:  Greg Eakin

namespace HelloMicroservices.Models
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