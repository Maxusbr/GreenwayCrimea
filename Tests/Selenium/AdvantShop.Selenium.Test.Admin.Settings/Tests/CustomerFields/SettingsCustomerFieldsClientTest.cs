using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Settings.CustomerFields.Client
{
    [TestFixture]
    public class SettingsCustomerFieldsShowInClientTest : BaseMultiSeleniumTest
    {

        [SetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Customers | ClearType.Catalog);
            InitializeService.LoadData(
                "data\\Admin\\Settings\\CustomerFieldsClient\\Catalog.Category.csv",
               "data\\Admin\\Settings\\CustomerFieldsClient\\Catalog.Offer.csv",
               "data\\Admin\\Settings\\CustomerFieldsClient\\Catalog.Product.csv",
              "data\\Admin\\Settings\\CustomerFieldsClient\\Catalog.Size.csv",
             "data\\Admin\\Settings\\CustomerFieldsClient\\Catalog.ProductCategories.csv",
            "data\\Admin\\Settings\\CustomerFieldsClient\\Customers.Customer.csv",
           "data\\Admin\\Settings\\CustomerFieldsClient\\Customers.CustomerGroup.csv",
               "data\\Admin\\Settings\\CustomerFieldsClient\\Customers.CustomerField.csv",
               "data\\Admin\\Settings\\CustomerFieldsClient\\Customers.CustomerFieldValue.csv"

           );

            Init();
        }

         


        [Test]
        public void ClientRegister()
        {
            testname = "ClientRegister";
            VerifyBegin(testname);

            GoToClient();

            driver.FindElement(By.LinkText("Выйти")).Click();
            driver.FindElement(By.LinkText("Регистрация")).Click();
            Thread.Sleep(4000);

            VerifyIsTrue(driver.PageSource.Contains("Customer Field 1"), "field 1 show in client");
            VerifyIsTrue(driver.PageSource.Contains("Customer Field 2"), "field 2 show in client");
            VerifyIsTrue(driver.PageSource.Contains("Customer Field 3"), "field 3 show in client");
            VerifyIsTrue(driver.PageSource.Contains("Customer Field 4"), "field 4 show in client");
            VerifyIsTrue(driver.PageSource.Contains("Customer Field 5"), "field 5 show in client");
            VerifyIsFalse(driver.PageSource.Contains("Customer Field 6"), "field 6 not show in client");
            VerifyIsFalse(driver.PageSource.Contains("Customer Field 7"), "field 7 disabled");
            VerifyIsFalse(driver.PageSource.Contains("Customer Field 8"), "field 8 disabled");
            VerifyIsFalse(driver.PageSource.Contains("Customer Field 9"), "field 9 disabled");
            VerifyIsFalse(driver.PageSource.Contains("Customer Field 10"), "field 10 disabled");

            VerifyIsTrue(driver.FindElement(By.Id("CustomerFields_1__Value")).GetAttribute("required").Equals("true"), "field 2 show in client required");
            VerifyIsTrue(driver.FindElement(By.Id("CustomerFields_2__Value")).GetAttribute("required").Equals("true"), "field 3 show in client required");

            VerifyIsTrue(driver.FindElement(By.Id("CustomerFields_0__Value")).GetAttribute("ng-required").Equals("false"), "field 1 show in client not required");
            VerifyIsTrue(driver.FindElement(By.Id("CustomerFields_3__Value")).GetAttribute("ng-required").Equals("false"), "field 4 show in client not required");
            VerifyIsTrue(driver.FindElement(By.Id("CustomerFields_4__Value")).GetAttribute("ng-required").Equals("false"), "field 5 show in client not required");

            VerifyFinally(testname);
        }

        [Test]
        public void ClientOrder()
        {
            testname = "ClientOrder";
            VerifyBegin(testname);
            
            GoToClient("products/test-product41");

            ScrollTo(By.CssSelector("[title=\"SizeName1\"]"));
            driver.FindElement(By.CssSelector("[data-product-id=\"41\"]")).Click();

            GoToClient("cart");

            driver.FindElement(By.XPath("//a[contains(text(), 'Оформить')]")).Click();
            Thread.Sleep(4000);

            VerifyIsTrue(driver.PageSource.Contains("Customer Field 1"), "field 1 show in client");
            VerifyIsTrue(driver.PageSource.Contains("Customer Field 2"), "field 2 show in client");
            VerifyIsTrue(driver.PageSource.Contains("Customer Field 3"), "field 3 show in client");
            VerifyIsTrue(driver.PageSource.Contains("Customer Field 4"), "field 4 show in client");
            VerifyIsTrue(driver.PageSource.Contains("Customer Field 5"), "field 5 show in client");
            VerifyIsFalse(driver.PageSource.Contains("Customer Field 6"), "field 6 not show in client");
            VerifyIsFalse(driver.PageSource.Contains("Customer Field 7"), "field 7 disabled");
            VerifyIsFalse(driver.PageSource.Contains("Customer Field 8"), "field 8 disabled");
            VerifyIsFalse(driver.PageSource.Contains("Customer Field 9"), "field 9 disabled");
            VerifyIsFalse(driver.PageSource.Contains("Customer Field 10"), "field 10 disabled");
            
            VerifyIsTrue(driver.FindElement(By.Id("checkout_newCustomer_CustomerFields_1__Value")).GetAttribute("required").Equals("true"), "field 2 show in client required");
            VerifyIsTrue(driver.FindElement(By.Id("checkout_newCustomer_CustomerFields_2__Value")).GetAttribute("required").Equals("true"), "field 3 show in client required");

            VerifyIsTrue(driver.FindElement(By.Id("checkout_newCustomer_CustomerFields_0__Value")).GetAttribute("ng-required").Equals("false"), "field 1 show in client not required");
            VerifyIsTrue(driver.FindElement(By.Id("checkout_newCustomer_CustomerFields_3__Value")).GetAttribute("ng-required").Equals("false"), "field 4 show in client not required");
            VerifyIsTrue(driver.FindElement(By.Id("checkout_newCustomer_CustomerFields_4__Value")).GetAttribute("ng-required").Equals("false"), "field 5 show in client not required");

            VerifyFinally(testname);
        }

        [Test]
        public void ClientMyAccount()
        {
            testname = "ClientMyAccount";
            VerifyBegin(testname);

            GoToClient("myaccount#?tab=commoninf");
            
            VerifyIsTrue(driver.PageSource.Contains("Customer Field 1"), "field 1 show in client");
            VerifyIsTrue(driver.PageSource.Contains("Customer Field 2"), "field 2 show in client");
            VerifyIsTrue(driver.PageSource.Contains("Customer Field 3"), "field 3 show in client");
            VerifyIsTrue(driver.PageSource.Contains("Customer Field 4"), "field 4 show in client");
            VerifyIsTrue(driver.PageSource.Contains("Customer Field 5"), "field 5 show in client");
            VerifyIsFalse(driver.PageSource.Contains("Customer Field 6"), "field 6 not show in client");
            VerifyIsFalse(driver.PageSource.Contains("Customer Field 7"), "field 7 disabled");
            VerifyIsFalse(driver.PageSource.Contains("Customer Field 8"), "field 8 disabled");
            VerifyIsFalse(driver.PageSource.Contains("Customer Field 9"), "field 9 disabled");
            VerifyIsFalse(driver.PageSource.Contains("Customer Field 10"), "field 10 disabled");

            VerifyIsTrue(driver.FindElement(By.Id("CustomerFields_1__Value")).GetAttribute("required").Equals("true"), "field 2 show in client required");
            VerifyIsTrue(driver.FindElement(By.Id("CustomerFields_2__Value")).GetAttribute("required").Equals("true"), "field 3 show in client required");

            VerifyIsTrue(driver.FindElement(By.Id("CustomerFields_0__Value")).GetAttribute("ng-required").Equals("false"), "field 1 show in client not required");
            VerifyIsTrue(driver.FindElement(By.Id("CustomerFields_3__Value")).GetAttribute("ng-required").Equals("false"), "field 4 show in client not required");
            VerifyIsTrue(driver.FindElement(By.Id("CustomerFields_4__Value")).GetAttribute("ng-required").Equals("false"), "field 5 show in client not required");

            VerifyFinally(testname);
        }

    }
}