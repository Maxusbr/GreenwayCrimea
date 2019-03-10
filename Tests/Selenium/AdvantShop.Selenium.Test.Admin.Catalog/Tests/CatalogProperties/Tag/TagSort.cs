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
    public class TagSort : BaseSeleniumTest
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
        public void SortByName()
        {
            GoToAdmin("tags");

            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCell\"][data-e2e-col-index=\"0\"]")).Click();
            Assert.AreEqual("New_Tag1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("New_Tag12", GetGridCell(9, "Name").Text);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCell\"][data-e2e-col-index=\"0\"]")).Click();
            Assert.AreEqual("New_Tag99", GetGridCell(0, "Name").Text);
            Assert.AreEqual("New_Tag90", GetGridCell(9, "Name").Text);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCell\"][data-e2e-col-index=\"0\"]")).Click();
            Assert.AreEqual("New_Tag1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("New_Tag12", GetGridCell(9, "Name").Text);
        }

        [Test]
        public void SortByUrl()
        {
            GoToAdmin("tags");

            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCell\"][data-e2e-col-index=\"1\"]")).Click();
            Assert.AreEqual("teg1", GetGridCell(0, "UrlPath").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("teg12", GetGridCell(9, "UrlPath").FindElement(By.TagName("input")).GetAttribute("value"));
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCell\"][data-e2e-col-index=\"1\"]")).Click();
            Assert.AreEqual("teg99", GetGridCell(0, "UrlPath").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("teg90", GetGridCell(9, "UrlPath").FindElement(By.TagName("input")).GetAttribute("value"));
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCell\"][data-e2e-col-index=\"1\"]")).Click();
            Assert.AreEqual("teg1", GetGridCell(0, "UrlPath").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("teg12", GetGridCell(9, "UrlPath").FindElement(By.TagName("input")).GetAttribute("value"));
        }
        [Test]
        public void SortByActive()
        {
            GoToAdmin("tags");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCell\"][data-e2e-col-index=\"2\"]")).Click();
            Assert.AreEqual("New_Tag102", GetGridCell(0, "Name").Text);
            Assert.AreEqual("New_Tag6", GetGridCell(9, "Name").Text);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCell\"][data-e2e-col-index=\"2\"]")).Click();
            Assert.AreEqual("New_Tag1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("New_Tag10", GetGridCell(9, "Name").Text);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCell\"][data-e2e-col-index=\"2\"]")).Click();
            Assert.AreEqual("New_Tag102", GetGridCell(0, "Name").Text);
            Assert.AreEqual("New_Tag6", GetGridCell(9, "Name").Text);
        }
    }
}
