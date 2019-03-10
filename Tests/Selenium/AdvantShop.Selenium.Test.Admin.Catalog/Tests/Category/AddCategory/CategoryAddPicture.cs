using System;
using NUnit.Framework;
using System.Threading;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using AdvantShop.Selenium.Core.Infrastructure;

namespace AdvantShop.SeleniumTest.Admin.MainPage.Category.AddCategory
{
    [TestFixture]
    public class CategoryAddPicture : BaseSeleniumTest
    {
         
         
        bool del = true;
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(
                "Data\\Admin\\Catalog\\AddCategory\\Catalog.Product.csv",
                "Data\\Admin\\Catalog\\AddCategory\\Catalog.Offer.csv"
          );

             
            Init();

        }

        //Pictures

        [Test]
        public void AddImgByHrefCategory()
        {
            GoToAdmin("catalog");

            driver.FindElement(By.CssSelector("[data-e2e=\"AddCategory\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.Id("Name")).SendKeys("New_Category");
            DropFocus("h1");
            driver.FindElement(By.Id("UrlPath")).Clear();
            driver.FindElement(By.Id("UrlPath")).SendKeys("new1");
            ScrollTo(By.TagName("figure"));
            driver.FindElement(By.CssSelector("[data-e2e=\"BigImg\"] [data-e2e=\"imgByHref\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("input")).Click();
            driver.FindElement(By.CssSelector("input")).Clear();
            driver.FindElement(By.CssSelector("input")).SendKeys("http://3.bp.blogspot.com/-6IH8wFoH2UU/UbFrnBX8_DI/AAAAAAAAZ5k/fd-2AmuvuoM/s1600/iPhone+5S.png");
            driver.FindElement(By.CssSelector("[data-e2e=\"imgByHrefBtnSave\"]")).Click();
            ScrollTo(By.TagName("figure"));
            driver.FindElement(By.CssSelector("[data-e2e=\"IconImg\"] [data-e2e=\"imgByHref\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("input")).Clear();
            driver.FindElement(By.CssSelector("input")).SendKeys("http://s00.yaplakal.com/pics/pics_original/4/6/8/8310864.jpg");
            driver.FindElement(By.CssSelector("[data-e2e=\"imgByHrefBtnSave\"]")).Click();
            ScrollTo(By.TagName("figure"));
            driver.FindElement(By.CssSelector("[data-e2e=\"SmallImg\"] [data-e2e=\"imgByHref\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("input")).Click();
            driver.FindElement(By.CssSelector("input")).Clear();
            driver.FindElement(By.CssSelector("input")).SendKeys("https://telegraf.com.ua/files/2016/11/cat.jpg");
            driver.FindElement(By.CssSelector("[data-e2e=\"imgByHrefBtnSave\"]")).Click();
            ScrollTo(By.Id("header-top"));
            Thread.Sleep(5000);
            GetButton(eButtonType.Save).Click();
            del = false;
            //Проверка
            GoToAdmin("catalog");
            string str = driver.FindElement(By.CssSelector("[data-e2e-block=\"CategoryDrag\"] img")).GetAttribute("src");
            Assert.IsFalse(str.Contains("nophoto"));
            //Проверка в клиентке
            GoToClient("categories/new1");
            str = driver.FindElement(By.CssSelector(".category-picture")).FindElement(By.TagName("img")).GetAttribute("src");
            Assert.IsFalse(str.Contains("nophoto"));
            GoToClient("catalog");
            str = driver.FindElement(By.CssSelector(".menu-dropdown-icon-img")).GetAttribute("src");
            Assert.IsFalse(str.Contains("nophoto"));
            str = driver.FindElement(By.CssSelector(".product-categories-item-photo-link-slim")).FindElement(By.TagName("img")).GetAttribute("src");
            Assert.IsFalse(str.Contains("nophoto"));

        }

        [Test]
        public void AddImgCategory()
        {
           GoToAdmin("catalog");
            Thread.Sleep(2000);
            if (del==false) 
                    del_category();
            driver.FindElement(By.CssSelector("[data-e2e=\"AddCategory\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.Id("Name")).SendKeys("New_Category");
            DropFocus("h1");
            driver.FindElement(By.Id("UrlPath")).Clear();
            driver.FindElement(By.Id("UrlPath")).SendKeys("new");
            ScrollTo(By.TagName("figure"));
            driver.FindElement(By.XPath("(//input[@type='file'])[6]")).Clear();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("(//input[@type='file'])[6]")).SendKeys(GetPicturePath("big.png"));
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("(//input[@type='file'])[4]")).Clear();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("(//input[@type='file'])[4]")).SendKeys(GetPicturePath("icon.jpg"));
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("(//input[@type='file'])[2]")).Clear();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("(//input[@type='file'])[2]")).SendKeys(GetPicturePath("small.jpg"));
            ScrollTo(By.Id("header-top"));
            Thread.Sleep(6000);
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);
            del = false;
            //Проверка
            GoToClient("categories/new");
            string str = driver.FindElement(By.CssSelector(".category-picture img")).GetAttribute("src");
            Assert.IsFalse(str.Contains("nophoto"));            
            GoToClient("catalog");
            str = driver.FindElement(By.CssSelector(".menu-dropdown-icon img")).GetAttribute("src");
            Assert.IsFalse(str.Contains("nophoto"));
            str = driver.FindElement(By.CssSelector(".product-categories-item-photo-link-slim img")).GetAttribute("src");
            Assert.IsFalse(str.Contains("nophoto"));
            GoToAdmin("catalog");
            str = driver.FindElement(By.CssSelector("[data-e2e-block=\"CategoryDrag\"] img")).GetAttribute("src");
            Assert.IsFalse(str.Contains("nophoto"));
        }
       
        [Test]
        public void DelImgCategory()
        {
            GoToAdmin("catalog");
            if (del == false)
                del_category();
            driver.FindElement(By.CssSelector("[data-e2e=\"AddCategory\"]")).Click();
            Thread.Sleep(2000);
            ScrollTo(By.TagName("figure"));

            driver.FindElement(By.XPath("(//input[@type='file'])[6]")).Clear();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("(//input[@type='file'])[6]")).SendKeys(GetPicturePath("big.png"));
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("(//input[@type='file'])[4]")).Clear();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("(//input[@type='file'])[4]")).SendKeys(GetPicturePath("icon.jpg"));
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("(//input[@type='file'])[2]")).Clear();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("(//input[@type='file'])[2]")).SendKeys(GetPicturePath("small.jpg"));

            driver.FindElement(By.Id("Name")).SendKeys("New_Category");
            driver.FindElement(By.Id("UrlPath")).Clear();
            driver.FindElement(By.Id("UrlPath")).SendKeys("new2");
            ScrollTo(By.Id("header-top"));
            Thread.Sleep(7000);
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"EditCategory\"]")).Click();
            Thread.Sleep(3000);
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
            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
             Thread.Sleep(2000);
            del = false;
            //Проверка
            GoToAdmin("catalog");
            string str = driver.FindElement(By.CssSelector("[data-e2e-block=\"CategoryDrag\"] img")).GetAttribute("src");
            Assert.IsTrue(str.Contains("nophoto"));

            GoToClient("categories/new2");
            Assert.IsTrue(driver.FindElements(By.CssSelector(".category-picture img")).Count == 0);
            GoToClient("catalog");
            Assert.IsTrue(driver.FindElements(By.CssSelector(".menu-dropdown-icon-img")).Count == 0);
            Assert.IsTrue(driver.FindElement(By.CssSelector(".product-categories-item-inner-slim img")).GetAttribute("src").Contains("nophoto"));
        }
        public void del_category()
        {
            driver.FindElement(By.CssSelector("[data-e2e=\"categoriesBlockItemDelete\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);           
        }
    }
}
