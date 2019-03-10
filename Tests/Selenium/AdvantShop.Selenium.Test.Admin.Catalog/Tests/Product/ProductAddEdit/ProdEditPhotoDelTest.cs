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
    public class ProductAddEditPhotoDelTest : BaseSeleniumTest
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
        public void ProductEditPhotoDelete()
        {
            //pre check
            GoToClient("products/test-product1");
            Assert.IsFalse(driver.FindElement(By.CssSelector(".gallery-picture.text-static img")).GetAttribute("src").Contains("nophoto"));

            GoToAdmin("product/edit/1");
            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Фотографии')]")).Click();
            Thread.Sleep(2000);
            Assert.IsFalse(driver.FindElement(By.Id("leftAsideProduct")).FindElement(By.TagName("img")).GetAttribute("src").Contains("nophoto"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".italic.hover-padding-left")).Count > 0);

            GoToAdmin("product/edit/1");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Фотографии')]")).Click();
            WaitForElem(By.CssSelector("[data-e2e=\"PhotoImg\"]"));

            Actions a = new Actions(driver);
            a.Build();
            a.MoveToElement(driver.FindElement(By.CssSelector("[data-e2e=\"PhotoImg\"]")));
            a.Perform();
            driver.FindElement(By.CssSelector(".product-block-state.clearfix")).FindElement(By.CssSelector("[data-e2e=\"PhotoItemDelete\"]")).Click();
            WaitForElem(By.ClassName("swal2-confirm"));
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("product/edit/1");
            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Фотографии')]")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(driver.FindElement(By.Id("leftAsideProduct")).FindElement(By.TagName("img")).GetAttribute("src").Contains("nophoto"));
            Assert.IsFalse(driver.FindElements(By.CssSelector(".italic.hover-padding-left")).Count > 0);

            //check client
            GoToClient("products/test-product1");
            Assert.IsTrue(driver.FindElement(By.CssSelector(".gallery-picture.text-static img")).GetAttribute("src").Contains("nophoto"));
        }

        
    }
}