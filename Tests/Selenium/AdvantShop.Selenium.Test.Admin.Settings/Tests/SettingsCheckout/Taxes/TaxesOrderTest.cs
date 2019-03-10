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
    public class SettingsTaxesOrderTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog | ClearType.Taxes);
            InitializeService.LoadData(

            "data\\Admin\\Settings\\Taxes\\TaxesClient\\Catalog.Product.csv",
            "data\\Admin\\Settings\\Taxes\\TaxesClient\\Catalog.Offer.csv",
            "data\\Admin\\Settings\\Taxes\\TaxesClient\\Catalog.Category.csv",
            "data\\Admin\\Settings\\Taxes\\TaxesClient\\Catalog.ProductCategories.csv",
            "data\\Admin\\Settings\\Taxes\\TaxesClient\\Catalog.Tax.csv",
           "data\\Admin\\Settings\\Taxes\\TaxesClient\\Settings.Settings.csv"

           );

            Init();
        }



        [Test]
        public void TaxProductOrderClient()
        {
            testname = "TaxProductOrderClient";
            VerifyBegin(testname);
            
            //pre check admin default tax
            GoToAdmin("product/edit/10");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Цена и наличие')]")).Click();
            Thread.Sleep(1000);

            IWebElement selectProd = driver.FindElement(By.Id("TaxId"));
            SelectElement select = new SelectElement(selectProd);
            VerifyIsTrue(select.SelectedOption.Text.Contains("Tax 10"), "pre check product tax");

            //order
            GoToClient("products/test-product10"); 

            ScrollTo(By.CssSelector(".details-row.details-rating"));
            driver.FindElement(By.CssSelector("[data-product-id=\"10\"]")).Click();
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
            GoToAdmin("orders");
            GetGridCell(0, "Number").FindElement(By.TagName("a")).Click();
            Thread.Sleep(2000);
            WaitForElem(By.CssSelector("[grid-unique-id=\"gridOrderItems\"]"));

            VerifyIsTrue(driver.FindElement(By.TagName("order-items-summary")).Text.Contains("В том числе Tax 10"), "product tax in order");

            VerifyFinally(testname);
        }

        [Test]
        public void TaxProductOrderAdmin()
        {
            testname = "TaxProductOrderAdmin";
            VerifyBegin(testname);

            //pre check admin default tax
            GoToAdmin("product/edit/11");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Цена и наличие')]")).Click();
            Thread.Sleep(1000);

            IWebElement selectProd = driver.FindElement(By.Id("TaxId"));
            SelectElement select = new SelectElement(selectProd);
            VerifyIsTrue(select.SelectedOption.Text.Contains("Tax 11"), "pre check product tax");

            //order
            GoToAdmin("orders/add");

            driver.FindElement(By.Id("Order_OrderCustomer_LastName")).Click();
            driver.FindElement(By.Id("Order_OrderCustomer_LastName")).Clear();
            driver.FindElement(By.Id("Order_OrderCustomer_LastName")).SendKeys("Customer");
            driver.FindElement(By.CssSelector(".inline")).Click();

            ScrollTo(By.Id("Order_OrderCustomer_Phone"));
            driver.FindElement(By.LinkText("Добавить товар")).Click();
            Thread.Sleep(2000);
            WaitForElem(By.CssSelector(".modal-content"));
            
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("TestProduct11");
            XPathContainsText("h2", "Выбор товара");
            WaitForAjax();

            GetGridCell(0, "selectionRowHeaderCol", "OffersSelectvizr").Click();
            XPathContainsText("button", "Выбрать");
            Thread.Sleep(2000);

            ScrollTo(By.Id("header-top"));
            Thread.Sleep(4000);
            driver.FindElement(By.CssSelector("[data-e2e=\"btnSave\"]")).Click();
            Thread.Sleep(2000);
            WaitForAjax();

            //check admin order
            GoToAdmin("orders");
            GetGridCell(0, "Number").FindElement(By.TagName("a")).Click();
            Thread.Sleep(2000);
            WaitForElem(By.CssSelector("[grid-unique-id=\"gridOrderItems\"]"));

            VerifyIsTrue(driver.FindElement(By.TagName("order-items-summary")).Text.Contains("В том числе Tax 11"), "product tax in order");

            VerifyFinally(testname);
        }

    }
}