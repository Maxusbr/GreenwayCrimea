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
    public class PropertiesInPlace : BaseSeleniumTest
    {
         
         

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(
                "Data\\Admin\\Properties\\Catalog.Category.csv",
                "Data\\Admin\\Properties\\Catalog.Brand.csv",
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
        public void EditUseInFilter()
        {
             GoToAdmin("properties");
            GetGridCell(0, "UseInFilter").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(GetGridCell(0, "UseInFilter").FindElement(By.CssSelector("[data-e2e=\"switchOnOffLabel\"] input")).Selected);
        }
        [Test]
        public void EditUseInDetails()
        {
             GoToAdmin("properties");
            GetGridCell(0, "UseInDetails").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Click();
            Thread.Sleep(2000);
            Assert.IsFalse(GetGridCell(0, "UseInDetails").FindElement(By.CssSelector("[data-e2e=\"switchOnOffLabel\"] input")).Selected);
        }
        [Test]
        public void EditUseInBrief()
        {
            GoToAdmin("properties");
            driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[0][\'UseInBrief\']\"] [data-e2e=\"switchOnOffInput\"]")).Click();
            Thread.Sleep(2000);
            Assert.IsFalse(driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[0][\'UseInBrief\']\"] [data-e2e=\"switchOnOffLabel\"] input")).Selected);
        }
        [Test]
        public void EditSortOrder()
        {
             GoToAdmin("properties");
            driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[0][\'SortOrder\']\"] input")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[0][\'SortOrder\']\"] input")).SendKeys("10");
            DropFocus("h1");
            Refresh();
            Assert.AreEqual("Property2", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property1", GetGridCell(9, "Name").Text);
        }
        [Test]
        public void PropertyValueInplaceEdit()
        {
             GoToAdmin("propertyValues?propertyId=2");
            driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[0][\'Value\']\"] input")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[0][\'Value\']\"] input")).SendKeys("new_name");
            DropFocus("h1");
            Assert.AreEqual("new_name", GetGridCell(0, "Value").FindElement(By.TagName("input")).GetAttribute("value"));

            driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[0][\'SortOrder\']\"] input")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[0][\'SortOrder\']\"] input")).SendKeys("1");
            DropFocus("h1");
            Refresh();
            Assert.AreEqual("0", GetGridCell(0, "SortOrder").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("1", GetGridCell(2, "SortOrder").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("new_name", GetGridCell(2, "Value").FindElement(By.TagName("input")).GetAttribute("value"));
        }
    }
}
