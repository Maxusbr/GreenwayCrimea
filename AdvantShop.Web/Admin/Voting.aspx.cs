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
using AdvantShop.CMS;
using AdvantShop.Configuration;
using AdvantShop.Core.Controls;
using AdvantShop.Core.SQL;
using AdvantShop.Helpers;
using Resources;

namespace Admin
{
    public partial class Voting : AdvantShopAdminPage
    {
        private bool _inverseSelection;
        private SqlPaging _paging;
        private InSetFieldFilter _selectionFilter;

        protected void Page_Load(object sender, EventArgs e)
        {
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_Voting_Voting));

            if (!IsPostBack)
            {
                _paging = new SqlPaging
                    {
                        TableName = "[Voice].[VoiceTheme]",
                        ItemsPerPage = 10
                    };

                _paging.AddFieldsRange(new List<Field>
                    {
                        new Field {Name = "VoiceThemeID as ID", IsDistinct = true},
                        new Field {Name = "PsyID"},
                        new Field {Name = "Name", Sorting = SortDirection.Ascending},
                        new Field {Name = "IsDefault"},
                        new Field {Name = "IsHaveNullVoice"},
                        new Field {Name = "IsClose"},
                        new Field {Name = "DateAdded"},
                        new Field {Name = "DateModify"},
                        new Field {Name= "(SELECT COUNT(*) FROM [Voice].[Answer] WHERE [FKIDTheme] = VoiceThemeID) as AnswerCount" }
                    });

                grid.ChangeHeaderImageUrl("arrowName", "images/arrowup.gif");

                _paging.ItemsPerPage = 10;

                pageNumberer.CurrentPageIndex = 1;
                _paging.CurrentPageIndex = 1;
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

                    _selectionFilter = new InSetFieldFilter();
                    if (arrids.Contains("-1"))
                    {
                        _selectionFilter.IncludeValues = false;
                        _inverseSelection = true;
                    }
                    else
                    {
                        _selectionFilter.IncludeValues = true;
                    }
                    _selectionFilter.Values = arrids.Where(id => id != "-1").ToArray();
                }
            }
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
                }
                _paging.Fields["ID"].Filter = _selectionFilter;
            }
            else
            {
                _paging.Fields["ID"].Filter = null;
            }

            _paging.Fields["Name"].Filter = !string.IsNullOrEmpty(txtName.Text)
                                                ? new CompareFieldFilter { Expression = txtName.Text, ParamName = "@Name" }
                                                : null;


            _paging.Fields["IsDefault"].Filter = (ddlIsDefault.SelectedValue != "any")
                                                     ? new EqualFieldFilter
                                                         {
                                                             ParamName = "@IsDefault",
                                                             Value = ddlIsDefault.SelectedValue
                                                         }
                                                     : null;
            _paging.Fields["IsHaveNullVoice"].Filter = (ddlIsHaveNullVoice.SelectedValue != "any")
                                                           ? new EqualFieldFilter
                                                               {
                                                                   ParamName = "@IsHaveNullVoice",
                                                                   Value = ddlIsHaveNullVoice.SelectedValue
                                                               }
                                                           : null;
            _paging.Fields["IsClose"].Filter = (ddlIsClose.SelectedValue != "any")
                                                   ? new EqualFieldFilter
                                                       {
                                                           ParamName = "@IsClose",
                                                           Value = ddlIsClose.SelectedValue
                                                       }
                                                   : null;
            //TODO date filter

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
                        VoiceService.DeleteTheme(SQLDataHelper.GetInt(id));
                    }
                }
                else
                {
                    var itemsIds = _paging.ItemsIds<int>("VoiceThemeID as ID");
                    foreach (int id in itemsIds.Where(id => !_selectionFilter.Values.Contains(id.ToString())))
                    {
                        VoiceService.DeleteTheme(id);
                    }
                }
            }
        }

        protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteTheme")
            {
                VoiceService.DeleteTheme(SQLDataHelper.GetInt(e.CommandArgument));
            }

            if (e.CommandName == "AddTheme")
            {
                //TODO validate input
                try
                {
                    VoiceService.AddTheme(new VoiceTheme
                        {
                            Name = ((TextBox)grid.FooterRow.FindControl("txtNewName")).Text,
                            IsDefault =
                                ((CheckBox)grid.FooterRow.FindControl("chkNewIsDefault")).Checked,
                            IsHaveNullVoice =
                                ((CheckBox)grid.FooterRow.FindControl("chkNewIsHaveNullVoice")).
                                              Checked,
                            IsClose =
                                ((CheckBox)grid.FooterRow.FindControl("chkNewIsClose")).Checked
                        });
                    grid.ShowFooter = false;
                }
                catch (Exception ex)
                {
                    MsgErr(ex.Message);
                }
            }


            if (e.CommandName == "CancelAdd")
            {
                grid.ShowFooter = false;
            }
        }

        protected void grid_Sorting(object sender, GridViewSortEventArgs e)
        {
            var arrows = new Dictionary<string, string>
                {
                    {"PsyID", "arrowPsyID"},
                    {"Name", "arrowName"},
                    {"IsDefault", "arrowIsDefault"},
                    {"IsHaveNullVoice", "arrowIsHaveNullVoice"},
                    {"IsClose", "arrowIsClose"},
                    {"DateAdded", "arrowDateAdded"},
                    {"DateModify", "arrowDateModify"},
                    {"AnswerCount", "arrowAnswerCount"}
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

        private void MsgErr(string text)
        {
            lblError.Visible = true;
            lblError.Text = text;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (grid.UpdatedRow != null)
            {
                //TODO fields validation
                try
                {
                    VoiceService.UpdateTheme(new VoiceTheme
                        {
                            VoiceThemeId = int.Parse(grid.UpdatedRow["ID"]),
                            Name = grid.UpdatedRow["Name"],
                            IsDefault = SQLDataHelper.GetBoolean(grid.UpdatedRow["IsDefault"]),
                            IsClose = SQLDataHelper.GetBoolean(grid.UpdatedRow["IsClose"]),
                            IsHaveNullVoice =
                                SQLDataHelper.GetBoolean(grid.UpdatedRow["IsHaveNullVoice"])
                        });
                }
                catch (Exception ex)
                {
                    MsgErr(ex.Message);
                }
            }

            DataTable data = _paging.PageItems;
            while (data.Rows.Count < 1 && _paging.CurrentPageIndex > 1)
            {
                _paging.CurrentPageIndex--;
                data = _paging.PageItems;
            }

            data.Columns.Add(new DataColumn("IsSelected", typeof(bool)) { DefaultValue = _inverseSelection });
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

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            grid.ShowFooter = true;
        }

        protected void sds_Init(object sender, EventArgs e)
        {
            ((SqlDataSource)sender).ConnectionString = Connection.GetConnectionString();
        }
    }
}