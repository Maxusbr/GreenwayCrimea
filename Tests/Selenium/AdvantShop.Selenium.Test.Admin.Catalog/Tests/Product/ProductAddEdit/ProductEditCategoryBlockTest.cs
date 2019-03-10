using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using AdvantShop.Selenium.Core.Infrastructure;

namespace AdvantShop.SeleniumTest.Admin.Catalog.ProductAddEdit.CategoryBlock
{
    [TestFixture]
    public class ProductEditCategoryBlockTest : BaseSeleniumTest
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
        public void ProductAdd()
        {
            GoToAdmin("catalog");
            driver.FindElement(By.CssSelector("[data-e2e=\"AddProduct\"]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Добавление товара", driver.FindElement(By.TagName("h2")).Text);
            driver.FindElement(By.XPath("//a[contains(text(), 'Выбрать')]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Родительская категория", driver.FindElement(By.TagName("h2")).Text);
            driver.FindElements(By.CssSelector(".jstree-anchor"))[1].Click();

            driver.FindElement(By.XPath("//button[contains(text(), 'Изменить')]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".modal-body")).FindElements(By.TagName("input"))[1].Click();
            driver.FindElement(By.CssSelector(".modal-body")).FindElements(By.TagName("input"))[1].SendKeys("ProductNew");

            driver.FindElement(By.XPath("//span[contains(text(), 'Добавить')]")).Click();
            Thread.Sleep(2000);

            GoToAdmin("catalog?categoryid=1");
            GetGridFilter().Click();
            GetGridFilter().SendKeys("ProductNew");
            DropFocus("h2");
            Assert.AreEqual("ProductNew", GetGridCell(0, "Name").Text);
        }
        
        [Test]
        public void ProductAddCheckLook()
        {
            GoToAdmin("catalog");
            driver.FindElement(By.CssSelector("[data-e2e=\"AddProduct\"]")).Click();
            Thread.Sleep(4000);
            driver.FindElement(By.XPath("//a[contains(text(), 'Выбрать')]")).Click();
            Thread.Sleep(4000);
            
            //  driver.FindElements(By.CssSelector(".jstree-anchor"))[1].Click();
            driver.FindElements(By.CssSelector(".jstree-anchor"))[1].FindElement(By.CssSelector(".jstree-advantshop-name")).Click();
            //driver.FindElements(By.Id("1"))[1].FindElement(By.CssSelector(".jstree-advantshop-name")).Click();
            Thread.Sleep(4000);
           
            driver.FindElement(By.XPath("//button[contains(text(), 'Изменить')]")).Click();
            Thread.Sleep(4000);
            driver.FindElement(By.CssSelector(".modal-body")).FindElements(By.TagName("input"))[1].Click();
            driver.FindElement(By.CssSelector(".modal-body")).FindElements(By.TagName("input"))[1].SendKeys("ProductNewCheckLookButton");

            driver.FindElement(By.XPath("//span[contains(text(), 'Добавить')]")).Click();
            Thread.Sleep(4000);

            driver.FindElement(By.XPath("//a[contains(text(), 'Просмотр')]")).Click();
            Thread.Sleep(5000);

            //focus to another browser tab
            Functions.OpenNewTab(driver, baseURL);
            Assert.IsTrue(driver.PageSource.Contains("ProductNewCheckLookButton"));
            Assert.IsTrue(driver.Url.Contains("products/productnewchecklookbutton"));

            //close tab
            Functions.CloseTab(driver, baseURL);
        }

        [Test]
        public void aProductEditToDisabledAddCategory()
        {
            GoToAdmin("product/edit/1");
            
            //check change name
            driver.FindElement(By.Id("Name")).Click();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys("ChangedNameTestProduct1");
            driver.FindElement(By.XPath("//h2[contains(text(), 'Основное')]")).Click();

            //check change activity
            driver.FindElement(By.CssSelector("[data-e2e=\"CheckBoxEnabledClick\"]")).Click();

            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            GoToAdmin("product/edit/1");

            Assert.AreEqual("TestCategory1 (Основная)", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElement(By.TagName("option")).Text);
            Assert.AreEqual("TestCategory1 (Основная)", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElement(By.CssSelector("[value=\"1\"]")).GetAttribute("label"));

            //check add category
            driver.FindElement(By.CssSelector("[data-e2e=\"CategoryAdd\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory2')]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//button[contains(text(), 'Выбрать')]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("TestCategory2", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElement(By.CssSelector("[value=\"2\"]")).GetAttribute("label"));

            GoToAdmin("product/edit/1");

            //check product card
            Assert.AreEqual("ChangedNameTestProduct1", driver.FindElement(By.Id("Name")).GetAttribute("value"));
            Assert.IsFalse(driver.FindElement(By.Id("Enabled")).Selected);
            Assert.AreEqual("TestCategory1 (Основная)", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElements(By.TagName("option"))[0].Text);
            Assert.AreEqual("TestCategory1 (Основная)", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElement(By.CssSelector("[value=\"1\"]")).GetAttribute("label"));
            Assert.AreEqual("TestCategory2", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElement(By.CssSelector("[value=\"2\"]")).GetAttribute("label"));
            Assert.AreEqual("TestCategory2", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElements(By.TagName("option"))[1].Text);
            
            //check admin grid
            GoToAdmin("catalog?categoryId=1");
            //GetGridFilter().Click();
            //GetGridFilter().Clear();
            //GetGridFilter().SendKeys("ChangedNameTestProduct1");
            //DropFocus("h2");
            //WaitForAjax();
            Assert.AreEqual("ChangedNameTestProduct1", GetGridCell(0, "Name").Text);
            Assert.IsFalse(GetGridCell(0, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);

            //check client
            Assert.IsTrue(Is404Page("products/test-product1"));
        }

        [Test]
        public void ProductEditToEnabledSetMainCategory()
        {
            GoToAdmin("product/edit/2");
            Assert.AreEqual("Товар \"TestProduct2\"", driver.FindElement(By.TagName("h1")).Text);
            Assert.AreEqual("TestCategory1", driver.FindElement(By.CssSelector(".breadcrumb")).FindElements(By.CssSelector(".link-invert"))[1].Text);

            //check change activity
            driver.FindElement(By.CssSelector("[data-e2e=\"CheckBoxEnabledClick\"]")).Click();

            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            GoToAdmin("product/edit/2");

            Assert.AreEqual("TestCategory1 (Основная)", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElement(By.TagName("option")).Text);
            Assert.AreEqual("TestCategory1 (Основная)", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElement(By.CssSelector("[value=\"1\"]")).GetAttribute("label"));

            //check add category
            driver.FindElement(By.CssSelector("[data-e2e=\"CategoryAdd\"]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Категория", driver.FindElement(By.TagName("h2")).Text);
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory3')]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//button[contains(text(), 'Выбрать')]")).Click();
            Thread.Sleep(2000);

            //check set another main category
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElements(By.TagName("option"))[1].Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"CategorySetMain\"]")).Click();
            Thread.Sleep(1000);

            GoToAdmin("product/edit/2");

            //check product card
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"ProductEnabled\"]")).FindElement(By.TagName("input")). Selected);
            Assert.AreEqual("TestCategory3 (Основная)", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElement(By.TagName("option")).Text);
            Assert.AreEqual("TestCategory3 (Основная)", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElement(By.CssSelector("[value=\"3\"]")).GetAttribute("label"));
            Assert.AreEqual("TestCategory1", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElement(By.CssSelector("[value=\"1\"]")).GetAttribute("label"));
            Assert.AreEqual("TestCategory3", driver.FindElement(By.CssSelector(".breadcrumb")).FindElements(By.CssSelector(".link-invert"))[1].Text);
            
            //check admin grid
            GoToAdmin("catalog?categoryId=1");
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("TestProduct2");
            DropFocus("h2");
            WaitForAjax();
            GetGridCell(-1, "SortOrder").Click();
            Assert.AreEqual("TestProduct2", GetGridCell(0, "Name").Text);
            Assert.IsTrue(GetGridCell(0, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);

            GoToAdmin("catalog?categoryId=3");
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("TestProduct2");
            DropFocus("h2");
            WaitForAjax();
            Assert.AreEqual("TestProduct2", GetGridCell(0, "Name").Text);
            Assert.IsTrue(GetGridCell(0, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);

            //check client
            GoToClient("products/test-product2");
            Assert.IsTrue(driver.PageSource.Contains("TestCategory3"));
            GoToClient("categories/test-category3");
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-product-id=\"2\"]")).Count > 0);
            GoToClient("categories/test-category1");
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-product-id=\"2\"]")).Count > 0);
        }

        [Test]
        public void ProductEnabledAddInDisabledCategory()
        {
            GoToAdmin("catalog");
            driver.FindElements(By.CssSelector(".jstree-icon.jstree-ocl"))[3].Click();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory5')]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"AddProduct\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".modal-body")).FindElements(By.TagName("input"))[1].Click();
            driver.FindElement(By.CssSelector(".modal-body")).FindElements(By.TagName("input"))[1].SendKeys("ProductActiveCategoryDisabled");

            driver.FindElement(By.XPath("//span[contains(text(), 'Добавить')]")).Click();
            Thread.Sleep(2000);

            //check admin grid
            GoToAdmin("catalog?categoryid=5");
            GetGridFilter().Click();
            GetGridFilter().SendKeys("ProductActiveCategoryDisabled");
            DropFocus("h2");
            Assert.AreEqual("ProductActiveCategoryDisabled", GetGridCell(0, "Name").Text);
            Assert.IsTrue(GetGridCell(0, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);

            //check client
            Assert.IsTrue(Is404Page("products/productactivecategorydisabled"));
        }
        
        [Test]
        public void ProductEditSetMainCategoryToEnabled()
        {
            GoToAdmin("product/edit/81");
            Assert.IsTrue(Is404Page("products/test-product81"));
            Assert.AreEqual("TestCategory4 → TestCategory5 (Основная)", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElement(By.TagName("option")).Text);
            Assert.AreEqual("TestCategory4 → TestCategory5 (Основная)", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElement(By.CssSelector("[value=\"5\"]")).GetAttribute("label"));

            //check add category
            driver.FindElement(By.CssSelector("[data-e2e=\"CategoryAdd\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory1')]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//button[contains(text(), 'Выбрать')]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("TestCategory1", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElement(By.CssSelector("[value=\"1\"]")).GetAttribute("label"));

            //check set another main category
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElements(By.TagName("option"))[1].Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"CategorySetMain\"]")).Click();
            Thread.Sleep(1000);

            GoToAdmin("product/edit/81");

            //check product card
            Assert.AreEqual("TestCategory1 (Основная)", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElements(By.TagName("option"))[0].Text);
            Assert.AreEqual("TestCategory1 (Основная)", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElement(By.CssSelector("[value=\"1\"]")).GetAttribute("label"));
            Assert.AreEqual("TestCategory4 → TestCategory5", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElement(By.CssSelector("[value=\"5\"]")).GetAttribute("label"));
            Assert.AreEqual("TestCategory4 → TestCategory5", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElements(By.TagName("option"))[1].Text);

            //check client
            Assert.IsFalse(Is404Page("products/test-product81"));
            GoToClient("products/test-product81");
            Assert.IsTrue(driver.PageSource.Contains("TestCategory1"));
            GoToClient("categories/test-category1");
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-product-id=\"81\"]")).Count > 0);
        }

        [Test]
        public void ProductEditSetMainCategoryToDisabled()
        {
            GoToAdmin("product/edit/21");
            Assert.IsFalse(Is404Page("products/test-product21"));
            Assert.AreEqual("TestCategory2 (Основная)", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElement(By.TagName("option")).Text);
            Assert.AreEqual("TestCategory2 (Основная)", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElement(By.CssSelector("[value=\"2\"]")).GetAttribute("label"));

            //check add category
            driver.FindElement(By.CssSelector("[data-e2e=\"CategoryAdd\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElements(By.CssSelector(".jstree-icon.jstree-ocl"))[3].Click();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory5')]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//button[contains(text(), 'Выбрать')]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("TestCategory4 → TestCategory5", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElement(By.CssSelector("[value=\"5\"]")).GetAttribute("label"));

            //check set another main category
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElements(By.TagName("option"))[1].Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"CategorySetMain\"]")).Click();
            Thread.Sleep(1000);

            GoToAdmin("product/edit/21");

            //check product card
            Assert.AreEqual("TestCategory4 → TestCategory5 (Основная)", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElements(By.TagName("option"))[0].Text);
            Assert.AreEqual("TestCategory4 → TestCategory5 (Основная)", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElement(By.CssSelector("[value=\"5\"]")).GetAttribute("label"));
            Assert.AreEqual("TestCategory2", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElement(By.CssSelector("[value=\"2\"]")).GetAttribute("label"));
            Assert.AreEqual("TestCategory2", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElements(By.TagName("option"))[1].Text);

            //check client
            Assert.IsTrue(Is404Page("products/test-product21"));
        }

        [Test]
        public void ProductEditDeleteCategory()
        {
            GoToAdmin("product/edit/31");
            Assert.AreEqual("TestCategory2 (Основная)", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElement(By.TagName("option")).Text);
            Assert.AreEqual("TestCategory2 (Основная)", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElement(By.CssSelector("[value=\"2\"]")).GetAttribute("label"));

            //check add category
            driver.FindElement(By.CssSelector("[data-e2e=\"CategoryAdd\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory1')]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//button[contains(text(), 'Выбрать')]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("TestCategory1", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElement(By.CssSelector("[value=\"1\"]")).GetAttribute("label"));
            
            GoToAdmin("product/edit/31");

            //check product card
            Assert.AreEqual("TestCategory2 (Основная)", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElement(By.TagName("option")).Text);
            Assert.AreEqual("TestCategory2 (Основная)", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElement(By.CssSelector("[value=\"2\"]")).GetAttribute("label"));
            Assert.AreEqual("TestCategory1", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElement(By.CssSelector("[value=\"1\"]")).GetAttribute("label"));
            Assert.AreEqual("TestCategory1", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElements(By.TagName("option"))[1].Text);

            //check delete category
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElements(By.TagName("option"))[1].Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"CategoryDelete\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);

            Assert.AreEqual("TestCategory2 (Основная)", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElement(By.TagName("option")).Text);
            Assert.AreEqual("TestCategory2 (Основная)", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElement(By.CssSelector("[value=\"2\"]")).GetAttribute("label"));
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElements(By.TagName("option")).Count == 1);

            //check client
            GoToClient("products/test-product31");
            Assert.IsTrue(driver.PageSource.Contains("TestCategory2"));
            GoToClient("categories/test-category2");
            //  Assert.IsTrue(driver.PageSource.Contains("TestProduct31"));
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-product-id=\"31\"]")).Count > 0);
            GoToClient("categories/test-category1");
            //   Assert.IsFalse(driver.PageSource.Contains("TestProduct31"));
            Assert.IsFalse(driver.FindElements(By.CssSelector("[data-product-id=\"31\"]")).Count > 0);
        }

        [Test]
        public void ProductEditDeleteMainEnabledCategory()
        {
            GoToAdmin("product/edit/41");
            Assert.AreEqual("TestCategory3 (Основная)", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElement(By.TagName("option")).Text);
            Assert.AreEqual("TestCategory3 (Основная)", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElement(By.CssSelector("[value=\"3\"]")).GetAttribute("label"));

            //check add category
            driver.FindElement(By.CssSelector("[data-e2e=\"CategoryAdd\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory1')]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//button[contains(text(), 'Выбрать')]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("TestCategory1", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElement(By.CssSelector("[value=\"1\"]")).GetAttribute("label"));
            
            GoToAdmin("product/edit/41");

            //check product card
            Assert.AreEqual("TestCategory3 (Основная)", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElement(By.TagName("option")).Text);
            Assert.AreEqual("TestCategory3 (Основная)", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElement(By.CssSelector("[value=\"3\"]")).GetAttribute("label"));
            Assert.AreEqual("TestCategory1", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElement(By.CssSelector("[value=\"1\"]")).GetAttribute("label"));
            Assert.AreEqual("TestCategory1", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElements(By.TagName("option"))[1].Text);

            //check delete category
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElement(By.TagName("option")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"CategoryDelete\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);

            GoToAdmin("product/edit/41");

            Assert.AreEqual("TestCategory1 (Основная)", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElement(By.TagName("option")).Text);
            Assert.AreEqual("TestCategory1 (Основная)", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElement(By.CssSelector("[value=\"1\"]")).GetAttribute("label"));
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElements(By.TagName("option")).Count == 1);

            //check client
            GoToClient("products/test-product41");
            Assert.IsTrue(driver.PageSource.Contains("TestCategory1"));
            GoToClient("categories/test-category1");
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-product-id=\"41\"]")).Count > 0);
            GoToClient("categories/test-category3");
            Assert.IsFalse(driver.FindElements(By.CssSelector("[data-product-id=\"41\"]")).Count > 0);
        }

        [Test]
        public void ProductEditDeleteMainEnabledCategoryToDisabled()
        {
            GoToAdmin("product/edit/42");
            Assert.IsFalse(Is404Page("products/test-product42"));
            Assert.AreEqual("TestCategory3 (Основная)", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElement(By.TagName("option")).Text);
            Assert.AreEqual("TestCategory3 (Основная)", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElement(By.CssSelector("[value=\"3\"]")).GetAttribute("label"));

            //check add category
            driver.FindElement(By.CssSelector("[data-e2e=\"CategoryAdd\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElements(By.CssSelector(".jstree-icon.jstree-ocl"))[3].Click();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory5')]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//button[contains(text(), 'Выбрать')]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("TestCategory4 → TestCategory5", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElement(By.CssSelector("[value=\"5\"]")).GetAttribute("label"));
            
            GoToAdmin("product/edit/42");

            //check product card
            Assert.AreEqual("TestCategory3 (Основная)", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElement(By.TagName("option")).Text);
            Assert.AreEqual("TestCategory3 (Основная)", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElement(By.CssSelector("[value=\"3\"]")).GetAttribute("label"));
            Assert.AreEqual("TestCategory4 → TestCategory5", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElement(By.CssSelector("[value=\"5\"]")).GetAttribute("label"));
            Assert.AreEqual("TestCategory4 → TestCategory5", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElements(By.TagName("option"))[1].Text);

            //check delete category
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElement(By.TagName("option")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"CategoryDelete\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);

            GoToAdmin("product/edit/42");

            Assert.AreEqual("TestCategory4 → TestCategory5 (Основная)", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElement(By.TagName("option")).Text);
            Assert.AreEqual("TestCategory4 → TestCategory5 (Основная)", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElement(By.CssSelector("[value=\"5\"]")).GetAttribute("label"));
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElements(By.TagName("option")).Count == 1);

            //check client
            Assert.IsTrue(Is404Page("products/test-product42"));
        }

        [Test]
        public void ProductEditDeleteMainDisabledCategoryToEnabled()
        {
            Assert.IsTrue(Is404Page("products/test-product82"));
            GoToAdmin("product/edit/82");
            Assert.AreEqual("TestCategory4 → TestCategory5 (Основная)", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElement(By.TagName("option")).Text);
            Assert.AreEqual("TestCategory4 → TestCategory5 (Основная)", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElement(By.CssSelector("[value=\"5\"]")).GetAttribute("label"));

            //check add category
            driver.FindElement(By.CssSelector("[data-e2e=\"CategoryAdd\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory1')]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//button[contains(text(), 'Выбрать')]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("TestCategory1", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElement(By.CssSelector("[value=\"1\"]")).GetAttribute("label"));
            
            GoToAdmin("product/edit/82");

            //check product card
            Assert.AreEqual("TestCategory4 → TestCategory5 (Основная)", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElement(By.TagName("option")).Text);
            Assert.AreEqual("TestCategory4 → TestCategory5 (Основная)", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElement(By.CssSelector("[value=\"5\"]")).GetAttribute("label"));
            Assert.AreEqual("TestCategory1", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElement(By.CssSelector("[value=\"1\"]")).GetAttribute("label"));
            Assert.AreEqual("TestCategory1", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElements(By.TagName("option"))[1].Text);

            //check delete category
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElement(By.TagName("option")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"CategoryDelete\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);

            GoToAdmin("product/edit/82");
            Assert.AreEqual("TestCategory1 (Основная)", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElement(By.TagName("option")).Text);
            Assert.AreEqual("TestCategory1 (Основная)", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElement(By.CssSelector("[value=\"1\"]")).GetAttribute("label"));
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElements(By.TagName("option")).Count == 1);

            //check client
            Assert.IsFalse(Is404Page("products/test-product82"));
            GoToClient("products/test-product82");
            Assert.IsTrue(driver.PageSource.Contains("TestCategory1"));
            GoToClient("categories/test-category1");
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-product-id=\"82\"]")).Count > 0);
            GoToClient("categories/test-category5");
            Assert.IsFalse(driver.FindElements(By.CssSelector("[data-product-id=\"82\"]")).Count > 0);
        }


        [Test]
        public void ProductEditDeleteAllCategories()
        {
            GoToAdmin("product/edit/22");
            Assert.IsFalse(Is404Page("products/test-product22"));
            Assert.AreEqual("TestCategory2 (Основная)", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElement(By.TagName("option")).Text);
            Assert.AreEqual("TestCategory2 (Основная)", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElement(By.CssSelector("[value=\"2\"]")).GetAttribute("label"));

            //check add category
            driver.FindElement(By.CssSelector("[data-e2e=\"CategoryAdd\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory1')]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//button[contains(text(), 'Выбрать')]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("TestCategory1", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElement(By.CssSelector("[value=\"1\"]")).GetAttribute("label"));
            
            GoToAdmin("product/edit/22");

            //check product card
            Assert.AreEqual("TestCategory2 (Основная)", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElement(By.TagName("option")).Text);
            Assert.AreEqual("TestCategory2 (Основная)", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElement(By.CssSelector("[value=\"2\"]")).GetAttribute("label"));
            Assert.AreEqual("TestCategory1", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElement(By.CssSelector("[value=\"1\"]")).GetAttribute("label"));
            Assert.AreEqual("TestCategory1", driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElements(By.TagName("option"))[1].Text);

            //check delete all categories
            driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElement(By.TagName("option")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"CategoryDelete\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElement(By.TagName("option")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"CategoryDelete\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);

            GoToAdmin("product/edit/22");

            Assert.IsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"ProductCategory\"]")).FindElements(By.TagName("option")).Count > 0);
            
            //check admin grid
            GoToAdmin("catalog?categoryId=2");
            GetGridFilter().Click();
            GetGridFilter().SendKeys("TestProduct22");
            DropFocus("h2");
            WaitForAjax();
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));

            GoToAdmin("catalog?showMethod=OnlyWithoutCategories");
            GetGridFilter().Click();
            GetGridFilter().SendKeys("TestProduct22");
            DropFocus("h2");
            WaitForAjax();
            Assert.AreEqual("TestProduct22", GetGridCell(0, "Name").Text);

            //check client
            Assert.IsTrue(Is404Page("products/test-product22"));
        }
    }
}