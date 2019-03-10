<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Admin.UserControls.Catalog.AddProduct" CodeBehind="AddProduct.ascx.cs" EnableViewState="false" %>

<script>
    $(function() {
        if ($(".autotranslitSource").length && $(".autotranslitOut").length) {
            $(".autotranslitSource").on("focusout", function () {
                if ($(".autotranslitSource").val().length)
                    $(".autotranslitOut").val(translite($(".autotranslitSource").val()));
            });
        }
    });
</script>

<div style="display: none;">
    <div id="addProductForm" class="modal-add-product">
        <div>
            <span id="errAddProduct" class="error"></span>
        </div>
        <div class="param-name">
            <%= Resources.Resource.Admin_Product_Name %>&nbsp;<span class="required">*</span>
        </div>
        <div class="param-value">
            <asp:TextBox ID="txtProductName" runat="server" CssClass="niceTextBox shortTextBoxClass autotranslitSource" 
                TabIndex="1" ValidationGroup="AddProduct" data-add-product-name />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtProductName" 
                CssClass="err-icon" Text="*" ValidationGroup="AddProduct" EnableClientScript="True" Display="Dynamic" />
        </div>
        <div class="param-name">
            <%= Resources.Resource.Admin_Product_StockNumber%>
        </div>
        <div class="param-value">
            <asp:TextBox ID="txtArtNo" runat="server" CssClass="niceTextBox shortTextBoxClass" 
                TabIndex="2" ValidationGroup="AddProduct" data-add-product-artno />
        </div>
        <div class="param-name">
            URL
        </div>
        <div class="param-value">
            <asp:TextBox ID="txtUrl" runat="server" CssClass="niceTextBox shortTextBoxClass autotranslitOut" 
                TabIndex="3" ValidationGroup="AddProduct" data-add-product-url />
        </div>
        <div class="bottom">
            <img src="images/loading.gif" alt="" id="waitAddProduct" class="wait-img" />
            <asp:LinkButton ID="lnkAddProduct" runat="server" Text="Добавить" CssClass="btn btn-middle btn-confirm" 
                ValidationGroup="AddProduct" OnClientClick="return false;" data-add-product-btn />
        </div>
    </div>
</div>
