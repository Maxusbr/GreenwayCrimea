using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;
using OpenQA.Selenium.Interactions;

namespace AdvantShop.SeleniumTest.Admin.Catalog.ProductAddEdit.ReviewsAndCopy
{
    [TestFixture]
    public class ProductAddEditReviewAndCopyTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog | ClearType.Customers | ClearType.CMS);
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
                "data\\Admin\\Catalog\\ProductAddEdit\\Catalog.Size.csv",
                    "data\\Admin\\Catalog\\ProductAddEdit\\Customers.Customer.csv",
           "data\\Admin\\Catalog\\ProductAddEdit\\Customers.CustomerGroup.csv",
           "data\\Admin\\Catalog\\ProductAddEdit\\Customers.Departments.csv",
           "data\\Admin\\Catalog\\ProductAddEdit\\Customers.Managers.csv",
            "data\\Admin\\Catalog\\ProductAddEdit\\Customers.ManagerTask.csv",
            "data\\Admin\\Catalog\\ProductAddEdit\\CMS.Review.csv",
            "data\\Admin\\Catalog\\ProductAddEdit\\CMS.StaticPage.csv",
            //"data\\Admin\\Catalog\\ProductAddEdit\\CMS.StaticBlock.csv",
            "data\\Admin\\Catalog\\ProductAddEdit\\CMS.Menu.csv"
           );

            Init();
        }

        [Test]
        public void ProductEditReviews()
        {
            GoToAdmin("product/edit/4");

            Assert.AreEqual("3 смотреть", driver.FindElement(By.CssSelector("[data-e2e=\"ProductReviewsCount\"]")).Text);
            driver.FindElement(By.CssSelector("[data-e2e=\"Review\"]")).Click();
            Thread.Sleep(2000);

            //focus to second browser tab
            Functions.OpenNewTab(driver, baseURL);

            //check admin
            Assert.AreEqual("Давно искала такое платье! Доставили очень оперативно, спасибо!", GetGridCell(1, "Text").Text);

            Functions.CloseTab(driver, baseURL);

            //check client
            GoToClient("products/test-product4");
            Assert.IsTrue(driver.PageSource.Contains("Давно искала такое платье! Доставили очень оперативно, спасибо!"));
            Assert.IsTrue(driver.PageSource.Contains("3 отзыва"));
        }


        [Test]
        public void ProductEditNoReviews()
        {
            GoToAdmin("product/edit/10");

            Assert.AreEqual("0 смотреть", driver.FindElement(By.CssSelector("[data-e2e=\"ProductReviewsCount\"]")).Text);
            driver.FindElement(By.CssSelector("[data-e2e=\"Review\"]")).Click();
            Thread.Sleep(2000);

            //focus to second browser tab
            Functions.OpenNewTab(driver, baseURL);

            //check admin
            Assert.AreEqual("Отзывы", driver.FindElement(By.TagName("h1")).Text);

            Functions.CloseTab(driver, baseURL);

            //check client
            GoToClient("products/test-product10");
            Assert.False(driver.PageSource.Contains("Отзывы (1)"));
        }


        [Test]
        public void ProductEditDoCopy()
        {
            GoToAdmin("product/edit/11");

            ScrollTo(By.Id("ArtNo"));
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductCopy\"]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Добавление копии товара", driver.FindElement(By.CssSelector(".modal-dialog")).FindElement(By.TagName("h2")).Text);
            Assert.AreEqual("TestProduct11 - копия", driver.FindElement(By.CssSelector(".modal-dialog")).FindElement(By.TagName("input")).GetAttribute("value"));
            
            driver.FindElement(By.XPath("//span[contains(text(), 'Добавить')]")).Click();
            Thread.Sleep(7000);
            WaitForAjax();
            
            //check admin
            Assert.AreEqual("Товар \"TestProduct11 - копия\"", driver.FindElement(By.TagName("h1")).Text);
            Assert.AreEqual("TestProduct11 - копия", driver.FindElement(By.Id("Name")).GetAttribute("value"));
            Assert.IsTrue(driver.FindElement(By.Id("Enabled")).Selected);
            Assert.AreEqual("TestCategory1 (Основная)", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElement(By.TagName("option")).Text);
            Assert.IsFalse(driver.FindElement(By.Id("Recomended")).Selected);
            Assert.IsFalse(driver.FindElement(By.Id("Sales")).Selected);
            Assert.IsFalse(driver.FindElement(By.Id("BestSeller")).Selected);
            Assert.IsFalse(driver.FindElement(By.Id("New")).Selected);
            driver.FindElement(By.XPath("//div[contains(text(), 'Цена и наличие')]")).Click();
            Thread.Sleep(2000);
            IWebElement selectCurrency = driver.FindElement(By.Id("CurrencyId"));
            SelectElement select1 = new SelectElement(selectCurrency);
            Assert.IsTrue(select1.AllSelectedOptions[0].Text.Contains("Рубли"));
            Assert.AreEqual("0", driver.FindElement(By.Id("DiscountPercent")).GetAttribute("value"));
            Assert.IsTrue(GetGridCell(0, "Size", "Offers").FindElement(By.CssSelector("[data-e2e=\"itemSelected\"]")).Text.Contains("SizeName11"));
            Assert.IsTrue(GetGridCell(0, "Color", "Offers").FindElement(By.CssSelector("[data-e2e=\"itemSelected\"]")).Text.Contains("Color11"));
            Assert.AreEqual("11", GetGridCell(0, "BasePrice", "Offers").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("11", GetGridCell(0, "SupplyPrice", "Offers").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("11", GetGridCell(0, "Amount", "Offers").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("unit", driver.FindElement(By.Id("Unit")).GetAttribute("value"));
            Assert.AreEqual("", driver.FindElement(By.Id("MinAmount")).GetAttribute("value"));
            Assert.AreEqual("", driver.FindElement(By.Id("MaxAmount")).GetAttribute("value"));
            Assert.AreEqual("1", driver.FindElement(By.Id("Multiplicity")).GetAttribute("value"));
            driver.FindElement(By.XPath("//div[contains(text(), 'Доставка')]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("11", driver.FindElement(By.Id("Length")).GetAttribute("value"));
            Assert.AreEqual("11", driver.FindElement(By.Id("Width")).GetAttribute("value"));
            Assert.AreEqual("11", driver.FindElement(By.Id("Height")).GetAttribute("value"));
            Assert.AreEqual("11", driver.FindElement(By.Id("Weight")).GetAttribute("value"));
            Assert.AreEqual("11", driver.FindElement(By.Id("ShippingPrice")).GetAttribute("value"));
            driver.FindElement(By.XPath("//div[contains(text(), 'Описание')]")).Click();
            Thread.Sleep(4000);
            AssertCkText("briefDesc11", "BriefDescription");
            AssertCkText("Desc11", "Description");
            driver.FindElement(By.XPath("//div[contains(text(), 'Свойства товара')]")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(driver.FindElement(By.CssSelector(".properties-item-value")).Text.Contains("PropertyValue12"));
            Assert.AreEqual("Property2", driver.FindElement(By.CssSelector(".properties-item-name")).Text);
            driver.FindElement(By.XPath("//div[contains(text(), 'SEO')]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("testproduct11-kopiya", driver.FindElement(By.Id("UrlPath")).GetAttribute("value"));
            Assert.IsTrue(driver.FindElement(By.Id("DefaultMeta")).Selected);
            
            //check client
            Assert.IsFalse(Is404Page("products/testproduct11-kopiya"));
        }

        [Test]
        public void ProductEditDelete()
        {
            Assert.IsFalse(Is404Page("products/test-product12"));

            GoToAdmin("product/edit/12");
            
            ScrollTo(By.XPath("//a[contains(text(), 'Удалить')]"));
            driver.FindElement(By.XPath("//a[contains(text(), 'Удалить')]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("catalog?categoryid=1");
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("TestProduct12");
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));

            GoToAdmin("catalog?showMethod=AllProducts");
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("TestProduct12");
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));

            //check client
            Assert.IsTrue(Is404Page("products/test-product12"));
        }

    }
}