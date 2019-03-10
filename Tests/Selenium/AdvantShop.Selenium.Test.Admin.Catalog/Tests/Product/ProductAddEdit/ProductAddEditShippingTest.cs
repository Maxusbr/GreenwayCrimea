using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;
using AdvantShop.Selenium.Core.Infrastructure;

namespace AdvantShop.SeleniumTest.Admin.Catalog.ProductAddEdit.Shipping
{
    [TestFixture]
    public class ProductAddEditShippingTest : BaseSeleniumTest
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
        public void ProductEditWeightEdit()
        {
            GoToClient("products/test-product1");
            Thread.Sleep(2000);
            Assert.AreEqual("1", driver.FindElement(By.CssSelector(".details-row.details-weight")).FindElement(By.CssSelector(".inplace-offset.details-param-value-weight")).Text);
            Assert.IsTrue(driver.FindElements(By.CssSelector(".details-row.details-weight")).Count == 1);

            GoToAdmin("product/edit/1");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Доставка')]")).Click();
            Thread.Sleep(1000);

            Assert.AreEqual("1", driver.FindElement(By.Id("Weight")).GetAttribute("value"));
            
            driver.FindElement(By.Id("Weight")).Click();
            driver.FindElement(By.Id("Weight")).Clear();
            driver.FindElement(By.Id("Weight")).SendKeys("100");

            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("product/edit/1");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Доставка')]")).Click();
            Thread.Sleep(1000);

            Assert.AreEqual("100", driver.FindElement(By.Id("Weight")).GetAttribute("value"));

            //check client
            GoToClient("products/test-product1");
            Assert.AreEqual("100", driver.FindElement(By.CssSelector(".details-row.details-weight")).FindElement(By.CssSelector(".inplace-offset.details-param-value-weight")).Text);
            Assert.IsTrue(driver.FindElements(By.CssSelector(".details-row.details-weight")).Count == 1);
        }


        [Test]
        public void ProductEditWeightAdd()
        {
            GoToClient("products/test-product4");
            Thread.Sleep(2000);
            Assert.IsTrue(driver.FindElements(By.CssSelector(".details-row.details-weight")).Count == 0);

            GoToAdmin("product/edit/4");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Доставка')]")).Click();
            Thread.Sleep(1000);

            Assert.AreEqual("0", driver.FindElement(By.Id("Weight")).GetAttribute("value"));
            
            driver.FindElement(By.Id("Weight")).Click();
            driver.FindElement(By.Id("Weight")).Clear();
            driver.FindElement(By.Id("Weight")).SendKeys("400");

            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("product/edit/4");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Доставка')]")).Click();
            Thread.Sleep(1000);

            Assert.AreEqual("400", driver.FindElement(By.Id("Weight")).GetAttribute("value"));

            //check client
            GoToClient("products/test-product4");
            Assert.AreEqual("400", driver.FindElement(By.CssSelector(".details-row.details-weight")).FindElement(By.CssSelector(".inplace-offset.details-param-value-weight")).Text);
            Assert.IsTrue(driver.FindElements(By.CssSelector(".details-row.details-weight")).Count == 1);
        }

        [Test]
        public void ProductEditLengthWidthHeightEdit()
        {
            GoToClient("products/test-product6");
            Assert.AreEqual("6 x 6 x 6", driver.FindElement(By.CssSelector(".details-row.details-dimensions")).FindElement(By.CssSelector(".details-param-value")).Text);
            Assert.IsTrue(driver.FindElements(By.CssSelector(".details-row.details-dimensions")).Count == 1);

            GoToAdmin("product/edit/6");
            
            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Доставка')]")).Click();
            Thread.Sleep(1000);

            Assert.AreEqual("6", driver.FindElement(By.Id("Length")).GetAttribute("value"));
            Assert.AreEqual("6", driver.FindElement(By.Id("Width")).GetAttribute("value"));
            Assert.AreEqual("6", driver.FindElement(By.Id("Height")).GetAttribute("value"));
            
            driver.FindElement(By.Id("Length")).Click();
            driver.FindElement(By.Id("Length")).Clear();
            driver.FindElement(By.Id("Length")).SendKeys("600");
            driver.FindElement(By.Id("Width")).Click();
            driver.FindElement(By.Id("Width")).Clear();
            driver.FindElement(By.Id("Width")).SendKeys("600");
            driver.FindElement(By.Id("Height")).Click();
            driver.FindElement(By.Id("Height")).Clear();
            driver.FindElement(By.Id("Height")).SendKeys("600");

            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("product/edit/6");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Доставка')]")).Click();
            Thread.Sleep(1000);

            Assert.AreEqual("600", driver.FindElement(By.Id("Length")).GetAttribute("value"));
            Assert.AreEqual("600", driver.FindElement(By.Id("Width")).GetAttribute("value"));
            Assert.AreEqual("600", driver.FindElement(By.Id("Height")).GetAttribute("value"));

            //check client
            GoToClient("products/test-product6");
            Assert.AreEqual("600 x 600 x 600", driver.FindElement(By.CssSelector(".details-row.details-dimensions")).FindElement(By.CssSelector(".details-param-value")).Text);
        }


        [Test]
        public void ProductEditLengthWidthHeightAdd()
        {
            GoToClient("products/test-product5");
            Assert.IsTrue(driver.FindElements(By.CssSelector(".details-row.details-dimensions")).Count == 0);

            GoToAdmin("product/edit/5");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Доставка')]")).Click();
            Thread.Sleep(1000);

            Assert.AreEqual("0", driver.FindElement(By.Id("Length")).GetAttribute("value"));
            Assert.AreEqual("0", driver.FindElement(By.Id("Width")).GetAttribute("value"));
            Assert.AreEqual("0", driver.FindElement(By.Id("Height")).GetAttribute("value"));
            
            driver.FindElement(By.Id("Length")).Click();
            driver.FindElement(By.Id("Length")).Clear();
            driver.FindElement(By.Id("Length")).SendKeys("500");
            driver.FindElement(By.Id("Width")).Click();
            driver.FindElement(By.Id("Width")).Clear();
            driver.FindElement(By.Id("Width")).SendKeys("500");
            driver.FindElement(By.Id("Height")).Click();
            driver.FindElement(By.Id("Height")).Clear();
            driver.FindElement(By.Id("Height")).SendKeys("500");
            
            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("product/edit/5");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Доставка')]")).Click();
            Thread.Sleep(1000);

            Assert.AreEqual("500", driver.FindElement(By.Id("Length")).GetAttribute("value"));
            Assert.AreEqual("500", driver.FindElement(By.Id("Width")).GetAttribute("value"));
            Assert.AreEqual("500", driver.FindElement(By.Id("Height")).GetAttribute("value"));

            //check client
            GoToClient("products/test-product5");
            Assert.AreEqual("500 x 500 x 500", driver.FindElement(By.CssSelector(".details-row.details-dimensions")).FindElement(By.CssSelector(".details-param-value")).Text);
        }


        [Test]
        public void ProductEditShippingPriceEdit()
        {
            GoToAdmin("product/edit/8");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Доставка')]")).Click();
            Thread.Sleep(1000);

            Assert.AreEqual("8", driver.FindElement(By.Id("Weight")).GetAttribute("value"));
            
            driver.FindElement(By.Id("ShippingPrice")).Click();
            driver.FindElement(By.Id("ShippingPrice")).Clear();
            driver.FindElement(By.Id("ShippingPrice")).SendKeys("800");

            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("product/edit/8");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Доставка')]")).Click();
            Thread.Sleep(1000);

            Assert.AreEqual("800", driver.FindElement(By.Id("ShippingPrice")).GetAttribute("value"));
        }


        [Test]
        public void ProductEditShippingPriceAdd()
        {
            GoToAdmin("product/edit/7");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Доставка')]")).Click();
            Thread.Sleep(1000);

            Assert.AreEqual("0", driver.FindElement(By.Id("ShippingPrice")).GetAttribute("value"));
            
            driver.FindElement(By.Id("ShippingPrice")).Click();
            driver.FindElement(By.Id("ShippingPrice")).Clear();
            driver.FindElement(By.Id("ShippingPrice")).SendKeys("700");

            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("product/edit/7");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Доставка')]")).Click();
            Thread.Sleep(1000);

            Assert.AreEqual("700", driver.FindElement(By.Id("ShippingPrice")).GetAttribute("value"));
        }
    }
}