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
    public class CategoryEditSotr : BaseSeleniumTest
    {
         
         

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(
               "Data\\Admin\\EditCategory\\Catalog.Category.csv",
               "Data\\Admin\\EditCategory\\Catalog.Brand.csv",
               "Data\\Admin\\EditCategory\\Catalog.Property.csv",
                "Data\\Admin\\EditCategory\\Catalog.PropertyValue.csv",
                "Data\\Admin\\EditCategory\\Catalog.ProductPropertyValue.csv",
               "Data\\Admin\\EditCategory\\Catalog.Product.csv",
               "Data\\Admin\\EditCategory\\Catalog.Offer.csv",
               "Data\\Admin\\EditCategory\\Catalog.ProductCategories.csv",
               "Data\\Admin\\EditCategory\\Catalog.PropertyGroup.csv"
                );

             
            Init();

        }
        
        [Test]
        public void SortCategory()
        {
            GoToClient("categories/newcategory");
            Assert.AreEqual("TestProduct1", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block"))[0].FindElement(By.CssSelector(".products-view-name-link")).Text);
            Assert.AreEqual("TestProduct20", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block"))[19].FindElement(By.CssSelector(".products-view-name-link")).Text);
        }
        [Test]
        public void SortNoSortingCategory()
        {
           GoToAdmin("catalog");
            driver.FindElement(By.CssSelector("[data-e2e=\"categoriesBlockItemEdit\"]")).Click();
            Thread.Sleep(2000);            
            
            driver.FindElement(By.Id("UrlPath")).Clear();
            driver.FindElement(By.Id("UrlPath")).SendKeys("newcategory");

            (new SelectElement(driver.FindElement(By.Id("Sorting")))).SelectByText("Без сортировки");           
            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            GoToClient("categories/newcategory");
            Assert.AreEqual("TestProduct1", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block"))[0].FindElement(By.CssSelector(".products-view-name-link")).Text);
            Assert.AreEqual("TestProduct20", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block"))[19].FindElement(By.CssSelector(".products-view-name-link")).Text);

        }
        [Test]
        public void SortByDateCategory()
        {
           GoToAdmin("catalog");
            driver.FindElement(By.CssSelector("[data-e2e=\"categoriesBlockItemEdit\"]")).Click();
            Thread.Sleep(2000);            
            
            driver.FindElement(By.Id("UrlPath")).Clear();
            driver.FindElement(By.Id("UrlPath")).SendKeys("newcategory");

            (new SelectElement(driver.FindElement(By.Id("Sorting")))).SelectByText("Дате добавления, по возрастанию");
             ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            GoToClient("categories/newcategory");
            Assert.AreEqual("TestProduct2", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block"))[0].FindElement(By.CssSelector(".products-view-name-link")).Text);
            Assert.AreEqual("TestProduct1", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block"))[23].FindElement(By.CssSelector(".products-view-name-link")).Text);
        }
        [Test]
        public void SortByNewCategory()
        {
           GoToAdmin("catalog");
            driver.FindElement(By.CssSelector("[data-e2e=\"categoriesBlockItemEdit\"]")).Click();
            Thread.Sleep(2000);            
            
            driver.FindElement(By.Id("UrlPath")).Clear();
            driver.FindElement(By.Id("UrlPath")).SendKeys("newcategory");

            (new SelectElement(driver.FindElement(By.Id("Sorting")))).SelectByText("Новизне");
            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            GoToClient("categories/newcategory");
            Assert.AreEqual("TestProduct1", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block"))[0].FindElement(By.CssSelector(".products-view-name-link")).Text);
            Assert.AreEqual("TestProduct24", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block"))[1].FindElement(By.CssSelector(".products-view-name-link")).Text);
            Assert.AreEqual("TestProduct2", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block"))[23].FindElement(By.CssSelector(".products-view-name-link")).Text);
        }
        [Test]
        public void SortByNameCategory()
        {
           GoToAdmin("catalog");
            driver.FindElement(By.CssSelector("[data-e2e=\"categoriesBlockItemEdit\"]")).Click();
            Thread.Sleep(2000);
                        
            driver.FindElement(By.Id("UrlPath")).Clear();
            driver.FindElement(By.Id("UrlPath")).SendKeys("newcategory");

            (new SelectElement(driver.FindElement(By.Id("Sorting")))).SelectByText("Названию, по возрастанию");
            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            GoToClient("categories/newcategory");
            Assert.AreEqual("TestProduct1", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block"))[0].FindElement(By.CssSelector(".products-view-name-link")).Text);
            Assert.AreEqual("TestProduct10", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block"))[1].FindElement(By.CssSelector(".products-view-name-link")).Text);
            Assert.AreEqual("TestProduct9", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block"))[23].FindElement(By.CssSelector(".products-view-name-link")).Text);
        }
        [Test]
        public void SortByNameDesCategory()
        {
           GoToAdmin("catalog");
            driver.FindElement(By.CssSelector("[data-e2e=\"categoriesBlockItemEdit\"]")).Click();
            Thread.Sleep(2000);            
            
            driver.FindElement(By.Id("UrlPath")).Clear();
            driver.FindElement(By.Id("UrlPath")).SendKeys("newcategory");

            (new SelectElement(driver.FindElement(By.Id("Sorting")))).SelectByText("Названию, по убыванию");
            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            GoToClient("categories/newcategory");
            Assert.AreEqual("TestProduct9", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block"))[0].FindElement(By.CssSelector(".products-view-name-link")).Text);
            Assert.AreEqual("TestProduct8", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block"))[1].FindElement(By.CssSelector(".products-view-name-link")).Text);
            Assert.AreEqual("TestProduct1", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block"))[23].FindElement(By.CssSelector(".products-view-name-link")).Text);
        }
        [Test]
        public void SortByPriceCategory()
        {
            //Functions.RecalculateProducts(driver, baseURL);
            GoToAdmin("catalog");
            driver.FindElement(By.CssSelector("[data-e2e=\"categoriesBlockItemEdit\"]")).Click();
            Thread.Sleep(2000);
                        
            driver.FindElement(By.Id("UrlPath")).Clear();
            driver.FindElement(By.Id("UrlPath")).SendKeys("newcategory");

            (new SelectElement(driver.FindElement(By.Id("Sorting")))).SelectByText("Цене, по возрастанию");
            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            GoToClient("categories/newcategory");
            Assert.AreEqual("TestProduct1", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block"))[0].FindElement(By.CssSelector(".products-view-name-link")).Text);
            Assert.AreEqual("TestProduct2", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block"))[1].FindElement(By.CssSelector(".products-view-name-link")).Text);
            Assert.AreEqual("TestProduct24", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block"))[23].FindElement(By.CssSelector(".products-view-name-link")).Text);
        }
        [Test]
        public void SortByPriceDesCategory()
        {
           // Functions.RecalculateProducts(driver, baseURL);
            GoToAdmin("catalog");
            driver.FindElement(By.CssSelector("[data-e2e=\"categoriesBlockItemEdit\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.Id("UrlPath")).Clear();
            driver.FindElement(By.Id("UrlPath")).SendKeys("newcategory");

            (new SelectElement(driver.FindElement(By.Id("Sorting")))).SelectByText("Цене, по убыванию");
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            GoToClient("categories/newcategory");
            Assert.AreEqual("TestProduct24", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block"))[0].FindElement(By.CssSelector(".products-view-name-link")).Text);
            Assert.AreEqual("TestProduct23", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block"))[1].FindElement(By.CssSelector(".products-view-name-link")).Text);
            Assert.AreEqual("TestProduct1", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block"))[23].FindElement(By.CssSelector(".products-view-name-link")).Text);
        }
        [Test]
        public void SortByRaitingCategory()
        {
           GoToAdmin("catalog");
            driver.FindElement(By.CssSelector("[data-e2e=\"categoriesBlockItemEdit\"]")).Click();
            Thread.Sleep(2000);            
            
            driver.FindElement(By.Id("UrlPath")).Clear();
            driver.FindElement(By.Id("UrlPath")).SendKeys("newcategory");

            (new SelectElement(driver.FindElement(By.Id("Sorting")))).SelectByText("Рейтингу, по возрастанию");
            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            GoToClient("categories/newcategory");
            Assert.AreEqual("TestProduct1", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block"))[0].FindElement(By.CssSelector(".products-view-name-link")).Text);
            Assert.AreEqual("TestProduct20", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block"))[19].FindElement(By.CssSelector(".products-view-name-link")).Text);
        }
        [Test]
        public void SortByRaitingDesCategory()
        {
           GoToAdmin("catalog");
            driver.FindElement(By.CssSelector("[data-e2e=\"categoriesBlockItemEdit\"]")).Click();
            Thread.Sleep(2000);            
            
            driver.FindElement(By.Id("UrlPath")).Clear();
            driver.FindElement(By.Id("UrlPath")).SendKeys("newcategory");

            (new SelectElement(driver.FindElement(By.Id("Sorting")))).SelectByText("Рейтингу, по убыванию");
            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            GoToClient("categories/newcategory");
            Assert.AreEqual("TestProduct1", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block"))[0].FindElement(By.CssSelector(".products-view-name-link")).Text);
            Assert.AreEqual("TestProduct20", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block"))[19].FindElement(By.CssSelector(".products-view-name-link")).Text);
        }
    }
}
