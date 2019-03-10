using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.Indicators
{
    [TestFixture]
    public class IndicatorsOrdersMainPageAdminTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog | ClearType.CMS | ClearType.CRM | ClearType.Orders);
            InitializeService.LoadData(
           "data\\Admin\\MainPage\\IndicatorsTest\\Catalog.Product.csv",
           "data\\Admin\\MainPage\\IndicatorsTest\\Catalog.Offer.csv",
           "data\\Admin\\MainPage\\IndicatorsTest\\Catalog.Category.csv",
           "data\\Admin\\MainPage\\IndicatorsTest\\Catalog.ProductCategories.csv",
               "data\\Admin\\MainPage\\IndicatorsTest\\CMS.Review.csv",
                           "data\\Admin\\MainPage\\IndicatorsTest\\[Order].OrderContact.csv",
                           "data\\Admin\\MainPage\\IndicatorsTest\\[Order].OrderCurrency.csv",
                            "data\\Admin\\MainPage\\IndicatorsTest\\[Order].OrderItems.csv",
                          "data\\Admin\\MainPage\\IndicatorsTest\\[Order].OrderSource.csv",
                          "data\\Admin\\MainPage\\IndicatorsTest\\[Order].OrderStatus.csv",
                             "data\\Admin\\MainPage\\IndicatorsTest\\[Order].[Order].csv",
                            "data\\Admin\\MainPage\\IndicatorsTest\\[Order].LeadItem.csv",
                            "data\\Admin\\MainPage\\IndicatorsTest\\[Order].LeadCurrency.csv",
                            "data\\Admin\\MainPage\\IndicatorsTest\\[Order].Lead.csv"
            );
             
            Init();
        }
        
        /*orders today tests*/
        [Test]
        public void IndicatorsNoBeforeOrdersToday()  //no indicators before
        {
            GoToAdmin();
            Functions.IndicatorsNoBeforeMainPageAdmin(driver, baseURL);
            driver.FindElement(By.CssSelector("[data-e2e-no-indicators=\"IndicatorsShow\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"OrdersTodayCount\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Заказов сегодня", driver.FindElement(By.CssSelector("[data-e2e-dashboard=\"OrdersTodayCount\"]")).Text);
            Assert.AreEqual("10", driver.FindElement(By.CssSelector("[data-e2e-dashboard-content=\"OrdersTodayCount\"]")).Text);
            Assert.AreEqual("55 руб.", driver.FindElement(By.CssSelector("[data-e2e-dashboard-quantity=\"OrdersTodayCount\"]")).Text);
        }

        [Test]
        public void IndicatorsOrdersToday()  //выбор индикатора через ссылку в верхнем правом углу
        {
            GoToAdmin();
            driver.FindElement(By.CssSelector("[data-e2e-indicators=\"IndicatorsShow\"]")).Click();
            Thread.Sleep(2000);
            if (
                driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"OrdersTodayCount\"]")).Selected
                )
            {
                driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
                Thread.Sleep(2000);
                Assert.AreEqual("Заказов сегодня", driver.FindElement(By.CssSelector("[data-e2e-dashboard=\"OrdersTodayCount\"]")).Text);
                Assert.AreEqual("10", driver.FindElement(By.CssSelector("[data-e2e-dashboard-content=\"OrdersTodayCount\"]")).Text);
                Assert.AreEqual("55 руб.", driver.FindElement(By.CssSelector("[data-e2e-dashboard-quantity=\"OrdersTodayCount\"]")).Text);
            }
            else
            {
                driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"OrdersTodayCount\"]")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
                Thread.Sleep(2000);
                Assert.AreEqual("Заказов сегодня", driver.FindElement(By.CssSelector("[data-e2e-dashboard=\"OrdersTodayCount\"]")).Text);
                Assert.AreEqual("10", driver.FindElement(By.CssSelector("[data-e2e-dashboard-content=\"OrdersTodayCount\"]")).Text);
                Assert.AreEqual("55 руб.", driver.FindElement(By.CssSelector("[data-e2e-dashboard-quantity=\"OrdersTodayCount\"]")).Text);
            }
        }
       
        /*orders yesterday tests*/
        [Test]
        public void IndicatorsNoBeforeOrdersYesterday()  //no indicators before
        {
            GoToAdmin();
            Functions.IndicatorsNoBeforeMainPageAdmin(driver, baseURL);
            driver.FindElement(By.CssSelector("[data-e2e-no-indicators=\"IndicatorsShow\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"OrdersYesterdayCount\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Заказов вчера", driver.FindElement(By.CssSelector("[data-e2e-dashboard=\"OrdersYesterdayCount\"]")).Text);
            Assert.AreEqual("10", driver.FindElement(By.CssSelector("[data-e2e-dashboard-content=\"OrdersYesterdayCount\"]")).Text);
            Assert.AreEqual("155 руб.", driver.FindElement(By.CssSelector("[data-e2e-dashboard-quantity=\"OrdersYesterdayCount\"]")).Text);
        }

        [Test]
        public void IndicatorsOrdersYesterday()  //выбор индикатора через ссылку в верхнем правом углу
        {
            GoToAdmin();
            driver.FindElement(By.CssSelector("[data-e2e-indicators=\"IndicatorsShow\"]")).Click();
            Thread.Sleep(2000);
            if
                (
                driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"OrdersYesterdayCount\"]")).Selected
                )
            {
                driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
                Thread.Sleep(2000);
                Assert.AreEqual("Заказов вчера", driver.FindElement(By.CssSelector("[data-e2e-dashboard=\"OrdersYesterdayCount\"]")).Text);
                Assert.AreEqual("10", driver.FindElement(By.CssSelector("[data-e2e-dashboard-content=\"OrdersYesterdayCount\"]")).Text);
                Assert.AreEqual("155 руб.", driver.FindElement(By.CssSelector("[data-e2e-dashboard-quantity=\"OrdersYesterdayCount\"]")).Text);
            }
            else
            {
                driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"OrdersYesterdayCount\"]")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
                Thread.Sleep(2000);
                Assert.AreEqual("Заказов вчера", driver.FindElement(By.CssSelector("[data-e2e-dashboard=\"OrdersYesterdayCount\"]")).Text);
                Assert.AreEqual("10", driver.FindElement(By.CssSelector("[data-e2e-dashboard-content=\"OrdersYesterdayCount\"]")).Text);
                Assert.AreEqual("155 руб.", driver.FindElement(By.CssSelector("[data-e2e-dashboard-quantity=\"OrdersYesterdayCount\"]")).Text);
            }
        }
      
        /*orders month tests*/
        [Test]
        public void IndicatorsNoBeforeOrdersMonth()  //no indicators before
        {
            GoToAdmin();
            Functions.IndicatorsNoBeforeMainPageAdmin(driver, baseURL);
            driver.FindElement(By.CssSelector("[data-e2e-no-indicators=\"IndicatorsShow\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"OrdersMonthCount\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Заказов за месяц", driver.FindElement(By.CssSelector("[data-e2e-dashboard=\"OrdersMonthCount\"]")).Text);
            Assert.AreEqual("20", driver.FindElement(By.CssSelector("[data-e2e-dashboard-content=\"OrdersMonthCount\"]")).Text);
            Assert.AreEqual("210 руб.", driver.FindElement(By.CssSelector("[data-e2e-dashboard-quantity=\"OrdersMonthCount\"]")).Text);
        }

        [Test]
        public void IndicatorsOrdersMonth()  //выбор индикатора через ссылку в верхнем правом углу
        {
            GoToAdmin();
            driver.FindElement(By.CssSelector("[data-e2e-indicators=\"IndicatorsShow\"]")).Click();
            Thread.Sleep(2000);
            if
                (
                driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"OrdersMonthCount\"]")).Selected
                )
            {

                driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
                Thread.Sleep(2000);
                Assert.AreEqual("Заказов за месяц", driver.FindElement(By.CssSelector("[data-e2e-dashboard=\"OrdersMonthCount\"]")).Text);
                Assert.AreEqual("20", driver.FindElement(By.CssSelector("[data-e2e-dashboard-content=\"OrdersMonthCount\"]")).Text);
                Assert.AreEqual("210 руб.", driver.FindElement(By.CssSelector("[data-e2e-dashboard-quantity=\"OrdersMonthCount\"]")).Text);
            }
            else
            {
                driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"OrdersMonthCount\"]")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
                Thread.Sleep(2000);
                Assert.AreEqual("Заказов за месяц", driver.FindElement(By.CssSelector("[data-e2e-dashboard=\"OrdersMonthCount\"]")).Text);
                Assert.AreEqual("20", driver.FindElement(By.CssSelector("[data-e2e-dashboard-content=\"OrdersMonthCount\"]")).Text);
                Assert.AreEqual("210 руб.", driver.FindElement(By.CssSelector("[data-e2e-dashboard-quantity=\"OrdersMonthCount\"]")).Text);
            }

        }
       
        /*orders all time tests*/
        [Test]
        public void IndicatorsNoBeforeOrdersAllTime()  //no indicators before
        {
            GoToAdmin();
            Functions.IndicatorsNoBeforeMainPageAdmin(driver, baseURL);
            driver.FindElement(By.CssSelector("[data-e2e-no-indicators=\"IndicatorsShow\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"OrdersAllTimeCount\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Заказов за все время", driver.FindElement(By.CssSelector("[data-e2e-dashboard=\"OrdersAllTimeCount\"]")).Text);
            Assert.AreEqual("30", driver.FindElement(By.CssSelector("[data-e2e-dashboard-content=\"OrdersAllTimeCount\"]")).Text);
            Assert.AreEqual("465 руб.", driver.FindElement(By.CssSelector("[data-e2e-dashboard-quantity=\"OrdersAllTimeCount\"]")).Text);
        }

        [Test]
        public void IndicatorsOrdersAllTime()  //выбор индикатора через ссылку в верхнем правом углу
        {
            GoToAdmin();
            driver.FindElement(By.CssSelector("[data-e2e-indicators=\"IndicatorsShow\"]")).Click();
            Thread.Sleep(2000);
            if
                (
                driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"OrdersAllTimeCount\"]")).Selected
                )
            {
                driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
                Thread.Sleep(2000);
                Assert.AreEqual("Заказов за все время", driver.FindElement(By.CssSelector("[data-e2e-dashboard=\"OrdersAllTimeCount\"]")).Text);
                Assert.AreEqual("30", driver.FindElement(By.CssSelector("[data-e2e-dashboard-content=\"OrdersAllTimeCount\"]")).Text);
                Assert.AreEqual("465 руб.", driver.FindElement(By.CssSelector("[data-e2e-dashboard-quantity=\"OrdersAllTimeCount\"]")).Text);
            }
            else
            {
                driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"OrdersAllTimeCount\"]")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
                Thread.Sleep(2000);
                Assert.AreEqual("Заказов за все время", driver.FindElement(By.CssSelector("[data-e2e-dashboard=\"OrdersAllTimeCount\"]")).Text);
                Assert.AreEqual("30", driver.FindElement(By.CssSelector("[data-e2e-dashboard-content=\"OrdersAllTimeCount\"]")).Text);
                Assert.AreEqual("465 руб.", driver.FindElement(By.CssSelector("[data-e2e-dashboard-quantity=\"OrdersAllTimeCount\"]")).Text);
            }
        }
       
    }
}