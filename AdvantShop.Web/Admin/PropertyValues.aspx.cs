//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using AdvantShop;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.SQL;
using AdvantShop.Helpers;
using Resources;
using AdvantShop.Core.UrlRewriter;

namespace Admin
{
    public partial class PropertyValues : AdvantShopAdminPage
    {
        #region Fields

        private bool _inverseSelection;
        private SqlPaging _paging;
        private InSetFieldFilter _selectionFilter;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            lblError.Visible = false;
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_Properties_ListPropreties));

            if (!IsPostBack)
            {
                _paging = new SqlPaging
                    {
                        TableName = "[Catalog].[PropertyValue]",
                        ItemsPerPage = 50,
                        CurrentPageIndex = 1
                    };

                _paging.AddFieldsRange(new[]
                    {
                        new Field {Name = "PropertyValueID as ID"},
                        new Field {Name = "PropertyID"},
                        new Field {Name = "Value"},
                        new Field {Name = "SortOrder", Sorting = SortDirection.Ascending},
                        new Field {Name = "(Select Count(ProductID) from Catalog.ProductPropertyValue Where propertyValueid=[PropertyValue].propertyValueid) as ProductsCount"},
                    });

                if (!String.IsNullOrEmpty(Request["propertyid"]))
                {
                    Property property = PropertyService.GetPropertyById(Request["propertyid"].TryParseInt());
                    if (property != null)
                    {
                        lnkBack.NavigateUrl = UrlService.GetAdminAbsoluteLink("properties.aspx" + (property.GroupId != null ? "?groupId=" + property.GroupId : string.Empty));

                        lblHead.Text += string.Format(" - \"{0}\"", property.Name);

                        _paging.Fields["PropertyID"].Filter = new EqualFieldFilter
                        {
                            ParamName = "@PropertyID",
                            Value = property.PropertyId.ToString()
                        };
                    }
                    else
                        Response.Redirect("Properties.aspx");
                }
                else
                    Response.Redirect("Properties.aspx");

                grid.ChangeHeaderImageUrl("arrowSortOrder", "images/arrowup.gif");
                pageNumberer.CurrentPageIndex = 1;
                ViewState["Paging"] = _paging;
            }
            else
            {
                _paging = (SqlPaging)(ViewState["Paging"]);
                _paging.ItemsPerPage = SQLDataHelper.GetInt(ddRowsPerPage.SelectedValue);

                if (_paging == null)
                    throw (new Exception("Paging lost"));

                string strIds = Request.Form["SelectedIds"];
                if (!string.IsNullOrEmpty(strIds))
                {
                    strIds = strIds.Trim();

                    var arrids = strIds.Split(' ');
                    var ids = new string[arrids.Length];

                    _selectionFilter = new InSetFieldFilter { IncludeValues = true };
                    for (int idx = 0; idx <= ids.Length - 1; idx++)
                    {
                        int t = int.Parse(arrids[idx]);
                        if (t != -1)
                        {
                            ids[idx] = t.ToString(CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            _selectionFilter.IncludeValues = false;
                            _inverseSelection = true;
                        }
                    }
                    _selectionFilter.Values = ids;
                }
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (grid.UpdatedRow != null)
            {
                int sortOrder = 0;
                int.TryParse(grid.UpdatedRow["SortOrder"], out sortOrder);

                PropertyService.UpdatePropertyValue(new PropertyValue
                {
                    PropertyValueId = SQLDataHelper.GetInt(grid.UpdatedRow["ID"]),
                    Value = grid.UpdatedRow["Value"],
                    PropertyId = SQLDataHelper.GetInt(grid.UpdatedRow["PropertyID"]),
                    SortOrder = sortOrder
                });
            }

            DataTable data = _paging.PageItems;
            while (data.Rows.Count < 1 && _paging.CurrentPageIndex > 1)
            {
                _paging.CurrentPageIndex--;
                data = _paging.PageItems;
            }

            var clmn = new DataColumn("IsSelected", typeof(bool)) { DefaultValue = _inverseSelection };
            data.Columns.Add(clmn);
            if ((_selectionFilter != null) && (_selectionFilter.Values != null))
            {
                for (int i = 0; i <= data.Rows.Count - 1; i++)
                {
                    int intIndex = i;
                    if (Array.Exists(_selectionFilter.Values, c => c == (data.Rows[intIndex]["ID"]).ToString()))
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
            lblFound.Text = _paging.TotalRowsCount.ToString(CultureInfo.InvariantCulture);
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            // ---- PropertyID filter
            if (!String.IsNullOrEmpty(Request["propertyid"]))
            {
                int propertyID = 0;
                Property prop = null;
                if (int.TryParse(Request["propertyid"], out propertyID))
                {
                    prop = PropertyService.GetPropertyById(propertyID);
                }
                if (prop != null)
                {
                    var ef = new EqualFieldFilter
                        {
                            ParamName = "@PropertyID",
                            Value = propertyID.ToString(CultureInfo.InvariantCulture)
                        };
                    _paging.Fields["PropertyID"].Filter = ef;
                }
                else
                {
                    Response.Redirect("Properties.aspx");
                }
            }

            //-----Selection filter
            if (String.CompareOrdinal(ddSelect.SelectedIndex.ToString(CultureInfo.InvariantCulture), "0") != 0)
            {
                if (String.CompareOrdinal(ddSelect.SelectedIndex.ToString(CultureInfo.InvariantCulture), "2") == 0 && _selectionFilter != null)
                {
                    _selectionFilter.IncludeValues = !_selectionFilter.IncludeValues;
                }
                _paging.Fields["ID"].Filter = _selectionFilter;
            }
            else
            {
                _paging.Fields["ID"].Filter = null;
            }

            //----Name filter
            _paging.Fields["Value"].Filter = string.IsNullOrEmpty(txtValue.Text)
                                                 ? null
                                                 : new CompareFieldFilter
                                                     {
                                                         Expression = txtValue.Text,
                                                         ParamName = "@Name"
                                                     };


            //----SortOrder filter
            _paging.Fields["SortOrder"].Filter = string.IsNullOrEmpty(txtSortOrder.Text)
                                                     ? null
                                                     : new EqualFieldFilter
                                                         {
                                                             ParamName = "@SortOrder",
                                                             Value = txtSortOrder.Text
                                                         };

            _paging.Fields["ProductsCount"].Filter = string.IsNullOrEmpty(txtProductsCount.Text)
                                                         ? null
                                                         : new EqualFieldFilter
                                                             {
                                                                 ParamName = "@ProductsCount",
                                                                 Value = txtProductsCount.Text
                                                             };

            pageNumberer.CurrentPageIndex = 1;
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            btnFilter_Click(sender, e);
            grid.ChangeHeaderImageUrl(null, null);
        }

        protected void pn_SelectedPageChanged(object sender, EventArgs e)
        {
            _paging.CurrentPageIndex = pageNumberer.CurrentPageIndex;
        }

        protected void linkGO_Click(object sender, EventArgs e)
        {
            int pagen;
            try
            {
                pagen = int.Parse(txtPageNum.Text);
            }
            catch (Exception)
            {
                pagen = -1;
            }
            if (pagen >= 1 && pagen <= _paging.PageCount)
            {
                pageNumberer.CurrentPageIndex = pagen;
                _paging.CurrentPageIndex = pagen;
            }
        }

        protected void lbDeleteSelected_Click(object sender, EventArgs e)
        {
            if ((_selectionFilter != null) && (_selectionFilter.Values != null))
            {
                if (!_inverseSelection)
                {
                    foreach (var id in _selectionFilter.Values)
                    {
                        PropertyService.DeletePropertyValueById(SQLDataHelper.GetInt(id));
                    }
                }
                else
                {
                    var itemsIds = _paging.ItemsIds<int>("PropertyValueID as ID");
                    foreach (var id in itemsIds.Where(id => !_selectionFilter.Values.Contains(id.ToString(CultureInfo.InvariantCulture))))
                    {
                        PropertyService.DeletePropertyValueById(id);
                    }
                }
            }
        }


        protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeletePropertyValue")
            {
                PropertyService.DeletePropertyValueById(SQLDataHelper.GetInt(e.CommandArgument));
            }

            if (e.CommandName == "AddPropertyValue")
            {

                GridViewRow footer = grid.FooterRow;
                if (string.IsNullOrEmpty(((TextBox)footer.FindControl("txtNewValue")).Text))
                {
                    grid.FooterStyle.BackColor = System.Drawing.Color.FromName("#ffcccc");
                    return;
                }

                var property = PropertyService.GetPropertyById(SQLDataHelper.GetInt(Request["propertyid"]));
                if (property.Type == (int)PropertyType.Range)
                {
                    var value = ((TextBox)footer.FindControl("txtNewValue")).Text.Trim();

                    if (value != "0" && value.TryParseFloat() == 0)
                    {
                        MsgErr(Resource.Admin_PropertyValues_NotValidNumber);
                        return;
                    }
                }

                if (PropertyService.AddPropertyValue(new PropertyValue
                    {
                        PropertyId = SQLDataHelper.GetInt(Request["propertyid"]),
                        Value = ((TextBox)footer.FindControl("txtNewValue")).Text.Trim(),
                        SortOrder = ((TextBox)footer.FindControl("txtNewSortOrder")).Text.TryParseInt()
                    })
                    != 0)
                    grid.ShowFooter = false;
            }


            if (e.CommandName == "CancelAdd")
            {
                grid.FooterStyle.BackColor = System.Drawing.Color.FromName("#ccffcc");
                grid.ShowFooter = false;
            }
        }

        protected void grid_Sorting(object sender, GridViewSortEventArgs e)
        {
            var arrows = new Dictionary<string, string>
                {
                    {"Value", "arrowValue"},
                    {"SortOrder", "arrowSortOrder"},
                    {"ProductsCount", "arrowProductsCount"}
                };
            const string urlArrowUp = "images/arrowup.gif";
            const string urlArrowDown = "images/arrowdown.gif";
            const string urlArrowGray = "images/arrowdownh.gif";


            Field csf = (from Field f in _paging.Fields.Values where f.Sorting.HasValue select f).First();
            Field nsf = _paging.Fields[e.SortExpression];

            if (nsf.Name.Equals(csf.Name))
            {
                csf.Sorting = csf.Sorting == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;
                grid.ChangeHeaderImageUrl(arrows[csf.Name],
                                          (csf.Sorting == SortDirection.Ascending ? urlArrowUp : urlArrowDown));
            }
            else
            {
                csf.Sorting = null;
                grid.ChangeHeaderImageUrl(arrows[csf.Name], urlArrowGray);

                nsf.Sorting = SortDirection.Ascending;
                grid.ChangeHeaderImageUrl(arrows[nsf.Name], urlArrowUp);
            }


            pageNumberer.CurrentPageIndex = 1;
            _paging.CurrentPageIndex = 1;
        }

        private void grid_DataBound(object sender, EventArgs e)
        {
            if (grid.ShowFooter)
            {
                grid.FooterRow.FindControl("txtNewValue").Focus();
            }
        }

        protected void btnAddProperty_Click(object sender, EventArgs e)
        {
            grid.ShowFooter = true;
            grid.FooterStyle.BackColor = System.Drawing.Color.FromName("#ccffcc");
            grid.DataBound += grid_DataBound;
        }

        private void MsgErr(string messageText)
        {
            lblError.Visible = true;
            lblError.Text = @"<br/>" + messageText;
        }
    }
}