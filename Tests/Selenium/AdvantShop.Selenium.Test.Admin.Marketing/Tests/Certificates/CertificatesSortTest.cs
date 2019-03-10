using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using AdvantShop.Selenium.Core.Infrastructure;

namespace AdvantShop.Selenium.Test.Admin.Marketing.Tests.Certificates
{
    [TestFixture]
    public class CertificatesSortTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Orders | ClearType.Catalog);
            InitializeService.LoadData(
            "data\\Admin\\Certificates\\Catalog.Product.csv",
            "data\\Admin\\Certificates\\Catalog.Photo.csv",
            "data\\Admin\\Certificates\\Catalog.Offer.csv",
            "data\\Admin\\Certificates\\Catalog.Category.csv",
            "data\\Admin\\Certificates\\Catalog.ProductCategories.csv",
            "data\\Admin\\Certificates\\Catalog.Brand.csv",
            "data\\Admin\\Certificates\\Catalog.Color.csv",
            "data\\Admin\\Certificates\\Catalog.Size.csv",
             "data\\Admin\\Certificates\\[Order].Certificate.csv",
            "data\\Admin\\Certificates\\[Order].OrderContact.csv",
            "data\\Admin\\Certificates\\[Order].OrderCurrency.csv",
             "data\\Admin\\Certificates\\[Order].OrderItems.csv",
             "data\\Admin\\Certificates\\[Order].OrderStatus.csv",
             "data\\Admin\\Certificates\\[Order].[Order].csv",
             "Data\\Admin\\Certificates\\[Order].OrderSource.csv"
           );
            Init();
            GoToAdmin("certificates");
        }

        [Test]
        public void CertificatesSortCode()
        {
            testname = "CertificatesSortCode";
            VerifyBegin(testname);
           VerifyAreEqual("Certificate1", GetGridCell(0, "CertificateCode").Text, "grid CertificateCode");
            VerifyAreEqual("6", GetGridCell(0, "OrderId").Text, "grid OrderId");
            VerifyAreEqual("7", GetGridCell(0, "ApplyOrderNumber").Text, "grid ApplyOrderNumber");
            VerifyAreEqual("100 руб.", GetGridCell(0, "FullSum").Text, "grid FullSum");

            VerifyIsTrue(GetGridCell(0, "Paid").FindElement(By.CssSelector("input")).Selected, "grid Paid");
            VerifyIsTrue(GetGridCell(0, "Enable").FindElement(By.CssSelector("input")).Selected, "grid Enable");
            VerifyIsTrue(GetGridCell(0, "Used").FindElement(By.CssSelector(" input")).Selected, "grid Used");
            VerifyAreEqual("05.04.2013 17:16", GetGridCell(0, "CreationDates").Text, "grid CreationDates");

            GetGridCell(-1, "CertificateCode").Click();
            WaitForAjax();
           VerifyAreEqual("Certificate1", GetGridCell(0, "CertificateCode").Text, "sort CertificateCode ASC  value1");
            VerifyAreEqual("Certificate14", GetGridCell(5, "CertificateCode").Text, "sort CertificateCode DESC  value5");

            GetGridCell(-1, "CertificateCode").Click();
            WaitForAjax();

           VerifyAreEqual("Certificate9", GetGridCell(0, "CertificateCode").Text, "sort CertificateCode ASC  value1");
            VerifyAreEqual("Certificate4", GetGridCell(5, "CertificateCode").Text, "sort CertificateCode DESC  value5");
            VerifyFinally(testname);
        }
        [Test]
        public void CertificatesSortOrder()
        {
            testname = "CertificatesSortOrder";
            VerifyBegin(testname);
            GetGridCell(-1, "OrderId").Click();
            WaitForAjax();
           VerifyAreEqual("Certificate2", GetGridCell(0, "CertificateCode").Text, "sort OrderId ASC  value1");
            VerifyAreEqual("Certificate7", GetGridCell(5, "CertificateCode").Text, "sort OrderId DESC  value5");

            GetGridCell(-1, "OrderId").Click();
            WaitForAjax();
           VerifyAreEqual("Certificate1", GetGridCell(0, "CertificateCode").Text, "sort OrderId ASC  value1");
            VerifyAreEqual("Certificate16", GetGridCell(5, "CertificateCode").Text, "sort OrderId DESC  value5");
            VerifyFinally(testname);
        }
        [Test]
        public void CertificatesSortApplyOrder()
        {
            testname = "CertificatesSortApply";
            VerifyBegin(testname);
            GetGridCell(-1, "ApplyOrderNumber").Click();
            WaitForAjax();
           VerifyAreEqual("Certificate2", GetGridCell(0, "CertificateCode").Text, "sort ApplyOrderNumber ASC  value1");
            VerifyAreEqual("Certificate7", GetGridCell(5, "CertificateCode").Text, "sort ApplyOrderNumber DESC  value5");

            GetGridCell(-1, "ApplyOrderNumber").Click();
            WaitForAjax();
           VerifyAreEqual("Certificate1", GetGridCell(0, "CertificateCode").Text, "sort ApplyOrderNumber ASC  value1");
            VerifyAreEqual("Certificate6", GetGridCell(5, "CertificateCode").Text, "sort ApplyOrderNumber DESC  value5");
            VerifyFinally(testname);
        }
        [Test]
        public void CertificatesSortSum()
        {
            testname = "CertificatesSortSum";
            VerifyBegin(testname);
            GetGridCell(-1, "FullSum").Click();
            WaitForAjax();
           VerifyAreEqual("Certificate1", GetGridCell(0, "CertificateCode").Text, "sort FullSum ASC  value1");
            VerifyAreEqual("Certificate6", GetGridCell(5, "CertificateCode").Text, "sort FullSum DESC  value5");

            GetGridCell(-1, "FullSum").Click();
            WaitForAjax();
           VerifyAreEqual("Certificate20", GetGridCell(0, "CertificateCode").Text, "sort FullSum ASC  value1");
            VerifyAreEqual("Certificate15", GetGridCell(5, "CertificateCode").Text, "sort FullSum DESC  value5");
            VerifyFinally(testname);
        }
        [Test]
        public void CertificatesSortPaid()
        {
            testname = "CertificatesSortPaid";
            VerifyBegin(testname);
            GetGridCell(-1, "Paid").Click();
            WaitForAjax();
           VerifyAreEqual("Certificate2", GetGridCell(0, "CertificateCode").Text, "sort Paid ASC  value1");
            VerifyAreEqual("Certificate7", GetGridCell(5, "CertificateCode").Text, "sort Paid DESC  value5");

            GetGridCell(-1, "Paid").Click();
            WaitForAjax();
           VerifyAreEqual("Certificate1", GetGridCell(0, "CertificateCode").Text, "sort Paid ASC  value1");
            VerifyAreEqual("Certificate6", GetGridCell(5, "CertificateCode").Text, "sort Paid DESC  value5");
            VerifyFinally(testname);
        }
        [Test]
        public void CertificatesSortEnable()
        {
            testname = "CertificatesSortEnable";
            VerifyBegin(testname);
            GetGridCell(-1, "Enable").Click();
            WaitForAjax();
           VerifyAreEqual("Certificate11", GetGridCell(0, "CertificateCode").Text, "sort Enable ASC  value1");
            VerifyAreEqual("Certificate16", GetGridCell(5, "CertificateCode").Text, "sort Enable DESC  value5");

            GetGridCell(-1, "Enable").Click();
            WaitForAjax();
           VerifyAreEqual("Certificate1", GetGridCell(0, "CertificateCode").Text, "sort Enable ASC  value1");
            VerifyAreEqual("Certificate6", GetGridCell(5, "CertificateCode").Text, "sort Enable DESC  value5");
            VerifyFinally(testname);
        }
        [Test]
        public void CertificatesSortUsed()
        {
            testname = "CertificatesSortUsed";
            VerifyBegin(testname);
            GetGridCell(-1, "Used").Click();
            WaitForAjax();
           VerifyAreEqual("Certificate2", GetGridCell(0, "CertificateCode").Text, "sort Used ASC value1");
            VerifyAreEqual("Certificate7", GetGridCell(5, "CertificateCode").Text, "sort Used DESC  value5");

            GetGridCell(-1, "Used").Click();
            WaitForAjax();
           VerifyAreEqual("Certificate1", GetGridCell(0, "CertificateCode").Text, "sort Used ASC  value1");
            VerifyAreEqual("Certificate6", GetGridCell(5, "CertificateCode").Text, "sort Used DESC  value5");
            VerifyFinally(testname);
        }
        [Test]
        public void CertificatesSortCreate()
        {
            testname = "CertificatesSortCreate";
            VerifyBegin(testname);
            GetGridCell(-1, "CreationDates").Click();
            WaitForAjax();
           VerifyAreEqual("Certificate20", GetGridCell(0, "CertificateCode").Text, "sort CreationDates ASC  value1");
            VerifyAreEqual("Certificate15", GetGridCell(5, "CertificateCode").Text, "sort CreationDates DESC  value5");

            GetGridCell(-1, "CreationDates").Click();
            WaitForAjax();
           VerifyAreEqual("Certificate1", GetGridCell(0, "CertificateCode").Text, "sort CreationDates ASC  value1");
            VerifyAreEqual("Certificate6", GetGridCell(5, "CertificateCode").Text, "sort CreationDates DESC  value5");

            //back default
            GetGridCell(-1, "FullSum").Click();
            WaitForAjax();
            VerifyFinally(testname);
        }
    }
}
