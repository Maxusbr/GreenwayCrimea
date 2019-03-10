using System;
using NUnit.Framework;
using System.Threading;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;
using AdvantShop.Selenium.Core.Infrastructure;

namespace AdvantShop.SeleniumTest.Admin.MainPage.Category.EditCategory
{
    [TestFixture]
    public class CategoryEditPicture : BaseSeleniumTest
    {
         
         

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(
                "Data\\Admin\\EditCategory\\EditCategoryColor\\Catalog.Category.csv"
                );

             
            Init();

        }
        

        [Test]
        public void ChangeImgCategory()
        {
           GoToAdmin("catalog");
            driver.FindElement(By.CssSelector("[data-e2e=\"categoriesBlockItemEdit\"]")).Click();
            Thread.Sleep(4000);

            var element = driver.FindElements(By.TagName("iframe"))[1];
            IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
            jse.ExecuteScript("arguments[0].scrollIntoView(true)", element);
            
            driver.FindElement(By.Id("UrlPath")).Clear();
            driver.FindElement(By.Id("UrlPath")).SendKeys("new");
            ScrollTo(By.TagName("figure"));
            driver.FindElement(By.XPath("(//input[@type='file'])[6]")).Clear();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("(//input[@type='file'])[6]")).SendKeys(GetPicturePath("big.png"));
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("(//input[@type='file'])[2]")).Clear();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("(//input[@type='file'])[2]")).SendKeys(GetPicturePath("small.jpg"));
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("(//input[@type='file'])[4]")).Clear();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("(//input[@type='file'])[4]")).SendKeys(GetPicturePath("icon.jpg"));
            Thread.Sleep(2000);

            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);
            //Проверка
            GoToAdmin("catalog");
            string str = driver.FindElement(By.CssSelector("[data-e2e-block-category-drag-id=\"1\"] img")).GetAttribute("src");
            Assert.IsFalse(str.Contains("nophoto"));
            //Проверка в клиентке
            GoToClient("catalog");
            str = driver.FindElement(By.CssSelector(".product-categories-item-photo-link-slim img")).GetAttribute("src");
            Assert.IsFalse(str.Contains("nophoto"));
            str = driver.FindElement(By.CssSelector(".menu-dropdown-icon img")).GetAttribute("src");
            Assert.IsFalse(str.Contains("nophoto"));
            driver.FindElement(By.CssSelector(".product-categories-header-slim-title")).Click();
            Thread.Sleep(2000);
            str = driver.FindElement(By.CssSelector(".category-picture img")).GetAttribute("src");
            Assert.IsFalse(str.Contains("nophoto"));
                        
        }
        [Test]
        public void HrefChangeImgCategory()
        {
            GoToAdmin("catalog");
            driver.FindElement(By.CssSelector("[data-e2e=\"categoriesBlockItemEdit\"]")).Click();
            Thread.Sleep(2000);

            var element = driver.FindElements(By.TagName("iframe"))[1];
            IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
            jse.ExecuteScript("arguments[0].scrollIntoView(true)", element);
            
            driver.FindElement(By.Id("UrlPath")).Clear();
            driver.FindElement(By.Id("UrlPath")).SendKeys("new");
            ScrollTo(By.TagName("figure"));
            driver.FindElement(By.CssSelector("[data-e2e=\"BigImg\"] [data-e2e=\"imgByHref\"]")).Click();
            Thread.Sleep(500);
            Assert.AreEqual("Загрузка изображения по ссылке", driver.FindElement(By.TagName("h2")).Text);
            driver.FindElement(By.CssSelector("input")).Click();
            driver.FindElement(By.CssSelector("input")).Clear();
            driver.FindElement(By.CssSelector("input")).SendKeys("http://3.bp.blogspot.com/-6IH8wFoH2UU/UbFrnBX8_DI/AAAAAAAAZ5k/fd-2AmuvuoM/s1600/iPhone+5S.png");
           
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            Thread.Sleep(2000);
            ScrollTo(By.TagName("figure"));
            driver.FindElement(By.CssSelector("[data-e2e=\"IconImg\"] [data-e2e=\"imgByHref\"]")).Click();
            driver.FindElement(By.CssSelector("input")).Clear();
            driver.FindElement(By.CssSelector("input")).SendKeys("http://xroniki-nauki.ru/wp-content/uploads/2011/01/phone.jpg");
      
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            Thread.Sleep(2000);
            ScrollTo(By.TagName("figure"));
            driver.FindElement(By.CssSelector("[data-e2e=\"SmallImg\"] [data-e2e=\"imgByHref\"]")).Click();

            driver.FindElement(By.CssSelector("input")).Click();
            driver.FindElement(By.CssSelector("input")).Clear();
            driver.FindElement(By.CssSelector("input")).SendKeys("https://texnomaniya.ru/pictures/pages/img_20926_page_26667_r.jpg");
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();

            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
           
            //Проверка
            GoToAdmin("catalog");
            string str = driver.FindElement(By.CssSelector("[data-e2e-block-category-drag-id=\"1\"] img")).GetAttribute("src");
            Assert.IsFalse(str.Contains("nophoto"));
            //Проверка в клиентке
            GoToClient("catalog");
            str = driver.FindElement(By.CssSelector(".product-categories-item-photo-link-slim img")).GetAttribute("src");
            Assert.IsFalse(str.Contains("nophoto"));
            str = driver.FindElement(By.CssSelector(".menu-dropdown-icon img")).GetAttribute("src");
            Assert.IsFalse(str.Contains("nophoto"));
            driver.FindElement(By.CssSelector(".product-categories-header-slim-title")).Click();
            Thread.Sleep(2000);

            str = driver.FindElement(By.CssSelector(".category-picture img")).GetAttribute("src");
            Assert.IsFalse(str.Contains("nophoto"));
        }
        [Test]
        public void DelImgByHrefCategory()
        {
           GoToAdmin("catalog");
            driver.FindElement(By.CssSelector("[data-e2e=\"categoriesBlockItemEdit\"]")).Click();
            Thread.Sleep(2000);
            ScrollTo(By.TagName("figure"));
            driver.FindElement(By.CssSelector("[data-e2e=\"BigImg\"] [data-e2e=\"imgDel\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"IconImg\"] [data-e2e=\"imgDel\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"SmallImg\"] [data-e2e=\"imgDel\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(7000);

            driver.FindElement(By.Id("UrlPath")).Clear();
            driver.FindElement(By.Id("UrlPath")).SendKeys("new");
            ScrollTo(By.Id("header-top"));
            Thread.Sleep(2000);
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            //Проверка
            GoToAdmin("catalog");
             string str = driver.FindElement(By.CssSelector("[data-e2e-block-category-drag-id=\"1\"] img")).GetAttribute("src");
            Assert.IsTrue(str.Contains("nophoto"));

            GoToClient("catalog");
            Assert.IsTrue(driver.FindElement(By.CssSelector(".product-categories-item-inner-slim img")).GetAttribute("src").Contains("nophoto")); 
            Assert.IsTrue(driver.FindElements(By.CssSelector(".menu-dropdown-icon-img")).Count == 0);

            GoToClient("categories/new");
            Assert.IsTrue(driver.FindElements(By.CssSelector(".category-picture img")).Count == 0);
        }

    }
}
