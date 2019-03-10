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
    public class PropertiesAdd : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(
               // "Data\\Admin\\Properties\\Catalog.Category.csv",
              //  "Data\\Admin\\Properties\\Catalog.Brand.csv",
            //    "Data\\Admin\\Properties\\Catalog.Property.csv",
             //    "Data\\Admin\\Properties\\Catalog.PropertyValue.csv",
               //  "Data\\Admin\\Properties\\Catalog.ProductPropertyValue.csv",
              //  "Data\\Admin\\Properties\\Catalog.Product.csv",
            //    "Data\\Admin\\Properties\\Catalog.Offer.csv",
            //    "Data\\Admin\\Properties\\Catalog.ProductCategories.csv",
                "Data\\Admin\\Properties\\Catalog.PropertyGroup.csv"
                );

             
            Init();

        }
        
        [Test]
        public void OpenWindowsAdd()
        {
           GoToAdmin("properties");
            driver.FindElement(By.CssSelector(".btn.btn-sm.btn-success.btn--margin-left")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(driver.FindElement(By.CssSelector(".modal-dialog")).Displayed);
        }
        [Test]
        public void AddProperties()
        {
           GoToAdmin("properties");
            driver.FindElement(By.CssSelector(".btn.btn-sm.btn-success.btn--margin-left")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".col-xs-9 input")).SendKeys("New_name_Check");
            driver.FindElement(By.CssSelector(".col-xs-9 textarea")).SendKeys("New_descriptions");
            driver.FindElement(By.CssSelector(".adv-checkbox-label")).Click();
            driver.FindElements(By.CssSelector(".adv-checkbox-label"))[1].Click();

            var element = driver.FindElements(By.CssSelector(".col-xs-9 input"))[7];
            IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
            jse.ExecuteScript("arguments[0].scrollIntoView(true)", element);
            Thread.Sleep(1000);

            driver.FindElements(By.CssSelector(".adv-checkbox-label"))[2].Click();
            driver.FindElements(By.CssSelector(".adv-checkbox-label"))[3].Click();
            (new SelectElement(driver.FindElements(By.CssSelector(".col-xs-9 select"))[1])).SelectByText("Group1");
            (new SelectElement(driver.FindElement(By.CssSelector(".col-xs-9 select")))).SelectByText("Раскрывающийся список (селект)");
            driver.FindElements(By.CssSelector(".col-xs-9 input"))[7].Clear();
            driver.FindElements(By.CssSelector(".col-xs-9 input"))[7].SendKeys("-1");

            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("New_name_Check", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Group1", GetGridCell(0, "GroupName").Text);
            Assert.IsFalse(driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[0][\'UseInFilter\']\"] [data-e2e=\"switchOnOffLabel\"] input")).Selected);
            Assert.IsFalse(driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[0][\'UseInDetails\']\"] [data-e2e=\"switchOnOffLabel\"] input")).Selected);
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[0][\'UseInBrief\']\"] [data-e2e=\"switchOnOffLabel\"] input")).Selected);
            driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[0][\'_serviceColumn\']\"] a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("New_descriptions", driver.FindElement(By.CssSelector(".col-xs-9 textarea")).GetAttribute("value"));
         }     
    }
}
