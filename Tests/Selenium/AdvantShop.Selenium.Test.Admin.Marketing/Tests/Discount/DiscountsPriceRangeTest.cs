using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Marketing.DiscountsPriceRange
{
    [TestFixture]
    public class DiscountsPriceRangeTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
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
        public void Grid()
        {
            testname = "DiscountGrid";
            VerifyBegin(testname);

            VerifyAreEqual("Найдено записей: 170", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "grid count all");
            VerifyAreEqual("11", GetGridCell(0, "PriceRange").FindElement(By.TagName("input")).GetAttribute("value"), "grid discount price range");
            VerifyAreEqual("1", GetGridCell(0, "PercentDiscount").FindElement(By.TagName("input")).GetAttribute("value"), "grid percent discount");
            VerifyFinally(testname);
        }

       
        [Test]
        public void GoToEdit()
        {
            testname = "GoToEdit";
            VerifyBegin(testname);

            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(3000);
            WaitForElem(By.CssSelector(".modal-content"));

            VerifyAreEqual("Скидка из стоимости заказа", driver.FindElement(By.TagName("h2")).Text, "pop up h2");
            XPathContainsText("button", "Отмена");

            VerifyFinally(testname);
        }


        [Test]
        public void InplacePriceRange()
        {
            testname = "InplacePriceRange";
            VerifyBegin(testname);

            VerifyAreEqual("11", GetGridCell(0, "PriceRange").FindElement(By.TagName("input")).GetAttribute("value"), "pre check inplace price range");

            GetGridCell(0, "PriceRange").FindElement(By.TagName("input")).Click();
            GetGridCell(0, "PriceRange").FindElement(By.TagName("input")).Clear();
            GetGridCell(0, "PriceRange").FindElement(By.TagName("input")).SendKeys("5");
            DropFocus("h1");
            VerifyAreEqual("5", GetGridCell(0, "PriceRange").FindElement(By.TagName("input")).GetAttribute("value"), "inplace edit price range");

            GoToAdmin("discountspricerange");
            VerifyAreEqual("5", GetGridCell(0, "PriceRange").FindElement(By.TagName("input")).GetAttribute("value"), "inplace edit price range after refresh");

            //back default
            GetGridCell(0, "PriceRange").FindElement(By.TagName("input")).Click();
            GetGridCell(0, "PriceRange").FindElement(By.TagName("input")).Clear();
            GetGridCell(0, "PriceRange").FindElement(By.TagName("input")).SendKeys("11");
            DropFocus("h1");
            VerifyAreEqual("11", GetGridCell(0, "PriceRange").FindElement(By.TagName("input")).GetAttribute("value"), "back default");

            VerifyFinally(testname);
        }

        [Test]
        public void InplacePercentDiscount()
        {
            testname = "InplacePercentDiscount";
            VerifyBegin(testname);

            VerifyAreEqual("2", GetGridCell(1, "PercentDiscount").FindElement(By.TagName("input")).GetAttribute("value"), "pre check inplace percent discount");

            GetGridCell(1, "PercentDiscount").FindElement(By.TagName("input")).Click();
            GetGridCell(1, "PercentDiscount").FindElement(By.TagName("input")).Clear();
            GetGridCell(1, "PercentDiscount").FindElement(By.TagName("input")).SendKeys("1");
            DropFocus("h1");
            VerifyAreEqual("1", GetGridCell(1, "PercentDiscount").FindElement(By.TagName("input")).GetAttribute("value"), "inplace edit percent discount");

            GoToAdmin("discountspricerange");
            VerifyAreEqual("1", GetGridCell(1, "PercentDiscount").FindElement(By.TagName("input")).GetAttribute("value"), "inplace edit percent discount after refresh");
            VerifyAreEqual("1", GetGridCell(0, "PercentDiscount").FindElement(By.TagName("input")).GetAttribute("value"), "inplace edit percent discount not edited");

            //back default
            GetGridCell(1, "PercentDiscount").FindElement(By.TagName("input")).Click();
            GetGridCell(1, "PercentDiscount").FindElement(By.TagName("input")).Clear();
            GetGridCell(1, "PercentDiscount").FindElement(By.TagName("input")).SendKeys("2");
            DropFocus("h1");
            VerifyAreEqual("2", GetGridCell(1, "PercentDiscount").FindElement(By.TagName("input")).GetAttribute("value"), "back default");
            VerifyAreEqual("12", GetGridCell(1, "PriceRange").FindElement(By.TagName("input")).GetAttribute("value"), "back default price range");

            VerifyFinally(testname);
        }

        [Test]
        public void zSelectDelete()
        {
            testname = "DiscountSelectDelete";
            VerifyBegin(testname);

            //check delete cancel 
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-times.link-invert")).Click();
            driver.FindElement(By.ClassName("swal2-cancel")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("11", GetGridCell(0, "PriceRange").FindElement(By.TagName("input")).GetAttribute("value"), "1 grid canсel delete");
            VerifyAreEqual("Найдено записей: 170", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all canсel delete");

            //check delete
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-times.link-invert")).Click();
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);
            VerifyAreNotEqual("11", GetGridCell(0, "PriceRange").FindElement(By.TagName("input")).GetAttribute("value"), "1 grid delete");

            //check select 
            GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            GetGridCell(1, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            GetGridCell(2, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            VerifyIsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected 2 grid");
            VerifyIsTrue(GetGridCell(1, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected 3 grid");
            VerifyIsTrue(GetGridCell(2, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected 4 grid");
            VerifyAreEqual("3", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text, "count selected");

            //check delete selected items
            Functions.GridDropdownDelete(driver, baseURL);
            VerifyAreEqual("15", GetGridCell(0, "PriceRange").FindElement(By.TagName("input")).GetAttribute("value"), "selected 2 grid delete");
            VerifyAreEqual("16", GetGridCell(1, "PriceRange").FindElement(By.TagName("input")).GetAttribute("value"), "selected 3 grid delete");
            VerifyAreEqual("17", GetGridCell(2, "PriceRange").FindElement(By.TagName("input")).GetAttribute("value"), "selected 4 grid delete");

            //check select all on page
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            VerifyIsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected all on page 1 grid");
            VerifyIsTrue(GetGridCell(9, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected all on page 10 grid");

            //check delete all on page
            Functions.GridDropdownDelete(driver, baseURL);
            VerifyAreEqual("25", GetGridCell(0, "PriceRange").FindElement(By.TagName("input")).GetAttribute("value"), "selected all on page 2 grid delete"); //default status
            VerifyAreEqual("34", GetGridCell(9, "PriceRange").FindElement(By.TagName("input")).GetAttribute("value"), "selected all on page 10 grid delete");

            //check select all
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            VerifyAreEqual("156", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text, "count all selected after deleting");

            //check deselect all 
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDeselectAllFn\"]")).Click();
            VerifyIsTrue(!GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "deselect all 1 grid");
            VerifyIsTrue(!GetGridCell(9, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "deselect all 10 grid");

            //check delete all
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "1 delete all");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "1 count all after deleting");

            GoToAdmin("discountspricerange");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "2 delete all");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "2 count all after deleting");

            VerifyFinally(testname);
        }
    }
}
