using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;
using AdvantShop.Selenium.Core.Infrastructure;
using System.Threading;

namespace AdvantShop.Web.Site.Selenium.Test.Admin.CRM.Leads
{
    [TestFixture]
    public class CRMLeadAddEditEventLetterTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();

            Init();

            //preconditions
            
            GoToAdmin("settingsmail#?notifyTab=emailsettings");

            driver.FindElement(By.Name("SMTP")).Click();
            driver.FindElement(By.Name("SMTP")).Clear();
            driver.FindElement(By.Name("SMTP")).SendKeys("smtp.yandex.ru");

            driver.FindElement(By.Name("Port")).Click();
            driver.FindElement(By.Name("Port")).Clear();
            driver.FindElement(By.Name("Port")).SendKeys("25");

            driver.FindElement(By.Name("Login")).Click();
            driver.FindElement(By.Name("Login")).Clear();
            driver.FindElement(By.Name("Login")).SendKeys("testmailimap@yandex.ru");

            driver.FindElement(By.Name("Password")).Click();
            driver.FindElement(By.Name("Password")).Clear();
            driver.FindElement(By.Name("Password")).SendKeys("ewqEWQ321#@!");

            if (!driver.FindElement(By.Name("SSL")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"SSL\"]")).Click();
            }

            driver.FindElement(By.Name("From")).Click();
            driver.FindElement(By.Name("From")).Clear();
            driver.FindElement(By.Name("From")).SendKeys("testmailimap@yandex.ru");

            driver.FindElement(By.Name("SenderName")).Click();
            driver.FindElement(By.Name("SenderName")).Clear();
            driver.FindElement(By.Name("SenderName")).SendKeys("Test Name");
            
            driver.FindElement(By.Name("ImapHost")).Click();
            driver.FindElement(By.Name("ImapHost")).Clear();
            driver.FindElement(By.Name("ImapHost")).SendKeys("imap.yandex.ru");

            driver.FindElement(By.Name("ImapPort")).Click();
            driver.FindElement(By.Name("ImapPort")).Clear();
            driver.FindElement(By.Name("ImapPort")).SendKeys("993");

            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"BtnSaveMailSettings\"]")).Click();
            Thread.Sleep(2000);
        }
        
        [Test]
        public void LetterToCustomerEvent()
        {
            testname = "LetterToCustomerEvent";
            VerifyBegin(testname);
            
            GoToAdmin("leads");

            GetButton(eButtonType.Add).Click();
            WaitForElem(By.CssSelector(".modal-content"));
            Thread.Sleep(2000);


            driver.FindElement(By.CssSelector("[data-e2e=\"LeadFirstName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadFirstName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadFirstName\"]")).SendKeys("test mail event");

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadPhoneNum\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadPhoneNum\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadPhoneNum\"]")).SendKeys("+71231212923");

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadEmail\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadEmail\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadEmail\"]")).SendKeys("ksyusha.ker1994@gmail.com");


            driver.FindElement(By.CssSelector("[data-e2e=\"LeadAdd\"]")).Click();
            Thread.Sleep(6000);
            
            ScrollTo(By.Id("Lead_Customer_FirstName"));
            driver.FindElement(By.LinkText("Отправить письмо")).Click();
            Thread.Sleep(7000);

            SetCkText("Test Letter To Customer from lead", "editor1");

            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            Thread.Sleep(3000);

            GoToAdmin("leads/edit/2");
            Thread.Sleep(3000);
            
            GoToAdmin("leads/edit/2");
            Thread.Sleep(3000);

            VerifyIsTrue(driver.FindElement(By.TagName("lead-events")).Text.Contains("Исходящее письмо"), "lead letter sent event");

            VerifyFinally(testname);
        }

    }
}