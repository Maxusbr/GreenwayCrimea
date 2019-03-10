using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using AdvantShop.Core.SQL;
using AdvantShop.Customers;
using AdvantShop.Module.Roistat.Domain;
using AdvantShop.Module.Roistat.Models.Roistat;

namespace AdvantShop.Module.Roistat.Handlers
{
    public class GetClients
    {
        #region Ctor
        
        private readonly RoistatOrdersExportModel _model;

        public GetClients(RoistatOrdersExportModel model)
        {
            _model = model;
        }
        
        #endregion

        public RoistatClientsResponse Execute()
        {
            var modifiedDate = DateTimeConverter.UnixTimeStampToDateTime(_model.Date);
            
            var response = new RoistatClientsResponse()
            {
                Clients =
                    GetCustomers(modifiedDate).Select(x => new RoistatClient()
                    {
                        Id = x.Id.ToString(),
                        Name = x.GetFullName(),
                        Email = x.EMail ?? "",
                        Phone = x.Phone ?? "",
                    }).ToList()
            };

            return response;
        }

        private List<Customer> GetCustomers(DateTime modifiedDate)
        {
            var clients =
                SQLDataAccess.ExecuteReadList(
                    "Select * From [Customers].[Customer] Where RegistrationDateTime > @RegistrationDateTime",
                    CommandType.Text,
                    CustomerService.GetFromSqlDataReader,
                    new SqlParameter("@RegistrationDateTime", modifiedDate));

            return clients;
        }

    }
}
