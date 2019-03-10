using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;
using System.Threading;

namespace AdvantShop.SeleniumTest.Admin.Settings.SettingsSEO
{
    [TestFixture]
    public class SettingsSeoProductTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.SettingsProductsPerPage);
            InitializeService.LoadData(

           "data\\Admin\\Settings\\SettingsSeo\\Settings.Settings.csv"

           );

            Init();
        }


        [Test]
        public void DefaultSeoProduct()
        {
            testname = "DefaultSeoProduct";
            VerifyBegin(testname);

            GoToAdmin("product/edit/348");
            driver.FindElement(By.XPath("//div[contains(text(), 'SEO')]")).Click();
            Thread.Sleep(3000);
            if (!driver.FindElement(By.CssSelector("[data-e2e=\"productDefaultMeta\"]")).FindElement(By.Id("DefaultMeta")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"productDefaultMeta\"]")).FindElement(By.TagName("span")).Click();
                ScrollTo(By.Id("header-top"));
                driver.FindElement(By.CssSelector("[data-e2e=\"btnSave\"]")).Click();
                Thread.Sleep(2000);
            }
            
            GoToAdmin("settingsseo");
            ScrollTo(By.Id("TagsDefaultH1"));

            driver.FindElement(By.Id("ProductsDefaultTitle")).Click();
            driver.FindElement(By.Id("ProductsDefaultTitle")).Clear();
            driver.FindElement(By.Id("ProductsDefaultTitle")).SendKeys("New title Product");

            driver.FindElement(By.Id("ProductsDefaultMetaKeywords")).Click();
            driver.FindElement(By.Id("ProductsDefaultMetaKeywords")).Clear();
            driver.FindElement(By.Id("ProductsDefaultMetaKeywords")).SendKeys("New meta keywords 1 Product, New meta keywords 2 Product, New meta keywords 3 Product");

            driver.FindElement(By.Id("ProductsDefaultMetaDescription")).Click();
            driver.FindElement(By.Id("ProductsDefaultMetaDescription")).Clear();
            driver.FindElement(By.Id("ProductsDefaultMetaDescription")).SendKeys("New description Product");

            driver.FindElement(By.Id("ProductsDefaultH1")).Click();
            driver.FindElement(By.Id("ProductsDefaultH1")).Clear();
            driver.FindElement(By.Id("ProductsDefaultH1")).SendKeys("New h1 Product");

            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"settingsSeoSave\"]")).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("settingsseo");

            VerifyAreEqual("New title Product", driver.FindElement(By.Id("ProductsDefaultTitle")).GetAttribute("value"), "default seo product title admin");
            VerifyAreEqual("New meta keywords 1 Product, New meta keywords 2 Product, New meta keywords 3 Product", driver.FindElement(By.Id("ProductsDefaultMetaKeywords")).GetAttribute("value"), "default seo product keywords admin");
            VerifyAreEqual("New description Product", driver.FindElement(By.Id("ProductsDefaultMetaDescription")).GetAttribute("value"), "default seo product description admin");
            VerifyAreEqual("New h1 Product", driver.FindElement(By.Id("ProductsDefaultH1")).GetAttribute("value"), "default seo product h1 admin");

            //check client
            GoToClient("products/ipd348");
            VerifyAreEqual("New title Product", driver.Title, "default seo product title client");
            VerifyAreEqual("New meta keywords 1 Product, New meta keywords 2 Product, New meta keywords 3 Product", driver.FindElement(By.CssSelector("[name=\"Keywords\"]")).GetAttribute("content"), "default seo product keywords client");
            VerifyAreEqual("New description Product", driver.FindElement(By.CssSelector("[name=\"Description\"]")).GetAttribute("content"), "default seo product description client");
            VerifyAreEqual("New h1 Product", driver.FindElement(By.TagName("h1")).Text, "default seo product h1 client");

            VerifyFinally(testname);
        }

        [Test]
        public void DefaultSeoProductVariables()
        {
            testname = "DefaultSeoProductVariables";
            VerifyBegin(testname);

            //set default meta
            GoToAdmin("product/edit/1642");
            driver.FindElement(By.XPath("//div[contains(text(), 'SEO')]")).Click();
            Thread.Sleep(3000);
            if (!driver.FindElement(By.CssSelector("[data-e2e=\"productDefaultMeta\"]")).FindElement(By.Id("DefaultMeta")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"productDefaultMeta\"]")).FindElement(By.TagName("span")).Click();
                ScrollTo(By.Id("header-top"));
                driver.FindElement(By.CssSelector("[data-e2e=\"btnSave\"]")).Click();
                Thread.Sleep(2000);
            }
            
            GoToAdmin("settingsseo");
            ScrollTo(By.Id("TagsDefaultH1"));

            driver.FindElement(By.Id("ProductsDefaultTitle")).Click();
            driver.FindElement(By.Id("ProductsDefaultTitle")).Clear();
            driver.FindElement(By.Id("ProductsDefaultTitle")).SendKeys("#STORE_NAME# - #PRODUCT_NAME# - #CATEGORY_NAME# - бренд #BRAND_NAME# - #PRICE# - #TAGS#");

            driver.FindElement(By.Id("ProductsDefaultMetaKeywords")).Click();
            driver.FindElement(By.Id("ProductsDefaultMetaKeywords")).Clear();
            driver.FindElement(By.Id("ProductsDefaultMetaKeywords")).SendKeys("#STORE_NAME# - #PRODUCT_NAME# - #CATEGORY_NAME# - бренд #BRAND_NAME# - #PRICE# - #TAGS#");

            driver.FindElement(By.Id("ProductsDefaultMetaDescription")).Click();
            driver.FindElement(By.Id("ProductsDefaultMetaDescription")).Clear();
            driver.FindElement(By.Id("ProductsDefaultMetaDescription")).SendKeys("#STORE_NAME# - #PRODUCT_NAME# - #CATEGORY_NAME# - бренд #BRAND_NAME# - #PRICE# - #TAGS#");

            driver.FindElement(By.Id("ProductsDefaultH1")).Click();
            driver.FindElement(By.Id("ProductsDefaultH1")).Clear();
            driver.FindElement(By.Id("ProductsDefaultH1")).SendKeys("#STORE_NAME# - #PRODUCT_NAME# - #CATEGORY_NAME# - бренд #BRAND_NAME# - #PRICE# - #TAGS#");

            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"settingsSeoSave\"]")).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("settingsseo");

            VerifyAreEqual("#STORE_NAME# - #PRODUCT_NAME# - #CATEGORY_NAME# - бренд #BRAND_NAME# - #PRICE# - #TAGS#", driver.FindElement(By.Id("ProductsDefaultTitle")).GetAttribute("value"), "default seo product title admin");
            VerifyAreEqual("#STORE_NAME# - #PRODUCT_NAME# - #CATEGORY_NAME# - бренд #BRAND_NAME# - #PRICE# - #TAGS#", driver.FindElement(By.Id("ProductsDefaultMetaKeywords")).GetAttribute("value"), "default seo product keywords admin");
            VerifyAreEqual("#STORE_NAME# - #PRODUCT_NAME# - #CATEGORY_NAME# - бренд #BRAND_NAME# - #PRICE# - #TAGS#", driver.FindElement(By.Id("ProductsDefaultMetaDescription")).GetAttribute("value"), "default seo product description admin");
            VerifyAreEqual("#STORE_NAME# - #PRODUCT_NAME# - #CATEGORY_NAME# - бренд #BRAND_NAME# - #PRICE# - #TAGS#", driver.FindElement(By.Id("ProductsDefaultH1")).GetAttribute("value"), "default seo product h1 admin");

            //check client
            GoToClient("products/samsung-md46c");
            VerifyAreEqual("Мой магазин - Samsung MD46C - Samsung - бренд Samsung - 51 399 руб. - LED ЖК", driver.Title, "default seo product title client");
            VerifyAreEqual("Мой магазин - Samsung MD46C - Samsung - бренд Samsung -  51 399  руб. -  LED ЖК", driver.FindElement(By.CssSelector("[name=\"Keywords\"]")).GetAttribute("content"), "default seo product keywords client");
            VerifyAreEqual("Мой магазин - Samsung MD46C - Samsung - бренд Samsung -  51 399  руб. -  LED ЖК", driver.FindElement(By.CssSelector("[name=\"Description\"]")).GetAttribute("content"), "default seo product description client");
            VerifyAreEqual("Мой магазин - Samsung MD46C - Samsung - бренд Samsung - 51 399 руб. - LED ЖК", driver.FindElement(By.TagName("h1")).Text, "default seo product h1 client");
            VerifyFinally(testname);
        }

        [Test]
        public void DefaultSeoProductReset()
        {
            testname = "DefaultSeoProductReset";
            VerifyBegin(testname);

            //admin set meta for product
            GoToAdmin("product/edit/10");
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//div[contains(text(), 'SEO')]")).Click();
            Thread.Sleep(3000);           
            driver.FindElement(By.CssSelector("[data-e2e=\"productDefaultMeta\"]")).FindElement(By.TagName("span")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//div[contains(text(), 'SEO')]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.Id("SeoTitle")).Clear();
            driver.FindElement(By.Id("SeoTitle")).SendKeys("Product_Title");
            driver.FindElement(By.Id("SeoKeywords")).Clear();
            driver.FindElement(By.Id("SeoKeywords")).SendKeys("Product_SeoKeywords");
            driver.FindElement(By.Id("SeoDescription")).Clear();
            driver.FindElement(By.Id("SeoDescription")).SendKeys("Product_SeoDescription");
            driver.FindElement(By.Id("SeoH1")).Clear();
            driver.FindElement(By.Id("SeoH1")).SendKeys("Product_H1");
            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"btnSave\"]")).Click();
            Thread.Sleep(2000);

            //pre check client
            GoToClient("products/ipd10");
            VerifyAreEqual("Product_Title", driver.Title, "pre check seo product title client");
            VerifyAreEqual("Product_H1", driver.FindElement(By.TagName("h1")).Text, "pre check seo product h1 client");
            VerifyAreEqual("Product_SeoKeywords", driver.FindElement(By.CssSelector("[name=\"Keywords\"]")).GetAttribute("content"), "pre check seo product keywords client");
            VerifyAreEqual("Product_SeoDescription", driver.FindElement(By.CssSelector("[name=\"Description\"]")).GetAttribute("content"), "pre check seo product description client");

            //reset meta
            GoToAdmin("settingsseo");
            ScrollTo(By.Id("TagsDefaultMetaDescription"));

            driver.FindElement(By.Id("ProductsDefaultTitle")).Click();
            driver.FindElement(By.Id("ProductsDefaultTitle")).Clear();
            driver.FindElement(By.Id("ProductsDefaultTitle")).SendKeys("1");

            driver.FindElement(By.Id("ProductsDefaultMetaKeywords")).Click();
            driver.FindElement(By.Id("ProductsDefaultMetaKeywords")).Clear();
            driver.FindElement(By.Id("ProductsDefaultMetaKeywords")).SendKeys("2");

            driver.FindElement(By.Id("ProductsDefaultMetaDescription")).Click();
            driver.FindElement(By.Id("ProductsDefaultMetaDescription")).Clear();
            driver.FindElement(By.Id("ProductsDefaultMetaDescription")).SendKeys("3");

            driver.FindElement(By.Id("ProductsDefaultH1")).Click();
            driver.FindElement(By.Id("ProductsDefaultH1")).Clear();
            driver.FindElement(By.Id("ProductsDefaultH1")).SendKeys("4");

            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"settingsSeoSave\"]")).Click();
            Thread.Sleep(2000);

            GoToAdmin("settingsseo");
            ScrollTo(By.Id("ProductsDefaultH1"));

            driver.FindElement(By.LinkText("Сбросить мета информацию для всех товаров")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(3000);

            //check client
            GoToClient("products/ipd10");
            VerifyAreEqual("1", driver.Title, "reset seo product title client");
            VerifyAreEqual("2", driver.FindElement(By.CssSelector("[name=\"Keywords\"]")).GetAttribute("content"), "reset seo product keywords client");
            VerifyAreEqual("3", driver.FindElement(By.CssSelector("[name=\"Description\"]")).GetAttribute("content"), "reset seo product description client");
            VerifyAreEqual("4", driver.FindElement(By.TagName("h1")).Text, "reset seo product h1 client");

            VerifyFinally(testname);
        }
    }
}