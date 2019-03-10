using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;

namespace AdvantShop.SeleniumTest.Admin.Catalog.ProductAddEdit.Photo
{
    [TestFixture]
    public class ProductAddEditPhotoTest : BaseSeleniumTest
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
        public void ProductEditPhotoAddByHref()
        {
            GoToClient("products/test-product15");
            Assert.IsTrue(driver.FindElement(By.CssSelector(".gallery-picture.text-static img")).GetAttribute("src").Contains("nophoto"));

            GoToAdmin("product/edit/15");
            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Фотографии')]")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(driver.FindElement(By.CssSelector(".aside-image img")).GetAttribute("ng-src").Contains("nophoto"));
            Assert.IsFalse(driver.FindElements(By.CssSelector(".italic.hover-padding-left")).Count > 0);
     
            driver.FindElement(By.CssSelector("[data-e2e=\"imgByHref\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"imgByHrefLinkText\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"imgByHrefLinkText\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"imgByHrefLinkText\"]")).SendKeys("http://www.porjati.ru/uploads/posts/2016-03/thumbs/1457852671_1.jpg");
            driver.FindElement(By.CssSelector("[data-e2e=\"imgByHrefBtnSave\"]")).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("product/edit/15");
            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Фотографии')]")).Click();
            WaitForElem(By.CssSelector("[data-e2e=\"MainPhotoInput\"]"));
            Assert.IsFalse(driver.FindElement(By.CssSelector(".aside-image img")).GetAttribute("ng-src").Contains("nophoto"));
            Assert.IsTrue(driver.PageSource.Contains("Главное фото"));

            //check client
            GoToClient("products/test-product15");
            Assert.IsFalse(driver.FindElement(By.CssSelector(".gallery-picture.text-static img")).GetAttribute("src").Contains("nophoto"));
        }

        [Test]
        public void ProductEditPhotoAdd()
        {
            GoToClient("products/test-product4");
            Assert.IsTrue(driver.FindElement(By.CssSelector(".gallery-picture.text-static img")).GetAttribute("src").Contains("nophoto"));

            GoToAdmin("product/edit/4");
            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Фотографии')]")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(driver.FindElement(By.CssSelector(".aside-image img")).GetAttribute("ng-src").Contains("nophoto"));
            Assert.IsFalse(driver.FindElements(By.CssSelector(".italic.hover-padding-left")).Count > 0);

            driver.FindElement(By.XPath("(//input[@type='file'])")).Clear();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("(//input[@type='file'])")).SendKeys(GetPicturePath("big.png"));
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("product/edit/4");
            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Фотографии')]")).Click();
            WaitForElem(By.CssSelector("[data-e2e=\"MainPhotoInput\"]"));
            Assert.IsFalse(driver.FindElement(By.CssSelector(".aside-image img")).GetAttribute("ng-src").Contains("nophoto"));
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"PhotoImg\"]")).Text.Contains("Главное фото"));

            //check client
            GoToClient("products/test-product4");
            Assert.IsFalse(driver.FindElement(By.CssSelector(".gallery-picture.text-static img")).GetAttribute("src").Contains("nophoto"));
        }

        [Test]
        public void ProductEditPhotoAddByPlus()
        {
            GoToClient("products/test-product5");
            Assert.IsTrue(driver.FindElement(By.CssSelector(".gallery-picture.text-static img")).GetAttribute("src").Contains("nophoto"));

            GoToAdmin("product/edit/5");
            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Фотографии')]")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(driver.FindElement(By.CssSelector(".aside-image img")).GetAttribute("ng-src").Contains("nophoto"));
            Assert.IsFalse(driver.FindElements(By.CssSelector(".italic.hover-padding-left")).Count > 0);
       
            driver.FindElement(By.XPath("(//input[@type='file'])[2]")).SendKeys(GetPicturePath("big.png"));
            Thread.Sleep(3000);

            //check admin
            GoToAdmin("product/edit/5");
            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Фотографии')]")).Click();
            WaitForElem(By.CssSelector("[data-e2e=\"MainPhotoInput\"]"));
            Assert.IsFalse(driver.FindElement(By.CssSelector(".aside-image img")).GetAttribute("src").Contains("nophoto"));
            Assert.IsTrue(driver.PageSource.Contains("Главное фото"));

            //check client
            GoToClient("products/test-product5");
            Assert.IsFalse(driver.FindElement(By.CssSelector(".gallery-picture.text-static img")).GetAttribute("src").Contains("nophoto"));
        }


        [Test]
        public void ProductEditPhotoAdd360()
        {
            GoToClient("products/test-product6");
            Assert.IsFalse(driver.FindElements(By.CssSelector("[data-modal-open=\"modalProductRotate\"]")).Count > 0);

            GoToAdmin("product/edit/6");
            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Фотографии')]")).Click();
            Thread.Sleep(2000);
            Assert.IsFalse(driver.FindElements(By.CssSelector("[data-e2e=\"Photo360Img\"]")).Count > 0);
            Assert.IsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"Photo360Input\"]")).Selected);
    
            driver.FindElement(By.CssSelector("[data-e2e=\"Photo360CheckBox\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("(//input[@type='file'])[3]")).SendKeys(GetPicturePath("pics3d\\1.jpg")); //selenium can't upload miltiple files
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("product/edit/6");
            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Фотографии')]")).Click();
            WaitForElem(By.Id("productPhotosSortable"));
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"Photo360Input\"]")).Selected);
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"Photo360Img\"]")).Count == 1);

            //check client
            GoToClient("products/test-product6");
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-modal-open=\"modalProductRotate\"]")).Displayed);
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-modal-open=\"modalProductRotate\"]")).Count == 1);
        }

        [Test]
        public void ProductEditPhotoAddColorFilter()
        {
            GoToClient("products/test-product7");
            Assert.IsTrue(driver.FindElement(By.CssSelector(".gallery-picture.text-static img")).GetAttribute("src").Contains("nophoto"));

            GoToAdmin("product/edit/7");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Цена и наличие')]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("//a[contains(text(), 'Добавить цену')]")).Click();
            Thread.Sleep(1000);
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"AddSize\"]")))).SelectByText("SizeName1");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"AddColor\"]")))).SelectByText("Color1");
            driver.FindElement(By.CssSelector("[data-e2e=\"AddPrice\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"AddPrice\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"AddPrice\"]")).SendKeys("102102");
            driver.FindElement(By.CssSelector("[data-e2e=\"AddAmount\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"AddAmount\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"AddAmount\"]")).SendKeys("102102");
            DropFocus("h2");

            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(2000);

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Фотографии')]")).Click();
            WaitForElem(By.Id("productPhotosSortable"));

            Assert.IsTrue(driver.FindElement(By.CssSelector(".aside-image img")).GetAttribute("ng-src").Contains("nophoto"));
            Assert.IsFalse(driver.FindElements(By.CssSelector(".italic.hover-padding-left")).Count > 0);

            driver.FindElement(By.XPath("(//input[@type='file'])")).Clear();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("(//input[@type='file'])")).SendKeys(GetPicturePath("big.png"));
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("(//input[@type='file'])")).Clear();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("(//input[@type='file'])")).SendKeys(GetPicturePath("pictures-1.jpg"));
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("(//input[@type='file'])")).Clear();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("(//input[@type='file'])")).SendKeys(GetPicturePath("brand_logo.jpg"));
            Thread.Sleep(2000);

            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"PhotoImg\"]")).Count == 3);

            IWebElement selectColor1 = driver.FindElement(By.CssSelector(".product-pic-list")).FindElements(By.TagName("select"))[0];
            SelectElement select1 = new SelectElement(selectColor1);
            Assert.IsTrue(select1.AllSelectedOptions[0].Text.Contains("Нет цвета"));

            IWebElement selectColor2 = driver.FindElement(By.CssSelector(".product-pic-list")).FindElements(By.TagName("select"))[1];
            SelectElement select2 = new SelectElement(selectColor2);
            Assert.IsTrue(select2.AllSelectedOptions[0].Text.Contains("Нет цвета"));

            IWebElement selectColor3 = driver.FindElement(By.CssSelector(".product-pic-list")).FindElements(By.TagName("select"))[2];
            SelectElement select3 = new SelectElement(selectColor3);
            Assert.IsTrue(select3.AllSelectedOptions[0].Text.Contains("Нет цвета"));
            
            (new SelectElement(driver.FindElement(By.CssSelector(".product-pic-list")).FindElements(By.TagName("select"))[0])).SelectByText("Color7");
            (new SelectElement(driver.FindElement(By.CssSelector(".product-pic-list")).FindElements(By.TagName("select"))[1])).SelectByText("Color1");

            //check admin
            GoToAdmin("product/edit/7");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Фотографии')]")).Click();
            WaitForElem(By.CssSelector("[data-e2e=\"MainPhotoInput\"]"));
            IWebElement selectColor4 = driver.FindElement(By.CssSelector(".product-pic-list")).FindElements(By.TagName("select"))[0];
            SelectElement select4 = new SelectElement(selectColor4);
            Assert.IsTrue(select4.AllSelectedOptions[0].Text.Contains("Color7"));

            IWebElement selectColor5 = driver.FindElement(By.CssSelector(".product-pic-list")).FindElements(By.TagName("select"))[1];
            SelectElement select5 = new SelectElement(selectColor5);
            Assert.IsTrue(select5.AllSelectedOptions[0].Text.Contains("Color1"));

            IWebElement selectColor6 = driver.FindElement(By.CssSelector(".product-pic-list")).FindElements(By.TagName("select"))[2];
            SelectElement select6 = new SelectElement(selectColor6);
            Assert.IsTrue(select6.AllSelectedOptions[0].Text.Contains("Нет цвета"));

            //check admin photo filter color
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"PhotoFilterColor\"]")))).SelectByText("Color7");
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"PhotoImg\"]")).Count == 1);

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"PhotoFilterColor\"]")))).SelectByText("Color1");
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"PhotoImg\"]")).Count == 1);

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"PhotoFilterColor\"]")))).SelectByText("Нет цвета");
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"PhotoImg\"]")).Count == 1);

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"PhotoFilterColor\"]")))).SelectByText("Все");
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"PhotoImg\"]")).Count == 3);

            //check client
            GoToClient("products/test-product7");
            Assert.IsFalse(driver.FindElement(By.CssSelector(".gallery-picture.text-static img")).GetAttribute("src").Contains("nophoto"));
        }
        
        [Test]
        public void ProductEditPhoto360DeleteByCheckBox()
        {
            GoToClient("products/test-product10");
            Assert.IsFalse(driver.FindElements(By.CssSelector("[data-modal-open=\"modalProductRotate\"]")).Count > 0);

            GoToAdmin("product/edit/10");
            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Фотографии')]")).Click();
            Thread.Sleep(2000);
            Assert.IsFalse(driver.FindElements(By.CssSelector("[data-e2e=\"Photo360Img\"]")).Count > 0);
            Assert.IsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"Photo360Input\"]")).Selected);
            
            ScrollTo(By.CssSelector("[data-e2e=\"imgByHref\"]"));
            driver.FindElement(By.CssSelector("[data-e2e=\"Photo360CheckBox\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("(//input[@type='file'])[3]")).SendKeys(GetPicturePath("pics3d\\2.jpg")); //selenium can't upload miltiple files
            Thread.Sleep(2000);
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"Photo360Input\"]")).Selected);
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"Photo360Img\"]")).Count == 1);

            GoToAdmin("product/edit/10");
            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Фотографии')]")).Click();
            WaitForElem(By.Id("productPhotosSortable"));
            ScrollTo(By.CssSelector("[data-e2e=\"imgByHref\"]"));
            WaitForElem(By.CssSelector("[data-e2e=\"Photo360CheckBox\"]"));
            driver.FindElement(By.CssSelector("[data-e2e=\"Photo360CheckBox\"]")).Click();
            Thread.Sleep(1000);

            //check admin
            GoToAdmin("product/edit/10");
            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Фотографии')]")).Click();
            Thread.Sleep(2000);
            Assert.IsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"Photo360Input\"]")).Selected);
            Assert.IsFalse(driver.FindElements(By.CssSelector("[data-e2e=\"Photo360Img\"]")).Count > 0);

            //check client
            GoToClient("products/test-product10");
            Assert.IsFalse(driver.FindElements(By.CssSelector("[data-modal-open=\"modalProductRotate\"]")).Count > 0);
        }

        [Test]
        public void ProductEditPhotoChangeMain()
        {
            GoToAdmin("product/edit/12");
            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Фотографии')]")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(driver.FindElement(By.CssSelector(".aside-image img")).GetAttribute("ng-src").Contains("nophoto"));
            Assert.IsFalse(driver.FindElements(By.CssSelector(".italic.hover-padding-left")).Count > 0);
            ScrollTo(By.CssSelector("[data-e2e=\"imgByHref\"]"));
            driver.FindElement(By.XPath("(//input[@type='file'])")).Clear();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("(//input[@type='file'])")).SendKeys(GetPicturePath("big.png"));
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("(//input[@type='file'])")).Clear();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("(//input[@type='file'])")).SendKeys(GetPicturePath("pictures-1.jpg"));
            Thread.Sleep(2000);

            GoToAdmin("product/edit/12");
            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Фотографии')]")).Click();
            WaitForElem(By.CssSelector("[data-e2e=\"MainPhotoInput\"]"));
            string Img1 = driver.FindElement(By.Id("leftAsideProduct")).FindElement(By.TagName("img")).GetAttribute("src");

            ScrollTo(By.CssSelector("[data-e2e=\"imgByHref\"]"));
            Thread.Sleep(4000);
            Assert.IsFalse(driver.FindElement(By.CssSelector(".product-pic-list")).FindElements(By.CssSelector("[data-e2e=\"MainPhotoInput\"]"))[1].Selected);
            Assert.IsTrue(driver.FindElement(By.CssSelector(".product-pic-list")).FindElements(By.CssSelector("[data-e2e=\"MainPhotoInput\"]"))[0].Selected);
            
            Actions a = new Actions(driver);
            a.Build();
            a.MoveToElement(driver.FindElement(By.CssSelector(".product-pic-list")).FindElements(By.CssSelector("[data-e2e=\"PhotoImg\"]"))[1]);
            a.Perform();
            driver.FindElement(By.CssSelector(".product-pic-list")).FindElements(By.CssSelector("[data-e2e=\"PhotoImg\"]"))[1].FindElement(By.CssSelector("[data-e2e=\"MainPhoto\"]")).Click();
            
            //check admin
            GoToAdmin("product/edit/12");
            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Фотографии')]")).Click();
            WaitForElem(By.CssSelector("[data-e2e=\"MainPhotoInput\"]"));
            string Img2 = driver.FindElement(By.Id("leftAsideProduct")).FindElement(By.TagName("img")).GetAttribute("src");
            Assert.IsFalse(Img1.Equals(Img2));
        }

        [Test]
        public void ProductEditPhotoAltAdd()
        {
            GoToAdmin("product/edit/13");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Фотографии')]")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("(//input[@type='file'])")).Clear();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("(//input[@type='file'])")).SendKeys(GetPicturePath("big.png"));
            Thread.Sleep(4000);

            Actions a = new Actions(driver);
            a.Build();
            a.MoveToElement(driver.FindElement(By.CssSelector(".product-pic-list")).FindElement(By.CssSelector("[data-e2e=\"PhotoImg\"]")));
            a.Perform();
            driver.FindElement(By.CssSelector(".product-pic-list")).FindElement(By.CssSelector("[data-e2e=\"PhotoItemEdit\"] a")).Click();
            WaitForElem(By.CssSelector("[data-e2e=\"PhotoAltEdit\"]"));

            driver.FindElement(By.CssSelector("[data-e2e=\"PhotoAltEdit\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"PhotoAltEdit\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"PhotoAltEdit\"]")).SendKeys("TestAltPhoto");
            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(1000);
            
            //check client
            GoToClient("products/test-product13");
            Assert.IsTrue(driver.FindElement(By.CssSelector(".gallery-picture.text-static img")).GetAttribute("alt").Contains("TestAltPhoto"));
        }

        [Test]
        public void ProductEditPhotoAltEdit()
        {
            //pre check client
            GoToClient("products/test-product14");
            Assert.IsTrue(driver.FindElement(By.CssSelector(".gallery-picture.text-static img")).GetAttribute("alt").Contains("TestAltPhotoFirst"));

            GoToAdmin("product/edit/14");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Фотографии')]")).Click();
            Thread.Sleep(4000);

            Actions a = new Actions(driver);
            a.Build();
            a.MoveToElement(driver.FindElement(By.CssSelector(".product-pic-list")).FindElement(By.CssSelector("[data-e2e=\"PhotoImg\"]")));
            a.Perform();
            driver.FindElement(By.CssSelector(".product-pic-list")).FindElement(By.CssSelector("[data-e2e=\"PhotoItemEdit\"] a")).Click();
            WaitForElem(By.CssSelector("[data-e2e=\"PhotoAltEdit\"]"));

            driver.FindElement(By.CssSelector("[data-e2e=\"PhotoAltEdit\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"PhotoAltEdit\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"PhotoAltEdit\"]")).SendKeys("Edited Alt Text");
            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(1000);

            //check client
            GoToClient("products/test-product14");
            Assert.IsTrue(driver.FindElement(By.CssSelector(".gallery-picture.text-static img")).GetAttribute("alt").Contains("Edited Alt Text"));

        }
    }
}