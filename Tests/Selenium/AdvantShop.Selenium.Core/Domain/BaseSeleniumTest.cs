using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.IO;
using System.Reflection;
using AdvantShop.Selenium.Core.Infrastructure;
using System.Threading;
using System.Net;
using OpenQA.Selenium.Support.UI;
using System.Linq;
using OpenQA.Selenium.Interactions;

namespace AdvantShop.SeleniumTest.Core
{
    public class BaseSeleniumTest
    {
        protected IWebDriver driver;
        protected string baseURL;
        protected string baseScrinshotsPath;
        private string verificationErrors = "";
        protected string logFilePath = "";


        public string testname = "";


        public void Debug(string message)
        {
            if (!string.IsNullOrWhiteSpace(logFilePath))
            {
                using (TextWriter tw = new StreamWriter(logFilePath, true))
                {
                    tw.WriteLine(string.Format("{0}: {1}", DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss"), message));
                }
            }
        }

                
        protected void Init()
        {
            Debug("begin test");
            InitializeService.InitBrowser(out driver, out baseURL, out baseScrinshotsPath, out logFilePath);
            Functions.LogAdmin(driver, baseURL);
        }

        protected void ReInit(bool useNgDriver = true)
        {
            driver.Quit();
            InitializeService.InitBrowser(out driver, out baseURL, out baseScrinshotsPath, out logFilePath, useNgDriver);
            Functions.LogAdmin(driver, baseURL);
        }

        protected void ReInitClient(bool useNgDriver = true)
        {
            driver.Quit();
            InitializeService.InitBrowser(out driver, out baseURL, out baseScrinshotsPath, out logFilePath, useNgDriver);
         }

        public void TakeScreenshot()
        {
            ITakesScreenshot screenshotDriver = driver as ITakesScreenshot;
            Screenshot screenshot = screenshotDriver.GetScreenshot();

            var dirPath = Directory.GetDirectories(baseScrinshotsPath).LastOrDefault();
            if (dirPath == null)
            {
                dirPath = baseScrinshotsPath + "temp";
                Directory.CreateDirectory(dirPath);
            }
            dirPath += "\\";

            var url = dirPath + driver.Url.Replace(baseURL, "");
            var filePath = (url.Length > 100 ?  url.Substring(0, 99) : url).Replace("/", "-").Replace("?", "@").Replace(":", "$").Trim('-');

            var file = filePath + ".jpg";

            int i = 1;
            while (File.Exists(file))
            {
                file = filePath + "_" + i++ +".jpg";
            }

            screenshot.SaveAsFile(file, ScreenshotImageFormat.Jpeg);
        }

        protected string GetPicturePath(string filename)
        {
            string dllPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string path = new DirectoryInfo(dllPath).Parent.Parent.FullName + "\\data\\pictures\\" + filename;
            return path;
        }
        
        protected IWebElement GetGridCell(int row, string column, string gridUniqueId = "")
        {
            return driver.FindElement(By.CssSelector(string.Format("[data-e2e-grid-cell=\"{0}[{1}][\'{2}\']\"]", "grid" + gridUniqueId + "", row, column)));
        }

        protected IWebElement GetButton(eButtonType type, string name = null)
        {
            return driver.FindElement(By.CssSelector(string.Format("[data-e2e=\"btn{0}{1}\"]", type != eButtonType.Simple ? type.ToString() : string.Empty, name)));
        }

        protected IWebElement GetGridFilter()
        {
            Thread.Sleep(1000);
            return driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterSearch\"]"));
        }

        protected void GetGridFilterTab(int tabIndex = 0, string text = "")
        {
            Thread.Sleep(1000);
            driver.FindElements(By.CssSelector("[data-e2e=\"gridFilterSearch\"]"))[tabIndex].Click();
            driver.FindElements(By.CssSelector("[data-e2e=\"gridFilterSearch\"]"))[tabIndex].Clear();
            driver.FindElements(By.CssSelector("[data-e2e=\"gridFilterSearch\"]"))[tabIndex].SendKeys(text);
        }

        protected void GetGridIdFilter(string gridName = "", string text = "")
        {
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[grid-unique-id=\"" + gridName + "\"]")).FindElement(By.CssSelector("[data-e2e=\"gridFilterSearch\"]")).Click();
            driver.FindElement(By.CssSelector("[grid-unique-id=\"" + gridName + "\"]")).FindElement(By.CssSelector("[data-e2e=\"gridFilterSearch\"]")).Clear();
            driver.FindElement(By.CssSelector("[grid-unique-id=\"" + gridName + "\"]")).FindElement(By.CssSelector("[data-e2e=\"gridFilterSearch\"]")).SendKeys(text);
        }
        
        protected void TabClick(string tabText = "", string tablink1 = "", string tablink2 = "")
        {
            var tabName = driver.FindElement(By.CssSelector(".tasks-navbar")).FindElement(By.CssSelector("[data-e2e=\"" + tabText + "\"]"));
            
            if (tabName.Displayed)
            {
                tabName.Click();
                Thread.Sleep(2000);
            }

            else
            {
                driver.FindElement(By.LinkText(tablink1)).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.LinkText(tablink2)).Click();
                Thread.Sleep(2000);
            }

        }
        
        protected void DropFocus(string tag)
        {
            Thread.Sleep(1000);
            driver.FindElement(By.TagName(tag)).Click();
            Thread.Sleep(1000);
        }

        public void Blur()
        {
            (driver as IJavaScriptExecutor).ExecuteScript("!!document.activeElement ? document.activeElement.blur() : 0");
        }

        protected void XPathContainsText(string tag = "", string text = "")
        {
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("//" + tag + "[contains(text(), '" + text + "')]")).Click();
            Thread.Sleep(2000);
        }

        protected void PageSelectItems(string num = "", int tabIndex = 0)
        {
            (new SelectElement(driver.FindElements(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"))[tabIndex])).SelectByText(num);
            Thread.Sleep(5000);
        }

        protected void PageSelectItemsTab(string num = "", string gridName = "")
        {
            (new SelectElement(driver.FindElement(By.CssSelector("[grid-unique-id=\"" + gridName + "\"]")).FindElement(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]")))).SelectByText(num);
            Thread.Sleep(5000);
        }


        protected void Refresh()
        {
            driver.Navigate().Refresh();
            Thread.Sleep(5000);
        }

        protected void GoBack()
        {
            driver.Navigate().Back();
            Thread.Sleep(5000);
        }

        protected void SetCkText(string text, string textareaId)
        {
            Thread.Sleep(5000);
            IWebElement iframe = null;
            try
            {
                iframe = driver.FindElement(By.CssSelector("#cke_" + textareaId + " iframe"));
            }
            catch (Exception)
            {
               // TakeScreenshot();
                //throw;
            }

            driver.SwitchTo().Frame(iframe);
            var body = driver.FindElement(By.TagName("body"));
            body.Clear();
            body.SendKeys(text);
            driver.SwitchTo().DefaultContent();
        }

        protected void AssertCkText(string text, string textareaId)
        {
            Thread.Sleep(5000);
            var iframe = driver.FindElement(By.CssSelector("#cke_" + textareaId + " iframe"));
            driver.SwitchTo().Frame(iframe);
            Assert.AreEqual(text, driver.FindElement(By.TagName("body")).Text);
            driver.SwitchTo().DefaultContent();
        }

        protected void ScrollTo(By by)
        {
            var element = driver.FindElement(by);
            IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
            jse.ExecuteScript("arguments[0].scrollIntoView(true)", element);
            Thread.Sleep(1000);
        }

        protected void ScrollToElements(By by, int tabIndex = 0)
        {
            var element = driver.FindElements(by)[tabIndex];
            IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
            jse.ExecuteScript("arguments[0].scrollIntoView(true)", element);
            Thread.Sleep(1000);
        }

        protected void WaitForElem(By by)
        {
            Thread.Sleep(200);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement elem = wait.Until<IWebElement>((d) =>
            {
                return d.FindElement(by);
            });
        }

        public static void MouseFocus(IWebDriver driver, By by)
        {
            Actions action = new Actions(driver);
            IWebElement elem = driver.FindElement(by);
            action.MoveToElement(elem);
            action.Perform();
            Thread.Sleep(500);
        }

        protected void GoToAdmin(string url = "")
        {
            driver.Navigate().GoToUrl(baseURL.Trim('/') + "/adminv2/" + url.Trim('/'));
            Thread.Sleep(2000);
        }
        protected void GoToOldAdmin(string url = "")
        {
            driver.Navigate().GoToUrl(baseURL.Trim('/') + "/admin/" + url.Trim('/'));
            Thread.Sleep(2000);
        }

        protected void GoToClient(string url = "")
        {
            driver.Navigate().GoToUrl(baseURL.Trim('/') + "/" + url.Trim('/'));
            Thread.Sleep(2000);
        }

        public void setLeadBuyInOneClick()
        {
            GoToAdmin("settingscheckout#?checkoutTab=common");

            IWebElement selectElem1 = driver.FindElement(By.Name("BuyInOneClickAction"));
            SelectElement select1 = new SelectElement(selectElem1);

            if (!select1.SelectedOption.Text.Contains("Создавать лид"))

            {
                ScrollTo(By.Name("BuyInOneClickButtonText"));
                (new SelectElement(driver.FindElement(By.Name("BuyInOneClickAction")))).SelectByText("Создавать лид");

                ScrollTo(By.Id("header-top"));
                driver.FindElement(By.CssSelector(".btn.btn-sm.btn-success.btn--margin-left")).Click();
                Thread.Sleep(2000);
            }
        }

        public void checkSelected(string id)
        {
            if (!driver.FindElement(By.Id(id)).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"" + id + "\"]")).Click();
                ScrollTo(By.Id("header-top"));
                driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCatalogSave\"]")).Click();
                Thread.Sleep(2000);
            }
        }

        public void checkNotSelected(string id)
        {
            if (driver.FindElement(By.Id(id)).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"" + id + "\"]")).Click();
                ScrollTo(By.Id("header-top"));
                driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCatalogSave\"]")).Click();
                Thread.Sleep(2000);
            }
        }

        public void leadBuyOneClick(string productId, string customerName, string customerPhone, string customerEmail = "")
        {
            GoToClient("products/test-product" + productId);
            Refresh();

            ScrollTo(By.CssSelector("[data-product-id=\"" + productId + "\"]"));
            driver.FindElement(By.LinkText("Купить в один клик")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.Name("buyOneClickFormName")).Click();
            driver.FindElement(By.Name("buyOneClickFormName")).Clear();
            driver.FindElement(By.Name("buyOneClickFormName")).SendKeys(customerName);

            if (customerEmail != "")
            {
                driver.FindElement(By.Name("buyOneClickFormEmail")).Click();
                driver.FindElement(By.Name("buyOneClickFormEmail")).Clear();
                driver.FindElement(By.Name("buyOneClickFormEmail")).SendKeys(customerEmail);
            }
            
            driver.FindElement(By.Name("buyOneClickFormPhone")).Click();
            driver.FindElement(By.Name("buyOneClickFormPhone")).SendKeys(customerPhone);

            driver.FindElement(By.CssSelector("[value=\"Заказать\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.LinkText("Закрыть")).Click();
            Thread.Sleep(2000);
        }

        public void gridReturnDefaultView10()
        {
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            Functions.GridPaginationSelect10(driver, baseURL);
            ScrollTo(By.Id("header-top"));
        }

        protected void VerifyAreEqual(string what = "", string where = "", string descript = "")
        {
            try
            {
                Assert.AreEqual(what, where);
            }
            catch (AssertionException e)
            {
                verificationErrors += Environment.NewLine + (descript + e.Message);
                Console.Error.WriteLine(descript + e.Message);
                Debug(descript + e.Message);
            }
        }
        protected void VerifyAreNotEqual(string what = "", string where = "", string descript = "")
        {
            try
            {
                Assert.AreNotEqual(what, where);
            }
            catch (AssertionException e)
            {
                verificationErrors += Environment.NewLine + (descript + e.Message);
                Console.Error.WriteLine(descript + e.Message);
                Debug(descript + e.Message);
            }
        }

        protected void VerifyIsTrue(bool what = false, string descript = "")
        {
            try
            {
                Assert.IsTrue(what);
            }
            catch (AssertionException e)
            {
                verificationErrors += Environment.NewLine + (descript + e.Message);
                Console.Error.WriteLine(descript + e.Message);
                Debug(descript + e.Message);
            }
        }

        protected void VerifyIsFalse(bool what = false, string descript = "")
        {
            try
            {
                Assert.IsFalse(what);
            }
            catch (AssertionException e)
            {
                verificationErrors += Environment.NewLine + (descript + e.Message);
                Console.Error.WriteLine(descript + e.Message);
                Debug(descript + e.Message);
            }
        }

        protected void VerifyBegin(string nametest = "")
        {
            verificationErrors = nametest;
            Console.Error.WriteLine(nametest);
            Debug(nametest);
        }
        protected void VerifyFinally(string nametest = "")
        {
           
            Assert.AreEqual(nametest, verificationErrors.ToString());
            Console.Error.WriteLine("-pass");
        }

        public void WaitForAjax()
        {
            //while (true) // Handle timeout somewhere
            //{
            //    var ajaxIsComplete = (bool)(driver as IJavaScriptExecutor).ExecuteScript("return window.ajaxIsComplete()");
            //    if (ajaxIsComplete)
            //        break;
            //    Thread.Sleep(100);
            //}

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            wait.Until(d => (bool)(d as IJavaScriptExecutor).ExecuteScript("return window.ajaxIsComplete();"));
        }
        
        public void isElementNotPresent(By by, string attr)
        {
            try
            {
               var a = driver.FindElement(by).GetAttribute(attr);
            Assert.IsTrue(a.Equals(null));
             }

            catch (NullReferenceException e)
            {
                Assert.Pass();
            }

                 Assert.Fail();
        }

        public void WaitForElemEnabled(By by)
        {
            if (!driver.FindElement(by).Enabled)
            {
                Thread.Sleep(3000);
            }
        }

        protected bool Is404Page(string url)
        {
            string baseurl = url.Contains("http://") || url.Contains("https://") ? url : baseURL.Trim('/') + "/" + url;
            try
            {
                WebClient client = new WebClient();
                client.DownloadString(baseurl);
                return false;
            }
            catch (WebException ex)
            {
                var response = ex.Response as HttpWebResponse;
                if (response == null)
                    throw new Exception("Empty response from " + baseurl);

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return true;
                }
                return false;
            }
        }

        public void reindexSearch()
        {
            GoToAdmin("settingscatalog#?catalogTab=search");
            driver.FindElement(By.LinkText("Обновить индекс поиска")).Click();
            Thread.Sleep(3000);
        }

        [OneTimeTearDown]
        public void TeardownTest()
        {
            try
            {
                if (driver != null)
                {
                    driver.Quit();
                }
            }
            catch (Exception ex)
            {
                Debug("OneTimeTearDown");
            }
        }
    }
}
