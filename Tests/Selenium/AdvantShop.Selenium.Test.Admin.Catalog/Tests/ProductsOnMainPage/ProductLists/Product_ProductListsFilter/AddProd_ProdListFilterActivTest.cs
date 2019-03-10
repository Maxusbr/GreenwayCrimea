using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.ProductLists
{
    [TestFixture]
    public class Prod_ProdListAddFilterActivTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(
      "data\\Admin\\Catalog\\ProductsOnMainPage\\ProductLists\\AddProd_ProdLists\\FilterTest\\Catalog.Product.csv",
           "data\\Admin\\Catalog\\ProductsOnMainPage\\ProductLists\\AddProd_ProdLists\\FilterTest\\Catalog.Offer.csv",
           "data\\Admin\\Catalog\\ProductsOnMainPage\\ProductLists\\AddProd_ProdLists\\FilterTest\\Catalog.Category.csv",
           "data\\Admin\\Catalog\\ProductsOnMainPage\\ProductLists\\AddProd_ProdLists\\FilterTest\\Catalog.ProductCategories.csv",
           "data\\Admin\\Catalog\\ProductsOnMainPage\\ProductLists\\AddProd_ProdLists\\FilterTest\\Catalog.ProductList.csv",
           "data\\Admin\\Catalog\\ProductsOnMainPage\\ProductLists\\AddProd_ProdLists\\FilterTest\\Catalog.Product_ProductList.csv"
           );
             
            Init();
        }
        
        [Test]
        public void AddProduct_ProductListsFilterActivityOn()
        {
            Functions.AddProduct_ProductListsFilterSelect(driver, baseURL, filter: "Enabled", select: "Активные");
            Assert.AreEqual("Найдено записей: 248", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);
            Assert.AreEqual("TestProduct1", GetGridCell(0, "Name", "ProductsSelectvizr").Text);
            Assert.AreEqual("TestProduct107", GetGridCell(9, "Name", "ProductsSelectvizr").Text);

            //close
            Functions.GridFilterClose(driver, baseURL, name: "Enabled");
            Assert.AreEqual("TestProduct1", GetGridCell(0, "Name", "ProductsSelectvizr").Text);
            Assert.AreEqual("TestProduct107", GetGridCell(9, "Name", "ProductsSelectvizr").Text);
        }

        [Test]
        public void AddProduct_ProductListsFilterActivityOff()
        {
            Functions.AddProduct_ProductListsFilterSelect(driver, baseURL, filter: "Enabled", select: "Неактивные");
            Assert.AreEqual("Найдено записей: 2", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);
            Assert.AreEqual("TestProduct2", GetGridCell(0, "Name", "ProductsSelectvizr").Text);
            Assert.AreEqual("TestProduct3", GetGridCell(1, "Name", "ProductsSelectvizr").Text);

            //close
            Functions.GridFilterClose(driver, baseURL, name: "Enabled");
            Assert.AreEqual("TestProduct1", GetGridCell(0, "Name", "ProductsSelectvizr").Text);
            Assert.AreEqual("TestProduct10", GetGridCell(1, "Name", "ProductsSelectvizr").Text);
        }
    }
}