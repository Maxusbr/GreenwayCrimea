using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;
using AdvantShop.Selenium.Core.Infrastructure;

namespace AdvantShop.SeleniumTest.Admin.Catalog.ProductAddEdit.Price
{
    [TestFixture]
    public class ProductAddEditPriceTest : BaseSeleniumTest
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
        public void ProductEditChangeCurrency()
        {
            GoToAdmin("product/edit/1");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Цена и наличие')]")).Click();
            Thread.Sleep(1000);

            //pre check
            IWebElement selectElemBegin = driver.FindElement(By.Id("CurrencyId"));
            SelectElement select = new SelectElement(selectElemBegin);
            Assert.IsTrue(select.AllSelectedOptions[0].Text.Contains("Рубли"));

            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"DiscountAmount\"]")).FindElement(By.TagName("span")).Text.Contains("руб."));

            (new SelectElement(driver.FindElement(By.Id("CurrencyId")))).SelectByText("Евро");

            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("product/edit/1");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Цена и наличие')]")).Click();
            Thread.Sleep(1000);

            IWebElement selectElemEnd = driver.FindElement(By.Id("CurrencyId"));
            SelectElement select1 = new SelectElement(selectElemEnd);
            Assert.IsTrue(select1.AllSelectedOptions[0].Text.Contains("Евро"));

            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"DiscountAmount\"]")).FindElement(By.TagName("span")).Text.Contains("€"));

            //check client
            GoToClient("products/test-product1");
            Assert.IsTrue(driver.FindElement(By.CssSelector(".details-payment-cell.details-payment-price .price-number")).Text.Contains("75"));
        }

        [Test]
        public void ProductDiscountAddAmount()
        {
            GoToAdmin("mainpageproducts?type=sale");
            Assert.AreEqual("TestProduct18", GetGridCell(0, "Name").Text);
            Assert.AreEqual("TestProduct20", GetGridCell(1, "Name").Text);
            Assert.AreEqual("Найдено записей: 2", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);

            GoToAdmin("product/edit/4");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Цена и наличие')]")).Click();
            Thread.Sleep(1000);

            if (driver.FindElement(By.CssSelector("[data-e2e=\"DiscountAmount\"]")).GetAttribute("class").Contains("active"))
            {
                Assert.AreEqual("0", driver.FindElement(By.Id("DiscountAmount")).GetAttribute("value"));
            }

            else if (driver.FindElement(By.CssSelector("[data-e2e=\"DiscountPercent\"]")).GetAttribute("class").Contains("active"))
            {
                Assert.AreEqual("0", driver.FindElement(By.Id("DiscountPercent")).GetAttribute("value"));
            }
            
            if (!driver.FindElement(By.CssSelector("[data-e2e=\"DiscountAmount\"]")).GetAttribute("class").Contains("active"))
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"DiscountAmount\"]")).Click();
            }

            driver.FindElement(By.Id("DiscountAmount")).Click();
            driver.FindElement(By.Id("DiscountAmount")).Clear();
            driver.FindElement(By.Id("DiscountAmount")).SendKeys("3");
            XPathContainsText("h2", "Цена и наличие");

            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("product/edit/4");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Цена и наличие')]")).Click();
            Thread.Sleep(1000);

            Assert.AreEqual("3", driver.FindElement(By.Id("DiscountAmount")).GetAttribute("value"));
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"DiscountAmount\"]")).GetAttribute("class").Contains("active"));
            
            GoToAdmin("mainpageproducts?type=sale");
            Assert.IsTrue(driver.FindElement(By.CssSelector(".ui-grid-contents-wrapper")).Text.Contains("TestProduct4"));
            Assert.AreEqual("Найдено записей: 3", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);

            //check client
            GoToClient("products/test-product4");
            Assert.IsTrue(driver.FindElement(By.CssSelector(".details-payment-cell.details-payment-price")).FindElements(By.CssSelector(".price-number"))[1].Text.Contains("1"));
            Assert.IsTrue(driver.PageSource.Contains("Скидка 3  руб."));
            Assert.IsTrue(driver.FindElement(By.CssSelector(".details-payment-block .price-discount")).Text.Contains("выгода 3 руб."));
        }

        [Test]
        public void ProductDiscountAddPercent()
        {
            GoToAdmin("mainpageproducts?type=sale");
            var gridCountBegin = driver.FindElements(By.CssSelector("[data-e2e=\"gridRow\"]")).Count;
            Assert.IsTrue(gridCountBegin >= 2);

            GoToAdmin("product/edit/26");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Цена и наличие')]")).Click();
            Thread.Sleep(1000);

            if (driver.FindElement(By.CssSelector("[data-e2e=\"DiscountAmount\"]")).GetAttribute("class").Contains("active"))
            {
                Assert.AreEqual("0", driver.FindElement(By.Id("DiscountAmount")).GetAttribute("value"));
            }

            else if (driver.FindElement(By.CssSelector("[data-e2e=\"DiscountPercent\"]")).GetAttribute("class").Contains("active"))
            {
                Assert.AreEqual("0", driver.FindElement(By.Id("DiscountPercent")).GetAttribute("value"));
            }

            if (!driver.FindElement(By.CssSelector("[data-e2e=\"DiscountPercent\"]")).GetAttribute("class").Contains("active"))
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"DiscountPercent\"]")).Click();
            }

            driver.FindElement(By.Id("DiscountPercent")).Click();
            driver.FindElement(By.Id("DiscountPercent")).Clear();
            driver.FindElement(By.Id("DiscountPercent")).SendKeys("50");
            XPathContainsText("h2", "Цена и наличие");

            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("product/edit/26");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Цена и наличие')]")).Click();
            Thread.Sleep(1000);

            Assert.AreEqual("50", driver.FindElement(By.Id("DiscountPercent")).GetAttribute("value"));
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"DiscountPercent\"]")).GetAttribute("class").Contains("active"));

            GoToAdmin("mainpageproducts?type=sale");
            var gridCountEnd = driver.FindElements(By.CssSelector("[data-e2e=\"gridRow\"]")).Count;
            Assert.IsTrue(gridCountBegin < gridCountEnd);

            //check client
            GoToClient("products/test-product26");
            Assert.IsTrue(driver.FindElement(By.CssSelector(".details-payment-cell.details-payment-price")).FindElements(By.CssSelector(".price-number"))[1].Text.Contains("13"));
            Assert.IsTrue(driver.PageSource.Contains("Скидка   50%"));
            Assert.AreEqual("выгода 13 руб. или 50%", driver.FindElement(By.CssSelector(".details-payment-block .price-discount")).Text);
        }

        [Test]
        public void ProductDiscountPercentEdit()
        {
            //pre check
            GoToAdmin("mainpageproducts?type=sale");
            var gridCountBegin = driver.FindElements(By.CssSelector("[data-e2e=\"gridRow\"]")).Count;

            GoToClient("products/test-product18");
            Assert.IsTrue(driver.FindElement(By.CssSelector(".details-payment-cell.details-payment-price")).FindElements(By.CssSelector(".price-number"))[1].Text.Contains("15"));
            Assert.IsTrue(driver.PageSource.Contains("Скидка   18%"));
            Assert.AreEqual("выгода 3 руб. или 18%", driver.FindElement(By.CssSelector(".details-payment-block .price-discount")).Text);

            GoToAdmin("product/edit/18");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Цена и наличие')]")).Click();
            Thread.Sleep(1000);

            Assert.AreEqual("18", driver.FindElement(By.Id("DiscountPercent")).GetAttribute("value"));
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"DiscountPercent\"]")).GetAttribute("class").Contains("active"));
            
            driver.FindElement(By.Id("DiscountPercent")).Click();
            driver.FindElement(By.Id("DiscountPercent")).Clear();
            driver.FindElement(By.Id("DiscountPercent")).SendKeys("50");
            XPathContainsText("h2", "Цена и наличие");

            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("product/edit/18");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Цена и наличие')]")).Click();
            Thread.Sleep(1000);

            Assert.AreEqual("50", driver.FindElement(By.Id("DiscountPercent")).GetAttribute("value"));
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"DiscountPercent\"]")).GetAttribute("class").Contains("active"));

            GoToAdmin("mainpageproducts?type=sale");
            var gridCountEnd = driver.FindElements(By.CssSelector("[data-e2e=\"gridRow\"]")).Count;
            Assert.IsTrue(gridCountBegin.Equals(gridCountEnd));

            //check client
            GoToClient("products/test-product18");

            Assert.IsTrue(driver.FindElement(By.CssSelector(".details-payment-cell.details-payment-price")).FindElements(By.CssSelector(".price-number"))[1].Text.Contains("9"));
            Assert.IsTrue(driver.PageSource.Contains("Скидка   50%"));
            Assert.AreEqual("выгода 9 руб. или 50%", driver.FindElement(By.CssSelector(".details-payment-block .price-discount")).Text);
        }

        [Test]
        public void ProductDiscountEdit()
        {
            //pre check
            GoToAdmin("mainpageproducts?type=sale");
            var gridCountBegin = driver.FindElements(By.CssSelector("[data-e2e=\"gridRow\"]")).Count;

            GoToClient("products/test-product20");
            Assert.IsTrue(driver.FindElement(By.CssSelector(".details-payment-cell.details-payment-price")).FindElements(By.CssSelector(".price-number"))[1].Text.Contains("15"));
            Assert.IsTrue(driver.PageSource.Contains("Скидка 5  руб."));
            Assert.IsTrue(driver.FindElement(By.CssSelector(".details-payment-block .price-discount")).Text.Contains("выгода 5 руб."));

            GoToAdmin("product/edit/20");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Цена и наличие')]")).Click();
            Thread.Sleep(1000);

            Assert.AreEqual("5", driver.FindElement(By.Id("DiscountAmount")).GetAttribute("value"));
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"DiscountAmount\"]")).GetAttribute("class").Contains("active"));
            
            driver.FindElement(By.CssSelector("[data-e2e=\"DiscountPercent\"]")).Click();

            driver.FindElement(By.Id("DiscountPercent")).Click();
            driver.FindElement(By.Id("DiscountPercent")).Clear();
            driver.FindElement(By.Id("DiscountPercent")).SendKeys("50");
            XPathContainsText("h2", "Цена и наличие");

            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("product/edit/20");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Цена и наличие')]")).Click();
            Thread.Sleep(1000);

            Assert.AreEqual("50", driver.FindElement(By.Id("DiscountPercent")).GetAttribute("value"));
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"DiscountPercent\"]")).GetAttribute("class").Contains("active"));

            GoToAdmin("mainpageproducts?type=sale");
            var gridCountEnd = driver.FindElements(By.CssSelector("[data-e2e=\"gridRow\"]")).Count;
            Assert.IsTrue(gridCountBegin.Equals(gridCountEnd));

            //check client
            GoToClient("products/test-product20");

            Assert.IsTrue(driver.FindElement(By.CssSelector(".details-payment-cell.details-payment-price")).FindElements(By.CssSelector(".price-number"))[1].Text.Contains("10"));
            Assert.IsTrue(driver.PageSource.Contains("Скидка   50%"));
            Assert.AreEqual("выгода 10 руб. или 50%", driver.FindElement(By.CssSelector(".details-payment-block .price-discount")).Text);
        }

        [Test]
        public void ProductEditAllowPreOrder()
        {
            GoToClient("products/test-product101");
            Assert.IsTrue(driver.PageSource.Contains("Нет в наличии"));
            Assert.IsFalse(driver.PageSource.Contains("Под заказ"));

            GoToAdmin("product/edit/101");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Цена и наличие')]")).Click();
            Thread.Sleep(1000);

            if (!driver.FindElement(By.Id("AllowPreOrder")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"AllowPreOrderClick\"]")).Click();

                ScrollTo(By.Id("header-top"));
                GetButton(eButtonType.Save).Click();
                Thread.Sleep(2000);
            }
            
            GoToAdmin("product/edit/101");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Цена и наличие')]")).Click();
            Thread.Sleep(1000);

            //check admin
            Assert.IsTrue(driver.FindElement(By.Id("AllowPreOrder")).Selected);

            //check client
            GoToClient("products/test-product101");
            Assert.IsTrue(driver.PageSource.Contains("Нет в наличии"));
            Assert.IsTrue(driver.PageSource.Contains("Под заказ"));
        }

        [Test]
        public void ProductEditDisallowPreOrder()
        {
            GoToClient("products/test-product17");
            Assert.IsTrue(driver.PageSource.Contains("Нет в наличии"));
            Assert.IsTrue(driver.PageSource.Contains("Под заказ"));

            GoToAdmin("product/edit/17");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Цена и наличие')]")).Click();
            Thread.Sleep(1000);

            if (driver.FindElement(By.Id("AllowPreOrder")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"AllowPreOrderClick\"]")).Click();

                ScrollTo(By.Id("header-top"));
                GetButton(eButtonType.Save).Click();
                Thread.Sleep(2000);
            }
            
            GoToAdmin("product/edit/17");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Цена и наличие')]")).Click();
            Thread.Sleep(1000);

            //check  admin
            Assert.IsFalse(driver.FindElement(By.Id("AllowPreOrder")).Selected);
            
            //check client
            GoToClient("products/test-product17");
            Assert.IsTrue(driver.PageSource.Contains("Нет в наличии"));
            Assert.IsFalse(driver.PageSource.Contains("Под заказ"));
        }

        [Test]
        public void ProductEditNewPriceAddMain()
        {
            GoToAdmin("product/edit/6");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Цена и наличие')]")).Click();
            Thread.Sleep(1000);

            Assert.IsTrue(GetGridCell(0, "Size", "Offers").FindElement(By.CssSelector("[data-e2e=\"itemSelected\"]")).Text.Contains("SizeName6"));
            Assert.IsTrue(GetGridCell(0, "Color", "Offers").FindElement(By.CssSelector("[data-e2e=\"itemSelected\"]")).Text.Contains("Color6"));
            Assert.IsTrue(GetGridCell(0, "Main", "Offers").FindElement(By.CssSelector("[data-e2e=\"switchOnOffSelect\"]")).Selected);
            
            driver.FindElement(By.XPath("//a[contains(text(), 'Добавить цену')]")).Click();
            Thread.Sleep(1000);
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"AddSize\"]")))).SelectByText("SizeName1");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"AddColor\"]")))).SelectByText("Color1");
            driver.FindElement(By.CssSelector("[data-e2e=\"AddArtNo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"AddArtNo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"AddArtNo\"]")).SendKeys("666");
            driver.FindElement(By.CssSelector("[data-e2e=\"AddPrice\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"AddPrice\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"AddPrice\"]")).SendKeys("666");
            driver.FindElement(By.CssSelector("[data-e2e=\"AddAmount\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"AddAmount\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"AddAmount\"]")).SendKeys("666");
            driver.FindElement(By.CssSelector("[data-e2e=\"AddSupplyPrice\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"AddSupplyPrice\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"AddSupplyPrice\"]")).SendKeys("666");
            DropFocus("h2");
            driver.FindElement(By.CssSelector("[data-e2e=\"MainClick\"]")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("product/edit/6");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Цена и наличие')]")).Click();
            Thread.Sleep(1000);

            Assert.IsTrue(GetGridCell(1, "Size", "Offers").FindElement(By.CssSelector("[data-e2e=\"itemSelected\"]")).Text.Contains("SizeName1"));
            Assert.IsTrue(GetGridCell(1, "Color", "Offers").FindElement(By.CssSelector("[data-e2e=\"itemSelected\"]")).Text.Contains("Color1"));
            Assert.IsTrue(GetGridCell(1, "Main", "Offers").FindElement(By.CssSelector("[data-e2e=\"switchOnOffSelect\"]")).Selected);
            Assert.AreEqual("666", GetGridCell(1, "ArtNo", "Offers").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("666", GetGridCell(1, "BasePrice", "Offers").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("666", GetGridCell(1, "SupplyPrice", "Offers").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("666", GetGridCell(1, "Amount", "Offers").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.IsFalse(GetGridCell(0, "Main", "Offers").FindElement(By.CssSelector("[data-e2e=\"switchOnOffSelect\"]")).Selected);

            //check client
            GoToClient("products/test-product6");
            Assert.IsTrue(driver.PageSource.Contains("SizeName1"));
            Assert.IsTrue(driver.PageSource.Contains("SizeName6"));
            Assert.IsTrue(driver.FindElement(By.CssSelector(".details-payment-cell.details-payment-price .price-number")).Text.Contains("666"));
        }

        [Test]
        public void ProductEditNewPriceAddNotMain()
        {
            GoToAdmin("product/edit/5");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Цена и наличие')]")).Click();
            Thread.Sleep(1000);

            Assert.IsTrue(GetGridCell(0, "Main", "Offers").FindElement(By.CssSelector("[data-e2e=\"switchOnOffSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(0, "Size", "Offers").FindElement(By.CssSelector("[data-e2e=\"itemSelected\"]")).Text.Contains("SizeName5"));

            //scroll
            driver.FindElement(By.XPath("//a[contains(text(), 'Добавить цену')]")).Click();
            Thread.Sleep(1000);
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"AddSize\"]")))).SelectByText("SizeName1");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"AddColor\"]")))).SelectByText("Color1");
            driver.FindElement(By.CssSelector("[data-e2e=\"AddPrice\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"AddPrice\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"AddPrice\"]")).SendKeys("555");
            driver.FindElement(By.CssSelector("[data-e2e=\"AddAmount\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"AddAmount\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"AddAmount\"]")).SendKeys("555");
            DropFocus("h2");

            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("product/edit/5");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Цена и наличие')]")).Click();
            Thread.Sleep(1000);

            Assert.IsTrue(GetGridCell(1, "Size", "Offers").FindElement(By.CssSelector("[data-e2e=\"itemSelected\"]")).Text.Contains("SizeName1"));
            Assert.IsFalse(GetGridCell(1, "Main", "Offers").FindElement(By.CssSelector("[data-e2e=\"switchOnOffSelect\"]")).Selected);

            //check client
            GoToClient("products/test-product5");
            Assert.IsTrue(driver.PageSource.Contains("SizeName1"));
            Assert.IsTrue(driver.PageSource.Contains("SizeName5"));
        }

        [Test]
        public void ProductEditNewPriceAdd()
        {
            GoToAdmin("product/edit/102");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Цена и наличие')]")).Click();
            Thread.Sleep(1000);

            Assert.IsTrue(GetGridCell(0, "Size", "Offers").FindElement(By.CssSelector("[data-e2e=\"itemSelected\"]")).Text.Contains("––––"));
            Assert.IsTrue(GetGridCell(0, "Color", "Offers").FindElement(By.CssSelector("[data-e2e=\"itemSelected\"]")).Text.Contains("––––"));

            Assert.IsTrue(GetGridCell(0, "Main", "Offers").FindElement(By.CssSelector("[data-e2e=\"switchOnOffSelect\"]")).Selected);
           
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

            //check admin
            GoToAdmin("product/edit/102");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Цена и наличие')]")).Click();
            Thread.Sleep(1000);

            Assert.IsTrue(driver.PageSource.Contains("Укажите размер"));
            
            Assert.IsTrue(GetGridCell(1, "Size", "Offers").FindElement(By.CssSelector("[data-e2e=\"itemSelected\"]")).Text.Contains("SizeName1"));
            Assert.IsTrue(GetGridCell(1, "Color", "Offers").FindElement(By.CssSelector("[data-e2e=\"itemSelected\"]")).Text.Contains("Color1"));
            Assert.IsFalse(GetGridCell(1, "Main", "Offers").FindElement(By.CssSelector("[data-e2e=\"switchOnOffSelect\"]")).Selected);
            
            GetGridCell(0, "Size", "Offers").FindElement(By.CssSelector("[data-e2e=\"select\"]")).Click();
            driver.FindElement(By.CssSelector(".ui-select-choices.ui-select-choices-content.ui-select-dropdown.dropdown-menu")).FindElement(By.XPath("//span[contains(text(), 'SizeName2')]")).Click();

            GetGridCell(0, "Color", "Offers").FindElement(By.CssSelector("[data-e2e=\"select\"]")).Click();
            driver.FindElement(By.CssSelector(".ui-select-choices.ui-select-choices-content.ui-select-dropdown.dropdown-menu")).FindElement(By.XPath("//span[contains(text(), 'Color2')]")).Click();

            //check admin with first price modified
            GoToAdmin("product/edit/102");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Цена и наличие')]")).Click();
            Thread.Sleep(1000);

            Assert.IsTrue(GetGridCell(0, "Size", "Offers").FindElement(By.CssSelector("[data-e2e=\"itemSelected\"]")).Text.Contains("SizeName2"));
            Assert.IsTrue(GetGridCell(0, "Color", "Offers").FindElement(By.CssSelector("[data-e2e=\"itemSelected\"]")).Text.Contains("Color2"));
            Assert.IsTrue(GetGridCell(1, "Size", "Offers").FindElement(By.CssSelector("[data-e2e=\"itemSelected\"]")).Text.Contains("SizeName1"));
            Assert.IsTrue(GetGridCell(1, "Color", "Offers").FindElement(By.CssSelector("[data-e2e=\"itemSelected\"]")).Text.Contains("Color1"));

            //check client
            GoToClient("products/test-product102");
            Assert.IsTrue(driver.PageSource.Contains("SizeName1"));
            Assert.IsTrue(driver.PageSource.Contains("SizeName2"));
            Assert.IsTrue(driver.FindElement(By.CssSelector(".color-viewer-header")).Displayed);
        }

        [Test]
        public void ProductEditPriceDelete()
        {
            GoToAdmin("product/edit/7");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Цена и наличие')]")).Click();
            Thread.Sleep(1000);

            Assert.IsTrue(GetGridCell(0, "Size", "Offers").FindElement(By.CssSelector("[data-e2e=\"itemSelected\"]")).Text.Contains("SizeName7"));

            //scroll
            driver.FindElement(By.XPath("//a[contains(text(), 'Добавить цену')]")).Click();
            Thread.Sleep(1000);
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"AddSize\"]")))).SelectByText("SizeName1");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"AddColor\"]")))).SelectByText("Color1");
            driver.FindElement(By.CssSelector("[data-e2e=\"AddPrice\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"AddPrice\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"AddPrice\"]")).SendKeys("777");
            driver.FindElement(By.CssSelector("[data-e2e=\"AddAmount\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"AddAmount\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"AddAmount\"]")).SendKeys("777");
            DropFocus("h2");

            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(2000);

            //check delete price
            GoToAdmin("product/edit/7");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Цена и наличие')]")).Click();
            Thread.Sleep(1000);

            Assert.IsTrue(GetGridCell(1, "Size", "Offers").FindElement(By.CssSelector("[data-e2e=\"itemSelected\"]")).Text.Contains("SizeName1"));
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridRow\"]")).Count == 2);
            
            GetGridCell(1, "_serviceColumn", "Offers").Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("product/edit/7");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Цена и наличие')]")).Click();
            Thread.Sleep(1000);

            Assert.IsTrue(GetGridCell(0, "Size", "Offers").FindElement(By.CssSelector("[data-e2e=\"itemSelected\"]")).Text.Contains("SizeName7"));
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridRow\"]")).Count == 1);

            //check client
            GoToClient("products/test-product7");
            Assert.IsFalse(driver.PageSource.Contains("SizeName1"));
            Assert.IsTrue(driver.PageSource.Contains("SizeName7"));
        }

        [Test]
        public void ProductEditPriceDeleteMain()
        {
            GoToAdmin("product/edit/8");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Цена и наличие')]")).Click();
            Thread.Sleep(1000);

            Assert.IsTrue(GetGridCell(0, "Main", "Offers").FindElement(By.CssSelector("[data-e2e=\"switchOnOffLabel\"]")).FindElement(By.TagName("input")).Selected);
            Assert.IsTrue(GetGridCell(0, "Size", "Offers").FindElement(By.CssSelector("[data-e2e=\"itemSelected\"]")).Text.Contains("SizeName8"));
            
            driver.FindElement(By.XPath("//a[contains(text(), 'Добавить цену')]")).Click();
            Thread.Sleep(1000);
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"AddSize\"]")))).SelectByText("SizeName1");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"AddColor\"]")))).SelectByText("Color1");
            driver.FindElement(By.CssSelector("[data-e2e=\"AddPrice\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"AddPrice\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"AddPrice\"]")).SendKeys("888");
            driver.FindElement(By.CssSelector("[data-e2e=\"AddAmount\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"AddAmount\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"AddAmount\"]")).SendKeys("888");
            DropFocus("h2");

            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(2000);

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Цена и наличие')]")).Click();
            Thread.Sleep(1000);

            Assert.IsTrue(GetGridCell(0, "Main", "Offers").FindElement(By.CssSelector("[data-e2e=\"switchOnOffSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(0, "Size", "Offers").FindElement(By.CssSelector("[data-e2e=\"itemSelected\"]")).Text.Contains("SizeName8"));
            Assert.IsFalse(GetGridCell(1, "Main", "Offers").FindElement(By.CssSelector("[data-e2e=\"switchOnOffSelect\"]")).Selected);
   
            //check delete price
            GoToAdmin("product/edit/8");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Цена и наличие')]")).Click();
            Thread.Sleep(1000);
            GetGridCell(0, "_serviceColumn", "Offers").Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("product/edit/8");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Цена и наличие')]")).Click();
            Thread.Sleep(1000);

            Assert.IsTrue(GetGridCell(0, "Main", "Offers").FindElement(By.CssSelector("[data-e2e=\"switchOnOffSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(0, "Size", "Offers").FindElement(By.CssSelector("[data-e2e=\"itemSelected\"]")).Text.Contains("SizeName1"));
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridRow\"]")).Count == 1);

            //check client
            GoToClient("products/test-product8");
            Assert.IsTrue(driver.PageSource.Contains("SizeName1"));
            Assert.IsFalse(driver.PageSource.Contains("SizeName8"));
        }
        
        [Test]
        public void ProductEditPriceDeleteAll()
        {
            GoToAdmin("product/edit/9");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Цена и наличие')]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("//a[contains(text(), 'Добавить цену')]")).Click();
            Thread.Sleep(1000);
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"AddSize\"]")))).SelectByText("SizeName1");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"AddColor\"]")))).SelectByText("Color1");
            driver.FindElement(By.CssSelector("[data-e2e=\"AddPrice\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"AddPrice\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"AddPrice\"]")).SendKeys("999");
            driver.FindElement(By.CssSelector("[data-e2e=\"AddAmount\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"AddAmount\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"AddAmount\"]")).SendKeys("999");
            DropFocus("h2");

            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(3000);

            GoToAdmin("product/edit/9");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Цена и наличие')]")).Click();
            Thread.Sleep(1000);

            Assert.IsTrue(GetGridCell(1, "Size", "Offers").FindElement(By.CssSelector("[data-e2e=\"itemSelected\"]")).Text.Contains("SizeName1"));
            Assert.IsTrue(GetGridCell(0, "Size", "Offers").FindElement(By.CssSelector("[data-e2e=\"itemSelected\"]")).Text.Contains("SizeName9"));

            //check delete price
            GoToAdmin("product/edit/9");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Цена и наличие')]")).Click();
            Thread.Sleep(1000);
            GetGridCell(1, "_serviceColumn", "Offers").Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);
            GetGridCell(0, "_serviceColumn", "Offers").Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("product/edit/9");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Цена и наличие')]")).Click();
            Thread.Sleep(1000);

            Assert.IsTrue(driver.PageSource.Contains("У товара нет цен"));
            Assert.IsTrue(driver.FindElement(By.XPath("//a[contains(text(), 'Добавить цену')]")).Enabled);
            Assert.AreEqual(0, driver.FindElements(By.CssSelector("[data-e2e=\"gridRow\"]")).Count);

            //check client
            GoToClient("products/test-product9");
            Assert.IsFalse(driver.PageSource.Contains("SizeName1"));
            Assert.IsFalse(driver.PageSource.Contains("SizeName9"));
        }

        [Test]
        public void ProductEditPriceGoToSizesColors()
        {
            GoToAdmin("product/edit/10");

            ScrollTo(By.Id("CurrencyId"));
            driver.FindElement(By.LinkText("размеров")).Click();
            Thread.Sleep(4000);

            //focus to second browser tab
            ReadOnlyCollection<String> windowHandles = driver.WindowHandles;
            String AdminProduct = (String)windowHandles[0];
            String AdminSizes = windowHandles[windowHandles.Count - 1];
            driver.SwitchTo().Window(AdminSizes);
            Assert.IsTrue(driver.FindElement(By.TagName("h1")).Text.Contains("Размеры"));
            Assert.IsTrue(driver.Url.Contains("sizes"));

            //close tab and focus to first tab
            driver.Close();
            driver.SwitchTo().Window(AdminProduct);
            
           // ScrollTo(By.Id("CurrencyId"));
            driver.FindElement(By.LinkText("цветов")).Click();
            Thread.Sleep(4000);

            //focus to next browser tab
            windowHandles = driver.WindowHandles;
            String AdminColors = windowHandles[windowHandles.Count - 1];
            driver.SwitchTo().Window(AdminColors);
            Assert.IsTrue(driver.FindElement(By.TagName("h1")).Text.Contains("Цвета"));
            Assert.IsTrue(driver.Url.Contains("colors"));

            //close tab
            driver.Close();
            driver.SwitchTo().Window(AdminProduct);
        }

        [Test]
        public void ProductEditPriceChangeMain()
        {
            GoToAdmin("product/edit/11");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Цена и наличие')]")).Click();
            Thread.Sleep(1000);

            Assert.IsTrue(GetGridCell(0, "Main", "Offers").FindElement(By.CssSelector("[data-e2e=\"switchOnOffSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(0, "Size", "Offers").FindElement(By.CssSelector("[data-e2e=\"itemSelected\"]")).Text.Contains("SizeName11"));

            //scroll
            driver.FindElement(By.XPath("//a[contains(text(), 'Добавить цену')]")).Click();
            Thread.Sleep(1000);
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"AddSize\"]")))).SelectByText("SizeName1");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"AddColor\"]")))).SelectByText("Color1");
            driver.FindElement(By.CssSelector("[data-e2e=\"AddPrice\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"AddPrice\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"AddPrice\"]")).SendKeys("1111");
            driver.FindElement(By.CssSelector("[data-e2e=\"AddAmount\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"AddAmount\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"AddAmount\"]")).SendKeys("1111");
            DropFocus("h2");

            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(2000);

            GoToAdmin("product/edit/11");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Цена и наличие')]")).Click();
            Thread.Sleep(1000);

            Assert.IsTrue(GetGridCell(0, "Main", "Offers").FindElement(By.CssSelector("[data-e2e=\"switchOnOffSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(0, "Size", "Offers").FindElement(By.CssSelector("[data-e2e=\"itemSelected\"]")).Text.Contains("SizeName11"));
            Assert.IsFalse(GetGridCell(1, "Main", "Offers").FindElement(By.CssSelector("[data-e2e=\"switchOnOffSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(1, "Size", "Offers").FindElement(By.CssSelector("[data-e2e=\"itemSelected\"]")).Text.Contains("SizeName1"));

            //check change main price 
            GetGridCell(1, "Main", "Offers").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Click();
            Thread.Sleep(1000);

            GoToAdmin("product/edit/11");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Цена и наличие')]")).Click();
            Thread.Sleep(1000);

            //check admin
            Assert.IsFalse(GetGridCell(0, "Main", "Offers").FindElement(By.CssSelector("[data-e2e=\"switchOnOffSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(1, "Main", "Offers").FindElement(By.CssSelector("[data-e2e=\"switchOnOffSelect\"]")).Selected);

            //check client
            GoToClient("products/test-product11");
            Assert.IsTrue(driver.PageSource.Contains("SizeName1"));
            Assert.AreEqual("true", driver.FindElement(By.CssSelector("[title=\"SizeName11\"]")).FindElement(By.TagName("input")).GetAttribute("disabled"));
        }


        [Test]
        public void ProductEditPriceInplace()
        {
            GoToAdmin("product/edit/12");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Цена и наличие')]")).Click();
            Thread.Sleep(1000);

            Assert.IsTrue(GetGridCell(0, "Size", "Offers").FindElement(By.CssSelector("[data-e2e=\"itemSelected\"]")).Text.Contains("SizeName12"));
            Assert.IsTrue(GetGridCell(0, "Color", "Offers").FindElement(By.CssSelector("[data-e2e=\"itemSelected\"]")).Text.Contains("Color12"));
            Assert.AreEqual("12", GetGridCell(0, "ArtNo", "Offers").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("12", GetGridCell(0, "BasePrice", "Offers").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("12", GetGridCell(0, "SupplyPrice", "Offers").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("12", GetGridCell(0, "Amount", "Offers").FindElement(By.TagName("input")).GetAttribute("value"));

            //check inplace edit
            GetGridCell(0, "ArtNo", "Offers").FindElement(By.TagName("input")).Click();
            GetGridCell(0, "ArtNo", "Offers").FindElement(By.TagName("input")).Clear();
            GetGridCell(0, "ArtNo", "Offers").FindElement(By.TagName("input")).SendKeys("121212");
            XPathContainsText("h2", "Цена и наличие");
            GetGridCell(0, "Size", "Offers").FindElement(By.CssSelector("[data-e2e=\"select\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".ui-select-choices.ui-select-choices-content.ui-select-dropdown.dropdown-menu")).FindElement(By.XPath("//span[contains(text(), 'SizeName5')]")).Click();
            Thread.Sleep(2000);
            GetGridCell(0, "Color", "Offers").FindElement(By.CssSelector("[data-e2e=\"select\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".ui-select-choices.ui-select-choices-content.ui-select-dropdown.dropdown-menu")).FindElement(By.XPath("//span[contains(text(), 'Color5')]")).Click();
            Thread.Sleep(2000);
            GetGridCell(0, "BasePrice", "Offers").FindElement(By.TagName("input")).Click();
            GetGridCell(0, "BasePrice", "Offers").FindElement(By.TagName("input")).Clear();
            GetGridCell(0, "BasePrice", "Offers").FindElement(By.TagName("input")).SendKeys("121212");
            XPathContainsText("h2", "Цена и наличие");
            GetGridCell(0, "SupplyPrice", "Offers").FindElement(By.TagName("input")).Click();
            GetGridCell(0, "SupplyPrice", "Offers").FindElement(By.TagName("input")).Clear();
            GetGridCell(0, "SupplyPrice", "Offers").FindElement(By.TagName("input")).SendKeys("121212");
            XPathContainsText("h2", "Цена и наличие");
            GetGridCell(0, "Amount", "Offers").FindElement(By.TagName("input")).Click();
            GetGridCell(0, "Amount", "Offers").FindElement(By.TagName("input")).Clear();
            GetGridCell(0, "Amount", "Offers").FindElement(By.TagName("input")).SendKeys("121212");
            XPathContainsText("h2", "Цена и наличие");

            //check admin
            GoToAdmin("product/edit/12");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Цена и наличие')]")).Click();
            Thread.Sleep(1000);

            Assert.IsTrue(GetGridCell(0, "Size", "Offers").FindElement(By.CssSelector("[data-e2e=\"itemSelected\"]")).Text.Contains("SizeName5"));
            Assert.IsTrue(GetGridCell(0, "Color", "Offers").FindElement(By.CssSelector("[data-e2e=\"itemSelected\"]")).Text.Contains("Color5"));
            Assert.AreEqual("121212", GetGridCell(0, "ArtNo", "Offers").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("121212", GetGridCell(0, "BasePrice", "Offers").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("121212", GetGridCell(0, "SupplyPrice", "Offers").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("121212", GetGridCell(0, "Amount", "Offers").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridRow\"]")).Count == 1);

            //check client
            GoToClient("products/test-product12");
            driver.FindElement(By.CssSelector(".details-row.details-sku")).Text.Contains("121212");
            driver.FindElement(By.CssSelector(".price")).Text.Contains("121212");
            Assert.IsTrue(driver.PageSource.Contains("SizeName5"));
            Assert.IsTrue(driver.PageSource.Contains("Color5"));
            Assert.IsFalse(driver.PageSource.Contains("SizeName12"));
            Assert.IsFalse(driver.PageSource.Contains("Color12"));
        }

        [Test]
        public void ProductEditPriceAddUnit()
        {
            GoToAdmin("product/edit/13");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Цена и наличие')]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.Id("Unit")).Click();
            driver.FindElement(By.Id("Unit")).Clear();
            driver.FindElement(By.Id("Unit")).SendKeys("км");

            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("product/edit/13");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Цена и наличие')]")).Click();
            Thread.Sleep(1000);

            Assert.AreEqual("км", driver.FindElement(By.Id("Unit")).GetAttribute("value"));

            //check client
            GoToClient("products/test-product13");
            Assert.IsTrue(driver.PageSource.Contains("км"));
        }

        [Test]
        public void ProductEditPriceEditUnit()
        {
            GoToAdmin("product/edit/14");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Цена и наличие')]")).Click();
            Thread.Sleep(1000);

            Assert.AreEqual("unit", driver.FindElement(By.Id("Unit")).GetAttribute("value"));

            driver.FindElement(By.Id("Unit")).Click();
            driver.FindElement(By.Id("Unit")).Clear();
            driver.FindElement(By.Id("Unit")).SendKeys("км");

            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("product/edit/14");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Цена и наличие')]")).Click();
            Thread.Sleep(1000);

            Assert.AreEqual("км", driver.FindElement(By.Id("Unit")).GetAttribute("value"));

            //check client
            Assert.IsTrue(driver.PageSource.Contains("км"));
        }
        
        [Test]
        public void ProductEditPriceAddMiMaxAmount()
        {
            GoToAdmin("product/edit/15");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Цена и наличие')]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.Id("MinAmount")).Click();
            driver.FindElement(By.Id("MinAmount")).Clear();
            driver.FindElement(By.Id("MinAmount")).SendKeys("10");
            driver.FindElement(By.Id("MaxAmount")).Click();
            driver.FindElement(By.Id("MaxAmount")).Clear();
            driver.FindElement(By.Id("MaxAmount")).SendKeys("15");

            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("product/edit/15");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Цена и наличие')]")).Click();
            Thread.Sleep(1000);

            Assert.AreEqual("10", driver.FindElement(By.Id("MinAmount")).GetAttribute("value"));
            Assert.AreEqual("15", driver.FindElement(By.Id("MaxAmount")).GetAttribute("value"));

            //check client
            GoToClient("products/test-product15");
            Assert.AreEqual("10", driver.FindElement(By.CssSelector(".details-spinbox-block input")).GetAttribute("min"));
            Assert.AreEqual("15", driver.FindElement(By.CssSelector(".details-spinbox-block input")).GetAttribute("max"));
        }

        [Test]
        public void ProductEditPriceEditMultiplicity()
        {
            GoToAdmin("product/edit/16");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Цена и наличие')]")).Click();
            Thread.Sleep(1000);

            Assert.AreEqual("1", driver.FindElement(By.Id("Multiplicity")).GetAttribute("value"));
            
            driver.FindElement(By.Id("Multiplicity")).Click();
            driver.FindElement(By.Id("Multiplicity")).Clear();
            driver.FindElement(By.Id("Multiplicity")).SendKeys("100");

            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("product/edit/16");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Цена и наличие')]")).Click();
            Thread.Sleep(1000);

            Assert.AreEqual("100", driver.FindElement(By.Id("Multiplicity")).GetAttribute("value"));
        }
    }
 }