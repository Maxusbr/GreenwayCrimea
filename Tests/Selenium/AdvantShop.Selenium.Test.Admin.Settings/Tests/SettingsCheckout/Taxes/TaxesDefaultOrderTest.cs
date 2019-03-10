using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using AdvantShop.Selenium.Core.Infrastructure;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.Settings.SettingsCheckout.Taxes.InOrders
{
    [TestFixture]
    public class SettingsTaxesDefaultOrderTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(

            "data\\Admin\\Settings\\Taxes\\TaxDefault\\Catalog.Product.csv",
            "data\\Admin\\Settings\\Taxes\\TaxDefault\\Catalog.Offer.csv",
            "data\\Admin\\Settings\\Taxes\\TaxDefault\\Catalog.Category.csv",
            "data\\Admin\\Settings\\Taxes\\TaxDefault\\Catalog.ProductCategories.csv"
           );

            Init();
        }
        
        [Test]
        public void TaxDefaultOrderClient()
        {
            testname = "TaxDefaultOrderClient";
            VerifyBegin(testname);

            //pre check admin default tax
            GoToAdmin("product/edit/1");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Цена и наличие')]")).Click();
            Thread.Sleep(1000);

            IWebElement selectProd = driver.FindElement(By.Id("TaxId"));
            SelectElement select = new SelectElement(selectProd);
            VerifyIsTrue(select.SelectedOption.Text.Contains("Без НДС"), "pre check tax default");

            //order
            GoToClient("products/test-product1");

            ScrollTo(By.CssSelector(".details-row.details-rating"));
            driver.FindElement(By.CssSelector("[data-product-id=\"1\"]")).Click();
            Thread.Sleep(4000);

            GoToClient("cart");

            ScrollTo(By.CssSelector(".cart-full-result-name"));
            driver.FindElement(By.CssSelector(".btn.btn-middle.btn-submit")).Click();
            Thread.Sleep(4000);
            WaitForElem(By.CssSelector(".breads"));
            WaitForElem(By.Id("rightCell"));

            ScrollTo(By.CssSelector(".btn.btn-small.btn-action.btn-expander"));
            driver.FindElement(By.CssSelector("input.btn.btn-big.btn-submit")).Click();
            Thread.Sleep(4000);

            //check admin order
            GoToAdmin("orders/edit/3");

            VerifyIsFalse(driver.FindElement(By.TagName("order-items-summary")).Text.Contains("В том числе Без НДС"), "default tax in order");

            VerifyFinally(testname);
        }

    }
}