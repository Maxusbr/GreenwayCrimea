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
    public class ColorMainPage : BaseSeleniumTest
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
        public void OpenColorWindows()
        {
           GoToAdmin("colors");
            Assert.AreEqual("Цвета", driver.FindElement(By.TagName("h1")).Text);
            Assert.AreEqual("color: rgb(0, 0, 0);", GetGridCell(0, "ColorIcon").FindElement(By.TagName("i")).GetAttribute("style"));
            Assert.AreEqual("Color1", GetGridCell(0, "ColorName").Text);
            Assert.AreEqual("1", GetGridCell(0, "SortOrder").FindElement(By.TagName("input")).GetAttribute("value")); 
        }
        [Test]
        public void SearchColor()
        {
           GoToAdmin("colors");
            GetGridFilter().SendKeys("Color11");
            DropFocus("h1");
            Assert.AreEqual("Color11", GetGridCell(0, "ColorName").Text);
            Assert.AreEqual("Color110", GetGridCell(1, "ColorName").Text);
            Assert.AreEqual("Color111", GetGridCell(2, "ColorName").Text);
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 3);
        }
        [Test]
        public void SearchNotExistColor()
        {
           GoToAdmin("colors");
            GetGridFilter().SendKeys("Color1111");
            DropFocus("h1");
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));
        }
        [Test]
        public void SearchLongNameColor()
        {
           GoToAdmin("colors");
            GetGridFilter().SendKeys("qqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqq");
            DropFocus("h1");
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));
        }
        [Test]
        public void SearchInvalidSimbolColor()
        {
           GoToAdmin("colors");
            GetGridFilter().SendKeys("123!@##$%5423");
            DropFocus("h1");
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));
        }
        [Test]
        public void zInplaceEdit()
        {
           GoToAdmin("colors");
            driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[0][\'SortOrder\']\"] input")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[0][\'SortOrder\']\"] input")).SendKeys("3");
            DropFocus("h1");
            Thread.Sleep(2000);
            Refresh();
            Assert.AreEqual("Color2", GetGridCell(0, "ColorName").Text);
            Assert.AreEqual("Color1", GetGridCell(1, "ColorName").Text);
        }
        [Test]
        public void SortByName()
        {
           GoToAdmin("colors");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCell\"][data-e2e-col-index=\"1\"]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Color1", GetGridCell(0, "ColorName").Text);
            Assert.AreEqual("Color107", GetGridCell(9, "ColorName").Text);
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 10);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCell\"][data-e2e-col-index=\"1\"]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Color99", GetGridCell(0, "ColorName").Text);
            Assert.AreEqual("Color90", GetGridCell(9, "ColorName").Text);
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 10);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCell\"][data-e2e-col-index=\"1\"]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Color1", GetGridCell(0, "ColorName").Text);
            Assert.AreEqual("Color107", GetGridCell(9, "ColorName").Text);
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 10);
        }
        [Test]
        public void SortBySortOrder()
        {
           GoToAdmin("colors");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCell\"][data-e2e-col-index=\"2\"]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Color1", GetGridCell(0, "ColorName").Text);
            Assert.AreEqual("Color10", GetGridCell(9, "ColorName").Text);
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 10);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCell\"][data-e2e-col-index=\"2\"]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Color111", GetGridCell(0, "ColorName").Text);
            Assert.AreEqual("Color102", GetGridCell(9, "ColorName").Text);
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 10);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCell\"][data-e2e-col-index=\"2\"]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Color1", GetGridCell(0, "ColorName").Text);
            Assert.AreEqual("Color10", GetGridCell(9, "ColorName").Text);
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 10);
        }
    }
}
