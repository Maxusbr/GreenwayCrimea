using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;

namespace AdvantShop.SeleniumTest.Admin.CMS.News
{
    [TestFixture]
    public class CMSNewsFilterAddDateTest : BaseSeleniumTest
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
        public void NewsFilterAddDate()
        {
            GoToAdmin("news");

            //set filter
            Functions.GridFilterSet(driver, baseURL, name: "AddingDateFormatted");
            WaitForElem(By.CssSelector("[data-e2e-grid-filter-block-name=\"AddingDateFormatted\"]"));

            driver.FindElement(By.Id("gridFilterDateFrom")).FindElement(By.TagName("input")).Click();
            driver.FindElement(By.Id("gridFilterDateFrom")).FindElement(By.TagName("input")).Clear();
            driver.FindElement(By.Id("gridFilterDateFrom")).FindElement(By.TagName("input")).SendKeys("01.01.2012 00:00");
            driver.FindElement(By.Id("gridFilterDateFrom")).FindElement(By.TagName("input")).Click();
            driver.FindElement(By.Id("gridFilterDateTo")).FindElement(By.TagName("input")).Click();
            driver.FindElement(By.Id("gridFilterDateTo")).FindElement(By.TagName("input")).Clear();
            driver.FindElement(By.Id("gridFilterDateTo")).FindElement(By.TagName("input")).SendKeys("31.12.2013 00:00");
            driver.FindElement(By.Id("gridFilterDateTo")).FindElement(By.TagName("input")).Click();

            Assert.AreEqual("Найдено записей: 75", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);


            //check delete filter
            Functions.GridFilterClose(driver, baseURL, name: "AddingDateFormatted");
            Assert.AreEqual("Найдено записей: 150", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);
        }

        [Test]
        public void NewsFilterAddDateByCalen()
        {
            GoToAdmin("news");

            //set filter
            Functions.GridFilterSet(driver, baseURL, name: "AddingDateFormatted");
            WaitForElem(By.CssSelector("[data-e2e-grid-filter-block-name=\"AddingDateFormatted\"]"));

            driver.FindElement(By.Id("gridFilterDateFrom")).FindElement(By.TagName("input")).Click();
            driver.FindElement(By.Id("gridFilterDateFrom")).FindElement(By.TagName("input")).Clear();
            driver.FindElement(By.CssSelector("[colspan=\"5\"]")).Click();
            driver.FindElement(By.CssSelector("[colspan=\"5\"]")).Click();
            driver.FindElement(By.XPath("//span[contains(text(),\"2012\")]")).Click();
            driver.FindElement(By.XPath("//span[contains(text(),\"янв.\")]")).Click();
            driver.FindElement(By.XPath("//td[contains(text(),\"17\")]")).Click();
            driver.FindElement(By.XPath("//span[contains(text(),\"00:00\")]")).Click();
            driver.FindElement(By.XPath("//span[contains(text(),\"00:00\")]")).Click();
            
            driver.FindElement(By.Id("gridFilterDateTo")).FindElement(By.TagName("input")).Click();
            driver.FindElement(By.Id("gridFilterDateTo")).FindElement(By.TagName("input")).Clear();
            //   driver.FindElement(By.CssSelector(".dropdown.col-sm-11.open")).FindElement(By.CssSelector("[colspan=\"5\"]")).Click();
            //  driver.FindElement(By.CssSelector(".dropdown.col-sm-11.open")).FindElement(By.CssSelector("[colspan=\"5\"]")).Click();
            driver.FindElements(By.CssSelector("[colspan=\"5\"]"))[1].Click();
            driver.FindElements(By.CssSelector("[colspan=\"5\"]"))[1].Click();
            driver.FindElement(By.XPath("//span[contains(text(),\"2013\")]")).Click();
            driver.FindElements(By.CssSelector("[colspan=\"7\"]"))[0].FindElement(By.XPath("//span[contains(text(),\"дек.\")]")).Click();
            driver.FindElement(By.XPath("//ui-grid-custom-filter-block/div/div[2]/ng-include/div/div[2]/div/div/ul/div/table/tbody/tr[2]/td[6]")).Click();
            //   driver.FindElement(By.CssSelector(".dropdown.col-sm-11.open")).FindElement(By.XPath("//span[contains(text(),\"00:00\")]")).Click();
            //  driver.FindElement(By.CssSelector(".dropdown.col-sm-11.open")).FindElement(By.XPath("//span[contains(text(),\"00:00\")]")).Click();
            driver.FindElement(By.XPath("//span[contains(text(),\"00:00\")]")).Click();
            driver.FindElement(By.XPath("//span[contains(text(),\"00:00\")]")).Click();

            Assert.AreEqual("Найдено записей: 75", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);


            //check delete filter
            Functions.GridFilterClose(driver, baseURL, name: "AddingDateFormatted");
            Assert.AreEqual("Найдено записей: 150", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);
        }


        [Test]
        public void NewsFilterAddDateInvalid()
        {
            GoToAdmin("news");

            //set filter
            Functions.GridFilterSet(driver, baseURL, name: "AddingDateFormatted");
            WaitForElem(By.CssSelector("[data-e2e-grid-filter-block-name=\"AddingDateFormatted\"]"));

            driver.FindElement(By.Id("gridFilterDateFrom")).FindElement(By.TagName("input")).Click();
            driver.FindElement(By.Id("gridFilterDateFrom")).FindElement(By.TagName("input")).Clear();
            driver.FindElement(By.Id("gridFilterDateFrom")).FindElement(By.TagName("input")).SendKeys("01.01.2015 00:00");
            driver.FindElement(By.Id("gridFilterDateFrom")).FindElement(By.TagName("input")).Click();
            driver.FindElement(By.Id("gridFilterDateTo")).FindElement(By.TagName("input")).Click();
            driver.FindElement(By.Id("gridFilterDateTo")).FindElement(By.TagName("input")).Clear();
            driver.FindElement(By.Id("gridFilterDateTo")).FindElement(By.TagName("input")).SendKeys("31.12.2016 00:00");
            driver.FindElement(By.Id("gridFilterDateTo")).FindElement(By.TagName("input")).Click();

            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));


            //check delete filter
            Functions.GridFilterClose(driver, baseURL, name: "AddingDateFormatted");
            Assert.AreEqual("Найдено записей: 150", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);
        }

        [Test]
        public void NewzFilterAddDateDelete()
        {
            GoToAdmin("news");

            Functions.GridFilterSet(driver, baseURL, name: "AddingDateFormatted");
            WaitForElem(By.CssSelector("[data-e2e-grid-filter-block-name=\"AddingDateFormatted\"]"));

            driver.FindElement(By.Id("gridFilterDateFrom")).FindElement(By.TagName("input")).Click();
            driver.FindElement(By.Id("gridFilterDateFrom")).FindElement(By.TagName("input")).Clear();
            driver.FindElement(By.Id("gridFilterDateFrom")).FindElement(By.TagName("input")).SendKeys("01.01.2012 00:00");
            driver.FindElement(By.Id("gridFilterDateFrom")).FindElement(By.TagName("input")).Click();
            driver.FindElement(By.Id("gridFilterDateTo")).FindElement(By.TagName("input")).Click();
            driver.FindElement(By.Id("gridFilterDateTo")).FindElement(By.TagName("input")).Clear();
            driver.FindElement(By.Id("gridFilterDateTo")).FindElement(By.TagName("input")).SendKeys("31.12.2013 00:00");
            driver.FindElement(By.Id("gridFilterDateTo")).FindElement(By.TagName("input")).Click();

            Assert.AreEqual("Найдено записей: 75", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);

            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(500);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Thread.Sleep(500);
            Functions.GridDropdownDelete(driver, baseURL);

            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));

            Functions.GridFilterClose(driver, baseURL, name: "AddingDateFormatted");

            Assert.AreEqual("Найдено записей: 75", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);

            GoToAdmin("news");

            Assert.AreEqual("Найдено записей: 75", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);
        }
    }
}
