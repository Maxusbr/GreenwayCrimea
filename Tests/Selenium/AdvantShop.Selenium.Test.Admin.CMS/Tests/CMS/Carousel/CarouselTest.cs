using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;

namespace AdvantShop.SeleniumTest.Admin.CMS.Carousel
{
    [TestFixture]
    public class CarouselTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.CMS | ClearType.Catalog);
            InitializeService.LoadData(
           "data\\Admin\\CMS\\Carousel\\Catalog.Product.csv",
           "data\\Admin\\CMS\\Carousel\\Catalog.Offer.csv",
           "data\\Admin\\CMS\\Carousel\\Catalog.Category.csv",
           "data\\Admin\\CMS\\Carousel\\Catalog.ProductCategories.csv",
           "data\\Admin\\CMS\\Carousel\\CMS.Carousel.csv",
           "data\\Admin\\CMS\\Carousel\\Catalog.Photo.csv"
           );

            Init();
        }
        [Test]
        public void ChekingCarousel()
        {
            GoToAdmin("carousel");

            Assert.IsFalse(GetGridCell(0, "ImageSrc").FindElement(By.TagName("img")).GetAttribute("src").Contains("nophoto"));
            Assert.AreEqual("products/test-product1", GetGridCell(0, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("teg1", GetGridCell(0, "Description").FindElement(By.TagName("input")).GetAttribute("value"));
            // Assert.AreEqual("1", GetGridCell(0, "SortOrder").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".ng-pristine.ng-valid .adv-checkbox-label input"))[0].Selected);
            Assert.IsFalse(driver.FindElements(By.CssSelector(".ng-pristine.ng-valid .adv-checkbox-label input"))[1].Selected);
            Assert.IsTrue(driver.FindElements(By.CssSelector(".ng-pristine.ng-valid .adv-checkbox-label input"))[2].Selected);
            Assert.IsTrue(driver.FindElements(By.CssSelector(".ng-pristine.ng-valid .adv-checkbox-label input"))[3].Selected);

            //в 2 колонки
            GoToClient();
            Assert.AreEqual("3", (driver.FindElements(By.CssSelector(".carousel-main-item.js-carousel-item")).Count - 2).ToString()); //2 clone
            Assert.IsTrue(driver.FindElement(By.CssSelector(".carousel-main-item.js-carousel-item a")).GetAttribute("href").Contains("products/test-product6"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".carousel-main-item.js-carousel-item a"))[1].GetAttribute("href").Contains("products/test-product4"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".carousel-main-item.js-carousel-item a"))[2].GetAttribute("href").Contains("products/test-product5"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".carousel-main-item.js-carousel-item a"))[3].GetAttribute("href").Contains("products/test-product6"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".carousel-main-item.js-carousel-item a"))[4].GetAttribute("href").Contains("products/test-product4"));
            Assert.AreEqual("teg6", driver.FindElement(By.CssSelector(".carousel-main-item.js-carousel-item img")).GetAttribute("alt"));

            driver.FindElement(By.CssSelector(".carousel-nav-next.icon-right-open-after.carousel-main-next.icon-right-circle-after")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector(".carousel-nav-prev.icon-left-open-after.carousel-main-prev.icon-left-circle-after")).Click();
            Thread.Sleep(1000);
            /*   Refresh();
               Thread.Sleep(1000);
               driver.FindElements(By.CssSelector(".carousel-main-item.js-carousel-item a"))[1].Click();
               Thread.Sleep(1000);
               Assert.IsTrue(driver.Url.Contains("products/test-product4"));
               Assert.AreEqual("TestProduct4", driver.FindElement(By.TagName("h1")).Text);
               */
            //в одну колонку
            Functions.TemplateSettingsSelect(driver, baseURL, select: "Одна колонка", settingsName: "Режим отображения главной страницы");
            GoToClient();
            Assert.AreEqual("3", (driver.FindElements(By.CssSelector(".carousel-main-item.js-carousel-item")).Count - 2).ToString()); //2 clone
            Assert.IsTrue(driver.FindElement(By.CssSelector(".carousel-main-item.js-carousel-item a")).GetAttribute("href").Contains("products/test-product3"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".carousel-main-item.js-carousel-item a"))[1].GetAttribute("href").Contains("products/test-product1"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".carousel-main-item.js-carousel-item a"))[2].GetAttribute("href").Contains("products/test-product2"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".carousel-main-item.js-carousel-item a"))[3].GetAttribute("href").Contains("products/test-product3"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".carousel-main-item.js-carousel-item a"))[4].GetAttribute("href").Contains("products/test-product1"));
            Assert.AreEqual("teg3", driver.FindElement(By.CssSelector(".carousel-main-item.js-carousel-item img")).GetAttribute("alt"));

            driver.FindElement(By.CssSelector(".carousel-nav-next.icon-right-open-after.carousel-main-next.icon-right-circle-after")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector(".carousel-nav-prev.icon-left-open-after.carousel-main-prev.icon-left-circle-after")).Click();
            Thread.Sleep(1000);
            /* driver.FindElements(By.CssSelector(".carousel-main-item.js-carousel-item a"))[1].Click();
             Thread.Sleep(1000);
             Assert.IsTrue(driver.Url.Contains("products/test-product1"));
             Assert.AreEqual("TestProduct1", driver.FindElement(By.TagName("h1")).Text);*/
            Functions.TemplateSettingsSelect(driver, baseURL, select: "Две колонки", settingsName: "Режим отображения главной страницы");
            //мобилка
            Functions.AdminMobileOn(driver, baseURL);
            GoToClient("/?forcedMobile=true ");
            Thread.Sleep(2000);
            Assert.AreEqual("3", (driver.FindElements(By.CssSelector(".carousel-main-item.js-carousel-item")).Count - 2).ToString()); //2 clone

            //  Assert.IsTrue(driver.FindElement(By.CssSelector(".carousel-main-item.js-carousel-item a")).GetAttribute("href").Contains("products/test-product6"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".carousel-main-item.js-carousel-item a"))[1].GetAttribute("href").Contains("products/test-product1"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".carousel-main-item.js-carousel-item a"))[2].GetAttribute("href").Contains("products/test-product2"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".carousel-main-item.js-carousel-item a"))[3].GetAttribute("href").Contains("products/test-product3"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".carousel-main-item.js-carousel-item a"))[4].GetAttribute("href").Contains("products/test-product1"));
            Assert.AreEqual("teg3", driver.FindElement(By.CssSelector(".carousel-main-item.js-carousel-item img")).GetAttribute("alt"));

            driver.FindElement(By.CssSelector(".carousel-nav-next.icon-right-open-after.carousel-main-next")).Click();
            driver.FindElement(By.CssSelector(".carousel-nav-next.icon-right-open-after.carousel-main-next")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector(".carousel-nav-prev.icon-left-open-after.carousel-main-prev")).Click();

            Functions.AdminMobileOff(driver, baseURL);


        }
        [Test]
        public void SortCarousel()
        {
            GoToAdmin("carousel");

            GetGridCell(-1, "CaruselUrl").Click();
            Assert.AreEqual("products/test-product1", GetGridCell(0, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("products/test-product16", GetGridCell(9, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));

            GetGridCell(-1, "CaruselUrl").Click();
            Assert.AreEqual("products/test-product99", GetGridCell(0, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("products/test-product90", GetGridCell(9, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));

            GetGridCell(-1, "Description").Click();
            Assert.AreEqual("products/test-product7", GetGridCell(0, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("products/test-product16", GetGridCell(9, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));

            GetGridCell(-1, "Description").Click();
            Assert.AreEqual("products/test-product6", GetGridCell(0, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("products/test-product10", GetGridCell(9, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));

            GetGridCell(-1, "DisplayInOneColumn").Click();
            Assert.AreEqual("products/test-product4", GetGridCell(0, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("products/test-product13", GetGridCell(9, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));

            GetGridCell(-1, "DisplayInOneColumn").Click();
            Assert.AreEqual("products/test-product7", GetGridCell(0, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("products/test-product16", GetGridCell(9, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));

            GetGridCell(-1, "DisplayInTwoColumns").Click();
            Assert.AreEqual("products/test-product1", GetGridCell(0, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("products/test-product10", GetGridCell(9, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));

            GetGridCell(-1, "DisplayInTwoColumns").Click();
            Assert.AreEqual("products/test-product4", GetGridCell(0, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("products/test-product13", GetGridCell(9, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));

            GetGridCell(-1, "DisplayInMobile").Click();
            Assert.AreEqual("products/test-product4", GetGridCell(0, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("products/test-product13", GetGridCell(9, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));

            GetGridCell(-1, "DisplayInMobile").Click();
            Assert.AreEqual("products/test-product7", GetGridCell(0, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("products/test-product16", GetGridCell(9, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));

            GetGridCell(-1, "SortOrder").Click();
            Assert.AreEqual("products/test-product1", GetGridCell(0, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("products/test-product10", GetGridCell(9, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));

            GetGridCell(-1, "SortOrder").Click();
            Assert.AreEqual("products/test-product101", GetGridCell(0, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("products/test-product92", GetGridCell(9, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));

            GetGridCell(-1, "Enabled").Click();
            Assert.AreEqual("products/test-product7", GetGridCell(0, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("products/test-product16", GetGridCell(9, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));

            GetGridCell(-1, "Enabled").Click();
            Assert.AreEqual("products/test-product1", GetGridCell(0, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("products/test-product10", GetGridCell(9, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));

            GetGridCell(-1, "Blank").Click();
            Assert.AreEqual("products/test-product5", GetGridCell(0, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("products/test-product14", GetGridCell(9, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));

            GetGridCell(-1, "Blank").Click();
            Assert.AreEqual("products/test-product1", GetGridCell(0, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("products/test-product10", GetGridCell(9, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));


        }
        [Test]
        public void SearchCarousel()
        {
            GoToAdmin("carousel");
            GetGridFilter().SendKeys("test-product100");
            DropFocus("h1");
            Assert.AreEqual("products/test-product100", GetGridCell(0, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));

            //search not exist product
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("test-product111");
            DropFocus("h1");
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));

            //search too much symbols
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("1111111111222222222222222222333333333333334444444444rrrrrrrrrrrreeeeeeeeewwwwwwwwwwwwwwwwwww");
            DropFocus("h1");
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));

            //search invalid symbols
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("########@@@@@@@@&&&&&&&******,,,,..");
            DropFocus("h1");
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));
        }

        [Test]
        public void FilterCarousel()
        {
            GoToAdmin("carousel");
            //Код
            Functions.GridFilterSet(driver, baseURL, "CaruselUrl");
            DropFocus("h1");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("test-product22");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            DropFocus("h1");
            Refresh();
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 1);
            Assert.AreEqual("products/test-product22", GetGridCell(0, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));
            Functions.GridFilterClose(driver, baseURL, "CaruselUrl");
            Assert.AreEqual("products/test-product1", GetGridCell(0, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("products/test-product10", GetGridCell(9, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));
            //тег
            Functions.GridFilterSet(driver, baseURL, "Description");
            DropFocus("h1");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("teg2");
            DropFocus("h1");
            Thread.Sleep(2000);
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 1);
            Assert.AreEqual("products/test-product2", GetGridCell(0, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));
            Functions.GridFilterClose(driver, baseURL, "Description");
            Assert.AreEqual("products/test-product1", GetGridCell(0, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("products/test-product10", GetGridCell(9, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));

            //одна колонка
            Functions.GridFilterSet(driver, baseURL, "DisplayInOneColumn");
            DropFocus("h1");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Да");
            DropFocus("h1");
            Thread.Sleep(2000);
            Assert.AreEqual("products/test-product1", GetGridCell(0, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("products/test-product13", GetGridCell(9, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Нет");
            DropFocus("h1");
            Thread.Sleep(2000);
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 3);
            Assert.AreEqual("products/test-product4", GetGridCell(0, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("products/test-product6", GetGridCell(2, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));
            Functions.GridFilterClose(driver, baseURL, "DisplayInOneColumn");
            Assert.AreEqual("products/test-product1", GetGridCell(0, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("products/test-product10", GetGridCell(9, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));
            //Две колонки
            Functions.GridFilterSet(driver, baseURL, "DisplayInTwoColumns");
            DropFocus("h1");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Да");
            DropFocus("h1");
            Thread.Sleep(2000);
            Assert.AreEqual("products/test-product4", GetGridCell(0, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("products/test-product13", GetGridCell(9, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Нет");
            DropFocus("h1");
            Thread.Sleep(2000);
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 3);
            Assert.AreEqual("products/test-product1", GetGridCell(0, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("products/test-product3", GetGridCell(2, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));
            Functions.GridFilterClose(driver, baseURL, "DisplayInTwoColumns");
            Assert.AreEqual("products/test-product1", GetGridCell(0, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("products/test-product10", GetGridCell(9, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));
            //мобилка
            Functions.GridFilterSet(driver, baseURL, "DisplayInMobile");
            DropFocus("h1");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Да");
            DropFocus("h1");
            Thread.Sleep(2000);
            Assert.AreEqual("products/test-product1", GetGridCell(0, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("products/test-product13", GetGridCell(9, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Нет");
            DropFocus("h1");
            Thread.Sleep(2000);
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 3);
            Assert.AreEqual("products/test-product4", GetGridCell(0, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("products/test-product6", GetGridCell(2, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));
            Functions.GridFilterClose(driver, baseURL, "DisplayInMobile");
            Assert.AreEqual("products/test-product1", GetGridCell(0, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("products/test-product10", GetGridCell(9, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));
            //Активность
            Functions.GridFilterSet(driver, baseURL, "Enabled");
            DropFocus("h1");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Да");
            DropFocus("h1");
            Thread.Sleep(2000);
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 6);
            Assert.AreEqual("products/test-product1", GetGridCell(0, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("products/test-product6", GetGridCell(5, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Нет");
            DropFocus("h1");
            Thread.Sleep(2000);
            Assert.AreEqual("products/test-product7", GetGridCell(0, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("products/test-product16", GetGridCell(9, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));
            Functions.GridFilterClose(driver, baseURL, "Enabled");
            Assert.AreEqual("products/test-product1", GetGridCell(0, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("products/test-product10", GetGridCell(9, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));

        }
        [Test]
        public void xInplaceCarousel()
        {
            GoToAdmin("carousel");

            Assert.IsTrue(GetGridCell(0, "DisplayInOneColumn").FindElement(By.CssSelector("input")).Selected);
            Assert.IsFalse(GetGridCell(0, "DisplayInTwoColumns").FindElement(By.CssSelector("input")).Selected);
            Assert.IsTrue(GetGridCell(0, "DisplayInMobile").FindElement(By.CssSelector("input")).Selected);
            Assert.IsTrue(GetGridCell(0, "Enabled").FindElement(By.CssSelector("input")).Selected);

            Assert.AreEqual("products/test-product1", GetGridCell(0, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));
            GetGridCell(0, "CaruselUrl").FindElement(By.TagName("input")).Click();
            GetGridCell(0, "CaruselUrl").FindElement(By.TagName("input")).Clear();
            GetGridCell(0, "CaruselUrl").FindElement(By.TagName("input")).SendKeys("edit1");
            DropFocus("h1");
            Assert.AreEqual("edit1", GetGridCell(0, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));

            Assert.AreEqual("teg1", GetGridCell(0, "Description").FindElement(By.TagName("input")).GetAttribute("value"));
            GetGridCell(0, "Description").FindElement(By.TagName("input")).Clear();
            GetGridCell(0, "Description").FindElement(By.TagName("input")).SendKeys("newDescription");
            DropFocus("h1");
            Assert.AreEqual("newDescription", GetGridCell(0, "Description").FindElement(By.TagName("input")).GetAttribute("value"));

            Assert.AreEqual("1", GetGridCell(0, "SortOrder").FindElement(By.TagName("input")).GetAttribute("value"));
            GetGridCell(0, "SortOrder").FindElement(By.TagName("input")).Clear();
            GetGridCell(0, "SortOrder").FindElement(By.TagName("input")).SendKeys("0");
            DropFocus("h1");
            Assert.AreEqual("0", GetGridCell(0, "SortOrder").FindElement(By.TagName("input")).GetAttribute("value"));

            Assert.IsTrue(GetGridCell(0, "DisplayInOneColumn").FindElement(By.CssSelector("input")).Selected);
            GetGridCell(0, "DisplayInOneColumn").FindElement(By.CssSelector("[data-e2e=\"switchOnOffLabel\"]")).Click();
            Thread.Sleep(1000);
            Assert.IsFalse(GetGridCell(0, "DisplayInOneColumn").FindElement(By.CssSelector("input")).Selected);

            Assert.IsFalse(GetGridCell(0, "DisplayInTwoColumns").FindElement(By.CssSelector("input")).Selected);
            GetGridCell(0, "DisplayInTwoColumns").FindElement(By.CssSelector("[data-e2e=\"switchOnOffLabel\"]")).Click();
            Thread.Sleep(1000);
            Assert.IsTrue(GetGridCell(0, "DisplayInTwoColumns").FindElement(By.CssSelector("input")).Selected);

            Assert.IsTrue(GetGridCell(0, "DisplayInMobile").FindElement(By.CssSelector("input")).Selected);
            GetGridCell(0, "DisplayInMobile").FindElement(By.CssSelector("[data-e2e=\"switchOnOffLabel\"]")).Click();
            Thread.Sleep(1000);
            Assert.IsFalse(GetGridCell(0, "DisplayInMobile").FindElement(By.CssSelector("input")).Selected);

            Assert.IsTrue(GetGridCell(1, "Enabled").FindElement(By.CssSelector("input")).Selected);
            GetGridCell(1, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffLabel\"]")).Click();
            Thread.Sleep(1000);
            Assert.IsFalse(GetGridCell(1, "Enabled").FindElement(By.CssSelector("input")).Selected);

            Refresh();
            Assert.AreEqual("edit1", GetGridCell(0, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("newDescription", GetGridCell(0, "Description").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("0", GetGridCell(0, "SortOrder").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.IsFalse(GetGridCell(0, "DisplayInOneColumn").FindElement(By.CssSelector("input")).Selected);
            Assert.IsTrue(GetGridCell(0, "DisplayInTwoColumns").FindElement(By.CssSelector("input")).Selected);
            Assert.IsFalse(GetGridCell(0, "DisplayInMobile").FindElement(By.CssSelector("input")).Selected);
            Assert.IsFalse(GetGridCell(1, "Enabled").FindElement(By.CssSelector("input")).Selected);

            GoToClient();
            //Assert.AreEqual("4", (driver.FindElements(By.CssSelector(".carousel-main-item.js-carousel-item")).Count - 2).ToString()); //2 clone

            Assert.IsTrue(driver.FindElement(By.CssSelector(".carousel-main-item.js-carousel-item a")).GetAttribute("href").Contains("products/test-product6"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".carousel-main-item.js-carousel-item a"))[1].GetAttribute("href").Contains("edit1"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".carousel-main-item.js-carousel-item a"))[2].GetAttribute("href").Contains("products/test-product4"));

            Assert.IsTrue(Is404Page(driver.FindElements(By.CssSelector(".carousel-main-item.js-carousel-item a"))[1].GetAttribute("href")));
            driver.FindElement(By.CssSelector(".carousel-nav-next.icon-right-open-after.carousel-main-next.icon-right-circle-after")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector(".carousel-nav-prev.icon-left-open-after.carousel-main-prev.icon-left-circle-after")).Click();
            Thread.Sleep(1000);
        }
        
        [Test]
       public void CarouselBlankDefault()
       {
            GoToAdmin("carousel");

            Assert.IsTrue(GetGridCell(3, "Blank").FindElement(By.CssSelector("input")).Selected);
            Assert.IsFalse(GetGridCell(4, "Blank").FindElement(By.CssSelector("input")).Selected);

            Assert.AreEqual("products/test-product4", GetGridCell(3, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("products/test-product5", GetGridCell(4, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));

            GoToClient();
            Assert.IsTrue(driver.FindElements(By.CssSelector(".carousel-main-item.js-carousel-item a"))[1].GetAttribute("href").Contains("products/test-product4"));
            driver.FindElement(By.XPath("//img[@id=\"inplaceImage_1\"]")).Click();
            Thread.Sleep(2000);

            Functions.OpenNewTab(driver, baseURL);
            ReadOnlyCollection<String> windowHandlesBlank = driver.WindowHandles;

            Assert.IsTrue(windowHandlesBlank.Count == 2);

            Assert.AreEqual("TestProduct4", driver.FindElement(By.TagName("h1")).Text);

            Functions.CloseTab(driver, baseURL);

            GoToClient();

            Assert.IsTrue(driver.FindElements(By.CssSelector(".carousel-main-item.js-carousel-item a"))[2].GetAttribute("href").Contains("products/test-product5"));
            driver.FindElement(By.CssSelector(".carousel-nav-next.icon-right-open-after.carousel-main-next.icon-right-circle-after")).Click();
            Thread.Sleep(500);
            driver.FindElement(By.XPath("//img[@id=\"inplaceImage_2\"]")).Click();
            Thread.Sleep(2000);

            Assert.AreEqual("TestProduct5", driver.FindElement(By.TagName("h1")).Text);

            ReadOnlyCollection<String> windowHandlesNotBlank = driver.WindowHandles;

            Assert.IsTrue(windowHandlesNotBlank.Count == 1);
        }

        [Test]
        public void CarouselBlankInplace()
        {
            GoToAdmin("carousel");

            Assert.IsTrue(GetGridCell(3, "Blank").FindElement(By.CssSelector("input")).Selected);
            Assert.IsFalse(GetGridCell(4, "Blank").FindElement(By.CssSelector("input")).Selected);

            GetGridCell(3, "Blank").FindElement(By.CssSelector("[data-e2e=\"switchOnOffLabel\"]")).Click();
            Thread.Sleep(1000);
            GetGridCell(4, "Blank").FindElement(By.CssSelector("[data-e2e=\"switchOnOffLabel\"]")).Click();
            Thread.Sleep(1000);

            //check inplace
            GoToAdmin("carousel");

            Assert.IsFalse(GetGridCell(3, "Blank").FindElement(By.CssSelector("input")).Selected);
            Assert.IsTrue(GetGridCell(4, "Blank").FindElement(By.CssSelector("input")).Selected);

            GoToClient();
            Assert.IsTrue(driver.FindElements(By.CssSelector(".carousel-main-item.js-carousel-item a"))[1].GetAttribute("href").Contains("products/test-product4"));
            driver.FindElement(By.XPath("//img[@id=\"inplaceImage_1\"]")).Click();
            Thread.Sleep(2000);

            Assert.AreEqual("TestProduct4", driver.FindElement(By.TagName("h1")).Text);

            ReadOnlyCollection<String> windowHandlesNotBlank = driver.WindowHandles;
            Assert.IsTrue(windowHandlesNotBlank.Count == 1);

            GoToClient();

            Assert.IsTrue(driver.FindElements(By.CssSelector(".carousel-main-item.js-carousel-item a"))[2].GetAttribute("href").Contains("products/test-product5"));
            driver.FindElement(By.CssSelector(".carousel-nav-next.icon-right-open-after.carousel-main-next.icon-right-circle-after")).Click();
            Thread.Sleep(500);
            driver.FindElement(By.XPath("//img[@id=\"inplaceImage_2\"]")).Click();
            Thread.Sleep(2000);
            
            Functions.OpenNewTab(driver, baseURL);
            ReadOnlyCollection<String> windowHandlesBlank = driver.WindowHandles;

            Assert.IsTrue(windowHandlesBlank.Count == 2);

            Assert.AreEqual("TestProduct5", driver.FindElement(By.TagName("h1")).Text);

            Functions.CloseTab(driver, baseURL);
        }

    }
}
