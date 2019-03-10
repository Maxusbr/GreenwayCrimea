using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Core.SQL2;
using AdvantShop.Web.Admin.Models.Cards;
using AdvantShop.Web.Infrastructure.Admin;
using AdvantShop.Helpers;

namespace AdvantShop.Web.Admin.Handlers.Cards
{
    public class GetCardHandler
    {
        private readonly CardFilterModel _filterModel;
        private SqlPaging _paging;

        public GetCardHandler(CardFilterModel filterModel)
        {
            _filterModel = filterModel;
        }

        public FilterResult<CardsModel> Execute()
        {
            var model = new FilterResult<CardsModel>();

            GetPaging();

            model.TotalItemsCount = _paging.TotalRowsCount;
            model.TotalPageCount = _paging.PageCount();
            model.TotalString = LocalizationService.GetResourceFormat("Admin.Grid.FildTotal", model.TotalItemsCount);

            if (model.TotalPageCount < _filterModel.Page && _filterModel.Page > 1)
            {
                return model;
            }

            model.DataItems = _paging.PageItemsList<CardsModel>();

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

            _paging.Select("CardId", "CardNumber", "Email", "StandardPhone".AsSqlField("MobilePhone"),
                           "isnull(LastName,'') + ' ' + isnull(FirstName,'')".AsSqlField("FIO"), "Grade.Name".AsSqlField("GradeName"),
                           "Grade.BonusPercent".AsSqlField("GradePersent"), "CreateOn".AsSqlField("Created"));
            _paging.From("[Bonus].[Card]");
            _paging.Inner_Join("[Bonus].Grade ON Grade.Id=Card.GradeId");
            _paging.Inner_Join("[Customers].Customer ON Customer.CustomerId=Card.CardId");

            if (_filterModel.CustomerFields != null)
            {
                _paging.Left_Join("Customers.CustomerFieldValuesMap ON CustomerFieldValuesMap.CustomerId = [Customer].[CustomerId]");
            }

            Sorting();
            Filter();
        }

        private void Filter()
        {
            if (_filterModel.Search.IsNotEmpty())
            {
                _paging.Where(
                    "(CardNumber LIKE '%'+{0}+'%' OR [Customer].Email LIKE '%'+{0}+'%' OR [Customer].Lastname + ' ' + [Customer].Firstname LIKE '%'+{0}+'%' OR [Customer].Phone LIKE '%'+{0}+'%')",
                    _filterModel.Search);
            }
            if (!string.IsNullOrEmpty(_filterModel.CardNumber))
            {
                _paging.Where("CardNumber = {0}", _filterModel.CardNumber.TryParseLong());
            }
            if (_filterModel.GradeId.HasValue)
            {
                _paging.Where("GradeId = {0}", _filterModel.GradeId.Value);
            }
            if (_filterModel.CreatedFrom.HasValue)
            {
                _paging.Where("CreateOn >= {0}", _filterModel.CreatedFrom.Value);
            }
            if (_filterModel.CreatedTo.HasValue)
            {
                _paging.Where("CreateOn <= {0}", _filterModel.CreatedTo.Value);
            }
            if (_filterModel.BonusAmountFrom.HasValue)
            {
                _paging.Where("BonusAmount >= {0}", _filterModel.BonusAmountFrom.Value);
            }
            if (_filterModel.BonusAmountTo.HasValue)
            {
                _paging.Where("BonusAmount <= {0}", _filterModel.BonusAmountTo.Value);
            }

            if (_filterModel.FIO.IsNotEmpty())
            {
                _paging.Where("[Customer].Lastname + ' ' + [Customer].Firstname LIKE '%'+{0}+'%' OR [Customer].Firstname + ' ' + [Customer].Lastname LIKE '%'+{0}+'%'", _filterModel.FIO);
            }
            if (_filterModel.Email.IsNotEmpty())
            {
                _paging.Where("Email LIKE '%'+{0}+'%'", _filterModel.Email);
            }
            if (_filterModel.MobilePhone.IsNotEmpty())
            {
                long? standartPhone = StringHelper.ConvertToStandardPhone(_filterModel.MobilePhone, true, true);
                _paging.Where("convert(nvarchar, StandardPhone) LIKE '%'+{0}+'%'", standartPhone != null ? standartPhone.ToString() : "null");
            }
            if (_filterModel.Location.IsNotEmpty())
            {
                _paging.Where("(SELECT TOP(1) City FROM Customers.Contact WHERE Contact.CustomerID = Customer.CustomerID) LIKE '%'+{0}+'%'", _filterModel.Location);
            }
            if (_filterModel.RegDateFrom.HasValue)
            {
                _paging.Where("RegistrationDateTime >= {0}", _filterModel.RegDateFrom.Value);
            }
            if (_filterModel.RegDateTo.HasValue)
            {
                _paging.Where("RegistrationDateTime <= {0}", _filterModel.RegDateTo.Value);
            }

            if (_filterModel.OrdersCountFrom.HasValue)
            {
                _paging.Where("(SELECT COUNT([Order].OrderId) FROM [Order].[Order] LEFT JOIN [Order].OrderCustomer ON [Order].OrderID = OrderCustomer.OrderId " +
                    "WHERE OrderCustomer.CustomerId = Customer.CustomerId AND [Order].PaymentDate IS NOT NULL) >= {0}", _filterModel.OrdersCountFrom.Value);
            }
            if (_filterModel.OrdersCountTo.HasValue)
            {
                _paging.Where("(SELECT COUNT([Order].OrderId) FROM [Order].[Order] LEFT JOIN [Order].OrderCustomer ON [Order].OrderID = OrderCustomer.OrderId " +
                    "WHERE OrderCustomer.CustomerId = Customer.CustomerId AND [Order].PaymentDate IS NOT NULL) <= {0}", _filterModel.OrdersCountTo.Value);
            }
            if (_filterModel.OrderSumFrom.HasValue)
            {
                _paging.Where("(SELECT ISNULL(SUM([Sum]),0) FROM [Order].[Order] LEFT JOIN [Order].OrderCustomer ON [Order].OrderID = OrderCustomer.OrderId " +
                    "WHERE OrderCustomer.CustomerId = Customer.CustomerId AND [Order].PaymentDate IS NOT NULL) >= {0}", _filterModel.OrderSumFrom.Value);
            }
            if (_filterModel.OrderSumTo.HasValue)
            {
                _paging.Where("(SELECT ISNULL(SUM([Sum]),0) FROM [Order].[Order] LEFT JOIN [Order].OrderCustomer ON [Order].OrderID = OrderCustomer.OrderId " +
                    "WHERE OrderCustomer.CustomerId = Customer.CustomerId AND [Order].PaymentDate IS NOT NULL) <= {0}", _filterModel.OrderSumTo.Value);
            }

            if (_filterModel.CustomerFields != null)
            {
                foreach (var fieldFilter in _filterModel.CustomerFields.Where(x => x.Value != null))
                {
                    var fieldsFilterModel = fieldFilter.Value;
                    if (fieldsFilterModel.DateFrom.HasValue)
                    {
                        _paging.Where("(CustomerFieldValuesMap.CustomerFieldId = {0} and CustomerFieldValuesMap.Value >= {1})", fieldFilter.Key, fieldsFilterModel.DateFrom.Value.ToString("yyyy-MM-dd"));
                    }
                    if (fieldsFilterModel.DateTo.HasValue)
                    {
                        _paging.Where("(CustomerFieldValuesMap.CustomerFieldId = {0} and CustomerFieldValuesMap.Value is not null and CustomerFieldValuesMap.Value <> '' and CustomerFieldValuesMap.Value <= {1})", fieldFilter.Key, fieldsFilterModel.DateTo.Value.ToString("yyyy-MM-dd"));
                    }
                    if (fieldsFilterModel.From.IsNotEmpty())
                    {
                        var value = fieldsFilterModel.From.TryParseInt(true);
                        _paging.Where("(CustomerFieldValuesMap.CustomerFieldId = {0} and CustomerFieldValuesMap.Value >= {1})", fieldFilter.Key, value ?? Int32.MaxValue);
                    }
                    if (fieldsFilterModel.To.IsNotEmpty())
                    {
                        var value = fieldsFilterModel.To.TryParseInt(true);
                        _paging.Where("(CustomerFieldValuesMap.CustomerFieldId = {0} and CustomerFieldValuesMap.Value is not null and CustomerFieldValuesMap.Value <> '' and CustomerFieldValuesMap.Value <= {1})", fieldFilter.Key, value ?? Int32.MaxValue);
                    }
                    if (fieldsFilterModel.ValueExact.IsNotEmpty())
                    {
                        _paging.Where("(CustomerFieldValuesMap.CustomerFieldId = {0} and CustomerFieldValuesMap.Value = {1})", fieldFilter.Key, fieldsFilterModel.ValueExact);
                    }
                    if (fieldsFilterModel.Value.IsNotEmpty())
                    {
                        _paging.Where("(CustomerFieldValuesMap.CustomerFieldId = {0} and CustomerFieldValuesMap.Value like '%' + {1} + '%')", fieldFilter.Key, fieldsFilterModel.Value);
                    }
                }
            }
        }

        private void Sorting()
        {
            if (string.IsNullOrEmpty(_filterModel.Sorting) || _filterModel.SortingType == FilterSortingType.None)
            {
                _paging.OrderByDesc("Created");
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