using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;

namespace AdvantShop.SeleniumTest.Admin.Export.ExportProducts
{
    [TestFixture]
    public class ExportProductsTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(
            "data\\Admin\\Catalog\\Export\\Catalog.Product.csv",
             "data\\Admin\\Catalog\\Export\\Catalog.Photo.csv",
           "data\\Admin\\Catalog\\Export\\Catalog.Offer.csv",
           "data\\Admin\\Catalog\\Export\\Catalog.Category.csv",
           "data\\Admin\\Catalog\\Export\\Catalog.ProductCategories.csv",
           "data\\Admin\\Catalog\\Export\\Catalog.Brand.csv",
             "data\\Admin\\Catalog\\Export\\Catalog.Property.csv",
                 "data\\Admin\\Catalog\\Export\\Catalog.PropertyValue.csv",
                 "data\\Admin\\Catalog\\Export\\Catalog.ProductPropertyValue.csv",
                "data\\Admin\\Catalog\\Export\\Catalog.PropertyGroup.csv",
                "Data\\Admin\\Catalog\\Export\\Catalog.Color.csv",
                "data\\Admin\\Catalog\\Export\\Catalog.Size.csv"
           );

            Init();
        }

        [Test]
        public void ExportProductsChoiceCateroiesFields()
        {
            GoToAdmin("exportfeeds");

            driver.FindElement(By.XPath("//a[contains(text(), 'Выбор товаров')]")).Click();

            if (!driver.FindElement(By.CssSelector("[data-e2e=\"ExportChoiceCategories\"] input")).Selected)
            { 
                driver.FindElement(By.CssSelector("[data-e2e=\"ExportChoiceCategories\"] span")).Click();
            }
            if (!driver.FindElement(By.Id("1")).GetAttribute("aria-selected").Equals("true"))
            { 
                driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory1')]")).Click();
            }

            driver.FindElement(By.XPath("//a[contains(text(), 'Поля выгрузки')]")).Click();
            WaitForElem(By.Name("0ddl"));
            driver.FindElement(By.CssSelector("[data-e2e=\"exportSetNone\"]")).Click();
            (new SelectElement(driver.FindElement(By.Name("0ddl")))).SelectByText("Артикул");
            (new SelectElement(driver.FindElement(By.Name("1ddl")))).SelectByText("Наименование");
            (new SelectElement(driver.FindElement(By.Name("2ddl")))).SelectByText("Урл синоним");
            (new SelectElement(driver.FindElement(By.Name("3ddl")))).SelectByText("Категории");
            (new SelectElement(driver.FindElement(By.Name("4ddl")))).SelectByText("Активность");
            (new SelectElement(driver.FindElement(By.Name("5ddl")))).SelectByText("Цена");
            (new SelectElement(driver.FindElement(By.Name("6ddl")))).SelectByText("Количество");
            (new SelectElement(driver.FindElement(By.Name("7ddl")))).SelectByText("Размеры");
            (new SelectElement(driver.FindElement(By.Name("8ddl")))).SelectByText("Производитель");
           // (new SelectElement(driver.FindElement(By.Name("9ddl")))).SelectByText("Изображения");
            (new SelectElement(driver.FindElement(By.Name("10ddl")))).SelectByText("Скидка (%, процент)");
            (new SelectElement(driver.FindElement(By.Name("11ddl")))).SelectByText("Свойства");
            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.XPath("//a[contains(text(), 'Параметры выгрузки')]")).Click();
            WaitForElem(By.Id("Name"));

            if (!driver.FindElement(By.Id("ExportFeedSettings_Active")).Selected)

            {
                Assert.IsFalse(driver.FindElement(By.Id("ExportFeedSettings_Interval")).Enabled);
                Assert.IsFalse(driver.FindElement(By.Id("ddlIntervalType")).Enabled);

                driver.FindElement(By.CssSelector("[data-e2e=\"exportCommonSettingsActive\"]")).Click();
                Assert.IsTrue(driver.FindElement(By.Id("ExportFeedSettings_Interval")).Enabled);
                Assert.IsTrue(driver.FindElement(By.Id("ddlIntervalType")).Enabled);
            }
            
            (new SelectElement(driver.FindElement(By.Name("ddlFileExtention")))).SelectByText("csv");
            (new SelectElement(driver.FindElement(By.Id("CsvSeparator")))).SelectByText("Точка с запятой");
            (new SelectElement(driver.FindElement(By.Id("CsvEnconing")))).SelectByText("UTF-8");
            driver.FindElement(By.Id("CsvColumSeparator")).Click();
            driver.FindElement(By.Id("CsvColumSeparator")).Clear();
            driver.FindElement(By.Id("CsvColumSeparator")).SendKeys(";");
            driver.FindElement(By.Id("CsvPropertySeparator")).Click();
            driver.FindElement(By.Id("CsvPropertySeparator")).Clear();
            driver.FindElement(By.Id("CsvPropertySeparator")).SendKeys(":");
           var priceMargin = driver.FindElement(By.Id("ExportFeedSettings_PriceMargin")).GetAttribute("value");
           if (!priceMargin.Equals("0"))
            {
                driver.FindElement(By.Id("ExportFeedSettings_PriceMargin")).Click();
                driver.FindElement(By.Id("ExportFeedSettings_PriceMargin")).Clear();
                driver.FindElement(By.Id("ExportFeedSettings_PriceMargin")).SendKeys("0");
            }

            DropFocus("h1");
            Functions.ExportProductsNoInCategoryOff(driver, baseURL);
            Functions.ExportProductsCategorySortOff(driver, baseURL);
           

            driver.FindElement(By.CssSelector("[data-e2e=\"ButtonSave\"]")).Click();
            Thread.Sleep(5000);
            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"Export\"]")).Click();
            //WaitForElem(By.CssSelector("[data-e2e=\"ExportCountValue\"]")); 
            Thread.Sleep(1000);
            GoToAdmin("exportfeeds/export/2");
            Thread.Sleep(1000);
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"ExportCountValue\"]")).Text.Contains("10"));
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"ExportCountTotal\"]")).Text.Contains("10"));
            GoToClient("catalog.csv?OpenInBrowser=true");
            WaitForElem(By.TagName("pre"));

            Assert.IsTrue(driver.Url.Contains("csv"));
            //  Assert.IsTrue(driver.PageSource.Contains("sku;name;paramsynonym;category;enabled;price;amount;size;producer;photos;discount;properties\r\n1;TestProduct1;test-product1;[TestCategory1];+;1,00;1;1x1x1;BrandName1;http://my-shop.ru/pictures/product/big/281_big.jpg;1,00;Property1:PropertyValue1\r\n2;TestProduct2;test-product2;[TestCategory1];-;2,00;2;2x2x2;BrandName2;http://my-shop.ru/pictures/product/big/284_big.jpg;2,00;Property2:PropertyValue2\r\n3;TestProduct3;test-product3;[TestCategory1];-;3,00;3;3x3x3;BrandName3;http://my-shop.ru/pictures/product/big/342_big.jpg;3,00;Property3:PropertyValue3\r\n4;TestProduct4;test-product4;[TestCategory1];+;4,00;4;4x4x4;BrandName4;http://my-shop.ru/pictures/product/big/348_big.jpg;4,00;Property4:PropertyValue4\r\n5;TestProduct5;test-product5;[TestCategory1];+;5,00;5;0x0x0;BrandName5;http://my-shop.ru/pictures/product/big/349_big.jpg;5,00;Property5:PropertyValue5\r\n6;TestProduct6;test-product6;[TestCategory1];+;6,00;6;6x6x6;BrandName6;http://my-shop.ru/pictures/product/big/350_big.jpg;6,00;Property1:PropertyValue6\r\n7;TestProduct7;test-product7;[TestCategory1];+;7,00;7;7x7x7;BrandName7;http://my-shop.ru/pictures/product/big/351_big.jpg;7,00;Property2:PropertyValue7\r\n8;TestProduct8;test-product8;[TestCategory1];+;8,00;8;8x8x8;BrandName8;http://my-shop.ru/pictures/product/big/353_big.jpg;8,00;Property3:PropertyValue8\r\n9;TestProduct9;test-product9;[TestCategory1];+;9,00;9;9x9x9;BrandName9;http://my-shop.ru/pictures/product/big/358_big.jpg;9,00;Property4:PropertyValue9\r\n10;TestProduct10;test-product10;[TestCategory1];+;10,00;10;10x10x10;BrandName10;http://my-shop.ru/pictures/product/big/360_big.jpg;10,00;\"Property1:PropertyValue1;Property5:PropertyValue10\""));
            Assert.IsTrue(driver.PageSource.Contains("sku;name;paramsynonym;category;enabled;price;amount;size;producer;discount;properties\r\n1;TestProduct1;test-product1;[TestCategory1];+;1,00;1;1x1x1;BrandName1;1,00;Property1:PropertyValue1\r\n2;TestProduct2;test-product2;[TestCategory1];-;2,00;2;2x2x2;BrandName2;2,00;Property2:PropertyValue2\r\n3;TestProduct3;test-product3;[TestCategory1];-;3,00;3;3x3x3;BrandName3;3,00;Property3:PropertyValue3\r\n4;TestProduct4;test-product4;[TestCategory1];+;4,00;4;4x4x4;BrandName4;4,00;Property4:PropertyValue4\r\n5;TestProduct5;test-product5;[TestCategory1];+;5,00;5;0x0x0;BrandName5;5,00;Property5:PropertyValue5\r\n6;TestProduct6;test-product6;[TestCategory1];+;6,00;6;6x6x6;BrandName6;6,00;Property1:PropertyValue6\r\n7;TestProduct7;test-product7;[TestCategory1];+;7,00;7;7x7x7;BrandName7;7,00;Property2:PropertyValue7\r\n8;TestProduct8;test-product8;[TestCategory1];+;8,00;8;8x8x8;BrandName8;8,00;Property3:PropertyValue8\r\n9;TestProduct9;test-product9;[TestCategory1];+;9,00;9;9x9x9;BrandName9;9,00;Property4:PropertyValue9\r\n10;TestProduct10;test-product10;[TestCategory1];+;10,00;10;10x10x10;BrandName10;10,00;\"Property1:PropertyValue1;Property5:PropertyValue10\""));
        }


        [Test]
        public void ExportProductzAllFieldsAll()
        {
            GoToAdmin("exportfeeds");

            driver.FindElement(By.XPath("//a[contains(text(), 'Выбор товаров')]")).Click();
            WaitForElem(By.CssSelector("[data-e2e=\"ExportAllProducts\"]"));

            if (!driver.FindElement(By.CssSelector("[data-e2e=\"ExportAllProducts\"] input")).Selected)
            { 
                driver.FindElement(By.CssSelector("[data-e2e=\"ExportAllProducts\"] span")).Click();
            }

            driver.FindElement(By.XPath("//a[contains(text(), 'Поля выгрузки')]")).Click();
            WaitForElem(By.Name("0ddl"));
            driver.FindElement(By.CssSelector("[data-e2e=\"exportSetNone\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"exportSetDefault\"]")).Click();

            driver.FindElement(By.XPath("//a[contains(text(), 'Параметры выгрузки')]")).Click();
            WaitForElem(By.Id("Name"));

            if (driver.FindElement(By.Id("ExportFeedSettings_Active")).Selected)

            {
                driver.FindElement(By.CssSelector("[data-e2e=\"exportCommonSettingsActive\"]")).Click();
            }

            (new SelectElement(driver.FindElement(By.Id("CsvSeparator")))).SelectByText("Точка с запятой");
            (new SelectElement(driver.FindElement(By.Id("CsvEnconing")))).SelectByText("UTF-8");
            (new SelectElement(driver.FindElement(By.Name("ddlFileExtention")))).SelectByText("txt");
            driver.FindElement(By.Id("ExportFeedSettings_FileName")).Click();
            driver.FindElement(By.Id("ExportFeedSettings_FileName")).Clear();
            driver.FindElement(By.Id("ExportFeedSettings_FileName")).SendKeys("ChangeName");
            driver.FindElement(By.Id("CsvColumSeparator")).Click();
            driver.FindElement(By.Id("CsvColumSeparator")).Clear();
            driver.FindElement(By.Id("CsvColumSeparator")).SendKeys(";");
            driver.FindElement(By.Id("CsvPropertySeparator")).Click();
            driver.FindElement(By.Id("CsvPropertySeparator")).Clear();
            driver.FindElement(By.Id("CsvPropertySeparator")).SendKeys(":");
            var priceMargin = driver.FindElement(By.Id("ExportFeedSettings_PriceMargin")).GetAttribute("value");
            if (!priceMargin.Equals("0"))
            { 
                driver.FindElement(By.Id("ExportFeedSettings_PriceMargin")).Click();
                driver.FindElement(By.Id("ExportFeedSettings_PriceMargin")).Clear();
                
                driver.FindElement(By.Id("ExportFeedSettings_PriceMargin")).SendKeys("0");
            }
            DropFocus("h1");
            Functions.ExportProductsNoInCategoryOff(driver, baseURL);
            Functions.ExportProductsCategorySortOff(driver, baseURL);
           

            driver.FindElement(By.CssSelector("[data-e2e=\"ButtonSave\"]")).Click();
            Thread.Sleep(5000);
            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"Export\"]")).Click();
            Thread.Sleep(1000);
            GoToAdmin("exportfeeds/export/2");
            Thread.Sleep(1000);
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"ExportCountValue\"]")).Text.Contains("20"));
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"ExportCountTotal\"]")).Text.Contains("20"));
            GoToClient("ChangeName.txt?OpenInBrowser=true");
            WaitForElem(By.TagName("pre"));
           
            Assert.IsTrue(driver.Url.Contains("txt"));
            Assert.IsTrue(driver.Url.Contains("ChangeName"));
            Assert.IsTrue(driver.PageSource.Contains("sku;name;paramsynonym;category;enabled;currency;price;purchaseprice;amount;sku:size:color:price:purchaseprice:amount;unit;discount;discountamount;shippingprice;weight;size;briefdescription;description;title;metakeywords;metadescription;h1;photos;videos;markers;properties;producer;preorder;salesnote;related sku;alternative sku;custom options;gtin;googleproductcategory;yandexproductcategory;yandextypeprefix;yandexmodel;adult;manufacturer_warranty;tags;gifts;minamount;maxamount;multiplicity;cbid;fee;barcode;tax\r\n1;TestProduct1;test-product1;[TestCategory1];+;RUB;1,00;1,00;1;;unit;1,00;;1,00;1,00;1x1x1;briefDesc1;Desc1;;;;;"));
            Assert.IsTrue(driver.PageSource.Contains(";s;\"Property1:PropertyValue1;Property5:PropertyValue10\";BrandName10;-;sales note 10;;;;gtin10;Google Product Category10;yandex market category10;yandex type prefix 10;yandex model 10;-;-;;;10,00;110,00;1,;10;10;10;Без НДС\r\n11;TestProduct11;test-product11;[TestCategory2];+;RUB;11,00;11,00;11;;unit;;1,00;11,00;11,00;11x11x11;briefDesc11;Desc11;;;;;;;s;Property1:PropertyValue11;BrandName1;-;sales note 11;;;;gtin11;Google Product Category11;yandex market category11;yandex type prefix 11;yandex model 11;-;-;;;11,00;111,00;1,;11;11;11;Без НДС\r\n12;TestProduct12;test-product12;[TestCategory2];+;RUB;12,00;12,00;12;;unit;;2,00;12,00;12,00;12x12x12;briefDesc12;Desc12;;;;;;;s;Property2:PropertyValue12;BrandName2;-;sales note 12;;;;gtin12;Google Product Category12;yandex market category12;yandex type prefix 12;yandex model 12;-;-;;;12,00;112,00;1,;12;12;12;Без НДС\r\n13;TestProduct13;test-product13;[TestCategory2];+;RUB;13,00;13,00;13;;;;3,00;13,00;13,00;13x13x13;briefDesc13;Desc13;;;;;;;;Property3:PropertyValue13;BrandName3;+;sales note 13;;;;gtin13;Google Product Category13;yandex market category13;yandex type prefix 13;yandex model 13;-;-;;;13,00;113,00;1,;13;13;13;Без НДС\r\n14;TestProduct14;test-product14;[TestCategory2];+;RUB;14,00;14,00;14;;unit;;4,00;14,00;14,00;14x14x14;briefDesc14;Desc14;;;;;;;;Property4:PropertyValue14;BrandName4;+;sales note 14;;;;gtin14;Google Product Category14;yandex market category14;yandex type prefix 14;yandex model 14;-;-;;;14,00;114,00;1,;14;14;14;Без НДС\r\n15;TestProduct15;test-product15;[TestCategory2];+;RUB;15,00;15,00;15;;unit;;5,00;15,00;15,00;15x15x15;briefDesc15;Desc15;;;;;;;;Property5:PropertyValue15;BrandName5;+;sales note 15;;;;gtin15;Google Product Category15;yandex market category15;yandex type prefix 15;yandex model 15;-;-;;;15,00;115,00;1,;15;15;15;Без НДС\r\n16;TestProduct16;test-product16;[TestCategory2];+;RUB;16,00;16,00;16;;unit;;6,00;16,00;16,00;16x16x16;briefDesc16;Desc16;;;;;;;;Property1:PropertyValue16;BrandName6;-;sales note 16;;;;gtin16;Google Product Category16;yandex market category16;yandex type prefix 16;yandex model 16;-;-;;;16,00;116,00;1,;16;16;16;Без НДС\r\n17;TestProduct17;test-product17;[TestCategory2];+;RUB;17,00;17,00;0;;unit;;7,00;17,00;17,00;17x17x17;briefDesc17;Desc17;;;;;;;;Property2:PropertyValue17;BrandName7;-;sales note 17;;;;gtin17;Google Product Category17;yandex market category17;yandex type prefix 17;yandex model 17;-;-;;;17,00;117,00;1,;17;17;17;Без НДС\r\n18;TestProduct18;test-product18;[TestCategory2];+;RUB;18,00;18,00;18;;unit;;8,00;18,00;18,00;18x18x18;briefDesc18;Desc18;;;;;;;;Property3:PropertyValue18;BrandName8;-;sales note 18;;;;gtin18;Google Product Category18;yandex market category18;yandex type prefix 18;yandex model 18;-;-;;;18,00;118,00;1,;18;18;18;Без НДС\r\n19;TestProduct19;test-product19;[TestCategory2];+;RUB;19,00;19,00;19;;unit;;9,00;19,00;19,00;19x19x19;briefDesc19;Desc19;;;;;;;;Property4:PropertyValue19;BrandName9;-;sales note 19;;;;gtin19;Google Product Category19;yandex market category19;yandex type prefix 19;yandex model 19;-;-;;;19,00;119,00;1,;19;19;19;Без НДС\r\n20;TestProduct20;test-product20;[TestCategory2];+;RUB;20,00;20,00;20;;unit;;10,00;20,00;20,00;20x20x20;briefDesc20;Desc20;;;;;;;;Property5:PropertyValue20;BrandName10;-;sales note 20;;;;gtin20;Google Product Category20;yandex market category20;yandex type prefix 20;yandex model 20;-;-;;;20,00;120,00;1,;20;20;20;Без НДС"));
            Assert.IsTrue(driver.PageSource.Contains(".jpg"));
            //all
            Assert.IsTrue(driver.PageSource.Contains("sku;name;paramsynonym;category;enabled;currency;price;purchaseprice;amount;sku:size:color:price:purchaseprice:amount;unit;discount;discountamount;shippingprice;weight;size;briefdescription;description;title;metakeywords;metadescription;h1;photos;videos;markers;properties;producer;preorder;salesnote;related sku;alternative sku;custom options;gtin;googleproductcategory;yandexproductcategory;yandextypeprefix;yandexmodel;adult;manufacturer_warranty;tags;gifts;minamount;maxamount;multiplicity;cbid;fee;barcode;tax\r\n1;TestProduct1;test-product1;[TestCategory1];+;RUB;1,00;1,00;1;;unit;1,00;;1,00;1,00;1x1x1;briefDesc1;Desc1;;;;;281.jpg;;r;Property1:PropertyValue1;BrandName1;-;sales note 1;;;;gtin1;Google Product Category1;yandex market category1;yandex type prefix 1;yandex model 1;-;-;;;1,00;101,00;1,;1;1;1;Без НДС\r\n2;TestProduct2;test-product2;[TestCategory1];-;RUB;2,00;2,00;2;;unit;2,00;;2,00;2,00;2x2x2;briefDesc2;Desc2;;;;;284.jpg;;r;Property2:PropertyValue2;BrandName2;-;sales note 2;;;;gtin2;Google Product Category2;yandex market category2;yandex type prefix 2;yandex model 2;-;-;;;2,00;102,00;1,;2;2;2;Без НДС\r\n3;TestProduct3;test-product3;[TestCategory1];-;RUB;3,00;3,00;3;;unit;3,00;;3,00;3,00;3x3x3;briefDesc3;Desc3;;;;;342.jpg;;r;Property3:PropertyValue3;BrandName3;-;sales note 3;;;;gtin3;Google Product Category3;yandex market category3;yandex type prefix 3;yandex model 3;-;-;;;3,00;103,00;1,;3;3;3;Без НДС\r\n4;TestProduct4;test-product4;[TestCategory1];+;RUB;4,00;4,00;4;;unit;4,00;;4,00;0,00;4x4x4;briefDesc4;Desc4;;;;;348.jpg;;n;Property4:PropertyValue4;BrandName4;-;sales note 4;;;;gtin4;Google Product Category4;yandex market category4;yandex type prefix 4;yandex model 4;-;-;;;4,00;104,00;1,;4;4;4;Без НДС\r\n5;TestProduct5;test-product5;[TestCategory1];+;RUB;5,00;5,00;5;;unit;5,00;;5,00;5,00;0x0x0;briefDesc5;Desc5;;;;;349.jpg;;n;Property5:PropertyValue5;BrandName5;-;sales note 5;;;;gtin5;Google Product Category5;yandex market category5;yandex type prefix 5;yandex model 5;-;-;;;5,00;105,00;1,;5;5;5;Без НДС\r\n6;TestProduct6;test-product6;[TestCategory1];+;RUB;6,00;6,00;6;;unit;6,00;;6,00;6,00;6x6x6;briefDesc6;Desc6;;;;;350.jpg;;n;Property1:PropertyValue6;BrandName6;-;sales note 6;;;;gtin6;Google Product Category6;yandex market category6;yandex type prefix 6;yandex model 6;-;-;;;6,00;106,00;1,;6;6;6;Без НДС\r\n7;TestProduct7;test-product7;[TestCategory1];+;RUB;7,00;7,00;7;;unit;7,00;;0,00;7,00;7x7x7;briefDesc7;Desc7;;;;;351.jpg;;b;Property2:PropertyValue7;BrandName7;-;sales note 7;;;;gtin7;Google Product Category7;yandex market category7;yandex type prefix 7;yandex model 7;-;-;;;7,00;107,00;1,;7;7;7;Без НДС\r\n8;TestProduct8;test-product8;[TestCategory1];+;RUB;8,00;8,00;8;;unit;8,00;;8,00;8,00;8x8x8;briefDesc8;Desc8;;;;;353.jpg;;b;Property3:PropertyValue8;BrandName8;-;sales note 8;;;;gtin8;Google Product Category8;yandex market category8;yandex type prefix 8;yandex model 8;-;-;;;8,00;108,00;1,;8;8;8;Без НДС\r\n9;TestProduct9;test-product9;[TestCategory1];+;RUB;9,00;9,00;9;;unit;9,00;;9,00;9,00;9x9x9;briefDesc9;Desc9;;;;;358.jpg;;b;Property4:PropertyValue9;BrandName9;-;sales note 9;;;;gtin9;Google Product Category9;yandex market category9;yandex type prefix 9;yandex model 9;-;-;;;9,00;109,00;1,;9;9;9;Без НДС\r\n10;TestProduct10;test-product10;[TestCategory1];+;RUB;10,00;10,00;10;;unit;10,00;;10,00;10,00;10x10x10;briefDesc10;Desc10;;;;;360.jpg;;s;\"Property1:PropertyValue1;Property5:PropertyValue10\";BrandName10;-;sales note 10;;;;gtin10;Google Product Category10;yandex market category10;yandex type prefix 10;yandex model 10;-;-;;;10,00;110,00;1,;10;10;10;Без НДС\r\n11;TestProduct11;test-product11;[TestCategory2];+;RUB;11,00;11,00;11;;unit;;1,00;11,00;11,00;11x11x11;briefDesc11;Desc11;;;;;;;s;Property1:PropertyValue11;BrandName1;-;sales note 11;;;;gtin11;Google Product Category11;yandex market category11;yandex type prefix 11;yandex model 11;-;-;;;11,00;111,00;1,;11;11;11;Без НДС\r\n12;TestProduct12;test-product12;[TestCategory2];+;RUB;12,00;12,00;12;;unit;;2,00;12,00;12,00;12x12x12;briefDesc12;Desc12;;;;;;;s;Property2:PropertyValue12;BrandName2;-;sales note 12;;;;gtin12;Google Product Category12;yandex market category12;yandex type prefix 12;yandex model 12;-;-;;;12,00;112,00;1,;12;12;12;Без НДС\r\n13;TestProduct13;test-product13;[TestCategory2];+;RUB;13,00;13,00;13;;;;3,00;13,00;13,00;13x13x13;briefDesc13;Desc13;;;;;;;;Property3:PropertyValue13;BrandName3;+;sales note 13;;;;gtin13;Google Product Category13;yandex market category13;yandex type prefix 13;yandex model 13;-;-;;;13,00;113,00;1,;13;13;13;Без НДС\r\n14;TestProduct14;test-product14;[TestCategory2];+;RUB;14,00;14,00;14;;unit;;4,00;14,00;14,00;14x14x14;briefDesc14;Desc14;;;;;;;;Property4:PropertyValue14;BrandName4;+;sales note 14;;;;gtin14;Google Product Category14;yandex market category14;yandex type prefix 14;yandex model 14;-;-;;;14,00;114,00;1,;14;14;14;Без НДС\r\n15;TestProduct15;test-product15;[TestCategory2];+;RUB;15,00;15,00;15;;unit;;5,00;15,00;15,00;15x15x15;briefDesc15;Desc15;;;;;;;;Property5:PropertyValue15;BrandName5;+;sales note 15;;;;gtin15;Google Product Category15;yandex market category15;yandex type prefix 15;yandex model 15;-;-;;;15,00;115,00;1,;15;15;15;Без НДС\r\n16;TestProduct16;test-product16;[TestCategory2];+;RUB;16,00;16,00;16;;unit;;6,00;16,00;16,00;16x16x16;briefDesc16;Desc16;;;;;;;;Property1:PropertyValue16;BrandName6;-;sales note 16;;;;gtin16;Google Product Category16;yandex market category16;yandex type prefix 16;yandex model 16;-;-;;;16,00;116,00;1,;16;16;16;Без НДС\r\n17;TestProduct17;test-product17;[TestCategory2];+;RUB;17,00;17,00;0;;unit;;7,00;17,00;17,00;17x17x17;briefDesc17;Desc17;;;;;;;;Property2:PropertyValue17;BrandName7;-;sales note 17;;;;gtin17;Google Product Category17;yandex market category17;yandex type prefix 17;yandex model 17;-;-;;;17,00;117,00;1,;17;17;17;Без НДС\r\n18;TestProduct18;test-product18;[TestCategory2];+;RUB;18,00;18,00;18;;unit;;8,00;18,00;18,00;18x18x18;briefDesc18;Desc18;;;;;;;;Property3:PropertyValue18;BrandName8;-;sales note 18;;;;gtin18;Google Product Category18;yandex market category18;yandex type prefix 18;yandex model 18;-;-;;;18,00;118,00;1,;18;18;18;Без НДС\r\n19;TestProduct19;test-product19;[TestCategory2];+;RUB;19,00;19,00;19;;unit;;9,00;19,00;19,00;19x19x19;briefDesc19;Desc19;;;;;;;;Property4:PropertyValue19;BrandName9;-;sales note 19;;;;gtin19;Google Product Category19;yandex market category19;yandex type prefix 19;yandex model 19;-;-;;;19,00;119,00;1,;19;19;19;Без НДС\r\n20;TestProduct20;test-product20;[TestCategory2];+;RUB;20,00;20,00;20;;unit;;10,00;20,00;20,00;20x20x20;briefDesc20;Desc20;;;;;;;;Property5:PropertyValue20;BrandName10;-;sales note 20;;;;gtin20;Google Product Category20;yandex market category20;yandex type prefix 20;yandex model 20;-;-;;;20,00;120,00;1,;20;20;20;Без НДС"));
            // Assert.IsTrue(driver.PageSource.Contains("sku;name;paramsynonym;category;enabled;currency;price;purchaseprice;amount;sku:size:color:price:purchaseprice:amount;unit;discount;discountamount;shippingprice;weight;size;briefdescription;description;title;metakeywords;metadescription;h1;photos;videos;markers;properties;producer;preorder;salesnote;related sku;alternative sku;custom options;gtin;googleproductcategory;yandexproductcategory;yandextypeprefix;yandexmodel;adult;manufacturer_warranty;tags;gifts;minamount;maxamount;multiplicity;cbid;fee;barcode;tax\r\n1;TestProduct1;test-product1;[TestCategory1];+;RUB;1,00;1,00;1;;unit;1,00;;1,00;1,00;1x1x1;briefDesc1;Desc1;;;;;http://my-shop.ru/pictures/product/big/281_big.jpg;;r;Property1:PropertyValue1;BrandName1;-;sales note 1;;;;gtin1;Google Product Category1;yandex market category1;yandex type prefix 1;yandex model 1;-;-;;;1,00;101,00;1,;1;1;1;Без НДС\r\n2;TestProduct2;test-product2;[TestCategory1];-;RUB;2,00;2,00;2;;unit;2,00;;2,00;2,00;2x2x2;briefDesc2;Desc2;;;;;http://my-shop.ru/pictures/product/big/284_big.jpg;;r;Property2:PropertyValue2;BrandName2;-;sales note 2;;;;gtin2;Google Product Category2;yandex market category2;yandex type prefix 2;yandex model 2;-;-;;;2,00;102,00;1,;2;2;2;Без НДС\r\n3;TestProduct3;test-product3;[TestCategory1];-;RUB;3,00;3,00;3;;unit;3,00;;3,00;3,00;3x3x3;briefDesc3;Desc3;;;;;http://my-shop.ru/pictures/product/big/342_big.jpg;;r;Property3:PropertyValue3;BrandName3;-;sales note 3;;;;gtin3;Google Product Category3;yandex market category3;yandex type prefix 3;yandex model 3;-;-;;;3,00;103,00;1,;3;3;3;Без НДС\r\n4;TestProduct4;test-product4;[TestCategory1];+;RUB;4,00;4,00;4;;unit;4,00;;4,00;0,00;4x4x4;briefDesc4;Desc4;;;;;http://my-shop.ru/pictures/product/big/348_big.jpg;;n;Property4:PropertyValue4;BrandName4;-;sales note 4;;;;gtin4;Google Product Category4;yandex market category4;yandex type prefix 4;yandex model 4;-;-;;;4,00;104,00;1,;4;4;4;Без НДС\r\n5;TestProduct5;test-product5;[TestCategory1];+;RUB;5,00;5,00;5;;unit;5,00;;5,00;5,00;0x0x0;briefDesc5;Desc5;;;;;http://my-shop.ru/pictures/product/big/349_big.jpg;;n;Property5:PropertyValue5;BrandName5;-;sales note 5;;;;gtin5;Google Product Category5;yandex market category5;yandex type prefix 5;yandex model 5;-;-;;;5,00;105,00;1,;5;5;5;Без НДС\r\n6;TestProduct6;test-product6;[TestCategory1];+;RUB;6,00;6,00;6;;unit;6,00;;6,00;6,00;6x6x6;briefDesc6;Desc6;;;;;http://my-shop.ru/pictures/product/big/350_big.jpg;;n;Property1:PropertyValue6;BrandName6;-;sales note 6;;;;gtin6;Google Product Category6;yandex market category6;yandex type prefix 6;yandex model 6;-;-;;;6,00;106,00;1,;6;6;6;Без НДС\r\n7;TestProduct7;test-product7;[TestCategory1];+;RUB;7,00;7,00;7;;unit;7,00;;0,00;7,00;7x7x7;briefDesc7;Desc7;;;;;http://my-shop.ru/pictures/product/big/351_big.jpg;;b;Property2:PropertyValue7;BrandName7;-;sales note 7;;;;gtin7;Google Product Category7;yandex market category7;yandex type prefix 7;yandex model 7;-;-;;;7,00;107,00;1,;7;7;7;Без НДС\r\n8;TestProduct8;test-product8;[TestCategory1];+;RUB;8,00;8,00;8;;unit;8,00;;8,00;8,00;8x8x8;briefDesc8;Desc8;;;;;http://my-shop.ru/pictures/product/big/353_big.jpg;;b;Property3:PropertyValue8;BrandName8;-;sales note 8;;;;gtin8;Google Product Category8;yandex market category8;yandex type prefix 8;yandex model 8;-;-;;;8,00;108,00;1,;8;8;8;Без НДС\r\n9;TestProduct9;test-product9;[TestCategory1];+;RUB;9,00;9,00;9;;unit;9,00;;9,00;9,00;9x9x9;briefDesc9;Desc9;;;;;http://my-shop.ru/pictures/product/big/358_big.jpg;;b;Property4:PropertyValue9;BrandName9;-;sales note 9;;;;gtin9;Google Product Category9;yandex market category9;yandex type prefix 9;yandex model 9;-;-;;;9,00;109,00;1,;9;9;9;Без НДС\r\n10;TestProduct10;test-product10;[TestCategory1];+;RUB;10,00;10,00;10;;unit;10,00;;10,00;10,00;10x10x10;briefDesc10;Desc10;;;;;http://my-shop.ru/pictures/product/big/360_big.jpg;;s;\"Property1:PropertyValue1;Property5:PropertyValue10\";BrandName10;-;sales note 10;;;;gtin10;Google Product Category10;yandex market category10;yandex type prefix 10;yandex model 10;-;-;;;10,00;110,00;1,;10;10;10;Без НДС\r\n11;TestProduct11;test-product11;[TestCategory2];+;RUB;11,00;11,00;11;;unit;;1,00;11,00;11,00;11x11x11;briefDesc11;Desc11;;;;;;;s;Property1:PropertyValue11;BrandName1;-;sales note 11;;;;gtin11;Google Product Category11;yandex market category11;yandex type prefix 11;yandex model 11;-;-;;;11,00;111,00;1,;11;11;11;Без НДС\r\n12;TestProduct12;test-product12;[TestCategory2];+;RUB;12,00;12,00;12;;unit;;2,00;12,00;12,00;12x12x12;briefDesc12;Desc12;;;;;;;s;Property2:PropertyValue12;BrandName2;-;sales note 12;;;;gtin12;Google Product Category12;yandex market category12;yandex type prefix 12;yandex model 12;-;-;;;12,00;112,00;1,;12;12;12;Без НДС\r\n13;TestProduct13;test-product13;[TestCategory2];+;RUB;13,00;13,00;13;;;;3,00;13,00;13,00;13x13x13;briefDesc13;Desc13;;;;;;;;Property3:PropertyValue13;BrandName3;+;sales note 13;;;;gtin13;Google Product Category13;yandex market category13;yandex type prefix 13;yandex model 13;-;-;;;13,00;113,00;1,;13;13;13;Без НДС\r\n14;TestProduct14;test-product14;[TestCategory2];+;RUB;14,00;14,00;14;;unit;;4,00;14,00;14,00;14x14x14;briefDesc14;Desc14;;;;;;;;Property4:PropertyValue14;BrandName4;+;sales note 14;;;;gtin14;Google Product Category14;yandex market category14;yandex type prefix 14;yandex model 14;-;-;;;14,00;114,00;1,;14;14;14;Без НДС\r\n15;TestProduct15;test-product15;[TestCategory2];+;RUB;15,00;15,00;15;;unit;;5,00;15,00;15,00;15x15x15;briefDesc15;Desc15;;;;;;;;Property5:PropertyValue15;BrandName5;+;sales note 15;;;;gtin15;Google Product Category15;yandex market category15;yandex type prefix 15;yandex model 15;-;-;;;15,00;115,00;1,;15;15;15;Без НДС\r\n16;TestProduct16;test-product16;[TestCategory2];+;RUB;16,00;16,00;16;;unit;;6,00;16,00;16,00;16x16x16;briefDesc16;Desc16;;;;;;;;Property1:PropertyValue16;BrandName6;-;sales note 16;;;;gtin16;Google Product Category16;yandex market category16;yandex type prefix 16;yandex model 16;-;-;;;16,00;116,00;1,;16;16;16;Без НДС\r\n17;TestProduct17;test-product17;[TestCategory2];+;RUB;17,00;17,00;0;;unit;;7,00;17,00;17,00;17x17x17;briefDesc17;Desc17;;;;;;;;Property2:PropertyValue17;BrandName7;-;sales note 17;;;;gtin17;Google Product Category17;yandex market category17;yandex type prefix 17;yandex model 17;-;-;;;17,00;117,00;1,;17;17;17;Без НДС\r\n18;TestProduct18;test-product18;[TestCategory2];+;RUB;18,00;18,00;18;;unit;;8,00;18,00;18,00;18x18x18;briefDesc18;Desc18;;;;;;;;Property3:PropertyValue18;BrandName8;-;sales note 18;;;;gtin18;Google Product Category18;yandex market category18;yandex type prefix 18;yandex model 18;-;-;;;18,00;118,00;1,;18;18;18;Без НДС\r\n19;TestProduct19;test-product19;[TestCategory2];+;RUB;19,00;19,00;19;;unit;;9,00;19,00;19,00;19x19x19;briefDesc19;Desc19;;;;;;;;Property4:PropertyValue19;BrandName9;-;sales note 19;;;;gtin19;Google Product Category19;yandex market category19;yandex type prefix 19;yandex model 19;-;-;;;19,00;119,00;1,;19;19;19;Без НДС\r\n20;TestProduct20;test-product20;[TestCategory2];+;RUB;20,00;20,00;20;;unit;;10,00;20,00;20,00;20x20x20;briefDesc20;Desc20;;;;;;;;Property5:PropertyValue20;BrandName10;-;sales note 20;;;;gtin20;Google Product Category20;yandex market category20;yandex type prefix 20;yandex model 20;-;-;;;20,00;120,00;1,;20;20;20;Без НДС"));

        }

        [Test]
        public void ExportProductsSeparaters()
        {
            GoToAdmin("exportfeeds");

            driver.FindElement(By.XPath("//a[contains(text(), 'Выбор товаров')]")).Click();

            if (!driver.FindElement(By.CssSelector("[data-e2e=\"ExportChoiceCategories\"] input")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"ExportChoiceCategories\"] span")).Click();
            }
          if (!driver.FindElement(By.Id("1")).GetAttribute("aria-selected").Equals("true"))
            { 
                driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory1')]")).Click();
            }

            driver.FindElement(By.XPath("//a[contains(text(), 'Поля выгрузки')]")).Click();
            WaitForElem(By.Name("0ddl"));
            driver.FindElement(By.CssSelector("[data-e2e=\"exportSetNone\"]")).Click();
            (new SelectElement(driver.FindElement(By.Name("0ddl")))).SelectByText("Артикул");
            (new SelectElement(driver.FindElement(By.Name("1ddl")))).SelectByText("Наименование");
            (new SelectElement(driver.FindElement(By.Name("2ddl")))).SelectByText("Урл синоним");
            (new SelectElement(driver.FindElement(By.Name("3ddl")))).SelectByText("Категории");
            (new SelectElement(driver.FindElement(By.Name("4ddl")))).SelectByText("Свойства");

            driver.FindElement(By.XPath("//a[contains(text(), 'Параметры выгрузки')]")).Click();
            WaitForElem(By.Id("Name"));

            if (driver.FindElement(By.Id("ExportFeedSettings_Active")).Selected)

            {
                driver.FindElement(By.CssSelector("[data-e2e=\"exportCommonSettingsActive\"]")).Click();
            }

            (new SelectElement(driver.FindElement(By.Name("ddlFileExtention")))).SelectByText("csv");
            (new SelectElement(driver.FindElement(By.Id("CsvSeparator")))).SelectByText("Запятая");
            (new SelectElement(driver.FindElement(By.Id("CsvEnconing")))).SelectByText("UTF-8");
            driver.FindElement(By.Id("CsvColumSeparator")).Click();
            driver.FindElement(By.Id("CsvColumSeparator")).Clear();
            
            driver.FindElement(By.Id("CsvColumSeparator")).SendKeys(".");
            driver.FindElement(By.Id("CsvPropertySeparator")).Click();
            driver.FindElement(By.Id("CsvPropertySeparator")).Clear();
            
            driver.FindElement(By.Id("CsvPropertySeparator")).SendKeys("!");
            var priceMargin = driver.FindElement(By.Id("ExportFeedSettings_PriceMargin")).GetAttribute("value");
            if (!priceMargin.Equals("0"))
            {
                driver.FindElement(By.Id("ExportFeedSettings_PriceMargin")).Click();
                driver.FindElement(By.Id("ExportFeedSettings_PriceMargin")).Clear();
                
                driver.FindElement(By.Id("ExportFeedSettings_PriceMargin")).SendKeys("0");
            }
            DropFocus("h1");
            Functions.ExportProductsNoInCategoryOff(driver, baseURL);
            Functions.ExportProductsCategorySortOff(driver, baseURL);
           

            driver.FindElement(By.CssSelector("[data-e2e=\"ButtonSave\"]")).Click();
            Thread.Sleep(5000);
            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"Export\"]")).Click();
             Thread.Sleep(1000);
            GoToAdmin("exportfeeds/export/2");
            Thread.Sleep(1000);
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"ExportCountValue\"]")).Text.Contains("10"));
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"ExportCountTotal\"]")).Text.Contains("10"));
            GoToClient("catalog.csv?OpenInBrowser=true");
            WaitForElem(By.TagName("pre"));
                
            Assert.IsTrue(driver.PageSource.Contains("sku,name,paramsynonym,category,properties\r\n1,TestProduct1,test-product1,[TestCategory1],Property1!PropertyValue1\r\n2,TestProduct2,test-product2,[TestCategory1],Property2!PropertyValue2\r\n3,TestProduct3,test-product3,[TestCategory1],Property3!PropertyValue3\r\n4,TestProduct4,test-product4,[TestCategory1],Property4!PropertyValue4\r\n5,TestProduct5,test-product5,[TestCategory1],Property5!PropertyValue5\r\n6,TestProduct6,test-product6,[TestCategory1],Property1!PropertyValue6\r\n7,TestProduct7,test-product7,[TestCategory1],Property2!PropertyValue7\r\n8,TestProduct8,test-product8,[TestCategory1],Property3!PropertyValue8\r\n9,TestProduct9,test-product9,[TestCategory1],Property4!PropertyValue9\r\n10,TestProduct10,test-product10,[TestCategory1],Property1!PropertyValue1.Property5!PropertyValue10"));
        }

        [Test]
        public void ExportProductsNoInCategory()
        {
            GoToAdmin("exportfeeds");

            driver.FindElement(By.XPath("//a[contains(text(), 'Выбор товаров')]")).Click();

            if (!driver.FindElement(By.CssSelector("[data-e2e=\"ExportChoiceCategories\"] input")).Selected)
            { 
                driver.FindElement(By.CssSelector("[data-e2e=\"ExportChoiceCategories\"] span")).Click();
            }

            if (!driver.FindElement(By.Id("1")).GetAttribute("aria-selected").Equals("true"))
            { 
                driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory1')]")).Click();
            }

            driver.FindElement(By.XPath("//a[contains(text(), 'Поля выгрузки')]")).Click();
            WaitForElem(By.Name("0ddl"));
            driver.FindElement(By.CssSelector("[data-e2e=\"exportSetNone\"]")).Click();
            (new SelectElement(driver.FindElement(By.Name("0ddl")))).SelectByText("Артикул");
            (new SelectElement(driver.FindElement(By.Name("1ddl")))).SelectByText("Наименование");
            (new SelectElement(driver.FindElement(By.Name("2ddl")))).SelectByText("Урл синоним");
            (new SelectElement(driver.FindElement(By.Name("3ddl")))).SelectByText("Категории");
            (new SelectElement(driver.FindElement(By.Name("4ddl")))).SelectByText("Свойства");

            driver.FindElement(By.XPath("//a[contains(text(), 'Параметры выгрузки')]")).Click();
            WaitForElem(By.Id("Name"));

            if (driver.FindElement(By.Id("ExportFeedSettings_Active")).Selected)

            {
                driver.FindElement(By.CssSelector("[data-e2e=\"exportCommonSettingsActive\"]")).Click();
            }

            (new SelectElement(driver.FindElement(By.Name("ddlFileExtention")))).SelectByText("csv");
            (new SelectElement(driver.FindElement(By.Id("CsvSeparator")))).SelectByText("Точка с запятой");
            (new SelectElement(driver.FindElement(By.Id("CsvEnconing")))).SelectByText("UTF-8");
            driver.FindElement(By.Id("CsvColumSeparator")).Clear();
            
            driver.FindElement(By.Id("CsvColumSeparator")).SendKeys(";");
            driver.FindElement(By.Id("CsvPropertySeparator")).Clear();
            
            driver.FindElement(By.Id("CsvPropertySeparator")).SendKeys(":");
            var priceMargin = driver.FindElement(By.Id("ExportFeedSettings_PriceMargin")).GetAttribute("value");
            if (!priceMargin.Equals("0"))
            { 
                driver.FindElement(By.Id("ExportFeedSettings_PriceMargin")).Click();
                driver.FindElement(By.Id("ExportFeedSettings_PriceMargin")).Clear();
                
                driver.FindElement(By.Id("ExportFeedSettings_PriceMargin")).SendKeys("0");
            }
            DropFocus("h1");
            Functions.ExportProductsNoInCategoryOn(driver, baseURL);
            Functions.ExportProductsCategorySortOff(driver, baseURL);
           

            driver.FindElement(By.CssSelector("[data-e2e=\"ButtonSave\"]")).Click();
            Thread.Sleep(5000);
            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"Export\"]")).Click();
             Thread.Sleep(1000);
            GoToAdmin("exportfeeds/export/2");
            Thread.Sleep(1000);
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"ExportCountValue\"]")).Text.Contains("15"));
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"ExportCountTotal\"]")).Text.Contains("15"));
            GoToClient("catalog.csv?OpenInBrowser=true");
            WaitForElem(By.TagName("pre"));

            Assert.IsTrue(driver.PageSource.Contains("sku;name;paramsynonym;category;properties\r\n1;TestProduct1;test-product1;[TestCategory1];Property1:PropertyValue1\r\n2;TestProduct2;test-product2;[TestCategory1];Property2:PropertyValue2\r\n3;TestProduct3;test-product3;[TestCategory1];Property3:PropertyValue3\r\n4;TestProduct4;test-product4;[TestCategory1];Property4:PropertyValue4\r\n5;TestProduct5;test-product5;[TestCategory1];Property5:PropertyValue5\r\n6;TestProduct6;test-product6;[TestCategory1];Property1:PropertyValue6\r\n7;TestProduct7;test-product7;[TestCategory1];Property2:PropertyValue7\r\n8;TestProduct8;test-product8;[TestCategory1];Property3:PropertyValue8\r\n9;TestProduct9;test-product9;[TestCategory1];Property4:PropertyValue9\r\n10;TestProduct10;test-product10;[TestCategory1];\"Property1:PropertyValue1;Property5:PropertyValue10\"\r\n21;TestProduct21;test-product21;;Property1:PropertyValue1\r\n22;TestProduct22;test-product22;;Property2:PropertyValue2\r\n23;TestProduct23;test-product23;;Property3:PropertyValue3\r\n24;TestProduct24;test-product24;;Property4:PropertyValue4\r\n25;TestProduct25;test-product25;;Property5:PropertyValue5"));
        }
        
        [Test]
        public void ExportProductsCategorySort()
        {
            GoToAdmin("exportfeeds#?exportfeedtab=1");

            driver.FindElement(By.XPath("//a[contains(text(), 'Выбор товаров')]")).Click();

            if (!driver.FindElement(By.CssSelector("[data-e2e=\"ExportChoiceCategories\"] input")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"ExportChoiceCategories\"] span")).Click();
            }

            if (!driver.FindElement(By.Id("1")).GetAttribute("aria-selected").Equals("true"))
            { 
                driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory1')]")).Click();
                
            }

            driver.FindElement(By.XPath("//a[contains(text(), 'Поля выгрузки')]")).Click();
            WaitForElem(By.Name("0ddl"));
            driver.FindElement(By.CssSelector("[data-e2e=\"exportSetNone\"]")).Click();
            (new SelectElement(driver.FindElement(By.Name("0ddl")))).SelectByText("Артикул");
            (new SelectElement(driver.FindElement(By.Name("1ddl")))).SelectByText("Наименование");
            (new SelectElement(driver.FindElement(By.Name("2ddl")))).SelectByText("Категории");

            driver.FindElement(By.XPath("//a[contains(text(), 'Параметры выгрузки')]")).Click();
            WaitForElem(By.Id("Name"));

            if (driver.FindElement(By.Id("ExportFeedSettings_Active")).Selected)

            {
                driver.FindElement(By.CssSelector("[data-e2e=\"exportCommonSettingsActive\"]")).Click();
            }

            (new SelectElement(driver.FindElement(By.Name("ddlFileExtention")))).SelectByText("csv");
            (new SelectElement(driver.FindElement(By.Id("CsvSeparator")))).SelectByText("Точка с запятой");
            (new SelectElement(driver.FindElement(By.Id("CsvEnconing")))).SelectByText("UTF-8");
            driver.FindElement(By.Id("CsvColumSeparator")).Click();
            driver.FindElement(By.Id("CsvColumSeparator")).Clear();
            
            driver.FindElement(By.Id("CsvColumSeparator")).SendKeys(";");
            driver.FindElement(By.Id("CsvPropertySeparator")).Click();
            driver.FindElement(By.Id("CsvPropertySeparator")).Clear();
            
            driver.FindElement(By.Id("CsvPropertySeparator")).SendKeys(":");
            DropFocus("h1");
            var priceMargin = driver.FindElement(By.Id("ExportFeedSettings_PriceMargin")).GetAttribute("value");
            if (!priceMargin.Equals("0"))
            { 
                driver.FindElement(By.Id("ExportFeedSettings_PriceMargin")).Click();
                driver.FindElement(By.Id("ExportFeedSettings_PriceMargin")).Clear();
                
                driver.FindElement(By.Id("ExportFeedSettings_PriceMargin")).SendKeys("0");
            }
            DropFocus("h1");
            Functions.ExportProductsNoInCategoryOff(driver, baseURL);
            Functions.ExportProductsCategorySortOn(driver, baseURL);
           

            driver.FindElement(By.CssSelector("[data-e2e=\"ButtonSave\"]")).Click();
            Thread.Sleep(5000);
            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"Export\"]")).Click();
             Thread.Sleep(1000);
            GoToAdmin("exportfeeds/export/2");
            Thread.Sleep(1000);
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"ExportCountValue\"]")).Text.Contains("10"));
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"ExportCountTotal\"]")).Text.Contains("10"));
            GoToClient("catalog.csv?OpenInBrowser=true");
            WaitForElem(By.TagName("pre"));

            Assert.IsTrue(driver.PageSource.Contains("sku;name;category;sorting\r\n1;TestProduct1;[TestCategory1];1\r\n2;TestProduct2;[TestCategory1];2\r\n3;TestProduct3;[TestCategory1];3\r\n4;TestProduct4;[TestCategory1];4\r\n5;TestProduct5;[TestCategory1];5\r\n6;TestProduct6;[TestCategory1];6\r\n7;TestProduct7;[TestCategory1];7\r\n8;TestProduct8;[TestCategory1];8\r\n9;TestProduct9;[TestCategory1];9\r\n10;TestProduct10;[TestCategory1];10"));
        }


        [Test]
        public void ExportProductsPriceMargin()
        {
            GoToAdmin("exportfeeds");

            driver.FindElement(By.XPath("//a[contains(text(), 'Выбор товаров')]")).Click();

            if (!driver.FindElement(By.CssSelector("[data-e2e=\"ExportChoiceCategories\"] input")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"ExportChoiceCategories\"] span")).Click();
            }

            if (!driver.FindElement(By.Id("1")).GetAttribute("aria-selected").Equals("true"))
            { 
                driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory1')]")).Click();
                
            }

            driver.FindElement(By.XPath("//a[contains(text(), 'Поля выгрузки')]")).Click();
            WaitForElem(By.Name("0ddl"));
            driver.FindElement(By.CssSelector("[data-e2e=\"exportSetNone\"]")).Click();
            (new SelectElement(driver.FindElement(By.Name("0ddl")))).SelectByText("Артикул");
            (new SelectElement(driver.FindElement(By.Name("1ddl")))).SelectByText("Наименование");
            (new SelectElement(driver.FindElement(By.Name("2ddl")))).SelectByText("Категории");
            (new SelectElement(driver.FindElement(By.Name("3ddl")))).SelectByText("Цена");

            driver.FindElement(By.XPath("//a[contains(text(), 'Параметры выгрузки')]")).Click();
            WaitForElem(By.Id("Name"));

            if (driver.FindElement(By.Id("ExportFeedSettings_Active")).Selected)

            {
                driver.FindElement(By.CssSelector("[data-e2e=\"exportCommonSettingsActive\"]")).Click();
            }

            (new SelectElement(driver.FindElement(By.Name("ddlFileExtention")))).SelectByText("csv");
            (new SelectElement(driver.FindElement(By.Id("CsvSeparator")))).SelectByText("Точка с запятой");
            (new SelectElement(driver.FindElement(By.Id("CsvEnconing")))).SelectByText("UTF-8");
            driver.FindElement(By.Id("CsvColumSeparator")).Clear();
            
            driver.FindElement(By.Id("CsvColumSeparator")).SendKeys(";");
            driver.FindElement(By.Id("CsvPropertySeparator")).Clear();
            
            driver.FindElement(By.Id("CsvPropertySeparator")).SendKeys(":");
            driver.FindElement(By.Id("ExportFeedSettings_PriceMargin")).Clear();
            
            driver.FindElement(By.Id("ExportFeedSettings_PriceMargin")).SendKeys("50");
            DropFocus("h1");
            Functions.ExportProductsNoInCategoryOff(driver, baseURL);
            Functions.ExportProductsCategorySortOff(driver, baseURL);
           

            driver.FindElement(By.CssSelector("[data-e2e=\"ButtonSave\"]")).Click();
            Thread.Sleep(5000);
            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"Export\"]")).Click();
             Thread.Sleep(1000);
            GoToAdmin("exportfeeds/export/2");
            Thread.Sleep(1000);
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"ExportCountValue\"]")).Text.Contains("10"));
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"ExportCountTotal\"]")).Text.Contains("10"));
            GoToClient("catalog.csv?OpenInBrowser=true");
            WaitForElem(By.TagName("pre"));

            Assert.IsTrue(driver.Url.Contains("csv"));
            Assert.IsTrue(driver.PageSource.Contains("sku;name;category;price\r\n1;TestProduct1;[TestCategory1];1,50\r\n2;TestProduct2;[TestCategory1];3,00\r\n3;TestProduct3;[TestCategory1];4,50\r\n4;TestProduct4;[TestCategory1];6,00\r\n5;TestProduct5;[TestCategory1];7,50\r\n6;TestProduct6;[TestCategory1];9,00\r\n7;TestProduct7;[TestCategory1];10,50\r\n8;TestProduct8;[TestCategory1];12,00\r\n9;TestProduct9;[TestCategory1];13,50\r\n10;TestProduct10;[TestCategory1];15,00"));
        }
    }
}
