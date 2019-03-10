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
    public class CMSMenu : BaseSeleniumTest
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
        public void CMSMenuMain()
        {
            GoToAdmin("menus");

            //check admin
            Assert.IsTrue(driver.FindElement(By.Id("1")).Text.Contains("Main Menu1"));
            Assert.IsTrue(driver.FindElement(By.Id("9")).Text.Contains("Main Menu4_Disabled"));
            driver.FindElement(By.Id("1")).FindElement(By.CssSelector(".jstree-icon.jstree-ocl")).Click();
            WaitForElem(By.Id("12"));
            Assert.IsTrue(driver.FindElement(By.Id("12")).Text.Contains("Main SubMenu5_Disabled"));

            //check client
            GoToClient();

            Assert.IsTrue(driver.FindElement(By.CssSelector(".row.rel.big-z.menu-block")).Text.Contains("Main Menu1"));
            Assert.IsTrue(driver.PageSource.Contains("Main Menu3"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".menu-general-sub-column")).Count == 1);
            Assert.IsFalse(driver.PageSource.Contains("Main SubMenu5_Disabled"));
        }

        [Test]
        public void CMSMenuBottom()
        {
            GoToAdmin("menus");

            //check admin
            Assert.IsTrue(driver.FindElement(By.Id("4")).Text.Contains("Bottom Menu1"));
            Assert.IsTrue(driver.FindElement(By.Id("5")).Text.Contains("Bottom Menu2"));
            Assert.IsTrue(driver.FindElement(By.Id("10")).Text.Contains("Bottom Menu4_Disabled"));
            driver.FindElement(By.Id("5")).FindElement(By.CssSelector(".jstree-icon.jstree-ocl")).Click();
            WaitForElem(By.Id("6"));
            Assert.IsTrue(driver.FindElement(By.Id("6")).Text.Contains("Bottom Menu3_Blank"));

            //check client
            GoToClient();

            Assert.IsTrue(driver.FindElement(By.CssSelector(".site-footer")).FindElements(By.CssSelector(".col-xs.footer-menu"))[0].Text.Contains("Bottom Menu1"));
            Assert.IsTrue(driver.FindElement(By.CssSelector(".site-footer")).FindElements(By.CssSelector(".col-xs.footer-menu"))[1].Text.Contains("Bottom Menu2"));
            Assert.IsTrue(driver.FindElement(By.CssSelector(".site-footer")).FindElements(By.CssSelector(".col-xs.footer-menu"))[1].Text.Contains("Bottom Menu3_Blank"));
            Assert.IsFalse(driver.FindElement(By.CssSelector(".site-footer")).Text.Contains("Bottom Menu4_Disabled"));
        }


        [Test]
        public void CMSMenuMobile()
        {
            GoToAdmin("menus");

            //check admin
            Assert.IsTrue(driver.FindElement(By.Id("7")).Text.Contains("Mobile Menu1"));
            Assert.IsTrue(driver.FindElement(By.Id("8")).Text.Contains("Mobile Menu2"));
            Assert.IsTrue(driver.FindElement(By.Id("11")).Text.Contains("Mobile Menu3_Disabled"));

            Functions.AdminMobileOn(driver, baseURL);

            //check client
            GoToClient("?forcedmobile=true");
            driver.FindElement(By.CssSelector(".toggle-sidebar-icon.icon-menu-before.icon-margin-drop.cs-t-8")).Click();
            Assert.IsTrue(driver.FindElement(By.CssSelector(".menu")).FindElements(By.CssSelector(".left.cs-br-4.cs-bg-14.inked.ink-light"))[0].Text.Contains("Mobile Menu1"));
            Assert.IsTrue(driver.FindElement(By.CssSelector(".menu")).FindElements(By.CssSelector(".left.cs-br-4.cs-bg-14.inked.ink-light"))[1].Text.Contains("Mobile Menu2"));
            Assert.IsFalse(driver.PageSource.Contains("Mobile Menu4_Disabled"));

            Functions.AdminMobileOff(driver, baseURL);
        }

        [Test]
        public void CMSMenuMainBlank()
        {
            GoToAdmin("menus");

            //check admin
            driver.FindElement(By.Id("2")).FindElement(By.CssSelector(".link-invert.fa.fa-pencil.link-decoration-none.menu-item-action-item")).Click();
            WaitForElem(By.CssSelector(".modal-header-title"));
            Thread.Sleep(2000);

            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemBlankSelected\"]")).Selected);

            //check client
            GoToClient();
            driver.FindElement(By.LinkText("Main Menu2_Blank")).Click();
            Thread.Sleep(2000);

            Functions.OpenNewTab(driver, baseURL);
            Assert.IsTrue(driver.WindowHandles.Count.Equals(2));

            Functions.CloseTab(driver, baseURL);
        }

        [Test]
        public void CMSMenuBottomBlank()
        {
            GoToAdmin("menus");

            //check admin
            driver.FindElement(By.Id("5")).FindElement(By.CssSelector(".jstree-icon.jstree-ocl")).Click();
            WaitForElem(By.Id("6"));
            driver.FindElement(By.Id("6")).FindElement(By.CssSelector(".link-invert.fa.fa-pencil.link-decoration-none.menu-item-action-item")).Click();
            WaitForElem(By.CssSelector(".modal-header-title"));
            Thread.Sleep(2000);

            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemBlankSelected\"]")).Selected);

            //check client
            GoToClient();
            
            ScrollTo(By.Name("subscribeEmailField"));
            driver.FindElement(By.LinkText("Bottom Menu3_Blank")).Click();
            Thread.Sleep(3000);

            Functions.OpenNewTab(driver, baseURL);
            Assert.IsTrue(driver.WindowHandles.Count.Equals(2));

            Functions.CloseTab(driver, baseURL);
        }
    }
}
