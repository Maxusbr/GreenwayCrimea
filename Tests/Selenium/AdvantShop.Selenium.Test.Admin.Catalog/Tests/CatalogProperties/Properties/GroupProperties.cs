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
    public class GroupProperties : BaseSeleniumTest
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
        public void OpenWindowsAdd()
        {
            GoToAdmin("properties");
            driver.FindElement(By.CssSelector(".pull-right.header-alt-icons")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(driver.FindElement(By.CssSelector(".modal-dialog")).Displayed);
         }
        [Test]
        public void GroupAdd()
        {
            GoToAdmin("properties");
            driver.FindElement(By.CssSelector(".pull-right.header-alt-icons")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(driver.FindElement(By.CssSelector(".modal-dialog")).Displayed);
            driver.FindElement(By.CssSelector(".col-xs-9 input")).SendKeys("New_group");
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(driver.PageSource.Contains("New_group"));
        }
        [Test]
        public void GroupAddCancel()
        {
            GoToAdmin("properties");
            driver.FindElement(By.CssSelector(".pull-right.header-alt-icons")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(driver.FindElement(By.CssSelector(".modal-dialog")).Displayed);
            driver.FindElement(By.CssSelector(".col-xs-9 input")).SendKeys("New_new_group");
            driver.FindElement(By.CssSelector(".btn.btn-default.btn-cancel")).Click();
            Thread.Sleep(2000);
            Assert.IsFalse(driver.PageSource.Contains("New_new_group"));
        }
        [Test]
        public void GroupEdit()
        {
            GoToAdmin("properties");
            driver.FindElement(By.XPath("//a[contains(text(), 'New_group')]")).Click();
            Thread.Sleep(1000);
            Refresh();         
             driver.FindElement(By.CssSelector(".link-invert.aside-menu-row.aside-menu-row-with-move.selected ui-modal-trigger")).Click();
            Thread.Sleep(2000);           
            Assert.IsTrue(driver.FindElement(By.CssSelector(".modal-dialog")).Displayed);
            driver.FindElement(By.CssSelector(".col-xs-9 input")).Clear();
            driver.FindElement(By.CssSelector(".col-xs-9 input")).SendKeys("Newname_group");
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(driver.PageSource.Contains("Newname_group"));
        }
        [Test]
        public void GroupEditCancel()
        {
            GoToAdmin("properties");
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("//a[contains(text(), 'Newname_group')]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".link-invert.aside-menu-row.aside-menu-row-with-move.selected ui-modal-trigger")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(driver.FindElement(By.CssSelector(".modal-dialog")).Displayed);
            driver.FindElement(By.CssSelector(".col-xs-9 input")).Clear();
            driver.FindElement(By.CssSelector(".col-xs-9 input")).SendKeys("Newname_group_edit");
            driver.FindElement(By.CssSelector(".btn.btn-default.btn-cancel")).Click();
            Thread.Sleep(2000);
            Assert.IsFalse(driver.PageSource.Contains("Newname_group_edit"));
        }
        [Test]
        public void GroupzDeletCancel()
        {
            GoToAdmin("properties?groupId=11");
            string name = driver.FindElement(By.TagName("h1")).Text;
            driver.FindElements(By.CssSelector(".link-invert.aside-menu-row.selected a"))[2].Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-cancel")).Click();
            Thread.Sleep(1000);
            Assert.IsTrue(driver.PageSource.Contains(name));
        }
        [Test]
        public void GroupzDelOk()
        {
            GoToAdmin("properties?groupId=11");
            string name = driver.FindElement(By.TagName("h1")).Text;
            driver.FindElements(By.CssSelector(".link-invert.aside-menu-row.selected a"))[2].Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(1000);
          //  GoToAdmin("properties");
           // Thread.Sleep(2000);
            Assert.IsFalse(driver.PageSource.Contains(name));
        }
    }
}
