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
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using Resources;

namespace Admin
{
    public partial class Colors : AdvantShopAdminPage
    {
        #region private

        private bool _inverseSelection;
        private SqlPaging _paging;
        private InSetFieldFilter _selectionFilter;

        private void MsgError(string error)
        {
            lblError.Visible = true;
            lblError.Text = error;
            upErrors.Update();
        }

        private void MsgError()
        {
            lblError.Visible = false;
            lblError.Text = "";
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_ColorsDictionary_Header));

            MsgError();

            if (!IsPostBack)
            {
                _paging = new SqlPaging
                {
                    TableName = "[Catalog].[Color] left join Catalog.Photo on Photo.ObjId=Color.ColorID and type='color'",
                    ItemsPerPage = 20,
                    CurrentPageIndex = 1
                };

                _paging.AddFieldsRange(new[]
                    {
                        new Field {Name = "Color.ColorID as ID", IsDistinct = true},
                        new Field {Name = "ColorName"},
                        new Field {Name = "ColorCode"},
                        new Field {Name = "PhotoName"},
                        new Field {Name = "SortOrder", Sorting = SortDirection.Ascending}
                    });
                grid.ChangeHeaderImageUrl("arrowSortOrder", "images/arrowup.gif");
                pageNumberer.CurrentPageIndex = 1;
                ViewState["Paging"] = _paging;
            }
            else
            {
                _paging = (SqlPaging)(ViewState["Paging"]);
                _paging.ItemsPerPage = SQLDataHelper.GetInt(ddRowsPerPage.SelectedValue);

                if (_paging == null)
                {
                    throw (new Exception("Paging lost"));
                }

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

        protected void btnFilter_Click(object sender, EventArgs e)
        {
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
            _paging.Fields["ColorName"].Filter = string.IsNullOrEmpty(txtNameFilter.Text)
                                                     ? null
                                                     : new CompareFieldFilter
                                                     {
                                                         Expression = txtNameFilter.Text,
                                                         ParamName = "@ColorName"
                                                     };

            //----Code filter
            _paging.Fields["ColorCode"].Filter = string.IsNullOrEmpty(txtColorCodeFilter.Text)
                                                     ? null
                                                     : new CompareFieldFilter
                                                     {
                                                         Expression = txtColorCodeFilter.Text,
                                                         ParamName = "@ColorCode"
                                                     };

            //----SortOrder filter
            _paging.Fields["SortOrder"].Filter = string.IsNullOrEmpty(txtSortOrderFilter.Text)
                                                     ? null
                                                     : new CompareFieldFilter
                                                     {
                                                         ParamName = "@SortOrder",
                                                         Expression = txtSortOrderFilter.Text
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
                        int colorId = SQLDataHelper.GetInt(id);

                        if (!ColorService.IsColorUsed(colorId))
                            ColorService.DeleteColor(colorId);
                        else
                            MsgError(Resource.Admin_ColorsDictionary_CantDeleteColors);
                    }
                }
                else
                {
                    var itemsIds = _paging.ItemsIds<int>("Color.ColorID as ID");
                    foreach (var id in itemsIds.Where(id => !_selectionFilter.Values.Contains(id.ToString())))
                    {
                        if (!ColorService.IsColorUsed(id))
                            ColorService.DeleteColor(id);
                        else
                            MsgError(Resource.Admin_ColorsDictionary_CantDeleteColors);
                    }
                }
            }
        }

        protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteColor")
            {
                int colorId = SQLDataHelper.GetInt(e.CommandArgument);

                if (!ColorService.IsColorUsed(colorId))
                    ColorService.DeleteColor(colorId);
                else
                    MsgError(string.Format(Resource.Admin_ColorsDictionary_CantDelete, colorId));
            }

            if (e.CommandName == "AddColor")
            {
                try
                {
                    GridViewRow footer = grid.FooterRow;
                    int temp;

                    var pic = (FileUpload)footer.FindControl("fuColorPicrure");
                    var a = pic;


                    int.TryParse(((TextBox)footer.FindControl("txtNewSortOrder")).Text, out temp);
                    if (string.IsNullOrEmpty(((TextBox)footer.FindControl("txtNewName")).Text))
                    {
                        grid.FooterStyle.BackColor = System.Drawing.Color.FromName("#ffcccc");
                        return;
                    }
                    if (!IsValidColorCode(((TextBox)footer.FindControl("txtNewColorCode")).Text))
                    {
                        grid.FooterStyle.BackColor = System.Drawing.Color.FromName("#ffcccc");
                        return;
                    }

                    if (ColorService.AddColor(new Color
                    {
                        ColorName = ((TextBox)footer.FindControl("txtNewName")).Text.Trim(),
                        ColorCode = ((TextBox)footer.FindControl("txtNewColorCode")).Text.Trim(),
                        SortOrder = temp
                    })
                        != 0)
                        grid.ShowFooter = false;
                }
                catch (Exception ex)
                {
                    Debug.Log.Error(ex);
                }
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
                    {"ColorName", "arrowName"},
                    {"ColorCode", "arrowColorCode"},
                    {"SortOrder", "arrowSortOrder"},
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

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (grid.UpdatedRow != null)
            {
                if (!string.IsNullOrEmpty(grid.UpdatedRow["ColorName"]) && IsValidColorCode(grid.UpdatedRow["ColorCode"]))
                {
                    ColorService.UpdateColor(new Color
                    {
                        ColorId = SQLDataHelper.GetInt(grid.UpdatedRow["ID"]),
                        ColorName = grid.UpdatedRow["ColorName"],
                        ColorCode = grid.UpdatedRow["ColorCode"],
                        SortOrder = grid.UpdatedRow["SortOrder"].TryParseInt()
                    });
                }
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

        protected void grid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
        }

        private void grid_DataBound(object sender, EventArgs e)
        {
            if (grid.ShowFooter)
            {
                grid.FooterRow.FindControl("txtNewName").Focus();
            }
        }

        protected void btnAddColor_Click(object sender, EventArgs e)
        {
            grid.ShowFooter = true;
            grid.FooterStyle.BackColor = System.Drawing.Color.FromName("#ccffcc");
            grid.DataBound += grid_DataBound;
        }

        private static bool IsValidColorCode(string code)
        {

            if (string.IsNullOrEmpty(code))
                return false;

            //remove '#'
            code = code.Replace("#", "");

            if (code.Length != 6)
                return false;

            uint tmp;
            if (!uint.TryParse(code, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out tmp))
                return false;
            // ffffff - 16777215
            if (tmp > 16777215)
                return false;

            return true;
        }

        protected bool CanDelete(int colorId)
        {
            return !ColorService.IsColorUsed(colorId);
        }

    }
}