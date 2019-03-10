using System;
using NUnit.Framework;
using System.Threading;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using AdvantShop.Selenium.Core.Infrastructure;

namespace AdvantShop.Web.Site.Selenium.Test.Admin.Brand
{
    [TestFixture]
    public class BrandEditTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(
            "data\\Admin\\Catalog\\Brands\\BrandEdit\\Catalog.Product.csv",
           "data\\Admin\\Catalog\\Brands\\BrandEdit\\Catalog.Offer.csv",
           "data\\Admin\\Catalog\\Brands\\BrandEdit\\Catalog.Category.csv",
           "data\\Admin\\Catalog\\Brands\\BrandEdit\\Catalog.Brand.csv",
           "data\\Admin\\Catalog\\Brands\\BrandEdit\\Catalog.ProductCategories.csv");
            Init();
        }


        [Test]
        public void BrandEdit()
        {
            GoToAdmin("brands/edit/1");
    
            driver.FindElement(By.CssSelector(".adv-checkbox-label.form-label-block span")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.Id("BrandName")).Click();
            driver.FindElement(By.Id("BrandName")).Clear();
            driver.FindElement(By.Id("BrandName")).SendKeys("Brand_Name_1_Edit_Changed");
            (new SelectElement(driver.FindElement(By.Id("CountryId")))).SelectByText("Япония");
            driver.FindElement(By.Id("BrandSiteUrl")).Click();
            driver.FindElement(By.Id("BrandSiteUrl")).Clear();
            driver.FindElement(By.Id("BrandSiteUrl")).SendKeys("www.testsite.ru");

            SetCkText("Brand_1_Changed_Description_here", "Description");
            SetCkText("Brand_1_Changed_Brief_Description_here", "BriefDescription");

            ScrollTo(By.Name("DefaultMeta"));
            driver.FindElement(By.Name("UrlPath")).Click();
            driver.FindElement(By.Name("UrlPath")).Clear();
            driver.FindElement(By.Name("UrlPath")).SendKeys("brand_name_1_url_changed");

            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(3000);

            //check details
            Assert.IsTrue(GetButton(eButtonType.Simple, "ViewBrand").GetAttribute("href").Contains("/manufacturers/brand_name_1_url_changed"));
            Assert.AreEqual("www.testsite.ru", driver.FindElement(By.Id("BrandSiteUrl")).GetAttribute("value"));

            //check grid
            GoToAdmin("brands");
            GetGridFilter().SendKeys("Brand_Name_1_Edit_Changed");
            DropFocus("h1");
            Blur();
            Assert.AreEqual("Brand_Name_1_Edit_Changed", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("Япония", GetGridCell(0, "CountryName").Text);

            //check client grid
            GoToClient("manufacturers");
            driver.FindElement(By.Id("SearchBrand")).SendKeys("Brand_Name_1_Edit_Changed");
            Thread.Sleep(5000);
            driver.FindElement(By.CssSelector(".btn-ghost.icon-search-before-abs")).Click();
            Thread.Sleep(5000);
            Assert.AreEqual(driver.FindElement(By.CssSelector(".brand-name")).Text, "Brand_Name_1_Edit_Changed");
            Assert.IsTrue(driver.PageSource.Contains("Brand_1_Changed_Brief_Description_here"));

            GoToClient("manufacturers");
            (new SelectElement(driver.FindElement(By.Id("country")))).SelectByText("Япония");
            DropFocus("h1");
            Assert.IsTrue(driver.PageSource.Contains("Brand_Name_1_Edit_Changed"));

            //check client details
            GoToClient("manufacturers/brand_name_1_url_changed");
            Assert.IsTrue(driver.PageSource.Contains("Brand_Name_1_Edit_Changed"));
            Assert.AreEqual(driver.FindElement(By.LinkText("Сайт производителя")).GetAttribute("href"), "http://www.testsite.ru/");
            Assert.IsTrue(driver.PageSource.Contains("Brand_1_Changed_Description_here"));
        }
        
        [Test]
        public void EditBrandCheckSEO()
        {
            GoToAdmin("brands/edit/6");
            ScrollTo(By.Name("DefaultMeta"));
            if (driver.FindElement(By.Id("DefaultMeta")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"DefaultMetaCheckbox\"]")).Click();
            }
            WaitForElem(By.Id("SeoTitle"));
            driver.FindElement(By.Id("SeoTitle")).SendKeys("New_Brand_Title");
            driver.FindElement(By.Id("SeoH1")).SendKeys("New_Brand_H1");
            driver.FindElement(By.Id("SeoKeywords")).SendKeys("New_Brand_SeoKeywords");
            driver.FindElement(By.Id("SeoDescription")).SendKeys("New_Brand_SeoDescription");

            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            /* change seo */
            GoToAdmin("brands/edit/6");
            ScrollTo(By.Name("DefaultMeta"));

            driver.FindElement(By.Id("SeoTitle")).Clear();
            driver.FindElement(By.Id("SeoTitle")).SendKeys("Brand_6_Changed_Title");
            driver.FindElement(By.Id("SeoH1")).Clear();
            driver.FindElement(By.Id("SeoH1")).SendKeys("Brand_6_Changed_H1");
            driver.FindElement(By.Id("SeoKeywords")).Clear();
            driver.FindElement(By.Id("SeoKeywords")).SendKeys("Brand_6_Changed_SeoKeywords");
            driver.FindElement(By.Id("SeoDescription")).Clear();
            driver.FindElement(By.Id("SeoDescription")).SendKeys("Brand_6_Changed_SeoDescription");

            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            //check admin 
            Assert.AreEqual("Brand_6_Changed_Title", driver.FindElement(By.Id("SeoTitle")).GetAttribute("value"));
            Assert.AreEqual("Brand_6_Changed_H1", driver.FindElement(By.Id("SeoH1")).GetAttribute("value"));
            Assert.AreEqual("Brand_6_Changed_SeoKeywords", driver.FindElement(By.Id("SeoKeywords")).GetAttribute("value"));
            Assert.AreEqual("Brand_6_Changed_SeoDescription", driver.FindElement(By.Id("SeoDescription")).GetAttribute("value"));

            //check client 
            GoToClient("manufacturers/6");
            Assert.AreEqual("Brand_6_Changed_Title", driver.Title);
            Assert.AreEqual("Brand_6_Changed_H1", driver.FindElement(By.TagName("h1")).Text);
            Assert.AreEqual("Brand_6_Changed_SeoKeywords", driver.FindElement(By.CssSelector("[name=\"Keywords\"]")).GetAttribute("content"));
            Assert.AreEqual("Brand_6_Changed_SeoDescription", driver.FindElement(By.CssSelector("[name=\"Description\"]")).GetAttribute("content"));
        }
        
        [Test]
        public void EditABrandCheckDisabled()
        {
            GoToAdmin("brands/edit/10");

            driver.FindElement(By.CssSelector(".adv-checkbox-label.form-label-block span")).Click();
            Thread.Sleep(2000);

            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            GoToAdmin("brands");
            GetGridFilter().SendKeys("BrandName10");
            DropFocus("h1");
            Blur();
            Assert.IsFalse(GetGridCell(0, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);

            //check client 
            GoToClient("manufacturers");
            driver.FindElement(By.Id("SearchBrand")).SendKeys("BrandName10");
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".btn-ghost.icon-search-before-abs")).Click();
            Thread.Sleep(2000);
            var element = driver.FindElement(By.LinkText("BrandName100"));
            IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
            jse.ExecuteScript("arguments[0].scrollIntoView(true)", element);
            Thread.Sleep(2000);
            Assert.IsTrue(driver.FindElements(By.CssSelector(".brand-item.col-xs-12.col-sm-6.col-md-4.col-lg-3")).Count == 6);
            Assert.IsTrue(Is404Page("manufacturers/10"));
        }
        
        [Test]
        public void AEditBrandCheckSort()
        {
            GoToAdmin("brands/edit/40");
            driver.FindElement(By.Id("SortOrder")).Clear();
            driver.FindElement(By.Id("SortOrder")).SendKeys("1");

            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(1000);

            GoToAdmin("brands");
            GetGridFilter().SendKeys("BrandName40");
            DropFocus("h1");
            Blur();
            Assert.AreEqual("1", GetGridCell(0, "SortOrder").FindElement(By.TagName("input")).GetAttribute("value"));

            //check client
            GoToClient("manufacturers");
            var element = driver.FindElements(By.CssSelector(".brand-item.col-xs-12.col-sm-6.col-md-4.col-lg-3"))[0];
            IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
            jse.ExecuteScript("arguments[0].scrollIntoView(true)", element);
            Assert.AreEqual("BrandName40", driver.FindElements(By.CssSelector(".brand-item.col-xs-12.col-sm-6.col-md-4.col-lg-3"))[0].FindElement(By.ClassName("brand-name")).Text);
        }

        [Test]
        public void EditBrandCheckAddImg()
        {
            GoToAdmin("brands/edit/7");
            ScrollTo(By.XPath("//h2[contains(text(), 'Изображения')]"));
            driver.FindElement(By.XPath("(//input[@type='file'])[2]")).SendKeys(GetPicturePath("brand_logo.jpg"));
            Thread.Sleep(2000);

            GoToAdmin("brands");
            GetGridFilter().SendKeys("BrandName7");
            DropFocus("h1");
            Blur();
            Assert.AreEqual("BrandName7", GetGridCell(0, "BrandName").Text);
            Assert.IsFalse(GetGridCell(0, "PhotoSrc").FindElement(By.TagName("img")).GetAttribute("src").Contains("nophoto"));
            string picFirst = GetGridCell(0, "PhotoSrc").FindElement(By.TagName("img")).GetAttribute("src");
            GetGridCell(0, "BrandName").FindElement(By.TagName("a")).Click();
            Thread.Sleep(4000);

            ScrollTo(By.XPath("//h2[contains(text(), 'Изображения')]"));
            driver.FindElement(By.XPath("(//input[@type='file'])[2]")).Clear();
            driver.FindElement(By.XPath("(//input[@type='file'])[2]")).SendKeys(GetPicturePath("brandeditpic.jpg"));
            Thread.Sleep(2000);

            GoToAdmin("brands");
            WaitForAjax();
            GetGridFilter().SendKeys("BrandName7");
            DropFocus("h1");
            Blur();
            Assert.AreEqual("BrandName7", GetGridCell(0, "BrandName").Text);
            Assert.IsFalse(GetGridCell(0, "PhotoSrc").FindElement(By.TagName("img")).GetAttribute("src").Contains("nophoto"));
            string picSecond = GetGridCell(0, "PhotoSrc").FindElement(By.TagName("img")).GetAttribute("src");
            Assert.IsFalse(picFirst.Equals(picSecond));
            
            //check client
            GoToClient("manufacturers/7");
            string strClient = driver.FindElement(By.CssSelector(".logo-container.center-aligner img")).GetAttribute("src");
            Assert.IsFalse(strClient.Contains("nophoto"));
        }

        [Test]
        public void EditBrandCheckAddImgByHref()
        {
            GoToAdmin("brands/edit/8");
            ScrollTo(By.XPath("//h2[contains(text(), 'Изображения')]"));

            driver.FindElement(By.CssSelector("[data-e2e=\"imgByHref\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".modal-body input")).Click();
            driver.FindElement(By.CssSelector(".modal-body input")).Clear();
            driver.FindElement(By.CssSelector(".modal-body input")).SendKeys("https://upload.wikimedia.org/wikipedia/en/thumb/3/34/Mandaue_Cebu.png/80px-Mandaue_Cebu.png");
            driver.FindElement(By.CssSelector("[data-e2e=\"imgByHrefBtnSave\"]")).Click();
            WaitForAjax();

            GoToAdmin("brands");
            GetGridFilter().SendKeys("BrandName8");
            DropFocus("h1");
            Blur();
            Assert.AreEqual("BrandName8", GetGridCell(0, "BrandName").Text);
            Assert.IsFalse(GetGridCell(0, "PhotoSrc").FindElement(By.TagName("img")).GetAttribute("src").Contains("nophoto"));
            string picFirst = GetGridCell(0, "PhotoSrc").FindElement(By.TagName("img")).GetAttribute("src");
            
            GoToAdmin("brands/edit/8");
            ScrollTo(By.XPath("//h2[contains(text(), 'Изображения')]"));

            driver.FindElement(By.CssSelector("[data-e2e=\"imgByHref\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".modal-body input")).Click();
            driver.FindElement(By.CssSelector(".modal-body input")).Clear();
            driver.FindElement(By.CssSelector(".modal-body input")).SendKeys("https://pbs.twimg.com/profile_images/642876497267703808/QD_L-v4q_400x400.jpg");
            driver.FindElement(By.CssSelector("[data-e2e=\"imgByHrefBtnSave\"]")).Click();
            WaitForAjax();

            GoToAdmin("brands");
            GetGridFilter().SendKeys("BrandName8");
            DropFocus("h1");
            Blur();

            Assert.AreEqual("BrandName8", GetGridCell(0, "BrandName").Text);
            string strAdmin = GetGridCell(0, "PhotoSrc").FindElement(By.TagName("img")).GetAttribute("src");
            Assert.IsFalse(strAdmin.Contains("nophoto"));
            Assert.IsFalse(picFirst.Equals(strAdmin));

            //check client
            GoToClient("manufacturers/8");
            string strClient = driver.FindElement(By.CssSelector(".logo-container.center-aligner img")).GetAttribute("src");
            Assert.IsFalse(strClient.Contains("nophoto"));
        }


        [Test]
        public void EditBrandCheckDeleteImg()
        {
            GoToAdmin("brands/edit/9");
            ScrollTo(By.XPath("//h2[contains(text(), 'Изображения')]"));
            driver.FindElement(By.XPath("(//input[@type='file'])[2]")).SendKeys(GetPicturePath("brandeditpic.jpg"));
            Thread.Sleep(2000);

            //img added
            GoToAdmin("brands");
            GetGridFilter().SendKeys("BrandName9");
            DropFocus("h1");
            Blur();
            Assert.AreEqual("BrandName9", GetGridCell(0, "BrandName").Text);
            Assert.IsFalse(GetGridCell(0, "PhotoSrc").FindElement(By.TagName("img")).GetAttribute("src").Contains("nophoto"));

            //check delete img
            GetGridCell(0, "BrandName").FindElement(By.TagName("a")).Click();
            Thread.Sleep(4000);
            ScrollTo(By.XPath("//h2[contains(text(), 'Изображения')]"));
            driver.FindElement(By.CssSelector("[data-e2e=\"imgDel\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);

            GoToAdmin("brands");
            GetGridFilter().SendKeys("BrandName9");
            DropFocus("h1");
            Blur();
            Assert.IsTrue(GetGridCell(0, "PhotoSrc").FindElement(By.TagName("img")).GetAttribute("src").Contains("nophoto"));
            Assert.AreEqual("BrandName9", GetGridCell(0, "BrandName").Text);

            //check client
            GoToClient("manufacturers/9");
            string strClient = driver.FindElement(By.CssSelector(".logo-container.center-aligner img")).GetAttribute("src");
            Assert.IsTrue(strClient.Contains("nophoto"));
        }

        [Test]
        public void EditBrandDelete()
        {
            GoToAdmin("brands/edit/94");
            driver.FindElement(By.XPath("//a[contains(text(), 'Удалить')]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);

            GoToAdmin("brands");
            GetGridFilter().SendKeys("BrandName94");
            DropFocus("h1");
            Blur();
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));

            //check client grid 
            GoToClient("manufacturers");
            driver.FindElement(By.Id("SearchBrand")).SendKeys("BrandName94");
            driver.FindElement(By.CssSelector(".btn-ghost.icon-search-before-abs")).Click();
            Thread.Sleep(5000);
            Assert.IsTrue(driver.FindElements(By.CssSelector(".brand-item.col-xs-12.col-sm-6.col-md-4.col-lg-3")).Count == 0);
            Assert.IsTrue(Is404Page("manufacturers/94"));
        }
    }
}
