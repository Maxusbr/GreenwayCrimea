using System;
using NUnit.Framework;
using System.Threading;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;

namespace AdvantShop.SeleniumTest.Admin.Colors
{
    [TestFixture]
    public class ColorPaginationAndView : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(
                "Data\\Admin\\Color\\Catalog.Color.csv"
                );

             
            Init();

        }
        
        [Test]
        public void Present10Page()
        {
           GoToAdmin("colors");

            Assert.AreEqual("Color1", GetGridCell(0, "ColorName").Text);
            Assert.AreEqual("Color10", GetGridCell(9, "ColorName").Text);
            ScrollTo(By.CssSelector(".version"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(4) a")).Click();
            Assert.AreEqual("Color11", GetGridCell(0, "ColorName").Text);
            Assert.AreEqual("Color20", GetGridCell(9, "ColorName").Text);

            ScrollTo(By.CssSelector(".version"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(5) a")).Click();
            Assert.AreEqual("Color21", GetGridCell(0, "ColorName").Text);
            Assert.AreEqual("Color30", GetGridCell(9, "ColorName").Text);
        }
        [Test]
        public void Present10PageToNext()
        {
           GoToAdmin("colors");

            Assert.AreEqual("Color1", GetGridCell(0, "ColorName").Text);
            Assert.AreEqual("Color10", GetGridCell(9, "ColorName").Text);

            ScrollTo(By.CssSelector(".version"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Assert.AreEqual("Color11", GetGridCell(0, "ColorName").Text);
            Assert.AreEqual("Color20", GetGridCell(9, "ColorName").Text);
            ScrollTo(By.CssSelector(".version"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Assert.AreEqual("Color21", GetGridCell(0, "ColorName").Text);
            Assert.AreEqual("Color30", GetGridCell(9, "ColorName").Text);
        }
        [Test]
        public void Present10PageToPrevious()
        {
           GoToAdmin("colors");

            Assert.AreEqual("Color1", GetGridCell(0, "ColorName").Text);
            Assert.AreEqual("Color10", GetGridCell(9, "ColorName").Text);
            ScrollTo(By.CssSelector(".version"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Assert.AreEqual("Color11", GetGridCell(0, "ColorName").Text);
            Assert.AreEqual("Color20", GetGridCell(9, "ColorName").Text);
            ScrollTo(By.CssSelector(".version"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            Assert.AreEqual("Color1", GetGridCell(0, "ColorName").Text);
            Assert.AreEqual("Color10", GetGridCell(9, "ColorName").Text);
        }
        [Test]
        public void Present10PageToEnd()
        {
           GoToAdmin("colors");

            Assert.AreEqual("Color1", GetGridCell(0, "ColorName").Text);
            Assert.AreEqual("Color10", GetGridCell(9, "ColorName").Text);
            //to end
            ScrollTo(By.CssSelector(".version"));
            driver.FindElement(By.CssSelector(".pagination-last a")).Click();
            Assert.AreEqual("Color111", GetGridCell(0, "ColorName").Text);
        }
        [Test]
        public void Present10PageToBegin()
        {
           GoToAdmin("colors");

            Assert.AreEqual("Color1", GetGridCell(0, "ColorName").Text);
            Assert.AreEqual("Color10", GetGridCell(9, "ColorName").Text);
            ScrollTo(By.CssSelector(".version"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Assert.AreEqual("Color11", GetGridCell(0, "ColorName").Text);
            Assert.AreEqual("Color20", GetGridCell(9, "ColorName").Text);
            ScrollTo(By.CssSelector(".version"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Assert.AreEqual("Color21", GetGridCell(0, "ColorName").Text);
            Assert.AreEqual("Color30", GetGridCell(9, "ColorName").Text);

            //to begin
            ScrollTo(By.CssSelector(".version"));
            driver.FindElement(By.CssSelector(".pagination-first a")).Click();
            Assert.AreEqual("Color1", GetGridCell(0, "ColorName").Text);
            Assert.AreEqual("Color10", GetGridCell(9, "ColorName").Text);
        }
    }
}
