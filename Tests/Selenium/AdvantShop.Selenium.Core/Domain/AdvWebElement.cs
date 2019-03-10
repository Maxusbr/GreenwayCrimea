using OpenQA.Selenium;
using Protractor;
using System.Threading;

namespace AdvantShop.SeleniumTest.Core
{
    public class AdvWebElement : NgWebElement
    {
        public AdvWebElement(NgWebDriver ngDriver, IWebElement element) : base(ngDriver, element)
        {
        }

        public new void SendKeys(string text)
        {
            if (string.IsNullOrWhiteSpace(text) || text.Length < 3)
            {
                base.SendKeys(text);
            }
            else
            {
                var firstStr = text.Substring(0, text.Length - 2);
                base.SendKeys(firstStr);

                Thread.Sleep(500);

                var lastSymbol = text.Substring(text.Length - 2);
                base.SendKeys(lastSymbol);
            }
        }
    }
}
