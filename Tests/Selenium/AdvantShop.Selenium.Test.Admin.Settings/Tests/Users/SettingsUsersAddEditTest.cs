using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;

namespace AdvantShop.SeleniumTest.Admin.Settings.Users
{
    [TestFixture]
    public class SettingsUsersAddEditTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Customers);
            InitializeService.LoadData(
            "data\\Admin\\Settings\\Users\\UserAddEdit\\Customers.Customer.csv",
           "data\\Admin\\Settings\\Users\\UserAddEdit\\Customers.CustomerGroup.csv",
               "data\\Admin\\Settings\\Users\\UserAddEdit\\Customers.Managers.csv",
                  "data\\Admin\\Settings\\Users\\UserAddEdit\\Customers.Departments.csv",
                  "data\\Admin\\Settings\\Users\\UserAddEdit\\Customers.ManagerRole.csv",
                  "data\\Admin\\Settings\\Users\\UserAddEdit\\Customers.ManagerRolesMap.csv"

           );

            Init();
        }

         

        [Test]
        public void SettingsUsersAdd()
        {
            testname = "SettingsUsersAdd";
            VerifyBegin(testname);

            GoToAdmin("settings/userssettings#?tab=Users");

            //  GetButton(eButtonType.Add).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"btnAdd\"]")).Click();
            Thread.Sleep(3000);

            VerifyAreEqual("Новый сотрудник", driver.FindElement(By.TagName("h2")).Text, "h2 add new user");

            driver.FindElement(By.CssSelector("[data-e2e=\"userLastName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"userLastName\"]")).SendKeys("NewSurname");

            driver.FindElement(By.CssSelector("[data-e2e=\"userFirstName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"userFirstName\"]")).SendKeys("NewName");

            driver.FindElement(By.CssSelector("[data-e2e=\"userEmail\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"userEmail\"]")).SendKeys("NewEmail@mail.ru");

            //add img from computer
            VerifyIsFalse(driver.FindElements(By.XPath("//a[contains(text(),\"Удалить\")]")).Count > 0);
            driver.FindElement(By.LinkText("Загрузить фото")).Click();
            driver.FindElement(By.CssSelector("input[type=\"file\"]")).SendKeys(GetPicturePath("avatar.jpg"));
            VerifyAreEqual("Загрузка изображения", driver.FindElement(By.TagName("h2")).Text, "h2 add new user img");
            driver.FindElement(By.XPath("//button[contains(text(),\"Применить\")]")).Click();

            driver.FindElement(By.CssSelector("[data-e2e=\"userPosition\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"userPosition\"]")).SendKeys("PositionTest");

            //check all departments
            IWebElement selectElem = driver.FindElement(By.CssSelector("[data-e2e=\"userDepartment\"]"));
            SelectElement select = new SelectElement(selectElem);

            IList<IWebElement> allOptionsDepart = select.Options;

            VerifyIsTrue(allOptionsDepart.Count == 10, "count departments"); //1 from 10 departments disabled + null select

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"userDepartment\"]")))).SelectByText("Department9");

            driver.FindElement(By.CssSelector("[data-e2e=\"userPhone\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"userPhone\"]")).SendKeys("+79278888888");
            ScrollTo(By.CssSelector("[data-e2e=\"userPhone\"]"));

            //birthday by calendar
            driver.FindElement(By.CssSelector("[data-e2e=\"userBirthDay\"]")).Click();
            driver.FindElement(By.CssSelector("[colspan=\"5\"]")).Click();
            driver.FindElement(By.CssSelector("[colspan=\"5\"]")).Click();
            driver.FindElement(By.XPath("//span[contains(text(),\"2013\")]")).Click();
            driver.FindElement(By.XPath("//span[contains(text(),\"янв.\")]")).Click();
            driver.FindElement(By.XPath("//td[contains(text(),\"17\")]")).Click();

            driver.FindElement(By.CssSelector("[data-e2e=\"userCity\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"userCity\"]")).SendKeys("CityTest");

            //check all head users
            IWebElement selectElem2 = driver.FindElement(By.CssSelector("[data-e2e=\"userHeadUser\"]"));
            SelectElement select2 = new SelectElement(selectElem2);

            IList<IWebElement> allOptionsHeadUser = select2.Options;

            VerifyIsTrue(allOptionsHeadUser.Count == 222, "count head users"); //all 221 users + null select

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"userHeadUser\"]")))).SelectByText("testlastname200 testfirstname200");

            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"userPermissionsModerInput\"]")).Selected, "selected moderator");

            //choose role from existing
            driver.FindElement(By.XPath("//input[@type='search']")).Click();
            WaitForElem(By.CssSelector("span.ui-select-choices-row-inner"));
            ScrollTo(By.CssSelector("[data-e2e=\"userHeadUser\"]"));
            driver.FindElement(By.CssSelector("span.ui-select-choices-row-inner")).Click();

            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"userEnabled\"]")).FindElement(By.TagName("input")).Selected, "selected user enabled");

            driver.FindElement(By.CssSelector("[data-e2e=\"userButtonSave\"]")).Click();
            Thread.Sleep(7000);

            //check admin
            GoToAdmin("settings/userssettings#?tab=Users");

            WaitForAjax();
            VerifyAreEqual("Найдено записей: 222", driver.FindElement(By.CssSelector("[grid-unique-id=\"gridUsers\"]")).FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all after adding");
            
            GetGridIdFilter("gridUsers", "NewName NewSurname");

            XPathContainsText("h1", "Сотрудники");
            WaitForAjax();
            

            VerifyIsTrue(!GetGridCell(0, "PhotoSrc", "Users").FindElement(By.TagName("img")).GetAttribute("src").Contains("no-avatar"), "avatar");

            VerifyAreEqual("NewName NewSurname", GetGridCell(0, "FullName", "Users").Text, "FullName");
            VerifyAreEqual("NewEmail@mail.ru", GetGridCell(0, "Email", "Users").Text, "Email");
            VerifyAreEqual("Department9", GetGridCell(0, "DepartmentName", "Users").Text, "DepartmentName");
            VerifyAreEqual("Модератор; Role1", GetGridCell(0, "Roles", "Users").Text, "Roles");

            VerifyIsTrue(GetGridCell(0, "Enabled", "Users").FindElement(By.TagName("input")).Selected, "Enabled");
            
            //check edit pop up
            GetGridCell(0, "_serviceColumn", "Users").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(2000);
            WaitForElem(By.CssSelector(".modal-content"));

            VerifyAreEqual("NewSurname", driver.FindElement(By.CssSelector("[data-e2e=\"userLastName\"]")).GetAttribute("value"), "edit pop up last name");
            VerifyAreEqual("NewName", driver.FindElement(By.CssSelector("[data-e2e=\"userFirstName\"]")).GetAttribute("value"), "edit pop up first name");
            VerifyAreEqual("NewEmail@mail.ru", driver.FindElement(By.CssSelector("[data-e2e=\"userEmail\"]")).GetAttribute("value"), "edit pop up email");
            VerifyIsTrue(driver.FindElements(By.XPath("//a[contains(text(),\"Удалить\")]")).Count > 0, "edit pop up img saved");
            VerifyAreEqual("PositionTest", driver.FindElement(By.CssSelector("[data-e2e=\"userPosition\"]")).GetAttribute("value"), "edit pop up position");

            IWebElement selectElem3 = driver.FindElement(By.CssSelector("[data-e2e=\"userDepartment\"]"));
            SelectElement select3 = new SelectElement(selectElem3);
            VerifyIsTrue(select3.AllSelectedOptions[0].Text.Contains("Department9"), "edit pop up department");

            VerifyAreEqual("+79278888888", driver.FindElement(By.CssSelector("[data-e2e=\"userPhone\"]")).GetAttribute("value"), "edit pop up phone");
            VerifyAreEqual("17.01.2013", driver.FindElement(By.CssSelector("[data-e2e=\"userBirthDay\"]")).GetAttribute("value"), "edit pop up birthday");
            VerifyAreEqual("CityTest", driver.FindElement(By.CssSelector("[data-e2e=\"userCity\"]")).GetAttribute("value"), "edit pop up city");

            IWebElement selectElem4 = driver.FindElement(By.CssSelector("[data-e2e=\"userHeadUser\"]"));
            SelectElement select4 = new SelectElement(selectElem4);
            VerifyIsTrue(select4.AllSelectedOptions[0].Text.Contains("testlastname200 testfirstname200"), "edit pop up head user");

            VerifyIsTrue(driver.FindElement(By.CssSelector(".ui-select-match")).Text.Contains("Role1"), "edit pop up role");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"userPermissionsModerInput\"]")).Selected, "edit pop up selected moderator");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"userEnabled\"]")).FindElement(By.TagName("input")).Selected, "edit pop up selected user enabled");

            VerifyFinally(testname);
        }

        [Test]
        public void SettingsUsersEdit()
        {
            testname = "SettingsUsersEdit";
            VerifyBegin(testname);

            GoToAdmin("settings/userssettings");
            
            GetGridIdFilter("gridUsers", "testfirstname9 testlastname9");
            XPathContainsText("h1", "Сотрудники");
            
            WaitForAjax();

            //pre check edit pop up
            VerifyIsTrue(GetGridCell(0, "FullName", "Users").Text.Contains("testfirstname9"), "pre check user to edit");
            string imgFirstGrid = GetGridCell(0, "PhotoSrc", "Users").FindElement(By.TagName("img")).GetAttribute("src");
            GetGridCell(0, "_serviceColumn", "Users").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(3000);
            WaitForElem(By.CssSelector(".modal-content"));

            VerifyAreEqual("Редактирование сотрудника", driver.FindElement(By.TagName("h2")).Text, "h2 edit user");

            VerifyAreEqual("testlastname9", driver.FindElement(By.CssSelector("[data-e2e=\"userLastName\"]")).GetAttribute("value"), "pre check edit pop up last name");
            VerifyAreEqual("testfirstname9", driver.FindElement(By.CssSelector("[data-e2e=\"userFirstName\"]")).GetAttribute("value"), "pre check edit pop up first name");
            VerifyAreEqual("testmail@mail.ru9", driver.FindElement(By.CssSelector("[data-e2e=\"userEmail\"]")).GetAttribute("value"), "pre check edit pop up email");
            VerifyIsTrue(driver.FindElements(By.XPath("//a[contains(text(),\"Удалить\")]")).Count > 0, "pre check edit pop up img");
            string imgFirstPopUp = driver.FindElement(By.TagName("img")).GetAttribute("src");
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"userPosition\"]")).GetAttribute("value"), "pre check edit pop up position");

            IWebElement selectElem1 = driver.FindElement(By.CssSelector("[data-e2e=\"userDepartment\"]"));
            SelectElement select1 = new SelectElement(selectElem1);
            VerifyIsTrue(select1.AllSelectedOptions[0].Text.Contains("Не выбран"), "pre check department text");
            
            VerifyIsTrue(select1.AllSelectedOptions[0].GetAttribute("selected").Equals("true"), "pre check department value");

            VerifyAreEqual("9", driver.FindElement(By.CssSelector("[data-e2e=\"userPhone\"]")).GetAttribute("value"), "pre check edit pop up phone");
            VerifyAreEqual("04.01.2003", driver.FindElement(By.CssSelector("[data-e2e=\"userBirthDay\"]")).GetAttribute("value"), "pre check edit pop up birthday");
            VerifyAreEqual("Москва", driver.FindElement(By.CssSelector("[data-e2e=\"userCity\"]")).GetAttribute("value"), "pre check edit pop up city");

            IWebElement selectElem2 = driver.FindElement(By.CssSelector("[data-e2e=\"userHeadUser\"]"));
            SelectElement select2 = new SelectElement(selectElem2);
            VerifyIsTrue(select2.AllSelectedOptions[0].Text.Contains("testlastname221 admin"), "pre check head user");

            VerifyAreEqual("", driver.FindElement(By.XPath("//input[@type='search']")).GetAttribute("value"), "pre check edit pop up role");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"userPermissionsAdminInput\"]")).Selected, "edit pop up selected admin");
            VerifyIsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"userEnabled\"]")).FindElement(By.TagName("input")).Selected, "edit pop up selected user disabled");
            
            //check edit
            driver.FindElement(By.CssSelector("[data-e2e=\"userLastName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"userLastName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"userLastName\"]")).SendKeys("Edited Surname");

            driver.FindElement(By.CssSelector("[data-e2e=\"userFirstName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"userFirstName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"userFirstName\"]")).SendKeys("Edited Name");

            driver.FindElement(By.CssSelector("[data-e2e=\"userEmail\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"userEmail\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"userEmail\"]")).SendKeys("EditedEmail@mail.ru");

            //add img by link
            driver.FindElement(By.LinkText("Загрузить фото")).Click();
            driver.FindElement(By.LinkText("Загрузить по ссылке")).Click();
            driver.FindElement(By.CssSelector("input[type=\"text\"]")).SendKeys("http://www.bugaga.ru/uploads/posts/2016-02/1454695389_prikol-7.jpg");
            driver.FindElement(By.LinkText("Загрузить")).Click();
            WaitForAjax();
            driver.FindElement(By.XPath("//button[contains(text(),\"Применить\")]")).Click();

            driver.FindElement(By.CssSelector("[data-e2e=\"userPosition\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"userPosition\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"userPosition\"]")).SendKeys("Edited PositionTest");
            
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"userDepartment\"]")))).SelectByText("Department5");

            driver.FindElement(By.CssSelector("[data-e2e=\"userPhone\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"userPhone\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"userPhone\"]")).SendKeys("+79279999999");
            ScrollTo(By.CssSelector("[data-e2e=\"userPhone\"]"));

            //birthday by print
            driver.FindElement(By.CssSelector("[data-e2e=\"userBirthDay\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"userBirthDay\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"userBirthDay\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"userBirthDay\"]")).SendKeys("23.12.1991");

            driver.FindElement(By.CssSelector("[data-e2e=\"userCity\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"userCity\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"userCity\"]")).SendKeys("Edited CityTest");
            
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"userHeadUser\"]")))).SelectByText("testlastname111 testfirstname111");

            driver.FindElement(By.CssSelector("[data-e2e=\"userPermissionsModer\"]")).Click();

            //choose role from existing
            driver.FindElement(By.XPath("//input[@type='search']")).Click();
            WaitForElem(By.CssSelector("span.ui-select-choices-row-inner"));
            ScrollTo(By.CssSelector("[data-e2e=\"userHeadUser\"]"));
            driver.FindElement(By.CssSelector("span.ui-select-choices-row-inner")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.CssSelector("[data-e2e=\"userEnabled\"]")).FindElement(By.TagName("span")).Click();
            Thread.Sleep(3000);

            driver.FindElement(By.CssSelector("[data-e2e=\"userButtonSave\"]")).Click();
            Thread.Sleep(3000);

            //check admin
            GoToAdmin("settings/userssettings");
            
            GetGridIdFilter("gridUsers", "testfirstname9 testlastname9");
            XPathContainsText("h1", "Сотрудники");
            WaitForAjax();
            

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "edited user");
            
            GetGridIdFilter("gridUsers", "Edited Name Edited Surname");
            XPathContainsText("h1", "Сотрудники");
            WaitForAjax();
            

            VerifyIsTrue(!GetGridCell(0, "PhotoSrc", "Users").FindElement(By.TagName("img")).GetAttribute("src").Contains("no-avatar"), "avatar");

            VerifyAreEqual("Edited Name Edited Surname", GetGridCell(0, "FullName", "Users").Text, "FullName");
            VerifyAreEqual("EditedEmail@mail.ru", GetGridCell(0, "Email", "Users").Text, "Email");
            VerifyAreEqual("Department5", GetGridCell(0, "DepartmentName", "Users").Text, "DepartmentName");
            VerifyAreEqual("Модератор; Role1", GetGridCell(0, "Roles", "Users").Text, "Roles");

            string imgSecondGrid = GetGridCell(0, "PhotoSrc", "Users").FindElement(By.TagName("img")).GetAttribute("src");
            VerifyIsFalse(imgFirstGrid.Equals(imgSecondGrid), "grid another img saved");

            VerifyIsTrue(GetGridCell(0, "Enabled", "Users").FindElement(By.TagName("input")).Selected, "Enabled");

            //check edit pop up
            GetGridCell(0, "_serviceColumn", "Users").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(3000);
            WaitForElem(By.CssSelector(".modal-content"));

            VerifyAreEqual("Edited Surname", driver.FindElement(By.CssSelector("[data-e2e=\"userLastName\"]")).GetAttribute("value"), "edit pop up last name");
            VerifyAreEqual("Edited Name", driver.FindElement(By.CssSelector("[data-e2e=\"userFirstName\"]")).GetAttribute("value"), "edit pop up first name");
            VerifyAreEqual("EditedEmail@mail.ru", driver.FindElement(By.CssSelector("[data-e2e=\"userEmail\"]")).GetAttribute("value"), "edit pop up email");
            VerifyIsTrue(driver.FindElements(By.XPath("//a[contains(text(),\"Удалить\")]")).Count > 0, "edit pop up img saved");
            string imgSecondPopUp = driver.FindElement(By.TagName("img")).GetAttribute("src");
            VerifyIsFalse(imgFirstPopUp.Equals(imgSecondPopUp), "edit pop up another img saved");
            VerifyAreEqual("Edited PositionTest", driver.FindElement(By.CssSelector("[data-e2e=\"userPosition\"]")).GetAttribute("value"), "edit pop up position");

            IWebElement selectElem3 = driver.FindElement(By.CssSelector("[data-e2e=\"userDepartment\"]"));
            SelectElement select3 = new SelectElement(selectElem3);
            VerifyIsTrue(select3.AllSelectedOptions[0].Text.Contains("Department5"), "edit pop up department");

            VerifyAreEqual("+79279999999", driver.FindElement(By.CssSelector("[data-e2e=\"userPhone\"]")).GetAttribute("value"), "edit pop up phone");
            VerifyAreEqual("23.12.1991", driver.FindElement(By.CssSelector("[data-e2e=\"userBirthDay\"]")).GetAttribute("value"), "edit pop up birthday");
            VerifyAreEqual("Edited CityTest", driver.FindElement(By.CssSelector("[data-e2e=\"userCity\"]")).GetAttribute("value"), "edit pop up city");

            IWebElement selectElem4 = driver.FindElement(By.CssSelector("[data-e2e=\"userHeadUser\"]"));
            SelectElement select4 = new SelectElement(selectElem4);
            VerifyIsTrue(select4.AllSelectedOptions[0].Text.Contains("testlastname111 testfirstname111"), "edit pop up head user");

            VerifyIsTrue(driver.FindElement(By.CssSelector(".ui-select-match")).Text.Contains("Role1"), "edit pop up role");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"userPermissionsModerInput\"]")).Selected, "edit pop up selected moderator");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"userEnabled\"]")).FindElement(By.TagName("input")).Selected, "edit pop up selected user enabled");
            
            VerifyFinally(testname);
        }

        [Test]
        public void SettingsUsersEditDelImg()
        {
            testname = "SettingsUsersEditDelImg";
            VerifyBegin(testname);

            GoToAdmin("settings/userssettings");
            
            GetGridIdFilter("gridUsers", "testfirstname26 testlastname26");
            XPathContainsText("h1", "Сотрудники");
            WaitForAjax();
            
            
            //pre check edit pop up
            VerifyAreEqual("Модератор", GetGridCell(0, "Roles", "Users").Text, "pre check grid Roles");
            VerifyIsTrue(!GetGridCell(0, "PhotoSrc", "Users").FindElement(By.TagName("img")).GetAttribute("src").Contains("no-avatar"), "pre check grid avatar");

            GetGridCell(0, "_serviceColumn", "Users").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(2000);
            WaitForElem(By.CssSelector(".modal-content"));

            VerifyIsTrue(driver.FindElements(By.XPath("//a[contains(text(),\"Удалить\")]")).Count > 0, "pre check edit pop up img");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"userPermissionsModerInput\"]")).Selected, "pre check edit pop up selected moderator");
            VerifyAreEqual("", driver.FindElement(By.XPath("//input[@type='search']")).GetAttribute("value"), "pre check edit pop up role");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"userEnabled\"]")).FindElement(By.TagName("input")).Selected, "pre check edit pop up selected user enabled");

            //check edit
            driver.FindElement(By.XPath("//a[contains(text(),\"Удалить\")]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"userPermissionsAdmin\"]")).Click();
            Thread.Sleep(1000);

            //choose 2 roles from existing
            driver.FindElement(By.XPath("//input[@type='search']")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector(".ui-select-choices-row-inner span")).Click();
           // XPathContainsText("span", "Role1");
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("//input[@type='search']")).Click();
            Thread.Sleep(1000);
            ScrollTo(By.CssSelector("[data-e2e=\"userHeadUser\"]"));
            driver.FindElement(By.CssSelector("#ui-select-choices-row-0-2")).Click();

           // XPathContainsText("span", "Role4");
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"userEnabled\"]")).FindElement(By.TagName("span")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"userButtonSave\"]")).Click();
            Thread.Sleep(1000);
            //check admin
            GoToAdmin("settings/userssettings");
            
            GetGridIdFilter("gridUsers", "testfirstname26 testlastname26");
            XPathContainsText("h1", "Сотрудники");
            WaitForAjax();
            

            VerifyIsTrue(GetGridCell(0, "PhotoSrc", "Users").FindElement(By.TagName("img")).GetAttribute("src").Contains("no-avatar"), "avatar");

            VerifyAreEqual("testfirstname26 testlastname26", GetGridCell(0, "FullName", "Users").Text, "FullName");
            VerifyAreEqual("Администратор; Role1; Role4", GetGridCell(0, "Roles", "Users").Text, "Roles");

            VerifyIsFalse(GetGridCell(0, "Enabled", "Users").FindElement(By.TagName("input")).Selected, "Enabled");

            //check edit pop up
            GetGridCell(0, "_serviceColumn", "Users").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(2000);
            WaitForElem(By.CssSelector(".modal-content"));

            VerifyIsFalse(driver.FindElements(By.XPath("//a[contains(text(),\"Удалить\")]")).Count > 0, "edit pop up img delete");
            
            VerifyIsTrue(driver.FindElement(By.CssSelector(".ui-select-match")).Text.Contains("Role1"), "edit pop up role 1");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".ui-select-match")).Text.Contains("Role4"), "edit pop up role 2");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"userPermissionsAdminInput\"]")).Selected, "edit pop up selected admin");
            VerifyIsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"userEnabled\"]")).FindElement(By.TagName("input")).Selected, "edit pop up selected user disabled");

            VerifyFinally(testname);
        }

        [Test]
        public void SettingsUsersEditDelRoles()
        {
            testname = "SettingsUsersEditDelRoles";
            VerifyBegin(testname);

            GoToAdmin("settings/userssettings");
            
            GetGridIdFilter("gridUsers", "testfirstname218 testlastname218");
            XPathContainsText("h1", "Сотрудники");
            WaitForAjax();
            

            //pre check edit pop up
            VerifyAreEqual("testfirstname218 testlastname218", GetGridCell(0, "FullName", "Users").Text, "pre check grid FullName");
            VerifyAreEqual("Модератор; Role3", GetGridCell(0, "Roles", "Users").Text, "pre check grid Roles");

            GetGridCell(0, "_serviceColumn", "Users").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(2000);
            WaitForElem(By.CssSelector(".modal-content"));

            VerifyIsTrue(driver.FindElement(By.CssSelector(".ui-select-match")).Text.Contains("Role3"), "pre check edit pop up role");

            //check edit delete role
            driver.FindElement(By.CssSelector(".close.ui-select-match-close")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"userButtonSave\"]")).Click();

            //check admin
            GoToAdmin("settings/userssettings");
            
            GetGridIdFilter("gridUsers", "testfirstname218 testlastname218");
            XPathContainsText("h1", "Сотрудники");
            WaitForAjax();
            

            VerifyAreEqual("testfirstname218 testlastname218", GetGridCell(0, "FullName", "Users").Text, "FullName");
            VerifyAreEqual("Модератор", GetGridCell(0, "Roles", "Users").Text, "Roles");

            //check edit pop up
            GetGridCell(0, "_serviceColumn", "Users").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(2000);
            WaitForElem(By.CssSelector(".modal-content"));

            VerifyAreEqual("", driver.FindElement(By.XPath("//input[@type='search']")).GetAttribute("value"), "edit pop up role 1");

            VerifyFinally(testname);
        }
    }
}