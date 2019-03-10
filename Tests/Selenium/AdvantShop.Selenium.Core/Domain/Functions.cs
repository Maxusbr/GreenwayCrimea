using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;
using System;
using OpenQA.Selenium.Interactions;

namespace AdvantShop.SeleniumTest.Core
{
    public static class Functions
    {
        public static void LogAdmin(IWebDriver driver, string baseURL)
        {
            driver.Navigate().GoToUrl(baseURL);
            Cookie adminCookies = new Cookie("customer", "835d82df-aaa6-4d54-a870-8d353e541af9", null, "/", null);
            Cookie authCoockies = new Cookie("Advantshop.AUTH", "D4C156E47F200E34F2673A4C6576A9C46AF0CA49D22B94466B8E604D846627161F35E5624C1E564AFBCBD16CEE904BFCA6061179F555483E98A3CE3E368D461754324B01EA6C1568014A332A09FE833C74FA64207387FF7DDAA623C0DBBC8C59B8C7DDEEC38A58FF658E3CC7CA07F63D2E2ECC1DDA9AD92576A13C941469B61691675B39", null, "/", null);
            Cookie adminNotifyCoockies = new Cookie("dontDisturbByNotify", "true", null, "/", null);

            driver.Manage().Cookies.AddCookie(adminCookies);
            driver.Manage().Cookies.AddCookie(authCoockies);
            driver.Manage().Cookies.AddCookie(adminNotifyCoockies);
        }

        public static void LogCustomer(IWebDriver driver, string baseURL)
        {
            driver.Navigate().GoToUrl(baseURL);
            Cookie adminCookies = new Cookie("customer", "cfc2c33b-1e84-415e-8482-e98156341604", null, "/", null);
            Cookie authCoockies = new Cookie("Advantshop.AUTH", "A7960D698D9AA98AEB85818706369ED1B4E181CCF13A19D4C1C4164E7431D12C2FC90A78E8E394D5B8570A07E138E26A1627CCBCCE3C8CD1A5470943A18123664742CD54B8C7AF67252D94A91FBDE288EA994DC0EA6DD4BE86E3AA23B18637E4807AEDC60BC6A295A1F227F68BA781C2F02983FDBECD022B27DD321DB665B4A405028E6734DB891C3E92B6BF98834E408D89F0B6", null, "/", null);
            Cookie adminNotifyCoockies = new Cookie("dontDisturbByNotify", "true", null, "/", null);

            driver.Manage().Cookies.AddCookie(adminCookies);
            driver.Manage().Cookies.AddCookie(authCoockies);
            driver.Manage().Cookies.AddCookie(adminNotifyCoockies);
        }

        public static void IndicatorsNoBeforeMainPageAdmin(IWebDriver driver, string baseURL)
        {
            driver.FindElement(By.CssSelector("[data-e2e-indicators=\"IndicatorsShow\"]")).Click();
            Thread.Sleep(2000);
            if
              (
              driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"ProductsCount\"]")).Selected
              )
            {
                driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"ProductsCount\"]")).Click();
            }

            if
               (
               driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"OrdersTodayCount\"]")).Selected
               )
            {
                driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"OrdersTodayCount\"]")).Click();
            }
            if
               (
               driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"OrdersYesterdayCount\"]")).Selected
               )

            {
                driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"OrdersYesterdayCount\"]")).Click();
            }
            if
              (
              driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"OrdersMonthCount\"]")).Selected
              )
            {
                driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"OrdersMonthCount\"]")).Click();
            }

            if
               (
               driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"OrdersAllTimeCount\"]")).Selected
               )
            {
                driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"OrdersAllTimeCount\"]")).Click();
            }
            if
               (
               driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"LeadsTodayCount\"]")).Selected
               )

            {
                driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"LeadsTodayCount\"]")).Click();
            }
            if
               (
               driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"LeadsYesterdayCount\"]")).Selected
               )

            {
                driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"LeadsYesterdayCount\"]")).Click();
            }
            if
               (
               driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"LeadsMonthCount\"]")).Selected
               )

            {
                driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"LeadsMonthCount\"]")).Click();
            }

            if
               (
               driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"CallsTodayCount\"]")).Selected
               )
            {
                driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"CallsTodayCount\"]")).Click();
            }

            if
               (
               driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"CallsYesterdayCount\"]")).Selected
               )
            {
                driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"CallsYesterdayCount\"]")).Click();
            }

            if
               (
               driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"CallsMonthCount\"]")).Selected
               )
            {
                driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"CallsMonthCount\"]")).Click();
            }

            if
              (
              driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"ReviewsTodayCount\"]")).Selected
              )
            {
                driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"ReviewsTodayCount\"]")).Click();
            }

            if
            (
            driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"ReviewsYesterdayCount\"]")).Selected
            )
            {
                driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"ReviewsYesterdayCount\"]")).Click();
            }

            if
           (
           driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"ReviewsMonthCount\"]")).Selected
           )
            {
                driver.FindElement(By.CssSelector("[data-e2e-dashboard-check=\"ReviewsMonthCount\"]")).Click();
            }

            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(2000);
        }

        public static void PriceRegulation(IWebDriver driver, string baseURL, string Select, string SelectOption)
        {
            driver.Navigate().GoToUrl(baseURL + "/adminv2/priceregulation");
            Thread.Sleep(2000);
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationSelect\"]")))).SelectByText(Select);
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationSelectOption\"]")))).SelectByText(SelectOption);
        }

        public static void GridPaginationSelect10(IWebDriver driver, string baseURL)
        {
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]")))).SelectByText("10");
            Thread.Sleep(5000);
        }

        public static void GridPaginationSelect20(IWebDriver driver, string baseURL)
        {
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]")))).SelectByText("20");
            Thread.Sleep(5000);
        }

        public static void GridPaginationSelect50(IWebDriver driver, string baseURL)
        {
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]")))).SelectByText("50");
            Thread.Sleep(5000);
        }


        public static void GridPaginationSelect100(IWebDriver driver, string baseURL)
        {
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]")))).SelectByText("100");
            Thread.Sleep(5000);
        }


        public static void AddProduct_ProductListsFilter(IWebDriver driver, string baseURL, string filterName)
        {
            driver.Navigate().GoToUrl(baseURL + "/adminv2/productlists");
            Thread.Sleep(3000);
            driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"gridProductLists[0][\'Name\']\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"product_ProductListAdd\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterDropdownButton\"]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterDropdownItem\"][data-e2e-filter-dropdown-name=\"" + filterName + "\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
        }

        public static void AddProduct_ProductListsFilterFromTo(IWebDriver driver, string baseURL, string filterName)
        {
            driver.Navigate().GoToUrl(baseURL + "/adminv2/productlists");
            Thread.Sleep(3000);
            driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"gridProductLists[0][\'Name\']\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"product_ProductListAdd\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterDropdownButton\"]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterDropdownItem\"][data-e2e-filter-dropdown-name=\"" + filterName + "\"]")).Click();
            Thread.Sleep(2000);
        }

        public static void AddProduct_ProductListsFilterSelect(IWebDriver driver, string baseURL, string filter, string select)
        {
            driver.Navigate().GoToUrl(baseURL + "/adminv2/productlists/products/1");
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"product_ProductListAdd\"]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterDropdownButton\"]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterDropdownItem\"][data-e2e-filter-dropdown-name=\"" + filter + "\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")).Click();
            //scroll in select
            var element = driver.FindElement(By.CssSelector("[data-e2e=\"" + select + "\"]"));
            IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
            jse.ExecuteScript("arguments[0].scrollIntoView(true)", element);
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"" + select + "\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector(".modal-header-title")).Click();
            Thread.Sleep(1000);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            wait.Until(d => (bool)(d as IJavaScriptExecutor).ExecuteScript("return window.ajaxIsComplete();"));
        }

        public static void FilterPageFromTo(IWebDriver driver, string baseURL, string tag)
        {
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).SendKeys("1");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).SendKeys("100");
            driver.FindElement(By.TagName(tag)).Click();
            Thread.Sleep(1000);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            wait.Until(d => (bool)(d as IJavaScriptExecutor).ExecuteScript("return window.ajaxIsComplete();"));
        }

        public static void GridFilterSelectDropFocus(IWebDriver driver, string baseURL, string filterName, string filterItem, string tag)
        {
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterDropdownButton\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterDropdownItem\"][data-e2e-filter-dropdown-name=\"" + filterName + "\"]")).Click();
            Thread.Sleep(1000);
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"" + filterName + "\"]")).Displayed);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")).Click();
            //scroll in select
            var element = driver.FindElement(By.CssSelector("[data-e2e=\"" + filterItem + "\"]"));
            IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
            jse.ExecuteScript("arguments[0].scrollIntoView(true)", element);
            element = driver.FindElement(By.Id("header-top"));
            jse = (IJavaScriptExecutor)driver;
            jse.ExecuteScript("arguments[0].scrollIntoView(true)", element);
            driver.FindElement(By.CssSelector("[data-e2e=\"" + filterItem + "\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.TagName(tag)).Click();
            Thread.Sleep(1000);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            wait.Until(d => (bool)(d as IJavaScriptExecutor).ExecuteScript("return window.ajaxIsComplete();"));
            element = driver.FindElement(By.Id("header-top"));
            IJavaScriptExecutor jse1 = (IJavaScriptExecutor)driver;
            jse1.ExecuteScript("arguments[0].scrollIntoView(true)", element);
        }

        public static void GridDropdownDelete(IWebDriver driver, string baseURL)
        {
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownButton\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownItem\"][data-e2e-grid-selection-dropdown-index=\"0\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            wait.Until(d => (bool)(d as IJavaScriptExecutor).ExecuteScript("return window.ajaxIsComplete();"));
        }

        public static void GridDropdownTabDelete(IWebDriver driver, string baseURL, string gridId)
        {
            driver.FindElement(By.CssSelector("[grid-unique-id=\"" + gridId + "\"]")).FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownButton\"]")).Click();
            driver.FindElement(By.CssSelector("[grid-unique-id=\"" + gridId + "\"]")).FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownItem\"][data-e2e-grid-selection-dropdown-index=\"0\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            wait.Until(d => (bool)(d as IJavaScriptExecutor).ExecuteScript("return window.ajaxIsComplete();"));
        }

        public static void AddProductToListByFilter(IWebDriver driver, string linkText, string filter, string item, int tabIndex = 0, string gridCell = "")
        {
            driver.FindElement(By.LinkText(linkText)).Click();
            Thread.Sleep(3000);
            driver.FindElements(By.CssSelector(".header-subtext .btn.btn-sm.btn-action"))[tabIndex].Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterDropdownButton\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterDropdownItem\"][data-e2e-filter-dropdown-name=\"" + filter + "\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e-grid-filter-block-name=\"" + filter + "\"] input")).SendKeys(item);
            Thread.Sleep(2000);
            driver.FindElement(By.TagName("h2")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual(item, driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"gridProductsSelectvizr[0][\'" + gridCell + "\']\"]")).Text);
            driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"gridProductsSelectvizr[0][\'Name\']\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            Thread.Sleep(2000);
        }

        public static void DelElement(IWebDriver driver)
        {
            int count = driver.FindElements(By.CssSelector(".pull-right a")).Count;
            for (int i = 0; i < count; i++)
            {
                driver.FindElement(By.CssSelector(".pull-right a")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.ClassName("swal2-confirm")).Click();
                Thread.Sleep(2000);
            }
        }

        public static void AdminSettingsReviewsImgUploadingOn(IWebDriver driver, string baseURL)
        {
            driver.Navigate().GoToUrl(baseURL + "/adminv2/settingscatalog#?catalogTab=product");
            Thread.Sleep(3000);
            if
                  (!driver.FindElement(By.CssSelector("[data-e2e=\"allowReviewsImageUploading\"]")).FindElement(By.Id("AllowReviewsImageUploading")).Selected)
            {
                var element = driver.FindElement(By.CssSelector("[data-e2e=\"EnableZoom\"]"));
                IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
                jse.ExecuteScript("arguments[0].scrollIntoView(true)", element);
                Thread.Sleep(1000);
                driver.FindElement(By.CssSelector("[data-e2e=\"allowReviewsImageUploading\"]")).FindElement(By.TagName("span")).Click();
                Thread.Sleep(2000);
                element = driver.FindElement(By.Id("header-top"));
                IJavaScriptExecutor jse2 = (IJavaScriptExecutor)driver;
                jse2.ExecuteScript("arguments[0].scrollIntoView(true)", element);
                Thread.Sleep(1000);
                driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCatalogSave\"]")).Click();
                Thread.Sleep(4000);
                driver.Navigate().GoToUrl(baseURL + "/adminv2/settingscatalog#?catalogTab=product");
                Thread.Sleep(3000);
            }

            if
            (driver.FindElement(By.CssSelector("[data-e2e=\"moderateReviews\"]")).FindElement(By.Id("ModerateReviews")).Selected)
            {
                driver.Navigate().GoToUrl(baseURL + "/adminv2/settingscatalog#?catalogTab=product");
                Thread.Sleep(3000);
                var element = driver.FindElement(By.CssSelector("[data-e2e=\"EnableZoom\"]"));
                IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
                jse.ExecuteScript("arguments[0].scrollIntoView(true)", element);
                Thread.Sleep(1000);
                driver.FindElement(By.CssSelector("[data-e2e=\"moderateReviews\"]")).FindElement(By.TagName("span")).Click();
                Thread.Sleep(1000);
                element = driver.FindElement(By.Id("header-top"));
                IJavaScriptExecutor jse2 = (IJavaScriptExecutor)driver;
                jse2.ExecuteScript("arguments[0].scrollIntoView(true)", element);
                Thread.Sleep(1000);
                driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCatalogSave\"]")).Click();
                Thread.Sleep(4000);
                driver.Navigate().GoToUrl(baseURL + "/adminv2/settingscatalog#?catalogTab=product");
                Thread.Sleep(3000);
            }

            if
              (!driver.FindElement(By.CssSelector("[data-e2e=\"allowReviews\"]")).FindElement(By.Id("AllowReviews")).Selected)
            {
                driver.Navigate().GoToUrl(baseURL + "/adminv2/settingscatalog#?catalogTab=product");
                Thread.Sleep(3000);
                var element = driver.FindElement(By.CssSelector("[data-e2e=\"EnableZoom\"]"));
                IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
                jse.ExecuteScript("arguments[0].scrollIntoView(true)", element);
                Thread.Sleep(1000);
                driver.FindElement(By.CssSelector("[data-e2e=\"allowReviews\"]")).FindElement(By.TagName("span")).Click();
                Thread.Sleep(1000);
                element = driver.FindElement(By.Id("header-top"));
                IJavaScriptExecutor jse2 = (IJavaScriptExecutor)driver;
                jse2.ExecuteScript("arguments[0].scrollIntoView(true)", element);
                Thread.Sleep(1000);
                driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCatalogSave\"]")).Click();
                Thread.Sleep(4000);
                driver.Navigate().GoToUrl(baseURL + "/adminv2/settingscatalog#?catalogTab=product");
                Thread.Sleep(3000);
            }
        }

        public static void AdminSettingsReviewsImgUploadingOff(IWebDriver driver, string baseURL)
        {
            driver.Navigate().GoToUrl(baseURL + "/adminv2/settingscatalog#?catalogTab=product");
            Thread.Sleep(3000);
            if
                  (driver.FindElement(By.CssSelector("[data-e2e=\"allowReviewsImageUploading\"]")).FindElement(By.Id("AllowReviewsImageUploading")).Selected)
            {
                var element = driver.FindElement(By.CssSelector("[data-e2e=\"EnableZoom\"]"));
                IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
                jse.ExecuteScript("arguments[0].scrollIntoView(true)", element);
                Thread.Sleep(1000);
                driver.FindElement(By.CssSelector("[data-e2e=\"allowReviewsImageUploading\"]")).FindElement(By.TagName("span")).Click();
                Thread.Sleep(1000);
                element = driver.FindElement(By.Id("header-top"));
                IJavaScriptExecutor jse2 = (IJavaScriptExecutor)driver;
                jse2.ExecuteScript("arguments[0].scrollIntoView(true)", element);
                Thread.Sleep(1000);
                driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCatalogSave\"]")).Click();
                Thread.Sleep(4000);
                driver.Navigate().GoToUrl(baseURL + "/adminv2/settingscatalog#?catalogTab=product");
                Thread.Sleep(3000);
            }

            if
            (driver.FindElement(By.CssSelector("[data-e2e=\"moderateReviews\"]")).FindElement(By.Id("ModerateReviews")).Selected)
            {
                driver.Navigate().GoToUrl(baseURL + "/adminv2/settingscatalog#?catalogTab=product");
                Thread.Sleep(3000);
                var element = driver.FindElement(By.CssSelector("[data-e2e=\"EnableZoom\"]"));
                IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
                jse.ExecuteScript("arguments[0].scrollIntoView(true)", element);
                Thread.Sleep(1000);
                driver.FindElement(By.CssSelector("[data-e2e=\"moderateReviews\"]")).FindElement(By.TagName("span")).Click();
                Thread.Sleep(1000);
                element = driver.FindElement(By.Id("header-top"));
                IJavaScriptExecutor jse2 = (IJavaScriptExecutor)driver;
                jse2.ExecuteScript("arguments[0].scrollIntoView(true)", element);
                Thread.Sleep(1000);
                driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCatalogSave\"]")).Click();
                Thread.Sleep(4000);
                driver.Navigate().GoToUrl(baseURL + "/adminv2/settingscatalog#?catalogTab=product");
                Thread.Sleep(3000);
            }

            if
        (!driver.FindElement(By.CssSelector("[data-e2e=\"allowReviews\"]")).FindElement(By.Id("AllowReviews")).Selected)
            {
                driver.Navigate().GoToUrl(baseURL + "/adminv2/settingscatalog#?catalogTab=product");
                Thread.Sleep(3000);
                var element = driver.FindElement(By.CssSelector("[data-e2e=\"EnableZoom\"]"));
                IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
                jse.ExecuteScript("arguments[0].scrollIntoView(true)", element);
                Thread.Sleep(1000);
                driver.FindElement(By.CssSelector("[data-e2e=\"allowReviews\"]")).FindElement(By.TagName("span")).Click();
                Thread.Sleep(1000);
                element = driver.FindElement(By.Id("header-top"));
                IJavaScriptExecutor jse2 = (IJavaScriptExecutor)driver;
                jse2.ExecuteScript("arguments[0].scrollIntoView(true)", element);
                Thread.Sleep(1000);
                driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCatalogSave\"]")).Click();
                Thread.Sleep(4000);
                driver.Navigate().GoToUrl(baseURL + "/adminv2/settingscatalog#?catalogTab=product");
                Thread.Sleep(3000);
            }
        }

        public static void AdminSettingsReviewsOn(IWebDriver driver, string baseURL)
        {
            driver.Navigate().GoToUrl(baseURL + "/adminv2/settingscatalog#?catalogTab=product");
            Thread.Sleep(3000);
            if
                  (!driver.FindElement(By.CssSelector("[data-e2e=\"allowReviews\"]")).FindElement(By.Id("AllowReviews")).Selected)
            {
                var element = driver.FindElement(By.CssSelector("[data-e2e=\"EnableZoom\"]"));
                IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
                jse.ExecuteScript("arguments[0].scrollIntoView(true)", element);
                Thread.Sleep(1000);
                driver.FindElement(By.CssSelector("[data-e2e=\"allowReviews\"]")).FindElement(By.TagName("span")).Click();
                Thread.Sleep(1000);
                element = driver.FindElement(By.Id("header-top"));
                IJavaScriptExecutor jse2 = (IJavaScriptExecutor)driver;
                jse2.ExecuteScript("arguments[0].scrollIntoView(true)", element);
                Thread.Sleep(1000);
                driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCatalogSave\"]")).Click();
                Thread.Sleep(4000);
                driver.Navigate().GoToUrl(baseURL + "/adminv2/settingscatalog#?catalogTab=product");
                Thread.Sleep(3000);
            }

            if
            (driver.FindElement(By.CssSelector("[data-e2e=\"moderateReviews\"]")).FindElement(By.Id("ModerateReviews")).Selected)
            {
                driver.Navigate().GoToUrl(baseURL + "/adminv2/settingscatalog#?catalogTab=product");
                Thread.Sleep(3000);
                var element = driver.FindElement(By.CssSelector("[data-e2e=\"EnableZoom\"]"));
                IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
                jse.ExecuteScript("arguments[0].scrollIntoView(true)", element);
                Thread.Sleep(1000);
                driver.FindElement(By.CssSelector("[data-e2e=\"moderateReviews\"]")).FindElement(By.TagName("span")).Click();
                Thread.Sleep(1000);
                element = driver.FindElement(By.Id("header-top"));
                IJavaScriptExecutor jse2 = (IJavaScriptExecutor)driver;
                jse2.ExecuteScript("arguments[0].scrollIntoView(true)", element);
                Thread.Sleep(1000);
                driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCatalogSave\"]")).Click();
                Thread.Sleep(4000);
                driver.Navigate().GoToUrl(baseURL + "/adminv2/settingscatalog#?catalogTab=product");
                Thread.Sleep(3000);
            }
        }

        public static void AdminSettingsReviewsOff(IWebDriver driver, string baseURL)
        {
            driver.Navigate().GoToUrl(baseURL + "/adminv2/settingscatalog#?catalogTab=product");
            Thread.Sleep(3000);
            if
                  (driver.FindElement(By.CssSelector("[data-e2e=\"allowReviews\"]")).FindElement(By.Id("AllowReviews")).Selected)
            {
                var element = driver.FindElement(By.CssSelector("[data-e2e=\"EnableZoom\"]"));
                IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
                jse.ExecuteScript("arguments[0].scrollIntoView(true)", element);
                Thread.Sleep(1000);
                driver.FindElement(By.CssSelector("[data-e2e=\"allowReviews\"]")).FindElement(By.TagName("span")).Click();
                Thread.Sleep(1000);
                element = driver.FindElement(By.Id("header-top"));
                IJavaScriptExecutor jse2 = (IJavaScriptExecutor)driver;
                jse2.ExecuteScript("arguments[0].scrollIntoView(true)", element);
                Thread.Sleep(1000);
                driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCatalogSave\"]")).Click();
                Thread.Sleep(4000);
                driver.Navigate().GoToUrl(baseURL + "/adminv2/settingscatalog#?catalogTab=product");
                Thread.Sleep(3000);
            }
        }

        public static void AdminSettingsReviewsShowImgOn(IWebDriver driver, string baseURL)
        {
            driver.Navigate().GoToUrl(baseURL + "/adminv2/settingscatalog#?catalogTab=product");
            Thread.Sleep(3000);
            if
                  (!driver.FindElement(By.CssSelector("[data-e2e=\"displayReviewsImage\"]")).FindElement(By.Id("DisplayReviewsImage")).Selected)
            {
                var element = driver.FindElement(By.CssSelector("[data-e2e=\"EnableZoom\"]"));
                IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
                jse.ExecuteScript("arguments[0].scrollIntoView(true)", element);
                Thread.Sleep(1000);
                driver.FindElement(By.CssSelector("[data-e2e=\"displayReviewsImage\"]")).FindElement(By.TagName("span")).Click();
                Thread.Sleep(1000);
                element = driver.FindElement(By.Id("header-top"));
                IJavaScriptExecutor jse2 = (IJavaScriptExecutor)driver;
                jse2.ExecuteScript("arguments[0].scrollIntoView(true)", element);
                Thread.Sleep(1000);
                driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCatalogSave\"]")).Click();
                Thread.Sleep(4000);
                driver.Navigate().GoToUrl(baseURL + "/adminv2/settingscatalog#?catalogTab=product");
                Thread.Sleep(3000);
            }

            if
            (driver.FindElement(By.CssSelector("[data-e2e=\"moderateReviews\"]")).FindElement(By.Id("ModerateReviews")).Selected)
            {
                driver.Navigate().GoToUrl(baseURL + "/adminv2/settingscatalog#?catalogTab=product");
                Thread.Sleep(3000);
                var element = driver.FindElement(By.CssSelector("[data-e2e=\"EnableZoom\"]"));
                IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
                jse.ExecuteScript("arguments[0].scrollIntoView(true)", element);
                Thread.Sleep(1000);
                driver.FindElement(By.CssSelector("[data-e2e=\"moderateReviews\"]")).FindElement(By.TagName("span")).Click();
                Thread.Sleep(1000);
                element = driver.FindElement(By.Id("header-top"));
                IJavaScriptExecutor jse2 = (IJavaScriptExecutor)driver;
                jse2.ExecuteScript("arguments[0].scrollIntoView(true)", element);
                Thread.Sleep(1000);
                driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCatalogSave\"]")).Click();
                Thread.Sleep(4000);
                driver.Navigate().GoToUrl(baseURL + "/adminv2/settingscatalog#?catalogTab=product");
                Thread.Sleep(3000);
            }

            if
        (!driver.FindElement(By.CssSelector("[data-e2e=\"allowReviews\"]")).FindElement(By.Id("AllowReviews")).Selected)
            {
                driver.Navigate().GoToUrl(baseURL + "/adminv2/settingscatalog#?catalogTab=product");
                Thread.Sleep(3000);
                var element = driver.FindElement(By.CssSelector("[data-e2e=\"EnableZoom\"]"));
                IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
                jse.ExecuteScript("arguments[0].scrollIntoView(true)", element);
                Thread.Sleep(1000);
                driver.FindElement(By.CssSelector("[data-e2e=\"allowReviews\"]")).FindElement(By.TagName("span")).Click();
                Thread.Sleep(1000);
                element = driver.FindElement(By.Id("header-top"));
                IJavaScriptExecutor jse2 = (IJavaScriptExecutor)driver;
                jse2.ExecuteScript("arguments[0].scrollIntoView(true)", element);
                Thread.Sleep(1000);
                driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCatalogSave\"]")).Click();
                Thread.Sleep(4000);
                driver.Navigate().GoToUrl(baseURL + "/adminv2/settingscatalog#?catalogTab=product");
                Thread.Sleep(3000);
            }
        }

        public static void AdminSettingsReviewsModerateOn(IWebDriver driver, string baseURL)
        {
            driver.Navigate().GoToUrl(baseURL + "/adminv2/settingscatalog#?catalogTab=product");
            Thread.Sleep(3000);
            if
                (!driver.FindElement(By.CssSelector("[data-e2e=\"moderateReviews\"]")).FindElement(By.Id("ModerateReviews")).Selected)
            {
                var element = driver.FindElement(By.CssSelector("[data-e2e=\"EnableZoom\"]"));
                IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
                jse.ExecuteScript("arguments[0].scrollIntoView(true)", element);
                Thread.Sleep(1000);
                driver.FindElement(By.CssSelector("[data-e2e=\"moderateReviews\"]")).FindElement(By.TagName("span")).Click();
                Thread.Sleep(1000);
                element = driver.FindElement(By.Id("header-top"));
                IJavaScriptExecutor jse2 = (IJavaScriptExecutor)driver;
                jse2.ExecuteScript("arguments[0].scrollIntoView(true)", element);
                Thread.Sleep(1000);
                driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCatalogSave\"]")).Click();
                Thread.Sleep(4000);
                driver.Navigate().GoToUrl(baseURL + "/adminv2/settingscatalog#?catalogTab=product");
                Thread.Sleep(3000);
            }

            if
        (!driver.FindElement(By.CssSelector("[data-e2e=\"allowReviews\"]")).FindElement(By.Id("AllowReviews")).Selected)
            {
                driver.Navigate().GoToUrl(baseURL + "/adminv2/settingscatalog#?catalogTab=product");
                Thread.Sleep(3000);
                var element = driver.FindElement(By.CssSelector("[data-e2e=\"EnableZoom\"]"));
                IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
                jse.ExecuteScript("arguments[0].scrollIntoView(true)", element);
                Thread.Sleep(1000);
                driver.FindElement(By.CssSelector("[data-e2e=\"allowReviews\"]")).FindElement(By.TagName("span")).Click();
                Thread.Sleep(1000);
                element = driver.FindElement(By.Id("header-top"));
                IJavaScriptExecutor jse2 = (IJavaScriptExecutor)driver;
                jse2.ExecuteScript("arguments[0].scrollIntoView(true)", element);
                Thread.Sleep(1000);
                driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCatalogSave\"]")).Click();
                Thread.Sleep(4000);
                driver.Navigate().GoToUrl(baseURL + "/adminv2/settingscatalog#?catalogTab=product");
                Thread.Sleep(3000);
            }

        }

        public static void AdminSettingsReviewsModerateOff(IWebDriver driver, string baseURL)
        {
            driver.Navigate().GoToUrl(baseURL + "/adminv2/settingscatalog#?catalogTab=product");
            Thread.Sleep(3000);
            if
                  (driver.FindElement(By.CssSelector("[data-e2e=\"moderateReviews\"]")).FindElement(By.Id("ModerateReviews")).Selected)
            {
                var element = driver.FindElement(By.CssSelector("[data-e2e=\"EnableZoom\"]"));
                IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
                jse.ExecuteScript("arguments[0].scrollIntoView(true)", element);
                Thread.Sleep(1000);
                driver.FindElement(By.CssSelector("[data-e2e=\"moderateReviews\"]")).FindElement(By.TagName("span")).Click();
                Thread.Sleep(1000);
                element = driver.FindElement(By.Id("header-top"));
                IJavaScriptExecutor jse2 = (IJavaScriptExecutor)driver;
                jse2.ExecuteScript("arguments[0].scrollIntoView(true)", element);
                Thread.Sleep(1000);
                driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCatalogSave\"]")).Click();
                Thread.Sleep(4000);
                driver.Navigate().GoToUrl(baseURL + "/adminv2/settingscatalog#?catalogTab=product");
                Thread.Sleep(3000);
            }

            if
        (!driver.FindElement(By.CssSelector("[data-e2e=\"allowReviews\"]")).FindElement(By.Id("AllowReviews")).Selected)
            {
                driver.Navigate().GoToUrl(baseURL + "/adminv2/settingscatalog#?catalogTab=product");
                Thread.Sleep(3000);
                var element = driver.FindElement(By.CssSelector("[data-e2e=\"EnableZoom\"]"));
                IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
                jse.ExecuteScript("arguments[0].scrollIntoView(true)", element);
                Thread.Sleep(1000);
                driver.FindElement(By.CssSelector("[data-e2e=\"allowReviews\"]")).FindElement(By.TagName("span")).Click();
                Thread.Sleep(1000);
                element = driver.FindElement(By.Id("header-top"));
                IJavaScriptExecutor jse2 = (IJavaScriptExecutor)driver;
                jse2.ExecuteScript("arguments[0].scrollIntoView(true)", element);
                Thread.Sleep(1000);
                driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCatalogSave\"]")).Click();
                Thread.Sleep(4000);
                driver.Navigate().GoToUrl(baseURL + "/adminv2/settingscatalog#?catalogTab=product");
                Thread.Sleep(3000);
            }
        }

        public static void AdminSettingsReviewsShowImgOff(IWebDriver driver, string baseURL)
        {
            driver.Navigate().GoToUrl(baseURL + "/adminv2/settingscatalog#?catalogTab=product");
            Thread.Sleep(3000);
            if
                  (driver.FindElement(By.CssSelector("[data-e2e=\"displayReviewsImage\"]")).FindElement(By.Id("DisplayReviewsImage")).Selected)
            {
                var element = driver.FindElement(By.CssSelector("[data-e2e=\"EnableZoom\"]"));
                IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
                jse.ExecuteScript("arguments[0].scrollIntoView(true)", element);
                Thread.Sleep(1000);
                driver.FindElement(By.CssSelector("[data-e2e=\"displayReviewsImage\"]")).FindElement(By.TagName("span")).Click();
                Thread.Sleep(1000);
                element = driver.FindElement(By.Id("header-top"));
                IJavaScriptExecutor jse2 = (IJavaScriptExecutor)driver;
                jse2.ExecuteScript("arguments[0].scrollIntoView(true)", element);
                Thread.Sleep(1000);
                driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCatalogSave\"]")).Click();
                Thread.Sleep(4000);
                driver.Navigate().GoToUrl(baseURL + "/adminv2/settingscatalog#?catalogTab=product");
                Thread.Sleep(3000);
            }

            if
            (driver.FindElement(By.CssSelector("[data-e2e=\"moderateReviews\"]")).FindElement(By.Id("ModerateReviews")).Selected)
            {
                driver.Navigate().GoToUrl(baseURL + "/adminv2/settingscatalog#?catalogTab=product");
                Thread.Sleep(3000);
                var element = driver.FindElement(By.CssSelector("[data-e2e=\"EnableZoom\"]"));
                IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
                jse.ExecuteScript("arguments[0].scrollIntoView(true)", element);
                Thread.Sleep(1000);
                driver.FindElement(By.CssSelector("[data-e2e=\"moderateReviews\"]")).FindElement(By.TagName("span")).Click();
                Thread.Sleep(1000);
                element = driver.FindElement(By.Id("header-top"));
                IJavaScriptExecutor jse2 = (IJavaScriptExecutor)driver;
                jse2.ExecuteScript("arguments[0].scrollIntoView(true)", element);
                Thread.Sleep(1000);
                driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCatalogSave\"]")).Click();
                Thread.Sleep(4000);
                driver.Navigate().GoToUrl(baseURL + "/adminv2/settingscatalog#?catalogTab=product");
                Thread.Sleep(3000);
            }

            if
        (!driver.FindElement(By.CssSelector("[data-e2e=\"allowReviews\"]")).FindElement(By.Id("AllowReviews")).Selected)
            {
                driver.Navigate().GoToUrl(baseURL + "/adminv2/settingscatalog#?catalogTab=product");
                Thread.Sleep(3000);
                var element = driver.FindElement(By.CssSelector("[data-e2e=\"EnableZoom\"]"));
                IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
                jse.ExecuteScript("arguments[0].scrollIntoView(true)", element);
                Thread.Sleep(1000);
                driver.FindElement(By.CssSelector("[data-e2e=\"allowReviews\"]")).FindElement(By.TagName("span")).Click();
                Thread.Sleep(1000);
                element = driver.FindElement(By.Id("header-top"));
                IJavaScriptExecutor jse2 = (IJavaScriptExecutor)driver;
                jse2.ExecuteScript("arguments[0].scrollIntoView(true)", element);
                Thread.Sleep(1000);
                driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCatalogSave\"]")).Click();
                Thread.Sleep(4000);
                driver.Navigate().GoToUrl(baseURL + "/adminv2/settingscatalog#?catalogTab=product");
                Thread.Sleep(3000);
            }
        }
        
        public static void GridFilterChecked(IWebDriver driver, string baseURL, string filter, string tag)
        {
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterDropdownButton\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterDropdownItem\"][data-e2e-filter-dropdown-name=\"Checked\"]")).Click();
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText(filter);
            driver.FindElement(By.TagName(tag)).Click();
            Thread.Sleep(1000);
        }
        
        //для любого фильтра
        public static void GridFilterSet(IWebDriver driver, string baseURL, string name)
        {
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterDropdownButton\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterDropdownItem\"][data-e2e-filter-dropdown-name=\"" + name + "\"]")).Click();
            Thread.Sleep(1000);
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"" + name + "\"]")).Displayed);
        }
        public static void GridFilterClose(IWebDriver driver, string baseURL, string name)
        {
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"" + name + "\"] [data-e2e=\"gridFilterItemClose\"]")).Click();
            Thread.Sleep(1000);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            wait.Until(d => (bool)(d as IJavaScriptExecutor).ExecuteScript("return window.ajaxIsComplete();"));
            Assert.IsFalse(driver.FindElements(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"" + name + "\"]")).Count > 0);
        }

        //для фильтра где табы
        public static void GridFilterTabSet(IWebDriver driver, string baseURL, string name, string gridId)
        {
            driver.FindElement(By.CssSelector("[grid-unique-id=\"" + gridId + "\"]")).FindElement(By.CssSelector("[data-e2e=\"gridFilterDropdownButton\"]")).Click();
            driver.FindElement(By.CssSelector("[grid-unique-id=\"" + gridId + "\"]")).FindElement(By.CssSelector("[data-e2e=\"gridFilterDropdownItem\"][data-e2e-filter-dropdown-name=\"" + name + "\"]")).Click();
            Assert.IsTrue(driver.FindElement(By.CssSelector("[grid-unique-id=\"" + gridId + "\"]")).FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"" + name + "\"]")).Displayed);
        }

        public static void GridFilterTabClose(IWebDriver driver, string baseURL, string name, string gridId)
        {
            driver.FindElement(By.CssSelector("[grid-unique-id=\"" + gridId + "\"]")).FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"" + name + "\"] [data-e2e=\"gridFilterItemClose\"]")).Click();
            Thread.Sleep(1000);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            wait.Until(d => (bool)(d as IJavaScriptExecutor).ExecuteScript("return window.ajaxIsComplete();"));

            Assert.IsFalse(driver.FindElement(By.CssSelector("[grid-unique-id=\"" + gridId + "\"]")).FindElements(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"" + name + "\"]")).Count > 0);
        }

        public static void AdminSettingsProductCart(IWebDriver driver, string baseURL)
        {
            driver.Navigate().GoToUrl(baseURL + "/adminv2/settingscatalog#?catalogTab=product");
            Thread.Sleep(2000);

            if (!driver.FindElement(By.Id("DisplayWeight")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"DisplayWeight\"]")).Click();
                Thread.Sleep(500);
                driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCatalogSave\"]")).Click();
                Thread.Sleep(2000);

                driver.Navigate().GoToUrl(baseURL + "/adminv2/settingscatalog#?catalogTab=product");
                Thread.Sleep(2000);
            }

            if (!driver.FindElement(By.Id("DisplayDimensions")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"DisplayDimensions\"]")).Click();
                Thread.Sleep(500);
                driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCatalogSave\"]")).Click();
                Thread.Sleep(2000);
            }
        }
        public static void DelProductCart(IWebDriver driver, string baseURL)
        {
            driver.Navigate().GoToUrl(baseURL + "/cart");
            Thread.Sleep(2000);
            if (driver.FindElements(By.CssSelector(".cart-full-body-item.cart-full-remove")).Count > 0)
            {
                driver.FindElement(By.CssSelector(".cart-full-body-item.cart-full-remove")).Click();
            }
        }
        public static void AdminMobileOn(IWebDriver driver, string baseURL)
        {
            driver.Navigate().GoToUrl(baseURL + "/adminv2/settings/mobileversion");
            Thread.Sleep(1000);
            if (!driver.FindElement(By.CssSelector("[data-e2e=\"mobileEnabled\"]")).FindElement(By.Id("Enabled")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"mobileEnabled\"]")).FindElement(By.TagName("span")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.CssSelector("[data-e2e=\"mobileSave\"]")).Click();
                Thread.Sleep(2000);
            }
        }

        public static void AdminMobileOff(IWebDriver driver, string baseURL)
        {
            driver.Navigate().GoToUrl(baseURL + "/adminv2/settings/mobileversion");
            Thread.Sleep(1000);

            driver.FindElement(By.CssSelector("[data-e2e=\"mobileEnabled\"]")).FindElement(By.TagName("span")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"mobileSave\"]")).Click();
            Thread.Sleep(2000);
        }
        public static void AdminMobileCheckoutOn(IWebDriver driver, string baseURL)
        {
            driver.Navigate().GoToUrl(baseURL + "/adminv2/settings/mobileversion");
            Thread.Sleep(1000);
            if (driver.FindElement(By.CssSelector("[data-e2e=\"mobileFullCheckout\"]")).FindElement(By.Name("IsFullCheckout")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"mobileFullCheckout\"]")).FindElement(By.TagName("span")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.CssSelector("[data-e2e=\"mobileSave\"]")).Click();
                Thread.Sleep(2000);
            }
        }

        public static void AdminMobileCheckoutOff(IWebDriver driver, string baseURL)
        {
            driver.Navigate().GoToUrl(baseURL + "/adminv2/settings/mobileversion");
            Thread.Sleep(1000);
            if (!driver.FindElement(By.CssSelector("[data-e2e=\"mobileFullCheckout\"]")).FindElement(By.Name("IsFullCheckout")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"mobileFullCheckout\"]")).FindElement(By.TagName("span")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.CssSelector("[data-e2e=\"mobileSave\"]")).Click();
                Thread.Sleep(2000);
            }
        }
        public static void TemplateSettingsSelect(IWebDriver driver, string baseURL, string select, string settingsName)
        {
            driver.Navigate().GoToUrl(baseURL + "/adminv2/design");
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".btn.btn-sm.btn-action.other-btn")).Click();
            Thread.Sleep(2000);
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"" + settingsName + "\"]")).FindElement(By.TagName("select")))).SelectByText(select);
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            Thread.Sleep(1000);
        }

        public static void ExportProductsNoInCategoryOn(IWebDriver driver, string baseURL)
        {
            if (!driver.FindElement(By.Name("CsvExportNoInCategory")).Selected)

            {
                var element = driver.FindElement(By.Name("CsvPropertySeparator"));
                IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
                jse.ExecuteScript("arguments[0].scrollIntoView(true)", element);
                Thread.Sleep(1000);

                driver.FindElement(By.CssSelector("[data-e2e=\"CsvExportNoInCategory\"] span")).Click();
            }
        }

        public static void ExportProductsNoInCategoryOff(IWebDriver driver, string baseURL)
        {
            if (driver.FindElement(By.Name("CsvExportNoInCategory")).Selected)

            {
                var element = driver.FindElement(By.Name("CsvPropertySeparator"));
                IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
                jse.ExecuteScript("arguments[0].scrollIntoView(true)", element);
                Thread.Sleep(1000);

                driver.FindElement(By.CssSelector("[data-e2e=\"CsvExportNoInCategory\"] span")).Click();
            }

        }

        public static void ExportProductsCategorySortOn(IWebDriver driver, string baseURL)
        {
            if (!driver.FindElement(By.Name("CsvCategorySort")).Selected)

            {
                var element = driver.FindElement(By.Name("CsvExportNoInCategory"));
                IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
                jse.ExecuteScript("arguments[0].scrollIntoView(true)", element);
                Thread.Sleep(1000);

                driver.FindElement(By.CssSelector("[data-e2e=\"CsvCategorySort\"] span")).Click();
            }
        }

        public static void ExportProductsCategorySortOff(IWebDriver driver, string baseURL)
        {
            if (driver.FindElement(By.Name("CsvCategorySort")).Selected)

            {
                var element = driver.FindElement(By.Name("CsvExportNoInCategory"));
                IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
                jse.ExecuteScript("arguments[0].scrollIntoView(true)", element);
                Thread.Sleep(1000);

                driver.FindElement(By.CssSelector("[data-e2e=\"CsvCategorySort\"] span")).Click();
            }
        }

        public static void CloseTab(IWebDriver driver, string baseURL)
        {
            driver.Close();
            Thread.Sleep(2000);
            ReadOnlyCollection<String> windowHandles = driver.WindowHandles;
            String PreviousTab = windowHandles[windowHandles.Count - 1];
            driver.SwitchTo().Window(PreviousTab);
            Thread.Sleep(2000);
        }

        public static void OpenNewTab(IWebDriver driver, string baseURL)
        {
            ReadOnlyCollection<String> windowHandles = driver.WindowHandles;
            String NextTab = windowHandles[windowHandles.Count - 1];
            driver.SwitchTo().Window(NextTab);
            Thread.Sleep(2000);
        }

        public static void NewOrderClient_450(IWebDriver driver, string baseURL)
        {
            driver.Navigate().GoToUrl(baseURL + "/products/test-product5");
            Thread.Sleep(2000);

            var element = driver.FindElement(By.CssSelector(".link-dotted-invert.cs-l-d-1"));
            IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
            jse.ExecuteScript("arguments[0].scrollIntoView(true)", element);
            Thread.Sleep(1000);

            driver.FindElement(By.CssSelector(".link-dotted-invert.cs-l-d-1")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".buy-one-click-buttons input")).Click();
            Thread.Sleep(2000);
        }
        public static void NewOrderClient_1350(IWebDriver driver, string baseURL)
        {
            driver.Navigate().GoToUrl(baseURL + "/products/test-product15");
            Thread.Sleep(2000);
            var element = driver.FindElement(By.CssSelector(".link-dotted-invert.cs-l-d-1"));
            IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
            jse.ExecuteScript("arguments[0].scrollIntoView(true)", element);
            Thread.Sleep(1000);

            driver.FindElement(By.CssSelector(".link-dotted-invert.cs-l-d-1")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".buy-one-click-buttons input")).Click();
            Thread.Sleep(2000);
        }
        public static void NewOrderClient_9000(IWebDriver driver, string baseURL)
        {
            driver.Navigate().GoToUrl(baseURL + "/products/test-product100");
            Thread.Sleep(2000);
            var element = driver.FindElement(By.CssSelector(".link-dotted-invert.cs-l-d-1"));
            IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
            jse.ExecuteScript("arguments[0].scrollIntoView(true)", element);
            Thread.Sleep(1000);

            driver.FindElement(By.CssSelector(".link-dotted-invert.cs-l-d-1")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".buy-one-click-buttons input")).Click();
            Thread.Sleep(2000);
        }
        public static void NewFullOrderClient_9000(IWebDriver driver, string baseURL)
        {
            driver.Navigate().GoToUrl(baseURL + "/products/test-product100");
            Thread.Sleep(2000);
            var element = driver.FindElement(By.CssSelector(".link-dotted-invert.cs-l-d-1"));
            IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
            jse.ExecuteScript("arguments[0].scrollIntoView(true)", element);
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector(".btn.btn-middle.btn-confirm.icon-bag-before")).Click();
            Thread.Sleep(1000);

            driver.Navigate().GoToUrl(baseURL + "/checkout");

            Thread.Sleep(5000);
            element = driver.FindElement(By.CssSelector(".checkout-result"));
            jse = (IJavaScriptExecutor)driver;
            jse.ExecuteScript("arguments[0].scrollIntoView(true)", element);
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"btnCheckout\"]")).Click();
            Thread.Sleep(2000);

        }
        public static void productToCart(IWebDriver driver, string baseURL, string id)
        {
            driver.Navigate().GoToUrl(baseURL + id);
            Thread.Sleep(2000);
            var element = driver.FindElement(By.CssSelector(".link-dotted-invert.cs-l-d-1"));
            IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
            jse.ExecuteScript("arguments[0].scrollIntoView(true)", element);
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector(".btn.btn-middle.btn-confirm.icon-bag-before")).Click();
            Thread.Sleep(1000);
        }
        public static void checkSelected(string id, IWebDriver driver)
        {
            if (!driver.FindElement(By.Id(id)).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"" + id + "\"]")).Click();
                Thread.Sleep(2000);
            }
        }

        public static void checkNotSelected(string id, IWebDriver driver)
        {
            if (driver.FindElement(By.Id(id)).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"" + id + "\"]")).Click();
                Thread.Sleep(2000);
            }

        }
        public static void CleanCart(IWebDriver driver, string baseURL)
        {
            driver.Navigate().GoToUrl(baseURL + "/cart");
            if (driver.FindElements(By.CssSelector(".cart-full-product")).Count > 0)
            {
                driver.FindElement(By.CssSelector(".cart-full-remove a")).Click();
                Thread.Sleep(2000);
            }

        }
        public static void KanbanTaskOn(IWebDriver driver, string baseURL)
        {
            driver.Navigate().GoToUrl(baseURL + "/adminv2/tasks");
            if (!driver.FindElement(By.Name("UseKanban")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"UseKanban\"]")).Click();
                Thread.Sleep(2000);
            }
        }
        public static void KanbanTaskOff(IWebDriver driver, string baseURL)
        {
            driver.Navigate().GoToUrl(baseURL + "/adminv2/tasks");
            if (driver.FindElement(By.CssSelector("UseKanban")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"UseGrid\"]")).Click();
                Thread.Sleep(2000);
            }
        }

        public static void DragDropKanban(IWebDriver driver, IWebElement dragElement, IWebElement dropElement)
        {
            Actions builder = new Actions(driver);
            builder.ClickAndHold(dragElement).MoveToElement(dropElement).Release().Build().Perform();
            Thread.Sleep(1000);
           
        }
        public static void DataTimePickerFilter(IWebDriver driver, string baseURL, string Monthfrom ="", string Yearfrom = "", string Datafrom = "", string Hourfrom = "", string Minfrom = "", string Monthto = "", string Yearto = "", string Datato = "", string Hourto = "", string Minto = "", string tagH = "", string fieldFrom ="", string fieldTo = "")
        {
            if (fieldFrom != "")

            {
                driver.FindElement(By.CssSelector("" + fieldFrom + "")).Click();
                driver.FindElement(By.CssSelector("" + fieldFrom + "")).Clear();
            }

            if (Monthfrom != "")
            {

                var curMonth = driver.FindElement(By.CssSelector(".flatpickr-calendar.hasTime.animate.arrowTop.open")).FindElement(By.CssSelector(".cur-month")).Text;
                if (!curMonth.Contains(Monthfrom))
                {

                    while (!curMonth.Contains(Monthfrom))
                    {
                        driver.FindElement(By.CssSelector(".flatpickr-calendar.hasTime.animate.arrowTop.open")).FindElement(By.CssSelector(".flatpickr-next-month")).Click();
                        Thread.Sleep(2000);
                        curMonth = driver.FindElement(By.CssSelector(".flatpickr-calendar.hasTime.animate.arrowTop.open")).FindElement(By.CssSelector(".cur-month")).Text;
                    }
                }

                var curYear = driver.FindElement(By.CssSelector(".flatpickr-calendar.hasTime.animate.arrowTop.open")).FindElement(By.CssSelector(".numInput.cur-year")).GetAttribute("value");
                if (!curYear.Contains(Yearfrom))
                {

                    while (!curYear.Contains(Yearfrom))
                    {
                        driver.FindElement(By.CssSelector(".flatpickr-calendar.hasTime.animate.arrowTop.open")).FindElement(By.CssSelector(".flatpickr-month")).FindElement(By.CssSelector(".arrowDown")).Click();
                        Thread.Sleep(2000);
                        curYear = driver.FindElement(By.CssSelector(".flatpickr-calendar.hasTime.animate.arrowTop.open")).FindElement(By.CssSelector(".numInput.cur-year")).GetAttribute("value");
                    }
                }

                IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
                jse.ExecuteScript("arguments[0].scrollIntoView(true)", driver.FindElement(By.CssSelector(tagH)));
                Thread.Sleep(1000);

                driver.FindElement(By.CssSelector("[aria-label=\"" + Datafrom + "\"]")).Click();
                Thread.Sleep(1000);

                if (Hourfrom != "")
                {
                    var curTimeHour = driver.FindElement(By.CssSelector(".flatpickr-calendar.hasTime.animate.arrowTop.open")).FindElement(By.CssSelector(".numInput.flatpickr-hour")).GetAttribute("value");
                    if (!curTimeHour.Contains(Hourfrom))
                    {

                        while (!curTimeHour.Contains(Hourfrom))
                        {
                            driver.FindElement(By.CssSelector(".flatpickr-calendar.hasTime.animate.arrowTop.open")).FindElement(By.CssSelector(".flatpickr-time.time24hr")).FindElement(By.CssSelector(".arrowUp")).Click();
                            Thread.Sleep(2000);
                            curTimeHour = driver.FindElement(By.CssSelector(".flatpickr-calendar.hasTime.animate.arrowTop.open")).FindElement(By.CssSelector(".numInput.flatpickr-hour")).GetAttribute("value");
                        }
                    }
                }
                if (Minfrom != "")
                { 
                    var curTimeMin = driver.FindElements(By.CssSelector(".numInput.flatpickr-minute"))[0].GetAttribute("value");
                    if (!curTimeMin.Contains(Minfrom))
                    {

                        while (!curTimeMin.Contains(Minfrom))
                        {
                            driver.FindElement(By.CssSelector(".flatpickr-calendar.hasTime.animate.arrowTop.open")).FindElement(By.CssSelector(".flatpickr-time.time24hr")).FindElements(By.CssSelector(".arrowUp"))[1].Click();
                            Thread.Sleep(2000);
                            curTimeMin = driver.FindElements(By.CssSelector(".numInput.flatpickr-minute"))[0].GetAttribute("value");
                        }
                    }
                }
                jse.ExecuteScript("arguments[0].scrollIntoView(true)", driver.FindElement(By.Id("header-top")));
                Thread.Sleep(1000);
                driver.FindElement(By.TagName(tagH)).Click();
                Thread.Sleep(1000);
            }

            if (fieldTo != "")
            {
                driver.FindElement(By.CssSelector("" + fieldTo + "")).Click();
                driver.FindElement(By.CssSelector("" + fieldTo + "")).Clear();
            }

            if (Monthto != "")
            {
                Thread.Sleep(2000);
                var curMonth = driver.FindElement(By.CssSelector(".flatpickr-calendar.hasTime.animate.arrowTop.open")).FindElement(By.CssSelector(".cur-month")).Text;
                if (!curMonth.Contains(Monthto))
                {

                    while (!curMonth.Contains(Monthto))
                    {
                        driver.FindElement(By.CssSelector(".flatpickr-calendar.hasTime.animate.arrowTop.open")).FindElement(By.CssSelector(".flatpickr-next-month")).Click();
                        Thread.Sleep(2000);
                        curMonth = driver.FindElement(By.CssSelector(".flatpickr-calendar.hasTime.animate.arrowTop.open")).FindElement(By.CssSelector(".cur-month")).Text;
                    }
                }

                var curYear = driver.FindElement(By.CssSelector(".flatpickr-calendar.hasTime.animate.arrowTop.open")).FindElement(By.CssSelector(".numInput.cur-year")).GetAttribute("value");
                if (!curYear.Contains(Yearto))
                {

                    while (!curYear.Contains(Yearto))
                    {
                        driver.FindElement(By.CssSelector(".flatpickr-calendar.hasTime.animate.arrowTop.open")).FindElement(By.CssSelector(".flatpickr-month")).FindElement(By.CssSelector(".arrowDown")).Click();
                        Thread.Sleep(2000);
                        curYear = driver.FindElement(By.CssSelector(".flatpickr-calendar.hasTime.animate.arrowTop.open")).FindElement(By.CssSelector(".numInput.cur-year")).GetAttribute("value");
                    }
                }

                if (Monthfrom == Monthto)
                {
                    driver.FindElements(By.CssSelector("[aria-label=\"" + Datato + "\"]"))[1].Click();
                    Thread.Sleep(1000);
                }
                else { 
                driver.FindElement(By.CssSelector("[aria-label=\"" + Datato + "\"]")).Click();
                Thread.Sleep(1000);
                }

                if (Hourto != "")
                {
                    var curTimeHour = driver.FindElement(By.CssSelector(".flatpickr-calendar.hasTime.animate.arrowTop.open")).FindElement(By.CssSelector(".numInput.flatpickr-hour")).GetAttribute("value");
                    if (!curTimeHour.Contains(Hourto))
                    {

                        while (!curTimeHour.Contains(Hourto))
                        {
                            driver.FindElement(By.CssSelector(".flatpickr-calendar.hasTime.animate.arrowTop.open")).FindElement(By.CssSelector(".flatpickr-time.time24hr")).FindElement(By.CssSelector(".arrowUp")).Click();
                            Thread.Sleep(2000);
                            curTimeHour = driver.FindElement(By.CssSelector(".flatpickr-calendar.hasTime.animate.arrowTop.open")).FindElement(By.CssSelector(".numInput.flatpickr-hour")).GetAttribute("value");
                        }
                    }
                }

                if (Minto != "")
                {
                    var curTimeMin = driver.FindElement(By.CssSelector(".flatpickr-calendar.hasTime.animate.arrowTop.open")).FindElement(By.CssSelector(".numInput.flatpickr-minute")).GetAttribute("value");
                    if (!curTimeMin.Contains(Minto))
                    {

                        while (!curTimeMin.Contains(Minto))
                        {
                            driver.FindElement(By.CssSelector(".flatpickr-calendar.hasTime.animate.arrowTop.open")).FindElement(By.CssSelector(".flatpickr-time.time24hr")).FindElements(By.CssSelector(".arrowUp"))[1].Click();
                            Thread.Sleep(2000);
                            curTimeMin = driver.FindElement(By.CssSelector(".flatpickr-calendar.hasTime.animate.arrowTop.open")).FindElement(By.CssSelector(".numInput.flatpickr-minute")).GetAttribute("value");
                        }
                    }
                }
          
                Thread.Sleep(1000);
                driver.FindElement(By.TagName(tagH)).Click();
                Thread.Sleep(1000);
            }
        }
    }
}