using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;
using AdvantShop.Selenium.Core.Infrastructure;

namespace AdvantShop.SeleniumTest.Admin.Catalog.ProductAddEdit.Description
{
    [TestFixture]
    public class ProductAddEditDescriptionTest : BaseSeleniumTest
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
        public void ProductEditChangeDescription()
        {
            GoToAdmin("product/edit/1");

            driver.FindElement(By.XPath("//div[contains(text(), 'Описание')]")).Click();
            Thread.Sleep(1000);

            SetCkText("Brief_Description_here", "BriefDescription");
            SetCkText("Description_here", "Description");

            Thread.Sleep(1000);
            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);
            
            GoToClient("products/test-product1");
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-tab-content=\"tabDescription\"]")).Text.Contains("Description_here"));
            GoToClient("categories/test-category1");
            Assert.IsTrue(driver.PageSource.Contains("Brief_Description_here"));
        }

        [Test]
        public void ProductAddDescription()
        {
            GoToAdmin("catalog");

            driver.FindElement(By.CssSelector("[data-e2e=\"AddProduct\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"inputProductName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"inputProductName\"]")).SendKeys("new_product");
            driver.FindElement(By.CssSelector(".modal-dialog .edit")).Click();
            driver.FindElement(By.CssSelector(".modal-dialog [data-tree-id=\"categoryItemId_2\"]")).Click();
            driver.FindElement(By.CssSelector(".modal-dialog .btn-save")).Click();// вторая кнопка во втором модальном окне
            driver.FindElement(By.CssSelector(".modal-dialog .btn-save")).Click(); // первая кнопка в первом модальном окне

            driver.FindElement(By.XPath("//div[contains(text(), 'Описание')]")).Click();
            Thread.Sleep(1000);

            SetCkText("NEW_Brief_Description_here", "BriefDescription");
            SetCkText("NEW_Description_here", "Description");

            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            
            GoToClient("products/new_product");
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-tab-content=\"tabDescription\"]")).Text.Contains("NEW_Description_here"));
            GoToClient("categories/test-category2");
            Assert.IsTrue(driver.PageSource.Contains("NEW_Brief_Description_here"));
        }
    }
}
