using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.Indicators
{
    [TestFixture]
    public class IndicatorsCallsMainPageAdminTest : BaseSeleniumTest
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
        
        /*calls today tests*/
        [Test]
        public void IndicatorsNoBeforeCallsToday()  //no indicators before
        {
            GoToAdmin();
            Functions.IndicatorsNoBeforeMainPageAdmin(driver, baseURL); 
            driver.FindElement(By.CssSelector("[data-e2e-no-indicators=\"IndicatorsShow\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"CallsTodayCount\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Звонков сегодня", driver.FindElement(By.CssSelector("[data-e2e-dashboard=\"CallsTodayCount\"]")).Text);
            Assert.AreEqual("0", driver.FindElement(By.CssSelector("[data-e2e-dashboard-content=\"CallsTodayCount\"]")).Text);
            Assert.AreEqual("Настроить телефонию", driver.FindElement(By.CssSelector("[data-e2e-dashboard-service=\"ConnectServiceCallsToday\"] a")).Text);
        }

        [Test]
        public void IndicatorsCallsToday()  //выбор индикатора через ссылку в верхнем правом углу
        {
            GoToAdmin();
            driver.FindElement(By.CssSelector("[data-e2e-indicators=\"IndicatorsShow\"]")).Click();
            Thread.Sleep(2000);
            if
                (
                driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"CallsTodayCount\"]")).Selected
                )

            {
                driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
                Thread.Sleep(2000);
                Assert.AreEqual("Звонков сегодня", driver.FindElement(By.CssSelector("[data-e2e-dashboard=\"CallsTodayCount\"]")).Text);
                Assert.AreEqual("0", driver.FindElement(By.CssSelector("[data-e2e-dashboard-content=\"CallsTodayCount\"]")).Text);
                Assert.AreEqual("Настроить телефонию", driver.FindElement(By.CssSelector("[data-e2e-dashboard-service=\"ConnectServiceCallsToday\"] a")).Text);
                string linkToday = driver.FindElement(By.CssSelector("[data-e2e-dashboard-service=\"ConnectServiceCallsToday\"] a")).GetAttribute("href");
                Assert.IsTrue(linkToday.Contains("adminv2/settingstelephony"));
            }
            else
            {
                driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"CallsTodayCount\"]")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
                Thread.Sleep(2000);
                Assert.AreEqual("Звонков сегодня", driver.FindElement(By.CssSelector("[data-e2e-dashboard=\"CallsTodayCount\"]")).Text);
                Assert.AreEqual("0", driver.FindElement(By.CssSelector("[data-e2e-dashboard-content=\"CallsTodayCount\"]")).Text);
                Assert.AreEqual("Настроить телефонию", driver.FindElement(By.CssSelector("[data-e2e-dashboard-service=\"ConnectServiceCallsToday\"] a")).Text);
                string linkToday = driver.FindElement(By.CssSelector("[data-e2e-dashboard-service=\"ConnectServiceCallsToday\"] a")).GetAttribute("href");
                Assert.IsTrue(linkToday.Contains("adminv2/settingstelephony"));
            }
        }
       
        /*calls yesterday tests*/
        [Test]
        public void IndicatorsNoBeforeCallsYesterday()  //no indicators before
        {
            GoToAdmin();
            Functions.IndicatorsNoBeforeMainPageAdmin(driver, baseURL);
            driver.FindElement(By.CssSelector("[data-e2e-no-indicators=\"IndicatorsShow\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"CallsYesterdayCount\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Звонков вчера", driver.FindElement(By.CssSelector("[data-e2e-dashboard=\"CallsYesterdayCount\"]")).Text);
            Assert.AreEqual("0", driver.FindElement(By.CssSelector("[data-e2e-dashboard-content=\"CallsYesterdayCount\"]")).Text);
            Assert.AreEqual("Настроить телефонию", driver.FindElement(By.CssSelector("[data-e2e-dashboard-service=\"ConnectServiceCallsYesterday\"] a")).Text);
        }

        [Test]
        public void IndicatorsCallsYesterday()  //выбор индикатора через ссылку в верхнем правом углу
        {
            GoToAdmin();
            driver.FindElement(By.CssSelector("[data-e2e-indicators=\"IndicatorsShow\"]")).Click();
            Thread.Sleep(2000);
            if
                (
                driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"CallsYesterdayCount\"]")).Selected
                )
            {
                driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
                Thread.Sleep(2000);
                Assert.AreEqual("Звонков вчера", driver.FindElement(By.CssSelector("[data-e2e-dashboard=\"CallsYesterdayCount\"]")).Text);
                Assert.AreEqual("0", driver.FindElement(By.CssSelector("[data-e2e-dashboard-content=\"CallsYesterdayCount\"]")).Text);
                Assert.AreEqual("Настроить телефонию", driver.FindElement(By.CssSelector("[data-e2e-dashboard-service=\"ConnectServiceCallsYesterday\"] a")).Text);
                string linkYesterday = driver.FindElement(By.CssSelector("[data-e2e-dashboard-service=\"ConnectServiceCallsYesterday\"] a")).GetAttribute("href");
                Assert.IsTrue(linkYesterday.Contains("adminv2/settingstelephony"));
            }
            else
            {
                driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"CallsYesterdayCount\"]")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
                Thread.Sleep(2000);
                Assert.AreEqual("Звонков вчера", driver.FindElement(By.CssSelector("[data-e2e-dashboard=\"CallsYesterdayCount\"]")).Text);
                Assert.AreEqual("0", driver.FindElement(By.CssSelector("[data-e2e-dashboard-content=\"CallsYesterdayCount\"]")).Text);
                Assert.AreEqual("Настроить телефонию", driver.FindElement(By.CssSelector("[data-e2e-dashboard-service=\"ConnectServiceCallsYesterday\"] a")).Text);
                string linkYesterday = driver.FindElement(By.CssSelector("[data-e2e-dashboard-service=\"ConnectServiceCallsYesterday\"] a")).GetAttribute("href");
                Assert.IsTrue(linkYesterday.Contains("adminv2/settingstelephony"));
            }
        }
       
        /*calls month tests*/
        [Test]
        public void IndicatorsNoBeforeCallsMonth()  //no indicators before
        {
            GoToAdmin();
            Functions.IndicatorsNoBeforeMainPageAdmin(driver, baseURL);
            driver.FindElement(By.CssSelector("[data-e2e-no-indicators=\"IndicatorsShow\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"CallsMonthCount\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Звонков за месяц", driver.FindElement(By.CssSelector("[data-e2e-dashboard=\"CallsMonthCount\"]")).Text);
            Assert.AreEqual("0", driver.FindElement(By.CssSelector("[data-e2e-dashboard-content=\"CallsMonthCount\"]")).Text);
            Assert.AreEqual("Настроить телефонию", driver.FindElement(By.CssSelector("[data-e2e-dashboard-service=\"ConnectServiceCallsMonth\"] a")).Text);
        }

        [Test]
        public void IndicatorsCallsMonth()  //выбор индикатора через ссылку в верхнем правом углу
        {
            GoToAdmin();
            driver.FindElement(By.CssSelector("[data-e2e-indicators=\"IndicatorsShow\"]")).Click();
            Thread.Sleep(2000);
            if
                (
                driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"CallsMonthCount\"]")).Selected
                )
            {
                driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
                Thread.Sleep(2000);
                Assert.AreEqual("Звонков за месяц", driver.FindElement(By.CssSelector("[data-e2e-dashboard=\"CallsMonthCount\"]")).Text);
                Assert.AreEqual("0", driver.FindElement(By.CssSelector("[data-e2e-dashboard-content=\"CallsMonthCount\"]")).Text);
                Assert.AreEqual("Настроить телефонию", driver.FindElement(By.CssSelector("[data-e2e-dashboard-service=\"ConnectServiceCallsMonth\"] a")).Text);
                string linkMonth = driver.FindElement(By.CssSelector("[data-e2e-dashboard-service=\"ConnectServiceCallsMonth\"] a")).GetAttribute("href");
                Assert.IsTrue(linkMonth.Contains("adminv2/settingstelephony"));
            }
            else
            {
                driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"CallsMonthCount\"]")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
                Thread.Sleep(2000);
                Assert.AreEqual("Звонков за месяц", driver.FindElement(By.CssSelector("[data-e2e-dashboard=\"CallsMonthCount\"]")).Text);
                Assert.AreEqual("0", driver.FindElement(By.CssSelector("[data-e2e-dashboard-content=\"CallsMonthCount\"]")).Text);
                Assert.AreEqual("Настроить телефонию", driver.FindElement(By.CssSelector("[data-e2e-dashboard-service=\"ConnectServiceCallsMonth\"] a")).Text);
                string linkMonth = driver.FindElement(By.CssSelector("[data-e2e-dashboard-service=\"ConnectServiceCallsMonth\"] a")).GetAttribute("href");
                Assert.IsTrue(linkMonth.Contains("adminv2/settingstelephony"));
            }
        }
       
    }
}