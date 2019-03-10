using System;
using NUnit.Framework;

namespace AdvantShop.SeleniumTest.Core
{
    public class BaseMultiSeleniumTest : BaseSeleniumTest
    {
        [TearDown]
        public void Teardown()
        {
            try
            {
              //  Console.Error.WriteLine(verificationErrors);
                driver.Quit();               
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
        }
    }
}
