using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.Indicators
{
    [TestFixture]
    public class IndicatorsReviewsMainPageAdminTest : BaseSeleniumTest
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
        
        /*reviews today tests*/
        [Test]
        public void IndicatorsNoBeforeReviewsToday()  //no indicators before
        {
            GoToAdmin();
            Functions.IndicatorsNoBeforeMainPageAdmin(driver, baseURL);
            driver.FindElement(By.CssSelector("[data-e2e-no-indicators=\"IndicatorsShow\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"ReviewsTodayCount\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Отзывов сегодня", driver.FindElement(By.CssSelector("[data-e2e-dashboard=\"ReviewsTodayCount\"]")).Text);
            Assert.AreEqual("20", driver.FindElement(By.CssSelector("[data-e2e-dashboard-content=\"ReviewsTodayCount\"]")).Text);
        }

        [Test]
        public void IndicatorsReviewsToday()  //выбор индикатора через ссылку в верхнем правом углу
        {
            GoToAdmin();
            driver.FindElement(By.CssSelector("[data-e2e-indicators=\"IndicatorsShow\"]")).Click();
            Thread.Sleep(2000);
            if
                (
                driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"ReviewsTodayCount\"]")).Selected
                )
            {
                driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
                Thread.Sleep(2000);
                Assert.AreEqual("Отзывов сегодня", driver.FindElement(By.CssSelector("[data-e2e-dashboard=\"ReviewsTodayCount\"]")).Text);
                Assert.AreEqual("20", driver.FindElement(By.CssSelector("[data-e2e-dashboard-content=\"ReviewsTodayCount\"]")).Text);
            }
            else
            {
                driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"ReviewsTodayCount\"]")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
                Thread.Sleep(2000);
                Assert.AreEqual("Отзывов сегодня", driver.FindElement(By.CssSelector("[data-e2e-dashboard=\"ReviewsTodayCount\"]")).Text);
                Assert.AreEqual("20", driver.FindElement(By.CssSelector("[data-e2e-dashboard-content=\"ReviewsTodayCount\"]")).Text);
            }  
        }
       
        /*reviews yesterday tests*/
        [Test]
        public void IndicatorsNoBeforeReviewsYesterday()  //no indicators before
        {
            GoToAdmin();
            Functions.IndicatorsNoBeforeMainPageAdmin(driver, baseURL);
            driver.FindElement(By.CssSelector("[data-e2e-no-indicators=\"IndicatorsShow\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"ReviewsYesterdayCount\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Отзывов вчера", driver.FindElement(By.CssSelector("[data-e2e-dashboard=\"ReviewsYesterdayCount\"]")).Text);
            Assert.AreEqual("31", driver.FindElement(By.CssSelector("[data-e2e-dashboard-content=\"ReviewsYesterdayCount\"]")).Text);
        }

        [Test]
        public void IndicatorsReviewsYesterday()  //выбор индикатора через ссылку в верхнем правом углу
        {
            GoToAdmin();
            driver.FindElement(By.CssSelector("[data-e2e-indicators=\"IndicatorsShow\"]")).Click();
            Thread.Sleep(2000);
            if
                (
                driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"ReviewsYesterdayCount\"]")).Selected
                )

            {
                driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
                Thread.Sleep(2000);
                Assert.AreEqual("Отзывов вчера", driver.FindElement(By.CssSelector("[data-e2e-dashboard=\"ReviewsYesterdayCount\"]")).Text);
                Assert.AreEqual("31", driver.FindElement(By.CssSelector("[data-e2e-dashboard-content=\"ReviewsYesterdayCount\"]")).Text);
            }
            else
            {
                driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"ReviewsYesterdayCount\"]")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
                Thread.Sleep(2000);
                Assert.AreEqual("Отзывов вчера", driver.FindElement(By.CssSelector("[data-e2e-dashboard=\"ReviewsYesterdayCount\"]")).Text);
                Assert.AreEqual("31", driver.FindElement(By.CssSelector("[data-e2e-dashboard-content=\"ReviewsYesterdayCount\"]")).Text);
            }
        }
       
        /*reviews month tests*/
        [Test]
        public void IndicatorsNoBeforeReviewsMonth()  //no indicators before
        {
            GoToAdmin();
            Functions.IndicatorsNoBeforeMainPageAdmin(driver, baseURL);
            driver.FindElement(By.CssSelector("[data-e2e-no-indicators=\"IndicatorsShow\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"ReviewsMonthCount\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Отзывы за месяц", driver.FindElement(By.CssSelector("[data-e2e-dashboard=\"ReviewsMonthCount\"]")).Text);
            Assert.AreEqual("51", driver.FindElement(By.CssSelector("[data-e2e-dashboard-content=\"ReviewsMonthCount\"]")).Text);
        }

        [Test]
        public void IndicatorsReviewsMonth()  //выбор индикатора через ссылку в верхнем правом углу
        {
            GoToAdmin();
            driver.FindElement(By.CssSelector("[data-e2e-indicators=\"IndicatorsShow\"]")).Click();
            Thread.Sleep(2000);
            if
                (
                driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"ReviewsMonthCount\"]")).Selected
                )
            {
                driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
                Thread.Sleep(2000);
                Assert.AreEqual("Отзывы за месяц", driver.FindElement(By.CssSelector("[data-e2e-dashboard=\"ReviewsMonthCount\"]")).Text);
                Assert.AreEqual("51", driver.FindElement(By.CssSelector("[data-e2e-dashboard-content=\"ReviewsMonthCount\"]")).Text);
            }
            else
            {
                driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"ReviewsMonthCount\"]")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
                Thread.Sleep(2000);
                Assert.AreEqual("Отзывы за месяц", driver.FindElement(By.CssSelector("[data-e2e-dashboard=\"ReviewsMonthCount\"]")).Text);
                Assert.AreEqual("51", driver.FindElement(By.CssSelector("[data-e2e-dashboard-content=\"ReviewsMonthCount\"]")).Text);
            }
        }
    }
}