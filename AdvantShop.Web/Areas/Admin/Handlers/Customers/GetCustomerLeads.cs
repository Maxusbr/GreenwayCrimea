using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Configuration;
using AdvantShop.Web.Admin.Models.Customers;
using AdvantShop.Web.Infrastructure.Admin;
using SqlPaging = AdvantShop.Core.SQL2.SqlPaging;

namespace AdvantShop.Web.Admin.Handlers.Customers
{
    public class GetCustomerLeads
    {
        private readonly CustomerLeadsFilterModel _filterModel;
        private SqlPaging _paging;

        public GetCustomerLeads(CustomerLeadsFilterModel filterModel)
        {
            _filterModel = filterModel;
        }

        public FilterResult<CustomerLeadModel> Execute()
        {
            var model = new FilterResult<CustomerLeadModel>();

            GetPaging();

            model.TotalItemsCount = _paging.TotalRowsCount;
            model.TotalPageCount = _paging.PageCount();

            if (model.TotalPageCount < _filterModel.Page && _filterModel.Page > 1)
            {
                return model;
            }

            model.DataItems = _paging.PageItemsList<CustomerLeadModel>();
            
            return model;
        }

        public List<Guid> GetItemsIds(string fieldName)
        {
            GetPaging();

            return _paging.ItemsIds<Guid>(fieldName);
        }

        private void GetPaging()
        {
            _paging = new SqlPaging()
            {
                ItemsPerPage = _filterModel.ItemsPerPage,
                CurrentPageIndex = _filterModel.Page
            };

            _paging.Select(
                "Lead.Id",
                "Lead.Name",
                "Lead.Phone",
                "Lead.Email",
                "Lead.CustomerId",
                "LeadStatus",
                "(Select Sum(Price*Amount) From [Order].[LeadItem] Where [LeadItem].[LeadId]=[Id]) as ItemsSum",
                "Lead.CreatedDate",
                "CurrencyValue",
                "CurrencyCode",
                "CurrencySymbol",
                "IsCodeBefore"
            );

            _paging.From("[Order].[Lead]");
            //_paging.Left_Join("[Order].[LeadItem] ON [LeadItem].[LeadId]=[Lead].[Id]");
            _paging.Left_Join("[Order].[LeadCurrency] ON [LeadCurrency].[LeadId]=[Lead].[Id]");

            if (SettingsCheckout.EnableManagersModule)
            {
                _paging.Select(
                    "ManagerCustomer.CustomerId as ManagerCustomerId",
                    "[ManagerCustomer].FirstName + ' ' + [ManagerCustomer].LastName as ManagerName"
                );

                _paging.Left_Join("[Customers].[Managers] ON [Lead].[ManagerId]=[Managers].[ManagerID]");
                _paging.Left_Join("[Customers].[Customer] as ManagerCustomer ON [Managers].[CustomerId]=[ManagerCustomer].[CustomerId]");
            }
            else
            {
                _paging.Select(
                    "null as ManagerCustomerId",
                    "'' as ManagerName"
                );
            }

            _paging.Where("Lead.CustomerId = {0}", _filterModel.CustomerId);

            Sorting();
        }

        

        private void Sorting()
        {
            if (string.IsNullOrEmpty(_filterModel.Sorting) || _filterModel.SortingType == FilterSortingType.None)
            {
                _paging.OrderByDesc("Lead.CreatedDate");
                return;
            }

            var sorting = _filterModel.Sorting.ToLower().Replace("formatted", "");

            var field = _paging.SelectFields().FirstOrDefault(x => x.FieldName == sorting);
            if (field != null)
            {
                if (_filterModel.SortingType == FilterSortingType.Asc)
                {
                    _paging.OrderBy(sorting);
                }
                else
                {
                    _paging.OrderByDesc(sorting);
                }
            }
        }
    }
}