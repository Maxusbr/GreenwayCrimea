using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;
using OpenQA.Selenium.Interactions;

namespace AdvantShop.SeleniumTest.Admin.CMS.Menu
{
    [TestFixture]
    public class CMSMenuEditEnabled : BaseSeleniumTest
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
        public void CMSMenuMainEditEnabled()
        {
            Assert.IsTrue(Is404Page("pages/Main_Menu4_Disabled"));
            GoToClient();
            
            Assert.IsFalse(driver.FindElement(By.CssSelector(".row.rel.big-z.menu-block")).Text.Contains("Main Menu4_Disabled"));

            GoToAdmin("menus");

            driver.FindElement(By.Id("9")).Click();
            driver.FindElement(By.CssSelector(".jstree-anchor.jstree-clicked")).FindElement(By.CssSelector(".link-invert.fa.fa-pencil.link-decoration-none.menu-item-action-item")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemEnabled\"]")).Click();

            WaitForElemEnabled(By.XPath("//button[contains(text(), 'Сохранить')]"));
            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(1000);

            //check admin
            GoToAdmin("menus");

            driver.FindElement(By.Id("9")).Click();
            driver.FindElement(By.CssSelector(".jstree-anchor.jstree-clicked")).FindElement(By.CssSelector(".link-invert.fa.fa-pencil.link-decoration-none.menu-item-action-item")).Click();
            WaitForElem(By.CssSelector(".modal-header-title"));
            Thread.Sleep(2000);
            Assert.AreEqual("Редактирование элемента меню", driver.FindElement(By.TagName("h2")).Text);

            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemEnabledSelected\"]")).Selected);

            //check client
            GoToClient();

            

            Assert.IsTrue(driver.FindElement(By.CssSelector(".row.rel.big-z.menu-block")).Text.Contains("Main Menu4_Disabled"));
            Assert.IsTrue(Is404Page("pages/Main_Menu4_Disabled")); //static page disabled
        }

        [Test]
        public void CMSMenuMainEditDisabled()
        {
            GoToClient();
            
            Assert.IsTrue(driver.FindElement(By.CssSelector(".row.rel.big-z.menu-block")).Text.Contains("Main Menu2_Blank"));

            GoToAdmin("menus");

            driver.FindElement(By.Id("2")).Click();
            driver.FindElement(By.CssSelector(".jstree-anchor.jstree-clicked")).FindElement(By.CssSelector(".link-invert.fa.fa-pencil.link-decoration-none.menu-item-action-item")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemEnabled\"]")).Click();

            WaitForElemEnabled(By.XPath("//button[contains(text(), 'Сохранить')]"));
            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(1000);

            //check admin
            GoToAdmin("menus");

            driver.FindElement(By.Id("2")).Click();
            driver.FindElement(By.CssSelector(".jstree-anchor.jstree-clicked")).FindElement(By.CssSelector(".link-invert.fa.fa-pencil.link-decoration-none.menu-item-action-item")).Click();
            WaitForElem(By.CssSelector(".modal-header-title"));
            Thread.Sleep(2000);
            Assert.AreEqual("Редактирование элемента меню", driver.FindElement(By.TagName("h2")).Text);

            Assert.IsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemEnabledSelected\"]")).Selected);

            //check client
            GoToClient();

            Assert.IsFalse(driver.FindElement(By.CssSelector(".row.rel.big-z.menu-block")).Text.Contains("Main Menu2_Blank"));
            Assert.IsFalse(Is404Page("pages/Main_Menu2_Blank")); //static page enabled
        }


        [Test]
        public void CMSMenuMainChildEditDisabled()
        {
            GoToClient();
            
            Assert.IsTrue(driver.FindElements(By.CssSelector(".menu-general-sub-column")).Count == 1);

            GoToAdmin("menus");

            driver.FindElement(By.Id("2")).FindElement(By.CssSelector(".jstree-icon.jstree-ocl")).Click();
            WaitForElem(By.Id("3"));
            driver.FindElement(By.Id("3")).FindElement(By.CssSelector(".link-invert.fa.fa-pencil.link-decoration-none.menu-item-action-item")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemEnabled\"]")).Click();

            WaitForElemEnabled(By.XPath("//button[contains(text(), 'Сохранить')]"));
            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(1000);

            //check admin
            GoToAdmin("menus");

            driver.FindElement(By.Id("2")).FindElement(By.CssSelector(".jstree-icon.jstree-ocl")).Click();
            WaitForElem(By.Id("3"));
            driver.FindElement(By.Id("3")).FindElement(By.CssSelector(".link-invert.fa.fa-pencil.link-decoration-none.menu-item-action-item")).Click();
            WaitForElem(By.CssSelector(".modal-header-title"));
            Thread.Sleep(2000);
            Assert.AreEqual("Редактирование элемента меню", driver.FindElement(By.TagName("h2")).Text);

            Assert.IsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemEnabledSelected\"]")).Selected);

            //check client
            GoToClient();

            Assert.IsFalse(driver.FindElements(By.CssSelector(".menu-general-sub-column")).Count > 0);
            Assert.IsFalse(Is404Page("pages/Main_Menu3")); //static page enabled
            Assert.IsTrue(driver.FindElement(By.CssSelector(".row.rel.big-z.menu-block")).Text.Contains("Main Menu2_Blank"));
        }


        [Test]
        public void CMSMenuBottomEditEnabled()
        {
            GoToClient();
            
            Assert.IsFalse(driver.FindElement(By.CssSelector(".site-footer")).Text.Contains("Bottom SubMenu1_Disabled"));

            GoToAdmin("menus");

            driver.FindElement(By.Id("4")).FindElement(By.CssSelector(".jstree-icon.jstree-ocl")).Click();
            WaitForElem(By.Id("13"));
            driver.FindElement(By.Id("13")).FindElement(By.CssSelector(".link-invert.fa.fa-pencil.link-decoration-none.menu-item-action-item")).Click();
            Thread.Sleep(1000);

            Assert.IsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemEnabledSelected\"]")).Selected);

            driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemEnabled\"]")).Click();

            WaitForElemEnabled(By.XPath("//button[contains(text(), 'Сохранить')]"));
            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(1000);

            //check admin
            GoToAdmin("menus");

            driver.FindElement(By.Id("4")).FindElement(By.CssSelector(".jstree-icon.jstree-ocl")).Click();
            WaitForElem(By.Id("13"));
            driver.FindElement(By.Id("13")).FindElement(By.CssSelector(".link-invert.fa.fa-pencil.link-decoration-none.menu-item-action-item")).Click();
            WaitForElem(By.CssSelector(".modal-header-title"));
            Thread.Sleep(2000);
            Assert.AreEqual("Редактирование элемента меню", driver.FindElement(By.TagName("h2")).Text);

            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemEnabledSelected\"]")).Selected);

            //check client
            GoToClient();

            Assert.IsTrue(driver.FindElement(By.CssSelector(".site-footer")).Text.Contains("Bottom SubMenu1_Disabled"));
            Assert.IsFalse(Is404Page("pages/Bottom_SubMenu1_Disabled")); //static page enabled
        }

        [Test]
        public void CMSMenuMobileEditEnabled()
        {
            GoToAdmin("menus");

            driver.FindElement(By.Id("11")).Click();
            driver.FindElement(By.CssSelector(".jstree-anchor.jstree-clicked")).FindElement(By.CssSelector(".link-invert.fa.fa-pencil.link-decoration-none.menu-item-action-item")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemEnabled\"]")).Click();

            WaitForElemEnabled(By.XPath("//button[contains(text(), 'Сохранить')]"));
            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(1000);

            //check admin
            GoToAdmin("menus");

            driver.FindElement(By.Id("11")).Click();
            driver.FindElement(By.CssSelector(".jstree-anchor.jstree-clicked")).FindElement(By.CssSelector(".link-invert.fa.fa-pencil.link-decoration-none.menu-item-action-item")).Click();
            Thread.Sleep(2000);

            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemEnabledSelected\"]")).Selected);
            Assert.AreEqual("Редактирование элемента меню", driver.FindElement(By.TagName("h2")).Text);

            Functions.AdminMobileOn(driver, baseURL);

            //check client
            GoToClient("?forcedmobile=true");

            driver.FindElement(By.CssSelector(".toggle-sidebar-icon.icon-menu-before.icon-margin-drop.cs-t-8")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(driver.FindElement(By.CssSelector(".menu")).Text.Contains("Mobile Menu3_Disabled"));

            Functions.AdminMobileOff(driver, baseURL);
        }
    }
}
