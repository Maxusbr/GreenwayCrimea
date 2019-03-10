using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.Web.Site.Selenium.Test.Admin.Customers
{
    [TestFixture]
    public class CustomersPresentTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Customers | ClearType.Orders | ClearType.Catalog);
            InitializeService.LoadData(
               "data\\Admin\\Customers\\CustomerGrid\\Customers.CustomerGroup.csv",
                  "data\\Admin\\Customers\\CustomerGrid\\Customers.Country.csv",
            "data\\Admin\\Customers\\CustomerGrid\\Customers.Region.csv",
            "data\\Admin\\Customers\\CustomerGrid\\Customers.City.csv",
            "data\\Admin\\Customers\\CustomerGrid\\Customers.Customer.csv",
            "data\\Admin\\Customers\\CustomerGrid\\Customers.Contact.csv",
                       "data\\Admin\\Customers\\CustomerGrid\\Customers.Departments.csv",
           "data\\Admin\\Customers\\CustomerGrid\\Customers.Managers.csv",
               "data\\Admin\\Customers\\CustomerGrid\\Customers.CustomerField.csv",
               "data\\Admin\\Customers\\CustomerGrid\\Customers.CustomerFieldValue.csv",
               "data\\Admin\\Customers\\CustomerGrid\\Customers.CustomerFieldValuesMap.csv",
             "data\\Admin\\Customers\\CustomerGrid\\Catalog.Product.csv",
           "data\\Admin\\Customers\\CustomerGrid\\Catalog.Offer.csv",
           "data\\Admin\\Customers\\CustomerGrid\\Catalog.Category.csv",
           "data\\Admin\\Customers\\CustomerGrid\\Catalog.ProductCategories.csv",
            "data\\Admin\\Customers\\CustomerGrid\\[Order].OrderContact.csv",
              "Data\\Admin\\Customers\\CustomerGrid\\[Order].OrderSource.csv",
            "data\\Admin\\Customers\\CustomerGrid\\[Order].OrderCurrency.csv",
             "data\\Admin\\Customers\\CustomerGrid\\[Order].OrderItems.csv",
             "data\\Admin\\Customers\\CustomerGrid\\[Order].OrderStatus.csv",
                 "data\\Admin\\Customers\\CustomerGrid\\[Order].PaymentMethod.csv",
            "data\\Admin\\Customers\\CustomerGrid\\[Order].ShippingMethod.csv",
               "data\\Admin\\Customers\\CustomerGrid\\[Order].[Order].csv",

               "data\\Admin\\Customers\\CustomerGrid\\[Order].OrderCustomer.csv"

           );

            Init();
            GoToAdmin("customers");
        }
        
        [Test]
        public void CustomerPresent10()
        {
            testname = "CustomerPresent10";
            VerifyBegin(testname);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            PageSelectItems("10");
            VerifyAreEqual("FirstName120 LastName120", GetGridCell(0, "Name").FindElement(By.TagName("a")).Text, "line 1");
            VerifyAreEqual("FirstName111 LastName111", GetGridCell(9, "Name").FindElement(By.TagName("a")).Text, "line 10");

            VerifyFinally(testname);
        }

        [Test]
        public void CustomerPresent20()
        {
            testname = "CustomerPresent20";
            VerifyBegin(testname);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            PageSelectItems("20");
            VerifyAreEqual("FirstName120 LastName120", GetGridCell(0, "Name").FindElement(By.TagName("a")).Text, "line 1");
            VerifyAreEqual("FirstName101 LastName101", GetGridCell(19, "Name").FindElement(By.TagName("a")).Text, "line 20");

            VerifyFinally(testname);
        }

        [Test]
        public void CustomerPresent50()
        {
            testname = "CustomerPresent50";
            VerifyBegin(testname);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            PageSelectItems("50");
            VerifyAreEqual("FirstName120 LastName120", GetGridCell(0, "Name").FindElement(By.TagName("a")).Text, "line 1");
            VerifyAreEqual("FirstName71 LastName71", GetGridCell(49, "Name").FindElement(By.TagName("a")).Text, "line 50");

            VerifyFinally(testname);
        }

        [Test]
        public void CustomerPresent100()
        {
            testname = "CustomerPresent100";
            VerifyBegin(testname);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            PageSelectItems("100");
            VerifyAreEqual("FirstName120 LastName120", GetGridCell(0, "Name").FindElement(By.TagName("a")).Text, "line 1");
            VerifyAreEqual("FirstName21 LastName21", GetGridCell(99, "Name").FindElement(By.TagName("a")).Text, "line 100");

            VerifyFinally(testname);
        }
    }
}