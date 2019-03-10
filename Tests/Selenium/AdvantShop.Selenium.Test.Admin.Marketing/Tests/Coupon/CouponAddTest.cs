using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using AdvantShop.Selenium.Core.Infrastructure;

namespace AdvantShop.SeleniumTest.Admin.Marketing.Coupons
{
    [TestFixture]
    public class CouponAdd : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(
            "data\\Admin\\Coupons\\Catalog.Product.csv",
            "data\\Admin\\Coupons\\Catalog.Photo.csv",
            "data\\Admin\\Coupons\\Catalog.Offer.csv",
            "data\\Admin\\Coupons\\Catalog.Category.csv",
            "data\\Admin\\Coupons\\Catalog.ProductCategories.csv",
            "data\\Admin\\Coupons\\Catalog.Brand.csv",
            "data\\Admin\\Coupons\\Catalog.Color.csv",
            "data\\Admin\\Coupons\\Catalog.Size.csv",
             "data\\Admin\\Coupons\\Catalog.Coupon.csv",
            "data\\Admin\\Coupons\\Catalog.CouponCategories.csv",
            "data\\Admin\\Coupons\\Catalog.CouponProducts.csv"
           );
            Init();
        }
        [Test]
        public void AddCoupon()
        {
            testname = "AddCoupon";
            VerifyBegin(testname);

            GoToAdmin("coupons");
            
            GetButton(eButtonType.Add).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Купон", driver.FindElement(By.TagName("h2")).Text, "modal add h2");

            driver.FindElement(By.CssSelector("[data-e2e=\"couponCode\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"couponCode\"]")).SendKeys("NewCoupons");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"couponType\"]")))).SelectByText("Процентный");
            driver.FindElement(By.CssSelector("[data-e2e=\"couponValue\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"couponValue\"]")).SendKeys("50");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"couponCurrency\"]")))).SelectByText("Рубли");
            //scroll
            IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
            jse.ExecuteScript("arguments[0].scrollIntoView(true)", driver.FindElement(By.CssSelector(".modal-body")).FindElement(By.CssSelector("[data-e2e=\"couponUseExpirationDate\"]")));
            Thread.Sleep(1000);

            driver.FindElement(By.CssSelector("[data-e2e=\"couponMinimalOrderPrice\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"couponMinimalOrderPrice\"]")).SendKeys("1500");

            driver.FindElement(By.CssSelector("[data-e2e=\"btnSave\"]")).Click();
            Thread.Sleep(2000);

            //cheking grid
            Refresh();
            GetGridFilter().SendKeys("NewCoupons");
            DropFocus("h1");
            VerifyAreEqual("NewCoupons", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "check grid code");
            VerifyAreEqual("50", GetGridCell(0, "Value").FindElement(By.TagName("input")).GetAttribute("value"), "check grid Value");
            VerifyAreEqual("1500", GetGridCell(0, "MinimalOrderPrice").FindElement(By.TagName("input")).GetAttribute("value"), "check grid MinimalOrderPrice");
            VerifyAreEqual("Процентный", GetGridCell(0, "TypeFormatted").Text, "check grid TypeFormatted");
            VerifyAreEqual("Бессрочно", GetGridCell(0, "ExpirationDateFormatted").Text, "check grid ExpirationDateFormatted");
            VerifyAreEqual("0 / 0", GetGridCell(0, "ActualUses").Text, "check grid ActualUses");
            VerifyIsTrue(GetGridCell(0, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected, "check grid Enabled");

            //cheking client
            ProductToCard("1");
            GoToClient("cart");
            VerifyAreEqual("Скидка: 10%",driver.FindElement(By.CssSelector(".price-discount")).Text, "client discount");
            VerifyAreEqual("900 руб.", driver.FindElement(By.CssSelector(".cart-full-result-price")).Text, "client result price");
            driver.FindElement(By.CssSelector(".col-xs-8 input")).SendKeys("NewCoupons");
            driver.FindElement(By.CssSelector(".btn-action.btn-expander")).Click();
            Thread.Sleep(1000);
            VerifyAreEqual("Купон:", driver.FindElements(By.CssSelector(".cart-full-summary-name"))[1].Text, "client coupon");
            VerifyAreEqual("0 руб.", driver.FindElements(By.CssSelector(".cart-full-summary-price span"))[0].Text, "client 0 sum coupon");
            VerifyAreEqual("1 000 руб.", driver.FindElement(By.CssSelector(".cart-full-result-price")).Text, "client rezult");

            ProductToCard("1");
            GoToClient("cart");
            VerifyIsFalse(driver.FindElement(By.CssSelector(".price-discount")).Displayed, "client display discount");
            VerifyAreEqual("Купон:", driver.FindElements(By.CssSelector(".cart-full-summary-name"))[1].Text, "client 2 coupon");
            VerifyAreEqual("1 000 руб.", driver.FindElements(By.CssSelector(".cart-full-summary-price span"))[0].Text, "client sum discount");
            VerifyAreEqual("( 50 %)", driver.FindElements(By.CssSelector(".cart-full-summary-price span"))[1].Text, "client percent discount ");
            VerifyAreEqual("1 000 руб.", driver.FindElement(By.CssSelector(".cart-full-result-price")).Text, "client rezult sum");
            ScrollTo(By.TagName("footer"));

            driver.FindElement(By.CssSelector(".btn.btn-middle.btn-submit")).Click();
            Thread.Sleep(5000); 
            WaitForElem(By.CssSelector(".breads"));
            WaitForElem(By.Id("rightCell"));

            ScrollTo(By.TagName("footer"));
            VerifyIsTrue(driver.FindElement(By.CssSelector(".checkout-result")).Text.Contains("1 000 руб."), "checkout rezult");
            driver.FindElements(By.CssSelector(".btn.btn-big.btn-submit"))[1].Click();
            Thread.Sleep(4000);

            GoToAdmin("coupons");
            GetGridFilter().SendKeys("NewCoupons");
            DropFocus("h1");
            VerifyAreEqual("NewCoupons", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "grid name coupon after order");
            VerifyAreEqual("1 / 0", GetGridCell(0, "ActualUses").Text, "ActualUses coupon");
            VerifyFinally(testname);
        }
       
        [Test]
        public void AddProductCoupon()
        {
            testname = "AddProductCoupon";
            VerifyBegin(testname);

            GoToAdmin("coupons");

            GetButton(eButtonType.Add).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Купон", driver.FindElement(By.TagName("h2")).Text, "");

            driver.FindElement(By.CssSelector("[data-e2e=\"couponCode\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"couponCode\"]")).SendKeys("NewCoupons4");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"couponType\"]")))).SelectByText("Фиксированный");
            driver.FindElement(By.CssSelector("[data-e2e=\"couponValue\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"couponValue\"]")).SendKeys("10000");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"couponCurrency\"]")))).SelectByText("Рубли");
            driver.FindElement(By.CssSelector("[data-e2e=\"couponMinimalOrderPrice\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"couponMinimalOrderPrice\"]")).SendKeys("300");
            driver.FindElement(By.CssSelector("[data-e2e=\"couponUseExpirationDate\"]")).FindElement(By.TagName("span")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"couponExpirationDate\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"couponExpirationDate\"]")).SendKeys("31.12.2030");
            driver.FindElement(By.CssSelector("[data-e2e=\"couponUsePosibleUses\"]")).FindElement(By.TagName("span")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"couponPosibleUses\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"couponPosibleUses\"]")).SendKeys("1");
           
            //category
            ScrollTo(By.TagName("ui-modal-trigger"));
            driver.FindElement(By.CssSelector("[data-e2e=\"couponCategories\"]")).Click();
            Thread.Sleep(2000);
            WaitForElem(By.XPath("//h2[contains(text(), 'Выбор категорий')]"));
            driver.FindElement(By.Id("1")).Click();
            Thread.Sleep(1000);
            VerifyAreEqual("true", driver.FindElement(By.CssSelector("li")).GetAttribute("aria-selected"), "check category true");
            VerifyAreEqual("false", driver.FindElements(By.CssSelector("li"))[1].GetAttribute("aria-selected"), "check category false");
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("3 категории(й)", driver.FindElement(By.CssSelector("[data-e2e=\"couponCategoriesList\"]")).Text, "count select category");
            driver.FindElement(By.CssSelector("[data-e2e=\"couponCategories\"]")).Click();
            Thread.Sleep(2000);
            WaitForElem(By.XPath("//h2[contains(text(), 'Выбор категорий')]"));
            driver.FindElement(By.Id("3")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("4 категории(й)", driver.FindElement(By.CssSelector("[data-e2e=\"couponCategoriesList\"]")).Text, "new count category");
            driver.FindElement(By.CssSelector("[data-e2e=\"couponCategoriesReset\"]")).Click();
            Thread.Sleep(1000);
            VerifyAreEqual("Все", driver.FindElement(By.CssSelector("[data-e2e=\"couponCategoriesAll\"]")).Text, "all select category");
            driver.FindElement(By.CssSelector("[data-e2e=\"couponCategories\"]")).Click();
            Thread.Sleep(2000);
            WaitForElem(By.XPath("//h2[contains(text(), 'Выбор категорий')]"));
            driver.FindElement(By.Id("3")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            Thread.Sleep(2000);

            //product
            driver.FindElement(By.CssSelector("[data-e2e=\"couponProducts\"]")).Click();
            Thread.Sleep(2000);
            WaitForElem(By.XPath("//h2[contains(text(), 'Выбор товара')]"));

            //artNo
            Functions.GridFilterSet(driver, baseURL, "ProductArtNo");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("10");
            VerifyAreEqual("TestProduct10", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "fiter product artNo");
            Functions.GridFilterClose(driver, baseURL, "ProductArtNo");
            VerifyAreEqual("TestProduct1", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "fiter product artNoc lose 1");
            VerifyAreEqual("TestProduct18", GetGridCell(9, "Name", "ProductsSelectvizr").Text, "fiter product artNo close 10");

            //Name
            Functions.GridFilterSet(driver, baseURL, "Name");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("testproduct11");
            VerifyAreEqual("TestProduct11", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "fiter product Name 1");
            Functions.GridFilterClose(driver, baseURL, "Name");
            VerifyAreEqual("TestProduct1", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "fiter product Name close 1");
            VerifyAreEqual("TestProduct18", GetGridCell(9, "Name", "ProductsSelectvizr").Text, "fiter product Name close 10");

            //Brand
            Functions.GridFilterSet(driver, baseURL, "BrandId");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("BrandName1");
            VerifyAreEqual("TestProduct1", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "fiter product BrandId 1");
            VerifyAreEqual("TestProduct11", GetGridCell(1, "Name", "ProductsSelectvizr").Text, "fiter product BrandId 10");
            Functions.GridFilterClose(driver, baseURL, "BrandId");
            VerifyAreEqual("TestProduct1", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "fiter product BrandId close 1");
            VerifyAreEqual("TestProduct18", GetGridCell(9, "Name", "ProductsSelectvizr").Text, "fiter product BrandId close 10");

            //Price
            Functions.GridFilterSet(driver, baseURL, "Price");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).SendKeys("1200");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).SendKeys("2000");
            Thread.Sleep(1000);
            VerifyAreEqual("TestProduct18", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "fiter product Price 1");
            VerifyAreEqual("TestProduct19", GetGridCell(1, "Name", "ProductsSelectvizr").Text, "fiter product Price 10");
            Functions.GridFilterClose(driver, baseURL, "Price");
            VerifyAreEqual("TestProduct1", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "fiter product Price close 1");
            VerifyAreEqual("TestProduct18", GetGridCell(9, "Name", "ProductsSelectvizr").Text, "fiter product Price close 10");

            //Amount
            Functions.GridFilterSet(driver, baseURL, "Amount");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).SendKeys("5");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).SendKeys("6");
            Thread.Sleep(1000);
            VerifyAreEqual("TestProduct5", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "fiter product Amount 1");
            VerifyAreEqual("TestProduct6", GetGridCell(1, "Name", "ProductsSelectvizr").Text, "fiter product Amount 10");
            Functions.GridFilterClose(driver, baseURL, "Amount");
            VerifyAreEqual("TestProduct1", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "fiter product Amount close 1");
            VerifyAreEqual("TestProduct18", GetGridCell(9, "Name", "ProductsSelectvizr").Text, "fiter product Amount close 10");

            //Enabled
            Functions.GridFilterSet(driver, baseURL, "Enabled");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Неактивные");
            VerifyAreEqual("TestProduct2", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "fiter product Enabled 1");
            VerifyAreEqual("TestProduct3", GetGridCell(1, "Name", "ProductsSelectvizr").Text, "fiter product Enabled 10");
            Functions.GridFilterClose(driver, baseURL, "Enabled");
            VerifyAreEqual("TestProduct1", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "fiter product Enabled close 1");
            VerifyAreEqual("TestProduct18", GetGridCell(9, "Name", "ProductsSelectvizr").Text, "fiter product Enabled close 10");

            GetGridCell(0, "Name", "ProductsSelectvizr").Click();
            GetGridCell(1, "Name", "ProductsSelectvizr").Click();
            GetGridCell(2, "Name", "ProductsSelectvizr").Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            Thread.Sleep(1000);
            VerifyAreEqual("3 товар(ов)", driver.FindElement(By.CssSelector("[data-e2e=\"couponProductsList\"]")).Text, "count product");
            driver.FindElement(By.CssSelector("[data-e2e=\"couponProductsReset\"]")).Click();
            Thread.Sleep(1000);
            VerifyAreEqual("Все", driver.FindElement(By.CssSelector("[data-e2e=\"couponProductsAll\"]")).Text, "all count roduct");
            driver.FindElement(By.CssSelector("[data-e2e=\"couponProducts\"]")).Click();
            Thread.Sleep(2000);
            WaitForElem(By.XPath("//h2[contains(text(), 'Выбор товара')]"));
            GetGridCell(0, "Name", "ProductsSelectvizr").Click();
            GetGridCell(1, "Name", "ProductsSelectvizr").Click();
            GetGridCell(2, "Name", "ProductsSelectvizr").Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"btnSave\"]")).Click();
            Thread.Sleep(2000);

            //cheking grid
            GoToAdmin("coupons");
            GetGridFilter().SendKeys("NewCoupons4");
            DropFocus("h1");
            VerifyAreEqual("NewCoupons4", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "grid Code");
            VerifyAreEqual("10000", GetGridCell(0, "Value").FindElement(By.TagName("input")).GetAttribute("value"), "grid Value");
            VerifyAreEqual("300", GetGridCell(0, "MinimalOrderPrice").FindElement(By.TagName("input")).GetAttribute("value"), "grid MinimalOrderPrice");
            VerifyAreEqual("Фиксированный", GetGridCell(0, "TypeFormatted").Text, "grid TypeFormatted");
            VerifyAreEqual("31.12.2030 23:59:59", GetGridCell(0, "ExpirationDateFormatted").Text, "grid ExpirationDateFormatted");
            VerifyAreEqual("0 / 1", GetGridCell(0, "ActualUses").Text, "grid ActualUses");
            VerifyIsTrue(GetGridCell(0, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected, "grid Enabled");

            //cheking client
            ProductToCard("15");
            GoToClient("cart");
            VerifyAreEqual("Скидка: 10%", driver.FindElement(By.CssSelector(".price-discount")).Text, "client discount percent");
            VerifyAreEqual("900 руб.", driver.FindElement(By.CssSelector(".cart-full-result-price")).Text, "client discount sum");
            driver.FindElement(By.CssSelector(".col-xs-8 input")).SendKeys("NewCoupons4");
            driver.FindElement(By.CssSelector(".btn-action.btn-expander")).Click();
            Thread.Sleep(1000);
           // VerifyIsTrue(driver.PageSource.Contains("Невозможно применить купон"), "client enabled coupon");
            VerifyAreEqual("900 руб.", driver.FindElement(By.CssSelector(".cart-full-result-price")).Text, "old rezult price");

            driver.FindElement(By.CssSelector(".cart-full-body-item.cart-full-remove a")).Click();
            Thread.Sleep(1000);
            ProductToCard("1");
            GoToClient("cart");
            VerifyAreEqual("Купон:", driver.FindElements(By.CssSelector(".cart-full-summary-name"))[1].Text, "client coupon name");
            VerifyAreEqual("10 000 руб.", driver.FindElements(By.CssSelector(".cart-full-summary-price span"))[0].Text, "client coupon sum");
            VerifyAreEqual("0 руб.", driver.FindElement(By.CssSelector(".cart-full-result-price")).Text, "client cart price");
            ScrollTo(By.TagName("footer"));

            driver.FindElement(By.CssSelector(".btn.btn-middle.btn-submit")).Click();
            Thread.Sleep(5000);
            WaitForElem(By.CssSelector(".breads"));
            WaitForElem(By.Id("rightCell"));

            ScrollTo(By.TagName("footer"));
            VerifyAreEqual("0 руб.", driver.FindElement(By.CssSelector(".checkout-result-price")).Text, "checkout price");
            driver.FindElements(By.CssSelector(".btn.btn-big.btn-submit"))[1].Click();
            Thread.Sleep(2000);

            GoToAdmin("coupons");
            GetGridFilter().SendKeys("NewCoupons4");
            DropFocus("h1");
            VerifyAreEqual("NewCoupons4", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "admin name coupon");
            VerifyAreEqual("1 / 1", GetGridCell(0, "ActualUses").Text, "admin count used");

            ProductToCard("1");
            GoToClient("cart");
            driver.FindElement(By.CssSelector(".col-xs-8 input")).SendKeys("NewCoupons4");
            driver.FindElement(By.CssSelector(".btn-action.btn-expander")).Click();
            Thread.Sleep(1000);
          //  VerifyIsTrue(driver.PageSource.Contains("Невозможно применить купон"), "return take coupon");
            VerifyAreEqual("900 руб.", driver.FindElement(By.CssSelector(".cart-full-result-price")).Text, "old price without coupon");
            driver.FindElement(By.CssSelector(".cart-full-body-item.cart-full-remove a")).Click();
            Thread.Sleep(1000);
            VerifyFinally(testname);
        }

        [Test]
        public void AddDisabledCoupon()
        {
            testname = "AddDisabledCoupon";
            VerifyBegin(testname);

            GoToAdmin("coupons");

            GetButton(eButtonType.Add).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Купон", driver.FindElement(By.TagName("h2")).Text);

            driver.FindElement(By.CssSelector("[data-e2e=\"couponCode\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"couponCode\"]")).SendKeys("NewCoupons2");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"couponType\"]")))).SelectByText("Фиксированный");
            driver.FindElement(By.CssSelector("[data-e2e=\"couponValue\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"couponValue\"]")).SendKeys("10000");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"couponCurrency\"]")))).SelectByText("Рубли");
            driver.FindElement(By.CssSelector("[data-e2e=\"couponMinimalOrderPrice\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"couponMinimalOrderPrice\"]")).SendKeys("15");
            driver.FindElement(By.CssSelector("[data-e2e=\"couponUseExpirationDate\"]")).FindElement(By.TagName("span")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"couponExpirationDate\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"couponExpirationDate\"]")).SendKeys("31.12.2030");
            driver.FindElement(By.CssSelector("[data-e2e=\"couponUsePosibleUses\"]")).FindElement(By.TagName("span")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"couponPosibleUses\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"couponPosibleUses\"]")).SendKeys("5");
            driver.FindElement(By.CssSelector("[data-e2e=\"couponEnabled\"]")).Click();

            driver.FindElement(By.CssSelector("[data-e2e=\"btnSave\"]")).Click();
            Thread.Sleep(2000);

            //cheking grid
            Refresh();
            GetGridFilter().SendKeys("NewCoupons2");
            DropFocus("h1");
            VerifyAreEqual("NewCoupons2", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "grid Code");
            VerifyAreEqual("10000", GetGridCell(0, "Value").FindElement(By.TagName("input")).GetAttribute("value"), "grid Value");
            VerifyAreEqual("15", GetGridCell(0, "MinimalOrderPrice").FindElement(By.TagName("input")).GetAttribute("value"), "grid MinimalOrderPrice");
            VerifyAreEqual("Фиксированный", GetGridCell(0, "TypeFormatted").Text, "grid TypeFormatted");
            VerifyAreEqual("31.12.2030 23:59:59", GetGridCell(0, "ExpirationDateFormatted").Text, "grid ExpirationDateFormatted");
            VerifyAreEqual("0 / 5", GetGridCell(0, "ActualUses").Text, "grid ActualUses");
            VerifyIsFalse(GetGridCell(0, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected, "grid Enabled");

            //cheking client
            ProductToCard("1");
            GoToClient("cart");
            VerifyAreEqual("Скидка: 10%", driver.FindElement(By.CssSelector(".price-discount")).Text, "client percent discount");
            VerifyAreEqual("900 руб.", driver.FindElement(By.CssSelector(".cart-full-result-price")).Text, "client sum discont");
            driver.FindElement(By.CssSelector(".col-xs-8 input")).SendKeys("NewCoupons2");
            driver.FindElement(By.CssSelector(".btn-action.btn-expander")).Click();
            Thread.Sleep(1000);
           // VerifyIsTrue(driver.PageSource.Contains("Невозможно применить купон"), "client not enabled coupon");
            VerifyAreEqual("900 руб.", driver.FindElement(By.CssSelector(".cart-full-result-price")).Text, "client old cart price");

            driver.FindElement(By.CssSelector(".cart-full-body-item.cart-full-remove a")).Click();
            Thread.Sleep(1000);
            VerifyFinally(testname);
        }

        [Test]
        public void EditCoupon()
        {
            testname = "EditCoupon";
            VerifyBegin(testname);
            GoToAdmin("coupons");

            GetGridFilter().SendKeys("test1");
            DropFocus("h1");
            GetGridCell(0, "_serviceColumn").FindElement(By.TagName("a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Купон", driver.FindElement(By.TagName("h2")).Text, "modal edit h2");
            //cheking modal win

            VerifyAreEqual("test1", driver.FindElement(By.CssSelector("[data-e2e=\"couponCode\"]")).GetAttribute("value"), "modal couponCode");
            VerifyAreEqual("100", driver.FindElement(By.CssSelector("[data-e2e=\"couponValue\"]")).GetAttribute("value"), "modal couponValue");
            VerifyAreEqual("0", driver.FindElement(By.CssSelector("[data-e2e=\"couponMinimalOrderPrice\"]")).GetAttribute("value"), "modal couponMinimalOrderPrice");

            IWebElement selectElem = driver.FindElement(By.CssSelector("[data-e2e=\"couponType\"]"));
            SelectElement select = new SelectElement(selectElem);
            VerifyIsTrue(select.AllSelectedOptions[0].Text.Contains("Фиксированный"), "modal couponType");

            selectElem = driver.FindElement(By.CssSelector("[data-e2e=\"couponCurrency\"]"));
            select = new SelectElement(selectElem);
            VerifyIsTrue(select.SelectedOption.Text.Contains("Рубли"), "modal Currency");
            
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"couponUseExpirationDate\"]")).FindElement(By.TagName("input")).Selected, "modal UseExpirationDate");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"couponUsePosibleUses\"]")).FindElement(By.TagName("input")).Selected, "modal UsePosibleUses");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"couponEnabled\"]")).FindElement(By.TagName("input")).Selected, "modal Enabled");

            driver.FindElement(By.CssSelector("[data-e2e=\"couponCode\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"couponCode\"]")).SendKeys("NewCoupons3");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"couponType\"]")))).SelectByText("Фиксированный");
            driver.FindElement(By.CssSelector("[data-e2e=\"couponValue\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"couponValue\"]")).SendKeys("10000");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"couponCurrency\"]")))).SelectByText("Евро");
            driver.FindElement(By.CssSelector("[data-e2e=\"couponMinimalOrderPrice\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"couponMinimalOrderPrice\"]")).SendKeys("15");
            driver.FindElement(By.CssSelector("[data-e2e=\"couponUseExpirationDate\"]")).FindElement(By.TagName("span")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"couponExpirationDate\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"couponExpirationDate\"]")).SendKeys("31.12.2016");
            driver.FindElement(By.CssSelector("[data-e2e=\"couponUsePosibleUses\"]")).FindElement(By.TagName("span")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"couponPosibleUses\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"couponPosibleUses\"]")).SendKeys("5");
            driver.FindElement(By.CssSelector("[data-e2e=\"couponEnabled\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"btnSave\"]")).Click();
            Thread.Sleep(2000);
            //cheking grid
            Refresh();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("NewCoupons3");
            DropFocus("h1");
            VerifyAreEqual("NewCoupons3", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "grid Code");
            VerifyAreEqual("10000", GetGridCell(0, "Value").FindElement(By.TagName("input")).GetAttribute("value"), "grid Value");
            VerifyAreEqual("15", GetGridCell(0, "MinimalOrderPrice").FindElement(By.TagName("input")).GetAttribute("value"), "grid MinimalOrderPrice");
            VerifyAreEqual("Фиксированный", GetGridCell(0, "TypeFormatted").Text, "grid TypeFormatted");
            VerifyAreEqual("31.12.2016 23:59:59", GetGridCell(0, "ExpirationDateFormatted").Text, "grid ExpirationDateFormatted");
            VerifyAreEqual("0 / 5", GetGridCell(0, "ActualUses").Text, "grid ActualUses");
            VerifyIsFalse(GetGridCell(0, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected, "grid Enabled");
            VerifyFinally(testname);
        }

        public void ProductToCard(string id)
        {
            GoToClient("products/test-product" + id);
            ScrollTo(By.CssSelector(".rating"));
            driver.FindElement(By.CssSelector(".details-payment-inline a")).Click();
            Thread.Sleep(2000);
          
        }
    }
}