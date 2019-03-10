using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;
using System.Threading;

namespace AdvantShop.SeleniumTest.Admin.Marketing.DiscountsPriceRange
{
    [TestFixture]
    public class DiscountsPriceRangeFilterTest : BaseMultiSeleniumTest
    {
        [SetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog | ClearType.Orders);
            InitializeService.LoadData(
        "data\\Admin\\Discount\\Catalog.Product.csv",
        "data\\Admin\\Discount\\Catalog.Offer.csv",
        "data\\Admin\\Discount\\Catalog.Category.csv",
        "data\\Admin\\Discount\\Catalog.ProductCategories.csv",
         "Data\\Admin\\Discount\\[Order].OrderSource.csv",
         "data\\Admin\\Discount\\[Order].OrderStatus.csv",
          "data\\Admin\\Discount\\[Order].OrderPriceDiscount.csv"
          );

            Init();
            GoToAdmin("discountspricerange");
        }

        [Test]
        public void FilterByPriceRange()
        {
            testname = "FilterByPriceRange";
            VerifyBegin(testname);

            //check filter 
            Functions.GridFilterSet(driver, baseURL, name: "PriceRange");

            //search by not exist 
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("50000");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "no such element");

            //search too much symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("111111111122222222222222222222222222222");
            //VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "too much symbols");
            DropFocus("h1");
            VerifyAreEqual("rgb(241, 89, 89)", driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).GetCssValue("border-top-color"), "too much symbols");

            //search invalid symbols
            GoToAdmin("discountspricerange");
            Functions.GridFilterSet(driver, baseURL, name: "PriceRange");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("########@@@@@@@@&&&&&&&******");
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Text, "invalid symbols");

            //search by exist
            GoToAdmin("discountspricerange");
            Functions.GridFilterSet(driver, baseURL, name: "PriceRange");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("11");

            VerifyAreEqual("11", GetGridCell(0, "PriceRange").FindElement(By.TagName("input")).GetAttribute("value"), "filter PriceRange line 1");
            VerifyAreEqual("118", GetGridCell(9, "PriceRange").FindElement(By.TagName("input")).GetAttribute("value"), "filter PriceRange line 10");
            VerifyAreEqual("Найдено записей: 11", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter PriceRange count");

            //check go to edit and back 
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(4000);
            WaitForElem(By.CssSelector(".modal-content"));

            VerifyAreEqual("Скидка из стоимости заказа", driver.FindElement(By.TagName("h2")).Text, "pop up h2");
            XPathContainsText("button", "Отмена");

            VerifyAreEqual("Найдено записей: 11", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter PriceRange return");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"PriceRange\"]")).Displayed);

            //check delete with filter
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete filtered items");

            //check delete filter
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterClose(driver, baseURL, name: "PriceRange");
            VerifyAreEqual("Найдено записей: 159", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter PriceRange deleting 1");

            GoToAdmin("discountspricerange");
            VerifyAreEqual("Найдено записей: 159", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter PriceRange deleting 2");

            VerifyFinally(testname);
        }



        [Test]
        public void FilterByPercentDiscount()
        {
            testname = "FilterByPercentDiscount";
            VerifyBegin(testname);

            //check filter 
            Functions.GridFilterSet(driver, baseURL, name: "PercentDiscount");

            //search by not exist 
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("50000");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "no such element");

            //search too much symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("111111111122222222222222222222222222222");
            //VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "too much symbols");
            DropFocus("h1");
            VerifyAreEqual("rgb(241, 89, 89)", driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).GetCssValue("border-top-color"), "too much symbols");

            //search invalid symbols
            GoToAdmin("discountspricerange");
            Functions.GridFilterSet(driver, baseURL, name: "PercentDiscount");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("########@@@@@@@@&&&&&&&******");
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Text, "invalid symbols");
            
            //search by exist
            GoToAdmin("discountspricerange");
            Functions.GridFilterSet(driver, baseURL, name: "PercentDiscount");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("20");

            VerifyAreEqual("20", GetGridCell(0, "PercentDiscount").FindElement(By.TagName("input")).GetAttribute("value"), "filter PercentDiscount line 1");
            VerifyAreEqual("20", GetGridCell(1, "PercentDiscount").FindElement(By.TagName("input")).GetAttribute("value"), "filter PercentDiscount line 2");
            VerifyAreEqual("30", GetGridCell(0, "PriceRange").FindElement(By.TagName("input")).GetAttribute("value"), "filter PriceRange line 1");
            VerifyAreEqual("120", GetGridCell(1, "PriceRange").FindElement(By.TagName("input")).GetAttribute("value"), "filter PriceRange line 2");
            VerifyAreEqual("Найдено записей: 2", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter PercentDiscount count");

            //check go to edit and back 
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(4000);
            WaitForElem(By.CssSelector(".modal-content"));

            VerifyAreEqual("Скидка из стоимости заказа", driver.FindElement(By.TagName("h2")).Text, "pop up h2");
            XPathContainsText("button", "Отмена");

            VerifyAreEqual("Найдено записей: 2", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter PercentDiscount return");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"PercentDiscount\"]")).Displayed);

            //check delete with filter
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete filtered items");

            //check delete filter
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterClose(driver, baseURL, name: "PercentDiscount");
            VerifyAreEqual("Найдено записей: 168", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter PercentDiscount deleting 1");

            GoToAdmin("discountspricerange");
            VerifyAreEqual("Найдено записей: 168", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter PercentDiscount deleting 2");

            VerifyFinally(testname);
        }

    }
}
