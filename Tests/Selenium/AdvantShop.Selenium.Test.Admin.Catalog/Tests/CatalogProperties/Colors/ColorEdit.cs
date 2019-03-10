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
    public class ColorEdit : BaseSeleniumTest
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
        public void EditByNameOpenWindows()
        {
           GoToAdmin("colors");
            driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[0][\'ColorName\']\"] a")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(driver.FindElement(By.CssSelector(".modal-dialog")).Displayed);
            Assert.AreEqual("Color1", driver.FindElement(By.CssSelector("[data-e2e=\"nameColor\"]")).GetAttribute("value"));
            Assert.AreEqual("1", driver.FindElement(By.CssSelector("[data-e2e=\"sortColor\"]")).GetAttribute("value"));
            Assert.AreEqual("#000000", driver.FindElement(By.CssSelector("[data-e2e=\"codColor\"] input")).GetAttribute("value"));
        }
        [Test]
        public void EditByPencilOpenWindows()
        {
           GoToAdmin("colors");
            driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[0][\'_serviceColumn\']\"] a")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(driver.FindElement(By.CssSelector(".modal-dialog")).Displayed);
            Assert.AreEqual("Color1", driver.FindElement(By.CssSelector("[data-e2e=\"nameColor\"]")).GetAttribute("value"));
            Assert.AreEqual("1", driver.FindElement(By.CssSelector("[data-e2e=\"sortColor\"]")).GetAttribute("value"));
            Assert.AreEqual("#000000", driver.FindElement(By.CssSelector("[data-e2e=\"codColor\"] input")).GetAttribute("value"));
        }
        [Test]
        public void EditCodColor()
        {
           GoToAdmin("colors");
            driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[0][\'ColorName\']\"] a")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"codColor\"] input")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"codColor\"] input")).SendKeys("#ffffff");
            driver.FindElement(By.CssSelector("[data-e2e=\"saveColor\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCell\"][data-e2e-col-index=\"2\"]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Color1", GetGridCell(0, "ColorName").Text);
            Assert.AreEqual("color: rgb(255, 255, 255);", GetGridCell(0, "ColorIcon").FindElement(By.TagName("i")).GetAttribute("style"));
        }
        [Test]
        public void EditIconColor()
        {
           GoToAdmin("colors");
            driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[0][\'ColorName\']\"] a")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("input[type=\"file\"]")).SendKeys(GetPicturePath("color.png"));
            driver.FindElement(By.CssSelector("[data-e2e=\"saveColor\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCell\"][data-e2e-col-index=\"2\"]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Color1", GetGridCell(0, "ColorName").Text);
            String str = GetGridCell(0, "ColorIcon").FindElement(By.TagName("img")).GetAttribute("src");
            Assert.IsTrue(str.Contains("pictures/color/details"));
        }
        [Test]
        public void EditIconDelColor()
        {
           GoToAdmin("colors");
            driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[0][\'ColorName\']\"] a")).Click();
            Thread.Sleep(2000);            
            String str = driver.FindElement(By.CssSelector(".col-xs-9 img")).GetAttribute("src");
            Assert.IsTrue(str.Contains("pictures/color/details"));
            driver.FindElement(By.LinkText("Удалить")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(driver.FindElements(By.CssSelector(".col-xs-9 img")).Count == 0);
            driver.FindElement(By.CssSelector("[data-e2e=\"saveColor\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCell\"][data-e2e-col-index=\"2\"]")).Click();
            Assert.AreEqual("color: rgb(255, 255, 255);", driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[0][\'ColorIcon\']\"] i")).GetAttribute("style"));
        }
        
        [Test]
        public void EditNameColor()
        {
           GoToAdmin("colors");
            driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[0][\'ColorName\']\"] a")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"nameColor\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"nameColor\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"nameColor\"]")).SendKeys("Edited_name");
            driver.FindElement(By.CssSelector("[data-e2e=\"saveColor\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCell\"][data-e2e-col-index=\"2\"]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Edited_name", GetGridCell(0, "ColorName").Text);
        }
        [Test]
        public void EditSortColor()
        {
           GoToAdmin("colors");
            driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[0][\'ColorName\']\"] a")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"sortColor\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"sortColor\"]")).SendKeys("3");
            driver.FindElement(By.CssSelector("[data-e2e=\"saveColor\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCell\"][data-e2e-col-index=\"2\"]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Color2", GetGridCell(0, "ColorName").Text);
            Assert.AreEqual("Edited_name", GetGridCell(2, "ColorName").Text);
        }
        [Test]
        public void EditColorCancel()
        {
           GoToAdmin("colors");
            driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[0][\'ColorName\']\"] a")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"nameColor\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"nameColor\"]")).SendKeys("New_name_color_cancel");
            driver.FindElement(By.CssSelector("[data-e2e=\"sortColor\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"sortColor\"]")).SendKeys("-5");
            driver.FindElement(By.CssSelector("[data-e2e=\"cancelColor\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCell\"][data-e2e-col-index=\"2\"]")).Click();
            Thread.Sleep(2000);
            Assert.AreNotEqual("New_name_color_cancel", GetGridCell(0, "ColorName").Text);
            Assert.AreEqual("Color2", GetGridCell(1, "ColorName").Text);
        }
    }
}
