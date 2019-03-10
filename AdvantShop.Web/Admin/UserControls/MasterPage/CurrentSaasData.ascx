<%@ Control Language="C#" AutoEventWireup="true" Inherits="Admin.UserControls.MasterPage.CurrentSaasData" Codebehind="CurrentSaasData.ascx.cs" %>
<%@ Import Namespace="AdvantShop.Catalog" %>
<%@ Import Namespace="Resources" %>

<div class="top-part-right battery" id="battery">
    <div class="battery-progress <%=RenderClass() %>">
    </div>
    <div class="battery-content" id="batteryContent">
        <table class="saasTable">
            <tr>
                <td>
                    <%= Resource.Admin_SaaS_Tarif %>:
                </td>
                <td>
                    <%= MySaasData.Name %>
                </td>
            </tr>
            <tr>
                <td>
                    <%= Resource.Admin_SaaS_DaysLeft %>:
                </td>
                <td>
                    <%= MySaasData.LeftDay  %>
                </td>
            </tr>
            <tr>
                <td>
                    <%= Resource.Admin_SaaS_Money %>:
                </td>
                <td>
                    <%= MySaasData.Money.ToString("F2")  %>
                </td>
            </tr>
            <tr>
                <td>
                    <%= Resource.Admin_SaaS_Bonus %>:
                </td>
                <td>
                    <%= MySaasData.Bonus.ToString("F2")  %>
                </td>
            </tr>
            <tr>
                <td>
                    <%= Resource.Admin_SaaS_ProductsCount %>:
                </td>
                <td>
                    <%= ProductService.GetProductsCount()%> / <%= MySaasData.ProductsCount %>
                </td>
            </tr>
            <%--<tr>
                <td>
                    <%= Resource.Admin_SaaS_MaxPhotoCount %>:
                </td>
                <td>
                    <%= MySaasData.PhotosCount %> 
                </td>
            </tr>
            <tr>
                <td>
                    <%= Resource.Admin_SaaS_ExcelCsvIntegration %>:
                </td>
                <td>
                     <%= MySaasData.HaveExcel ? Resource.Admin_SaaS_Yes : Resource.Admin_SaaS_No%> 
                </td>
            </tr>
            <tr>
                <td>
                    <%= Resource.Admin_SaaS_1cIntegration %>:
                </td>
                <td>
                    <%= MySaasData.Have1C ? Resource.Admin_SaaS_Yes : Resource.Admin_SaaS_No%> 
                </td>
            </tr>
            <tr>
                <td>
                    <%= Resource.Admin_SaaS_ExportFeedIntegration %>:
                </td>
                <td>
                    <%= MySaasData.HaveExportFeeds ? Resource.Admin_SaaS_Yes : Resource.Admin_SaaS_No%> 
                </td>
            </tr>
            <tr>
                <td>
                    <%= Resource.Admin_SaaS_PriceRegulation %>:
                </td>
                <td>
                    <%= MySaasData.HavePriceRegulating ? Resource.Admin_SaaS_Yes : Resource.Admin_SaaS_No%> 
                </td>
            </tr>
            <tr>
                <td>
                    <%= Resource.Admin_SaaS_CurrencyUpdate%>:
                </td>
                <td>
                    <%= MySaasData.HaveBankIntegration ? Resource.Admin_SaaS_Yes : Resource.Admin_SaaS_No%> 
                </td>
            </tr>--%>
            <tr>
                <td>
                    <asp:LinkButton runat="server" ID="lbUpdate" OnClick="lbUpdate_OnClick"><%= Resource.Admin_Modules_Update%></asp:LinkButton>
                </td>
                <td>
                    
                </td>
            </tr>
        </table>
    </div>
</div>
