using System;
using NUnit.Framework;
using System.Threading;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.MainPage.Category.EditCategory
{
    [TestFixture]
    public class CategoryEditRecommendationProduct : BaseSeleniumTest
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
        public void GroupPropertyCategory()
        {
            GoToAdmin("catalog");
            driver.FindElement(By.CssSelector("[data-e2e=\"categoriesBlockItemEdit\"]")).Click();
            Thread.Sleep(2000);

            ScrollTo(By.CssSelector("[data-e2e=\"imgDel\"]"));
            driver.FindElement(By.CssSelector("[data-e2e=\"categoryGroupAdd\"]")).Click();
            Thread.Sleep(2000);
            WaitForElem(By.CssSelector(".modal-body"));
            Assert.AreEqual("Добавление группы свойств", driver.FindElement(By.TagName("h2")).Text);
            (new SelectElement(driver.FindElement(By.CssSelector(".modal-body select")))).SelectByText("Group1");
            Thread.Sleep(2000);
            XPathContainsText("button", "Добавить группу");
            ScrollTo(By.Id("UrlPath"));
            Assert.AreEqual("Group1", GetGridCell(0, "Name", "PropertyGroups").Text);
            driver.FindElement(By.CssSelector("[data-e2e=\"categoryGroupAdd\"]")).Click();
            Thread.Sleep(2000);
            WaitForElem(By.CssSelector(".modal-body"));
            (new SelectElement(driver.FindElement(By.CssSelector(".modal-body select")))).SelectByText("Group3");
            XPathContainsText("button", "Добавить группу");
            ScrollTo(By.Id("UrlPath"));
            Assert.AreEqual("Group1", GetGridCell(0, "Name", "PropertyGroups").Text);
            Assert.AreEqual("Group3", GetGridCell(1, "Name", "PropertyGroups").Text);
        }
        [Test]
        public void DelGroupPropertyCategory()
        {
            GoToAdmin("catalog");
            driver.FindElement(By.CssSelector("[data-e2e=\"categoriesBlockItemEdit\"]")).Click();
            Thread.Sleep(2000);

            ScrollTo(By.CssSelector("[data-e2e=\"imgDel\"]"));
            driver.FindElement(By.CssSelector("[data-e2e=\"categoryGroupAdd\"]")).Click();
            Thread.Sleep(2000);
            WaitForElem(By.CssSelector(".modal-body"));
            (new SelectElement(driver.FindElement(By.CssSelector(".modal-body select")))).SelectByText("Group1");
            XPathContainsText("button", "Добавить группу");
            ScrollTo(By.Id("UrlPath"));
            Assert.AreEqual("Group1", GetGridCell(0, "Name", "PropertyGroups").Text);

            driver.FindElement(By.CssSelector("[data-e2e=\"categoryGroupAdd\"]")).Click();
            Thread.Sleep(2000);
            WaitForElem(By.CssSelector(".modal-body"));
            (new SelectElement(driver.FindElement(By.CssSelector(".modal-body select")))).SelectByText("Group3");
            Thread.Sleep(2000);
            XPathContainsText("button", "Добавить группу");
            ScrollTo(By.Id("UrlPath"));
            Assert.AreEqual("Group1", GetGridCell(0, "Name", "PropertyGroups").Text);
            Assert.AreEqual("Group3", GetGridCell(1, "Name", "PropertyGroups").Text);
            GetGridCell(0, "_serviceColumn", "PropertyGroups").FindElement(By.TagName("a")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Group3", GetGridCell(0, "Name", "PropertyGroups").Text);

        }
        [Test]
        public void AddRelatedCategory()
        {
            GoToAdmin("category/edit/7");
            ScrollTo(By.Id("UrlPath"));
            ScrollTo(By.CssSelector("[data-e2e=\"categoryGroupAdd\"]"));

            driver.FindElement(By.CssSelector("[data-type=\"Related\"]")).FindElement(By.CssSelector("[data-e2e=\"RecommendationCategoryAdd\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.Id("3")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("TestCategory3", driver.FindElements(By.CssSelector(".col-xs-4.col-wl-5 .m-b-sm"))[1].Text);
            driver.FindElement(By.CssSelector("[data-type=\"Related\"]")).FindElement(By.CssSelector("[data-e2e=\"RecommendationCategoryAdd\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.Id("2")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("TestCategory2", driver.FindElements(By.CssSelector(".col-xs-4.col-wl-5 .m-b-sm"))[1].Text);
            Assert.AreEqual("TestCategory3", driver.FindElements(By.CssSelector(".col-xs-4.col-wl-5 .m-b-sm"))[2].Text);
            driver.FindElement(By.CssSelector("[data-e2e=\"RecommendationCategoryDelete-id-3\"]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("TestCategory2", driver.FindElements(By.CssSelector(".col-xs-4.col-wl-5 .m-b-sm"))[1].Text);
            
            GoToClient("/products/test-product32");
            Thread.Sleep(2000);
            var element9 = driver.FindElement(By.CssSelector(".tabs.tabs-horizontal.details-tabs"));
            IJavaScriptExecutor jse9 = (IJavaScriptExecutor)driver;
            jse9.ExecuteScript("arguments[0].scrollIntoView(true)", element9);
            Thread.Sleep(2000);

            //С этим товаром покупают
            Assert.AreEqual("С этим товаром покупают", driver.FindElement(By.CssSelector(".h2")).Text);
            Assert.IsTrue(driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item")).Count == 4);

            Assert.AreEqual("TestProduct28", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[0].FindElement(By.CssSelector(".products-view-name-link")).Text);
            Assert.AreEqual("TestProduct30", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[2].FindElement(By.CssSelector(".products-view-name-link")).Text);
            //next 
            driver.FindElement(By.CssSelector(".carousel-nav-next.icon-right-open-after.cs-l-1-interactive")).Click();
            Thread.Sleep(3000);
            Assert.AreEqual("TestProduct29", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[1].FindElement(By.CssSelector(".products-view-name-link")).Text);
            Assert.AreEqual("TestProduct31", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[3].FindElement(By.CssSelector(".products-view-name-link")).Text);
            //pre
            driver.FindElement(By.CssSelector(".carousel-nav-prev.icon-left-open-after.cs-l-1-interactive")).Click();
            Thread.Sleep(3000);
            Assert.AreEqual("TestProduct28", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[0].FindElement(By.CssSelector(".products-view-name-link")).Text);
            Assert.AreEqual("TestProduct30", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[2].FindElement(By.CssSelector(".products-view-name-link")).Text);

        }
        [Test]
        public void AddRelatedProperty()
        {
            GoToAdmin("category/edit/8");
            ScrollTo(By.Id("UrlPath"));
            ScrollTo(By.CssSelector("[data-e2e=\"categoryGroupAdd\"]"));
            driver.FindElement(By.CssSelector("[data-type=\"Related\"]")).FindElement(By.CssSelector("[data-e2e=\"RecommendationPropertyAdd\"]")).Click();
            Thread.Sleep(2000);
            (new SelectElement(driver.FindElement(By.CssSelector(".col-xs-6 select")))).SelectByText("Property5");
            Thread.Sleep(2000);

            (new SelectElement(driver.FindElements(By.CssSelector(".col-xs-6 select"))[1])).SelectByText("5");
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            Thread.Sleep(3000);
            
            driver.FindElement(By.CssSelector("[data-type=\"Related\"]")).FindElement(By.CssSelector("[data-e2e=\"RecommendationPropertyAdd\"]")).Click();
            Thread.Sleep(2000);
            (new SelectElement(driver.FindElement(By.CssSelector(".col-xs-6 select")))).SelectByText("Property1");
            Thread.Sleep(2000);

            (new SelectElement(driver.FindElements(By.CssSelector(".col-xs-6 select"))[1])).SelectByText("1");
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            Thread.Sleep(2000);

            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"RecommendationProperty\"]"))[0].Text.Contains("Property5 - 5"));
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"RecommendationProperty\"]"))[0].Text.Contains("Property1 - 1"));
            driver.FindElement(By.CssSelector("[data-e2e=\"RecommendationPropertyWithValueDelete-value-id-12\"]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.CssSelector("[data-e2e=\"RecommendationPropertyWithValueDelete-value-id-1\"]")).Click();
            Thread.Sleep(2000);
            Assert.IsFalse(driver.FindElements(By.CssSelector("[data-e2e=\"RecommendationProperty\"]"))[0].Text.Contains("Property1 - 1"));
            Assert.IsFalse(driver.FindElements(By.CssSelector("[data-e2e=\"RecommendationProperty\"]"))[0].Text.Contains("Property5 - 5"));
            
           GoToClient("/products/test-product48");
            Thread.Sleep(2000);

            //С этим товаром покупают
            Assert.IsFalse(driver.PageSource.Contains("С этим товаром покупают"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item")).Count == 0);
        }
        [Test]
        public void AddRelatedCategoryAndProperty()
        {
            GoToAdmin("category/edit/9");
            ScrollTo(By.Id("UrlPath"));
            ScrollTo(By.CssSelector("[data-e2e=\"categoryGroupAdd\"]"));

            driver.FindElement(By.CssSelector("[data-type=\"Related\"]")).FindElement(By.CssSelector("[data-e2e=\"RecommendationCategoryAdd\"]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.Id("2")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            Thread.Sleep(3000);
            Assert.AreEqual("TestCategory2", driver.FindElements(By.CssSelector(".col-xs-4.col-wl-5 .m-b-sm"))[1].Text);

            driver.FindElement(By.CssSelector("[data-type=\"Related\"]")).FindElement(By.CssSelector("[data-e2e=\"RecommendationPropertyAdd\"]")).Click();
            Thread.Sleep(3000);

            (new SelectElement(driver.FindElement(By.CssSelector(".col-xs-6 select")))).SelectByText("Property1");
            Thread.Sleep(2000);

            (new SelectElement(driver.FindElements(By.CssSelector(".col-xs-6 select"))[1])).SelectByText("1");
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            Thread.Sleep(3000);

            GoToClient("/products/test-product62");
            Thread.Sleep(3000);
            //С этим товаром покупают
            Assert.AreEqual("С этим товаром покупают", driver.FindElement(By.CssSelector(".h2")).Text);
            Assert.IsTrue(driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item")).Count == 2);

            Assert.AreEqual("TestProduct30", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[0].FindElement(By.CssSelector(".products-view-name-link")).Text);
            Assert.AreEqual("TestProduct31", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[1].FindElement(By.CssSelector(".products-view-name-link")).Text);

        }
        [Test]
        public void AddAlternativeCategory()
        {
            GoToAdmin("category/edit/10");
            ScrollTo(By.Id("UrlPath"));
            ScrollTo(By.CssSelector("[data-e2e=\"categoryGroupAdd\"]"));

            driver.FindElement(By.CssSelector("[data-type=\"Alternative\"]")).FindElement(By.CssSelector("[data-e2e=\"RecommendationCategoryAdd\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.Id("2")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("TestCategory2", driver.FindElements(By.CssSelector(".col-xs-4.col-wl-5 .m-b-sm"))[5].Text);
            driver.FindElement(By.CssSelector("[data-type=\"Alternative\"]")).FindElement(By.CssSelector("[data-e2e=\"RecommendationCategoryAdd\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.Id("1")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("TestCategory1", driver.FindElements(By.CssSelector(".col-xs-4.col-wl-5 .m-b-sm"))[5].Text);
            Assert.AreEqual("TestCategory2", driver.FindElements(By.CssSelector(".col-xs-4.col-wl-5 .m-b-sm"))[6].Text);
            driver.FindElement(By.CssSelector("[data-e2e=\"RecommendationCategoryDelete-id-1\"]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("TestCategory2", driver.FindElements(By.CssSelector(".col-xs-4.col-wl-5 .m-b-sm"))[5].Text);

            GoToClient("/products/test-product79");
            Thread.Sleep(3000);
            var element7 = driver.FindElement(By.CssSelector(".h2"));
            IJavaScriptExecutor jse7 = (IJavaScriptExecutor)driver;
            jse7.ExecuteScript("arguments[0].scrollIntoView(true)", element7);

            //Похожие товары    
            Assert.AreEqual("TestProduct28", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[0].FindElement(By.CssSelector(".products-view-name-link")).Text);
            Assert.AreEqual("TestProduct30", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[2].FindElement(By.CssSelector(".products-view-name-link")).Text);
            //next 
            driver.FindElement(By.CssSelector(".carousel-nav-next.icon-right-open-after.cs-l-1-interactive")).Click();
            Thread.Sleep(3000);
            Assert.AreEqual("TestProduct29", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[1].FindElement(By.CssSelector(".products-view-name-link")).Text);
            Assert.AreEqual("TestProduct31", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[3].FindElement(By.CssSelector(".products-view-name-link")).Text);
            //pre
            driver.FindElement(By.CssSelector(".carousel-nav-prev.icon-left-open-after.cs-l-1-interactive")).Click();
            Thread.Sleep(3000);
            Assert.AreEqual("TestProduct28", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[0].FindElement(By.CssSelector(".products-view-name-link")).Text);
            Assert.AreEqual("TestProduct30", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[2].FindElement(By.CssSelector(".products-view-name-link")).Text);
        }
        [Test]
        public void AddAlternativeProperty()
        {
            GoToAdmin("category/edit/1");

            ScrollTo(By.Id("UrlPath"));
            ScrollTo(By.CssSelector("[data-e2e=\"categoryGroupAdd\"]"));

            driver.FindElement(By.CssSelector("[data-type=\"Alternative\"]")).FindElement(By.CssSelector("[data-e2e=\"RecommendationPropertyAdd\"]")).Click();
            Thread.Sleep(2000);

            (new SelectElement(driver.FindElement(By.CssSelector(".col-xs-6 select")))).SelectByText("Property5");
            Thread.Sleep(2000);

            (new SelectElement(driver.FindElements(By.CssSelector(".col-xs-6 select"))[1])).SelectByText("5");
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-type=\"Alternative\"]")).FindElement(By.CssSelector("[data-e2e=\"RecommendationPropertyAdd\"]")).Click();
            Thread.Sleep(2000);

            (new SelectElement(driver.FindElement(By.CssSelector(".col-xs-6 select")))).SelectByText("Property1");
            Thread.Sleep(2000);

            (new SelectElement(driver.FindElements(By.CssSelector(".col-xs-6 select"))[1])).SelectByText("1");
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            Thread.Sleep(2000);

            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"RecommendationProperty\"]"))[1].Text.Contains("Property5 - 5"));
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"RecommendationProperty\"]"))[1].Text.Contains("Property1 - 1"));

            driver.FindElement(By.CssSelector("[data-e2e=\"RecommendationPropertyWithValueDelete-value-id-1\"]")).Click();
            Thread.Sleep(2000);
            Assert.IsFalse(driver.FindElements(By.CssSelector("[data-e2e=\"RecommendationProperty\"]"))[1].Text.Contains("Property1 - 1"));
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"RecommendationProperty\"]"))[1].Text.Contains("Property5 - 5"));

            GoToClient("/products/test-product1");
            Thread.Sleep(3000);
            //Похожие товары
            Assert.IsFalse(driver.PageSource.Contains("Похожие товары"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".carousel-inner")).Count == 0);
        }
        [Test]
        public void AddAlternativeCategoryAndProperty()
        {
            GoToAdmin("category/edit/3");
            ScrollTo(By.Id("UrlPath"));
            ScrollTo(By.CssSelector("[data-e2e=\"categoryGroupAdd\"]"));

            driver.FindElement(By.CssSelector("[data-type=\"Alternative\"]")).FindElement(By.CssSelector("[data-e2e=\"RecommendationCategoryAdd\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.Id("2")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-type=\"Alternative\"]")).FindElement(By.CssSelector("[data-e2e=\"RecommendationPropertyAdd\"]")).Click();
            Thread.Sleep(2000);

            (new SelectElement(driver.FindElement(By.CssSelector(".col-xs-6 select")))).SelectByText("Property1");
            Thread.Sleep(2000);

            (new SelectElement(driver.FindElements(By.CssSelector(".col-xs-6 select"))[1])).SelectByText("1");
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            Thread.Sleep(2000);

            GoToClient("/products/test-product25");
            Thread.Sleep(3000);
            //Похожие товары
            Assert.AreEqual("Похожие товары", driver.FindElement(By.CssSelector(".h2")).Text);
            Assert.IsTrue(driver.PageSource.Contains("Похожие товары"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".products-view-item.text-static.cs-br-1.js-products-view-item")).Count == 2);
            Assert.AreEqual("TestProduct30", driver.FindElements(By.CssSelector(".products-view-item.text-static.cs-br-1.js-products-view-item"))[0].FindElement(By.CssSelector(".products-view-name-link")).Text);
            Assert.AreEqual("TestProduct31", driver.FindElements(By.CssSelector(".products-view-item.text-static.cs-br-1.js-products-view-item"))[1].FindElement(By.CssSelector(".products-view-name-link")).Text);
        }

        [Test]
        public void AddAlternativeRelatedCategoryAndProperty()
        {
            GoToAdmin("category/edit/6");
            ScrollTo(By.Id("UrlPath"));
            ScrollTo(By.CssSelector("[data-e2e=\"categoryGroupAdd\"]"));

            //с этим товаром покупают
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-type=\"Related\"]")).FindElement(By.CssSelector("[data-e2e=\"RecommendationCategoryAdd\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.Id("2")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            Thread.Sleep(2000);
            
            driver.FindElement(By.CssSelector("[data-type=\"Related\"]")).FindElement(By.CssSelector("[data-e2e=\"RecommendationPropertyAdd\"]")).Click();
            Thread.Sleep(2000);

            (new SelectElement(driver.FindElement(By.CssSelector(".col-xs-6 select")))).SelectByText("Property1");
            Thread.Sleep(2000);

            (new SelectElement(driver.FindElements(By.CssSelector(".col-xs-6 select"))[1])).SelectByText("1");
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            Thread.Sleep(2000);

            //Похожие товары
            driver.FindElement(By.CssSelector("[data-type=\"Alternative\"]")).FindElement(By.CssSelector("[data-e2e=\"RecommendationCategoryAdd\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.Id("2")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-type=\"Alternative\"]")).FindElement(By.CssSelector("[data-e2e=\"RecommendationPropertyAdd\"]")).Click();
            Thread.Sleep(2000);

            (new SelectElement(driver.FindElement(By.CssSelector(".col-xs-6 select")))).SelectByText("Property1");
            Thread.Sleep(2000);

            (new SelectElement(driver.FindElements(By.CssSelector(".col-xs-6 select"))[1])).SelectByText("2");
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            Thread.Sleep(2000);
            GoToClient("/products/test-product26");
            Thread.Sleep(3000);

            //Похожие товары
            Assert.AreEqual("С этим товаром покупают", driver.FindElement(By.CssSelector(".h2")).Text);
            Assert.IsTrue(driver.PageSource.Contains("Похожие товары"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".products-view-item.text-static.cs-br-1.js-products-view-item")).Count == 4);
            Assert.AreEqual("TestProduct30", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[0].FindElement(By.CssSelector(".products-view-name-link")).Text);
            Assert.AreEqual("TestProduct31", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[1].FindElement(By.CssSelector(".products-view-name-link")).Text);
            Assert.AreEqual("Похожие товары", driver.FindElements(By.CssSelector(".h2"))[1].Text);
            Assert.AreEqual("TestProduct28", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[2].FindElement(By.CssSelector(".products-view-name-link")).Text);
            Assert.AreEqual("TestProduct29", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[3].FindElement(By.CssSelector(".products-view-name-link")).Text);

        }
    }
}
