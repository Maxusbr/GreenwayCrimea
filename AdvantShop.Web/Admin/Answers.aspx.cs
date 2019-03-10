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
using AdvantShop.CMS;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.SQL;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;

namespace Admin
{
    public partial class Answers : AdvantShopAdminPage
    {
        private bool _inverseSelection;
        private SqlPaging _paging;

        private InSetFieldFilter _selectionFilter;

        private int ThemeId
        {
            get { return Request["theme"].TryParseInt(); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["theme"] == null)
            {
                Response.Redirect("Voting.aspx");
            }
            lblHead.Text = VoiceService.GetVotingName(ThemeId);
            SetMeta(string.Format("{0} - {1} - {2}", SettingsMain.ShopName, lblHead.Text, lblSubHead.Text));

            if (!IsPostBack)
            {
                _paging = new SqlPaging
                    {
                        TableName = "[Voice].[Answer]",
                        ItemsPerPage = 10
                    };

                _paging.AddFieldsRange(new List<Field>
                    {
                        new Field {Name = "AnswerID as ID", IsDistinct = true},
                        new Field {Name = "Name"},
                        new Field {Name = "CountVoice"},
                        new Field {Name = "Sort", Sorting = SortDirection.Ascending},
                        new Field {Name = "IsVisible"},
                        new Field {Name = "DateAdded"},
                        new Field {Name = "DateModify"},
                        new Field
                            {
                                Name = "FKIDTheme",
                                Filter = new EqualFieldFilter {ParamName = "@Theme", Value = ThemeId.ToString()}
                            }
                    });

                grid.ChangeHeaderImageUrl("arrowSort", "images/arrowup.gif");

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
                    string[] arrids = strIds.Split(' ');

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

            //----Name filter

            _paging.Fields["Name"].Filter = !string.IsNullOrEmpty(txtName.Text)
                                                ? new CompareFieldFilter { Expression = txtName.Text, ParamName = "@Name" }
                                                : null;
            _paging.Fields["CountVoice"].Filter = !string.IsNullOrEmpty(txtCountVoice.Text)
                                                      ? new CompareFieldFilter { Expression = txtCountVoice.Text, ParamName = "@CountVoice" }
                                                      : null;
            _paging.Fields["Sort"].Filter = !string.IsNullOrEmpty(txtSort.Text)
                                                ? new CompareFieldFilter { Expression = txtSort.Text, ParamName = "@Sort" }
                                                : null;

            _paging.Fields["IsVisible"].Filter = (ddlIsVisible.SelectedValue != "any")
                                                     ? new EqualFieldFilter
                                                         {
                                                             ParamName = "@IsVisible",
                                                             Value = ddlIsVisible.SelectedValue
                                                         }
                                                     : null;

            //TODO date filter

            pageNumberer.CurrentPageIndex = 1;
            _paging.CurrentPageIndex = 1;
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
                        VoiceService.DeleteAnswer(SQLDataHelper.GetInt(id));
                    }
                }
                else
                {
                    var itemsIds = _paging.ItemsIds<int>("AnswerID as ID");
                    foreach (int id in itemsIds.Where(id => !_selectionFilter.Values.Contains(id.ToString())))
                    {
                        VoiceService.DeleteAnswer(id);
                    }
                }
            }
        }

        protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteAnswer")
            {
                VoiceService.DeleteAnswer(SQLDataHelper.GetInt(e.CommandArgument));
            }

            if (e.CommandName == "AddAnswer")
            {
                try
                {
                    var answer = new Answer
                        {
                            Name = ((TextBox)grid.FooterRow.FindControl("txtNewName")).Text,
                            CountVoice = ((TextBox)grid.FooterRow.FindControl("txtNewCountVoice")).Text.TryParseInt(),
                            Sort = ((TextBox)grid.FooterRow.FindControl("txtNewSort")).Text.TryParseInt(),
                            IsVisible = ((CheckBox)grid.FooterRow.FindControl("chkNewIsVisible")).Checked,
                            FkidTheme = ThemeId
                        };

                    VoiceService.InsertAnswer(answer);
                }
                catch (Exception ex)
                {
                    Debug.Log.Error(ex);
                }
                grid.ShowFooter = false;
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
                    {"CountVoice", "arrowCountVoice"},
                    {"Name", "arrowName"},
                    {"Sort", "arrowSort"},
                    {"IsVisible", "arrowIsVisible"},
                    {"DateAdded", "arrowDateAdded"},
                    {"DateModify", "arrowDateModify"},
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
                try
                {
                    int countVoices = 0;
                    int sort = 0;
                    if (Int32.TryParse(grid.UpdatedRow["CountVoice"], out countVoices) && Int32.TryParse(grid.UpdatedRow["Sort"], out sort))
                    {
                        var answer = new Answer
                            {
                                AnswerId = SQLDataHelper.GetInt(grid.UpdatedRow["ID"]),
                                Name = grid.UpdatedRow["Name"],
                                IsVisible = SQLDataHelper.GetBoolean(grid.UpdatedRow["IsVisible"]),
                                FkidTheme = ThemeId,
                                CountVoice = countVoices,
                                Sort = sort
                            };
                        VoiceService.UpdateAnswer(answer);
                    }

                }
                catch (Exception ex)
                {
                    Debug.Log.Error(ex);
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
            lblFound.Text = _paging.TotalRowsCount.ToString();
        }

        protected void grid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
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