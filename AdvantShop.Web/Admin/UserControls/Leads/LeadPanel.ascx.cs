using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Core.Services.Statistic;
using AdvantShop.Core.SQL;

namespace AdvantShop.Admin.UserControls.Leads
{
    public class LeadStatusModel
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Color { get; set; }
        public int LeadsCount { get; set; }
    }

    public partial class LeadPanel : System.Web.UI.UserControl
    {
        protected SqlPaging _paging;
        protected string Status;

        protected void Page_PreRender(object sender, EventArgs e)
        {
            lvOrders.DataSource = _paging.PageItems;
            lvOrders.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Status = Request["status"];

            var statuses = new List<LeadStatusModel>();

            foreach (LeadStatus status in Enum.GetValues(typeof (LeadStatus)))
            {
                statuses.Add(new LeadStatusModel()
                {
                    Name = status.Localize(),
                    Value = status.ToString(),
                    Color = status.ColorValue(),
                    LeadsCount = LeadService.GetLeadsCount(default(int?))
                });
            }
            lvStatuses.DataSource = statuses;
            lvStatuses.DataBind();


            if (!IsPostBack)
            {
                LoadLeads(Status, Request["page"].TryParseInt(1));
                lblTotalOrdersCount.Text = LeadService.GetLeadsCount(default(int?)).ToString();
            }
            else
            {
                _paging = (SqlPaging) (ViewState["Paging"]);
            }

        }

        protected void sdsStatuses_Init(object sender, EventArgs e)
        {
            ((SqlDataSource) sender).ConnectionString = Connection.GetConnectionString();
        }

        protected void lvOrderStatuses_OnItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "ShowByStatus")
            {
                Status = Convert.ToString(e.CommandArgument);
                LoadLeads(Status);
            }
        }

        protected void lbtnAllOrders_Click(object sender, EventArgs e)
        {
            Status = "";
            LoadLeads();
        }

        protected void LoadLeads(string status = null, int page = 1)
        {
            _paging = new SqlPaging
            {
                TableName = "[Order].[Lead] " +
                            "INNER JOIN [Order].[LeadCurrency] ON [Lead].[Id] = [LeadCurrency].[LeadId] "
            };

            _paging.AddFieldsRange(new Field[]
            {
                new Field {Name = "[Lead].[Id]"},
                new Field {Name = "[Lead].[LeadStatus]"},
                new Field {Name = "([Lead].[FirstName] + ' ' + [Lead].[LastName]) as Name"},
                new Field {Name = "[Lead].[CreatedDate]", Sorting = SortDirection.Descending},
            });

            if (!string.IsNullOrWhiteSpace(status))
            {
                _paging.Fields["[Lead].[LeadStatus]"].Filter = new EqualFieldFilter
                {
                    ParamName = "@Status",
                    Value = status
                };
            }

            _paging.ItemsPerPage = 10;
            _paging.CurrentPageIndex = page;

            pageNumberer.PageCount = _paging.PageCount;
            pageNumberer.CurrentPageIndex = _paging.CurrentPageIndex;
            ViewState["Paging"] = _paging;
        }

        protected void pn_SelectedPageChanged(object sender, EventArgs e)
        {
            _paging.CurrentPageIndex = pageNumberer.CurrentPageIndex;
        }

        protected string GetStatusColor(string status)
        {
            LeadStatus leadStatus;

            Enum.TryParse(status, true, out leadStatus);

            return leadStatus.ColorValue();
        }
    }
}