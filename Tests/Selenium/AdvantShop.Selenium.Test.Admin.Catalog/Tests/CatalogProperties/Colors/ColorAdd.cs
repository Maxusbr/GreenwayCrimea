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
    public class ColorAdd : BaseSeleniumTest
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
        public void AddNameColor()
        {
           GoToAdmin("colors");
            driver.FindElement(By.CssSelector(".btn.btn-sm.btn-success.btn--margin-left")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(driver.FindElement(By.CssSelector(".modal-dialog")).Displayed);
            driver.FindElement(By.CssSelector("[data-e2e=\"codColor\"] input")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"codColor\"] input")).SendKeys("#180000");
            driver.FindElement(By.CssSelector("[data-e2e=\"nameColor\"]")).SendKeys("New_name_color");
            driver.FindElement(By.CssSelector("[data-e2e=\"sortColor\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"sortColor\"]")).SendKeys("-2");
            driver.FindElement(By.CssSelector("[data-e2e=\"codColor\"] input")).Click();
            DropFocus("h2");
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"saveColor\"]")).Click();
            Thread.Sleep(2000);
            GetGridCell(-1, "SortOrder").Click();
            Thread.Sleep(2000);
            Assert.AreEqual("New_name_color", GetGridCell(0, "ColorName").Text);
            Assert.AreEqual("color: rgb(24, 0, 0);", GetGridCell(0, "ColorIcon").FindElement(By.TagName("i")).GetAttribute("style"));
        }
        
        [Test]
        public void AddIconColor()
        {
           GoToAdmin("colors");
            driver.FindElement(By.CssSelector(".btn.btn-sm.btn-success.btn--margin-left")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("input[type=\"file\"]")).Clear();
            driver.FindElement(By.CssSelector("input[type=\"file\"]")).SendKeys(GetPicturePath("color.png"));
            driver.FindElement(By.CssSelector("[data-e2e=\"nameColor\"]")).SendKeys("New_name_color_icon");
            driver.FindElement(By.CssSelector("[data-e2e=\"sortColor\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"sortColor\"]")).SendKeys("-1");
            DropFocus("h2");
            driver.FindElement(By.CssSelector("[data-e2e=\"saveColor\"]")).Click();
            Thread.Sleep(2000);
            GetGridCell(-1, "SortOrder").Click();
            Assert.AreEqual("New_name_color_icon", GetGridCell(0, "ColorName").Text);
            string str = GetGridCell(0, "ColorIcon").FindElement(By.TagName("img")).GetAttribute("src");
            Assert.IsTrue(str.Contains("pictures/color/details"));
        }
        [Test]
        public void AddNewColorCancel()
        {
           GoToAdmin("colors");
            driver.FindElement(By.CssSelector(".btn.btn-sm.btn-success.btn--margin-left")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"nameColor\"]")).SendKeys("New_name_color_cancel");
            driver.FindElement(By.CssSelector("[data-e2e=\"sortColor\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"sortColor\"]")).SendKeys("-5");
            driver.FindElement(By.CssSelector("[data-e2e=\"cancelColor\"]")).Click();
            Thread.Sleep(2000);
            GetGridCell(-1, "SortOrder").Click();
            Assert.AreNotEqual("New_name_color_cancel", GetGridCell(0, "ColorName").Text);
        }
        //Mozila browser automatically fiils field cod color
        /*
        [Test]
        public void AddOnlyNameColor()
        {
            GoToAdmin("colors");
            driver.FindElement(By.CssSelector(".btn.btn-sm.btn-success.btn--margin-left")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(driver.FindElement(By.CssSelector(".modal-dialog")).Displayed);
            driver.FindElement(By.CssSelector("[data-e2e=\"nameColor\"]")).SendKeys("New_onlyname_color");
            driver.FindElement(By.CssSelector("[data-e2e=\"sortColor\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"sortColor\"]")).SendKeys("-6");
          
            driver.FindElement(By.CssSelector("[data-e2e=\"saveColor\"]")).Click();
            Thread.Sleep(1000);
            Assert.IsTrue(driver.FindElement(By.CssSelector(".toast-container")).Displayed);
            Assert.IsTrue(driver.FindElement(By.CssSelector(".modal-dialog")).Displayed);
            driver.FindElement(By.ClassName("close")).Click();
            Thread.Sleep(2000);
            GetGridCell(-1, "SortOrder").Click();
            Thread.Sleep(2000);
            Assert.AreEqual("New_name_color", GetGridCell(0, "ColorName").Text);
        }
        */
    }
}
