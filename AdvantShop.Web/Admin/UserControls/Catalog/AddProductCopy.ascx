<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Admin.UserControls.Catalog.AddProductCopy" CodeBehind="AddProductCopy.ascx.cs" %>

<div style="display: none;">
    <div id="addProductCopyForm" class="modal-add-product">
        <div>
            <span id="errAddProductCopy" class="error"></span>
        </div>
        <div class="param-name">
            <%= Resources.Resource.Admin_Product_Name %>
        </div>
        <div class="param-value">
            <asp:TextBox ID="txtProductName" runat="server" CssClass="niceTextBox shortTextBoxClass" 
                TabIndex="1" ValidationGroup="AddProductCopy" data-add-productcopy-name />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtProductName" 
                CssClass="err-icon" Text="*" ValidationGroup="AddProductCopy" EnableClientScript="True" Display="Dynamic" />
        </div>
        <div class="param-name">
            <%= Resources.Resource.Admin_Product_StockNumber%>
        </div>
        <div class="param-value">
            <asp:TextBox ID="txtArtNo" runat="server" CssClass="niceTextBox shortTextBoxClass" 
                TabIndex="2" ValidationGroup="AddProductCopy" data-add-productcopy-artno />
        </div>
        <div class="param-name">
            URL
        </div>
        <div class="param-value">
            <asp:TextBox ID="txtUrl" runat="server" CssClass="niceTextBox shortTextBoxClass"
                TabIndex="3" ValidationGroup="AddProductCopy" data-add-productcopy-url />
        </div>
        <div class="bottom">
            <img src="images/loading.gif" alt="" id="waitAddProductCopy" class="wait-img" />
            <asp:LinkButton ID="lnkAddProductCopy" runat="server" Text="Добавить" CssClass="btn btn-middle btn-confirm"
                ValidationGroup="AddProductCopy" OnClientClick="return false;" data-add-productcopy-btn ></asp:LinkButton>
        </div>
    </div>
</div>
