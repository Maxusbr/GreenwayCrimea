using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;
using OpenQA.Selenium.Interactions;

namespace AdvantShop.SeleniumTest.Admin.Catalog.ProductAddEdit.Photo
{
    [TestFixture]
    public class ProductAddEditPhoto360DelTest : BaseSeleniumTest
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
           "data\\Admin\\Catalog\\ProductAddEdit\\Catalog.Photo.csv",
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
        public void ProductEditPhoto360Delete()
        {
            GoToClient("products/test-product8");
            Assert.IsFalse(driver.FindElements(By.CssSelector("[data-modal-open=\"modalProductRotate\"]")).Count > 0);

            GoToAdmin("product/edit/8");
            driver.FindElement(By.XPath("//div[contains(text(), 'Фотографии')]")).Click();
            Thread.Sleep(2000);
            Assert.IsFalse(driver.FindElements(By.CssSelector("[data-e2e=\"Photo360Img\"]")).Count > 0);
            Assert.IsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"Photo360Input\"]")).Selected);

            ScrollTo(By.CssSelector("[data-e2e=\"imgByHref\"]"));
            driver.FindElement(By.CssSelector("[data-e2e=\"Photo360CheckBox\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("(//input[@type='file'])[3]")).SendKeys(GetPicturePath("pics3d\\2.jpg")); //selenium can't upload multiple files
            Thread.Sleep(2000);

            //check after uploading file
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"Photo360Input\"]")).Selected);
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"Photo360Img\"]")).Count == 1);
            Assert.IsTrue(driver.FindElement(By.CssSelector(".aside-image img")).GetAttribute("src").Contains("nophoto"));
            Assert.IsFalse(driver.PageSource.Contains("Главное фото"));

            GoToAdmin("product/edit/8");

            driver.FindElement(By.XPath("//div[contains(text(), 'Фотографии')]")).Click();
            WaitForElem(By.CssSelector("[data-e2e=\"imgByHref\"]"));
            ScrollTo(By.CssSelector("[data-e2e=\"imgByHref\"]"));
            Thread.Sleep(4000);

            //check after refreshing page
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"Photo360Input\"]")).Selected);
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"Photo360Img\"]")).Count == 1);
            Assert.IsTrue(driver.FindElement(By.CssSelector(".aside-image img")).GetAttribute("src").Contains("nophoto"));
            Assert.IsFalse(driver.PageSource.Contains("Главное фото"));

            Actions a = new Actions(driver);
            a.Build();
            a.MoveToElement(driver.FindElement(By.CssSelector("[data-e2e=\"Photo360Img\"]")));
            a.Perform();
            driver.FindElement(By.CssSelector(".product-block-state.clearfix")).FindElement(By.CssSelector("[data-e2e=\"Photo360ImgDelete\"]")).Click();
            WaitForElem(By.ClassName("swal2-confirm"));
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("product/edit/8");
            driver.FindElement(By.XPath("//div[contains(text(), 'Фотографии')]")).Click();
            WaitForElem(By.CssSelector("[data-e2e=\"Photo360Input\"]"));
            Assert.IsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"Photo360Input\"]")).Selected);
            Assert.IsFalse(driver.FindElements(By.CssSelector("[data-e2e=\"Photo360Img\"]")).Count > 0);

            //check client
            GoToClient("products/test-product8");
            Assert.IsFalse(driver.FindElements(By.CssSelector("[data-modal-open=\"modalProductRotate\"]")).Count > 0);
        }
        
    }
}