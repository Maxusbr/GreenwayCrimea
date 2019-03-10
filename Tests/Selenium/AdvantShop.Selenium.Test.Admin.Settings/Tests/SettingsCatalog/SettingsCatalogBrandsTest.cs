using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Settings.SettingsCatalog.ProductCart
{
    [TestFixture]
    public class SettingsCatalogBrandsTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();

            Init();
        }

        [Test]
        public void BrandsPerPage()
        {
            testname = "SettingsCatalogBrandsPerPage";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=brand");

            driver.FindElement(By.Id("BrandsPerPage")).Click();
            driver.FindElement(By.Id("BrandsPerPage")).Clear();
            driver.FindElement(By.Id("BrandsPerPage")).SendKeys("3");

            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCatalogSave\"]")).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("settingscatalog#?catalogTab=brand");
            VerifyAreEqual("3", driver.FindElement(By.Id("BrandsPerPage")).GetAttribute("value"), "brands per page value admin");

            //check client
            GoToClient("manufacturers");
            VerifyIsTrue(driver.FindElements(By.CssSelector(".brand-name")).Count == 3, "brands per page value client");

            VerifyFinally(testname);
        }

        [Test]
        public void ShowProductsInBrandOn()
        {
            testname = "SettingsCatalogBrandsShowProductsInBrandOn";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=brand");

            checkSelected("ShowProductsInBrand");

            GoToClient("manufacturers/advanced-micro-devices-amd");

            VerifyIsTrue(driver.PageSource.Contains("Список товаров бренда AMD"), "show products in brand h1");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".products-view-name-link")).Displayed, "show products in brand");

            VerifyFinally(testname);
        }

        [Test]
        public void ShowProductsInBrandOff()
        {
            testname = "SettingsCatalogBrandsShowProductsInBrandOff";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=brand");

            checkNotSelected("ShowProductsInBrand");

            GoToClient("manufacturers/advanced-micro-devices-amd");

            VerifyIsFalse(driver.PageSource.Contains("Список товаров бренда AMD"), "show products in brand h1");
            VerifyIsFalse(driver.FindElements(By.CssSelector(".products-view-name-link")).Count > 0, "show products in brand");

            VerifyFinally(testname);
        }

        [Test]
        public void ShowCategoryTreeInBrandOn()
        {
            testname = "SettingsCatalogBrandsShowCategoryTreeInBrandOn";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=brand");

            checkSelected("ShowCategoryTreeInBrand");

            GoToClient("manufacturers/apple");
            
            VerifyIsTrue(driver.FindElement(By.CssSelector(".menu-dropdown.menu-dropdown-accordion.menu-dropdown-expanded")).Displayed, "show category tree in brand");

            VerifyFinally(testname);
        }

        [Test]
        public void ShowCategoryTreeInBrandOff()
        {
            testname = "SettingsCatalogBrandsShowCategoryTreeInBrandOff";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=brand");

            checkNotSelected("ShowCategoryTreeInBrand");

            GoToClient("manufacturers/apple");
            
            VerifyIsFalse(driver.FindElements(By.CssSelector(".menu-dropdown.menu-dropdown-accordion.menu-dropdown-expanded")).Count > 0, "show category tree in brand");

            VerifyFinally(testname);
        }
    }
}