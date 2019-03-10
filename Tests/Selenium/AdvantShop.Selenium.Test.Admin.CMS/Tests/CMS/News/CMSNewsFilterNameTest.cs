using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.CMS.News
{
    [TestFixture]
    public class CMSNewsFilterNameTest : BaseSeleniumTest
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
        public void NewsFilterName()
        {
            GoToAdmin("news");

            //search by exist name 
            Functions.GridFilterSet(driver, baseURL, name: "Title");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("title Test News 113");
            DropFocus("h1");
            Assert.AreEqual("title Test News 113", GetGridCell(0, "Title").Text);
            Assert.AreEqual("Найдено записей: 1", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);

            //search by not exist name
            GoToAdmin("news");

            Functions.GridFilterSet(driver, baseURL, name: "Title");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("123123123 News 3");
            DropFocus("h1");
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));

            //search too much symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("1111111111222222222222222222333333333333334444444444rrrrrrrrrrrreeeeeeeeewwwwwwwwwwwwwwwwwww");
            DropFocus("h1");
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));

            //search invalid symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("########@@@@@@@@&&&&&&&******,,,,..");
            DropFocus("h1");
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));

            //check delete filter
            Functions.GridFilterClose(driver, baseURL, name: "Title");
            Assert.AreEqual("Найдено записей: 150", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);
        }


        [Test]
        public void NewzFilterNameDelete()
        {
            GoToAdmin("news");

            Functions.GridFilterSet(driver, baseURL, name: "Title");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("12");
            DropFocus("h1");

            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(500);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Thread.Sleep(500);
            Functions.GridDropdownDelete(driver, baseURL);

            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));

            Functions.GridFilterClose(driver, baseURL, name: "Title");

            Assert.AreEqual("Найдено записей: 138", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);

            GoToAdmin("news");

            Assert.AreEqual("Найдено записей: 138", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);
        }
    }
}
