using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.GridProducts
{
    [TestFixture]
    public class GridSelectedProductsTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(
           "data\\Admin\\Grid\\SelectProductsTest\\Catalog.Product.csv",
           "data\\Admin\\Grid\\SelectProductsTest\\Catalog.Offer.csv",
           "data\\Admin\\Grid\\SelectProductsTest\\Catalog.Category.csv",
           "data\\Admin\\Grid\\SelectProductsTest\\Catalog.ProductCategories.csv");
             
            Init();
        }
        
        [Test]
        public void ProductInCategoryPresent()
        {
            GoToAdmin("catalog?categoryid=1");
            Functions.GridPaginationSelect10(driver, baseURL);
            Assert.AreEqual("Найдено записей: 20", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);
            Assert.AreEqual("100", driver.FindElements(By.CssSelector(".aside-menu-inner"))[0].FindElement(By.CssSelector(".aside-menu-count")).Text);
            Assert.AreEqual("TestProduct1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("TestProduct10", GetGridCell(9, "Name").Text);

            Functions.GridPaginationSelect20(driver, baseURL);
            Assert.AreEqual("TestProduct1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("TestProduct20", GetGridCell(19, "Name").Text);

            GoToAdmin("catalog?showMethod=AllProducts");
            Functions.GridPaginationSelect50(driver, baseURL);
            Assert.AreEqual("TestProduct1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("TestProduct53", GetGridCell(49, "Name").Text);

            Functions.GridPaginationSelect100(driver, baseURL);
            Assert.AreEqual("TestProduct1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("TestProduct99", GetGridCell(99, "Name").Text);
        }
       
        [Test]
        public void ProductAllOnPageSelect()
        {
            GoToAdmin("catalog?categoryid=1");
            Functions.GridPaginationSelect10(driver, baseURL);
            ScrollTo(By.Id("header-top"));

            //выбрать все товары на странице
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();        
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(9, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.AreEqual("10", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text);

            Refresh();

            Functions.GridPaginationSelect20(driver, baseURL);
            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(19, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.AreEqual("20", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text);

            GoToAdmin("catalog?showMethod=AllProducts");
            Functions.GridPaginationSelect50(driver, baseURL);
            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(49, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.AreEqual("50", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text);

            Refresh();

            Functions.GridPaginationSelect100(driver, baseURL);
            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(99, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.AreEqual("100", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text);
        }
       
        /* 2, 3 товары неактивны в CSV */
        [Test]
        public void ProductChangeActive()
        {
            GoToAdmin("catalog?categoryid=1");

            //check do not active
            GetGridCell(4, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffLabel\"]")).Click();
            GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            GetGridCell(3, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownButton\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownItem\"][data-e2e-grid-selection-dropdown-index=\"5\"]")).Click();
           
            Refresh();

            Assert.IsFalse(GetGridCell(4, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);
            Assert.IsFalse(GetGridCell(0, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);
            Assert.IsFalse(GetGridCell(3, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);

            //check do active
            Refresh();

            GetGridCell(4, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffLabel\"]")).Click();
            GetGridCell(1, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            GetGridCell(2, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownButton\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownItem\"][data-e2e-grid-selection-dropdown-index=\"4\"]")).Click();
          
            Refresh();

            Assert.IsTrue(GetGridCell(4, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);
            Assert.IsTrue(GetGridCell(1, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);
            Assert.IsTrue(GetGridCell(2, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);
        }
        
        [Test]
        public void ProductzSelectAndDelete()
        {
            GoToAdmin("catalog?categoryid=4");
            gridReturnDefaultView10();
            //check delete from category
            GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            GetGridCell(1, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownButton\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownItem\"][data-e2e-grid-selection-dropdown-index=\"1\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(1000);
          //  Refresh();
            Assert.AreNotEqual("TestProduct61", GetGridCell(0, "Name").Text);
            Assert.AreNotEqual("TestProduct62", GetGridCell(1, "Name").Text);

            GoToAdmin("catalog?showMethod=OnlyWithoutCategories");

            Assert.AreEqual("TestProduct61", GetGridCell(0, "Name").Text);
            Assert.AreEqual("TestProduct62", GetGridCell(1, "Name").Text);

            //check select item
            GoToAdmin("catalog?categoryid=5");
            GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            Thread.Sleep(1000);
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);

            //check delete cancel
            GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[0][\'_serviceColumn\']\"] ui-grid-custom-delete")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-cancel")).Click();
            Thread.Sleep(1000);
            Assert.AreEqual("TestProduct81", GetGridCell(0, "Name").Text);

            //check delete
            driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[0][\'_serviceColumn\']\"] ui-grid-custom-delete")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(1000);
            Assert.AreNotEqual("TestProduct81", GetGridCell(0, "Name").Text);

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
            Assert.AreNotEqual("TestProduct82", GetGridCell(0, "Name").Text);
            Assert.AreNotEqual("TestProduct83", GetGridCell(1, "Name").Text);
            Assert.AreNotEqual("TestProduct84", GetGridCell(2, "Name").Text);

            //check select all on page
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(9, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);

            //check delete all on page
            Functions.GridDropdownDelete(driver, baseURL);
           Assert.AreEqual("TestProduct95", GetGridCell(0, "Name").Text);
            Assert.AreEqual("TestProduct100", GetGridCell(5, "Name").Text);

            //check select all
            GoToAdmin("catalog?showMethod=AllProducts");
           driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Assert.AreEqual("86", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text);

            //check deselect all 
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDeselectAllFn\"]")).Click();
           Assert.IsFalse(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsFalse(GetGridCell(9, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);

            //check delete all
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);
          
            GoToAdmin("catalog?showMethod=AllProducts");
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));
        }
      
        [Test]
        public void ProductSelectedToAnotherCategory()
        {
            GoToAdmin("catalog?categoryid=1");

            //to another category
            GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            GetGridCell(1, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownButton\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownItem\"][data-e2e-grid-selection-dropdown-index=\"3\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory3')]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//button[contains(text(), 'Добавить товары')]")).Click();
            Thread.Sleep(1000);
            Refresh();
            //move to another category
            GetGridCell(2, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            GetGridCell(3, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownButton\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownItem\"][data-e2e-grid-selection-dropdown-index=\"2\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.Id("2")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//button[contains(text(), 'Перенести товары')]")).Click();
            Thread.Sleep(1000);
            //check first category
            Assert.AreEqual("TestProduct1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("TestProduct2", GetGridCell(1, "Name").Text);

            Assert.AreNotEqual("TestProduct3", GetGridCell(2, "Name").Text);
            Assert.AreNotEqual("TestProduct4", GetGridCell(3, "Name").Text);

            //check others categories
            GoToAdmin("catalog?categoryid=3");
            Assert.AreEqual("TestProduct1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("TestProduct2", GetGridCell(1, "Name").Text);

            GoToAdmin("catalog?categoryid=2");
            Assert.AreEqual("TestProduct3", GetGridCell(0, "Name").Text);
            Assert.AreEqual("TestProduct4", GetGridCell(1, "Name").Text);
        }
    }
}