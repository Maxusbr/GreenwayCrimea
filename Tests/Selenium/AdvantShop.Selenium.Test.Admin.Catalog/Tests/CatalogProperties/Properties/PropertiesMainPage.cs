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
    public class PropertiesMainPage : BaseSeleniumTest
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
            Assert.AreEqual("Property1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property10", GetGridCell(9, "Name").Text);
            Assert.IsTrue(driver.PageSource.Contains("Group1"));
        }
        [Test]
        public void OpenPropertiesWithoutGroup()
        {
             GoToAdmin("properties");
            driver.FindElement(By.XPath("//a[contains(text(), 'Свойства без группы')]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Property100", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property101", GetGridCell(1, "Name").Text);
       }
        [Test]
        public void OpenAllProperties()
        {
             GoToAdmin("properties");
            driver.FindElements(By.CssSelector(".as-sortable-item a"))[0].Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Property1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property10", GetGridCell(9, "Name").Text);
        }
        [Test]
        public void OpenPropertiesInGroup()
        {
             GoToAdmin("properties");
            driver.FindElement(By.XPath("//a[contains(text(), 'Group1')]")).Click();
            Thread.Sleep(1000);
            Refresh();
            Assert.AreEqual("Group1", driver.FindElement(By.TagName("h1")).Text);
            Assert.AreEqual("Property1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property9", GetGridCell(8, "Name").Text);
            driver.FindElement(By.XPath("//a[contains(text(), 'Group2')]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Group2", driver.FindElement(By.TagName("h1")).Text);
            Assert.AreEqual("Property10", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property19", GetGridCell(9, "Name").Text);
        }
        [Test]
        public void SearchPropertiesAll()
        {
             GoToAdmin("properties");
            GetGridFilter().SendKeys("Property1");
            DropFocus("h1");
            Assert.AreEqual("Property1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property18", GetGridCell(9, "Name").Text);
        }
        [Test]
        public void SearchPropertiesInGroup()
        {
             GoToAdmin("properties?groupId=1");
            GetGridFilter().SendKeys("Property1");
            DropFocus("h1");
            Assert.AreEqual("Property1", GetGridCell(0, "Name").Text);
        }
    }
}
