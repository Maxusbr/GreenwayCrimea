using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.Properties
{
    [TestFixture]
    public class PropertiesFilterUseInFilter : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(
                "Data\\Admin\\Properties\\Catalog.Category.csv",
                "data\\Admin\\Properties\\Catalog.Brand.csv",
                "Data\\Admin\\Properties\\Catalog.Property.csv",
                 "Data\\Admin\\Properties\\Catalog.PropertyValue.csv",
                 "Data\\Admin\\Properties\\Catalog.ProductPropertyValue.csv",
                "Data\\Admin\\Properties\\Catalog.Product.csv",
                "Data\\Admin\\Properties\\Catalog.Offer.csv",
                "Data\\Admin\\Properties\\Catalog.ProductCategories.csv",
                "Data\\Admin\\Properties\\Catalog.PropertyGroup.csv"
                );

             
            Init();

        }
        
        [Test]
        public void ByUseInFilterYes()
        {
             GoToAdmin("properties");
            gridReturnDefaultView10();
            Functions.GridFilterSet(driver, baseURL, "UseInFilter");
            DropFocus("h1");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Да");
            DropFocus("h1");

            Assert.AreEqual("Property6", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property15", GetGridCell(9, "Name").Text);
            //close
            Functions.GridFilterClose(driver, baseURL, "UseInFilter");
            Assert.AreEqual("Property1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property10", GetGridCell(9, "Name").Text);
        }
        [Test]
        public void ByUseInFilterYesPresent20()
        {
             GoToAdmin("properties");
            Functions.GridPaginationSelect20(driver, baseURL);
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterSet(driver, baseURL, "UseInFilter");
            DropFocus("h1");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Да");
            DropFocus("h1");
            Assert.AreEqual("Property6", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property25", GetGridCell(19, "Name").Text);
            //close
            Functions.GridFilterClose(driver, baseURL, "UseInFilter");
            Assert.AreEqual("Property1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property20", GetGridCell(19, "Name").Text);
        }
        [Test]
        public void ByUseInFilterYesPresent50()
        {
             GoToAdmin("properties");
            Functions.GridPaginationSelect50(driver, baseURL);
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterSet(driver, baseURL, "UseInFilter");
            DropFocus("h1");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Да");
            DropFocus("h1");

            Assert.AreEqual("Property6", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property55", GetGridCell(49, "Name").Text);
            //close
            Functions.GridFilterClose(driver, baseURL, "UseInFilter");
            Assert.AreEqual("Property1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property50", GetGridCell(49, "Name").Text);
        }

        [Test]
        public void ByUseInFilterNo()
        {
             GoToAdmin("properties");
            gridReturnDefaultView10();
            Functions.GridFilterSet(driver, baseURL, "UseInFilter");
            DropFocus("h1");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Нет");
            DropFocus("h1");

            Assert.AreEqual("Property1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property5", GetGridCell(4, "Name").Text);
            //close
            Functions.GridFilterClose(driver, baseURL, "UseInFilter");
            Assert.AreEqual("Property1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property10", GetGridCell(9, "Name").Text);
        }
        [Test]
        public void ByUseInFilterNoPresent20()
        {
             GoToAdmin("properties");
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            Functions.GridPaginationSelect20(driver, baseURL);
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterSet(driver, baseURL, "UseInFilter");
            DropFocus("h1");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Нет");
            DropFocus("h1");

            Assert.AreEqual("Property1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property5", GetGridCell(4, "Name").Text);
            ScrollTo(By.Id("header-top"));
            //close
            Functions.GridFilterClose(driver, baseURL, "UseInFilter");
            Assert.AreEqual("Property1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property20", GetGridCell(19, "Name").Text);
        }
        [Test]
        public void ByUseInFilterNoPresent50()
        {
             GoToAdmin("properties");
            Functions.GridPaginationSelect50(driver, baseURL);
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterSet(driver, baseURL, "UseInFilter");
            DropFocus("h1");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Нет");
            DropFocus("h1");

            Assert.AreEqual("Property1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property5", GetGridCell(4, "Name").Text);
            ScrollTo(By.Id("header-top"));
            //close
            Functions.GridFilterClose(driver, baseURL, "UseInFilter");
            Assert.AreEqual("Property1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property50", GetGridCell(49, "Name").Text);
        }
        [Test]
        public void ByUseInFilterYesPage()
        {
             GoToAdmin("properties");
            gridReturnDefaultView10();
            Functions.GridFilterSet(driver, baseURL, "UseInFilter");
            DropFocus("h1");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Да");
            DropFocus("h1");

            Assert.AreEqual("Property6", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property15", GetGridCell(9, "Name").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(4) a")).Click();
            Assert.AreEqual("Property16", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property25", GetGridCell(9, "Name").Text);
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(5) a")).Click();
            Assert.AreEqual("Property26", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property35", GetGridCell(9, "Name").Text);
            ScrollTo(By.Id("header-top"));
            //close
            Functions.GridFilterClose(driver, baseURL, "UseInFilter");
            Assert.AreEqual("Property1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property10", GetGridCell(9, "Name").Text);
        }

        [Test]
        public void ByUseInFilterYesPageToBegin()
        {
             GoToAdmin("properties");
            gridReturnDefaultView10();
            Functions.GridFilterSet(driver, baseURL, "UseInFilter");
            DropFocus("h1");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Да");
            DropFocus("h1");

            Assert.AreEqual("Property6", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property15", GetGridCell(9, "Name").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Assert.AreEqual("Property16", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property25", GetGridCell(9, "Name").Text);
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Assert.AreEqual("Property26", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property35", GetGridCell(9, "Name").Text);

            //to begin
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-first a")).Click();
            Assert.AreEqual("Property6", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property15", GetGridCell(9, "Name").Text);
            ScrollTo(By.Id("header-top"));
            //close
            Functions.GridFilterClose(driver, baseURL, "UseInFilter");
            Assert.AreEqual("Property1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property10", GetGridCell(9, "Name").Text);
        }

        [Test]
        public void ByUseInFilterYesPageToEnd()
        {
             GoToAdmin("properties");
            gridReturnDefaultView10();
            Functions.GridFilterSet(driver, baseURL, "UseInFilter");
            DropFocus("h1");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Да");
            DropFocus("h1");

            Assert.AreEqual("Property6", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property15", GetGridCell(9, "Name").Text);
            //to end
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-last a")).Click();
            Assert.AreEqual("Property96", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property101", GetGridCell(5, "Name").Text);
            ScrollTo(By.Id("header-top"));
            //close
            Functions.GridFilterClose(driver, baseURL, "UseInFilter");
            Assert.AreEqual("Property1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property10", GetGridCell(9, "Name").Text);
        }

        [Test]
        public void ByUseInFilterYesPageToNext()
        {
             GoToAdmin("properties");
            gridReturnDefaultView10();
            Functions.GridFilterSet(driver, baseURL, "UseInFilter");
            DropFocus("h1");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Да");
            DropFocus("h1");

            Assert.AreEqual("Property6", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property15", GetGridCell(9, "Name").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Assert.AreEqual("Property16", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property25", GetGridCell(9, "Name").Text);
            ScrollTo(By.Id("header-top"));
            //close
            Functions.GridFilterClose(driver, baseURL, "UseInFilter");
            Assert.AreEqual("Property1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property10", GetGridCell(9, "Name").Text);
        }

        [Test]
        public void ByUseInFilterYesPageToPrevious()
        {
             GoToAdmin("properties");
            gridReturnDefaultView10();
            Functions.GridFilterSet(driver, baseURL, "UseInFilter");
            DropFocus("h1");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Да");
            DropFocus("h1");

            Assert.AreEqual("Property6", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property15", GetGridCell(9, "Name").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Assert.AreEqual("Property16", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property25", GetGridCell(9, "Name").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Assert.AreEqual("Property26", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property35", GetGridCell(9, "Name").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            Assert.AreEqual("Property16", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property25", GetGridCell(9, "Name").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            Assert.AreEqual("Property6", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property15", GetGridCell(9, "Name").Text);
            ScrollTo(By.Id("header-top"));
            //close
            Functions.GridFilterClose(driver, baseURL, "UseInFilter");
            Assert.AreEqual("Property1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property10", GetGridCell(9, "Name").Text);
        }
    }
}
