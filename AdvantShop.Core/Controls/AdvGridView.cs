//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AdvantShop.Helpers;

namespace AdvantShop.Core.Controls
{
    public class AdvGridView : GridView
    {
        private string _controlId;
        private string _dataFieldForEditUrlParam = "";
        private string _dataFieldForImageDescription = "";
        private string _dataFieldForImagePath = "";
        private IDictionary<string, string> _updatedRow;
        private string _imageUrl;
        private bool _readOnly;
        private bool _readOnlyRowInInit = true;
        private bool _resetToDefaultValueOnRowEditCancel = true;
        private int _tooltipImgCellIndex = -1;
        private int _tooltipTextCellIndex = -1;
        private GridViewRow _headerRow;
        private GridViewRow _footerRow;
        private bool _showHeaderWhenEmpty = true;
        private bool _showFooterWhenEmpty = true;
        public string EditURL { get; set; }

        public int TooltipImgCellIndex
        {
            get { return _tooltipImgCellIndex; }
            set { _tooltipImgCellIndex = value; }
        }

        public int TooltipTextCellIndex
        {
            get { return _tooltipTextCellIndex; }
            set { _tooltipTextCellIndex = value; }
        }

        public bool ResetToDefaultValueOnRowEditCancel
        {
            get { return _resetToDefaultValueOnRowEditCancel; }
            set { _resetToDefaultValueOnRowEditCancel = value; }
        }

        public bool ReadOnlyRowInInit
        {
            get { return _readOnlyRowInInit; }
            set { _readOnlyRowInInit = value; }
        }

        public bool ReadOnlyGrid
        {
            get { return _readOnly; }
            set { _readOnly = value; }
        }

        public string DataFieldForEditURLParam
        {
            get { return _dataFieldForEditUrlParam; }
            set { _dataFieldForEditUrlParam = value; }
        }

        public string Confirmation { get; set; }

        public IDictionary<string, string> UpdatedRow
        {
            get { return _updatedRow; }
        }


        private string ControlId
        {
            get
            {
                if (_controlId == null)
                {
                    object o = ViewState["ControlId"];
                    _controlId = (o != null) ? (o.ToString()) : null;
                }
                return _controlId;
            }
            set
            {
                ViewState["ControlId"] = value;
                _controlId = value;
            }
        }

        public string DataFieldForImagePath
        {
            get { return _dataFieldForImagePath; }
            set { _dataFieldForImagePath = value; }
        }

        public string DataFieldForImageDescription
        {
            get { return _dataFieldForImageDescription; }
            set { _dataFieldForImageDescription = value; }
        }


        private string ImageUrl
        {
            get
            {
                if (_imageUrl == null)
                {
                    object o = ViewState["ImageUrl"];
                    _imageUrl = (o != null) ? (o.ToString()) : null;
                }
                return _imageUrl;
            }
            set
            {
                ViewState["ImageUrl"] = value;
                _imageUrl = value;
            }
        }

        [Category("Behavior")]
        [Themeable(true)]
        [Bindable(BindableSupport.No)]
        public new bool ShowHeaderWhenEmpty
        {
            get { return _showHeaderWhenEmpty; }
            set { _showHeaderWhenEmpty = value; }
        }

        [Category("Behavior")]
        [Themeable(true)]
        [Bindable(BindableSupport.No)]
        public bool ShowFooterWhenEmpty
        {
            get { return _showFooterWhenEmpty; }
            set { _showFooterWhenEmpty = value; }
        }

        public override GridViewRow HeaderRow
        {
            get { return base.HeaderRow ?? _headerRow; }
        }

        public override GridViewRow FooterRow
        {
            get { return (Rows.Count > 0) ? base.FooterRow : _footerRow; }
        }

        protected override int CreateChildControls(IEnumerable dataSource, bool dataBinding)
        {
            int rows = base.CreateChildControls(dataSource, dataBinding);

            // no data rows created, create empty table if enabled
            if (rows == 0 && (ShowFooterWhenEmpty || ShowHeaderWhenEmpty))
            {
                // create the table
                Table table = CreateChildTable();

                Controls.Clear();
                Controls.Add(table);

                DataControlField[] fields;
                if (AutoGenerateColumns)
                {
                    var source = new PagedDataSource { DataSource = dataSource };

                    ICollection autoGeneratedColumns = CreateColumns(source, true);
                    fields = new DataControlField[autoGeneratedColumns.Count];
                    autoGeneratedColumns.CopyTo(fields, 0);
                }
                else
                {
                    fields = new DataControlField[Columns.Count];
                    Columns.CopyTo(fields, 0);
                }

                TableRowCollection newRows = table.Rows;
                if (ShowHeaderWhenEmpty)
                {
                    // create a new header row
                    _headerRow = CreateRow(-1, -1, DataControlRowType.Header, DataControlRowState.Normal);
                    InitializeRow(_headerRow, fields, newRows);
                }

                // create the empty row
                GridViewRow emptyRow = new GridViewRow(-1, -1, DataControlRowType.EmptyDataRow, DataControlRowState.Normal);
                TableCell cell = new TableCell();
                List<DataControlField> f = fields.Where(dataControlField => dataControlField.Visible).ToList();
                cell.ColumnSpan = f.Count;
                //cell.Width = Unit.Percentage(100);

                // respect the precedence order if both EmptyDataTemplate
                // and EmptyDataText are both supplied ...
                if (EmptyDataTemplate != null)
                {
                    EmptyDataTemplate.InstantiateIn(cell);
                }
                else if (!string.IsNullOrEmpty(EmptyDataText))
                {
                    cell.Controls.Add(new LiteralControl(EmptyDataText));
                }

                emptyRow.Cells.Add(cell);
                GridViewRowEventArgs e = new GridViewRowEventArgs(emptyRow);
                OnRowCreated(e);

                newRows.Add(emptyRow);

                emptyRow.DataBind();
                OnRowDataBound(e);
                emptyRow.DataItem = null;

                if (ShowFooterWhenEmpty && ShowFooter)
                {
                    // create footer row
                    _footerRow = CreateRow(-1, -1, DataControlRowType.Footer, DataControlRowState.Normal);
                    InitializeRow(_footerRow, fields, newRows);
                    newRows.Remove(emptyRow);
                }

            }

            return rows;
        }

        private void InitializeRow(GridViewRow row, DataControlField[] fields, TableRowCollection newRows)
        {
            var e = new GridViewRowEventArgs(row);
            InitializeRow(row, fields);
            OnRowCreated(e);

            newRows.Add(row);

            row.DataBind();
            OnRowDataBound(e);
            row.DataItem = null;
        }

        protected override GridViewRow CreateRow(int rowIndex, int dataSourceIndex, DataControlRowType rowType,
                                                 DataControlRowState rowState)
        {
            return base.CreateRow(rowIndex, dataSourceIndex, rowType, DataControlRowState.Edit);
        }

        protected override void OnRowCommand(GridViewCommandEventArgs e)
        {
            base.OnRowCommand(e);
            if (!string.IsNullOrEmpty(EditURL))
            {
                if (e.CommandName.Equals("GoToEdit"))
                {
                    Page.Response.Redirect(string.Format(EditURL, e.CommandArgument));
                }
            }
        }

        protected override void OnRowCreated(GridViewRowEventArgs e)
        {
            base.OnRowCreated(e);
            string editParam = null;
            var productId = -1;

            if (e.Row.DataItem != null)
            {
                if (!string.IsNullOrEmpty(DataFieldForEditURLParam))
                {
                    editParam = (((DataRowView)e.Row.DataItem)[DataFieldForEditURLParam] is DBNull)
                                    ? null
                                    : (((DataRowView)e.Row.DataItem)[DataFieldForEditURLParam].ToString());
                    if (((DataRowView)e.Row.DataItem).Row.Table.Columns.Contains("ID"))
                    {
                        productId = (((DataRowView)e.Row.DataItem)["ID"] is DBNull)
                                        ? -1
                                        : SQLDataHelper.GetInt(((DataRowView)e.Row.DataItem)["ID"]);
                    }
                }
            }


            if (e.Row.RowType != DataControlRowType.Header)
            {
                for (int i = 0; i <= e.Row.Cells.Count - 1; i++)
                {
                    if (i > 0 && i < e.Row.Cells.Count - 1)
                    {

                        e.Row.Cells[i].Attributes.Add("onkeypress",
                                                      "if(event.keyCode == 13) { " +
                                                      Page.ClientScript.GetPostBackEventReference(this,
                                                                                                  "Update$" +
                                                                                                  e.Row.RowIndex) +
                                                      "; return false;}");

                    }

                    if (i < Columns.Count)
                    {
                        if (Columns[i] is CommandField)
                        {
                            var dbtn = (ImageButton)(from Control c in e.Row.Cells[i].Controls
                                                     where c is ImageButton && ((ImageButton)c).CommandName.Equals("Delete")
                                                     select c).Single();
                            if (dbtn != null)
                            {
                                dbtn.Attributes.Add("onmousedown", "if(confirm(\'" + Confirmation + "\'))this.click();");
                            }
                            var ebtn = (ImageButton)(from Control c in e.Row.Cells[i].Controls
                                                     where c is ImageButton && ((ImageButton)c).CommandName.Equals("Edit")
                                                     select c).SingleOrDefault();
                            if (ebtn != null)
                            {
                                if (!string.IsNullOrEmpty(EditURL))
                                {
                                    ebtn.CommandName = "GoToEdit";
                                    ebtn.CommandArgument = editParam;
                                }
                                else
                                {
                                    ebtn.OnClientClick = "open_window(\'m_Product.aspx?productid=" + editParam +
                                                         "\' ,750,640);return false;";
                                }
                            }
                        }
                    }
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.ControlStyle.BackColor = FooterStyle.BackColor;
            }
            if (e.Row.RowType != DataControlRowType.Header)
            {

            }
        }

        protected override void OnRowCancelingEdit(GridViewCancelEditEventArgs e)
        {
            EditIndex = -1;
        }

        protected override void OnRowDeleting(GridViewDeleteEventArgs e)
        {
            GridViewRow row = Rows[e.RowIndex];

            for (int i = 0; i <= Columns.Count - 1; i++)
            {
                if (!string.IsNullOrEmpty(Columns[i].AccessibleHeaderText))
                {
                    foreach (Control c in row.Cells[i].Controls)
                    {
                        if (c is TextBox)
                        {
                            e.Values.Add(Columns[i].AccessibleHeaderText, (((TextBox)c)).Text);
                            break;
                        }
                        if (c is Label)
                        {
                            e.Values.Add(Columns[i].AccessibleHeaderText, (((Label)c)).Text);
                            break;
                        }
                        if (c is CheckBox)
                        {
                            e.Values.Add(Columns[i].AccessibleHeaderText, (((CheckBox)c)).Checked.ToString());
                            break;
                        }
                    }
                }
            }

            base.OnRowDeleting(e);
        }


        protected override void OnRowUpdating(GridViewUpdateEventArgs e)
        {
            //MyBase.OnRowUpdating(e)
            if (Rows.Count <= e.RowIndex || e.RowIndex == -1)
            {
                return;
            }
            GridViewRow row = Rows[e.RowIndex];
            var values = new Dictionary<string, string>();

            for (int i = 0; i <= Columns.Count - 1; i++)
            {
                if (string.IsNullOrEmpty(Columns[i].AccessibleHeaderText))
                    continue;
                foreach (Control c in row.Cells[i].Controls)
                {
                    if (c is TextBox)
                    {
                        values.Add(Columns[i].AccessibleHeaderText, (((TextBox)c)).Text);
                        break;
                    }
                    if (c is HiddenField)
                    {
                        values.Add(Columns[i].AccessibleHeaderText, (((HiddenField)c)).Value);
                        break;
                    }
                    if (c is Label)
                    {
                        values.Add(Columns[i].AccessibleHeaderText, (((Label)c)).Text);
                        break;
                    }
                    if (c is CheckBox)
                    {
                        values.Add(Columns[i].AccessibleHeaderText, (((CheckBox)c)).Checked.ToString());
                        break;
                    }
                    if (c is PlainCheckBox)
                    {
                        values.Add(Columns[i].AccessibleHeaderText, (((PlainCheckBox)c)).Checked.ToString());
                        break;
                    }
                    if (c is DropDownList)
                    {
                        values.Add(Columns[i].AccessibleHeaderText, (((DropDownList)c)).SelectedValue.ToString());
                        break;
                    }
                }
            }

            _updatedRow = values;

            EditIndex = -1;
        }

        public void ChangeHeaderImageUrl(string ctrlId, string imgUrl)
        {
            ControlId = ctrlId;
            ImageUrl = imgUrl;
        }


        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if ((ControlId != null) && (HeaderRow != null))
            {
                ((Image)(HeaderRow.FindControl(ControlId))).ImageUrl = ImageUrl;
            }
            if (Controls.Count > 0)
            {
                var scripTag = new HtmlGenericControl("input");
                scripTag.Attributes.Add("class", "resetToDefaultValue");
                scripTag.Attributes.Add("type", "hidden");
                scripTag.Attributes.Add("value", _resetToDefaultValueOnRowEditCancel.ToString());
                Controls.Add(scripTag);
                scripTag = new HtmlGenericControl("input");
                scripTag.Attributes.Add("class", "readOnlyRowInInit");
                scripTag.Attributes.Add("type", "hidden");
                scripTag.Attributes.Add("value", _readOnlyRowInInit.ToString());
                Controls.Add(scripTag);
                //ReadOnlyRowInInit

                scripTag = new HtmlGenericControl("input");
                scripTag.Attributes.Add("class", "readOnlyGrid");
                scripTag.Attributes.Add("type", "hidden");
                scripTag.Attributes.Add("value", _readOnly.ToString());
                Controls.Add(scripTag);
            }
        }
    }
}