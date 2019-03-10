using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AdvantShop.Customers;
using AdvantShop.FilePath;
using AdvantShop.Saas;
using AdvantShop.Web.Admin.Models.CustomerSegments;
using AdvantShop.Web.Infrastructure.Admin;
using CsvHelper;
using CsvHelper.Configuration;

namespace AdvantShop.Web.Admin.Handlers.CustomerSegments
{
    public class ExportCustomersBySegment
    {
        private readonly FilterResult<CustomerBySegmentViewModel> _collection;
        private readonly string _fileName;
        private readonly bool _showAdditionalFileds;

        public ExportCustomersBySegment(FilterResult<CustomerBySegmentViewModel> collection, string fileName)
        {
            _collection = collection;
            _fileName = fileName;
            _showAdditionalFileds = !SaasDataService.IsSaasEnabled ||
                                    (SaasDataService.IsSaasEnabled &&
                                     SaasDataService.CurrentSaasData.CustomerAdditionFields);
        }

        public string Execute()
        {
            if (_collection == null || _collection.DataItems == null)
                return "";

            var fileDirectory = FoldersHelper.GetPathAbsolut(FolderType.PriceTemp);

            using (var writer = new CsvWriter(new StreamWriter(fileDirectory + _fileName, false, Encoding.UTF8), new CsvConfiguration {Delimiter = ";"}))
            {
                var additionalFields = new List<CustomerField>();
                if (_showAdditionalFileds)
                    additionalFields = CustomerFieldService.GetCustomerFields(true);

                WriteHeader(writer, additionalFields);

                foreach (var item in _collection.DataItems)
                {
                    WriteItem(writer, item, additionalFields);
                }
            }
            return fileDirectory + _fileName;
        }

        private void WriteHeader(CsvWriter writer, List<CustomerField> additionalFields)
        {
            writer.WriteField("CustomerId");
            writer.WriteField("Name");
            writer.WriteField("Email");
            writer.WriteField("Phone");

            //writer.WriteField("ManagerId");
            writer.WriteField("ManagerName");

            //writer.WriteField("Rating");

            writer.WriteField("LastOrderId");
            writer.WriteField("LastOrderNumber");

            writer.WriteField("OrdersSum");
            writer.WriteField("OrdersCount");

            writer.WriteField("Location");

            writer.WriteField("RegistrationDateTime");

            foreach (var field in additionalFields)
            {
                writer.WriteField(field.Name);
            }

            writer.NextRecord();
        }

        private void WriteItem(CsvWriter writer, CustomerBySegmentViewModel customer, List<CustomerField> additionalFields)
        {
            writer.WriteField(customer.CustomerId);
            writer.WriteField(customer.Name);
            writer.WriteField(customer.Email);
            writer.WriteField(customer.Phone);

            //writer.WriteField(customer.ManagerId);
            writer.WriteField(customer.ManagerName);

            //writer.WriteField(customer.Rating);

            writer.WriteField(customer.LastOrderId);
            writer.WriteField(customer.LastOrderNumber);

            writer.WriteField(customer.OrdersSum);
            writer.WriteField(customer.OrdersCount);

            writer.WriteField(customer.Location);

            writer.WriteField(customer.RegistrationDateTime);

            if (_showAdditionalFileds)
            {
                var values = CustomerFieldService.GetCustomerFieldsWithValue(customer.CustomerId);
                foreach (var field in additionalFields)
                {
                    var customerFieldValue = values.FirstOrDefault(item => item.Id == field.Id);
                    writer.WriteField(customerFieldValue != null ? customerFieldValue.Value : string.Empty);
                }
            }

            writer.NextRecord();
        }
    }
}
