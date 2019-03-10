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
    public class TagEdit : BaseSeleniumTest
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
        public void AOpenPage()
        {
            GoToAdmin("tags");
            GetGridCell(0, "Name").Click();
            Thread.Sleep(5000);
            WaitForElem(By.CssSelector("#cke_Description iframe"));
            Assert.AreEqual("Тег New_Tag1", driver.FindElement(By.TagName("h1")).Text);
            Assert.AreEqual("New_Tag1", driver.FindElement(By.Id("Name")).GetAttribute("value"));

            AssertCkText("new new new1", "Description");
            AssertCkText("new tag1", "BriefDescription");

            Assert.AreEqual("teg1", driver.FindElement(By.Id("URL")).GetAttribute("value"));
        }
        [Test]
        public void AOpenPageByPencil()
        {
            GoToAdmin("tags");

            GetGridCell(0, "_serviceColumn").FindElement(By.TagName("a")).Click();
            Thread.Sleep(5000);
            WaitForElem(By.CssSelector("#cke_Description iframe"));
            Assert.AreEqual("Тег New_Tag1", driver.FindElement(By.TagName("h1")).Text);
            Assert.AreEqual("New_Tag1", driver.FindElement(By.Id("Name")).GetAttribute("value"));

            AssertCkText("new new new1", "Description");
            AssertCkText("new tag1", "BriefDescription");

            Assert.AreEqual("teg1", driver.FindElement(By.Id("URL")).GetAttribute("value"));
        }
        [Test]
        public void AOpenAllTags()
        {
            GoToAdmin("tags");
            GetGridCell(0, "Name").Click();
            Thread.Sleep(5000);
            WaitForElem(By.CssSelector("#cke_Description iframe"));
            Assert.AreEqual("Тег New_Tag1", driver.FindElement(By.TagName("h1")).Text);
            driver.FindElement(By.CssSelector(".link-invert")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(driver.Url.Contains("/adminv2/tags"));
            Assert.AreEqual("New_Tag1", GetGridCell(0, "Name").Text);
        }
        [Test]
        public void DelTag()
        {
            GoToAdmin("tags");
            GetGridCell(0, "Name").Click();
            Thread.Sleep(5000);
            WaitForElem(By.CssSelector("#cke_Description iframe"));
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys("New_new_Tag");
            DropFocus("h1");
            driver.FindElement(By.CssSelector("[data-e2e=\"SaveTag\"]")).Click();
            Thread.Sleep(2000);
            
            driver.FindElement(By.CssSelector(".link-danger.m-r-xs")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.ClassName("swal2-cancel")).Click();
            Thread.Sleep(3000);
            GoToAdmin("tags");
            GetGridFilter().SendKeys("New_new_Tag");
            DropFocus("h1");
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count  == 1);
        }

        public void DelTagOk()
        {
            GoToAdmin("tags");
            driver.FindElement(By.CssSelector(".btn.btn-sm.btn-success.btn--margin-left")).Click();
            Thread.Sleep(5000);
            WaitForElem(By.CssSelector("#cke_Description iframe"));
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys("New_new_Tagdel");
            DropFocus("h1");
            driver.FindElement(By.CssSelector("[data-e2e=\"SaveTag\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".link-danger.m-r-xs")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(3000);
            GetGridFilter().SendKeys("New_new_Tagdel");
            DropFocus("h1");
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count  == 0);
        }
   
        [Test]
        public void SaveNameTag()
        {
            GoToAdmin("tags");
            GetGridCell(0, "Name").Click();
            Thread.Sleep(5000);
            WaitForElem(By.CssSelector("#cke_Description iframe"));
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys("New_Name_Tag1");
            DropFocus("h1");
            driver.FindElement(By.CssSelector(".adv-checkbox-emul")).Click();
            SetCkText("Edit_Tag_Description_here", "Description");
            SetCkText("Edit_Tag_Brief_Description_here", "BriefDescription");

            driver.FindElement(By.Name("UrlPath")).Clear();
            driver.FindElement(By.Name("UrlPath")).SendKeys("newtesttag1");
            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"SaveTag\"]")).Click();
            Thread.Sleep(2000);
          //  Assert.AreEqual("Тег New_new_Tag", driver.FindElement(By.TagName("h1")).Text);
            //в админке 
            GoToAdmin("tags");
            GetGridFilter().SendKeys("New_Name_Tag1");
            DropFocus("h1");
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count  == 1);
            Assert.IsFalse(GetGridCell(0, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);
            GetGridCell(0, "Name").Click();
            Thread.Sleep(5000);
            WaitForElem(By.CssSelector("#cke_Description iframe"));
            AssertCkText("Edit_Tag_Description_here", "Description");
            AssertCkText("Edit_Tag_Brief_Description_here", "BriefDescription");
        }

        [Test]
        public void TagCheckaSEOaH1()
        {
            GoToAdmin("tags");
            GetGridCell(0, "Name").Click();
            Thread.Sleep(5000);
            WaitForElem(By.CssSelector("#cke_Description iframe"));
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys("New_Tag_H1");
            Thread.Sleep(2000);
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
            GetGridFilter().SendKeys("New_Tag_H1");
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
