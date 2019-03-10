using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using AdvantShop.Selenium.Core.Infrastructure;
using System.Collections.ObjectModel;

namespace AdvantShop.SeleniumTest.Admin.Settings.Service
{
    [TestFixture]
    public class SuppotCenter : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            Init();
        }
         

        [Test]
        public void OpenSuppotCenterReadyAnswer()
        {
            GoToAdmin();
            testname = "OpenSuppotCenterReadyAnswer";
            VerifyBegin(testname);
            // driver.FindElement(By.XPath("//span[contains(text(), 'Помощь')]")).Click();
            driver.FindElement(By.XPath("//span[contains(text(), 'Центр поддержки')]")).Click();
            Thread.Sleep(3000);
            VerifyIsTrue(driver.Url.Contains("service/supportcenter"), "url supportcenter");
            VerifyIsTrue(driver.FindElements(By.TagName("iframe")).Count > 0, "iframe on page");
            VerifyIsTrue(driver.FindElement(By.TagName("iframe")).GetAttribute("src").ToString().Contains("advantshop.net/shop/SupportCenterShop.aspx"), "src page");

            var iframe = driver.FindElement(By.TagName("iframe"));
            driver.SwitchTo().Frame(iframe);

            Thread.Sleep(1000);
            driver.FindElement(By.XPath("//span[contains(text(), 'Товар и категории')]")).Click();
            Thread.Sleep(2000);
            Functions.OpenNewTab(driver, baseURL);
          
            VerifyIsTrue(driver.Url.Contains("advantshop.net/help/section/working_with_catalog"), "url link");
            VerifyAreEqual("Работа с каталогом", driver.FindElement(By.TagName("h1")).Text, "open page by link, h1");

            Functions.CloseTab(driver, baseURL);
            VerifyFinally(testname);
        }

        [Test]
        public void OpenSuppotCenterSearch()
        {
            GoToAdmin();
            testname = "OpenSuppotCenterSearch";
            VerifyBegin(testname);
            // driver.FindElement(By.XPath("//span[contains(text(), 'Помощь')]")).Click();
            driver.FindElement(By.XPath("//span[contains(text(), 'Центр поддержки')]")).Click();
            Thread.Sleep(3000);
          
            var iframe = driver.FindElement(By.TagName("iframe"));
            driver.SwitchTo().Frame(iframe);

            Thread.Sleep(1000);

            driver.FindElement(By.CssSelector(".form-control.txtQuestion")).SendKeys("Хостинг");
            Thread.Sleep(1000);
            driver.FindElement(By.Id("btnSearchQuestion")).Click();
            Thread.Sleep(2000);

            Functions.OpenNewTab(driver, baseURL);

            VerifyIsTrue(driver.Url.Contains("advantshop.net/help/search.aspx"), "url link");
            VerifyAreEqual("С чем вам помочь?", driver.FindElement(By.TagName("h3")).Text, "help, h3");
            VerifyIsTrue(driver.FindElements(By.TagName("h3"))[1].Text.Contains("Найдено"), "rezult search, h3");
            VerifyAreEqual("Хостинг", driver.FindElement(By.CssSelector(".input-wrap.search-input-wrap input")).GetAttribute("value"), "placeholder search value"+ driver.FindElement(By.CssSelector(".input-wrap.search-input-wrap input")).GetAttribute("value").ToString());

            Functions.CloseTab(driver, baseURL);
            VerifyFinally(testname);
        }

        [Test]
        public void OpenSuppotCenterCallBackMessange()
        {
            GoToAdmin();
            testname = "CallBackMessange";
            VerifyBegin(testname);

           // driver.FindElement(By.XPath("//span[contains(text(), 'Помощь')]")).Click();
             driver.FindElement(By.XPath("//span[contains(text(), 'Центр поддержки')]")).Click();
            Thread.Sleep(3000);
           
            var iframe = driver.FindElement(By.TagName("iframe"));
            driver.SwitchTo().Frame(iframe);
            Thread.Sleep(1000);
            
            VerifyAreEqual("Центр поддержки", driver.FindElement(By.CssSelector(".support-center-header-text-title")).Text, "open page, h2");
            VerifyAreEqual("Не нашли ответ на вопрос ?", driver.FindElement(By.CssSelector(".section-title.support-center-callback-title")).Text, "callback title, h2");
            
            VerifyIsTrue(driver.FindElements(By.CssSelector(".support-center-callback-block-icon")).Count > 0, "callback icon");
            VerifyAreEqual("Написать сообщение", driver.FindElement(By.CssSelector(".support-center-callback-block-name")).Text, "callback name messange");
            VerifyAreEqual("Позвонить\r\nнам", driver.FindElements(By.CssSelector(".support-center-callback-block-name"))[1].Text, "callback name call");

            //messange
            ScrollTo(By.CssSelector(".support-center-callback-block-icon"));
            driver.FindElement(By.CssSelector(".support-center-callback-block-icon")).Click();
            Thread.Sleep(4000);
           
            VerifyAreEqual("Написать сообщение", driver.FindElement(By.TagName("h1")).Text, "open send messange, h1");            
            VerifyIsTrue(driver.FindElements(By.Name("ctl00$cphMain$btnSend")).Count==1, "btn send messange");

            VerifyFinally(testname);
        }

        [Test]
        public void OpenSuppotCenterCallBackPhone()
        {
             GoToAdmin();
            testname = "CallBackPhone";
            VerifyBegin(testname);
            // driver.FindElement(By.XPath("//span[contains(text(), 'Помощь')]")).Click();
            driver.FindElement(By.XPath("//span[contains(text(), 'Центр поддержки')]")).Click();
            Thread.Sleep(3000);

            var iframe = driver.FindElement(By.TagName("iframe"));
            driver.SwitchTo().Frame(iframe);

            Thread.Sleep(1000);
                
            //call
            ScrollTo(By.CssSelector(".section-title.support-center-callback-title"));
            driver.FindElement(By.CssSelector(".support-center-callback-item.support-center-callback-item--green")).Click();
            Thread.Sleep(2000);
            VerifyIsTrue(driver.FindElements(By.CssSelector(".modal.modal-dialog")).Count == 1, " open modal windows");
            VerifyAreEqual("Позвоните нам", driver.FindElement(By.CssSelector(".title")).Text, " modal windows call, title");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".js-support-center-acccount")).Text.Contains("Ваш номер клиента: "), " modal windows call, account");
            VerifyAreEqual("Написать сообщение", driver.FindElement(By.CssSelector(".call-us-info a")).Text, " modal windows call, send messange");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".call-us-info a")).GetAttribute("href").ToString().Contains("Shop/CreateTicket.aspx"), "href send messange");

            driver.FindElement(By.CssSelector(".btn-success.js-btn-call-us")).Click();
            Thread.Sleep(2000);
            VerifyIsTrue(driver.FindElements(By.CssSelector(".modal modal-dialog")).Count == 0, " close modal windows");
            VerifyIsTrue(driver.Url.Contains("service/supportcenter"), "url supportcenter");
            VerifyAreEqual("Центр поддержки", driver.FindElement(By.CssSelector(".support-center-header-text-title")).Text, "open page, h2");

            VerifyFinally(testname);
        }
    }
}