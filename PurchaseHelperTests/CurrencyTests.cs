using System;
using Xunit;
using PurchaseHelper;

namespace PurchaseHelperTests
{
    public class PurchaseHelperTests
    {
        [Fact]
        public void currencyTest()
        {
            CurrencyHelper obj = new CurrencyHelper();
            decimal price = obj.GetExchangeRates("CNY");
            decimal price2 = obj.GetExchangeRates("KRW");
        }
    }
}
