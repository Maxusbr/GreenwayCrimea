using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;


namespace AdvantShop.SeleniumTest.Admin.Catalog.ExportCategories
{
    [TestFixture]
    public class ExportCategopieTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(
            "data\\Admin\\Catalog\\Export\\ExportCategories\\Catalog.Product.csv",
             "data\\Admin\\Catalog\\Export\\ExportCategories\\Catalog.Photo.csv",
           "data\\Admin\\Catalog\\Export\\ExportCategories\\Catalog.Offer.csv",
           "data\\Admin\\Catalog\\Export\\ExportCategories\\Catalog.Category.csv",
           "data\\Admin\\Catalog\\Export\\ExportCategories\\Catalog.ProductCategories.csv",
           "data\\Admin\\Catalog\\Export\\ExportCategories\\Catalog.Brand.csv",
           "Data\\Admin\\Catalog\\Export\\ExportCategories\\Catalog.Color.csv",
           "data\\Admin\\Catalog\\Export\\ExportCategories\\Catalog.Size.csv"
           );

            Init();
        }

        [Test]
        public void ExportProductsChoiceCateroiesFields()
        {
            GoToAdmin("exportcategories");

            (new SelectElement(driver.FindElement(By.Name("CsvSeparator")))).SelectByText("Точка с запятой");
            (new SelectElement(driver.FindElement(By.Name("CsvEncoding")))).SelectByText("Utf8");
            Thread.Sleep(500);

            driver.FindElement(By.CssSelector("[data-e2e=\"ExportSetNone\"]")).Click();
            Thread.Sleep(500);
           
            (new SelectElement(driver.FindElement(By.Name("0ddl")))).SelectByText("Id");
            (new SelectElement(driver.FindElement(By.Name("1ddl")))).SelectByText("Название");
            (new SelectElement(driver.FindElement(By.Name("2ddl")))).SelectByText("Урл синоним");
            (new SelectElement(driver.FindElement(By.Name("3ddl")))).SelectByText("Id род. категории");
            (new SelectElement(driver.FindElement(By.Name("4ddl")))).SelectByText("Сортировка");
            (new SelectElement(driver.FindElement(By.Name("5ddl")))).SelectByText("Активность");
            Thread.Sleep(1000);


            driver.FindElement(By.CssSelector("[data-e2e=\"ButtonSave\"]")).Click();
            Thread.Sleep(2000);
           // Assert.AreEqual("11 / 11", driver.FindElement(By.CssSelector(".progress-bar span")).Text);
            GoToClient("content/price_temp/export_categories.csv?OpenInBrowser=true");
            Thread.Sleep(2000);

            Assert.IsTrue(driver.Url.Contains("csv"));
            Assert.IsTrue(driver.PageSource.Contains("categoryid;name;slug;parentcategory;sortorder;enabled\r\n0;Каталог;catalog;0;0;+\r\n1;TestCategory1;test-category1;0;1;+\r\n2;TestCategory2;test-category2;0;2;+\r\n3;TestCategory3;test-category3;0;3;+\r\n4;TestCategory4;test-category4;0;4;+\r\n5;TestCategory5;test-category5;0;5;+\r\n6;TestCategory6;test-category6;1;6;+\r\n7;TestCategory7;test-category7;1;7;+\r\n8;TestCategory8;test-category8;2;8;+\r\n9;TestCategory9;test-category9;2;9;-\r\n10;TestCategory10;test-category10;5;10;+"));
        }
        [Test]
        public void ExportProductsNullCateroiesFields()
        {
            GoToAdmin("exportcategories");

            (new SelectElement(driver.FindElement(By.Name("CsvSeparator")))).SelectByText("Точка с запятой");
            (new SelectElement(driver.FindElement(By.Name("CsvEncoding")))).SelectByText("Utf8");

            driver.FindElement(By.CssSelector("[data-e2e=\"ExportSetNone\"]")).Click();
                       
            driver.FindElement(By.CssSelector("[data-e2e=\"ButtonSave\"]")).Click();
            Thread.Sleep(2000);
            Assert.IsFalse(driver.Url.Contains("exportcategories/export"));
            Assert.IsTrue(driver.Url.Contains("exportcategories"));
       
        }

        [Test]
        public void ExportProductsAllFieldsAll()
        {
            GoToAdmin("exportcategories");
            (new SelectElement(driver.FindElement(By.Name("CsvSeparator")))).SelectByText("Точка с запятой");
            (new SelectElement(driver.FindElement(By.Name("CsvEncoding")))).SelectByText("Utf8");
            Thread.Sleep(500);
            driver.FindElement(By.CssSelector("[data-e2e=\"ExportSetDefault\"]")).Click();
            Thread.Sleep(500);

            IWebElement selectElem = driver.FindElement(By.Name("21ddl"));
            SelectElement select = new SelectElement(selectElem);
            Assert.IsTrue(select.AllSelectedOptions[0].Text.Contains("Группы свойств"));
                      
            
            driver.FindElement(By.CssSelector("[data-e2e=\"ButtonSave\"]")).Click();
            Thread.Sleep(2000);
          //  Assert.AreEqual("11 / 11", driver.FindElement(By.CssSelector(".progress-bar span")).Text);
            GoToClient("content/price_temp/export_categories.csv?OpenInBrowser=true");
            Thread.Sleep(2000);

            Assert.IsTrue(driver.Url.Contains("csv"));
            Assert.IsTrue(driver.PageSource.Contains("categoryid;name;slug;parentcategory;sortorder;enabled;hidden;briefdescription;description;displaystyle;sorting;displaybrandsinmenu;displaysubcategoriesinmenu;tags;picture;minipicture;icon;title;metakeywords;metadescription;h1;propertygroups\r\n0;Каталог;catalog;0;0;+;-;;;Tile;NoSorting;+;-;;;;;;;;;\r\n1;TestCategory1;test-category1;0;1;+;-;;;Tile;NoSorting;-;-;;;;;;;;;\r\n2;TestCategory2;test-category2;0;2;+;-;;;Tile;NoSorting;-;-;;;;;;;;;\r\n3;TestCategory3;test-category3;0;3;+;-;;;Tile;NoSorting;-;-;;;;;;;;;\r\n4;TestCategory4;test-category4;0;4;+;-;;;Tile;NoSorting;-;-;;;;;;;;;\r\n5;TestCategory5;test-category5;0;5;+;-;;;Tile;NoSorting;-;-;;;;;;;;;\r\n6;TestCategory6;test-category6;1;6;+;-;;;Tile;NoSorting;-;-;;;;;;;;;\r\n7;TestCategory7;test-category7;1;7;+;-;;;Tile;NoSorting;-;-;;;;;;;;;\r\n8;TestCategory8;test-category8;2;8;+;-;;;Tile;NoSorting;-;-;;;;;;;;;\r\n9;TestCategory9;test-category9;2;9;-;-;;;Tile;NoSorting;-;-;;;;;;;;;\r\n10;TestCategory10;test-category10;5;10;+;-;;;Tile;NoSorting;-;-;;;;;;;;;"));
        }
        [Test]
        public void ExportProductsChoiceCateroiesFieldsTrimСomma()
        {
            GoToAdmin("exportcategories");

            (new SelectElement(driver.FindElement(By.Name("CsvSeparator")))).SelectByText("Запятая");
            (new SelectElement(driver.FindElement(By.Name("CsvEncoding")))).SelectByText("Utf8");
            Thread.Sleep(500);

            driver.FindElement(By.CssSelector("[data-e2e=\"ExportSetNone\"]")).Click();
            Thread.Sleep(500);

            (new SelectElement(driver.FindElement(By.Name("0ddl")))).SelectByText("Id");
            (new SelectElement(driver.FindElement(By.Name("1ddl")))).SelectByText("Название");
            Thread.Sleep(500);


            driver.FindElement(By.CssSelector("[data-e2e=\"ButtonSave\"]")).Click();
            Thread.Sleep(3000);
           // Assert.AreEqual("11 / 11", driver.FindElement(By.CssSelector(".progress-bar span")).Text);
            GoToClient("content/price_temp/export_categories.csv?OpenInBrowser=true");
            Thread.Sleep(2000);

            Assert.IsTrue(driver.Url.Contains("csv"));
            Assert.IsTrue(driver.PageSource.Contains("categoryid,name\r\n0,Каталог\r\n1,TestCategory1\r\n2,TestCategory2\r\n3,TestCategory3\r\n4,TestCategory4\r\n5,TestCategory5\r\n6,TestCategory6\r\n7,TestCategory7\r\n8,TestCategory8\r\n9,TestCategory9\r\n10,TestCategory10"));
        }
        [Test]
        public void ExportProductsChoiceCateroiesFieldsTrimTabu()
        {
            GoToAdmin("exportcategories");

            (new SelectElement(driver.FindElement(By.Name("CsvSeparator")))).SelectByText("Символ табуляции");
            (new SelectElement(driver.FindElement(By.Name("CsvEncoding")))).SelectByText("Utf8");

            driver.FindElement(By.CssSelector("[data-e2e=\"ExportSetNone\"]")).Click();

            (new SelectElement(driver.FindElement(By.Name("0ddl")))).SelectByText("Id");
            (new SelectElement(driver.FindElement(By.Name("1ddl")))).SelectByText("Название");

            driver.FindElement(By.CssSelector("[data-e2e=\"ButtonSave\"]")).Click();
            Thread.Sleep(2000);
           // Assert.AreEqual("11 / 11", driver.FindElement(By.CssSelector(".progress-bar span")).Text);

          //  ReInit(false);

            GoToClient("content/price_temp/export_categories.csv?OpenInBrowser=true");
            Thread.Sleep(2000);

            Assert.IsTrue(driver.Url.Contains("csv"));
            Assert.AreEqual("categoryid name\r\n0 Каталог\r\n1 TestCategory1\r\n2 TestCategory2\r\n3 TestCategory3\r\n4 TestCategory4\r\n5 TestCategory5\r\n6 TestCategory6\r\n7 TestCategory7\r\n8 TestCategory8\r\n9 TestCategory9\r\n10 TestCategory10", driver.FindElement(By.TagName("pre")).Text);
        }
    }
}
