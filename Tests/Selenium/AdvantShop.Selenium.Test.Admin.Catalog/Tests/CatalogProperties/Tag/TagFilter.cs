using System;
using NUnit.Framework;
using System.Threading;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;

namespace AdvantShop.SeleniumTest.Admin.Catalog.Tag
{
    [TestFixture]
    public class TagFilter : BaseSeleniumTest
    {
         
         

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(
                "Data\\Admin\\Catalog\\Tag\\Catalog.Category.csv",
                "Data\\Admin\\Catalog\\Tag\\Catalog.Tag.csv",
                "Data\\Admin\\Catalog\\Tag\\Catalog.Product.csv",
                "Data\\Admin\\Catalog\\Tag\\Catalog.ProductCategories.csv",
                "Data\\Admin\\Catalog\\Tag\\Catalog.TagMap.csv"
                );

             
            Init();

        }
        
        [Test]
        public void ByName()
        {
            GoToAdmin("tags");
            Functions.GridFilterSet(driver, baseURL, "Name");
            DropFocus("h1");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"Name\"] [data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"Name\"] [data-e2e=\"gridFilterItemInput\"]")).SendKeys("New_tag10");
            DropFocus("h1");

            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 7);
            Assert.AreEqual("New_Tag10",GetGridCell(0, "Name").Text);
            Assert.AreEqual("New_Tag105", GetGridCell(6, "Name").Text);

            GetGridCell(0, "Name").Click();
            Thread.Sleep(1000);
            Assert.IsTrue(driver.Url.Contains("tags/edit/10"));
            GoBack();
            Assert.AreEqual("New_Tag10", GetGridCell(0, "Name").Text);
            Assert.AreEqual("New_Tag105", GetGridCell(6, "Name").Text);

            //close
            Functions.GridFilterClose(driver, baseURL, "Name");
            Assert.AreEqual("New_Tag1",GetGridCell(0, "Name").Text);
            Assert.AreEqual("New_Tag10", GetGridCell(9, "Name").Text);
        }

        [Test]
        public void ByURLPath()
        {
            GoToAdmin("tags");
            Functions.GridFilterSet(driver, baseURL, "UrlPath");
            DropFocus("h1");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"UrlPath\"] [data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"UrlPath\"] [data-e2e=\"gridFilterItemInput\"]")).SendKeys("teg10");
            DropFocus("h1");

            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 7);
            Assert.AreEqual("New_Tag10",GetGridCell(0, "Name").Text);
            Assert.AreEqual("New_Tag105", GetGridCell(6, "Name").Text);
            //close
            Functions.GridFilterClose(driver, baseURL, "UrlPath");
            Assert.AreEqual("New_Tag1",GetGridCell(0, "Name").Text);
            Assert.AreEqual("New_Tag10", GetGridCell(9, "Name").Text);
        }

        [Test]
        public void ByEnabledYes()
        {
            GoToAdmin("tags");
            Functions.GridPaginationSelect100(driver, baseURL);
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterSet(driver, baseURL, "Enabled");           
            DropFocus("h1");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Активные");
            DropFocus("h1");

            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 100);
            Assert.AreEqual("New_Tag1",GetGridCell(0, "Name").Text);
            Assert.AreEqual("New_Tag100", GetGridCell(99, "Name").Text);
            //close
            Functions.GridFilterClose(driver, baseURL, "Enabled");
            Assert.AreEqual("New_Tag1",GetGridCell(0, "Name").Text);
            Assert.AreEqual("New_Tag10", GetGridCell(9, "Name").Text);
        }
        [Test]
        public void ByEnabledNo()
        {
            GoToAdmin("tags");
            Functions.GridPaginationSelect100(driver, baseURL);
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterSet(driver, baseURL, "Enabled");
            DropFocus("h1");           
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Неактивные");
            DropFocus("h1");
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 4);
            Assert.AreEqual("New_Tag102",GetGridCell(0, "Name").Text);
            Assert.AreEqual("New_Tag105",GetGridCell(3, "Name").Text);
            //close
            Functions.GridFilterClose(driver, baseURL, "Enabled");
            Assert.AreEqual("New_Tag1",GetGridCell(0, "Name").Text);
            Assert.AreEqual("New_Tag10", GetGridCell(9, "Name").Text);
        }
        [Test]
        public void ByNameAndActive()
        {
            GoToAdmin("tags");
            Functions.GridFilterSet(driver, baseURL, "Name");
            DropFocus("h1");           
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("New_tag10");
            DropFocus("h1");

            Assert.AreEqual("New_Tag10",GetGridCell(0, "Name").Text);
            Assert.AreEqual("New_Tag105", GetGridCell(6, "Name").Text);

            Functions.GridFilterSet(driver, baseURL, "Enabled");
            DropFocus("h1");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Активные");
            DropFocus("h1");

            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 3);
            Assert.AreEqual("New_Tag10",GetGridCell(0, "Name").Text);
            Assert.AreEqual("New_Tag101", GetGridCell(2, "Name").Text);

            //close name
            Functions.GridFilterClose(driver, baseURL, "Name");
            Assert.AreEqual("New_Tag1",GetGridCell(0, "Name").Text);
            Assert.AreEqual("New_Tag10", GetGridCell(9, "Name").Text);
           
            Functions.GridFilterClose(driver, baseURL, "Enabled");
            Assert.AreEqual("New_Tag1",GetGridCell(0, "Name").Text);
            Assert.AreEqual("New_Tag10", GetGridCell(9, "Name").Text);
         }
        [Test]
        public void DelByName()
        {
            GoToAdmin("tags");
            Functions.GridFilterSet(driver, baseURL, "Name");
            DropFocus("h1");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"Name\"] [data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"Name\"] [data-e2e=\"gridFilterItemInput\"]")).SendKeys("New_tag10");
            DropFocus("h1");

            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 7);
            Assert.AreEqual("New_Tag10",GetGridCell(0, "Name").Text);
            Assert.AreEqual("New_Tag105", GetGridCell(6, "Name").Text);
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"Name\"]")).Displayed);
          
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownButton\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e-grid-selection-dropdown-index=\"0\"]")).Click();
            Thread.Sleep(500);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"Name\"]")).Displayed);

            //close
            Functions.GridFilterClose(driver, baseURL, "Name");
            Assert.AreEqual("New_Tag1",GetGridCell(0, "Name").Text);
            Assert.AreEqual("New_Tag11", GetGridCell(9, "Name").Text);
        }
    }
}
