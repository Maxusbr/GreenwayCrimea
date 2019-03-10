using System;
using System.Web.UI.WebControls;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Core.SQL;

namespace AdvantShop.Admin.UserControls.Customer
{
    public partial class Leads : System.Web.UI.UserControl
    {
        public Guid CustomerId { get; set; }
        public string Phone { get; set; }

        private SqlPaging _paging;

        protected void Page_Load(object sender, EventArgs e)
        {
            _paging = new SqlPaging
            {
                TableName =
                    "[Order].[Lead] " +
                    "LEFT JOIN [Order].[LeadItem] ON [LeadItem].[LeadId]=[Lead].[Id] " +
                    "LEFT JOIN [Order].[LeadCurrency] ON [LeadCurrency].[LeadId]=[Lead].[Id]",
                ItemsPerPage = 10
            };

            _paging.AddFieldsRange(
                new Field("Lead.Id as ID") { IsDistinct = true },
                new Field("Lead.Name"),
                new Field("Lead.Phone"),
                new Field("Lead.Email"),
                //new Field("Lead.OrderSourceId"),
                new Field("Lead.CustomerId"),
                new Field("LeadStatus"),
                new Field("(Select Sum(Price*Amount) From [Order].[LeadItem] Where [LeadItem].[LeadId]=[Id]) as Sum"),
                new Field("Lead.CreatedDate") { Sorting = SortDirection.Descending },
                new Field("CurrencyValue"),
                new Field("CurrencyCode"),
                new Field("CurrencySymbol"),
                new Field("IsCodeBefore")
                );

            if (SettingsCheckout.EnableManagersModule)
            {
                _paging.TableName +=
                    " LEFT JOIN [Customers].[Managers] ON [Lead].[ManagerId]=[Managers].[ManagerID] " +
                    " LEFT JOIN [Customers].[Customer] as ManagerCustomer ON [Managers].[CustomerId]=[ManagerCustomer].[CustomerId] ";

                _paging.AddField(new Field("ManagerCustomer.CustomerId as ManagerCustomerId"));
                _paging.AddField(new Field("[ManagerCustomer].FirstName + ' ' + [ManagerCustomer].LastName as ManagerName"));
            }
            else
            {
                _paging.AddField(new Field("null as ManagerCustomerId"));
                _paging.AddField(new Field("'' as ManagerName"));
            }

            // Vladimir: тут нужна хитрая sql
            //if (Phone.IsNotEmpty())
            //{
            //    _paging.AddCondition("OR Lead.Phone = @phone");
            //    _paging.AddParam(new SqlParam() { ParameterName = "@phone", Value = Phone });
            //}
            

            if (CustomerId != Guid.Empty)
            {
                _paging.Fields["Lead.CustomerId"].Filter = new EqualFieldFilter()
                {
                    ParamName = "@CustomerId",
                    Value = CustomerId.ToString()
                };
            }

            //if (!string.IsNullOrWhiteSpace(Phone))
            //{
            //    _paging.Fields["Lead.Phone"].Filter = new EqualFieldFilter() { ParamName = "@Phone", Value = Phone };
            //}
            

            //lvLeads.DataSource = _paging.PageItems;
            //lvLeads.DataBind();
        }

        protected string RenderLeadStatus(string type)
        {
            var t = (LeadStatus)Enum.Parse(typeof(LeadStatus), type);
            return t.Localize();
        }
    }
}