using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Catalog.ProductAddEdit.Video
{
    [TestFixture]
    public class ProductAddEditVideoTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(
           "data\\Admin\\Catalog\\ProductAddEdit\\Catalog.Product.csv",
           "data\\Admin\\Catalog\\ProductAddEdit\\Catalog.Offer.csv",
           "data\\Admin\\Catalog\\ProductAddEdit\\Catalog.Category.csv",
           "data\\Admin\\Catalog\\ProductAddEdit\\Catalog.ProductCategories.csv",
           "data\\Admin\\Catalog\\ProductAddEdit\\Catalog.Brand.csv",
           "data\\Admin\\Catalog\\ProductAddEdit\\Catalog.Tag.csv",
             "data\\Admin\\Catalog\\ProductAddEdit\\Catalog.Property.csv",
                 "data\\Admin\\Catalog\\ProductAddEdit\\Catalog.PropertyValue.csv",
                 "data\\Admin\\Catalog\\ProductAddEdit\\Catalog.ProductPropertyValue.csv",
                "data\\Admin\\Catalog\\ProductAddEdit\\Catalog.PropertyGroup.csv",
                "Data\\Admin\\Catalog\\ProductAddEdit\\Catalog.Color.csv",
                "data\\Admin\\Catalog\\ProductAddEdit\\Catalog.Size.csv"
           );

            Init();
        }

        [Test]
        public void ProductEditVideoYouTubeAdd()
        {
            GoToClient("products/test-product1");
            Assert.IsFalse(driver.FindElements(By.CssSelector(".prod-photo-view-change.video")).Count > 0);

            GoToAdmin("product/edit/1");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Видео')]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoAdd\"]")).Click();
            Thread.Sleep(1000);
            Assert.AreEqual("Видео", driver.FindElement(By.TagName("h2")).Text);

            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoName\"]")).SendKeys("VideoNameYouTube");
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoLink\"]")).Click();
            Assert.IsTrue(driver.PageSource.Contains("Cсылка на видео"));
            Assert.IsFalse(driver.PageSource.Contains("Код плеера"));
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoLinkAdd\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoLinkAdd\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoLinkAdd\"]")).SendKeys("https://www.youtube.com/watch?v=N7sGh4KKDg8");
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoDesc\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoDesc\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoDesc\"]")).SendKeys("VideoDescriptoin");
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoSortOrder\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoSortOrder\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoSortOrder\"]")).SendKeys("10");

            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("product/edit/1");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Видео')]")).Click();
            Thread.Sleep(1000);

            Assert.AreEqual("VideoNameYouTube", GetGridCell(0, "Name", "Videos").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("10", GetGridCell(0, "VideoSortOrder", "Videos").FindElement(By.TagName("input")).GetAttribute("value"));

            //check client
            GoToClient("products/test-product1");
            Assert.IsTrue(driver.FindElements(By.CssSelector(".prod-photo-view-change.video")).Count == 1);
            Assert.IsTrue(driver.PageSource.Contains("VideoDescriptoin"));
        }


        [Test]
        public void ProductEditVideoCodeAdd()
        {
            GoToClient("products/test-product4");
            Assert.IsFalse(driver.FindElements(By.CssSelector(".prod-photo-view-change.video")).Count > 0);

            GoToAdmin("product/edit/4");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Видео')]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoAdd\"]")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoName\"]")).SendKeys("VideoNameCode");
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoCode\"]")).Click();
            Assert.IsFalse(driver.PageSource.Contains("Cсылка на видео"));
            Assert.IsTrue(driver.PageSource.Contains("Код плеера"));
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoCodeAdd\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoCodeAdd\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoCodeAdd\"]")).SendKeys("LetItBeCode");
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoDesc\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoDesc\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoDesc\"]")).SendKeys("VideoDescriptoin");
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoSortOrder\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoSortOrder\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoSortOrder\"]")).SendKeys("10");

            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("product/edit/4");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Видео')]")).Click();
            Thread.Sleep(1000);

            Assert.AreEqual("VideoNameCode", GetGridCell(0, "Name", "Videos").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("10", GetGridCell(0, "VideoSortOrder", "Videos").FindElement(By.TagName("input")).GetAttribute("value"));

            //check client
            GoToClient("products/test-product4");
            Assert.IsTrue(driver.FindElements(By.CssSelector(".prod-photo-view-change.video")).Count == 1);
            Assert.IsTrue(driver.PageSource.Contains("VideoDescriptoin"));
        }

        [Test]
        public void ProductEditVideoEdit()
        {
            GoToAdmin("product/edit/5");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Видео')]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoAdd\"]")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoName\"]")).SendKeys("VideoNameYouTube");
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoLink\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoLinkAdd\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoLinkAdd\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoLinkAdd\"]")).SendKeys("https://www.youtube.com/watch?v=N7sGh4KKDg8");
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoDesc\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoDesc\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoDesc\"]")).SendKeys("VideoDescriptoin");
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoSortOrder\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoSortOrder\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoSortOrder\"]")).SendKeys("10");

            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(2000);

            GoToAdmin("product/edit/5");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Видео')]")).Click();
            Thread.Sleep(1000);

            GetGridCell(0, "_serviceColumn", "Videos").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(1000);
            WaitForElem(By.CssSelector(".modal-content"));

            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoName\"]")).SendKeys("EditName");
            
            IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
            jse.ExecuteScript("arguments[0].scrollIntoView(true)", driver.FindElement(By.CssSelector(".modal-content")).FindElement(By.CssSelector("[data-e2e=\"ProductVideoCodeAdd\"]")));
            Thread.Sleep(1000);

            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoDesc\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoDesc\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoDesc\"]")).SendKeys("VideoDescEdited");
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoSortOrder\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoSortOrder\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoSortOrder\"]")).SendKeys("50");

            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("product/edit/5");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Видео')]")).Click();
            Thread.Sleep(1000);

            Assert.AreEqual("EditName", GetGridCell(0, "Name", "Videos").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("50", GetGridCell(0, "VideoSortOrder", "Videos").FindElement(By.TagName("input")).GetAttribute("value"));

            //check client
            GoToClient("products/test-product5");
            Assert.IsTrue(driver.FindElements(By.CssSelector(".prod-photo-view-change.video")).Count == 1);
            Assert.IsTrue(driver.PageSource.Contains("VideoDescEdited"));
        }

        [Test]
        public void ProductEditVideoEditInplace()
        {
            GoToAdmin("product/edit/6");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Видео')]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoAdd\"]")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoName\"]")).SendKeys("VideoNameCheckInplace");
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoLink\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoLinkAdd\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoLinkAdd\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoLinkAdd\"]")).SendKeys("https://www.youtube.com/watch?v=N7sGh4KKDg8");
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoDesc\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoDesc\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoDesc\"]")).SendKeys("VideoDescriptoin");
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoSortOrder\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoSortOrder\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoSortOrder\"]")).SendKeys("10");

            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(2000);

            GoToAdmin("product/edit/6");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Видео')]")).Click();
            Thread.Sleep(1000);
            GetGridCell(0, "Name", "Videos").FindElement(By.TagName("input")).Click();
            GetGridCell(0, "Name", "Videos").FindElement(By.TagName("input")).Clear();
            GetGridCell(0, "Name", "Videos").FindElement(By.TagName("input")).SendKeys("EditNameInplace");
            GetGridCell(0, "VideoSortOrder", "Videos").FindElement(By.TagName("input")).Click();
            GetGridCell(0, "VideoSortOrder", "Videos").FindElement(By.TagName("input")).Clear();
            GetGridCell(0, "VideoSortOrder", "Videos").FindElement(By.TagName("input")).SendKeys("999");
            driver.FindElement(By.XPath("//h2[contains(text(), 'Видео')]")).Click();
            
            //check admin
            GoToAdmin("product/edit/6");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Видео')]")).Click();
            Thread.Sleep(1000);

            Assert.AreEqual("EditNameInplace", GetGridCell(0, "Name", "Videos").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("999", GetGridCell(0, "VideoSortOrder", "Videos").FindElement(By.TagName("input")).GetAttribute("value"));

            //check client
            GoToClient("products/test-product6");
            Assert.IsTrue(driver.FindElements(By.CssSelector(".prod-photo-view-change.video")).Count == 1);
            Assert.IsTrue(driver.PageSource.Contains("VideoDescriptoin"));
        }
        
        [Test]
        public void ProductEditVideoAddSeveral()
        {
            GoToClient("products/test-product7");
            Assert.IsFalse(driver.FindElements(By.CssSelector(".prod-photo-view-change.video")).Count > 0);

            GoToAdmin("product/edit/7");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Видео')]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoAdd\"]")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoName\"]")).SendKeys("Video1");
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoCode\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoCodeAdd\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoCodeAdd\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoCodeAdd\"]")).SendKeys("LetItBeCode");
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoDesc\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoDesc\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoDesc\"]")).SendKeys("VideoDescriptoin1");
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoSortOrder\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoSortOrder\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoSortOrder\"]")).SendKeys("10");

            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(2000);

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Видео')]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoAdd\"]")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoName\"]")).SendKeys("Video2");
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoLink\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoLinkAdd\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoLinkAdd\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoLinkAdd\"]")).SendKeys("https://www.youtube.com/watch?v=N7sGh4KKDg8");
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoDesc\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoDesc\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoDesc\"]")).SendKeys("VideoDescriptoin2");
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoSortOrder\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoSortOrder\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoSortOrder\"]")).SendKeys("20");
            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("product/edit/7");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Видео')]")).Click();
            Thread.Sleep(1000);
            
            Assert.AreEqual("Video1", GetGridCell(0, "Name", "Videos").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("10", GetGridCell(0, "VideoSortOrder", "Videos").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("Video2", GetGridCell(1, "Name", "Videos").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("20", GetGridCell(1, "VideoSortOrder", "Videos").FindElement(By.TagName("input")).GetAttribute("value"));

            //check client
            GoToClient("products/test-product7");
            Assert.IsTrue(driver.FindElements(By.CssSelector(".prod-photo-view-change.video")).Count == 1);
            Assert.IsTrue(driver.PageSource.Contains("VideoDescriptoin1"));
            Assert.IsTrue(driver.PageSource.Contains("VideoDescriptoin2"));
        }

        [Test]
        public void ProductEditVideoDeleteFromSeveral()
        {
            GoToClient("products/test-product8");
            Assert.IsFalse(driver.FindElements(By.CssSelector(".prod-photo-view-change.video")).Count > 0);

            GoToAdmin("product/edit/8");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Видео')]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoAdd\"]")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoName\"]")).SendKeys("Video1");
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoCode\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoCodeAdd\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoCodeAdd\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoCodeAdd\"]")).SendKeys("LetItBeCode");
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoDesc\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoDesc\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoDesc\"]")).SendKeys("VideoDescriptoin1");
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoSortOrder\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoSortOrder\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoSortOrder\"]")).SendKeys("10");

            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(2000);

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Видео')]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoAdd\"]")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoName\"]")).SendKeys("Video2");
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoLink\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoLinkAdd\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoLinkAdd\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoLinkAdd\"]")).SendKeys("https://www.youtube.com/watch?v=N7sGh4KKDg8");
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoDesc\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoDesc\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoDesc\"]")).SendKeys("VideoDescriptoin2");
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoSortOrder\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoSortOrder\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoSortOrder\"]")).SendKeys("20");
            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(2000);

            GoToAdmin("product/edit/8");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Видео')]")).Click();
            Thread.Sleep(1000);
            GetGridCell(1, "_serviceColumn", "Videos").FindElement(By.TagName("ui-grid-custom-delete")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("product/edit/8");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Видео')]")).Click();
            Thread.Sleep(1000);

            Assert.AreEqual("Video1", GetGridCell(0, "Name", "Videos").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("10", GetGridCell(0, "VideoSortOrder", "Videos").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.IsFalse(driver.PageSource.Contains("Video2"));

            //check client
            GoToClient("products/test-product8");
            Assert.IsTrue(driver.FindElements(By.CssSelector(".prod-photo-view-change.video")).Count == 1);
            Assert.IsTrue(driver.PageSource.Contains("VideoDescriptoin1"));
            Assert.IsFalse(driver.PageSource.Contains("VideoDescriptoin2"));
        }


        [Test]
        public void ProductEditVideoDelete()
        {
            GoToClient("products/test-product9");
            Assert.IsFalse(driver.FindElements(By.CssSelector(".prod-photo-view-change.video")).Count > 0);

            GoToAdmin("product/edit/9");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Видео')]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoAdd\"]")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoName\"]")).SendKeys("Video1");
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoCode\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoCodeAdd\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoCodeAdd\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoCodeAdd\"]")).SendKeys("LetItBeCode");
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoDesc\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoDesc\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoDesc\"]")).SendKeys("VideoDescriptoin1");
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoSortOrder\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoSortOrder\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductVideoSortOrder\"]")).SendKeys("10");

            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Video1", GetGridCell(0, "Name", "Videos").FindElement(By.TagName("input")).GetAttribute("value"));

            GoToAdmin("product/edit/9");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Видео')]")).Click();
            Thread.Sleep(1000);
            GetGridCell(0, "_serviceColumn", "Videos").FindElement(By.TagName("ui-grid-custom-delete")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("product/edit/9");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Видео')]")).Click();
            Thread.Sleep(1000);

            Assert.IsFalse(driver.PageSource.Contains("Video1"));

            //check client
            GoToClient("products/test-product9");
            Assert.IsFalse(driver.FindElements(By.CssSelector(".prod-photo-view-change.video")).Count == 1);
            Assert.IsFalse(driver.PageSource.Contains("VideoDescriptoin1"));
        }
    }
}