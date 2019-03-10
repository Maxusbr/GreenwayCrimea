using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.SQL;
using AdvantShop.Module.AbandonedCarts.Domain;
using AdvantShop.Orders;
using Newtonsoft.Json;

namespace AdvantShop.Module.AbandonedCarts
{
    public partial class AbandonedCartsModuleWishList : UserControl
    {
        private bool _inverseSelection;
        private SqlPaging _paging;
        private InSetFieldFilter _selectionFilter;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                _paging = new SqlPaging
                {
                    TableName = "[Catalog].[ShoppingCart] as sc " +
                                "Inner Join [Customers].[Customer] On sc.[CustomerId] = [Customer].[CustomerID] ",
                    ItemsPerPage = 30,
                };

                _paging.AddFieldsRange(new List<Field>
                {
                    new Field {Name = "sc.[CustomerId] as CustomerId", IsDistinct = true},
                    new Field {Name = "ShoppingCartType"},
                    new Field
                    {
                        Name = "(Select top(1) Email From [Customers].[Customer] " +
                               "Where [Customer].[CustomerID] = sc.CustomerId) as Email"
                    },
                    new Field
                    {
                        Name = "(Select top(1) UpdatedOn From [Catalog].[ShoppingCart] " +
                               "Where [ShoppingCart].[CustomerID] = sc.CustomerId and ShoppingCartType = @CartType Order By UpdatedOn Desc) as LastUpdate",
                        Sorting = SortDirection.Descending
                    },
                    //new Field
                    //    {
                    //        Name = "(Select SUM(Price * [ShoppingCart].[Amount]) " +
                    //               "From [Catalog].[ShoppingCart] " +
                    //               "Inner Join [Catalog].[Offer] On [Offer].[OfferID] = [ShoppingCart].[OfferId] " +
                    //               "Where [ShoppingCart].[CustomerID] = sc.CustomerId and ShoppingCartType = @CartType) As Sum"
                    //    },
                    new Field
                    {
                        Name = "(Select Count(*) From [Module].[AbandonedCartLetter] " +
                               "Where AbandonedCartLetter.CustomerId = sc.CustomerId) as SendingCount"
                    },
                    new Field
                    {
                        Name = "(Select Top(1)SendingDate From [Module].[AbandonedCartLetter] " +
                               "Where AbandonedCartLetter.CustomerId = sc.CustomerId Order By SendingDate Desc) as SendingDate"
                    },
                });

                _paging.Fields["ShoppingCartType"].Filter = new EqualFieldFilter
                {
                    ParamName = "@CartType",
                    Value = ((int) ShoppingCartType.Wishlist).ToString()
                };

                pageNumberer.CurrentPageIndex = 1;
                _paging.CurrentPageIndex = 1;
                ViewState["Paging"] = _paging;
            }
            else
            {
                _paging = (SqlPaging) (ViewState["Paging"]);

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


            if (!IsPostBack)
            {
                ddlTemplate.Items.Clear();
                ddlTemplate.Items.Add(new ListItem("не выбран", "-1"));
                foreach (AbandonedCartTemplate template in AbandonedCartsService.GetTemplates())
                {
                    ddlTemplate.Items.Add(new ListItem(template.Name, template.Id.ToString()));
                }
            }
        }

        protected void grid_Sorting(object sender, GridViewSortEventArgs e)
        {
            pageNumberer.CurrentPageIndex = 1;
            _paging.CurrentPageIndex = 1;
        }

        protected void pn_SelectedPageChanged(object sender, EventArgs e)
        {
            _paging.CurrentPageIndex = pageNumberer.CurrentPageIndex;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            DataTable data = _paging.PageItems;
            while (data.Rows.Count < 1 && _paging.CurrentPageIndex > 1)
            {
                _paging.CurrentPageIndex--;
                data = _paging.PageItems;
            }

            var clmn = new DataColumn("IsSelected", typeof (bool)) {DefaultValue = _inverseSelection};
            data.Columns.Add(clmn);
            if ((_selectionFilter != null) && (_selectionFilter.Values != null))
            {
                for (int i = 0; i <= data.Rows.Count - 1; i++)
                {
                    int intIndex = i;
                    if (Array.Exists(_selectionFilter.Values, c => c == (data.Rows[intIndex]["CustomerId"]).ToString()))
                    {
                        data.Rows[i]["IsSelected"] = !_inverseSelection;
                    }
                }
            }

            grid.DataSource = data;
            grid.DataBind();

            pageNumberer.PageCount = _paging.PageCount;
        }

        protected void ddlTemplate_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            pnlTemplate.Visible = false;
            btnSendLetter.Enabled = false;

            if (ddlTemplate.SelectedIndex != 0)
            {
                AbandonedCartTemplate template =
                    AbandonedCartsService.GetTemplate(ddlTemplate.SelectedValue.TryParseInt());
                if (template != null)
                {
                    pnlTemplate.Visible = true;
                    txtSubject.Text = template.Subject;
                    ckeBody.Text = template.Body;
                    btnSendLetter.Enabled = true;
                }
            }
        }

        protected void btnSendLetter_OnClick(object sender, EventArgs e)
        {
            lblError.Visible = false;
            lblSuccess.Visible = false;

            if (ddlTemplate.SelectedIndex == 0)
            {
                return;
            }

            string strIds = Request.Form["SelectedIds"];

            if (string.IsNullOrEmpty(strIds))
            {
                lblError.Visible = true;
                lblError.Text = "Выберите хотя бы одну корзину";
                return;
            }

            strIds = strIds.Trim();
            List<Guid> cartIds = strIds.Split(' ').Where(x => x != "-1").Select(x => x.TryParseGuid()).ToList();

            if (cartIds.Count == 0)
            {
                lblError.Visible = true;
                lblError.Text = "Выберите хотя бы одну корзину";
                return;
            }

            var template = new AbandonedCartTemplate
            {
                Id = ddlTemplate.SelectedValue.TryParseInt(),
                Subject = txtSubject.Text,
                Body = ckeBody.Text
            };

            int count = AbandonedCartsService.SendMessageReg(template, cartIds, ShoppingCartType.Wishlist);

            lblSuccess.Visible = true;
            lblSuccess.Text = string.Format("Письмо было отослано {0} раз(а)", count);
        }

        protected string RenderEmail(string data)
        {
            if (data.IsNotEmpty())
            {
                var confirmData = JsonConvert.DeserializeObject<CheckoutData>(data);
                if (confirmData != null && confirmData.User != null)
                {
                    return !string.IsNullOrEmpty(confirmData.User.Email)
                        ? confirmData.User.Email
                        : "не заполнен";
                }
            }

            return "не заполнен";
        }

        protected string RenderSendingCount(int count, DateTime? sendingDate)
        {
            if (count > 0 && sendingDate != null)
                return string.Format("{0} {1}. Последний раз: {2}", count,
                    Strings.Numerals(count, "писем", "письмо", "письма", "писем"), sendingDate);

            return string.Empty;
        }

        protected string RenderPrice(Guid customerId)
        {
            var shoppingCart = ShoppingCartService.GetShoppingCart(ShoppingCartType.Wishlist, customerId);
            return shoppingCart.TotalPrice.FormatPrice();
        }
    }
}