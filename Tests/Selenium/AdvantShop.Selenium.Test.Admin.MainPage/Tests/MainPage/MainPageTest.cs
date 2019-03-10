using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using AdvantShop.Selenium.Core.Infrastructure;
using System.Collections.ObjectModel;

namespace AdvantShop.Selenium.Test.Admin.MainPage.Tests.MainPage
{
    [TestFixture]
    public class MainPageTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Orders | ClearType.Catalog| ClearType.CRM| ClearType.Customers);
            InitializeService.LoadData(
           "data\\Admin\\MainPage\\Page\\Catalog.Product.csv",
           "data\\Admin\\MainPage\\Page\\Catalog.Offer.csv",
           "data\\Admin\\MainPage\\Page\\Catalog.Category.csv",
           "data\\Admin\\MainPage\\Page\\Catalog.ProductCategories.csv",
            "data\\Admin\\MainPage\\Page\\[Order].OrderContact.csv",
            "data\\Admin\\MainPage\\Page\\[Order].OrderCurrency.csv",
             "data\\Admin\\MainPage\\Page\\[Order].OrderItems.csv",
             "data\\Admin\\MainPage\\Page\\[Order].OrderSource.csv",
             "data\\Admin\\MainPage\\Page\\[Order].OrderStatus.csv",
             "data\\Admin\\MainPage\\Page\\[Order].[Order].csv",
             "data\\Admin\\MainPage\\Page\\[Order].LeadItem.csv",
             "data\\Admin\\MainPage\\Page\\[Order].LeadCurrency.csv",
             "data\\Admin\\MainPage\\Page\\[Order].Lead.csv",
              "data\\Admin\\MainPage\\Page\\Customers.CustomerGroup.csv",
             "data\\Admin\\MainPage\\Page\\Customers.Country.csv",
             "data\\Admin\\MainPage\\Page\\Customers.City.csv",
             "data\\Admin\\MainPage\\Page\\Customers.Contact.csv",
             "data\\Admin\\MainPage\\Page\\Customers.Customer.csv",
             "data\\Admin\\MainPage\\Page\\Customers.Departments.csv",
             "data\\Admin\\MainPage\\Page\\Customers.Managers.csv",
             "data\\Admin\\MainPage\\Page\\Customers.Region.csv"
            );

            Init();
        }
        [Test]
        public void MainPageOrderStatus()
        {
            GoToAdmin();
            testname = "MainPageOrderStatus";
            VerifyBegin(testname);

            VerifyAreEqual("Все заказы", driver.FindElement(By.CssSelector("[data-e2e=\"status-id-0\"]")).Text, "orders all");
            VerifyAreEqual("Новый", driver.FindElement(By.CssSelector("[data-e2e=\"status-id-2\"]")).Text, "orders new");
            VerifyAreEqual("В обработке", driver.FindElement(By.CssSelector("[data-e2e=\"status-id-3\"]")).Text, "orders check");
            VerifyAreEqual("Отправлен", driver.FindElement(By.CssSelector("[data-e2e=\"status-id-4\"]")).Text, "orders shipping");
            VerifyAreEqual("Доставлен", driver.FindElement(By.CssSelector("[data-e2e=\"status-id-5\"]")).Text, "orders teke");
            VerifyAreEqual("Отменён", driver.FindElement(By.CssSelector("[data-e2e=\"status-id-27\"]")).Text, "orders cancel");
            VerifyAreEqual("Отменен навсегда", driver.FindElement(By.CssSelector("[data-e2e=\"status-id-28\"]")).Text, "orders cancel forever");
            
            VerifyAreEqual("30", driver.FindElement(By.CssSelector("[data-e2e=\"count-status-id-0\"]")).Text, "count orders all");
            VerifyAreEqual("2", driver.FindElement(By.CssSelector("[data-e2e=\"count-status-id-2\"]")).Text, "count orders new");
            VerifyAreEqual("3", driver.FindElement(By.CssSelector("[data-e2e=\"count-status-id-3\"]")).Text, "count orders check");
            VerifyAreEqual("4", driver.FindElement(By.CssSelector("[data-e2e=\"count-status-id-4\"]")).Text, "count orders shipping");
            VerifyAreEqual("5", driver.FindElement(By.CssSelector("[data-e2e=\"count-status-id-5\"]")).Text, "count orders teke");
            VerifyAreEqual("6", driver.FindElement(By.CssSelector("[data-e2e=\"count-status-id-27\"]")).Text, "count orders cancel");
            VerifyAreEqual("10", driver.FindElement(By.CssSelector("[data-e2e=\"count-status-id-28\"]")).Text, "count orders cancel forever");

            driver.FindElement(By.CssSelector("[data-e2e=\"status-id-0\"]")).Click();
            Thread.Sleep(2000);
            VerifyIsTrue(driver.Url.Contains("/orders"), "url link");
            VerifyAreEqual("1", GetGridCell(0, "Number").Text, "check first item num");
            VerifyAreEqual("Новый", GetGridCell(0, "StatusName").Text, "check first item status");
            VerifyAreEqual("Найдено записей: 30", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all order on page");
            VerifyIsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridFilterBlock\"]")).Count==0);

            driver.FindElement(By.CssSelector(".logo-block-cell-text")).Click();
            Thread.Sleep(2000);
            VerifyIsTrue(driver.Url.EndsWith("/adminv2/"), "url link main page");
            driver.FindElement(By.CssSelector("[data-e2e=\"status-id-4\"]")).Click();
            Thread.Sleep(2000);
            VerifyIsTrue(driver.Url.Contains("/orders"), "url link 2");
            VerifyAreEqual("6", GetGridCell(0, "Number").Text, "check first item num 2");
            VerifyAreEqual("Отправлен", GetGridCell(0, "StatusName").Text, "check first item status 2");
            VerifyAreEqual("Найдено записей: 4", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all order on page 2");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"_noopColumnStatuses\"]")).Displayed);

            IWebElement selectElem1 = driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]"));
            SelectElement select3 = new SelectElement(selectElem1);
            VerifyIsTrue(select3.AllSelectedOptions[0].Text.Contains("Отправлен"), "right selected item");
            VerifyFinally(testname);
        }
        [Test]
        public void MainPageOrderSource()
        {
            GoToAdmin();
            testname = "MainPageOrderSource";
            VerifyBegin(testname);

            VerifyAreEqual("Корзина интернет магазина", driver.FindElement(By.CssSelector("[data-e2e=\"source-id-1\"]")).Text, "orders Cart");
            VerifyAreEqual("Оффлайн", driver.FindElement(By.CssSelector("[data-e2e=\"source-id-2\"]")).Text, "orders Offline");
            VerifyAreEqual("В один клик", driver.FindElement(By.CssSelector("[data-e2e=\"source-id-3\"]")).Text, "orders OneClick");
            VerifyAreEqual("Посадочная страница", driver.FindElement(By.CssSelector("[data-e2e=\"source-id-4\"]")).Text, "orders LandingPage");
            VerifyAreEqual("Мобильная версия", driver.FindElement(By.CssSelector("[data-e2e=\"source-id-5\"]")).Text, "orders Mobile");
            VerifyAreEqual("По телефону", driver.FindElement(By.CssSelector("[data-e2e=\"source-id-6\"]")).Text, "orders Phone");
            VerifyAreEqual("Онлайн консультант", driver.FindElement(By.CssSelector("[data-e2e=\"source-id-7\"]")).Text, "orders LiveChat");
            VerifyAreEqual("Социальные сети", driver.FindElement(By.CssSelector("[data-e2e=\"source-id-8\"]")).Text, "orders SocialNetworks");
            VerifyAreEqual("Нашли дешевле", driver.FindElement(By.CssSelector("[data-e2e=\"source-id-9\"]")).Text, "orders FindCheaper");
            VerifyAreEqual("Брошенные корзины", driver.FindElement(By.CssSelector("[data-e2e=\"source-id-10\"]")).Text, "orders AbandonedCart");

            VerifyAreEqual("20%", driver.FindElement(By.CssSelector("[data-e2e=\"percent-source-id-1\"]")).Text, "percent orders Cart");
            VerifyAreEqual("17%", driver.FindElement(By.CssSelector("[data-e2e=\"percent-source-id-2\"]")).Text, "percent orders Offline");
            VerifyAreEqual("13%", driver.FindElement(By.CssSelector("[data-e2e=\"percent-source-id-3\"]")).Text, "percent orders OneClick");
            VerifyAreEqual("13%", driver.FindElement(By.CssSelector("[data-e2e=\"percent-source-id-4\"]")).Text, "percent orders LandingPage");
            VerifyAreEqual("10%", driver.FindElement(By.CssSelector("[data-e2e=\"percent-source-id-5\"]")).Text, "percent orders Mobile");
            VerifyAreEqual("10%", driver.FindElement(By.CssSelector("[data-e2e=\"percent-source-id-6\"]")).Text, "percent orders Phone");
            VerifyAreEqual("3%", driver.FindElement(By.CssSelector("[data-e2e=\"percent-source-id-7\"]")).Text, "percent orders LiveChat");
            VerifyAreEqual("3%", driver.FindElement(By.CssSelector("[data-e2e=\"percent-source-id-8\"]")).Text, "percent orders SocialNetworks");
            VerifyAreEqual("3%", driver.FindElement(By.CssSelector("[data-e2e=\"percent-source-id-9\"]")).Text, "percent orders FindCheaper");
            VerifyAreEqual("3%", driver.FindElement(By.CssSelector("[data-e2e=\"percent-source-id-10\"]")).Text, "percent orders AbandonedCart");
                      
            VerifyFinally(testname);
        }
        [Test]
        public void MainPageOrders()
        {
            GoToAdmin();
            testname = "MainPageOrders";
            VerifyBegin(testname);
            ScrollTo(By.TagName("footer"));

            VerifyAreEqual("1", GetGridCell(0, "№", "AllOrders").Text, "check first item num");
            VerifyAreEqual("7", GetGridCell(6, "№", "AllOrders").Text, "check last item num");
            VerifyAreEqual(" Новый", GetGridCell(0, "StatusName", "AllOrders").Text, "check first item status");
            VerifyAreEqual(" Отправлен", GetGridCell(6, "StatusName", "AllOrders").Text, "check last item status");
            VerifyIsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 7, "count all orders row");

            driver.FindElement(By.CssSelector("[heading=\"Назначенные мне\"]")).Click();

            VerifyAreEqual("1", GetGridCell(0, "№", "MyOrders").Text, "check my first item num");
            VerifyAreEqual("5", GetGridCell(4, "№", "MyOrders").Text, "check my last item num");
            VerifyAreEqual(" Новый", GetGridCell(0, "StatusName", "MyOrders").Text, "check first my item status");
            VerifyAreEqual(" В обработке", GetGridCell(4, "StatusName", "MyOrders").Text, "check last my item status");
            VerifyIsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 5, "count my orders row");

            driver.FindElement(By.CssSelector("[heading=\"Не назначенные\"]")).Click();

            VerifyAreEqual("10", GetGridCell(0, "№", "NotMyOrders").Text, "check alien first item num");
            VerifyAreEqual("16", GetGridCell(6, "№", "NotMyOrders").Text, "check alien last item num");
            VerifyAreEqual(" Доставлен", GetGridCell(0, "StatusName", "NotMyOrders").Text, "check first alien item status");
            VerifyAreEqual(" Отменён", GetGridCell(6, "StatusName", "NotMyOrders").Text, "check last alien item status");
            VerifyIsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 7, "count alien orders row");

            driver.FindElement(By.CssSelector("[heading=\"Все заказы\"]")).Click();

            VerifyAreEqual("1", GetGridCell(0, "№", "AllOrders").Text, "check first item num");
            VerifyAreEqual("7", GetGridCell(6, "№", "AllOrders").Text, "check last item num");
            VerifyAreEqual(" Новый", GetGridCell(0, "StatusName", "AllOrders").Text, "check first item status");
            VerifyAreEqual(" Отправлен", GetGridCell(6, "StatusName", "AllOrders").Text, "check last item status");
            VerifyIsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 7, "count all orders row 2");

            VerifyFinally(testname);
        }
        [Test]
        public void MainPagePlannedSales()
        {
            GoToAdmin();
            testname = "MainPageTotalSales";
            VerifyBegin(testname);
            VerifyAreEqual("210 руб.", driver.FindElement(By.CssSelector("[data-e2e=\"totalSales\"]")).Text, "total sales");
            VerifyAreEqual("Цель: 200 000 руб.", driver.FindElement(By.CssSelector("[data-e2e=\"plannedSales\"]")).Text, "planned sales");

            GoToAdmin("settings#?indexTab=plan");
            driver.FindElement(By.Id("SalesPlan")).Clear();
            driver.FindElement(By.Id("SalesPlan")).SendKeys("1000000");

            driver.FindElement(By.Id("ProfitPlan")).Clear();
            driver.FindElement(By.Id("ProfitPlan")).SendKeys("100");

            driver.FindElement(By.CssSelector("[data-e2e=\"btnSave\"] input")).Click();
            Thread.Sleep(2000);
            GoToAdmin();
            VerifyAreEqual("Цель: 1 000 000 руб.", driver.FindElement(By.CssSelector("[data-e2e=\"plannedSales\"]")).Text, "planned sales");

            VerifyFinally(testname);
        }
        }
}
