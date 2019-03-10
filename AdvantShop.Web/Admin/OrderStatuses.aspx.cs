//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using AdvantShop;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.SQL;
using AdvantShop.Helpers;
using AdvantShop.Orders;
using Resources;
using AdvantShop.Saas;

namespace Admin
{
    public partial class OrderStatuses : AdvantShopAdminPage
    {
        private bool _inverseSelection;
        private SqlPaging _paging;
        private InSetFieldFilter _selectionFilter;

        protected string SelectedCount()
        {
            if (_inverseSelection)
                return lblFound.Text;
            if (_selectionFilter != null && _selectionFilter.Values != null)
                return _selectionFilter.Values.Length.ToString();
            return "0";
        }

        private void MsgErr(bool clean)
        {
            if (clean)
            {
                Message.Visible = false;
                Message.Text = "";
            }
            else
            {
                Message.Visible = false;
            }
        }

        private void MsgErr(string messageText)
        {
            Message.Visible = true;
            Message.Text = @"<br/>" + messageText;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Message.Visible = false;
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_OrderStatuses_OrderStatuses));

            MsgErr(true);

            grid.ReadOnlyGrid = SaasDataService.IsSaasEnabled && !SaasDataService.CurrentSaasData.OrderStatuses;
			btnAddStatus.Visible = !grid.ReadOnlyGrid;

            if (!IsPostBack)
            {
                _paging = new SqlPaging { TableName = "[Order].[OrderStatus]" };
                _paging.AddFieldsRange(
                    new Field("OrderStatusID as ID"),
                    new Field("StatusName"),
                    new Field("IsDefault"),
                    new Field("IsCanceled"),
                    new Field("IsCompleted"),
                    new Field("CommandID"),
                    new Field("Color"),
                    new Field("Hidden"),
                    new Field("SortOrder") { Sorting = SortDirection.Ascending });
                grid.ChangeHeaderImageUrl("arrowSortOrder", "images/arrowup.gif");


                //pageNumberer.CurrentPageIndex = 1;
                ViewState["Paging"] = _paging;

                if (Request["productid"] != null)
                {
                    _paging.Fields["ProductID"].Filter = new CompareFieldFilter
                        {
                            ParamName = "@ProductID",
                            Expression = Request["productid"]
                        };
                }
            }
            else
            {
                _paging = (SqlPaging)ViewState["Paging"];
                _paging.ItemsPerPage = SQLDataHelper.GetInt(ddRowsPerPage.SelectedValue);

                if (_paging == null)
                {
                    throw new Exception("Paging lost");
                }

                string strIds = Request.Form["SelectedIds"];

                if (!string.IsNullOrEmpty(strIds))
                {
                    List<string> arrids = strIds.Trim().Split(' ').ToList();
                    if (arrids.Contains("-1"))
                    {
                        _inverseSelection = true;
                        arrids.Remove("-1");
                    }
                    int t;
                    _selectionFilter = new InSetFieldFilter
                        {
                            IncludeValues = !_inverseSelection,
                            Values = arrids.Where(id => int.TryParse(id, out t)).ToArray()
                        };
                }
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            btnFilter_Click(sender, e);
            grid.ChangeHeaderImageUrl(null, null);
        }

        protected void grid_Sorting(object sender, GridViewSortEventArgs e)
        {
            var arrows = new Dictionary<string, string>
                {
                    {"CommandID", "arrowCommandID"},
                    {"IsDefault", "arrowIsDefault"},
                    {"IsCanceled", "arrowCanceled"},
                    {"IsCompleted", "arrowCompleted"},
                    {"Hidden", "arrowHidden"},
                    {"StatusName", "arrowStatusName"},
                    {"SortOrder", "arrowSortOrder"}
                };

            const string urlArrowUp = "images/arrowup.gif";
            const string urlArrowDown = "images/arrowdown.gif";
            const string urlArrowGray = "images/arrowdownh.gif";

            var csf = _paging.Fields.Values.First(f => f.Sorting.HasValue);
            var nsf = _paging.Fields[e.SortExpression];

            if (nsf.Name.Equals(csf.Name))
            {
                csf.Sorting = csf.Sorting == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;
                grid.ChangeHeaderImageUrl(arrows[csf.Name],
                                          csf.Sorting == SortDirection.Ascending ? urlArrowUp : urlArrowDown);
            }
            else
            {
                csf.Sorting = null;
                //If Not csf.Name.Contains("SortOrder") Then
                grid.ChangeHeaderImageUrl(arrows[csf.Name], urlArrowGray);
                //End If

                nsf.Sorting = SortDirection.Ascending;
                grid.ChangeHeaderImageUrl(arrows[nsf.Name], urlArrowUp);
            }

            pageNumberer.CurrentPageIndex = 1;
            _paging.CurrentPageIndex = 1;
        }

        protected void grid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ((DropDownList)e.Row.FindControl("ddlCommandID")).SelectedValue =
                    ((DataRowView)e.Row.DataItem)["CommandID"].ToString();
            }
        }

        protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "StatusDelete")
            {
                if (!OrderStatusService.DeleteOrderStatus(SQLDataHelper.GetInt(e.CommandArgument)))
                    MsgErr(Resource.Admin_OrderStatuses_DeleteFailed);
            }
            if (e.CommandName == "Add")
            {
                GridViewRow footer = ((AdvGridView)sender).FooterRow;
                string name = ((TextBox)footer.FindControl("txtStatusNameAdd")).Text;
                string color = ((TextBox)footer.FindControl("txtColorAdd")).Text;
                bool isDefault = ((CheckBox)footer.FindControl("chkIsDefaultAdd")).Checked;
                bool canceled = ((CheckBox)footer.FindControl("chkCanceledAdd")).Checked;
                bool completed = ((CheckBox)footer.FindControl("chkCompletedAdd")).Checked;
                bool hidden = ((CheckBox)footer.FindControl("chkHiddenAdd")).Checked;
                int commandId = SQLDataHelper.GetInt(((DropDownList)footer.FindControl("ddlCommandIDAdd")).SelectedValue);
                int sortOrder = ((TextBox)footer.FindControl("txtAddSortOrder")).Text.TryParseInt();
                if (!string.IsNullOrEmpty(name))
                {
                    OrderStatusService.AddOrderStatus(new OrderStatus
                        {
                            StatusID = 0,
                            StatusName = name,
                            IsDefault = isDefault,
                            Command = (OrderStatusCommand)commandId,
                            IsCanceled = canceled,
                            IsCompleted = completed,
                            Color = color,
                            SortOrder = sortOrder,
                            Hidden = hidden
                        });
                    grid.ShowFooter = false;
                }
            }
            if (e.CommandName == "CancelAdd")
            {
                grid.FooterStyle.BackColor = System.Drawing.Color.FromName("#ccffcc");
                grid.ShowFooter = false;
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (grid.UpdatedRow != null)
            {
                int sortOrder = 0;
                if (Int32.TryParse(grid.UpdatedRow["SortOrder"], out sortOrder))
                {
                    OrderStatusService.UpdateOrderStatus(
                       new OrderStatus
                           {
                               StatusID = SQLDataHelper.GetInt(grid.UpdatedRow["ID"]),
                               StatusName = grid.UpdatedRow["StatusName"],
                               IsDefault = SQLDataHelper.GetBoolean(grid.UpdatedRow["IsDefault"]),
                               Command = (OrderStatusCommand)SQLDataHelper.GetInt(grid.UpdatedRow["CommandID"]),
                               IsCanceled = SQLDataHelper.GetBoolean(grid.UpdatedRow["IsCanceled"]),
                               Hidden = SQLDataHelper.GetBoolean(grid.UpdatedRow["Hidden"]),
                               IsCompleted = SQLDataHelper.GetBoolean(grid.UpdatedRow["IsCompleted"]),
                               Color = grid.UpdatedRow["Color"],
                               SortOrder = sortOrder
                           });
                }
            }


            DataTable data = _paging.PageItems;
            while (data.Rows.Count < 1 & _paging.CurrentPageIndex > 1)
            {
                _paging.CurrentPageIndex -= 1;
                data = _paging.PageItems;
            }

            data.Columns.Add(new DataColumn("IsSelected", typeof(bool)) { DefaultValue = _inverseSelection });
            if (_selectionFilter != null && _selectionFilter.Values != null)
            {
                for (int i = 0; i <= data.Rows.Count - 1; i++)
                {
                    Int32 intIndex = i;
                    if (Array.Exists(_selectionFilter.Values, c => c == data.Rows[intIndex]["ID"].ToString()))
                    {
                        data.Rows[i]["IsSelected"] = !_inverseSelection;
                    }
                }
            }
            if (data.Rows.Count < 1)
            {
                goToPage.Visible = false;
            }
            grid.DataSource = data;
            grid.DataBind();
            pageNumberer.PageCount = _paging.PageCount;
            lblFound.Text = _paging.TotalRowsCount.ToString();
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            //-----Selection filter
            if (ddSelect.SelectedIndex != 0)
            {
                if (ddSelect.SelectedIndex == 2)
                {
                    if (_selectionFilter != null)
                    {
                        _selectionFilter.IncludeValues = !_selectionFilter.IncludeValues;
                    }
                    else
                    {
                        _selectionFilter = null;
                    }
                }
                _paging.Fields["ID"].Filter = _selectionFilter;
            }
            else
            {
                _paging.Fields["ID"].Filter = null;
            }

            //----UserName filter 


            _paging.Fields["StatusName"].Filter = !string.IsNullOrEmpty(txtStatusName.Text)
                                                      ? new CompareFieldFilter { ParamName = "@StatusName", Expression = txtStatusName.Text }
                                                      : null;

            //----CommandID filter

            _paging.Fields["CommandID"].Filter = ddlCommandIDFilter.SelectedValue != "any"
                                                     ? new EqualFieldFilter
                                                         {
                                                             ParamName = "@CommandID",
                                                             Value = ddlCommandIDFilter.SelectedValue
                                                         }
                                                     : null;
            _paging.Fields["IsDefault"].Filter = ddlIsDefaultFilter.SelectedValue != "any"
                                                     ? new EqualFieldFilter
                                                         {
                                                             ParamName = "@IsDefault",
                                                             Value = ddlIsDefaultFilter.SelectedValue
                                                         }
                                                     : null;
            _paging.Fields["IsCanceled"].Filter = ddlCanceledFilter.SelectedValue != "any"
                                                      ? new EqualFieldFilter
                                                          {
                                                              ParamName = "@IsCanceled",
                                                              Value = ddlCanceledFilter.SelectedValue
                                                          }
                                                      : null;

            _paging.Fields["IsCompleted"].Filter = ddlCompletedFilter.SelectedValue != "any"
                                                      ? new EqualFieldFilter
                                                      {
                                                          ParamName = "@IsCompleted",
                                                          Value = ddlCompletedFilter.SelectedValue
                                                      }
                                                      : null;

            _paging.Fields["Hidden"].Filter = ddlHiddenFilter.SelectedValue != "any"
                                                                ? new EqualFieldFilter
                                                                {
                                                                    ParamName = "@Hidden",
                                                                    Value = ddlHiddenFilter.SelectedValue
                                                                }
                                                                : null;

            pageNumberer.CurrentPageIndex = 1;
            _paging.CurrentPageIndex = 1;
        }

        protected void pn_SelectedPageChanged(object sender, EventArgs e)
        {
            _paging.CurrentPageIndex = pageNumberer.CurrentPageIndex;
        }

        protected void lbDeleteSelected_Click(object sender, EventArgs e)
        {
            if ((_selectionFilter != null) && (_selectionFilter.Values != null))
            {
                if (!_inverseSelection)
                {
                    foreach (var id in _selectionFilter.Values)
                    {
                        OrderStatusService.DeleteOrderStatus(SQLDataHelper.GetInt(id));
                    }
                }
                else
                {
                    var itemsIds = _paging.ItemsIds<int>("OrderStatusID as ID");
                    foreach (int id in itemsIds.Where(iId => !_selectionFilter.Values.Contains(iId.ToString())))
                    {
                        OrderStatusService.DeleteOrderStatus(id);
                    }
                }
            }
        }

        protected void ddlCommandIDFilter_Databound(object sender, EventArgs e)
        {
            ddlCommandIDFilter.Items.Insert(0, new ListItem(Resource.Admin_Catalog_Any, "any"));
        }

        protected void btnAddStatus_Click(object sender, EventArgs e)
        {
				grid.ShowFooter = true;
        }
    }
}