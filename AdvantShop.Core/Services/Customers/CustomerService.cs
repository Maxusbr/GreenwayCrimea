//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using AdvantShop.Core.Caching;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Loging;
using AdvantShop.Core.Services.Loging.Calls;
using AdvantShop.Core.Services.Loging.Emails;
using AdvantShop.Core.Services.Loging.Events;
using AdvantShop.Core.Services.Loging.Smses;
using AdvantShop.Core.SQL;
using AdvantShop.Helpers;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Services.Bonuses.Service;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Core.Services.Mails;


namespace AdvantShop.Customers
{
    public class CustomerService
    {
        public static IEmailLoger EmailLoger = LogingManager.GetEmailLoger();
        public static ISmsLoger SmsLoger = LogingManager.GetSmsLoger();
        public static IEventLoger EventLoger = LogingManager.GetEventLoger();
        public static ICallLoger CallLoger = LogingManager.GetCallLoger();


        public static void UpdateLogers()
        {
            EmailLoger = LogingManager.GetEmailLoger();
            SmsLoger = LogingManager.GetSmsLoger();
            EventLoger = LogingManager.GetEventLoger();
            CallLoger = LogingManager.GetCallLoger();
        }


        public static string GetCustomerEmailById(Guid custId)
        {
            return SQLDataAccess.ExecuteScalar<string>(
                "Select [Email] From [Customers].[Customer] Where [CustomerID] = @CustomerID",
                CommandType.Text,
                new SqlParameter("@CustomerID", custId));
        }

        public static void DeleteCustomer(Guid customerId)
        {
            var customerEmail = GetCustomerEmailById(customerId);
            SubscriptionService.Unsubscribe(customerEmail);
            CardService.Delete(customerId);
            SQLDataAccess.ExecuteNonQuery("[Customers].[sp_DeleteCustomer]", CommandType.StoredProcedure,
                new SqlParameter("@CustomerID", customerId));

            ModulesExecuter.DeleteCustomer(customerId);

            CacheManager.RemoveByPattern(CacheNames.Customer);
        }

        public static void DeleteContact(Guid contactId)
        {
            SQLDataAccess.ExecuteNonQuery("[Customers].[sp_DeleteCustomerContact]", CommandType.StoredProcedure,
                new SqlParameter("@ContactID", contactId));

            ModulesExecuter.DeleteContact(contactId);

            CacheManager.RemoveByPattern(CacheNames.Customer);
        }

        public static int GetCustomerGroupId(Guid customerId)
        {
            return
                SQLDataHelper.GetInt(
                    SQLDataAccess.ExecuteScalar(
                        "SELECT [CustomerGroupId] FROM [Customers].[Customer] WHERE [CustomerID] = @CustomerID",
                        CommandType.Text, new SqlParameter { ParameterName = "@CustomerID", Value = customerId }),
                    CustomerGroupService.DefaultCustomerGroup);
        }

        public static Customer GetCustomer(Guid customerId)
        {
            Customer customer;

            var cacheName = CacheNames.Customer + customerId;

            if (!CacheManager.TryGetValue(cacheName, out customer))
            {
                customer = SQLDataAccess.ExecuteReadOne<Customer>(
                    "SELECT * FROM [Customers].[Customer] WHERE [CustomerID] = @CustomerID",
                    CommandType.Text,
                    GetFromSqlDataReader,
                    new SqlParameter("@CustomerID", customerId));

                CacheManager.Insert(cacheName, customer ?? new Customer());
            }

            return customer != null && customer.Id != Guid.Empty ? customer : null;
        }

        public static Customer GetCustomer(int innerId)
        {
            return
                SQLDataAccess.ExecuteReadOne<Customer>(
                    "SELECT * FROM [Customers].[Customer] WHERE [InnerId] = @InnerId",
                    CommandType.Text,
                    GetFromSqlDataReader,
                    new SqlParameter("@InnerId", innerId));
        }

        public static List<Customer> GetCustomersbyRole(Role role)
        {
            return
                SQLDataAccess.ExecuteReadList<Customer>(
                    "SELECT * FROM [Customers].[Customer] WHERE [CustomerRole] = @CustomerRole",
                    CommandType.Text,
                    GetFromSqlDataReader,
                    new SqlParameter("@CustomerRole", ((int)role).ToString()));
        }

        public static List<Customer> GetCustomersByRoles(Role role, params Role[] roles)
        {
            var rolesList = new List<int>() { (int)role };
            rolesList.AddRange(roles.Where(x => role != x).Select(x => (int)x));
            return
                SQLDataAccess.ExecuteReadList<Customer>(
                    string.Format("SELECT * FROM [Customers].[Customer] WHERE [CustomerRole] in ({0})", rolesList.AggregateString(",")),
                    CommandType.Text,
                    GetFromSqlDataReader);
        }

        public static List<Customer> GetCustomersForAutocomplete(string query)
        {
            if (query.IsDecimal())
            {
                return
                SQLDataAccess.ExecuteReadList<Customer>(
                    "SELECT * FROM [Customers].[Customer] " +
                    "WHERE [CustomerRole] = @CustomerRole AND (" +

                    (query.Length >= 6 ?  
                    "[Phone] like '%' + @q + '%' " +
                    "OR [StandardPhone] like '%' + @q + '%' " +
                    "OR "  
                    : "") +

                    "[Email] like @q + '%')",
                    CommandType.Text,
                    GetFromSqlDataReader,
                    new SqlParameter("@q", query),
                    new SqlParameter("@CustomerRole", ((int)Role.User).ToString()));
            }
            else
            {
                var translitKeyboard = StringHelper.TranslitToRusKeyboard(query);

                return
                    SQLDataAccess.ExecuteReadList<Customer>(
                        "SELECT * FROM [Customers].[Customer] " +
                        "WHERE [CustomerRole] = @CustomerRole AND " +
                        "([FirstName] like @q + '%' OR [FirstName] like @qtr + '%' " +
                        "OR [LastName] like @q + '%' OR [LastName] like @qtr + '%' " +
                        "OR [Phone] like '%' + @q + '%' " +
                        "OR [Email] like @q + '%')",
                        CommandType.Text,
                        GetFromSqlDataReader,
                        new SqlParameter("@q", query),
                        new SqlParameter("@qtr", translitKeyboard),
                        new SqlParameter("@CustomerRole", ((int)Role.User).ToString()));
            }
        }

        public static bool ExistsCustomer(Guid customerId)
        {
            bool isExist;
            var cacheName = CacheNames.Customer + customerId + "_ExistsCustomer";

            if (!CacheManager.TryGetValue(cacheName, out isExist))
            {
                isExist = SQLDataAccess.ExecuteScalar(
                              "SELECT [CustomerID] FROM [Customers].[Customer] WHERE [CustomerID] = @CustomerID",
                              CommandType.Text,
                              new SqlParameter("@CustomerID", customerId)) != null;

                CacheManager.Insert(cacheName, isExist, 1);
            }
            return isExist;
        }

        public static Customer GetCustomerByEmail(string email)
        {
            return SQLDataAccess.ExecuteReadOne<Customer>(
                "[Customers].[sp_GetCustomerByEmail]", CommandType.StoredProcedure,
                GetFromSqlDataReader, new SqlParameter("@Email", email));
        }

        public static List<Customer> GetCustomersByPhone(string phone)
        {
            var phoneLong = phone.TryParseLong(true);
            return SQLDataAccess.ExecuteReadList<Customer>(
                "Select * from Customers.Customer where Phone=@phone " + (phoneLong.HasValue ? "or StandardPhone=@phoneLong" : string.Empty), CommandType.Text,
                GetFromSqlDataReader,
                new SqlParameter("@phone", phone),
                new SqlParameter("@phoneLong", phoneLong ?? (object)DBNull.Value)
                );
        }

        public static List<Customer> GetCustomerBirthdayInTheNextWeek()
        {
            var dateFrom = DateTime.Now;
            var dateTo = dateFrom.AddDays(7);
            return SQLDataAccess.ExecuteReadList<Customer>(
                "SELECT TOP(5) * " +
                "FROM [Customers].[Customer] where [BirthDay] is not null and month(BirthDay) >= @MonthFrom and month(BirthDay) <= @MonthTo " +
                (dateFrom.Day < dateTo.Day ? "and day(BirthDay) >= @DayFrom and day(BirthDay) <= @DayTo " : "and (day(BirthDay) >= @DayFrom or day(BirthDay) <= @DayTo) ") +
                " and CustomerRole > 0 order by BirthDay", CommandType.Text, GetFromSqlDataReader,
                new SqlParameter("@DayFrom", dateFrom.Day),
                new SqlParameter("@DayTo", dateTo.Day),
                new SqlParameter("@MonthFrom", dateFrom.Month),
                new SqlParameter("@MonthTo", dateTo.Month)
                );
        }

        public static Customer GetCustomerByRole(Role role)
        {
            return SQLDataAccess.ExecuteReadOne<Customer>(
                "Select top(1) * from Customers.Customer where CustomerRole=@CustomerRole", CommandType.Text,
                GetFromSqlDataReader, new SqlParameter { ParameterName = "@CustomerRole", Value = role });
        }


        public static Customer GetCustomerByOpenAuthIdentifier(string identifier)
        {
            return SQLDataAccess.ExecuteReadOne<Customer>(
                "[Customers].[sp_GetCustomerByOpenAuthIdentifier]", CommandType.StoredProcedure,
                GetFromSqlDataReader, new SqlParameter { ParameterName = "@Identifier", Value = identifier });
        }

        public static Customer GetFromSqlDataReader(SqlDataReader reader)
        {
            var customer = new Customer(true)
            {
                Id = SQLDataHelper.GetGuid(reader, "CustomerID"),
                CustomerGroupId = SQLDataHelper.GetInt(reader, "CustomerGroupId", 0),
                EMail = SQLDataHelper.GetString(reader, "EMail"),
                FirstName = SQLDataHelper.GetString(reader, "FirstName"),
                LastName = SQLDataHelper.GetString(reader, "LastName"),
                Patronymic = SQLDataHelper.GetString(reader, "Patronymic"),
                RegistrationDateTime = SQLDataHelper.GetDateTime(reader, "RegistrationDateTime"),
                Phone = SQLDataHelper.GetString(reader, "Phone"),
                StandardPhone = SQLDataHelper.GetNullableLong(reader, "StandardPhone"),
                Password = SQLDataHelper.GetString(reader, "Password"),
                CustomerRole = (Role)SQLDataHelper.GetInt(reader, "CustomerRole"),
                BonusCardNumber = SQLDataHelper.GetNullableLong(reader, "BonusCardNumber"),
                AdminComment = SQLDataHelper.GetString(reader, "AdminComment"),
                ManagerId = SQLDataHelper.GetNullableInt(reader, "ManagerId"),
                Rating = SQLDataHelper.GetInt(reader, "Rating"),
                Avatar = SQLDataHelper.GetString(reader, "Avatar"),
                Enabled = SQLDataHelper.GetBoolean(reader, "Enabled"),
                HeadCustomerId = SQLDataHelper.GetNullableGuid(reader, "HeadCustomerId"),
                BirthDay = SQLDataHelper.GetNullableDateTime(reader, "BirthDay"),
                City = SQLDataHelper.GetString(reader, "City"),
                InnerId = SQLDataHelper.GetInt(reader, "InnerId"),
            };

            return customer;
        }

        public static CustomerContact GetContactFromSqlDataReader(SqlDataReader reader)
        {
            var contact = new CustomerContact
            {
                ContactId = SQLDataHelper.GetGuid(reader, "ContactID"),
                CustomerGuid = SQLDataHelper.GetGuid(reader, "CustomerID"),
                Name = SQLDataHelper.GetString(reader, "Name"),
                City = SQLDataHelper.GetString(reader, "City"),
                Country = SQLDataHelper.GetString(reader, "Country"),
                Zip = SQLDataHelper.GetString(reader, "Zip"),
                Region = SQLDataHelper.GetString(reader, "Zone"),
                CountryId = SQLDataHelper.GetInt(reader, "CountryID"),
                RegionId = SQLDataHelper.GetNullableInt(reader, "RegionID"),

                Street = SQLDataHelper.GetString(reader, "Street"),
                House = SQLDataHelper.GetString(reader, "House"),
                Apartment = SQLDataHelper.GetString(reader, "Apartment"),
                Structure = SQLDataHelper.GetString(reader, "Structure"),
                Entrance = SQLDataHelper.GetString(reader, "Entrance"),
                Floor = SQLDataHelper.GetString(reader, "Floor"),
            };

            return contact;
        }

        public static CustomerContact GetCustomerContact(string contactId)
        {
            var id = Guid.Empty;
            if (Guid.TryParse(contactId, out id))
            {
                return GetCustomerContact(id);
            }
            return null;
        }

        public static CustomerContact GetCustomerContact(Guid contactId)
        {
            var contact = SQLDataAccess.ExecuteReadOne(
                "SELECT * FROM [Customers].[Contact] WHERE [ContactID] = @id",
                CommandType.Text,
                GetContactFromSqlDataReader,
                new SqlParameter("@id", contactId));

            return contact;
        }

        public static List<CustomerContact> GetCustomerContacts(Guid customerId)
        {
            return SQLDataAccess.ExecuteReadList(
                "SELECT * FROM [Customers].[Contact] WHERE CustomerId = @CustomerId",
                CommandType.Text, GetContactFromSqlDataReader,
                new SqlParameter("@CustomerId", customerId));
        }

        public static IList<Customer> GetCustomers()
        {
            return SQLDataAccess.ExecuteReadList<Customer>("SELECT * FROM [Customers].[Customer]", CommandType.Text,
                GetFromSqlDataReader);
        }

        public static Dictionary<Guid, long> GetCustomersPhones()
        {
            var dict = new Dictionary<Guid, long>();
            dict.AddRange(
                SQLDataAccess.ExecuteReadIEnumerable<KeyValuePair<Guid, long>>(
                    "SELECT distinct CustomerId, StandardPhone FROM [Customers].[Customer] where StandardPhone is not null",
                    CommandType.Text,
                    reader =>
                        new KeyValuePair<Guid, long>(SQLDataHelper.GetGuid(reader, "CustomerID"),
                            SQLDataHelper.GetLong(reader, "StandardPhone"))));
            return dict;
        }

        public static Dictionary<Guid, long> GetSubscribedCustomersPhones()
        {
            var dict = new Dictionary<Guid, long>();
            dict.AddRange(
                SQLDataAccess.ExecuteReadIEnumerable<KeyValuePair<Guid, long>>(
                    "SELECT distinct CustomerId, StandardPhone FROM [Customers].[Customer] " +
                    "INNER JOIN [Customers].[Subscription] ON [Subscription].[Email] = [Customer].[Email] " +
                    "where StandardPhone is not null AND Subscribe = 1",
                    CommandType.Text,
                    reader =>
                        new KeyValuePair<Guid, long>(SQLDataHelper.GetGuid(reader, "CustomerID"),
                            SQLDataHelper.GetLong(reader, "StandardPhone"))));
            return dict;
        }

        public static List<string> GetCustomersEmails()
        {
            return SQLDataAccess.ExecuteReadColumn<string>("SELECT Email FROM [Customers].[Customer]", CommandType.Text,
                "Email");
        }

        public static Guid AddContact(CustomerContact contact, Guid customerId)
        {
            var id = SQLDataAccess.ExecuteScalar(
                "INSERT INTO [Customers].[Contact] " +
                "(CustomerID, Name, Country, City, Zone, Zip, CountryID, RegionID, Street, House, Apartment, Structure, Entrance, Floor) OUTPUT Inserted.ContactID VALUES " +
                "(@CustomerID, @Name, @Country, @City, @Zone, @Zip, @CountryID, @RegionID, @Street, @House, @Apartment, @Structure, @Entrance, @Floor); ",
                CommandType.Text,

                new SqlParameter("@CustomerID", customerId),
                new SqlParameter("@Name", contact.Name ?? string.Empty),
                new SqlParameter("@Country", contact.Country ?? string.Empty),
                new SqlParameter("@City", contact.City ?? string.Empty),
                new SqlParameter("@Zone", contact.Region ?? string.Empty),
                new SqlParameter("@Zip", contact.Zip ?? string.Empty),
                new SqlParameter("@CountryID", contact.CountryId != 0 ? contact.CountryId : (object)DBNull.Value),
                new SqlParameter("@RegionID", contact.RegionId.HasValue && contact.RegionId > 0 ? contact.RegionId : (object)DBNull.Value),

                new SqlParameter("@Street", contact.Street ?? string.Empty),
                new SqlParameter("@House", contact.House ?? string.Empty),
                new SqlParameter("@Apartment", contact.Apartment ?? string.Empty),
                new SqlParameter("@Structure", contact.Structure ?? string.Empty),
                new SqlParameter("@Entrance", contact.Entrance ?? string.Empty),
                new SqlParameter("@Floor", contact.Floor ?? string.Empty)
                );

            contact.CustomerGuid = customerId;
            contact.ContactId = SQLDataHelper.GetGuid(id);

            ModulesExecuter.AddContact(contact);

            CacheManager.RemoveByPattern(CacheNames.Customer + customerId);

            return contact.ContactId;
        }

        public static void UpdateContact(CustomerContact contact)
        {
            SQLDataAccess.ExecuteNonQuery(
                //"[Customers].[sp_UpdateCustomerContact]", CommandType.StoredProcedure,
                "Update [Customers].[Contact] Set " +
                "Name=@Name, Country=@Country, City=@City, Zone=@Zone, Zip=@Zip, CountryID=@CountryID, RegionID=@RegionID, " +
                "Street=@Street, House=@House, Apartment=@Apartment, Structure=@Structure, Entrance=@Entrance, Floor=@Floor " +
                "WHERE ContactID = @ContactID ",
                CommandType.Text,

                new SqlParameter("@ContactID", contact.ContactId),
                new SqlParameter("@Name", contact.Name ?? string.Empty),
                new SqlParameter("@Country", contact.Country ?? string.Empty),
                new SqlParameter("@City", contact.City ?? string.Empty),
                new SqlParameter("@Zone", contact.Region ?? string.Empty),
                new SqlParameter("@Zip", contact.Zip ?? string.Empty),
                new SqlParameter("@CountryID", contact.CountryId != 0 ? contact.CountryId : (object)DBNull.Value),
                new SqlParameter("@RegionID", contact.RegionId.HasValue && contact.RegionId > 0 ? contact.RegionId : (object)DBNull.Value),

                new SqlParameter("@Street", contact.Street ?? string.Empty),
                new SqlParameter("@House", contact.House ?? string.Empty),
                new SqlParameter("@Apartment", contact.Apartment ?? string.Empty),
                new SqlParameter("@Structure", contact.Structure ?? string.Empty),
                new SqlParameter("@Entrance", contact.Entrance ?? string.Empty),
                new SqlParameter("@Floor", contact.Floor ?? string.Empty)
            );

            ModulesExecuter.UpdateContact(contact);

            CacheManager.RemoveByPattern(CacheNames.Customer + contact.CustomerGuid);
        }

        public static bool UpdateCustomer(Customer customer)
        {
            if (customer == null)
                return false;

            SQLDataAccess.ExecuteNonQuery("[Customers].[sp_UpdateCustomerInfo]", CommandType.StoredProcedure,
                new SqlParameter("@CustomerID", customer.Id),
                new SqlParameter("@FirstName", customer.FirstName ?? String.Empty),
                new SqlParameter("@LastName", customer.LastName ?? String.Empty),
                new SqlParameter("@Patronymic", customer.Patronymic ?? String.Empty),
                new SqlParameter("@Phone", customer.Phone ?? String.Empty),
                new SqlParameter("@StandardPhone", customer.StandardPhone ?? (object)DBNull.Value),
                new SqlParameter("@Email", customer.EMail ?? string.Empty),
                new SqlParameter("@CustomerGroupId", customer.CustomerGroupId == 0 ? (object)DBNull.Value : customer.CustomerGroupId),
                new SqlParameter("@CustomerRole", customer.CustomerRole),
                new SqlParameter("@BonusCardNumber", customer.BonusCardNumber ?? (object)DBNull.Value),
                new SqlParameter("@AdminComment", customer.AdminComment ?? (object)DBNull.Value),
                new SqlParameter("@ManagerId", customer.ManagerId ?? (object)DBNull.Value),
                new SqlParameter("@Rating", customer.Rating),
                new SqlParameter("@Avatar", customer.Avatar ?? (object)DBNull.Value),
                new SqlParameter("@Enabled", customer.Enabled),
                new SqlParameter("@HeadCustomerId", customer.HeadCustomerId ?? (object)DBNull.Value),
                new SqlParameter("@BirthDay", customer.BirthDay ?? (object)DBNull.Value),
                new SqlParameter("@City", customer.City ?? (object)DBNull.Value)
                );

            if (customer.EMail.IsNotEmpty() &&
                SubscriptionService.IsSubscribe(customer.EMail) != customer.SubscribedForNews)
            {
                if (customer.SubscribedForNews)
                {
                    SubscriptionService.Subscribe(customer.EMail);
                }
                else
                {
                    SubscriptionService.Unsubscribe(customer.EMail);
                }
            }

            ModulesExecuter.UpdateCustomer(customer);

            CacheManager.RemoveByPattern(CacheNames.Customer + customer.Id);

            return true;
        }

        public static void UpdateCustomerEmail(Guid id, string email)
        {
            SQLDataAccess.ExecuteNonQuery("Update Customers.Customer Set Email = @Email Where CustomerID = @CustomerID",
                CommandType.Text, new SqlParameter("@CustomerID", id), new SqlParameter("@Email", email));

            ModulesExecuter.UpdateCustomer(id);

            CacheManager.RemoveByPattern(CacheNames.Customer + id);
        }

        public static Customer GetCustomerByEmailAndPassword(string email, string password, bool isHash)
        {
            return SQLDataAccess.ExecuteReadOne<Customer>("[Customers].[sp_GetCustomerByEmailAndPassword]",
                CommandType.StoredProcedure, GetFromSqlDataReader,
                new SqlParameter { ParameterName = "@Email", Value = email },
                new SqlParameter
                {
                    ParameterName = "@Password",
                    Value = isHash ? password : SecurityHelper.GetPasswordHash(password)
                });
        }

        public static string ConvertToLinedAddress(CustomerContact cc)
        {
            var address = string.Empty;

            if (!string.IsNullOrEmpty(cc.Country.Trim()))
            {
                address += cc.Country + ", ";
            }

            if (cc.Region.Trim() != "-")
            {
                address += cc.Region + ", ";
            }

            if (!string.IsNullOrEmpty(cc.City.Trim()))
            {
                address += cc.City + ", ";
            }

            if (cc.Zip.Trim() != "-")
            {
                address += cc.Zip + ", ";
            }

            if (!string.IsNullOrEmpty(cc.Street))
            {
                address += cc.Street + " " + cc.House + (cc.Apartment != null ? ", " + cc.Apartment : "");
            }

            return address;
        }

        public static bool ExistsEmail(string strUserEmail)
        {
            if (String.IsNullOrEmpty(strUserEmail))
            {
                return false;
            }

            bool boolRes =
                SQLDataAccess.ExecuteScalar<int>(
                    "SELECT COUNT(CustomerID) FROM [Customers].[Customer] WHERE [Email] = @Email;", CommandType.Text,
                    new SqlParameter("@Email", strUserEmail)) > 0;

            return boolRes;
        }

        public static void ChangePassword(Guid customerId, string strNewPassword, bool isPassHashed)
        {
            SQLDataAccess.ExecuteNonQuery("[Customers].[sp_ChangePassword]", CommandType.StoredProcedure,
                new SqlParameter { ParameterName = "@CustomerID", Value = customerId },
                new SqlParameter
                {
                    ParameterName = "@Password",
                    Value = isPassHashed ? strNewPassword : SecurityHelper.GetPasswordHash(strNewPassword)
                }
            );

            CacheManager.RemoveByPattern(CacheNames.Customer + customerId);
        }

        public static Guid InsertNewCustomer(Customer customer)
        {
            if (!string.IsNullOrEmpty(customer.EMail) && CheckCustomerExist(customer.EMail))
                return Guid.Empty;

            var temp = SQLDataAccess.ExecuteScalar("[Customers].[sp_AddCustomer]", CommandType.StoredProcedure,
                new SqlParameter("@CustomerID", customer.Id != Guid.Empty ? customer.Id : (object)DBNull.Value),
                new SqlParameter("@CustomerGroupID", customer.CustomerGroupId),
                new SqlParameter("@Password", SecurityHelper.GetPasswordHash(customer.Password)),
                new SqlParameter("@FirstName", customer.FirstName ?? String.Empty),
                new SqlParameter("@LastName", customer.LastName ?? String.Empty),
                new SqlParameter("@Patronymic", customer.Patronymic ?? String.Empty),
                new SqlParameter("@Phone", String.IsNullOrEmpty(customer.Phone) ? (object)DBNull.Value : customer.Phone),
                new SqlParameter("@StandardPhone", customer.StandardPhone ?? (object)DBNull.Value),
                new SqlParameter("@RegistrationDateTime", DateTime.Now),
                new SqlParameter("@Email", customer.EMail ?? string.Empty),
                new SqlParameter("@CustomerRole", customer.CustomerRole),
                new SqlParameter("@BonusCardNumber", customer.BonusCardNumber ?? (object)DBNull.Value),
                new SqlParameter("@AdminComment", customer.AdminComment ?? (object)DBNull.Value),
                new SqlParameter("@ManagerId", customer.ManagerId ?? (object)DBNull.Value),
                new SqlParameter("@Rating", customer.Rating),
                new SqlParameter("@Enabled", customer.Enabled),
                new SqlParameter("@HeadCustomerId", customer.HeadCustomerId ?? (object)DBNull.Value),
                new SqlParameter("@BirthDay", customer.BirthDay ?? (object)DBNull.Value),
                new SqlParameter("@City", customer.City ?? (object)DBNull.Value)
                ).ToString();

            if (customer.SubscribedForNews)
                SubscriptionService.Subscribe(customer.EMail);

            customer.Id = new Guid(temp);

            ModulesExecuter.AddCustomer(customer);

            CacheManager.RemoveByPattern(CacheNames.Customer);

            return customer.Id;
        }

        public static string GetContactId(CustomerContact contact)
        {
            var res =
                SQLDataHelper.GetNullableGuid(SQLDataAccess.ExecuteScalar("[Customers].[sp_GetContactIDByContent]",
                    CommandType.StoredProcedure,
                    new SqlParameter("@Name", contact.Name),
                    new SqlParameter("@Country", contact.Country),
                    new SqlParameter("@City", contact.City),
                    new SqlParameter("@Zone", contact.Region ?? ""),
                    new SqlParameter("@Zip", contact.Zip ?? ""),
                    new SqlParameter("@Street", contact.Street ?? ""),
                    new SqlParameter("@CustomerID", contact.CustomerGuid)
                    ));
            return res == null ? null : res.ToString();
        }

        public static bool CheckCustomerExist(string email)
        {
            return SQLDataAccess.ExecuteScalar<int>(
                "SELECT COUNT([CustomerID]) FROM [Customers].[Customer] WHERE [Email] = @Email",
                CommandType.Text, new SqlParameter("@Email", email)) != 0;
        }

        public static bool AddOpenIdLinkCustomer(Guid customerGuid, string identifier)
        {
            return SQLDataHelper.GetInt(SQLDataAccess.ExecuteScalar(
                "Insert Into [Customers].[OpenIdLinkCustomer] (CustomerID, OpenIdIdentifier) Values (@CustomerID, @OpenIdIdentifier)",
                CommandType.Text,
                new SqlParameter("@CustomerID", customerGuid),
                new SqlParameter("@OpenIdIdentifier", identifier))) != 0;
        }

        public static bool IsExistOpenIdLinkCustomer(string identifier)
        {
            return SQLDataAccess.ExecuteScalar<int>(
                "SELECT COUNT([CustomerID]) FROM [Customers].[OpenIdLinkCustomer] WHERE [OpenIdIdentifier] = @OpenIdIdentifier",
                CommandType.Text,
                new SqlParameter("@OpenIdIdentifier", identifier)) != 0;
        }

        public static void ChangeCustomerGroup(Guid customerId, int customerGroupId)
        {
            SQLDataAccess.ExecuteNonQuery(
                "Update [Customers].[Customer] Set CustomerGroupId = @CustomerGroupId WHERE CustomerID = @CustomerID",
                CommandType.Text, new SqlParameter("@CustomerID", customerId),
                new SqlParameter("@CustomerGroupId", customerGroupId));

            ModulesExecuter.UpdateCustomer(customerId);

            CacheManager.RemoveByPattern(CacheNames.Customer + customerId);
        }

        public static void UpdateAdminComment(Guid customerId, string comment)
        {
            SQLDataAccess.ExecuteNonQuery(
                "Update [Customers].[Customer] Set AdminComment = @AdminComment WHERE CustomerID = @CustomerID",
                CommandType.Text,
                new SqlParameter("@CustomerID", customerId),
                new SqlParameter("@AdminComment", comment));

            ModulesExecuter.UpdateCustomer(customerId);

            CacheManager.RemoveByPattern(CacheNames.Customer + customerId);
        }

        public static void UpdateCustomerRating(Guid customerId, int rating)
        {
            SQLDataAccess.ExecuteNonQuery(
                "Update [Customers].[Customer] Set [Rating] = @Rating WHERE CustomerID = @CustomerID",
                CommandType.Text,
                new SqlParameter("@CustomerID", customerId),
                new SqlParameter("@Rating", rating));

            ModulesExecuter.UpdateCustomer(customerId);

            CacheManager.RemoveByPattern(CacheNames.Customer + customerId);
        }

        public static void ChangeCustomerManager(Guid customerId, int? managerId)
        {
            SQLDataAccess.ExecuteNonQuery(
                "Update [Customers].[Customer] Set ManagerId = @ManagerId WHERE CustomerID = @CustomerID",
                CommandType.Text,
                new SqlParameter("@CustomerID", customerId),
                new SqlParameter("@ManagerId", managerId ?? (object)DBNull.Value));

            ModulesExecuter.UpdateCustomer(customerId);

            CacheManager.RemoveByPattern(CacheNames.Customer + customerId);
        }

        public static string GetCurrentCustomerManager()
        {
            return SQLDataAccess.ExecuteScalar<string>(
                "Select [Customer].FirstName + ' ' + [Customer].[LastName] From [Customers].[Customer] LEFT JOIN [Customers].[Managers] On [Customer].ManagerId = [Managers].ManagerId WHERE [Customer].CustomerID = @CustomerID",
                CommandType.Text,
                new SqlParameter("@CustomerID", CustomerContext.CurrentCustomer.Id));
        }

        public static bool CanDelete(Guid customerId)
        {
            List<string> messages;
            return CanDelete(customerId, out messages);
        }

        public static bool CanDelete(Guid customerId, out List<string> messages)
        {
            messages = new List<string>();
            var currentCustomer = CustomerContext.CurrentCustomer;
            if (currentCustomer == null)
                return false;
            var customer = GetCustomer(customerId);
            if (customer == null)
            {
                messages.Add(LocalizationService.GetResource("Core.Customers.ErrorDeleteCustomer.NotFound"));
                return false;
            }

            if (customer.Id == currentCustomer.Id)
                messages.Add(LocalizationService.GetResource("Core.Customers.ErrorDeleteCustomer.SelfDelete"));
            if (customer.IsAdmin && currentCustomer.IsModerator)
                messages.Add(LocalizationService.GetResource("Core.Customers.ErrorDeleteCustomer.IsAdmin"));

            Manager manager;
            if (customer.IsManager && (manager = ManagerService.GetManager(customer.Id)) != null)
            {
                string managerMessage;
                if (!ManagerService.CanDelete(manager.ManagerId, out managerMessage))
                    messages.Add(managerMessage);
            }

            return !messages.Any();
        }

        /// <summary>
        /// Get sended by site emails
        /// </summary>
        public static List<Email> GetEmails(Guid customerId, string email)
        {
            return EmailLoger.GetEmails(customerId, email);
        }

        /// <summary>
        /// Get emails from imap host
        /// </summary>
        public static List<EmailImap> GetEmails(string email)
        {
            return CacheManager.Get("GetEmailsByImap" + email, 1, () =>
            {
                var imapService = new ImapMailService();
                return imapService.GetEmails(email);
            });
        }

        public static EmailImap GetEmailImap(string uid, string folder)
        {
            var imapService = new ImapMailService();
            return imapService.GetEmail(uid, folder);
        }


        public static List<TextMessage> GetSms(Guid customerId, long phone)
        {
            return SmsLoger.GetSms(customerId, phone);
        }

        public static List<Event> GetEvent(Guid customerId)
        {
            return EventLoger.GetEvents(customerId);
        }

        public static List<Call> GetCalls(Guid customerId, string phone)
        {
            return CallLoger.GetCalls(customerId, phone);
        }
    }
}