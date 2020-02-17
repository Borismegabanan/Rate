using System;

namespace MyLib
{
    public class CurrencyCodeAttribute : Attribute
    {
        public string Value { get; }

        public CurrencyCodeAttribute(string value)
        {
            Value = value;
        }
    }
}