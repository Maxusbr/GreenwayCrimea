using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.CMS.News
{
    [TestFixture]
    public class CMSNewsFilterEnabledTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.CMS);
            InitializeService.LoadData(
                "data\\Admin\\CMS\\News\\Settings.News.csv",
                "data\\Admin\\CMS\\News\\Settings.NewsCategory.csv"
                );

            Init();
        }


        [Test]
        public void NewsFilterEnabled()
        {
            GoToAdmin("news");

            //filter enabled
            Functions.GridFilterSet(driver, baseURL, name: "Enabled");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Активные");
            DropFocus("h1");
            Assert.AreEqual("title Test News 145", GetGridCell(0, "Title").Text);
            Assert.AreEqual("title Test News 31", GetGridCell(9, "Title").Text);
            Assert.IsTrue(GetGridCell(0, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);
            Assert.IsTrue(GetGridCell(9, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);
            Assert.AreEqual("Найдено записей: 48", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Неактивные");
            DropFocus("h1");
            Assert.AreEqual("title Test News 131", GetGridCell(0, "Title").Text);
            Assert.AreEqual("title Test News 115", GetGridCell(9, "Title").Text);
            Assert.IsFalse(GetGridCell(0, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);
            Assert.IsFalse(GetGridCell(9, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);
            Assert.AreEqual("Найдено записей: 102", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);

            //check delete filter
            Functions.GridFilterClose(driver, baseURL, name: "Enabled");
            Assert.AreEqual("Найдено записей: 150", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);
        }


        [Test]
        public void NewzFilterEnabledDelete()
        {
            GoToAdmin("news");

            Functions.GridFilterSet(driver, baseURL, name: "Enabled");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Неактивные");
            DropFocus("h1");

            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(500);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Thread.Sleep(500);
            Functions.GridDropdownDelete(driver, baseURL);

            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));

            Functions.GridFilterClose(driver, baseURL, name: "Enabled");

            Assert.AreEqual("Найдено записей: 48", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);

            GoToAdmin("news");

            Assert.AreEqual("Найдено записей: 48", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);
        }
    }
}
