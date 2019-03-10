using System;
using NUnit.Framework;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium;

namespace AdvantShop.SeleniumTest.Admin.Orders
{
    [TestFixture]
    public class OrdersPresentTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog | ClearType.Customers | ClearType.Orders);
            InitializeService.LoadData(

             "Data\\Admin\\Orders\\OrderGrid\\Catalog.Product.csv",
           "Data\\Admin\\Orders\\OrderGrid\\Catalog.Offer.csv",
           "Data\\Admin\\Orders\\OrderGrid\\Catalog.Category.csv",
          "Data\\Admin\\Orders\\OrderGrid\\Catalog.ProductCategories.csv",
           "Data\\Admin\\Orders\\OrderGrid\\Customers.CustomerGroup.csv",
           "data\\Admin\\Orders\\OrderGrid\\Customers.Country.csv",
            "data\\Admin\\Orders\\OrderGrid\\Customers.Region.csv",
            "data\\Admin\\Orders\\OrderGrid\\Customers.City.csv",
            "data\\Admin\\Orders\\OrderGrid\\Customers.Customer.csv",
            "data\\Admin\\Orders\\OrderGrid\\Customers.Contact.csv",
              "data\\Admin\\Orders\\OrderGrid\\Customers.Managers.csv",
            "data\\Admin\\Orders\\OrderGrid\\[Order].OrderCustomer.csv",
            "Data\\Admin\\Orders\\OrderGrid\\[Order].OrderSource.csv",
            "data\\Admin\\Orders\\OrderGrid\\[Order].OrderContact.csv",
            "data\\Admin\\Orders\\OrderGrid\\[Order].OrderCurrency.csv",
             "data\\Admin\\Orders\\OrderGrid\\[Order].OrderItems.csv",
             "data\\Admin\\Orders\\OrderGrid\\[Order].OrderStatus.csv",
             "data\\Admin\\Orders\\OrderGrid\\[Order].[Order].csv"
          );

            Init();
            GoToAdmin("orders");
        }

        [Test]
        public void OrdersPresent10()
        {
            testname = "OrdersPresent10";
            VerifyBegin(testname);

            ScrollToElements(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            PageSelectItems("10");
            VerifyAreEqual("96", GetGridCell(0, "Number").Text, "line 1");
            VerifyAreEqual("92", GetGridCell(9, "Number").Text, "line 10");

            VerifyFinally(testname);
        }
        
        [Test]
        public void OrdersPresent20()
        {
            testname = "OrdersPresent20";
            VerifyBegin(testname);

            ScrollToElements(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            PageSelectItems("20");
            VerifyAreEqual("96", GetGridCell(0, "Number").Text, "line 1");
            VerifyAreEqual("82", GetGridCell(19, "Number").Text, "line 20");

            VerifyFinally(testname);
        }

        [Test]
        public void OrdersPresent50()
        {
            testname = "OrdersPresent50";
            VerifyBegin(testname);

            ScrollToElements(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            PageSelectItems("50");
            VerifyAreEqual("96", GetGridCell(0, "Number").Text, "line 1");
            VerifyAreEqual("52", GetGridCell(49, "Number").Text, "line 50");

            VerifyFinally(testname);
        }


        [Test]
        public void OrdersPresent100()
        {
            testname = "OrdersPresent100";
            VerifyBegin(testname);

            ScrollToElements(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            PageSelectItems("100");
            VerifyAreEqual("96", GetGridCell(0, "Number").Text, "line 1");
            VerifyAreEqual("1", GetGridCell(98, "Number").Text, "line 99");

            VerifyFinally(testname);
        }

    }
}