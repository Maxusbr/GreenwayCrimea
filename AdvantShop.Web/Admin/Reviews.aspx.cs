//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.SQL;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Payment;
using Resources;
using AdvantShop.CMS;
using System.IO;
using System.Web;
using AdvantShop.Diagnostics;

namespace Admin
{
    partial class Reviews : AdvantShopAdminPage
    {
        private bool _inverseSelection;
        private SqlPaging _paging;
        private InSetFieldFilter _selectionFilter;

        private Review _review;

        private void MsgErr(string message)
        {
            pnlErrors.InnerHtml += string.Format("<div>{0}</div>", message);
        }

        private void ResetMsgErr()
        {
            pnlErrors.InnerHtml = string.Empty;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_Reviews_Reviews));
            ResetMsgErr();

            if (!IsPostBack)
            {
                _review = null;

                _paging = new SqlPaging
                {
                    TableName = "[CMS].[Review] LEFT JOIN [Catalog].[Photo] ON Photo.ObjId = Review.ReviewId AND Photo.Type = @PhotoType AND Main = 1 " +
                                    "LEFT JOIN Catalog.Product ON Product.ProductId = Review.EntityId AND Review.Type = @EntityType", // for filter
                    ItemsPerPage = 10
                };

                _paging.AddFieldsRange(
                    new Field { Name = "ReviewId as ID", IsDistinct = true },
                    new Field { Name = "PhotoName" },
                    new Field { Name = "Review.Name as Name" },
                    new Field { Name = "Email" },
                    new Field { Name = "AddDate", Sorting = SortDirection.Descending },
                    new Field { Name = "Checked" },
                    new Field { Name = "[Text]" },
                    new Field { Name = "Review.Type" },
                    new Field { Name = "EntityId" },
                    new Field { Name = "ProductId" },
                    new Field { Name = "Product.Name as ProductName" },
                    new Field { Name = "ArtNo" }
                    );

                _paging.AddParam(new SqlParam { ParameterName = "@PhotoType", Value = PhotoType.Review.ToString() });
                _paging.AddParam(new SqlParam { ParameterName = "@EntityType", Value = EntityType.Product });

                grid.ChangeHeaderImageUrl("arrowAddDate", "images/arrowdown.gif");

                pageNumberer.CurrentPageIndex = 1;
                _paging.CurrentPageIndex = 1;
                ViewState["Paging"] = _paging;
                if (Request["ReviewId"] != null)
                {
                    _paging.Fields["ID"].Filter = new EqualFieldFilter
                    {
                        ParamName = "@ReviewId",
                        Value = Request["ReviewId"]
                    };
                }
                if (Request["ArtNo"].IsNotEmpty())
                {
                    txtArtNo.Text = HttpUtility.UrlDecode(Request["ArtNo"]);
                    btnFilter_Click(sender, e);
                }
                if (Request["AddReview"].IsNotEmpty())
                {
                    btnAddReview_Click(sender, e);
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

                var strIds = Request.Form["SelectedIds"];

                if (!string.IsNullOrEmpty(strIds))
                {
                    List<string> arrids = strIds.Trim().Split(' ').ToList();

                    _selectionFilter = new InSetFieldFilter();
                    if (arrids.Contains("-1"))
                    {
                        _selectionFilter.IncludeValues = false;
                        _inverseSelection = true;
                        arrids.Remove("-1");
                    }
                    else
                    {
                        _selectionFilter.IncludeValues = true;
                    }
                    _selectionFilter.Values = arrids.ToArray();
                }
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (grid.UpdatedRow != null)
            {

                var comment = ReviewService.GetReview(SQLDataHelper.GetInt(grid.UpdatedRow["Id"]));

                if (comment != null && ValidationHelper.IsValidEmail(grid.UpdatedRow["Email"]))
                {
                    comment.Name = grid.UpdatedRow["Name"];
                    comment.Email = grid.UpdatedRow["Email"];
                    comment.Text = grid.UpdatedRow["[Text]"];
                    comment.Checked = SQLDataHelper.GetBoolean(grid.UpdatedRow["Checked"]);
                    comment.AddDate = grid.UpdatedRow["AddDate"].TryParseDateTime();
                    ReviewService.UpdateReview(comment);
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
                    if (Array.Exists(_selectionFilter.Values, (string c) => c == data.Rows[intIndex]["ID"].ToString()))
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

            InitFooter();
        }

        protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "DeleteReview":
                    ReviewService.DeleteReview(SQLDataHelper.GetInt(e.CommandArgument));
                    break;
                case "AddReview":
                    try
                    {
                        GridViewRow footer = grid.FooterRow;

                        bool isValid = true;

                        var artNo = ((TextBox)footer.FindControl("txtNewArtNo")).Text.Trim();
                        Product product = null;
                        if (artNo.IsNullOrEmpty() || (product = ProductService.GetProduct(artNo)) == null)
                        {
                            isValid = false;
                            MsgErr(Resource.Admin_Reviews_Error_EnterArtNo);
                        }

                        _review = new Review
                        {
                            ParentId = 0,
                            CustomerId = AdvantShop.Customers.CustomerContext.CustomerId,
                            Name = HttpUtility.HtmlEncode(((TextBox)footer.FindControl("txtNewName")).Text.Trim()),
                            Text = HttpUtility.HtmlEncode(((TextBox)footer.FindControl("txtNewText")).Text.Trim()),
                            Checked = ((CheckBox)footer.FindControl("cbNewChecked")).Checked,
                            Type = EntityType.Product,
                            Email = ((TextBox)footer.FindControl("txtNewEmail")).Text.Trim(),
                            AddDate = ((TextBox)footer.FindControl("txtNewAddDate")).Text.Trim().TryParseDateTime(DateTime.Now),
                            Ip = Request.UserHostAddress != "::1" && Request.UserHostAddress != "127.0.0.1" ? Request.UserHostAddress : "local"
                        };

                        if (_review.Email.IsNullOrEmpty() || !ValidationHelper.IsValidEmail(_review.Email))
                        {
                            MsgErr(Resource.Admin_Reviews_Error_InvalidEmail);
                            isValid = false;
                        }

                        isValid &= _review.Name.IsNotEmpty();
                        isValid &= _review.Text.IsNotEmpty();

                        if (!isValid)
                        {
                            MsgErr(Resource.Admin_Reviews_Error_EnterFields);
                            grid.FooterStyle.BackColor = System.Drawing.Color.FromName("#ffcccc");
                            return;
                        }
                        _review.EntityId = product.ProductId;
                        ReviewService.AddReview(_review);

                        if (_review.ReviewId != 0)
                            grid.ShowFooter = false;
                    }
                    catch (Exception ex)
                    {
                        Debug.Log.Error(ex);
                    }
                    break;
                case "CancelAdd":
                    _review = null;
                    grid.FooterStyle.BackColor = System.Drawing.Color.FromName("#ccffcc");
                    grid.ShowFooter = false;
                    break;
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            btnFilter_Click(sender, e);
            grid.ChangeHeaderImageUrl(null, null);

            if (Request["ArtNo"].IsNotEmpty())
                Response.Redirect("Reviews.aspx");
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            //-----Selection filter
            if (ddSelect.SelectedIndex != 0)
            {
                if (ddSelect.SelectedIndex == 2)
                {
                    if (_selectionFilter != null)
                        _selectionFilter.IncludeValues = !_selectionFilter.IncludeValues;
                    else
                        _selectionFilter = null;
                }
                _paging.Fields["ID"].Filter = _selectionFilter;
            }
            else
                _paging.Fields["ID"].Filter = null;


            _paging.Fields["Email"].Filter = !string.IsNullOrEmpty(txtEmail.Text)
                ? new CompareFieldFilter { ParamName = "@Email", Expression = txtEmail.Text }
                : null;

            _paging.Fields["Name"].Filter = !string.IsNullOrEmpty(txtName.Text)
                ? new CompareFieldFilter { ParamName = "@Name", Expression = txtName.Text }
                : null;

            _paging.Fields["[Text]"].Filter = !string.IsNullOrEmpty(txtText.Text)
                ? new CompareFieldFilter { ParamName = "@Text", Expression = txtText.Text }
                : null;

            _paging.Fields["ProductName"].Filter = !string.IsNullOrEmpty(txtEntityName.Text)
                ? new CompareFieldFilter { ParamName = "@ProductName", Expression = txtEntityName.Text }
                : null;

            _paging.Fields["ArtNo"].Filter = !string.IsNullOrEmpty(txtArtNo.Text)
                ? new EqualFieldFilter { ParamName = "@ArtNo", Value = txtArtNo.Text }
                : null;

            _paging.Fields["Checked"].Filter = ddlChecked.SelectedValue != "-1"
                ? new EqualFieldFilter { ParamName = "@Checked", Value = ddlChecked.SelectedValue }
                : null;

            _paging.Fields["PhotoName"].Filter = ddlPhoto.SelectedValue != "any"
                ? new NullFieldFilter { ParamName = "@PhotoName", Null = !ddlPhoto.SelectedValue.TryParseBool() }
                : null;

            DateTime? dateFrom;
            DateTime? dateTo;
            var callDateFilter = new DateTimeRangeFieldFilter
            {
                ParamName = "@AddDate",
                From = (dateFrom = txtDateFrom.Text.TryParseDateTime(true)).HasValue
                    ? new DateTime(dateFrom.Value.Year, dateFrom.Value.Month, dateFrom.Value.Day, 0, 0, 0, 0)
                    : (DateTime?)null,
                To = (dateTo = txtDateTo.Text.TryParseDateTime(true)).HasValue
                    ? new DateTime(dateTo.Value.Year, dateTo.Value.Month, dateTo.Value.Day, 23, 59, 59, 99)
                    : (DateTime?)null
            };
            _paging.Fields["AddDate"].Filter = callDateFilter.From.HasValue || callDateFilter.To.HasValue
                ? callDateFilter
                : null;
        }

        protected void grid_Sorting(object sender, GridViewSortEventArgs e)
        {
            var arrows = new Dictionary<string, string>
                {
                    {"Checked", "arrowChecked"},
                    {"Name", "arrowName"},
                    {"Email", "arrowEmail"},
                    {"[Text]", "arrowText"},
                    {"AddDate", "arrowAddDate"},
                    {"ProductName", "arrowProductName"},
                    {"ArtNo", "arrowArtNo"}
                };

            const string urlArrowUp = "images/arrowup.gif";
            const string urlArrowDown = "images/arrowdown.gif";
            const string urlArrowGray = "images/arrowdownh.gif";

            var csf = (from Field f in _paging.Fields.Values where f.Sorting.HasValue select f).First();
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
                grid.ChangeHeaderImageUrl(arrows[csf.Name], urlArrowGray);

                nsf.Sorting = SortDirection.Ascending;
                grid.ChangeHeaderImageUrl(arrows[nsf.Name], urlArrowUp);
            }

            pageNumberer.CurrentPageIndex = 1;
            _paging.CurrentPageIndex = 1;
        }

        protected void btnAddReview_Click(object sender, EventArgs e)
        {
            _review = null;
            grid.ShowFooter = true;
            grid.FooterStyle.BackColor = System.Drawing.Color.FromName("#ccffcc");
        }

        protected void pn_SelectedPageChanged(object sender, EventArgs e)
        {
            _paging.CurrentPageIndex = pageNumberer.CurrentPageIndex;
        }

        protected void lbDeleteSelected_Click(object sender, EventArgs e)
        {
            if ((_selectionFilter == null) || (_selectionFilter.Values == null))
            {
                return;
            }

            if (!_inverseSelection)
            {
                foreach (var id in _selectionFilter.Values)
                {
                    ReviewService.DeleteReview(SQLDataHelper.GetInt(id));
                }
            }
            else
            {
                var itemsIds = _paging.ItemsIds<int>("ReviewId as ID");
                foreach (int id in itemsIds.Where(iId => !_selectionFilter.Values.Contains(iId.ToString())))
                {
                    ReviewService.DeleteReview(id);
                }
            }
        }

        protected void lbSetChecked_Click(object sender, EventArgs e)
        {
            if ((_selectionFilter == null) || (_selectionFilter.Values == null))
            {
                return;
            }

            if (!_inverseSelection)
            {
                foreach (var id in _selectionFilter.Values)
                {
                    ReviewService.CheckReview(SQLDataHelper.GetInt(id), true);
                }
            }
            else
            {
                var itemsIds = _paging.ItemsIds<int>("ReviewId as ID");
                foreach (int id in itemsIds.Where(iId => !_selectionFilter.Values.Contains(iId.ToString())))
                {
                    ReviewService.CheckReview(id, true);
                }
            }
        }

        protected void lbSetNotChecked_Click(object sender, EventArgs e)
        {
            if ((_selectionFilter == null) || (_selectionFilter.Values == null))
            {
                return;
            }
            if (!_inverseSelection)
            {
                foreach (var id in _selectionFilter.Values)
                {
                    ReviewService.CheckReview(SQLDataHelper.GetInt(id), false);
                }
            }
            else
            {
                var itemsIds = _paging.ItemsIds<int>("ReviewId as ID");
                foreach (int id in itemsIds.Where(iId => !_selectionFilter.Values.Contains(iId.ToString())))
                {
                    ReviewService.CheckReview(id, false);
                }
            }
        }

        private void InitFooter()
        {
            if (!grid.ShowFooter) return;
            ((TextBox)grid.FooterRow.FindControl("txtNewArtNo")).Text = HttpUtility.UrlDecode(Request["AddReview"]);
            if (_review != null)
            {
                ((TextBox)grid.FooterRow.FindControl("txtNewName")).Text = _review.Name ?? string.Empty;
                ((TextBox)grid.FooterRow.FindControl("txtNewEmail")).Text = _review.Email ?? string.Empty;
                ((TextBox)grid.FooterRow.FindControl("txtNewText")).Text = _review.Text ?? string.Empty;
                ((CheckBox)grid.FooterRow.FindControl("cbNewChecked")).Checked = _review.Checked;
            }

            grid.FooterRow.FindControl("txtNewArtNo").Focus();
        }

        protected string GetEntityAdminUrl(int entityId, EntityType type)
        {
            return ReviewService.GetEntityAdminUrl(entityId, type);
        }

        protected string GetEntityName(int commentId)
        {
            var entity = ReviewService.GetReviewEntity(commentId);
            return entity != null ? entity.Name : string.Empty;
        }

        protected string GetImageItem(string photoName)
        {
            if (photoName.IsNotEmpty() && File.Exists(FoldersHelper.GetPathAbsolut(FolderType.ReviewImage, photoName)))
            {
                return string.Format("<div><img src='{0}'></div>", FoldersHelper.GetPath(FolderType.ReviewImage, photoName, true));
            }

            return "";
        }
    }
}