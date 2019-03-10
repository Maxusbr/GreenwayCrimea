using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;

namespace AdvantShop.SeleniumTest.Admin.CMS.Menu
{
    [TestFixture]
    public class CMSMenuDeleteItem : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.CMS);
            InitializeService.LoadData(
               "data\\Admin\\CMS\\Menu\\CMS.Menu.csv",
            "data\\Admin\\CMS\\Menu\\CMS.StaticBlock.csv",
               "data\\Admin\\CMS\\Menu\\CMS.StaticPage.csv"
           );

            Init();
        }
        
        [Test]
        public void CMSMenuMainDelete()
        {
            GoToAdmin("menus");

            //check admin
            driver.FindElement(By.Id("1")).FindElement(By.CssSelector(".link-invert.fa.fa-remove.link-decoration-none.m-l-sm.menu-item-action-item")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);

            GoToAdmin("menus");

            Assert.IsFalse(driver.PageSource.Contains("Main Menu1"));

            //check client
            GoToClient();

            Assert.IsFalse(driver.PageSource.Contains("Main Menu1"));
        }

        [Test]
        public void CMSMenuSubMainDelete()
        {
            GoToAdmin("menus");

            //check admin
            driver.FindElement(By.Id("2")).FindElement(By.CssSelector(".jstree-icon.jstree-ocl")).Click();
            WaitForElem(By.Id("3"));
            driver.FindElement(By.Id("3")).FindElement(By.CssSelector(".link-invert.fa.fa-remove.link-decoration-none.m-l-sm.menu-item-action-item")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);

            GoToAdmin("menus");

            Assert.IsFalse(driver.PageSource.Contains("Main Menu3"));

            //check client
            GoToClient();

            Assert.IsFalse(driver.FindElements(By.CssSelector(".menu-general-sub-column")).Count > 0);
            Assert.IsFalse(driver.PageSource.Contains("Main Menu3"));
        }

        [Test]
        public void CMSMenuBottomDelete()
        {
            GoToAdmin("menus");

            //check admin
            driver.FindElement(By.Id("4")).FindElement(By.CssSelector(".link-invert.fa.fa-remove.link-decoration-none.m-l-sm.menu-item-action-item")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);

            GoToAdmin("menus");

            Assert.IsFalse(driver.PageSource.Contains("Bottom Menu1"));

            //check client
            GoToClient();

            Assert.IsFalse(driver.PageSource.Contains("Bottom Menu1"));
        }

        [Test]
        public void CMSMenuSubBottomDelete()
        {
            GoToAdmin("menus");

            //check admin
            driver.FindElement(By.Id("5")).FindElement(By.CssSelector(".jstree-icon.jstree-ocl")).Click();
            WaitForElem(By.Id("6"));
            driver.FindElement(By.Id("6")).FindElement(By.CssSelector(".link-invert.fa.fa-remove.link-decoration-none.m-l-sm.menu-item-action-item")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);

            GoToAdmin("menus");

            Assert.IsFalse(driver.PageSource.Contains("Bottom Menu3_Blank"));

            //check client
            GoToClient();

            Assert.IsFalse(driver.FindElement(By.CssSelector(".site-footer")).Text.Contains("Bottom Menu3_Blank"));
        }

        [Test]
        public void CMSMenuMobileDelete()
        {
            GoToAdmin("menus");

            //check admin
            driver.FindElement(By.Id("7")).FindElement(By.CssSelector(".link-invert.fa.fa-remove.link-decoration-none.m-l-sm.menu-item-action-item")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);

            GoToAdmin("menus");

            Assert.IsFalse(driver.PageSource.Contains("Mobile Menu1"));

            Functions.AdminMobileOn(driver, baseURL);

            //check client
            GoToClient("?forcedmobile=true");
            driver.FindElement(By.CssSelector(".toggle-sidebar-icon.icon-menu-before.icon-margin-drop.cs-t-8")).Click();
            Assert.IsFalse(driver.PageSource.Contains("Mobile Menu1"));

            Functions.AdminMobileOff(driver, baseURL);
        }
    }
}
