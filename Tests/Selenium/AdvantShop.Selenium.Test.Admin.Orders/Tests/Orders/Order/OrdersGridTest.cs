using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Orders
{
    [TestFixture]
    public class OrdersGridTest : BaseSeleniumTest
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
        public void OpenOrders()
        {
            testname = "OpenOrders";
            VerifyBegin(testname);

            VerifyAreEqual("Заказы", driver.FindElement(By.TagName("h1")).Text, "page h1");

            VerifyAreEqual("96", GetGridCell(0, "Number").Text, "order number");
            VerifyAreEqual("Отменен навсегда", GetGridCell(0, "StatusName").Text, "order status");
            VerifyAreEqual("FirstName1 LastName1", GetGridCell(0, "BuyerName").Text, "order customer");
            VerifyIsTrue(GetGridCell(0, "IsPaid").FindElement(By.TagName("input")).Selected, "order paid");
            VerifyAreEqual("Manager2 Name2", GetGridCell(0, "ManagerName").Text, "order manager");
            VerifyAreEqual("96", GetGridCell(0, "SumFormatted").Text, "order sum");
            VerifyAreEqual("31.08.2016 23:00", GetGridCell(0, "OrderDateFormatted").Text, "order date");

            VerifyAreEqual("Найдено записей: 99", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);
        }

        [Test]
        public void OpenTabNewOrders()
        {
            testname = "OpenTabNewOrders";
            VerifyBegin(testname);

            driver.FindElement(By.LinkText("Новые")).Click();
            WaitForAjax();
            VerifyAreEqual("5", GetGridCell(0, "Number").Text, "number order status new");
            VerifyIsTrue(GetGridCell(0, "StatusName").Text.Contains("Новый"), "order status new");
            VerifyAreEqual("Найдено записей: 5", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);
        }
        [Test]
        public void OpenTabPaidOrders()
        {
            testname = "OpenTabPaidOrders";
            VerifyBegin(testname);

            driver.FindElement(By.LinkText("Оплаченные")).Click();
            WaitForAjax();
            VerifyAreEqual("96", GetGridCell(0, "Number").Text, "number order paid line 1");
            VerifyIsTrue(GetGridCell(0, "IsPaid").FindElement(By.TagName("input")).Selected, "order paid line 1");
            VerifyAreEqual("92", GetGridCell(9, "Number").Text, "number order paid line 10");
            VerifyIsTrue(GetGridCell(9, "IsPaid").FindElement(By.TagName("input")).Selected, "order paid line 10");
            VerifyAreEqual("Найдено записей: 85", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);
        }

        [Test]
        public void OpenTabNotPaidOrders()
        {
            testname = "OpenTabNotPaidOrders";
            VerifyBegin(testname);

            driver.FindElement(By.LinkText("Неоплаченные")).Click();
            WaitForAjax();
            VerifyAreEqual("14", GetGridCell(0, "Number").Text, "number order not paid line 1");
            VerifyIsFalse(GetGridCell(0, "IsPaid").FindElement(By.TagName("input")).Selected, "order paid line 1");
            VerifyAreEqual("5", GetGridCell(9, "Number").Text, "number order not paid line 10");
            VerifyIsFalse(GetGridCell(9, "IsPaid").FindElement(By.TagName("input")).Selected, "order paid line 10");
            VerifyAreEqual("Найдено записей: 14", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);
        }

        [Test]
        public void OpenTabDrafts()
        {
            testname = "OpenTabDrafts";
            VerifyBegin(testname);

            driver.FindElement(By.LinkText("Черновики")).Click();
            WaitForAjax();
            VerifyAreEqual("100", GetGridCell(0, "Number").Text, "number order draft line 1");
            VerifyAreEqual("101", GetGridCell(1, "Number").Text, "number order draft line 2");
            VerifyAreEqual("102", GetGridCell(2, "Number").Text, "number order draft line 3");
            VerifyAreEqual("Найдено записей: 3", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);
        }
        
    }
}