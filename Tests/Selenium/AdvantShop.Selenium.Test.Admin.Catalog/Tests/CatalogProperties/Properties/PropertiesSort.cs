using System;
using NUnit.Framework;
using System.Threading;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;

namespace AdvantShop.SeleniumTest.Admin.Properties
{
    [TestFixture]
    public class PropertiesSort : BaseSeleniumTest
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
        public void OpenProperties()
        {
             GoToAdmin("properties");
            driver.FindElement(By.CssSelector(".link-invert")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Property1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property10", GetGridCell(9, "Name").Text);
         }

        [Test]
        public void SortByName()
        {
             GoToAdmin("properties");

            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCell\"][data-e2e-col-index=\"0\"]")).Click();
            Assert.AreEqual("Property1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property16", GetGridCell(9, "Name").Text);
             driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCell\"][data-e2e-col-index=\"0\"]")).Click();
            Assert.AreEqual("Property99", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property90", GetGridCell(9, "Name").Text);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCell\"][data-e2e-col-index=\"0\"]")).Click();
            Assert.AreEqual("Property1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property16", GetGridCell(9, "Name").Text);
         }

        [Test]
        public void SortByGroup()
        {
             GoToAdmin("properties");

            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCell\"][data-e2e-col-index=\"1\"]")).Click();
            Assert.AreEqual("Property100", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property101", GetGridCell(1, "Name").Text);
            Assert.AreEqual("Group1", driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[9][\'GroupName\']\"]")).Text);

            Assert.AreEqual("Property8", GetGridCell(9, "Name").Text);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCell\"][data-e2e-col-index=\"1\"]")).Click();
            Assert.AreEqual("Property80", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property89", GetGridCell(9, "Name").Text);
            Assert.AreEqual("Group9", driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[1][\'GroupName\']\"]")).Text);

            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCell\"][data-e2e-col-index=\"1\"]")).Click();
            Assert.AreEqual("Property100", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property1", GetGridCell(2, "Name").Text);
        }
        [Test]
        public void SortByFilter()
        {
             GoToAdmin("properties");

            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCell\"][data-e2e-col-index=\"2\"]")).Click();
            Assert.AreEqual("Property1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property10", GetGridCell(9, "Name").Text);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCell\"][data-e2e-col-index=\"2\"]")).Click();
            Assert.AreEqual("Property6", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property15", GetGridCell(9, "Name").Text);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCell\"][data-e2e-col-index=\"2\"]")).Click();
            Assert.AreEqual("Property1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property10", GetGridCell(9, "Name").Text);
        }
            [Test]
        public void SortByViewInCart()
        {
             GoToAdmin("properties");

            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCell\"][data-e2e-col-index=\"3\"]")).Click();
            Assert.AreEqual("Property11", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property20", GetGridCell(9, "Name").Text);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCell\"][data-e2e-col-index=\"3\"]")).Click();
            Assert.AreEqual("Property16", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property25", GetGridCell(9, "Name").Text);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCell\"][data-e2e-col-index=\"3\"]")).Click();
            Assert.AreEqual("Property11", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property20", GetGridCell(9, "Name").Text);
        }
        [Test]
        public void SortByBrief()
        {
             GoToAdmin("properties");

            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCell\"][data-e2e-col-index=\"4\"]")).Click();
            Assert.AreEqual("Property21", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property30", GetGridCell(9, "Name").Text);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCell\"][data-e2e-col-index=\"4\"]")).Click();
            Assert.AreEqual("Property26", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property35", GetGridCell(9, "Name").Text);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCell\"][data-e2e-col-index=\"4\"]")).Click();
            Assert.AreEqual("Property21", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property30", GetGridCell(9, "Name").Text);
        }
        [Test]
        public void SortBySort()
        {
             GoToAdmin("properties");

            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCell\"][data-e2e-col-index=\"5\"]")).Click();
            Assert.AreEqual("Property1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property10", GetGridCell(9, "Name").Text);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCell\"][data-e2e-col-index=\"5\"]")).Click();
            Assert.AreEqual("Property101", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property92", GetGridCell(9, "Name").Text);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCell\"][data-e2e-col-index=\"5\"]")).Click();
            Assert.AreEqual("Property1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property10", GetGridCell(9, "Name").Text);
        }
        [Test]
        public void SortByUseProduct()
        {
             GoToAdmin("properties");

            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCell\"][data-e2e-col-index=\"6\"]")).Click();
            Assert.AreEqual("Property6", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property15", GetGridCell(9, "Name").Text);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCell\"][data-e2e-col-index=\"6\"]")).Click();
            Assert.AreEqual("Property1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property10", GetGridCell(9, "Name").Text);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCell\"][data-e2e-col-index=\"6\"]")).Click();
            Assert.AreEqual("Property6", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property15", GetGridCell(9, "Name").Text);
        }

    }
}
