using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;
using AdvantShop.Selenium.Core.Infrastructure;

namespace AdvantShop.SeleniumTest.Admin.Catalog.ProductAddEdit.Additional
{
    [TestFixture]
    public class ProductAddEditAdditional : BaseMultiSeleniumTest
    {
        [SetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(
           "data\\Admin\\Catalog\\ProductAddEdit\\Additional\\Catalog.Product.csv",
           "data\\Admin\\Catalog\\ProductAddEdit\\Additional\\Catalog.Offer.csv",
           "data\\Admin\\Catalog\\ProductAddEdit\\Additional\\Catalog.Category.csv",
           "data\\Admin\\Catalog\\ProductAddEdit\\Additional\\Catalog.ProductCategories.csv"
           );

            Init();
            
            GoToAdmin("product/edit/101");
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//div[contains(text(), 'Свойства товара')]")).Click();
            Thread.Sleep(3000);

            driver.FindElement(By.Id("customoptions")).FindElement(By.TagName("ui-modal-trigger")).Click();
            Thread.Sleep(2000);
            
            var iframe = driver.FindElement(By.TagName("iframe"));
            driver.SwitchTo().Frame(iframe);
            Thread.Sleep(2000);
            WaitForElem(By.Id("ctl00_cphMain_productCustomOption_btnAddCustomOption"));
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_btnAddCustomOption")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_txtTitle")).SendKeys("New custom options 6");
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_txtSortOrder")).Clear();
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_txtSortOrder")).SendKeys("6");
            (new SelectElement(driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_ddlInputType")))).SelectByText("Выпадающий список");
            Thread.Sleep(1000);

            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl02_txtTitle")).Clear();
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl02_txtTitle")).SendKeys("select button1");
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl02_txtBasePrice")).Clear();
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl02_txtBasePrice")).SendKeys("300");
            (new SelectElement(driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl02_ddlPriceType")))).SelectByText("Фиксированная");
            Thread.Sleep(1000);
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl02_txtSortOrder")).Clear();
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl02_txtSortOrder")).SendKeys("1");
            
            driver.SwitchTo().DefaultContent();
            ScrollTo(By.CssSelector(".btn.btn-save.btn-primary"));
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            Thread.Sleep(5000);
        }
         

        [Test]
        public void ProductEditAdditional()
        {
            Functions.DelProductCart(driver, baseURL);
            GoToAdmin("product/edit/2");
            testname = "ProductEditAdditional";
            VerifyBegin(testname);
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//div[contains(text(), 'Свойства товара')]")).Click();
            Thread.Sleep(3000);

            driver.FindElement(By.Id("customoptions")).FindElement(By.TagName("ui-modal-trigger")).Click();
            Thread.Sleep(1000);
            VerifyIsTrue(driver.FindElements(By.CssSelector(".modal-dialog")).Count == 1, "view custom options modal dialog");

            VerifyIsTrue(driver.FindElements(By.TagName("iframe")).Count > 0, "iframe on page");
            VerifyIsTrue(driver.FindElement(By.TagName("iframe")).GetAttribute("src").ToString().Contains("productCustomOptions.aspx"), "src page");

            var iframe = driver.FindElement(By.TagName("iframe"));
            driver.SwitchTo().Frame(iframe);
            Thread.Sleep(1000);
            WaitForElem(By.Id("ctl00_cphMain_productCustomOption_btnAddCustomOption"));
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_btnAddCustomOption")).Click();
            Thread.Sleep(2000);
            VerifyIsTrue(driver.FindElements(By.CssSelector(".option-box")).Count == 1, "view custom options box");

            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_txtTitle")).SendKeys("New custom options 11");
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_cbIsRequired")).Click();
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_txtSortOrder")).Clear();
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_txtSortOrder")).SendKeys("10");
            (new SelectElement(driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_ddlInputType")))).SelectByText("Однострочное текстовое поле");
            Thread.Sleep(2000);
            driver.SwitchTo().DefaultContent();
            ScrollTo(By.CssSelector(".btn.btn-save.btn-primary"));
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            Thread.Sleep(5000);

            //  ScrollTo(By.Id("header-top"));
            //  GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.Id("customoptions")).FindElement(By.TagName("ui-modal-trigger")).Click();
            Thread.Sleep(1000);
            VerifyFinally(testname);

        }
        [Test]
       public void ProductEditAdditionalTextField()
        {
            Functions.DelProductCart(driver, baseURL);
            GoToAdmin("product/edit/9");
            testname = "ProductEditAdditionalTextField";
            VerifyBegin(testname);
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//div[contains(text(), 'Свойства товара')]")).Click();
            Thread.Sleep(3000);

            driver.FindElement(By.Id("customoptions")).FindElement(By.TagName("ui-modal-trigger")).Click();
            Thread.Sleep(1000);
            VerifyIsTrue(driver.FindElements(By.CssSelector(".modal-dialog")).Count == 1, "view custom options modal dialog");

            VerifyIsTrue(driver.FindElements(By.TagName("iframe")).Count > 0, "iframe on page");
            VerifyIsTrue(driver.FindElement(By.TagName("iframe")).GetAttribute("src").ToString().Contains("productCustomOptions.aspx"), "src page");

            var iframe = driver.FindElement(By.TagName("iframe"));
            driver.SwitchTo().Frame(iframe);
            Thread.Sleep(1000);
            WaitForElem(By.Id("ctl00_cphMain_productCustomOption_btnAddCustomOption"));
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_btnAddCustomOption")).Click();
            Thread.Sleep(2000);
            VerifyIsTrue(driver.FindElements(By.CssSelector(".option-box")).Count == 1, "view custom options box");

            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_txtTitle")).SendKeys("New custom options 1");
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_cbIsRequired")).Click();
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_txtSortOrder")).Clear();
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_txtSortOrder")).SendKeys("10");
            (new SelectElement(driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_ddlInputType")))).SelectByText("Однострочное текстовое поле");
            Thread.Sleep(2000);
            driver.SwitchTo().DefaultContent();
            ScrollTo(By.CssSelector(".btn.btn-save.btn-primary"));
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            Thread.Sleep(5000);

            GoToClient("products/test-product9");
            ScrollTo(By.ClassName("rating"));
            Thread.Sleep(3000);
            VerifyIsTrue(driver.FindElements(By.CssSelector(".custom-options-list")).Count == 1, "view custom options list");
            VerifyAreEqual("New custom options 1", driver.FindElement(By.CssSelector(".custom-options-name")).Text, " custom options name");
            VerifyIsTrue(driver.FindElement(By.Name("customOptionsForm")).FindElements(By.CssSelector(".custom-options-value")).Count == 1, "product view custom options field");

            driver.FindElement(By.Name("customOptionsForm")).FindElement(By.CssSelector(".custom-options-value input")).Click();
            driver.FindElement(By.Name("customOptionsForm")).FindElement(By.CssSelector(".custom-options-value input")).SendKeys("test custom options1");
            Thread.Sleep(5000);
            driver.FindElement(By.CssSelector(".btn.btn-middle.btn-confirm.icon-bag-before")).Click();
            Thread.Sleep(5000);

            GoToClient("cart");
            VerifyAreEqual("New custom options 1:", driver.FindElement(By.CssSelector(".cart-full-properties-name")).Text, " cart custom options name");
            VerifyAreEqual("test custom options1", driver.FindElement(By.CssSelector(".cart-full-properties-value")).Text, " cart custom options value");

            GoToClient("checkout");
            Thread.Sleep(2000);
            VerifyAreEqual("New custom options 1:", driver.FindElement(By.CssSelector(".cart-full-properties-name")).Text, " checkout custom options name");
            VerifyAreEqual("test custom options1", driver.FindElement(By.CssSelector(".cart-full-properties-value")).Text, " checkout custom options value");
            
            VerifyFinally(testname);

        }

        [Test]
        public void ProductEditAdditionalBigTextField()
        {
            Functions.DelProductCart(driver, baseURL);
            GoToAdmin("product/edit/4");
            testname = "ProductEditAdditionalBigTextField";
            VerifyBegin(testname);
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//div[contains(text(), 'Свойства товара')]")).Click();
            Thread.Sleep(3000);

            driver.FindElement(By.Id("customoptions")).FindElement(By.TagName("ui-modal-trigger")).Click();
            VerifyIsTrue(driver.FindElements(By.CssSelector(".modal-dialog")).Count == 1, "view custom options modal dialog");

            VerifyIsTrue(driver.FindElements(By.TagName("iframe")).Count > 0, "iframe on page");
            VerifyIsTrue(driver.FindElement(By.TagName("iframe")).GetAttribute("src").ToString().Contains("productCustomOptions.aspx"), "src page");

            var iframe = driver.FindElement(By.TagName("iframe"));
            driver.SwitchTo().Frame(iframe);
            Thread.Sleep(1000);
            WaitForElem(By.Id("ctl00_cphMain_productCustomOption_btnAddCustomOption"));
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_btnAddCustomOption")).Click();
            Thread.Sleep(3000);
            VerifyIsTrue(driver.FindElements(By.CssSelector(".option-box")).Count == 1, "view custom options box");

            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_txtTitle")).SendKeys("New custom options 2");
            Thread.Sleep(1000);
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_txtSortOrder")).Clear();
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_txtSortOrder")).SendKeys("9");
            (new SelectElement(driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_ddlInputType")))).SelectByText("Многострочное поле ввода");
            Thread.Sleep(2000);

            driver.SwitchTo().DefaultContent();
            ScrollTo(By.CssSelector(".btn.btn-save.btn-primary"));
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            Thread.Sleep(5000);

            GoToClient("products/test-product4");
            ScrollTo(By.ClassName("rating"));
            VerifyIsTrue(driver.FindElements(By.CssSelector(".custom-options-list")).Count == 1, "view custom options list");
            VerifyAreEqual("New custom options 2", driver.FindElement(By.CssSelector(".custom-options-name")).Text, " custom options name");
            VerifyIsTrue(driver.FindElement(By.Name("customOptionsForm")).FindElements(By.CssSelector(".custom-options-value")).Count == 1, "product view custom options field");

            driver.FindElement(By.Name("customOptionsForm")).FindElement(By.CssSelector(".custom-options-value textarea")).Click();
            driver.FindElement(By.Name("customOptionsForm")).FindElement(By.CssSelector(".custom-options-value textarea")).SendKeys("test custom options2");
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".btn.btn-middle.btn-confirm.icon-bag-before")).Click();
            Thread.Sleep(5000);

            GoToClient("cart");
            VerifyAreEqual("New custom options 2:", driver.FindElement(By.CssSelector(".cart-full-properties-name")).Text, " cart custom options name");
            VerifyAreEqual("test custom options2", driver.FindElement(By.CssSelector(".cart-full-properties-value")).Text, " cart custom options value");

            GoToClient("checkout");
            VerifyAreEqual("New custom options 2:", driver.FindElement(By.CssSelector(".cart-full-properties-name")).Text, " checkout custom options name");
            VerifyAreEqual("test custom options2", driver.FindElement(By.CssSelector(".cart-full-properties-value")).Text, " checkout custom options value");
        
            VerifyFinally(testname);

        }
        [Test]
        public void ProductEditAdditionalCheckBox()
        {
            Functions.DelProductCart(driver, baseURL);
            GoToAdmin("product/edit/5");
            testname = "ProductEditAdditionalCheckBox";
            VerifyBegin(testname);
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//div[contains(text(), 'Свойства товара')]")).Click();
            Thread.Sleep(3000);

            driver.FindElement(By.Id("customoptions")).FindElement(By.TagName("ui-modal-trigger")).Click();
            VerifyIsTrue(driver.FindElements(By.CssSelector(".modal-dialog")).Count == 1, "view custom options modal dialog");

            VerifyIsTrue(driver.FindElements(By.TagName("iframe")).Count > 0, "iframe on page");
            VerifyIsTrue(driver.FindElement(By.TagName("iframe")).GetAttribute("src").ToString().Contains("productCustomOptions.aspx"), "src page");

            var iframe = driver.FindElement(By.TagName("iframe"));
            driver.SwitchTo().Frame(iframe);
            Thread.Sleep(1000);
            WaitForElem(By.Id("ctl00_cphMain_productCustomOption_btnAddCustomOption"));
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_btnAddCustomOption")).Click();
            Thread.Sleep(3000);
            VerifyIsTrue(driver.FindElements(By.CssSelector(".option-box")).Count == 1, "view custom options box");

            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_txtTitle")).SendKeys("New custom options 3");
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_txtSortOrder")).Clear();
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_txtSortOrder")).SendKeys("8");
            (new SelectElement(driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_ddlInputType")))).SelectByText("Галочка");
            Thread.Sleep(2000);

            driver.FindElement(By.Name("ctl00$cphMain$productCustomOption$rCustomOptions$ctl00$txtBasePrice")).Clear();
            driver.FindElement(By.Name("ctl00$cphMain$productCustomOption$rCustomOptions$ctl00$txtBasePrice")).SendKeys("200");
            (new SelectElement(driver.FindElement(By.Name("ctl00$cphMain$productCustomOption$rCustomOptions$ctl00$ddlPriceType")))).SelectByText("Фиксированная");
            Thread.Sleep(2000);

            driver.SwitchTo().DefaultContent();
            ScrollTo(By.CssSelector(".btn.btn-save.btn-primary"));
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            Thread.Sleep(5000);

            //ScrollTo(By.Id("header-top"));
            //GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            GoToClient("products/test-product5");
            ScrollTo(By.ClassName("rating"));
            VerifyIsTrue(driver.FindElements(By.CssSelector(".custom-options-list")).Count == 1, "view custom options list");
            VerifyAreEqual("New custom options 3", driver.FindElement(By.CssSelector(".custom-options-name")).Text, " custom options name");
            VerifyIsTrue(driver.FindElements(By.CssSelector(".custom-input-checkbox")).Count == 1, "view custom options checkbox");
            VerifyAreEqual("+200 руб.", driver.FindElement(By.CssSelector(".custom-input-text")).Text, " custom options text price");
            driver.FindElement(By.CssSelector(".custom-input-checkbox")).Click();
            Thread.Sleep(1000);
            VerifyAreEqual("205", driver.FindElement(By.CssSelector(".price-number")).Text, " cart custom options rezult price");
            driver.FindElement(By.CssSelector(".btn.btn-middle.btn-confirm.icon-bag-before")).Click();
            Thread.Sleep(5000);

            GoToClient("cart");
            VerifyAreEqual("New custom options 3:", driver.FindElement(By.CssSelector(".cart-full-properties-name")).Text, " cart custom options name");
            VerifyAreEqual("+ 200 руб.", driver.FindElement(By.CssSelector(".cart-full-properties-value")).Text, " cart custom options value");

            GoToClient("checkout");
            VerifyAreEqual("New custom options 3:", driver.FindElement(By.CssSelector(".cart-full-properties-name")).Text, " checkout custom options name");
            VerifyAreEqual("+ 200 руб.", driver.FindElement(By.CssSelector(".cart-full-properties-value")).Text, " checkout custom options value");
            
            VerifyFinally(testname);

        }
        [Test]
        public void ProductEditAdditionalRadioButton()
        {
            Functions.DelProductCart(driver, baseURL);
            GoToAdmin("product/edit/6");
            testname = "ProductEditAdditionalRadioButton";
            VerifyBegin(testname);
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//div[contains(text(), 'Свойства товара')]")).Click();
            Thread.Sleep(3000);

            driver.FindElement(By.Id("customoptions")).FindElement(By.TagName("ui-modal-trigger")).Click();
            VerifyIsTrue(driver.FindElements(By.CssSelector(".modal-dialog")).Count == 1, "view custom options modal dialog");

            VerifyIsTrue(driver.FindElements(By.TagName("iframe")).Count > 0, "iframe on page");
            VerifyIsTrue(driver.FindElement(By.TagName("iframe")).GetAttribute("src").ToString().Contains("productCustomOptions.aspx"), "src page");

            var iframe = driver.FindElement(By.TagName("iframe"));
            driver.SwitchTo().Frame(iframe);
            Thread.Sleep(1000);
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_btnAddCustomOption")).Click();
            Thread.Sleep(3000);
            VerifyIsTrue(driver.FindElements(By.CssSelector(".option-box")).Count == 1, "view custom options box");

            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_txtTitle")).SendKeys("New custom options 4");
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_txtSortOrder")).Clear();
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_txtSortOrder")).SendKeys("7");
            (new SelectElement(driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_ddlInputType")))).SelectByText("Радио кнопки");
            Thread.Sleep(2000);
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl02_txtTitle")).Clear();
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl02_txtTitle")).SendKeys("radio button1");
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl02_txtBasePrice")).Clear();
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl02_txtBasePrice")).SendKeys("300");
            (new SelectElement(driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl02_ddlPriceType")))).SelectByText("Фиксированная");
            Thread.Sleep(2000);
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl02_txtSortOrder")).Clear();
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl02_txtSortOrder")).SendKeys("1");

            //ScrollTo(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_btnAdd"));
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_btnAdd")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl03_txtTitle")).Clear();
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl03_txtTitle")).SendKeys("radio button2");
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl03_txtBasePrice")).Clear();
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl03_txtBasePrice")).SendKeys("100");
            (new SelectElement(driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl03_ddlPriceType")))).SelectByText("Процент");
            Thread.Sleep(2000);
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl03_txtSortOrder")).Clear();
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl03_txtSortOrder")).SendKeys("2");

            ScrollTo(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_btnAdd"));
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_btnAdd")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl04_txtTitle")).Clear();
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl04_txtTitle")).SendKeys("radio button5");
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl04_txtBasePrice")).Clear();
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl04_txtBasePrice")).SendKeys("50000");
            (new SelectElement(driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl04_ddlPriceType")))).SelectByText("Фиксированная");
            Thread.Sleep(2000);
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl04_txtSortOrder")).Clear();
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl04_txtSortOrder")).SendKeys("3");

            ScrollTo(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_btnAdd"));
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_btnAdd")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl05_txtTitle")).Clear();
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl05_txtTitle")).SendKeys("radio button3");
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl05_txtBasePrice")).Clear();
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl05_txtBasePrice")).SendKeys("500");
            (new SelectElement(driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl05_ddlPriceType")))).SelectByText("Фиксированная");
            Thread.Sleep(2000);
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl05_txtSortOrder")).Clear();
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl05_txtSortOrder")).SendKeys("3");
            Thread.Sleep(1000);

            ScrollTo(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl04_lbDelete"));
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl04_lbDelete")).Click();
            Thread.Sleep(3000);

            driver.SwitchTo().DefaultContent();
            ScrollTo(By.CssSelector(".btn.btn-save.btn-primary"));
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            Thread.Sleep(5000);

            GoToClient("products/test-product6");
            ScrollTo(By.ClassName("rating"));
            Thread.Sleep(3000);
            VerifyIsTrue(driver.FindElements(By.CssSelector(".custom-options-list")).Count == 1, "view custom options list");
            VerifyAreEqual("New custom options 4", driver.FindElement(By.CssSelector(".custom-options-name")).Text, " custom options name");

            VerifyIsTrue(driver.FindElements(By.CssSelector(".custom-options-value .custom-input-radio")).Count == 4, "view custom options field");

            VerifyAreEqual("6", driver.FindElement(By.CssSelector(".price-number")).Text, " cart custom options rezult price  with 0 option");
            driver.FindElements(By.CssSelector(".custom-options-value  .custom-input-radio"))[1].Click();
            VerifyAreEqual("306", driver.FindElement(By.CssSelector(".price-number")).Text, " cart custom options rezult price with 1 option");

            driver.FindElements(By.CssSelector(".custom-options-value  .custom-input-radio"))[2].Click();
            VerifyAreEqual("12", driver.FindElement(By.CssSelector(".price-number")).Text, " cart custom options rezult price  with 2 option");

            driver.FindElements(By.CssSelector(".custom-options-value  .custom-input-radio"))[3].Click();
            VerifyAreEqual("506", driver.FindElement(By.CssSelector(".price-number")).Text, " cart custom options rezult price with 3 option");

            driver.FindElement(By.CssSelector(".btn.btn-middle.btn-confirm.icon-bag-before")).Click();
            Thread.Sleep(5000);

            GoToClient("cart");
            VerifyAreEqual("New custom options 4:", driver.FindElement(By.CssSelector(".cart-full-properties-name")).Text, " cart custom options name");
            VerifyAreEqual("radio button3 + 500 руб.", driver.FindElement(By.CssSelector(".cart-full-properties-value")).Text, " cart custom options value");

            GoToClient("checkout");
            VerifyAreEqual("New custom options 4:", driver.FindElement(By.CssSelector(".cart-full-properties-name")).Text, " checkout custom options name");
            VerifyAreEqual("radio button3 + 500 руб.", driver.FindElement(By.CssSelector(".cart-full-properties-value")).Text, " checkout custom options value");
            
            VerifyFinally(testname);

        }
        [Test]
        public void ProductEditAdditionalSelect()
        {
            Functions.DelProductCart(driver, baseURL);
            GoToAdmin("product/edit/7");
            testname = "ProductEditAdditionalSelect";
            VerifyBegin(testname);
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//div[contains(text(), 'Свойства товара')]")).Click();
            Thread.Sleep(3000);

            driver.FindElement(By.Id("customoptions")).FindElement(By.TagName("ui-modal-trigger")).Click();
            VerifyIsTrue(driver.FindElements(By.CssSelector(".modal-dialog")).Count == 1, "view custom options modal dialog");

            VerifyIsTrue(driver.FindElements(By.TagName("iframe")).Count > 0, "iframe on page");
            VerifyIsTrue(driver.FindElement(By.TagName("iframe")).GetAttribute("src").ToString().Contains("productCustomOptions.aspx"), "src page");

            var iframe = driver.FindElement(By.TagName("iframe"));
            driver.SwitchTo().Frame(iframe);
            Thread.Sleep(1000);
            WaitForElem(By.Id("ctl00_cphMain_productCustomOption_btnAddCustomOption"));
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_btnAddCustomOption")).Click();
            Thread.Sleep(3000);
            VerifyIsTrue(driver.FindElements(By.CssSelector(".option-box")).Count == 1, "view custom options box");

            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_txtTitle")).SendKeys("New custom options 5");
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_txtSortOrder")).Clear();
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_txtSortOrder")).SendKeys("6");
            (new SelectElement(driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_ddlInputType")))).SelectByText("Выпадающий список");
            Thread.Sleep(2000);

            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl02_txtTitle")).Clear();
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl02_txtTitle")).SendKeys("select button1");
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl02_txtBasePrice")).Clear();
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl02_txtBasePrice")).SendKeys("300");
            (new SelectElement(driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl02_ddlPriceType")))).SelectByText("Фиксированная");
            Thread.Sleep(2000);
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl02_txtSortOrder")).Clear();
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl02_txtSortOrder")).SendKeys("1");

            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_btnAdd")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl03_txtTitle")).Clear();
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl03_txtTitle")).SendKeys("select button2");
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl03_txtBasePrice")).Clear();
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl03_txtBasePrice")).SendKeys("100");
            (new SelectElement(driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl03_ddlPriceType")))).SelectByText("Процент");
            Thread.Sleep(2000);
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl03_txtSortOrder")).Clear();
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl03_txtSortOrder")).SendKeys("2");

            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_btnAdd")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl04_txtTitle")).Clear();
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl04_txtTitle")).SendKeys("select button3");
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl04_txtBasePrice")).Clear();
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl04_txtBasePrice")).SendKeys("500");
            (new SelectElement(driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl04_ddlPriceType")))).SelectByText("Фиксированная");
            Thread.Sleep(2000);
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl04_txtSortOrder")).Clear();
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl04_txtSortOrder")).SendKeys("3");

            driver.SwitchTo().DefaultContent();
            ScrollTo(By.CssSelector(".btn.btn-save.btn-primary"));
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            Thread.Sleep(5000);

            GoToClient("products/test-product7");
            ScrollTo(By.ClassName("rating"));
            Thread.Sleep(3000);
            VerifyIsTrue(driver.FindElements(By.CssSelector(".custom-options-list")).Count == 1, "view custom options list");
            VerifyAreEqual("New custom options 5", driver.FindElement(By.CssSelector(".custom-options-name")).Text, " custom options name");
            VerifyIsTrue(driver.FindElements(By.CssSelector(".custom-options-value select")).Count == 1, "view custom options field");

            VerifyAreEqual("7", driver.FindElement(By.CssSelector(".price-number")).Text, " cart custom options rezult price  with 0 option");
            (new SelectElement(driver.FindElement(By.CssSelector(".custom-options-value select")))).SelectByText("Не выбрано");
            Thread.Sleep(1000);
            VerifyAreEqual("7", driver.FindElement(By.CssSelector(".price-number")).Text, " cart custom options rezult price  with 0 option");
            (new SelectElement(driver.FindElement(By.CssSelector(".custom-options-value select")))).SelectByText("select button1 +300 руб.");
            Thread.Sleep(1000);
         //   VerifyAreEqual("307", driver.FindElement(By.CssSelector(".price-number")).Text, " cart custom options rezult price with 1 option");

            (new SelectElement(driver.FindElement(By.CssSelector(".custom-options-value select")))).SelectByText("select button2 +100%");
            Thread.Sleep(1000);
            VerifyAreEqual("14", driver.FindElement(By.CssSelector(".price-number")).Text, " cart custom options rezult price  with 2 option");

            (new SelectElement(driver.FindElement(By.CssSelector(".custom-options-value select")))).SelectByText("select button3 +500 руб.");
            Thread.Sleep(1000);
            VerifyAreEqual("507", driver.FindElement(By.CssSelector(".price-number")).Text, " cart custom options rezult price with 3 option");

            driver.FindElement(By.CssSelector(".btn.btn-middle.btn-confirm.icon-bag-before")).Click();
            Thread.Sleep(5000);

            GoToClient("cart");
            VerifyAreEqual("New custom options 5:", driver.FindElement(By.CssSelector(".cart-full-properties-name")).Text, " cart custom options name");
            VerifyAreEqual("select button3 + 500 руб.", driver.FindElement(By.CssSelector(".cart-full-properties-value")).Text, " cart custom options value");

            GoToClient("checkout");
            VerifyAreEqual("New custom options 5:", driver.FindElement(By.CssSelector(".cart-full-properties-name")).Text, " checkout custom options name");
            VerifyAreEqual("select button3 + 500 руб.", driver.FindElement(By.CssSelector(".cart-full-properties-value")).Text, " checkout custom options value");
            
            VerifyFinally(testname);
        }
        [Test]
        public void ProductEditAdditionalTextBigFieldAndSelect()
        {
            Functions.DelProductCart(driver, baseURL);
            GoToAdmin("product/edit/8");
            testname = "ProductEditAdditionalaTextFieldAndSelect";
            VerifyBegin(testname);
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//div[contains(text(), 'Свойства товара')]")).Click();
            Thread.Sleep(3000);

            driver.FindElement(By.Id("customoptions")).FindElement(By.TagName("ui-modal-trigger")).Click();
            VerifyIsTrue(driver.FindElements(By.CssSelector(".modal-dialog")).Count == 1, "view custom options modal dialog");

            VerifyIsTrue(driver.FindElements(By.TagName("iframe")).Count > 0, "iframe on page");
            VerifyIsTrue(driver.FindElement(By.TagName("iframe")).GetAttribute("src").ToString().Contains("productCustomOptions.aspx"), "src page");

            var iframe = driver.FindElement(By.TagName("iframe"));
            driver.SwitchTo().Frame(iframe);
            Thread.Sleep(1000);
            WaitForElem(By.Id("ctl00_cphMain_productCustomOption_btnAddCustomOption"));
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_btnAddCustomOption")).Click();
            Thread.Sleep(4000);
            VerifyIsTrue(driver.FindElements(By.CssSelector(".option-box")).Count == 1, "view custom options box");
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_txtTitle")).SendKeys("New custom options 6");
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_txtSortOrder")).Clear();
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_txtSortOrder")).SendKeys("6");
            (new SelectElement(driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_ddlInputType")))).SelectByText("Выпадающий список");
            Thread.Sleep(1000);

            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl02_txtTitle")).Clear();
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl02_txtTitle")).SendKeys("select button1");
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl02_txtBasePrice")).Clear();
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl02_txtBasePrice")).SendKeys("300");
            (new SelectElement(driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl02_ddlPriceType")))).SelectByText("Фиксированная");
            Thread.Sleep(1000);
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl02_txtSortOrder")).Clear();
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl02_txtSortOrder")).SendKeys("1");
            
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_btnAdd")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl03_txtTitle")).Clear();
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl03_txtTitle")).SendKeys("select button2");
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl03_txtBasePrice")).Clear();
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl03_txtBasePrice")).SendKeys("100");
            (new SelectElement(driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl03_ddlPriceType")))).SelectByText("Процент");
            Thread.Sleep(1000);
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl03_txtSortOrder")).Clear();
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl03_txtSortOrder")).SendKeys("2");

            ScrollTo(By.Id("ctl00_cphMain_productCustomOption_btnAddCustomOption"));
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_btnAddCustomOption")).Click();
            Thread.Sleep(1000);
            ScrollTo(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl03_txtSortOrder"));
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl01_txtTitle")).SendKeys("New custom options 10");
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl01_cbIsRequired")).Click();
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl01_txtSortOrder")).Clear();
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl01_txtSortOrder")).SendKeys("1");
            (new SelectElement(driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl01_ddlInputType")))).SelectByText("Многострочное поле ввода");
            Thread.Sleep(1000);

            ScrollTo(By.Id("ctl00_cphMain_productCustomOption_btnAddCustomOption"));
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_btnAddCustomOption")).Click();
            Thread.Sleep(1000);
            ScrollTo(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl01_txtSortOrder"));
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl02_txtTitle")).SendKeys("New custom options 7");
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl02_cbIsRequired")).Click();
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl02_txtSortOrder")).Clear();
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl02_txtSortOrder")).SendKeys("10");
            (new SelectElement(driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl02_ddlInputType")))).SelectByText("Многострочное поле ввода");
            Thread.Sleep(1000);

            ScrollTo(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl00_grid_ctl03_txtSortOrder"));
            driver.FindElement(By.Id("ctl00_cphMain_productCustomOption_rCustomOptions_ctl01_Button1")).Click();
            Thread.Sleep(1000);
            driver.SwitchTo().DefaultContent();
            ScrollTo(By.CssSelector(".btn.btn-save.btn-primary"));
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            Thread.Sleep(5000);

            GoToClient("products/test-product8");
            ScrollTo(By.ClassName("rating"));
            Thread.Sleep(3000);
            VerifyIsTrue(driver.FindElements(By.CssSelector(".custom-options-list")).Count == 1, "view custom options list");
            VerifyIsTrue(driver.FindElements(By.CssSelector(".custom-options-name")).Count == 2, "custom options count");
            VerifyAreEqual("New custom options 6", driver.FindElements(By.CssSelector(".custom-options-name"))[0].Text, " 1 custom options name");
            VerifyIsTrue(driver.FindElements(By.CssSelector(".custom-options-value"))[0].FindElements(By.TagName("select")).Count == 1, "1 view custom options field");

            VerifyAreEqual("New custom options 7", driver.FindElements(By.CssSelector(".custom-options-name"))[1].Text, "2 custom options name");
            VerifyIsTrue(driver.FindElements(By.CssSelector(".custom-options-value"))[1].FindElements(By.TagName("textarea")).Count == 1, " 2 view custom options field");

            (new SelectElement(driver.FindElement(By.CssSelector(".custom-options-value select")))).SelectByText("select button1 +300 руб.");
            Thread.Sleep(3000);
            driver.FindElements(By.CssSelector(".custom-options-value"))[1].FindElement(By.TagName("textarea")).SendKeys("test custom options1");
            Thread.Sleep(3000);
            
            driver.FindElement(By.CssSelector(".btn.btn-middle.btn-confirm.icon-bag-before")).Click();
            Thread.Sleep(4000);

            GoToClient("cart");
            Thread.Sleep(2000);
            VerifyAreEqual("New custom options 6:", driver.FindElements(By.CssSelector(".cart-full-properties-name"))[0].Text, " cart custom options name 1");
           VerifyAreEqual("select button1 + 300 руб.", driver.FindElements(By.CssSelector(".cart-full-properties-value"))[0].Text, " cart custom options value 1");

            VerifyAreEqual("New custom options 7:", driver.FindElements(By.CssSelector(".cart-full-properties-name"))[1].Text, " cart custom options name 2");
             VerifyAreEqual("test custom options1", driver.FindElements(By.CssSelector(".cart-full-properties-value"))[1].Text, " cart custom options value 2");

            GoToClient("checkout");
            Thread.Sleep(5000);
            VerifyAreEqual("New custom options 6:", driver.FindElements(By.CssSelector(".cart-full-properties-name"))[0].Text, " checkout custom options name 1");
            VerifyAreEqual("select button1 + 300 руб.", driver.FindElements(By.CssSelector(".cart-full-properties-value"))[0].Text, " cart custom options value 1");

            VerifyAreEqual("New custom options 7:", driver.FindElements(By.CssSelector(".cart-full-properties-name"))[1].Text, " cart custom options name 2");
            VerifyAreEqual("test custom options1", driver.FindElements(By.CssSelector(".cart-full-properties-value"))[1].Text, " checkout custom options value 2");
            
            VerifyFinally(testname);

        }

    }
}
