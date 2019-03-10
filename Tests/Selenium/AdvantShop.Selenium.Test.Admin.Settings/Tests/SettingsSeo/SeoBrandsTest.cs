using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;
using System.Threading;

namespace AdvantShop.SeleniumTest.Admin.Settings.SettingsSEO
{
    [TestFixture]
    public class SettingsSeoBrandsTest : BaseSeleniumTest
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
        public void DefaultSeoBrands()
        {
            testname = "DefaultSeoBrands";
            VerifyBegin(testname);

            GoToAdmin("settingsseo");
            ScrollTo(By.Id("NewsDefaultH1"));

            driver.FindElement(By.Id("BrandsDefaultTitle")).Click();
            driver.FindElement(By.Id("BrandsDefaultTitle")).Clear();
            driver.FindElement(By.Id("BrandsDefaultTitle")).SendKeys("New title Brands");

            driver.FindElement(By.Id("BrandsDefaultMetaKeywords")).Click();
            driver.FindElement(By.Id("BrandsDefaultMetaKeywords")).Clear();
            driver.FindElement(By.Id("BrandsDefaultMetaKeywords")).SendKeys("New meta keywords 1 Brands, New meta keywords 2 Brands, New meta keywords 3 Brands");

            driver.FindElement(By.Id("BrandsDefaultMetaDescription")).Click();
            driver.FindElement(By.Id("BrandsDefaultMetaDescription")).Clear();
            driver.FindElement(By.Id("BrandsDefaultMetaDescription")).SendKeys("New description Brands");
            
            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"settingsSeoSave\"]")).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("settingsseo");

            VerifyAreEqual("New title Brands", driver.FindElement(By.Id("BrandsDefaultTitle")).GetAttribute("value"), "default seo Brands title admin");
            VerifyAreEqual("New meta keywords 1 Brands, New meta keywords 2 Brands, New meta keywords 3 Brands", driver.FindElement(By.Id("BrandsDefaultMetaKeywords")).GetAttribute("value"), "default seo Brands keywords admin");
            VerifyAreEqual("New description Brands", driver.FindElement(By.Id("BrandsDefaultMetaDescription")).GetAttribute("value"), "default seo Brands description admin");

            //check client
            GoToClient("manufacturers");
            VerifyAreEqual("New title Brands", driver.Title, "default seo Brands title client");
            VerifyAreEqual("New meta keywords 1 Brands, New meta keywords 2 Brands, New meta keywords 3 Brands", driver.FindElement(By.CssSelector("[name=\"Keywords\"]")).GetAttribute("content"), "default seo Brands keywords client");
            VerifyAreEqual("New description Brands", driver.FindElement(By.CssSelector("[name=\"Description\"]")).GetAttribute("content"), "default seo Brands description client");

            VerifyFinally(testname);
        }

        [Test]
        public void DefaultSeoBrandsVariables()
        {
            testname = "DefaultSeoBrandsVariables";
            VerifyBegin(testname);
            
            GoToAdmin("settingsseo");
            ScrollTo(By.Id("NewsDefaultH1"));

            driver.FindElement(By.Id("BrandsDefaultTitle")).Click();
            driver.FindElement(By.Id("BrandsDefaultTitle")).Clear();
            driver.FindElement(By.Id("BrandsDefaultTitle")).SendKeys("#STORE_NAME#");

            driver.FindElement(By.Id("BrandsDefaultMetaKeywords")).Click();
            driver.FindElement(By.Id("BrandsDefaultMetaKeywords")).Clear();
            driver.FindElement(By.Id("BrandsDefaultMetaKeywords")).SendKeys("#STORE_NAME#");

            driver.FindElement(By.Id("BrandsDefaultMetaDescription")).Click();
            driver.FindElement(By.Id("BrandsDefaultMetaDescription")).Clear();
            driver.FindElement(By.Id("BrandsDefaultMetaDescription")).SendKeys("#STORE_NAME#");
            
            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"settingsSeoSave\"]")).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("settingsseo");

            VerifyAreEqual("#STORE_NAME#", driver.FindElement(By.Id("BrandsDefaultTitle")).GetAttribute("value"), "default seo Brands title admin");
            VerifyAreEqual("#STORE_NAME#", driver.FindElement(By.Id("BrandsDefaultMetaKeywords")).GetAttribute("value"), "default seo Brands keywords admin");
            VerifyAreEqual("#STORE_NAME#", driver.FindElement(By.Id("BrandsDefaultMetaDescription")).GetAttribute("value"), "default seo Brands description admin");

            //check client
            GoToClient("manufacturers");
            VerifyAreEqual("Мой магазин", driver.Title, "default seo Brands title client");
            VerifyAreEqual("Мой магазин", driver.FindElement(By.CssSelector("[name=\"Keywords\"]")).GetAttribute("content"), "default seo Brands keywords client");
            VerifyAreEqual("Мой магазин", driver.FindElement(By.CssSelector("[name=\"Description\"]")).GetAttribute("content"), "default seo Brands description client");

            VerifyFinally(testname);
        }
        
    }
}