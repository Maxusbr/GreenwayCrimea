using System;
using System.IO;
using System.Text;
using System.Globalization;
using System.Threading.Tasks;
using System.Collections.Generic;

using AdvantShop.Core.Caching;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using AdvantShop.Statistic;
using AdvantShop.Customers;
using AdvantShop.Repository;
using CsvHelper;

namespace AdvantShop.ExportImport
{
    public class CsvImportCustomers
    {
        private readonly string _fullPath;
        private readonly bool _hasHeadrs;

        private readonly string _separator;
        private readonly string _encodings;

        private Dictionary<string, int> _fieldMapping;

        public CsvImportCustomers(string filePath, bool hasHeadrs, string separator, string encodings, Dictionary<string, int> fieldMapping)
        {
            _fullPath = filePath;
            _hasHeadrs = hasHeadrs;
            _fieldMapping = fieldMapping;
            _encodings = encodings;
            _separator = separator;
        }

        private CsvReader InitReader(bool? hasHeaderRecord = null)
        {
            var reader = new CsvReader(new StreamReader(_fullPath, Encoding.GetEncoding(_encodings ?? EncodingsEnum.Utf8.StrName())));

            reader.Configuration.Delimiter = _separator ?? SeparatorsEnum.SemicolonSeparated.StrName();
            reader.Configuration.HasHeaderRecord = hasHeaderRecord ?? _hasHeadrs;

            return reader;
        }

        public List<string[]> ReadFirstRecord()
        {
            var list = new List<string[]>();
            using (var csv = InitReader())
            {
                int count = 0;
                while (csv.Read())
                {
                    if (count == 2)
                        break;

                    if (csv.CurrentRecord != null)
                        list.Add(csv.CurrentRecord);
                    count++;
                }
            }
            return list;
        }

        public Task Process()
        {
            return CommonStatistic.StartNew(() =>
           {
               CommonStatistic.IsRun = true;
               try
               {
                   _process();
               }
               catch (Exception ex)
               {
                   Debug.Log.Error(ex);
                   CommonStatistic.WriteLog(ex.Message);
               }
               finally
               {
                   CommonStatistic.IsRun = false;
               }
           });
        }

        private void _process()
        {
            if (_fieldMapping == null)
                MapFileds();

            if (_fieldMapping == null)
                throw new Exception("can mapping colums");


            CommonStatistic.TotalRow = GetRowCount();


            ProcessRow();


            CommonStatistic.IsRun = false;
            CacheManager.Clean();
            FileHelpers.DeleteFile(_fullPath);
        }

        private void MapFileds()
        {
            _fieldMapping = new Dictionary<string, int>();
            using (var csv = InitReader(false))
            {
                csv.Read();
                for (var i = 0; i < csv.CurrentRecord.Length; i++)
                {
                    if (csv.CurrentRecord[i] == ECustomerFields.None.StrName()) continue;
                    if (!_fieldMapping.ContainsKey(csv.CurrentRecord[i]))
                        _fieldMapping.Add(csv.CurrentRecord[i], i);
                }
            }
        }

        private long GetRowCount()
        {
            long count = 0;
            using (var csv = InitReader())
            {
                while (csv.Read())
                    count++;
            }
            return count;
        }

        private void ProcessRow()
        {
            if (!File.Exists(_fullPath)) return;
            using (var csv = InitReader())
            {
                while (csv.Read())
                {
                    if (!CommonStatistic.IsRun)
                    {
                        csv.Dispose();
                        FileHelpers.DeleteFile(_fullPath);
                        return;
                    }
                    try
                    {
                        var customerInStrings = PrepareRow(csv);
                        if (customerInStrings == null) continue;

                        UpdateInsertCustomer(customerInStrings);

                    }
                    catch (Exception ex)
                    {
                        Debug.Log.Error(ex);
                    }
                }
            }
        }

        private Dictionary<ECustomerFields, object> PrepareRow(ICsvReader csv)
        {
            var customerInStrings = new Dictionary<ECustomerFields, object>();

            foreach (ECustomerFields field in Enum.GetValues(typeof(ECustomerFields)))
            {
                switch (field.Status())
                {
                    case CsvFieldStatus.String:
                        GetString(field, csv, customerInStrings);
                        break;
                    case CsvFieldStatus.StringRequired:
                        GetStringRequired(field, csv, customerInStrings);
                        break;
                    case CsvFieldStatus.NotEmptyString:
                        GetStringNotNull(field, csv, customerInStrings);
                        break;
                    case CsvFieldStatus.Float:
                        if (!GetDecimal(field, csv, customerInStrings))
                            return null;
                        break;
                    case CsvFieldStatus.NullableFloat:
                        if (!GetNullableDecimal(field, csv, customerInStrings))
                            return null;
                        break;
                    case CsvFieldStatus.Int:
                        if (!GetInt(field, csv, customerInStrings))
                            return null;
                        break;
                }
            }
            return customerInStrings;
        }

        private void UpdateInsertCustomer(Dictionary<ECustomerFields, object> customerInStrings)
        {
            try
            {
                Customer customer = null;

                var email = customerInStrings.ContainsKey(ECustomerFields.Email)
                    ? Convert.ToString(customerInStrings[ECustomerFields.Email])
                    : string.Empty;

                if (string.IsNullOrEmpty(email))
                {
                    CommonStatistic.TotalErrorRow++;
                    Log(CommonStatistic.RowPosition + ": not email");

                    customerInStrings.Clear();
                    CommonStatistic.RowPosition++;
                }

                customer = CustomerService.GetCustomerByEmail(email);
                if (customer == null)
                {
                    customer = new Customer (CustomerGroupService.DefaultCustomerGroup)
                    {
                        EMail = email
                    };
                }

                if (customerInStrings.ContainsKey(ECustomerFields.FirstName))
                    customer.FirstName = Convert.ToString(customerInStrings[ECustomerFields.FirstName]);

                if (customerInStrings.ContainsKey(ECustomerFields.LastName))
                    customer.LastName = Convert.ToString(customerInStrings[ECustomerFields.LastName]);

                if (customerInStrings.ContainsKey(ECustomerFields.Patronymic))
                    customer.Patronymic = Convert.ToString(customerInStrings[ECustomerFields.Patronymic]);

                if (customerInStrings.ContainsKey(ECustomerFields.Patronymic))
                    customer.Patronymic = Convert.ToString(customerInStrings[ECustomerFields.Patronymic]);

                if (customerInStrings.ContainsKey(ECustomerFields.Phone))
                {
                    customer.Phone = Convert.ToString(customerInStrings[ECustomerFields.Phone]);
                    customer.StandardPhone = Convert.ToString(customerInStrings[ECustomerFields.Phone]).TryParseLong();
                }

                if (customerInStrings.ContainsKey(ECustomerFields.Enabled))
                    customer.Enabled = Convert.ToBoolean(customerInStrings[ECustomerFields.Enabled]);

                if (customerInStrings.ContainsKey(ECustomerFields.AdminComment))
                    customer.AdminComment = Convert.ToString(customerInStrings[ECustomerFields.AdminComment]);

                if (customerInStrings.ContainsKey(ECustomerFields.CustomerGroup))
                {
                    var customerGroupName = Convert.ToString(customerInStrings[ECustomerFields.CustomerGroup]);
                    var customerGroup = CustomerGroupService.GetCustomerGroup(customerGroupName);
                    if (customerGroup == null)
                    {
                        customerGroup = new CustomerGroup
                        {
                            GroupDiscount = 0,
                            MinimumOrderPrice = 0,
                            GroupName = customerGroupName
                        };
                        CustomerGroupService.AddCustomerGroup(customerGroup);
                    }
                    customer.CustomerGroupId = customerGroup.CustomerGroupId;
                }

                if (customer.Id == Guid.Empty)
                {
                    customer.Id = CustomerService.InsertNewCustomer(customer);
                    CommonStatistic.TotalAddRow++;
                }
                else
                {
                    CustomerService.UpdateCustomer(customer);
                    CommonStatistic.TotalUpdateRow++;
                }

                if (customer.Id != Guid.Empty)
                {
                    CustomerContactFields(customerInStrings, customer.Id);
                }
            }
            catch (Exception e)
            {
                CommonStatistic.TotalErrorRow++;
                Log(CommonStatistic.RowPosition + ": " + e.Message);
            }

            customerInStrings.Clear();
            CommonStatistic.RowPosition++;
        }

        private void CustomerContactFields(IDictionary<ECustomerFields, object> fields, Guid customerId)
        {
            var customerContact = new CustomerContact();

            if (fields.ContainsKey(ECustomerFields.Country) && !string.IsNullOrEmpty(Convert.ToString(fields[ECustomerFields.Country])))
            {
                var country = CountryService.GetCountryByName(Convert.ToString(fields[ECustomerFields.Country]));
                if (country == null)
                {
                    country = new Country
                    {
                        Name = Convert.ToString(fields[ECustomerFields.Country]),
                        Iso2= string.Empty,
                        Iso3 = string.Empty
                    };
                    CountryService.Add(country);
                }
                customerContact.CountryId = country.CountryId;
            }
            if (fields.ContainsKey(ECustomerFields.Region) && !string.IsNullOrEmpty(Convert.ToString(fields[ECustomerFields.Region])))
            {
                var region = RegionService.GetRegion(Convert.ToString(fields.ContainsKey(ECustomerFields.Region)), customerContact.CountryId);
                if (region == null)
                {
                    region = new Region
                    {
                        CountryId = customerContact.CountryId,
                        Name = Convert.ToString(fields.ContainsKey(ECustomerFields.Region))
                    };
                    RegionService.InsertRegion(region);
                }

                customerContact.RegionId = region.RegionId;
            }

            if (fields.ContainsKey(ECustomerFields.City) && !string.IsNullOrEmpty(Convert.ToString(fields[ECustomerFields.City])))
            {
                var city = CityService.GetCityByName(Convert.ToString(fields[ECustomerFields.City]));
                if (city == null)
                {
                    city = new City
                    {
                        Name = Convert.ToString(fields[ECustomerFields.City]),
                        RegionId = (int)customerContact.RegionId
                    };
                    CityService.Add(city);
                }
                customerContact.City = city.Name;
            }

            if (fields.ContainsKey(ECustomerFields.Zip))
            {
                customerContact.Zip = Convert.ToString(fields[ECustomerFields.Zip]);
            }
            if (fields.ContainsKey(ECustomerFields.House))
            {
                customerContact.House = Convert.ToString(fields[ECustomerFields.House]);
            }
            if (fields.ContainsKey(ECustomerFields.Street))
            {
                customerContact.Street = Convert.ToString(fields[ECustomerFields.Street]);
            }
            if (fields.ContainsKey(ECustomerFields.Apartment))
            {
                customerContact.Apartment = Convert.ToString(fields[ECustomerFields.Apartment]);
            }

            CustomerService.AddContact(customerContact, customerId);
        }

        #region Help methods

        private bool GetString(ECustomerFields rEnum, ICsvReaderRow csv, IDictionary<ECustomerFields, object> customerInStrings)
        {
            var nameField = rEnum.StrName();
            if (_fieldMapping.ContainsKey(nameField))
                customerInStrings.Add(rEnum, TrimAnyWay(csv[_fieldMapping[nameField]]));
            return true;
        }

        private bool GetStringNotNull(ECustomerFields rEnum, ICsvReaderRow csv, IDictionary<ECustomerFields, object> customerInStrings)
        {
            var nameField = rEnum.StrName();
            if (!_fieldMapping.ContainsKey(nameField)) return true;
            var tempValue = TrimAnyWay(csv[_fieldMapping[nameField]]);
            if (!string.IsNullOrEmpty(tempValue))
                customerInStrings.Add(rEnum, tempValue);
            return true;
        }

        private bool GetStringRequired(ECustomerFields rEnum, ICsvReaderRow csv, IDictionary<ECustomerFields, object> customerInStrings)
        {
            var nameField = rEnum.StrName();
            if (!_fieldMapping.ContainsKey(nameField)) return true;
            var tempValue = TrimAnyWay(csv[_fieldMapping[nameField]]);
            if (!string.IsNullOrEmpty(tempValue))
                customerInStrings.Add(rEnum, tempValue);
            else
            {
                LogInvalidData(string.Format(LocalizationService.GetResource("Core.ExportImport.ImportCsv.CanNotEmpty"), rEnum.Localize(), CommonStatistic.RowPosition + 2));
                return false;
            }
            return true;
        }

        private bool GetDecimal(ECustomerFields rEnum, ICsvReaderRow csv, IDictionary<ECustomerFields, object> customerInStrings)
        {
            var nameField = rEnum.StrName();
            if (!_fieldMapping.ContainsKey(nameField)) return true;
            var value = TrimAnyWay(csv[_fieldMapping[nameField]]);
            if (string.IsNullOrEmpty(value))
                value = "0";
            float decValue;
            if (float.TryParse(value, out decValue))
            {
                customerInStrings.Add(rEnum, decValue);
            }
            else if (float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out decValue))
            {
                customerInStrings.Add(rEnum, decValue);
            }
            else
            {
                LogInvalidData(string.Format(LocalizationService.GetResource("Core.ExportImport.ImportCsv.MustBeNumber"), rEnum.Localize(), CommonStatistic.RowPosition + 2));
                return false;
            }
            return true;
        }

        private bool GetNullableDecimal(ECustomerFields rEnum, ICsvReaderRow csv, IDictionary<ECustomerFields, object> customerInStrings)
        {
            var nameField = rEnum.StrName();
            if (!_fieldMapping.ContainsKey(nameField)) return true;
            var value = TrimAnyWay(csv[_fieldMapping[nameField]]);

            if (string.IsNullOrEmpty(value))
            {
                customerInStrings.Add(rEnum, default(float?));
                return true;
            }

            float decValue;
            if (float.TryParse(value, out decValue))
            {
                customerInStrings.Add(rEnum, decValue);
            }
            else if (float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out decValue))
            {
                customerInStrings.Add(rEnum, decValue);
            }
            else
            {
                LogInvalidData(string.Format(LocalizationService.GetResource("Core.ExportImport.ImportCsv.MustBeNumber"), rEnum.Localize(), CommonStatistic.RowPosition + 2));
                return false;
            }
            return true;
        }

        private bool GetInt(ECustomerFields rEnum, ICsvReaderRow csv, IDictionary<ECustomerFields, object> customerInStrings)
        {
            var nameField = rEnum.StrName();
            if (!_fieldMapping.ContainsKey(nameField)) return true;
            var value = TrimAnyWay(csv[_fieldMapping[nameField]]);
            if (string.IsNullOrEmpty(value))
                value = "0";
            int intValue;
            if (int.TryParse(value, out intValue))
            {
                customerInStrings.Add(rEnum, intValue);
            }
            else
            {
                LogInvalidData(string.Format(LocalizationService.GetResource("Core.ExportImport.ImportCsv.MustBeNumber"), rEnum.Localize(), CommonStatistic.RowPosition + 2));
                return false;
            }
            return true;
        }

        private static string TrimAnyWay(string str)
        {
            return string.IsNullOrEmpty(str) ? str : str.Trim();
        }

        private static void LogInvalidData(string message)
        {
            CommonStatistic.WriteLog(message);
            CommonStatistic.TotalErrorRow++;
            CommonStatistic.RowPosition++;
        }

        private static void Log(string message)
        {
            CommonStatistic.WriteLog(message);
        }

        #endregion
    }
}