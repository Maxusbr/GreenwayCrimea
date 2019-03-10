using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using AdvantShop.SeleniumTest.Core;
using AdvantShop.Selenium.Core.Infrastructure;
using System.Threading;

namespace AdvantShop.Web.Site.Selenium.Test.Admin.Brand
{
    [TestFixture]
    public class BrandAddTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest() 
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(
            "data\\Admin\\Catalog\\Brands\\BrandAdd\\Catalog.Product.csv",
           "data\\Admin\\Catalog\\Brands\\BrandAdd\\Catalog.Offer.csv",
           "data\\Admin\\Catalog\\Brands\\BrandAdd\\Catalog.Category.csv",
           "data\\Admin\\Catalog\\Brands\\BrandAdd\\Catalog.Brand.csv",
           "data\\Admin\\Catalog\\Brands\\BrandAdd\\Catalog.ProductCategories.csv");
            Init();
        }

        [Test]
        public void AddNewBrand()
        {
            GoToAdmin("brands");
            GetButton(eButtonType.Add).Click();
            Thread.Sleep(4000);
            Assert.AreEqual("Новый производитель", driver.FindElement(By.TagName("h1")).Text);

            driver.FindElement(By.Id("BrandName")).SendKeys("New_Brand_name");
            (new SelectElement(driver.FindElement(By.Id("CountryId")))).SelectByText("Россия");
            
            driver.FindElement(By.Id("SortOrder")).Clear();
            driver.FindElement(By.Id("SortOrder")).SendKeys("1");
            driver.FindElement(By.Id("BrandSiteUrl")).SendKeys("www.testsite.ru");

            SetCkText("New_Brand_Description_here", "Description");
            ScrollTo(By.Id("cke_BriefDescription"));
            SetCkText("New_Brand_Brief_Description_here", "BriefDescription");

            ScrollTo(By.XPath("//h2[contains(text(), 'Изображения')]"));
            driver.FindElement(By.XPath("(//input[@type='file'])[2]")).SendKeys(GetPicturePath("brand_logo.jpg"));

            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            //cheking details
            GoToAdmin("brands/edit/106");
            Assert.IsTrue(GetButton(eButtonType.Simple, "ViewBrand").GetAttribute("href").Contains("/manufacturers/new_brand"));
            Assert.AreEqual("www.testsite.ru", driver.FindElement(By.Id("BrandSiteUrl")).GetAttribute("value"));

            //cheking grid
            GoToAdmin("brands");
            GetGridFilter().SendKeys("New_Brand_name");
            DropFocus("h1");
            Assert.AreEqual("1", GetGridCell(0, "SortOrder").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("Россия", GetGridCell(0, "CountryName").Text);
            Assert.IsTrue(GetGridCell(0, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);
            Assert.IsFalse(GetGridCell(0, "PhotoSrc").FindElement(By.TagName("img")).GetAttribute("src").Contains("nophoto"));


            //cheking client grid
            GoToClient("manufacturers");
            var element = driver.FindElements(By.CssSelector(".brand-item.col-xs-12.col-sm-6.col-md-4.col-lg-3"))[0];
            Assert.AreEqual("New_Brand_name", driver.FindElements(By.CssSelector(".brand-item.col-xs-12.col-sm-6.col-md-4.col-lg-3"))[0].FindElement(By.ClassName("brand-name")).Text);

            GoToClient("manufacturers");
            driver.FindElement(By.Name("SearchBrand")).SendKeys("New_Brand_name");
            driver.FindElement(By.CssSelector(".btn-ghost.icon-search-before-abs")).Click();
            Thread.Sleep(3000);
            Assert.AreEqual(driver.FindElement(By.CssSelector(".brand-name a")).Text, "New_Brand_name");
            //Assert.AreEqual(driver.FindElement(By.CssSelector(".brand-bDescr div p")).Text, "New_Brand_Brief_Description_here");
            Assert.IsTrue(driver.PageSource.Contains("New_Brand_Brief_Description_here"));
            Assert.IsFalse(driver.PageSource.Contains("New_Brand_Description_here"));

            GoToClient("manufacturers");
            (new SelectElement(driver.FindElement(By.Id("country")))).SelectByText("Россия");
            DropFocus("h1");
            Assert.IsTrue(driver.PageSource.Contains("New_Brand_name"));

            // checking client details
            GoToClient("manufacturers/New_Brand_name");
            Assert.IsTrue(driver.PageSource.Contains("New_Brand_name"));
            Assert.AreEqual(driver.FindElement(By.LinkText("Сайт производителя")).GetAttribute("href"), "http://www.testsite.ru/");
            Assert.IsTrue(driver.PageSource.Contains("New_Brand_Description_here"));
            Assert.IsFalse(driver.PageSource.Contains("New_Brand_Brief_Description_here"));
            Assert.IsFalse(driver.FindElement(By.CssSelector(".logo-container.center-aligner img")).GetAttribute("src").Contains("nophoto"));
        }

        [Test]
        public void AddNewBrandCheckDisabled()
        {
            GoToAdmin("brands/add");
            driver.FindElement(By.Id("BrandName")).SendKeys("New_Brand_Disabled");
            driver.FindElement(By.CssSelector(".adv-checkbox-label.form-label-block span")).Click();
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            GoToAdmin("brands");
            GetGridFilter().SendKeys("New_Brand_Disabled");
            DropFocus("h1");
            Blur();
            Assert.IsFalse(GetGridCell(0, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);

            //client 
            GoToClient("manufacturers");
            driver.FindElement(By.Id("SearchBrand")).SendKeys("New_Brand_Disabled");
            driver.FindElement(By.CssSelector(".btn-ghost.icon-search-before-abs")).Click();
            Thread.Sleep(3000);
            Assert.IsFalse(driver.FindElements(By.CssSelector(".brand-item.col-xs-12.col-sm-6.col-md-4.col-lg-3")).Count > 0);

            Assert.IsTrue(Is404Page("manufacturers/New_Brand_Disabled"));
        }

        [Test]
        public void AddNewBrandCheckSEO()
        {
            GoToAdmin("brands/add");
            driver.FindElement(By.Id("BrandName")).SendKeys("new_brand_seo");
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

            // check admin
            Assert.AreEqual("New_Brand_Title", driver.FindElement(By.Id("SeoTitle")).GetAttribute("value"));
            Assert.AreEqual("New_Brand_H1", driver.FindElement(By.Id("SeoH1")).GetAttribute("value"));
            Assert.AreEqual("New_Brand_SeoKeywords", driver.FindElement(By.Id("SeoKeywords")).GetAttribute("value"));
            Assert.AreEqual("New_Brand_SeoDescription", driver.FindElement(By.Id("SeoDescription")).GetAttribute("value"));

            //check client 
            driver.Navigate().GoToUrl(baseURL + "/manufacturers/new_brand_seo");
            Assert.AreEqual("New_Brand_Title", driver.Title);
            Assert.AreEqual("New_Brand_H1", driver.FindElement(By.TagName("h1")).Text);
            Assert.AreEqual("New_Brand_SeoKeywords", driver.FindElement(By.CssSelector("[name=\"Keywords\"]")).GetAttribute("content"));
            Assert.AreEqual("New_Brand_SeoDescription", driver.FindElement(By.CssSelector("[name=\"Description\"]")).GetAttribute("content"));
        }
      

        [Test]
        public void AddNewBrandCheckAddImgByHref()
        {
            GoToAdmin("brands/add");
            driver.FindElement(By.Id("BrandName")).SendKeys("New_Brand_Img_By_Href");

            ScrollTo(By.CssSelector("[data-e2e=\"imgByHref\"]"));
            driver.FindElement(By.CssSelector("[data-e2e=\"imgByHref\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"imgByHrefLinkText\"]")).SendKeys("https://upload.wikimedia.org/wikipedia/en/thumb/3/34/Mandaue_Cebu.png/80px-Mandaue_Cebu.png");
            driver.FindElement(By.CssSelector("[data-e2e=\"imgByHrefBtnSave\"]")).Click();
            WaitForAjax();

            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            GoToAdmin("brands");
            GetGridFilter().SendKeys("New_Brand_Img_By_Href");

            DropFocus("h1");

            Assert.AreEqual("New_Brand_Img_By_Href", GetGridCell(0, "BrandName").Text);
            Assert.IsFalse(GetGridCell(0, "PhotoSrc").FindElement(By.TagName("img")).GetAttribute("src").Contains("nophoto"));

            //client
            GoToClient("manufacturers/New_Brand_Img_By_Href");
            Assert.IsFalse(driver.FindElement(By.CssSelector(".logo-container.center-aligner img")).GetAttribute("src").Contains("nophoto"));
        }

    }
}
