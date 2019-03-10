using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;
using System.Threading;

namespace AdvantShop.SeleniumTest.Admin.Settings.SettingsSEO
{
    [TestFixture]
    public class SettingsSeoDefaultTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.SettingsProductsPerPage);
            InitializeService.LoadData(

           "data\\Admin\\Settings\\SettingsSeo\\Settings.Settings.csv"

           );

            Init();
        }

        [Test]
        public void DefaultSeoMain()
        {
            testname = "DefaultSeoMain";
            VerifyBegin(testname);

            GoToAdmin("settingsseo");

            driver.FindElement(By.Id("DefaultTitle")).Click();
            driver.FindElement(By.Id("DefaultTitle")).Clear();
            driver.FindElement(By.Id("DefaultTitle")).SendKeys("New title Main Page");

            driver.FindElement(By.Id("DefaultMetaKeywords")).Click();
            driver.FindElement(By.Id("DefaultMetaKeywords")).Clear();
            driver.FindElement(By.Id("DefaultMetaKeywords")).SendKeys("New meta keywords 1 Main Page, New meta keywords 2 Main Page, New meta keywords 3 Main Page");

            driver.FindElement(By.Id("DefaultMetaDescription")).Click();
            driver.FindElement(By.Id("DefaultMetaDescription")).Clear();
            driver.FindElement(By.Id("DefaultMetaDescription")).SendKeys("New description Main Page");

            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"settingsSeoSave\"]")).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("settingsseo");

            VerifyAreEqual("New title Main Page", driver.FindElement(By.Id("DefaultTitle")).GetAttribute("value"), "default seo main title admin");
            VerifyAreEqual("New meta keywords 1 Main Page, New meta keywords 2 Main Page, New meta keywords 3 Main Page", driver.FindElement(By.Id("DefaultMetaKeywords")).GetAttribute("value"), "default seo main keywords admin");
            VerifyAreEqual("New description Main Page", driver.FindElement(By.Id("DefaultMetaDescription")).GetAttribute("value"), "default seo main description admin");

            //check client
            GoToClient();
            VerifyAreEqual("New title Main Page", driver.Title, "default seo main title client");
            VerifyAreEqual("New meta keywords 1 Main Page, New meta keywords 2 Main Page, New meta keywords 3 Main Page", driver.FindElement(By.CssSelector("[name=\"Keywords\"]")).GetAttribute("content"), "default seo main keywords client");
            VerifyAreEqual("New description Main Page", driver.FindElement(By.CssSelector("[name=\"Description\"]")).GetAttribute("content"), "default seo main description client");

            VerifyFinally(testname);
        }

        [Test]
        public void DefaultSeoMainVariables()
        {
            testname = "DefaultSeoMainVariables";
            VerifyBegin(testname);

            GoToAdmin("settingsseo");

            driver.FindElement(By.Id("DefaultTitle")).Click();
            driver.FindElement(By.Id("DefaultTitle")).Clear();
            driver.FindElement(By.Id("DefaultTitle")).SendKeys("#STORE_NAME# - #STORE_NAME#");

            driver.FindElement(By.Id("DefaultMetaKeywords")).Click();
            driver.FindElement(By.Id("DefaultMetaKeywords")).Clear();
            driver.FindElement(By.Id("DefaultMetaKeywords")).SendKeys("#STORE_NAME# - #STORE_NAME#");

            driver.FindElement(By.Id("DefaultMetaDescription")).Click();
            driver.FindElement(By.Id("DefaultMetaDescription")).Clear();
            driver.FindElement(By.Id("DefaultMetaDescription")).SendKeys("#STORE_NAME# - #STORE_NAME#");

            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"settingsSeoSave\"]")).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("settingsseo");

            VerifyAreEqual("#STORE_NAME# - #STORE_NAME#", driver.FindElement(By.Id("DefaultTitle")).GetAttribute("value"), "default seo main title admin");
            VerifyAreEqual("#STORE_NAME# - #STORE_NAME#", driver.FindElement(By.Id("DefaultMetaKeywords")).GetAttribute("value"), "default seo main keywords admin");
            VerifyAreEqual("#STORE_NAME# - #STORE_NAME#", driver.FindElement(By.Id("DefaultMetaDescription")).GetAttribute("value"), "default seo main description admin");

            //check client
            GoToClient();
            VerifyAreEqual("Мой магазин - Мой магазин", driver.Title, "default seo main title client");
            VerifyAreEqual("Мой магазин - Мой магазин", driver.FindElement(By.CssSelector("[name=\"Keywords\"]")).GetAttribute("content"), "default seo main keywords client");
            VerifyAreEqual("Мой магазин - Мой магазин", driver.FindElement(By.CssSelector("[name=\"Description\"]")).GetAttribute("content"), "default seo main description client");

            VerifyFinally(testname);
        }

        [Test]
        public void DefaultSeoInstruction()
        {
            testname = "DefaultSeoInstruction";
            VerifyBegin(testname);

            GoToAdmin("settingsseo");

            driver.FindElement(By.CssSelector("[data-e2e=\"metaInstruction\"]")).Click();
            Thread.Sleep(4000);

            Functions.OpenNewTab(driver, baseURL);
            VerifyIsTrue(driver.Url.Contains("help") && driver.Url.Contains("seo-module"), "default seo instruction");
            Functions.CloseTab(driver, baseURL);

            VerifyFinally(testname);
        }
        
    }
}