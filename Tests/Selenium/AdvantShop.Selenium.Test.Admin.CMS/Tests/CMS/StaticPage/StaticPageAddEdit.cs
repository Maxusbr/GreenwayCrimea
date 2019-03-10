using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using AdvantShop.Selenium.Core.Infrastructure;

namespace AdvantShop.SeleniumTest.Admin.CMS.StaticPage
{
    [TestFixture]
    public class StaticPageAddEdit : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.CMS);
            InitializeService.LoadData(
           "data\\Admin\\CMS\\StaticPage\\CMS.StaticPage.csv"
           );

            Init();
        }
        [Test]
        public void EditPage()
        {
            GoToAdmin("staticpages");
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("page1");
            DropFocus("h1");
            GetGridCell(0, "PageName").Click();
            Thread.Sleep(3000);
            Assert.AreEqual("Статическая страница \"Page1\"", driver.FindElement(By.TagName("h1")).Text);
            Assert.AreEqual("Page1", driver.FindElement(By.Name("PageName")).GetAttribute("value"));
            Assert.AreEqual("1", driver.FindElement(By.Name("SortOrder")).GetAttribute("value"));
            Assert.AreEqual("page1", driver.FindElement(By.Name("UrlPath")).GetAttribute("value"));

            AssertCkText("text1", "PageText");
          
            driver.FindElement(By.Name("PageName")).Clear();
            driver.FindElement(By.Name("PageName")).SendKeys("EditName");
            driver.FindElement(By.CssSelector(".edit")).Click();
            GetGridFilter().SendKeys("page100");
            DropFocus("h2");
           driver.FindElement(By.CssSelector(".ui-grid-cell-contents a")).Click();
            Thread.Sleep(1000);
            
            driver.FindElement(By.Name("UrlPath")).Clear();
            driver.FindElement(By.Name("UrlPath")).SendKeys("pageedit1");
            SetCkText("edited text 1", "PageText");
            GetButton(eButtonType.Save).Click();

            GoToAdmin("staticpages");
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("EditName");
            DropFocus("h1");
            Assert.AreEqual("EditName", GetGridCell(0, "PageName").Text);
            Assert.AreEqual("true", driver.FindElement(By.Name("switchOnOff_0")).GetAttribute("value"));

            GoToClient("pages/page100");
            Assert.IsTrue(driver.FindElements(By.CssSelector(".news-categories")).Count > 0);
            driver.FindElement(By.CssSelector(".news-categories a")).Click();
            Thread.Sleep(1000);
            Assert.AreEqual("EditName", driver.FindElement(By.TagName("h1")).Text);
            Assert.AreEqual("edited text 1", driver.FindElement(By.CssSelector(".staticpage-content")).Text);
        }
        [Test]
        public void EditEnabledPage()
        {
            GoToAdmin("staticpages");
            Assert.IsFalse(Is404Page("pages/page2"));
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Page2");
            DropFocus("h1");
            GetGridCell(0, "PageName").Click();
            Thread.Sleep(1000);
            Assert.AreEqual("Статическая страница \"Page2\"", driver.FindElement(By.TagName("h1")).Text);
            Assert.AreEqual("Page2", driver.FindElement(By.Name("PageName")).GetAttribute("value"));
            driver.FindElement(By.CssSelector(".adv-checkbox-label.form-label-block")).Click();
            Thread.Sleep(1000);

            GetButton(eButtonType.Save).Click();
            Thread.Sleep(1000);
            GoToAdmin("staticpages");
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Page2");
            DropFocus("h1");
            Assert.AreEqual("false", driver.FindElement(By.Name("switchOnOff_0")).GetAttribute("value"));
            Assert.IsTrue(Is404Page("pages/page2"));
        }
        [Test]
        public void AddPage()
        {
            GoToAdmin("staticpages");

            GetButton(eButtonType.Add).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.Name("PageName")).SendKeys("NewName");
            driver.FindElement(By.CssSelector(".edit")).Click();
            GetGridFilter().SendKeys("page5");
            DropFocus("h2");
            driver.FindElement(By.CssSelector(".ui-grid-cell-contents a")).Click();
            Thread.Sleep(1000);
            SetCkText("new text 1", "PageText");
            driver.FindElement(By.Name("UrlPath")).Clear();
            driver.FindElement(By.Name("UrlPath")).SendKeys("newpage");
            if (driver.FindElement(By.Id("DefaultMeta")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"DefaultMetaCheckbox\"]")).Click();
            }
            Thread.Sleep(2000);
            driver.FindElement(By.Id("SeoTitle")).SendKeys("New_Category_Title");
            driver.FindElement(By.Id("SeoH1")).SendKeys("New_Category_H1");
            driver.FindElement(By.Id("SeoKeywords")).SendKeys("New_Category_SeoKeywords");
            driver.FindElement(By.Id("SeoDescription")).SendKeys("New_Category_SeoDescription");
                   
          
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            //check admin grid
            GoToAdmin("staticpages");
            GetGridFilter().SendKeys("NewName");
            DropFocus("h1");
            Assert.AreEqual("NewName", GetGridCell(0, "PageName").Text);
            Assert.AreEqual("true", driver.FindElement(By.Name("switchOnOff_0")).GetAttribute("value"));
            
            //check admin details
            GetGridCell(0, "PageName").FindElement(By.TagName("a")).Click();
            Thread.Sleep(2000);
            WaitForElem(By.Id("PageName"));

            Assert.IsTrue(driver.FindElement(By.Id("Enabled")).Selected);
            Assert.IsTrue(driver.FindElement(By.Id("IndexAtSiteMap")).Selected);

            Assert.AreEqual("NewName", driver.FindElement(By.Name("PageName")).GetAttribute("value"));
            Assert.AreEqual("newpage", driver.FindElement(By.Name("UrlPath")).GetAttribute("value"));
            AssertCkText("new text 1", "PageText");

            Assert.IsFalse(driver.FindElement(By.Id("DefaultMeta")).Selected);

            Assert.AreEqual("New_Category_Title", driver.FindElement(By.Id("SeoTitle")).GetAttribute("value"));
            Assert.AreEqual("New_Category_H1", driver.FindElement(By.Id("SeoH1")).GetAttribute("value"));
            Assert.AreEqual("New_Category_SeoKeywords", driver.FindElement(By.Id("SeoKeywords")).GetAttribute("value"));
            Assert.AreEqual("New_Category_SeoDescription", driver.FindElement(By.Id("SeoDescription")).GetAttribute("value"));

            //check client
            GoToClient("pages/page5");
            Assert.IsTrue(driver.FindElements(By.CssSelector(".news-categories")).Count > 0);
            driver.FindElement(By.CssSelector(".news-categories a")).Click();
            Thread.Sleep(1000);
            Assert.AreEqual("new text 1", driver.FindElement(By.CssSelector(".staticpage-content")).Text);
            Assert.AreEqual("New_Category_Title", driver.Title);
            Assert.AreEqual("New_Category_H1", driver.FindElement(By.TagName("h1")).Text);
            Assert.AreEqual("New_Category_SeoKeywords", driver.FindElement(By.CssSelector("[name=\"Keywords\"]")).GetAttribute("content"));
            Assert.AreEqual("New_Category_SeoDescription", driver.FindElement(By.CssSelector("[name=\"Description\"]")).GetAttribute("content"));


        }

    }
}
