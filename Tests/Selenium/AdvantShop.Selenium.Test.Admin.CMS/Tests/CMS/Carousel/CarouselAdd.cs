using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using AdvantShop.Selenium.Core.Infrastructure;

namespace AdvantShop.SeleniumTest.Admin.CMS.Carousel
{
    [TestFixture]
    public class CarouselAdd : BaseMultiSeleniumTest
    {
        [SetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.CMS | ClearType.Catalog);
            InitializeService.LoadData(
           "data\\Admin\\CMS\\Carousel\\Catalog.Product.csv",
           "data\\Admin\\CMS\\Carousel\\Catalog.Offer.csv",
           "data\\Admin\\CMS\\Carousel\\Catalog.Category.csv",
           "data\\Admin\\CMS\\Carousel\\Catalog.ProductCategories.csv"
           );

            Init();
        }
        [Test]
        public void AddEnabledCarousel()
        {
            GoToAdmin("carousel");
            GetButton(eButtonType.Add).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Добавить изображение карусели", driver.FindElement(By.TagName("h2")).Text);
            driver.FindElement(By.XPath("(//input[@type='file'])[2]")).SendKeys(GetPicturePath("brandeditpic.jpg"));

            driver.FindElement(By.CssSelector("[data-e2e=\"carouselAddUrl\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"carouselAddUrl\"]")).SendKeys("products/test-product10");
            driver.FindElement(By.CssSelector("[data-e2e=\"carouselAddAltTag\"]")).SendKeys("newtag10");
            driver.FindElement(By.CssSelector("[data-e2e=\"carouselAddSortOrder\"]")).SendKeys("0");
            if (!driver.FindElement(By.CssSelector("[data-e2e=\"carouselAddOneColumn\"]")).FindElement(By.TagName("input")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"carouselAddOneColumn\"]")).FindElement(By.TagName("span")).Click();
            }

            if (!driver.FindElement(By.CssSelector("[data-e2e=\"carouselAddTwoColumns\"]")).FindElement(By.TagName("input")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"carouselAddTwoColumns\"]")).FindElement(By.TagName("span")).Click();
            }

            if (!driver.FindElement(By.CssSelector("[data-e2e=\"carouselAddDisplayInMobile\"]")).FindElement(By.TagName("input")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"carouselAddDisplayInMobile\"]")).FindElement(By.TagName("span")).Click();
            }

            ScrollTo(By.CssSelector("[data-e2e=\"carouselAddEnabled\"]"));
            if (!driver.FindElement(By.CssSelector("[data-e2e=\"carouselAddEnabled\"]")).FindElement(By.TagName("input")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"carouselAddEnabled\"]")).FindElement(By.TagName("span")).Click();
            }
            
            driver.FindElement(By.CssSelector("[data-e2e=\"carouselAdd\"]")).Click();
            Thread.Sleep(2000);

            //checking grid
            GoToAdmin("carousel");
            Assert.IsFalse(GetGridCell(0, "ImageSrc").FindElement(By.TagName("img")).GetAttribute("src").Contains("nophoto"));
            Assert.AreEqual("products/test-product10", GetGridCell(0, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("newtag10", GetGridCell(0, "Description").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("0", GetGridCell(0, "SortOrder").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.IsTrue(GetGridCell(0, "DisplayInOneColumn").FindElement(By.CssSelector("input")).Selected);
            Assert.IsTrue(GetGridCell(0, "DisplayInTwoColumns").FindElement(By.CssSelector("input")).Selected);
            Assert.IsTrue(GetGridCell(0, "DisplayInMobile").FindElement(By.CssSelector("input")).Selected);
            Assert.IsTrue(GetGridCell(0, "Enabled").FindElement(By.CssSelector("input")).Selected);
           
            //checking client
            GoToClient();
            Assert.IsTrue(driver.FindElement(By.CssSelector(".carousel-main-item.js-carousel-item a")).GetAttribute("href").Contains("products/test-product10"));
            Assert.AreEqual("newtag10", driver.FindElement(By.CssSelector(".carousel-main-item.js-carousel-item img")).GetAttribute("alt"));
        }

        [Test]
        public void AddDisabledCarousel()
        {
            GoToAdmin("carousel");
            GetButton(eButtonType.Add).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Добавить изображение карусели", driver.FindElement(By.TagName("h2")).Text);
            driver.FindElement(By.CssSelector("[data-e2e=\"imgByHref\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"imgByHrefLinkText\"]")).SendKeys("http://www.fullhdoboi.com/wallpapers/thumbs/6/kartinka-apelsiny-1885.jpg");
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"imgByHrefBtnSave\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"carouselAddUrl\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"carouselAddUrl\"]")).SendKeys("products/test-product11");
            driver.FindElement(By.CssSelector("[data-e2e=\"carouselAddAltTag\"]")).SendKeys("newtag11");
            driver.FindElement(By.CssSelector("[data-e2e=\"carouselAddSortOrder\"]")).SendKeys("1");
            if (driver.FindElement(By.CssSelector("[data-e2e=\"carouselAddOneColumn\"]")).FindElement(By.TagName("input")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"carouselAddOneColumn\"]")).FindElement(By.TagName("span")).Click();
            }

            if (driver.FindElement(By.CssSelector("[data-e2e=\"carouselAddTwoColumns\"]")).FindElement(By.TagName("input")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"carouselAddTwoColumns\"]")).FindElement(By.TagName("span")).Click();
            }

            if (driver.FindElement(By.CssSelector("[data-e2e=\"carouselAddDisplayInMobile\"]")).FindElement(By.TagName("input")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"carouselAddDisplayInMobile\"]")).FindElement(By.TagName("span")).Click();
            }

            ScrollTo(By.CssSelector("[data-e2e=\"carouselAddEnabled\"]"));
            if (driver.FindElement(By.CssSelector("[data-e2e=\"carouselAddEnabled\"]")).FindElement(By.TagName("input")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"carouselAddEnabled\"]")).FindElement(By.TagName("span")).Click();
            }
            driver.FindElement(By.CssSelector("[data-e2e=\"carouselAdd\"]")).Click();
            Thread.Sleep(2000);

            //checking grid
            GoToAdmin("carousel");
            Assert.IsFalse(GetGridCell(0, "ImageSrc").FindElement(By.TagName("img")).GetAttribute("src").Contains("nophoto"));
            Assert.AreEqual("products/test-product11", GetGridCell(0, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("newtag11", GetGridCell(0, "Description").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("1", GetGridCell(0, "SortOrder").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.IsFalse(GetGridCell(0, "DisplayInOneColumn").FindElement(By.CssSelector("input")).Selected);
            Assert.IsFalse(GetGridCell(0, "DisplayInTwoColumns").FindElement(By.CssSelector("input")).Selected);
            Assert.IsFalse(GetGridCell(0, "DisplayInMobile").FindElement(By.CssSelector("input")).Selected);
            Assert.IsFalse(GetGridCell(0, "Enabled").FindElement(By.CssSelector("input")).Selected);
            
            //checking client
            GoToClient();
            Thread.Sleep(2000);
            NoSuchElement();
        }
        

        public void NoSuchElement()
        {
            int count = driver.FindElement(By.CssSelector(".carousel-main-list.text-static.carousel-list")).FindElements(By.TagName("a")).Count;

            for (int i = 0; i < count; i++)
            {
                string link = driver.FindElement(By.CssSelector(".carousel-main-list.text-static.carousel-list")).FindElement(By.CssSelector(".carousel-main-item.js-carousel-item")).FindElement(By.TagName("a")).GetAttribute("href");
                if (link.EndsWith("products/test-product11"))
                {
                    Assert.Fail();
                }

            }
        } 
    }
}
