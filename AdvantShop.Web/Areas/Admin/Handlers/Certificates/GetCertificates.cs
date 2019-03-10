using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.SQL2;
using AdvantShop.Web.Admin.Models.Certificates;
using AdvantShop.Web.Infrastructure.Admin;
using AdvantShop.Orders;

namespace AdvantShop.Web.Admin.Handlers.Certificates
{
    public class GetCertificates
    {
        private readonly CertificatesFilterModel _filterModel;
        private SqlPaging _paging;

        public GetCertificates(CertificatesFilterModel filterModel)
        {
            _filterModel = filterModel;
        }

        public FilterResult<AdminCertificatesModel> Execute()
        {
            var model = new FilterResult<AdminCertificatesModel>();

            GetPaging();

            model.TotalItemsCount = _paging.TotalRowsCount;
            model.TotalPageCount = _paging.PageCount();
            model.TotalString = string.Format("Найдено сертификатов: {0}", model.TotalItemsCount);

            if (model.TotalPageCount < _filterModel.Page && _filterModel.Page > 1)
            {
                return model;
            }

            model.DataItems = _paging.PageItemsList<AdminCertificatesModel>();
            
            return model;
        }

        public List<int> GetItemsIds()
        {
            GetPaging();

            return _paging.ItemsIds<int>("[Certificate].CertificateID");
        }

        private void GetPaging()
        {
            _paging = new SqlPaging()
            {
                ItemsPerPage = _filterModel.ItemsPerPage,
                CurrentPageIndex = _filterModel.Page
            };

            _paging.Select(
                "[Certificate].CertificateID",
                "[Certificate].CertificateCode",
                "[Certificate].OrderID",
                "FromName",
                "ToName",
                "[Certificate].Sum",
                "Used",
                "Enable",
                "ToEmail",
                "CreationDate",
                "ApplyOrderNumber",
                "[Order].PaymentDate".AsSqlField("Payed"));

            _paging.From("[Order].[Certificate]");
            _paging.Left_Join("[Order].[Order] on [Order].OrderID = [Certificate].OrderID");
            
            Sorting();
            Filter();
        }

        private void Filter()
        {
            if (!string.IsNullOrEmpty(_filterModel.Search))
                _paging.Where("[Certificate].CertificateCode LIKE '%'+{0}+'%' OR [Certificate].OrderID LIKE '%'+{0}+'%'", _filterModel.Search);
            
            if (!string.IsNullOrEmpty(_filterModel.CertificateCode))
                _paging.Where("[Certificate].CertificateCode LIKE '%'+{0}+'%'", _filterModel.CertificateCode);

            if (_filterModel.OrderId != null)
                _paging.Where("[Certificate].OrderID = {0}", _filterModel.OrderId.TryParseInt());
            
            if (!string.IsNullOrEmpty(_filterModel.ApplyOrderNumber))
                _paging.Where("ApplyOrderNumber LIKE '%'+{0}+'%'", _filterModel.ApplyOrderNumber);

            if (_filterModel.Sum != null)
                _paging.Where("[Certificate].Sum = {0}", _filterModel.Sum.TryParseFloat());

            if (_filterModel.Used != null)
                _paging.Where("Used = {0}", (bool)_filterModel.Used ? "1" : "0");

            if (_filterModel.Enable != null)
                _paging.Where("Enable = {0}", (bool)_filterModel.Enable ? "1" : "0");

            if (_filterModel.Payed != null)
                _paging.Where((bool)_filterModel.Payed ? "[Order].PaymentDate is not null" : "[Order].PaymentDate is null");
            
            DateTime from, to;

            if (!string.IsNullOrWhiteSpace(_filterModel.CreationDateFrom) && DateTime.TryParse(_filterModel.CreationDateFrom, out from))
            {
                _paging.Where("CreationDate >= {0}", from);
            }

            if (!string.IsNullOrWhiteSpace(_filterModel.CreationDateTo) && DateTime.TryParse(_filterModel.CreationDateTo, out to))
            {
                _paging.Where("CreationDate <= {0}", to);
            }
        }

        private void Sorting()
        {
            if (string.IsNullOrEmpty(_filterModel.Sorting) || _filterModel.SortingType == FilterSortingType.None)
            {
                _paging.OrderByDesc("CreationDate");
                return;
            }

            var sorting = _filterModel.Sorting.ToLower().Replace("formatted", "");
            if (sorting == "fullsum") sorting = "sum";
            if (sorting == "paid") sorting = "payed";
            if (sorting == "creationdates") sorting = "creationdate";

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