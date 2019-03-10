<%@ Control Language="C#" AutoEventWireup="true" Inherits="AdvantShop.Module.MoySklad.MoySkladExportExcel" Codebehind="MoySkladExportExcel.ascx.cs" %>

<div style="padding-left: 10px;">
    <div>
        <label><asp:CheckBox runat="server" ID="chkOnlyProducts"/> Товары без остатков</label>
    </div>
    <asp:Button runat="server" OnClick="btnExportCatalog_Click" Text="Экспорт товаров из магазина"/>
</div>