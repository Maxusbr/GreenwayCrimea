using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.Indicators
{
    [TestFixture]
    public class IndicatorsProductsMainPageAdminTest : BaseSeleniumTest
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

        /*products tests*/
        [Test]
        public void IndicatorsNoBeforeProducts()  //no indicators before
        {
            GoToAdmin();
            Functions.IndicatorsNoBeforeMainPageAdmin(driver, baseURL);
            driver.FindElement(By.CssSelector("[data-e2e-no-indicators=\"IndicatorsShow\"]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Индикаторы", driver.FindElement(By.CssSelector("[data-e2e-header=\"IndicatorsTitle\"]")).Text);
            driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"ProductsCount\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Всего товаров", driver.FindElement(By.CssSelector("[data-e2e-dashboard=\"ProductsCount\"]")).Text);
            Assert.AreEqual("100", driver.FindElement(By.CssSelector("[data-e2e-dashboard-content=\"ProductsCount\"]")).Text);
            Assert.AreEqual("штук", driver.FindElement(By.CssSelector("[data-e2e-dashboard-quantity=\"ProductsCount\"]")).Text);
        }

        [Test]
        public void IndicatorsProducts()  //открытие pop up с индикаторами через ссылку в верхнем правом углу
        {
            GoToAdmin();
            driver.FindElement(By.CssSelector("[data-e2e-indicators=\"IndicatorsShow\"]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Индикаторы", driver.FindElement(By.CssSelector("[data-e2e-header=\"IndicatorsTitle\"]")).Text);
            if (
            driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"ProductsCount\"]")).Selected
            )
            {
                driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
                Thread.Sleep(2000);
                Assert.AreEqual("Всего товаров", driver.FindElement(By.CssSelector("[data-e2e-dashboard=\"ProductsCount\"]")).Text);
                Assert.AreEqual("100", driver.FindElement(By.CssSelector("[data-e2e-dashboard-content=\"ProductsCount\"]")).Text);
                Assert.AreEqual("штук", driver.FindElement(By.CssSelector("[data-e2e-dashboard-quantity=\"ProductsCount\"]")).Text);
            }
            else
            {
                driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"ProductsCount\"]")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
                Thread.Sleep(2000);
                Assert.AreEqual("Всего товаров", driver.FindElement(By.CssSelector("[data-e2e-dashboard=\"ProductsCount\"]")).Text);
                Assert.AreEqual("100", driver.FindElement(By.CssSelector("[data-e2e-dashboard-content=\"ProductsCount\"]")).Text);
                Assert.AreEqual("штук", driver.FindElement(By.CssSelector("[data-e2e-dashboard-quantity=\"ProductsCount\"]")).Text);
            }
        }
        
        /* close indicators pop up */
        [Test]
        public void IndicatorsNoBeforeNotChooseClose()   //no indicators before and do not choose
        {
            GoToAdmin();
            Functions.IndicatorsNoBeforeMainPageAdmin(driver, baseURL);
            driver.FindElement(By.CssSelector("[data-e2e-no-indicators=\"IndicatorsShow\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//button[contains(text(), 'Отмена')]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Выбрать индикаторы", driver.FindElement(By.CssSelector("[data-e2e-no-indicators=\"IndicatorsShow\"]")).Text);
        }

        /* more than 1 indicators */
        [Test]
        public void IndicatorsNoBeforeChooseMoreThanOne()   //no indicators before and do not choose
        {
            GoToAdmin();
            Functions.IndicatorsNoBeforeMainPageAdmin(driver, baseURL);
            driver.FindElement(By.CssSelector("[data-e2e-no-indicators=\"IndicatorsShow\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"ProductsCount\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"LeadsYesterdayCount\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"OrdersTodayCount\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(2000);

            Assert.AreEqual("Заказов сегодня", driver.FindElement(By.CssSelector("[data-e2e-dashboard=\"OrdersTodayCount\"]")).Text);
            Assert.AreEqual("Всего товаров", driver.FindElement(By.CssSelector("[data-e2e-dashboard=\"ProductsCount\"]")).Text);
            Assert.AreEqual("Лиды вчера", driver.FindElement(By.CssSelector("[data-e2e-dashboard=\"LeadsYesterdayCount\"]")).Text);
        }
    }
}