using System.Linq;
using System.Xml.Linq;

namespace PurchaseHelper
{
    public class CurrencyHelper
    {
        private string exchangeRateUrl = "http://finance.yahoo.com/webservice/v1/symbols/allcurrencies/quote?format=xml";

        public decimal GetExchangeRates(string currencySymbol)
        {
            string symbol = $"USD/{currencySymbol}";
            decimal result = 0;
            XDocument doc = XDocument.Load(exchangeRateUrl);
            foreach(XElement currency in doc.Descendants("resource"))
            {
                string name = (from field in currency.Descendants("field") where (string)field.Attribute("name") == "name" select field).FirstOrDefault().Value;
                if (name == symbol)
                {
                    string price = (from field in currency.Descendants("field") where (string)field.Attribute("name") == "price" select field).FirstOrDefault().Value;
                    result = decimal.Parse(price);
                    break;
                }
            }
            
            return result;
        }
    }
}
