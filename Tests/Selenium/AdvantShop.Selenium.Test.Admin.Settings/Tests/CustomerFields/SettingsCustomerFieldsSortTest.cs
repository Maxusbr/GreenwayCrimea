using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Settings.CustomerFields
{
    [TestFixture]
    public class SettingsCustomerFieldsSortTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Customers);
            InitializeService.LoadData(
            "data\\Admin\\Settings\\CustomerFields\\Customers.Customer.csv",
           "data\\Admin\\Settings\\CustomerFields\\Customers.CustomerGroup.csv",
               "data\\Admin\\Settings\\CustomerFields\\Customers.CustomerField.csv",
               "data\\Admin\\Settings\\CustomerFields\\Customers.CustomerFieldValue.csv"

           );

            Init();

            GoToAdmin("settingscustomers#?tab=customerFields");
        }

        string customerNameFirstStr = "";
        string customerNameLastStr = "";

        [Test]
        public void SortName()
        {
            testname = "SettingsCustomerFieldsSortName";
            VerifyBegin(testname);

            GetGridCell(-1, "Name", "CustomerFields").Click();
            WaitForAjax();
            VerifyAreEqual("Customer Field 1", GetGridCell(0, "Name", "CustomerFields").Text, "sort Name 1 asc");
            VerifyAreEqual("Customer Field 107", GetGridCell(9, "Name", "CustomerFields").Text, "sort Name 10 asc");

            GetGridCell(-1, "Name", "CustomerFields").Click();
            WaitForAjax();
            VerifyAreEqual("Customer Field 99", GetGridCell(0, "Name", "CustomerFields").Text, "sort Name 1 desc");
            VerifyAreEqual("Customer Field 90", GetGridCell(9, "Name", "CustomerFields").Text, "sort Name 10 desc");

            VerifyFinally(testname);
        }

        [Test]
        public void SortFieldType()
        {
            testname = "SettingsCustomerFieldsSortFieldType";
            VerifyBegin(testname);

            GetGridCell(-1, "FieldTypeFormatted", "CustomerFields").Click();
            WaitForAjax();
            VerifyAreEqual("Выбор", GetGridCell(0, "FieldTypeFormatted", "CustomerFields").Text, "sort type 1 asc");
            VerifyAreEqual("Текстовое поле", GetGridCell(9, "FieldTypeFormatted", "CustomerFields").Text, "sort type 10 asc");

            customerNameFirstStr = GetGridCell(0, "Name", "CustomerFields").Text;
            customerNameLastStr = GetGridCell(9, "Name", "CustomerFields").Text;

            VerifyIsFalse(customerNameFirstStr.Equals(customerNameLastStr), "sort type diff names asc");

            GetGridCell(-1, "FieldTypeFormatted", "CustomerFields").Click();
            WaitForAjax();
            VerifyAreEqual("Многострочное текстовое поле", GetGridCell(0, "FieldTypeFormatted", "CustomerFields").Text, "sort type 1 desc");
            VerifyAreEqual("Многострочное текстовое поле", GetGridCell(9, "FieldTypeFormatted", "CustomerFields").Text, "sort type 10 desc");

            customerNameFirstStr = GetGridCell(0, "Name", "CustomerFields").Text;
            customerNameLastStr = GetGridCell(9, "Name", "CustomerFields").Text;

            VerifyIsFalse(customerNameFirstStr.Equals(customerNameLastStr), "sort type diff names desc");

            VerifyFinally(testname);
        }

        [Test]
        public void SortRequired()
        {
            testname = "SettingsCustomerFieldsSortRequired";
            VerifyBegin(testname);

            GetGridCell(-1, "Required", "CustomerFields").Click();
            WaitForAjax();
            VerifyIsFalse(GetGridCell(0, "Required", "CustomerFields").FindElement(By.TagName("input")).Selected, "sort Required 1 asc");
            VerifyIsFalse(GetGridCell(9, "Required", "CustomerFields").FindElement(By.TagName("input")).Selected, "sort Required 10 asc");

            customerNameFirstStr = GetGridCell(0, "Name", "CustomerFields").Text;
            customerNameLastStr = GetGridCell(9, "Name", "CustomerFields").Text;

            VerifyIsFalse(customerNameFirstStr.Equals(customerNameLastStr), "sort Required diff names asc");

            GetGridCell(-1, "Required", "CustomerFields").Click();
            WaitForAjax();
            VerifyIsTrue(GetGridCell(0, "Required", "CustomerFields").FindElement(By.TagName("input")).Selected, "sort Required 1 desc");
            VerifyIsTrue(GetGridCell(9, "Required", "CustomerFields").FindElement(By.TagName("input")).Selected, "sort Required 10 desc");

            customerNameFirstStr = GetGridCell(0, "Name", "CustomerFields").Text;
            customerNameLastStr = GetGridCell(9, "Name", "CustomerFields").Text;

            VerifyIsFalse(customerNameFirstStr.Equals(customerNameLastStr), "sort Required diff names desc");

            VerifyFinally(testname);
        }

        [Test]
        public void SortSortOrder()
        {
            testname = "SettingsCustomerFieldsSortSortOrder";
            VerifyBegin(testname);

            GetGridCell(-1, "SortOrder", "CustomerFields").Click();
            WaitForAjax();
            VerifyAreEqual("1", GetGridCell(0, "SortOrder", "CustomerFields").FindElement(By.TagName("input")).GetAttribute("value"), "sort SortOrder 1 asc");
            VerifyAreEqual("10", GetGridCell(9, "SortOrder", "CustomerFields").FindElement(By.TagName("input")).GetAttribute("value"), "sort SortOrder 10 asc");

            GetGridCell(-1, "SortOrder", "CustomerFields").Click();
            WaitForAjax();
            VerifyAreEqual("150", GetGridCell(0, "SortOrder", "CustomerFields").FindElement(By.TagName("input")).GetAttribute("value"), "sort SortOrder 1 desc");
            VerifyAreEqual("141", GetGridCell(9, "SortOrder", "CustomerFields").FindElement(By.TagName("input")).GetAttribute("value"), "sort SortOrder 10 desc");

            VerifyFinally(testname);
        }

        [Test]
        public void SortEnabled()
        {
            testname = "SettingsCustomerFieldsSortEnabled";
            VerifyBegin(testname);

            GetGridCell(-1, "Enabled", "CustomerFields").Click();
            WaitForAjax();
            VerifyIsFalse(GetGridCell(0, "Enabled", "CustomerFields").FindElement(By.TagName("input")).Selected, "sort Enabled 1 asc");
            VerifyIsFalse(GetGridCell(9, "Enabled", "CustomerFields").FindElement(By.TagName("input")).Selected, "sort Enabled 10 asc");

            customerNameFirstStr = GetGridCell(0, "Name", "CustomerFields").Text;
            customerNameLastStr = GetGridCell(9, "Name", "CustomerFields").Text;

            VerifyIsFalse(customerNameFirstStr.Equals(customerNameLastStr), "sort Enabled diff names asc");

            GetGridCell(-1, "Enabled", "CustomerFields").Click();
            WaitForAjax();
            VerifyIsTrue(GetGridCell(0, "Enabled", "CustomerFields").FindElement(By.TagName("input")).Selected, "sort Enabled 1 desc");
            VerifyIsTrue(GetGridCell(9, "Enabled", "CustomerFields").FindElement(By.TagName("input")).Selected, "sort Enabled 10 desc");

            customerNameFirstStr = GetGridCell(0, "Name", "CustomerFields").Text;
            customerNameLastStr = GetGridCell(9, "Name", "CustomerFields").Text;

            VerifyIsFalse(customerNameFirstStr.Equals(customerNameLastStr), "sort Enabled diff names desc");

            VerifyFinally(testname);
        }

        [Test]
        public void SortShowInClient()
        {
            testname = "SettingsCustomerFieldsSortShowInClient";
            VerifyBegin(testname);

            GetGridCell(-1, "ShowInClient", "CustomerFields").Click();
            WaitForAjax();
            VerifyIsFalse(GetGridCell(0, "ShowInClient", "CustomerFields").FindElement(By.TagName("input")).Selected, "sort ShowInClient 1 asc");
            VerifyIsFalse(GetGridCell(9, "ShowInClient", "CustomerFields").FindElement(By.TagName("input")).Selected, "sort ShowInClient 10 asc");

            customerNameFirstStr = GetGridCell(0, "Name", "CustomerFields").Text;
            customerNameLastStr = GetGridCell(9, "Name", "CustomerFields").Text;

            VerifyIsFalse(customerNameFirstStr.Equals(customerNameLastStr), "sort ShowInClient diff names asc");

            GetGridCell(-1, "ShowInClient", "CustomerFields").Click();
            WaitForAjax();
            VerifyIsTrue(GetGridCell(0, "ShowInClient", "CustomerFields").FindElement(By.TagName("input")).Selected, "sort ShowInClient 1 desc");
            VerifyIsTrue(GetGridCell(9, "ShowInClient", "CustomerFields").FindElement(By.TagName("input")).Selected, "sort ShowInClient 10 desc");

            customerNameFirstStr = GetGridCell(0, "Name", "CustomerFields").Text;
            customerNameLastStr = GetGridCell(9, "Name", "CustomerFields").Text;

            VerifyIsFalse(customerNameFirstStr.Equals(customerNameLastStr), "sort ShowInClient diff names desc");

            VerifyFinally(testname);
        }

    }
}