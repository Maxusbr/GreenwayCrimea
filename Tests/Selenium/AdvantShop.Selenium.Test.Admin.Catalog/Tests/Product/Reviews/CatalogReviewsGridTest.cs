using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
namespace AdvantShop.Web.Site.Selenium.Test.Admin.Catalog.Reviews
{
    [TestFixture]
    public class CatalogReviewsGridTest : BaseSeleniumTest
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
        public void GridChecked()
        {
            Functions.AdminSettingsReviewsShowImgOn(driver, baseURL);

            GoToAdmin("reviews");

            //check admin
            Assert.IsTrue(GetGridCell(0, "PhotoName").FindElement(By.TagName("img")).GetAttribute("src").Contains("nophoto"));
            Assert.IsTrue(GetGridCell(0, "Name").Text.Contains("CustomerName2"));
            Assert.IsTrue(GetGridCell(0, "Name").Text.Contains("asd@asd.asd"));
            Assert.AreEqual("Текст отзыва 30", GetGridCell(0, "Text").Text);
            Assert.AreEqual("TestProduct1", GetGridCell(0, "ProductName").Text);
            Assert.AreEqual("26.07.2015 14:22:05", GetGridCell(0, "AddDateFormatted").Text);
            Assert.IsTrue(GetGridCell(0, "Checked").FindElement(By.TagName("input")).Selected);

            //check go to product
            GetGridCell(0, "ProductName").FindElement(By.TagName("a")).Click();
            Thread.Sleep(4000);
            Assert.AreEqual("Товар \"TestProduct1\"", driver.FindElement(By.TagName("h1")).Text);

            //check client
            GoToClient("products/test-product1");
            Assert.IsTrue(driver.PageSource.Contains("CustomerName2"));
            Assert.IsTrue(driver.PageSource.Contains("Отзывы (49)"));
            Assert.IsTrue(driver.PageSource.Contains("Текст отзыва 30"));
            Assert.IsTrue(driver.FindElement(By.CssSelector(".reviews")).FindElement(By.TagName("img")).GetAttribute("src").Contains("nophoto"));
        }

        [Test]
        public void GridNotChecked()
        {
            Functions.AdminSettingsReviewsModerateOn(driver, baseURL);

            GoToAdmin("reviews");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Текст отзыва 200");

            //check admin
            Assert.IsTrue(GetGridCell(0, "PhotoName").FindElement(By.TagName("img")).GetAttribute("src").Contains("nophoto"));
            Assert.IsTrue(GetGridCell(0, "Name").Text.Contains("CustomerName2"));
            Assert.IsTrue(GetGridCell(0, "Name").Text.Contains("asd@asd.asd"));
            Assert.AreEqual("Текст отзыва 200", GetGridCell(0, "Text").Text);
            Assert.AreEqual("TestProduct10", GetGridCell(0, "ProductName").Text);
            Assert.AreEqual("04.09.2013 14:22:00", GetGridCell(0, "AddDateFormatted").Text);
            Assert.IsFalse(GetGridCell(0, "Checked").FindElement(By.TagName("input")).Selected);

            //check client
            GoToClient("products/test-product10");
            Assert.IsFalse(driver.PageSource.Contains("Отзывы (100)")); //с проверенным 100
            Assert.IsFalse(driver.PageSource.Contains("Текст отзыва 200"));
        }

        [Test]
        public void ReviewzEdit()
        {
            Functions.AdminSettingsReviewsModerateOff(driver, baseURL);

            GoToAdmin("reviews");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Текст отзыва 33");
            Blur();
            Assert.AreEqual("Текст отзыва 33", GetGridCell(0, "Text").Text);

            Assert.AreEqual("Отзывы", driver.FindElement(By.TagName("h1")).Text);
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(4000);
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewName\"]")).SendKeys("Name111");
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewEmail\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewEmail\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewEmail\"]")).SendKeys("mail@mail.ru");
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewDateAdd\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewDateAdd\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewDateAdd\"]")).SendKeys("01.01.2001 01:01:01");
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewText\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewText\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewText\"]")).SendKeys("Измененный текст отзыва");
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewCheckedClick\"]")).Click();
            driver.FindElement(By.CssSelector("input[type=\"file\"]")).Clear();
            driver.FindElement(By.CssSelector("input[type=\"file\"]")).SendKeys(GetPicturePath("icon.jpg"));

            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("reviews");
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Текст отзыва 33");
            Blur();
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Измененный текст отзыва");
            Blur();
            Assert.IsTrue(GetGridCell(0, "Name").Text.Contains("Name111"));
            Assert.AreEqual("Измененный текст отзыва", GetGridCell(0, "Text").Text);
            Assert.IsFalse(GetGridCell(0, "PhotoName").FindElement(By.TagName("img")).GetAttribute("src").Contains("nophoto"));
            Assert.IsTrue(GetGridCell(0, "Name").Text.Contains("mail@mail.ru"));
            Assert.AreEqual("TestProduct1", GetGridCell(0, "ProductName").Text);
            Assert.AreEqual("01.01.2001 01:01:01", GetGridCell(0, "AddDateFormatted").Text);
            Assert.IsFalse(GetGridCell(0, "Checked").FindElement(By.TagName("input")).Selected);

            //check client
            GoToClient("products/test-product1");
            Assert.IsTrue(driver.PageSource.Contains("Измененный текст отзыва"));
            Assert.IsTrue(driver.PageSource.Contains("Name111"));
            Assert.IsFalse(driver.PageSource.Contains("Текст отзыва 33"));
            Assert.IsTrue(driver.PageSource.Contains("1 января 2001"));
        }

        [Test]
        public void GridSearch()
        {
            GoToAdmin("reviews");

            //search by exist name
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("CustomerName3");
            Blur();

            Assert.IsTrue(GetGridCell(0, "Name").Text.Contains("CustomerName3"));
            Assert.AreEqual("Найдено записей: 100", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);
            
            //search by not exist name
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("CustomerName10");
            Blur();
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));

            //search by not review text
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Текст отзыва 500");
            Blur();
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));

            //search too much symbols
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("1111111111222222222222222222333333333333334444444444rrrrrrrrrrrreeeeeeeeewwwwwwwwwwwwwwwwwww");
            Blur();
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));

            //search invalid symbols
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("########@@@@@@@@&&&&&&&******,,,,..");
            Blur();
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));
        }

        [Test]
        public void PresentSelectOnPage()
        {
            Functions.AdminSettingsReviewsModerateOff(driver, baseURL);

            GoToAdmin("reviews");

            Assert.AreEqual("Найдено записей: 300", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            PageSelectItems("10");
            Assert.AreEqual("Текст отзыва 30", GetGridCell(0, "Text").Text);
            Assert.AreEqual("Текст отзыва 20", GetGridCell(9, "Text").Text);
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.TagName("input")).Selected);
            Assert.IsTrue(GetGridCell(9, "selectionRowHeaderCol").FindElement(By.TagName("input")).Selected);

            GoToAdmin("reviews");
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            PageSelectItems("20");
            Thread.Sleep(3000);
            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Assert.AreEqual("Текст отзыва 30", GetGridCell(0, "Text").Text);
            Assert.AreEqual("Текст отзыва 10", GetGridCell(19, "Text").Text);
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.TagName("input")).Selected);
            Assert.IsTrue(GetGridCell(19, "selectionRowHeaderCol").FindElement(By.TagName("input")).Selected);
        }

        [Test]
        public void zDelete()
        {
            Functions.AdminSettingsReviewsModerateOff(driver, baseURL);

            GoToAdmin("reviews");
            gridReturnDefaultView10();

            //check delete cancel 
            GetGridCell(0, "_serviceColumn").FindElement(By.TagName("ui-grid-custom-delete")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-cancel")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Текст отзыва 30", GetGridCell(0, "Text").Text);

            //check delete
            GetGridCell(0, "_serviceColumn").FindElement(By.TagName("ui-grid-custom-delete")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);
            GoToAdmin("reviews");
            Assert.AreNotEqual("Текст отзыва 30", GetGridCell(0, "Text").Text);

            //check select 
            GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            GetGridCell(1, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            GetGridCell(2, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(1, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(2, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.AreEqual("3", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text);

            //check delete selected items
            Functions.GridDropdownDelete(driver, baseURL);
            Assert.AreNotEqual("Текст отзыва 31", GetGridCell(0, "Text").Text);
            Assert.AreNotEqual("Текст отзыва 26", GetGridCell(1, "Text").Text);
            Assert.AreNotEqual("Текст отзыва 27", GetGridCell(2, "Text").Text);

            //check select all on page
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(9, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);

            //check delete all on page
            Functions.GridDropdownDelete(driver, baseURL);
            Assert.AreEqual("Текст отзыва 18", GetGridCell(0, "Text").Text);
            Assert.AreEqual("Текст отзыва 9", GetGridCell(9, "Text").Text);

            //check select all
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Assert.AreEqual("286", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text);

            //check deselect all 
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDeselectAllFn\"]")).Click();
            Assert.IsFalse(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsFalse(GetGridCell(9, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);

            //check delete all
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);

            GoToAdmin("reviews");
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));
        }

        [Test]
        public void aSort()
        {
            GoToAdmin("reviews");

            Functions.GridPaginationSelect100(driver, baseURL);
            ScrollTo(By.Id("header-top"));

            //check sort by name
            GetGridCell(-1, "Name").Click();
            WaitForAjax();
            Assert.IsTrue(GetGridCell(0, "Name").Text.Contains("CustomerName1"));
            Assert.IsTrue(GetGridCell(99, "Name").Text.Contains("CustomerName2"));

            GetGridCell(-1, "Name").Click();
            WaitForAjax();
            Assert.IsTrue(GetGridCell(0, "Name").Text.Contains("CustomerName3"));
            Assert.IsTrue(GetGridCell(99, "Name").Text.Contains("CustomerName3"));

            //sort by text
            GetGridCell(-1, "Text").Click();
            WaitForAjax();
            Assert.AreEqual("Текст отзыва 1", GetGridCell(0, "Text").Text);
            Assert.AreEqual("Текст отзыва 189", GetGridCell(99, "Text").Text);

            GetGridCell(-1, "Text").Click();
            WaitForAjax();
            Assert.AreEqual("Текст отзыва 99", GetGridCell(0, "Text").Text);
            Assert.AreEqual("Текст отзыва 28", GetGridCell(99, "Text").Text);

            //sort by add date
            GetGridCell(-1, "AddDateFormatted").Click();
            WaitForAjax();
            Assert.AreEqual("26.07.2012 14:22:00", GetGridCell(0, "AddDateFormatted").Text);
            Assert.AreEqual("26.07.2013 14:22:00", GetGridCell(99, "AddDateFormatted").Text);

            GetGridCell(-1, "AddDateFormatted").Click();
            WaitForAjax();
            Assert.AreEqual("26.07.2015 14:22:05", GetGridCell(0, "AddDateFormatted").Text);
            Assert.AreEqual("04.09.2013 14:22:00", GetGridCell(99, "AddDateFormatted").Text);

            //sort by checked
            GetGridCell(-1, "Checked").Click();
            WaitForAjax();
            Assert.IsFalse(GetGridCell(0, "Checked").FindElement(By.TagName("input")).Selected);
            Assert.IsFalse(GetGridCell(99, "Checked").FindElement(By.TagName("input")).Selected);

            GetGridCell(-1, "Checked").Click();
            WaitForAjax();
            Assert.IsTrue(GetGridCell(0, "Checked").FindElement(By.TagName("input")).Selected);
            Assert.IsTrue(GetGridCell(99, "Checked").FindElement(By.TagName("input")).Selected);
        }
    }
}