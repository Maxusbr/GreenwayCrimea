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
    public class TagPaginationAndView : BaseSeleniumTest
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
        public void Present10Page()
        {
            GoToAdmin("tags");

            Assert.AreEqual("New_Tag1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("New_Tag10",GetGridCell(9, "Name").Text);
            ScrollTo(By.CssSelector(".version"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(4) a")).Click();
            Assert.AreEqual("New_Tag11", GetGridCell(0, "Name").Text);
            Assert.AreEqual("New_Tag20",GetGridCell(9, "Name").Text);
            ScrollTo(By.CssSelector(".version"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(5) a")).Click();
            Assert.AreEqual("New_Tag21", GetGridCell(0, "Name").Text);
            Assert.AreEqual("New_Tag30",GetGridCell(9, "Name").Text);
        }
        [Test]
        public void Present10PageToNext()
        {
            GoToAdmin("tags");

            Assert.AreEqual("New_Tag1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("New_Tag10",GetGridCell(9, "Name").Text);
            ScrollTo(By.CssSelector(".version"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Assert.AreEqual("New_Tag11", GetGridCell(0, "Name").Text);
            Assert.AreEqual("New_Tag20",GetGridCell(9, "Name").Text);
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Assert.AreEqual("New_Tag21", GetGridCell(0, "Name").Text);
            Assert.AreEqual("New_Tag30",GetGridCell(9, "Name").Text);
        }
        [Test]
        public void Present10PageToPrevious()
        {
            GoToAdmin("tags");

            Assert.AreEqual("New_Tag1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("New_Tag10",GetGridCell(9, "Name").Text);
           ScrollTo(By.CssSelector(".version"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Assert.AreEqual("New_Tag11", GetGridCell(0, "Name").Text);
            Assert.AreEqual("New_Tag20",GetGridCell(9, "Name").Text);
           ScrollTo(By.CssSelector(".version"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            Assert.AreEqual("New_Tag1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("New_Tag10",GetGridCell(9, "Name").Text);
        }
        [Test]
        public void Present10PageToEnd()
        {
            GoToAdmin("tags");

            Assert.AreEqual("New_Tag1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("New_Tag10",GetGridCell(9, "Name").Text);
            //to end
           ScrollTo(By.CssSelector(".version"));
            driver.FindElement(By.CssSelector(".pagination-last a")).Click();
            Assert.AreEqual("New_Tag101", GetGridCell(0, "Name").Text);
        }
        [Test]
        public void Present10PageToBegin()
        {
            GoToAdmin("tags");

            Assert.AreEqual("New_Tag1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("New_Tag10",GetGridCell(9, "Name").Text);
           ScrollTo(By.CssSelector(".version"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Assert.AreEqual("New_Tag11", GetGridCell(0, "Name").Text);
            Assert.AreEqual("New_Tag20",GetGridCell(9, "Name").Text);
            Thread.Sleep(2000);
           ScrollTo(By.CssSelector(".version"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Assert.AreEqual("New_Tag21", GetGridCell(0, "Name").Text);
            Assert.AreEqual("New_Tag30",GetGridCell(9, "Name").Text);

            //to begin
           ScrollTo(By.CssSelector(".version"));
            driver.FindElement(By.CssSelector(".pagination-first a")).Click();
            Assert.AreEqual("New_Tag1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("New_Tag10",GetGridCell(9, "Name").Text);
        }
    }
}
