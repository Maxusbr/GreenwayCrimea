using AdvantShop.Web.Admin.Models.Leads;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Core.SQL2;
using AdvantShop.Web.Infrastructure.Admin;
using System;
using AdvantShop.Configuration;
using AdvantShop.Customers;
using AdvantShop.Repository.Currencies;

namespace AdvantShop.Web.Admin.Handlers.Leads
{
    public class GetLeads
    {
        private LeadsFilterModel _filterModel;
        private SqlPaging _paging;

        public GetLeads(LeadsFilterModel filterModel)
        {
            _filterModel = filterModel;
        }

        public FilterResult<LeadsFilterResultModel> Execute()
        {
            var model = new FilterResult<LeadsFilterResultModel>();

            GetPaging();

            model.TotalItemsCount = _paging.TotalRowsCount;
            model.TotalPageCount = _paging.PageCount();

            if (model.TotalPageCount < _filterModel.Page && _filterModel.Page > 1)
                return model;
            
            model.DataItems = _paging.PageItemsList<LeadsFilterResultModel>();
            
            model.TotalString += " " +
                                 LocalizationService.GetResourceFormat("Admin.Leads.Grid.TotalPrice",
                                     _paging.GetCustomData("Sum ([Lead].[Sum]) as totalPrice", "", reader => Helpers.SQLDataHelper.GetFloat(reader, "totalPrice"), true).FirstOrDefault()) + 
                                 " " + CurrencyService.CurrentCurrency.Symbol;
            
            return model;
        }

        public List<int> GetItemsIds()
        {
            GetPaging();

            return _paging.ItemsIds<int>("[Lead].Id");
        }

        private void GetPaging()
        {
            _paging = new SqlPaging()
            {
                ItemsPerPage = _filterModel.ItemsPerPage,
                CurrentPageIndex = _filterModel.Page
            };

            _paging.Select(
                "[Lead].Id",

                "[Lead].FirstName", // для обратной совместимости
                "[Lead].LastName",
                "[Lead].Patronymic",

                "[Lead].Phone",
                "[Lead].Email",

                "[Lead].CustomerId",
                "[LeadCustomer].FirstName as CustomerFirstName",
                "[LeadCustomer].LastName as CustomerLastName",
                "[LeadCustomer].Patronymic as CustomerPatronymic",

                "(isNull([LeadCustomer].LastName, '') + isNull([LeadCustomer].FirstName, '') + isNull([LeadCustomer].Patronymic, ''))".AsSqlField("FullName"),

                "[LeadCustomer].Phone as CustomerPhone",
                "[LeadCustomer].Email as CustomerEmail",

                "[Lead].Sum",
                "isNull((Select Sum(Price*Amount) From [Order].[LeadItem] as items Left Join [Order].[LeadCurrency] On [LeadCurrency].[LeadId] = [Lead].[Id] Where items.LeadId = [Lead].[Id]), 0)".AsSqlField("ProductsSum"),
                "isNull((Select Sum(Amount) From [Order].[LeadItem] as items Left Join [Order].[LeadCurrency] On [LeadCurrency].[LeadId] = [Lead].[Id] Where items.LeadId = [Lead].[Id]), 0)".AsSqlField("ProductsCount"),
                
                "[Lead].ManagerId",
                "[ManagerCustomer].FirstName + ' ' + [ManagerCustomer].LastName".AsSqlField("ManagerName"),
                "[Lead].CreatedDate",
                "[DealStatus].[Name]".AsSqlField("DealStatusName")
                );

            _paging.From("[Order].[Lead]");
            _paging.Left_Join("[Customers].[Customer] as LeadCustomer on [Lead].[CustomerId] = [LeadCustomer].[CustomerId]");
            _paging.Left_Join("[Customers].[Managers] ON [Lead].[ManagerId] = [Managers].[ManagerID]");
            _paging.Left_Join("[Customers].[Customer] as ManagerCustomer ON [Managers].[CustomerId] = [ManagerCustomer].[CustomerId]");
            _paging.Left_Join("[CRM].[DealStatus] ON [DealStatus].[Id] = [Lead].[DealStatusId]");


            Sorting();
            Filter();
        }

        private void Filter()
        {
            if (!string.IsNullOrWhiteSpace(_filterModel.CustomerId))
            {
                _paging.Where("Lead.CustomerId = {0}", _filterModel.CustomerId);
            }

            if (_filterModel.DealStatusId != null)
            {
                _paging.Where("DealStatusId = {0}", _filterModel.DealStatusId.Value);
            }
            
            if (!string.IsNullOrWhiteSpace(_filterModel.Search))
            {
                _paging.Where(
                    "[Lead].Id LIKE '%'+{0}+'%' OR [Lead].FirstName LIKE '%'+{0}+'%' OR [Lead].LastName LIKE '%'+{0}+'%' OR [Lead].Patronymic LIKE '%'+{0}+'%' OR [Lead].Phone LIKE '%'+{0}+'%' OR [Lead].Email LIKE '%'+{0}+'%'" +
                    " OR [LeadCustomer].FirstName LIKE '%'+{0}+'%' OR [LeadCustomer].LastName LIKE '%'+{0}+'%' OR [LeadCustomer].Patronymic LIKE '%'+{0}+'%' OR [LeadCustomer].Phone LIKE '%'+{0}+'%' OR [LeadCustomer].Email LIKE '%'+{0}+'%' ",
                    _filterModel.Search);
            }

            if (!string.IsNullOrWhiteSpace(_filterModel.Name))
            {
                _paging.Where(
                    "[Lead].FirstName LIKE '%'+{0}+'%' OR [Lead].LastName LIKE '%'+{0}+'%' OR [Lead].Patronymic LIKE '%'+{0}+'%'" +
                    " OR [LeadCustomer].FirstName LIKE '%'+{0}+'%' OR [LeadCustomer].LastName LIKE '%'+{0}+'%' OR [LeadCustomer].Patronymic LIKE '%'+{0}+'%' ",
                    _filterModel.Name);
            }

            DateTime dateFrom, dateTo;
            if (_filterModel.CreatedDateFrom != null && DateTime.TryParse(_filterModel.CreatedDateFrom, out dateFrom))
            {
                _paging.Where("Lead.CreatedDate >= {0}", dateFrom);
            }
            if (_filterModel.CreatedDateTo != null && DateTime.TryParse(_filterModel.CreatedDateTo, out dateTo))
            {
                _paging.Where("Lead.CreatedDate <= {0}", dateTo);
            }
            
            if (_filterModel.SumFrom != null)
            {
                _paging.Where("Lead.Sum >= {0}", _filterModel.SumFrom.Value);
            }
            if (_filterModel.SumTo != null)
            {
                _paging.Where("Lead.Sum <= {0}", _filterModel.SumTo.Value);
            }

            if (!string.IsNullOrWhiteSpace(_filterModel.ManagerCustomerId))
            {
                _paging.Where("ManagerCustomer.CustomerId = {0}", _filterModel.ManagerCustomerId);
            }

            if (_filterModel.OrderSourceId != null)
            {
                _paging.Where("Lead.OrderSourceId = {0}", _filterModel.OrderSourceId.Value);
            }

            var customer = CustomerContext.CurrentCustomer;

            if (customer.IsModerator)
            {
                var manager = ManagerService.GetManager(customer.Id);
                if (manager != null && manager.Active)
                {
                    if (SettingsManager.ManagersLeadConstraint == ManagersLeadConstraint.Assigned)
                    {
                        _paging.Where("Lead.ManagerId = {0}", manager.ManagerId);
                    }
                    else if (SettingsManager.ManagersLeadConstraint == ManagersLeadConstraint.AssignedAndFree)
                    {
                        _paging.Where("(Lead.ManagerId = {0} or Lead.ManagerId is null)", manager.ManagerId);
                    }
                }
            }
        }

        private void Sorting()
        {
            if (string.IsNullOrEmpty(_filterModel.Sorting) || _filterModel.SortingType == FilterSortingType.None)
            {
                _paging.OrderByDesc("[Lead].Id");
                return;
            }

            string sorting = _filterModel.Sorting.ToLower().Replace("formatted", "");
            
            var field = _paging.SelectFields().FirstOrDefault(x => x.FieldName == sorting);
            if (field != null)
            {
                if (_filterModel.SortingType == FilterSortingType.Asc)
                {
                    _paging.OrderBy(field);
                }
                else
                {
                    _paging.OrderByDesc(field);
                }
            }
        }
    }
}
