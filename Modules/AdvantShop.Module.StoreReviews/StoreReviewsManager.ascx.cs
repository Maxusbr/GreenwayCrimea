using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop.Module.StoreReviews.Domain;
using System.Collections.Generic;
using AdvantShop.Helpers;
using System.Data;
using System.Web.UI.HtmlControls;
using AdvantShop.Module.StoreReviews.App_LocalResources;
using AdvantShop.Core.SQL;
using System.Linq;

namespace AdvantShop.Module.StoreReviews
{
    public partial class Admin_StoreReviewsManager : UserControl
    {

        private InSetFieldFilter _selectionFilter;
        private bool _inverseSelection;

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
            if (IsPostBack)
            {
                string strIds = Request.Form["SelectedIds"];
                if (!string.IsNullOrEmpty(strIds))
                {
                    strIds = strIds.Trim();
                    var arrids = strIds.Split(' ');

                    var ids = new string[arrids.Length];
                    _selectionFilter = new InSetFieldFilter { IncludeValues = true };

                    for (int idx = 0; idx <= ids.Length - 1; idx++)
                    {
                        string t = arrids[idx];
                        var idParts = t.Split('_');
                        switch (idParts[0])
                        {
                            case "Product":
                                if (idParts[1] != "-1")
                                {
                                    ids[idx] = idParts[1];
                                }
                                else
                                {
                                    _selectionFilter.IncludeValues = false;
                                    _inverseSelection = true;
                                }
                                break;
                            default:
                                _inverseSelection = true;
                                break;
                        }
                    }
                    _selectionFilter.Values = ids.Distinct().Where(item => item != null).ToArray();
                }
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            //lvReviews.DataSource = StoreReviewRepository.GetStoreReviews();
            //lvReviews.DataBind();
            var table = StoreReviewRepository.GetStoreReviews();
            DataColumn col = new DataColumn("IsSelected", typeof(bool));
            table.Columns.Add(col);
            if ((_selectionFilter != null) && (_selectionFilter.Values != null))
            {
                for (var i = 0; i <= table.Rows.Count - 1; i++)
                {
                    var intIndex = i;
                    if (Array.Exists(_selectionFilter.Values, c => c == table.Rows[intIndex]["ID"].ToString()))
                    {
                        table.Rows[i]["IsSelected"] = !_inverseSelection;
                    }
                }
            }
            grid.DataSource = table;
            grid.DataBind();
            lblProducts.Text = table.Rows.Count.ToString();
        }

        //protected void lvReviewsItemCommand(object sender, ListViewCommandEventArgs e)
        //{
        //    switch (e.CommandName)
        //    {
        //        case "deleteReview":
        //            int reviewId;
        //            if (Int32.TryParse(e.CommandArgument.ToString(), out reviewId))
        //            {
        //                StoreReviewRepository.DeleteStoreReviewsById(reviewId);
        //            }
        //            break;
        //    }
        //}
        
        protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "deleteReview")
            {
                StoreReviewRepository.DeleteStoreReviewsById(Convert.ToInt32(e.CommandArgument));
            }
        }

        protected void grid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem == null) return;
            var t = (DataRowView)e.Row.DataItem;

            ((HtmlAnchor)(e.Row.Cells[e.Row.Cells.Count - 1].FindControl("cmdlink"))).HRef =
                "/modules/StoreReviews/editreview.aspx?id=" + t["ID"];



            ((HtmlAnchor)(e.Row.Cells[e.Row.Cells.Count - 1].FindControl("cmdlink"))).Title = "Редактировать";
            ((LinkButton)(e.Row.Cells[e.Row.Cells.Count - 1].FindControl("buttonDelete"))).CommandName =
                "deleteReview";

            ((LinkButton)
             (e.Row.Cells[e.Row.Cells.Count - 1].FindControl("buttonDelete"))).Attributes["data-confirm"]
                = string.Format("Вы действительно хотите удалить отзыв {0} ?", t["ReviewerName"]);
            

            var tr = new AsyncPostBackTrigger
            {
                ControlID = ((e.Row.Cells[e.Row.Cells.Count - 1].FindControl("buttonDelete"))).UniqueID,
                EventName = "Click"
            };

            UpdatePanel1.Triggers.Add(tr);
        }

        protected void lbDeleteSelected_Click(object sender, EventArgs e)
        {
            string[] str = Request.Form["SelectedIds"].Split(' ');
            foreach(string item in str)
            {
                if(!string.IsNullOrEmpty(item) && Request.Form["SelectedIds"] != "-1")
                    StoreReviewRepository.DeleteStoreReviewsById(Convert.ToInt32(item));
            }
            if (Equals(Request.Form["SelectedIds"], "-1"))
            {
                DataTable table = StoreReviewRepository.GetStoreReviews();
                foreach (DataRow item in table.Rows)
                {
                    if (!string.IsNullOrEmpty(item[0].ToString()))
                    {
                        StoreReviewRepository.DeleteStoreReviewsById(Convert.ToInt32(item[0].ToString()));
                    }
                }
            }
        }

        protected void lbConfirmSelected_Click(object sender, EventArgs e)
        {
            string[] str = Request.Form["SelectedIds"].Split(' ');
            foreach (string item in str)
            {
                if (!string.IsNullOrEmpty(item) && !Equals(item,"-1"))
                {
                    StoreReview review = StoreReviewRepository.GetStoreReview(Convert.ToInt32(item));
                    review.Moderated = true;
                    StoreReviewRepository.UpdateStoreReview(review);
                }
            }
            if (Equals(Request.Form["SelectedIds"], "-1"))
            {
                DataTable table = StoreReviewRepository.GetStoreReviews();
                foreach(DataRow item in table.Rows)
                {
                    if (!string.IsNullOrEmpty(item[0].ToString()))
                    {
                        StoreReview review = StoreReviewRepository.GetStoreReview(Convert.ToInt32(item[0].ToString()));
                        review.Moderated = true;
                        StoreReviewRepository.UpdateStoreReview(review);
                    }
                }
            }
        }
    }
}