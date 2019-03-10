using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.Indicators
{
    [TestFixture]
    public class IndicatorsLeadsMainPageAdminTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Orders | ClearType.Catalog | ClearType.CMS | ClearType.CRM);
            InitializeService.LoadData(
           "data\\Admin\\MainPage\\IndicatorsTest\\Catalog.Product.csv",
           "data\\Admin\\MainPage\\IndicatorsTest\\Catalog.Offer.csv",
           "data\\Admin\\MainPage\\IndicatorsTest\\Catalog.Category.csv",
           "data\\Admin\\MainPage\\IndicatorsTest\\Catalog.ProductCategories.csv",
            "data\\Admin\\MainPage\\IndicatorsTest\\[Order].OrderContact.csv",
            "data\\Admin\\MainPage\\IndicatorsTest\\[Order].OrderCurrency.csv",
             "data\\Admin\\MainPage\\IndicatorsTest\\[Order].OrderItems.csv",
             "data\\Admin\\MainPage\\IndicatorsTest\\[Order].OrderSource.csv",
             "data\\Admin\\MainPage\\IndicatorsTest\\[Order].OrderStatus.csv",
             "data\\Admin\\MainPage\\IndicatorsTest\\[Order].[Order].csv",
             "data\\Admin\\MainPage\\IndicatorsTest\\[Order].LeadItem.csv",
             "data\\Admin\\MainPage\\IndicatorsTest\\[Order].LeadCurrency.csv",
             "data\\Admin\\MainPage\\IndicatorsTest\\[Order].Lead.csv",
               "data\\Admin\\MainPage\\IndicatorsTest\\CMS.Review.csv"
            );
             
            Init();
        }
        
        /*leads today tests*/
        [Test]
        public void IndicatorsNoBeforeLeadsToday()  //no indicators before
        {
            GoToAdmin();
            Functions.IndicatorsNoBeforeMainPageAdmin(driver, baseURL);
            driver.FindElement(By.CssSelector("[data-e2e-no-indicators=\"IndicatorsShow\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"LeadsTodayCount\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Лиды сегодня", driver.FindElement(By.CssSelector("[data-e2e-dashboard=\"LeadsTodayCount\"]")).Text);
            Assert.AreEqual("1", driver.FindElement(By.CssSelector("[data-e2e-dashboard-content=\"LeadsTodayCount\"]")).Text);
            Assert.AreEqual("10 руб.", driver.FindElement(By.CssSelector("[data-e2e-dashboard-quantity=\"LeadsTodayCount\"]")).Text);
        }

        [Test]
        public void IndicatorsLeadsToday()  //выбор индикатора через ссылку в верхнем правом углу
        {
            GoToAdmin();
            driver.FindElement(By.CssSelector("[data-e2e-indicators=\"IndicatorsShow\"]")).Click();
            Thread.Sleep(2000);
            if
                (
                driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"LeadsTodayCount\"]")).Selected
                )
            {
                driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
                Thread.Sleep(2000);
                Assert.AreEqual("Лиды сегодня", driver.FindElement(By.CssSelector("[data-e2e-dashboard=\"LeadsTodayCount\"]")).Text);
                Assert.AreEqual("1", driver.FindElement(By.CssSelector("[data-e2e-dashboard-content=\"LeadsTodayCount\"]")).Text);
                Assert.AreEqual("10 руб.", driver.FindElement(By.CssSelector("[data-e2e-dashboard-quantity=\"LeadsTodayCount\"]")).Text);
            }
            else
            {
                driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"LeadsTodayCount\"]")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
                Thread.Sleep(2000);
                Assert.AreEqual("Лиды сегодня", driver.FindElement(By.CssSelector("[data-e2e-dashboard=\"LeadsTodayCount\"]")).Text);
                Assert.AreEqual("1", driver.FindElement(By.CssSelector("[data-e2e-dashboard-content=\"LeadsTodayCount\"]")).Text);
                Assert.AreEqual("10 руб.", driver.FindElement(By.CssSelector("[data-e2e-dashboard-quantity=\"LeadsTodayCount\"]")).Text);
            }
        }
       
        /*leads yesterday tests*/
        [Test]
        public void IndicatorsNoBeforeLeadsYesterday()  //no indicators before
        {
            GoToAdmin();
            Functions.IndicatorsNoBeforeMainPageAdmin(driver, baseURL);
            driver.FindElement(By.CssSelector("[data-e2e-no-indicators=\"IndicatorsShow\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"LeadsYesterdayCount\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Лиды вчера", driver.FindElement(By.CssSelector("[data-e2e-dashboard=\"LeadsYesterdayCount\"]")).Text);
            Assert.AreEqual("2", driver.FindElement(By.CssSelector("[data-e2e-dashboard-content=\"LeadsYesterdayCount\"]")).Text);
            Assert.AreEqual("23 руб.", driver.FindElement(By.CssSelector("[data-e2e-dashboard-quantity=\"LeadsYesterdayCount\"]")).Text);
        }

        [Test]
        public void IndicatorsLeadsYesterday()  //выбор индикатора через ссылку в верхнем правом углу
        {
            GoToAdmin();
            driver.FindElement(By.CssSelector("[data-e2e-indicators=\"IndicatorsShow\"]")).Click();
            Thread.Sleep(2000);
            if
                (
                driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"LeadsYesterdayCount\"]")).Selected
                )
            {
                driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
                Thread.Sleep(2000);
                Assert.AreEqual("Лиды вчера", driver.FindElement(By.CssSelector("[data-e2e-dashboard=\"LeadsYesterdayCount\"]")).Text);
                Assert.AreEqual("2", driver.FindElement(By.CssSelector("[data-e2e-dashboard-content=\"LeadsYesterdayCount\"]")).Text);
                Assert.AreEqual("23 руб.", driver.FindElement(By.CssSelector("[data-e2e-dashboard-quantity=\"LeadsYesterdayCount\"]")).Text);
            }
            else
            {
                driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"LeadsYesterdayCount\"]")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
                Thread.Sleep(2000);
                Assert.AreEqual("Лиды вчера", driver.FindElement(By.CssSelector("[data-e2e-dashboard=\"LeadsYesterdayCount\"]")).Text);
                Assert.AreEqual("2", driver.FindElement(By.CssSelector("[data-e2e-dashboard-content=\"LeadsYesterdayCount\"]")).Text);
                Assert.AreEqual("23 руб.", driver.FindElement(By.CssSelector("[data-e2e-dashboard-quantity=\"LeadsYesterdayCount\"]")).Text);
            }
        }
      
        /*leads month tests*/
        [Test]
        public void IndicatorsNoBeforeLeadsMonth()  //no indicators before
        {
            GoToAdmin();
            Functions.IndicatorsNoBeforeMainPageAdmin(driver, baseURL);
            driver.FindElement(By.CssSelector("[data-e2e-no-indicators=\"IndicatorsShow\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"LeadsMonthCount\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Лиды за месяц", driver.FindElement(By.CssSelector("[data-e2e-dashboard=\"LeadsMonthCount\"]")).Text);
            Assert.AreEqual("3", driver.FindElement(By.CssSelector("[data-e2e-dashboard-content=\"LeadsMonthCount\"]")).Text);
            Assert.AreEqual("33 руб.", driver.FindElement(By.CssSelector("[data-e2e-dashboard-quantity=\"LeadsMonthCount\"]")).Text);
        }

        [Test]
        public void IndicatorsLeadsMonth()  //выбор индикатора через ссылку в верхнем правом углу
        {
            GoToAdmin();
            driver.FindElement(By.CssSelector("[data-e2e-indicators=\"IndicatorsShow\"]")).Click();
            Thread.Sleep(2000);
            if
                (
                driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"LeadsMonthCount\"]")).Selected
                )
            {
                driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
                Thread.Sleep(2000);
                Assert.AreEqual("Лиды за месяц", driver.FindElement(By.CssSelector("[data-e2e-dashboard=\"LeadsMonthCount\"]")).Text);
                Assert.AreEqual("3", driver.FindElement(By.CssSelector("[data-e2e-dashboard-content=\"LeadsMonthCount\"]")).Text);
                Assert.AreEqual("33 руб.", driver.FindElement(By.CssSelector("[data-e2e-dashboard-quantity=\"LeadsMonthCount\"]")).Text);
            }
            else
            {
                driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"LeadsMonthCount\"]")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
                Thread.Sleep(2000);
                Assert.AreEqual("Лиды за месяц", driver.FindElement(By.CssSelector("[data-e2e-dashboard=\"LeadsMonthCount\"]")).Text);
                Assert.AreEqual("3", driver.FindElement(By.CssSelector("[data-e2e-dashboard-content=\"LeadsMonthCount\"]")).Text);
                Assert.AreEqual("33 руб.", driver.FindElement(By.CssSelector("[data-e2e-dashboard-quantity=\"LeadsMonthCount\"]")).Text);
            }
        }
      
    }
}