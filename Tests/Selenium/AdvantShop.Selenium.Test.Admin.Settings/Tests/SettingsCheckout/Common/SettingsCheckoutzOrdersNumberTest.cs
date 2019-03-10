using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using AdvantShop.Selenium.Core.Infrastructure;
using System.Collections.ObjectModel;

namespace AdvantShop.Selenium.Test.Admin.Settings.Tests.SettingsCheckout.Common
{
    [TestFixture]
    public class SettingsCheckoutzOrdersNumberTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(
                       "Data\\Admin\\Settings\\SettingCheckout\\Catalog.Product.csv",
                       "Data\\Admin\\Settings\\SettingCheckout\\Catalog.Offer.csv",
                       "Data\\Admin\\Settings\\SettingCheckout\\Catalog.Category.csv",
                       "Data\\Admin\\Settings\\SettingCheckout\\Catalog.ProductCategories.csv"
                );
            Init();
        }
        [Test]
        public void ChangeNumOrder()
        {
            testname = "ChangeNumOrder";
            VerifyBegin(testname);
            GoToAdmin("settingscheckout");
            ScrollTo(By.CssSelector("[data-e2e=\"NextOrderNumber\"]"));

            driver.FindElement(By.Id("NextOrderNumber")).Clear();
            driver.FindElement(By.Id("NextOrderNumber")).SendKeys("1111");
            driver.FindElement(By.Id("OrderNumberFormat")).Clear();
            driver.FindElement(By.Id("OrderNumberFormat")).SendKeys("#NUMBER#");

            driver.FindElement(By.CssSelector("[data-e2e=\"NextOrderNumber\"]")).Click();
            Thread.Sleep(2000);
            ScrollTo(By.Id("header-top"));
            try
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCheckoutSave\"]")).Click();
            }
            catch { }
            Functions.NewOrderClient_450(driver, baseURL);
            VerifyAreEqual("Ваш заказ принят под номером 1111", driver.FindElement(By.CssSelector(".congrat-num")).Text, "num 1 order");
            Functions.NewOrderClient_450(driver, baseURL);
            VerifyAreEqual("Ваш заказ принят под номером 1112", driver.FindElement(By.CssSelector(".congrat-num")).Text, "num 2 order");

            GoToAdmin("settingscheckout");
            ScrollTo(By.CssSelector("[data-e2e=\"NextOrderNumber\"]"));
            VerifyAreEqual("1113", driver.FindElement(By.Id("NextOrderNumber")).GetAttribute("value"), "num next order");

            VerifyFinally(testname);
        }
        [Test]
        public void ChangeNumOrderUseFormat()
        {
            testname = "ChangeNumOrderUseFormat";
            VerifyBegin(testname);
            GoToAdmin("settingscheckout");
            ScrollTo(By.CssSelector("[data-e2e=\"NextOrderNumber\"]"));

            driver.FindElement(By.Id("NextOrderNumber")).Clear();
            driver.FindElement(By.Id("NextOrderNumber")).SendKeys("2222");
            driver.FindElement(By.CssSelector("[data-e2e=\"NextOrderNumber\"]")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.Id("OrderNumberFormat")).Clear();
            driver.FindElement(By.Id("OrderNumberFormat")).SendKeys("Order:#NUMBER#.#YEAR#.#MONTH#.#DAY#.");
            ScrollTo(By.Id("header-top"));
            try
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCheckoutSave\"]")).Click();
            }
            catch { }
            Functions.NewOrderClient_450(driver, baseURL);
            string s = System.String.Format("Ваш заказ принят под номером Order:2222.{0}.{1}.{2}.", DateTime.Now.ToString("yy"),DateTime.Now.ToString("MM"), DateTime.Now.ToString("dd"));
            VerifyAreEqual(s , driver.FindElement(By.CssSelector(".congrat-num")).Text, "num 1 order");
            Functions.NewOrderClient_450(driver, baseURL);
            s = System.String.Format("Ваш заказ принят под номером Order:2223.{0}.{1}.{2}.", DateTime.Now.ToString("yy"), DateTime.Now.ToString("MM"), DateTime.Now.ToString("dd"));            
            VerifyAreEqual(s, driver.FindElement(By.CssSelector(".congrat-num")).Text, "num 2 order");

            GoToAdmin("settingscheckout");
            ScrollTo(By.CssSelector("[data-e2e=\"NextOrderNumber\"]"));
            driver.FindElement(By.Id("OrderNumberFormat")).Clear();
            driver.FindElement(By.Id("OrderNumberFormat")).SendKeys("#NUMBER#");
            ScrollTo(By.Id("header-top"));
            try
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCheckoutSave\"]")).Click();
            }
            catch { }
            VerifyFinally(testname);
        }
    }
}
