using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.Web.Site.Selenium.Test.Admin.Catalog.Reviews
{
    [TestFixture]
    public class CatalogReviewsFilterAddDateCalendarTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog | ClearType.Customers | ClearType.CMS);
            InitializeService.LoadData(
           "data\\Admin\\Catalog\\Reviews\\Catalog.Product.csv",
           "data\\Admin\\Catalog\\Reviews\\Catalog.Offer.csv",
           "data\\Admin\\Catalog\\Reviews\\Catalog.Category.csv",
           "data\\Admin\\Catalog\\Reviews\\Catalog.ProductCategories.csv",
                    "data\\Admin\\Catalog\\Reviews\\Customers.Customer.csv",
           "data\\Admin\\Catalog\\Reviews\\Customers.CustomerGroup.csv",
            "data\\Admin\\Catalog\\Reviews\\CMS.Review.csv"
           );

            Init();
        }
        
        [Test]
        public void FilterAddDateCalendarMinMax()
        {
            GoToAdmin("reviews");
            Functions.GridFilterSet(driver, baseURL, name: "AddDateFormatted");
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Clear();

            driver.FindElement(By.CssSelector("[colspan=\"5\"]")).Click();
            driver.FindElement(By.CssSelector("[colspan=\"5\"]")).Click();
            driver.FindElement(By.XPath("//span[contains(text(),\"2013\")]")).Click();
            driver.FindElement(By.XPath("//span[contains(text(),\"янв.\")]")).Click();
            driver.FindElement(By.XPath("//td[contains(text(),\"17\")]")).Click();
            driver.FindElement(By.XPath("//span[contains(text(),\"13:00\")]")).Click();
            driver.FindElement(By.XPath("//span[contains(text(),\"13:30\")]")).Click();

            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Clear();
            driver.FindElements(By.CssSelector("[colspan=\"5\"]"))[1].Click();
            driver.FindElements(By.CssSelector("[colspan=\"5\"]"))[1].Click();
            driver.FindElement(By.XPath("//span[contains(text(),\"2013\")]")).Click();
            driver.FindElements(By.CssSelector("[colspan=\"7\"]"))[0].FindElement(By.XPath("//span[contains(text(),\"дек.\")]")).Click();
            driver.FindElement(By.XPath("//ui-grid-custom-filter-block/div/div[2]/ng-include/div/div[2]/div/div/ul/div/table/tbody/tr[2]/td[6]")).Click();
            driver.FindElement(By.XPath("//span[contains(text(),\"13:00\")]")).Click();
            driver.FindElement(By.XPath("//span[contains(text(),\"13:30\")]")).Click();
            WaitForAjax();
            Assert.AreEqual("Найдено записей: 210", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);
        }

        [Test]
        public void FilterAddDateCalendarNotExistMin()
        {
            GoToAdmin("reviews");

            Functions.GridFilterSet(driver, baseURL, name: "AddDateFormatted");
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Clear();
     
            driver.FindElement(By.CssSelector("[colspan=\"5\"]")).Click();
            driver.FindElement(By.CssSelector("[colspan=\"5\"]")).Click();
            driver.FindElement(By.XPath("//span[contains(text(),\"2018\")]")).Click();
            driver.FindElement(By.XPath("//span[contains(text(),\"сент.\")]")).Click();
            driver.FindElement(By.XPath("//td[contains(text(),\"17\")]")).Click();
            driver.FindElement(By.XPath("//span[contains(text(),\"13:00\")]")).Click();
            driver.FindElement(By.XPath("//span[contains(text(),\"13:30\")]")).Click();
            WaitForAjax();
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));
        }

        [Test]
        public void FilterAddDateCalendarNotExistMax()
        {
            GoToAdmin("reviews");

            Functions.GridFilterSet(driver, baseURL, name: "AddDateFormatted");
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Clear();
            driver.FindElements(By.CssSelector("[colspan=\"5\"]"))[1].Click();
            driver.FindElements(By.CssSelector("[colspan=\"5\"]"))[1].Click();
            driver.FindElement(By.XPath("//span[contains(text(),\"2018\")]")).Click();
            driver.FindElements(By.CssSelector("[colspan=\"7\"]"))[0].FindElement(By.XPath("//span[contains(text(),\"дек.\")]")).Click();
            driver.FindElement(By.XPath("//ui-grid-custom-filter-block/div/div[2]/ng-include/div/div[2]/div/div/ul/div/table/tbody/tr[2]/td[5]")).Click();
            driver.FindElement(By.XPath("//span[contains(text(),\"13:00\")]")).Click();
            driver.FindElement(By.XPath("//span[contains(text(),\"13:30\")]")).Click();
            WaitForAjax();

            Assert.AreEqual("Найдено записей: 300", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);
        }

        [Test]
        public void FilterAddDateCalendarNotExistMinMax()
        {
            GoToAdmin("reviews");

            Functions.GridFilterSet(driver, baseURL, name: "AddDateFormatted");
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Clear();

            driver.FindElement(By.CssSelector("[colspan=\"5\"]")).Click();
            driver.FindElement(By.CssSelector("[colspan=\"5\"]")).Click();
            driver.FindElement(By.XPath("//span[contains(text(),\"2017\")]")).Click();
            driver.FindElement(By.XPath("//span[contains(text(),\"сент.\")]")).Click();
            driver.FindElement(By.XPath("//td[contains(text(),\"17\")]")).Click();
            driver.FindElement(By.XPath("//span[contains(text(),\"13:00\")]")).Click();
            driver.FindElement(By.XPath("//span[contains(text(),\"13:30\")]")).Click();

            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Clear();

            driver.FindElements(By.CssSelector("[colspan=\"5\"]"))[1].Click();
            driver.FindElements(By.CssSelector("[colspan=\"5\"]"))[1].Click();
            driver.FindElement(By.XPath("//span[contains(text(),\"2018\")]")).Click();
            driver.FindElements(By.CssSelector("[colspan=\"7\"]"))[0].FindElement(By.XPath("//span[contains(text(),\"дек.\")]")).Click();
            driver.FindElement(By.XPath("//ui-grid-custom-filter-block/div/div[2]/ng-include/div/div[2]/div/div/ul/div/table/tbody/tr[2]/td[5]")).Click();
            driver.FindElement(By.XPath("//span[contains(text(),\"13:00\")]")).Click();
            driver.FindElement(By.XPath("//span[contains(text(),\"13:30\")]")).Click();

            WaitForAjax();
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));
        }
    }
}