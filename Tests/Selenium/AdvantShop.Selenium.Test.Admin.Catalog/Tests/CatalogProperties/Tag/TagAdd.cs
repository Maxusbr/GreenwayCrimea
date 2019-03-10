using System;
using NUnit.Framework;
using System.Threading;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;

namespace AdvantShop.SeleniumTest.Admin.Catalog.Tag
{
    [TestFixture]
    public class TagAdd : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(
                "Data\\Admin\\Catalog\\Tag\\Catalog.Category.csv",
                "Data\\Admin\\Catalog\\Tag\\Catalog.Tag.csv",
                "Data\\Admin\\Catalog\\Tag\\Catalog.Product.csv",
                "Data\\Admin\\Catalog\\Tag\\Catalog.ProductCategories.csv",
                "Data\\Admin\\Catalog\\Tag\\Catalog.TagMap.csv"
                );             
            Init();
        }        
       
        [Test]
        public void DelTagCancel()
        {
            GoToAdmin("tags");
            driver.FindElement(By.CssSelector(".btn.btn-sm.btn-success.btn--margin-left")).Click();
            Thread.Sleep(5000);
            WaitForElem(By.CssSelector("#cke_Description iframe"));
            driver.FindElement(By.Id("Name")).SendKeys("New_new_Tag");
            DropFocus("h1");
            driver.FindElement(By.CssSelector("[data-e2e=\"SaveTag\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".link-danger.m-r-xs")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.ClassName("swal2-cancel")).Click();
            Thread.Sleep(1000);

            GoToAdmin("tags");
            GetGridFilter().SendKeys("New_new_Tag");
            DropFocus("h1");           
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 1);
        }
        [Test]
        public void DelTagOk()
        {
            GoToAdmin("tags");
            driver.FindElement(By.CssSelector(".btn.btn-sm.btn-success.btn--margin-left")).Click();
            Thread.Sleep(5000);
            WaitForElem(By.CssSelector("#cke_Description iframe"));
            driver.FindElement(By.Id("Name")).SendKeys("New_new_Tagdel");
            DropFocus("h1");
            driver.FindElement(By.CssSelector("[data-e2e=\"SaveTag\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".link-danger.m-r-xs")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(1000);
            GetGridFilter().SendKeys("New_new_Tagdel");
            DropFocus("h1");           
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 0);
        }
        
        [Test]
        public void OpenAllTags()
        {
            GoToAdmin("tags");
            driver.FindElement(By.CssSelector(".btn.btn-sm.btn-success.btn--margin-left")).Click();
            Thread.Sleep(5000);
            WaitForElem(By.CssSelector("#cke_Description iframe"));
            Assert.AreEqual("Новый тег", driver.FindElement(By.CssSelector("h1")).Text);
            driver.FindElement(By.CssSelector(".link-invert")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(driver.Url.Contains("/adminv2/tags"));
        }
        [Test]
        public void OpenPage()
        {
            GoToAdmin("tags");
            driver.FindElement(By.CssSelector(".btn.btn-sm.btn-success.btn--margin-left")).Click();
            Thread.Sleep(5000);
            WaitForElem(By.CssSelector("#cke_Description iframe"));
            Assert.AreEqual("Новый тег", driver.FindElement(By.TagName("h1")).Text);
        }
        [Test]
        public void SaveNameTag()
        {
            GoToAdmin("tags");
            driver.FindElement(By.CssSelector(".btn.btn-sm.btn-success.btn--margin-left")).Click();
            Thread.Sleep(5000);
            WaitForElem(By.CssSelector("#cke_Description iframe"));
            driver.FindElement(By.Id("Name")).SendKeys("New_Name_Tag1");
            DropFocus("h1");
            driver.FindElement(By.CssSelector(".adv-checkbox-emul")).Click();
            SetCkText("Edit_Tag_Description_here", "Description");
            SetCkText("Edit_Tag_Brief_Description_here", "BriefDescription");
            driver.FindElement(By.Name("UrlPath")).Clear();
            driver.FindElement(By.Name("UrlPath")).SendKeys("newtesttag1");
            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"SaveTag\"]")).Click();
            Thread.Sleep(1000);
            //в админке 
            GoToAdmin("tags");
            GetGridFilter().SendKeys("New_Name_Tag1");
            DropFocus("h1");
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 1);
            Assert.AreEqual("newtesttag1", GetGridCell(0, "UrlPath").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.IsFalse(GetGridCell(0, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);
            GetGridCell(0, "Name").Click();
            Thread.Sleep(5000);
            WaitForElem(By.CssSelector("#cke_Description iframe"));
            AssertCkText("Edit_Tag_Description_here", "Description");
            AssertCkText("Edit_Tag_Brief_Description_here", "BriefDescription");
          
        }    
      
        [Test]
        public void TagCheckMeta1()
        {
            GoToAdmin("tags");
            driver.FindElement(By.CssSelector(".btn.btn-sm.btn-success.btn--margin-left")).Click();
            Thread.Sleep(5000);
            WaitForElem(By.CssSelector("#cke_Description iframe"));
            driver.FindElement(By.Id("Name")).SendKeys("New_Meta");
            ScrollTo(By.Name("DefaultMeta"));
            driver.FindElements(By.CssSelector(".adv-checkbox-emul"))[2].Click();
            ScrollTo(By.TagName("footer"));
            driver.FindElement(By.Id("SeoH1")).SendKeys("New_Tag_H1");
            driver.FindElement(By.Id("SeoKeywords")).SendKeys("New_Tag_SeoKeywords");
            driver.FindElement(By.Id("SeoDescription")).SendKeys("New_Tag_SeoDescription");
            driver.FindElement(By.Id("SeoTitle")).SendKeys("New_Category_Title");
            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"SaveTag\"]")).Click();
            Thread.Sleep(2000);

            GoToAdmin("tags");
            GetGridFilter().SendKeys("New_Meta");
            DropFocus("h1");
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 1);
            GetGridCell(0, "Name").Click();
            Thread.Sleep(5000);
            WaitForElem(By.CssSelector("#cke_Description iframe"));
            Assert.AreEqual("New_Tag_H1", driver.FindElement(By.Id("SeoH1")).GetAttribute("value"));
            Assert.AreEqual("New_Tag_SeoKeywords", driver.FindElement(By.Id("SeoKeywords")).GetAttribute("value"));
            Assert.AreEqual("New_Tag_SeoDescription", driver.FindElement(By.Id("SeoDescription")).GetAttribute("value"));
            Assert.AreEqual("New_Category_Title", driver.FindElement(By.Id("SeoTitle")).GetAttribute("value"));
        }
    }
}
