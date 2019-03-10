<%@ Control Language="C#" AutoEventWireup="true" Inherits="AdvantShop.Module.StoreReviews.Admin_StoreReviewsSettings" Codebehind="StoreReviewsSettings.ascx.cs" %>
<%@ Import Namespace="Resources" %>
<div>
    <table border="0" cellpadding="2" cellspacing="0">
        <tr class="rowsPost">
            <td colspan="2" style="height: 34px;">
                <span class="spanSettCategory">
                    <asp:Localize ID="Localize9" runat="server" Text="<%$ Resources: StoreReviews_Header%>" /></span>
                <asp:Label ID="lblMessage" runat="server" Visible="False" Style="float: right; margin-left: 10px;"></asp:Label>
                <hr color="#C2C2C4" size="1px" />
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="width: 300px;">
                <asp:Localize ID="Localize2" runat="server" Text="<%$ Resources: StoreReviews_ShowRatio%>"></asp:Localize>
            </td>
            <td>
                <asp:CheckBox runat="server" ID="chkShowRatio" Checked="True" />
            </td>
        </tr>
        <tr class="rowsPost">
            <td>
                <asp:Localize ID="Localize10" runat="server" Text="<%$ Resources: StoreReviews_UseCaptcha%>"></asp:Localize>
            </td>
            <td>
                <asp:CheckBox runat="server" ID="chkUseCaptcha" Checked="False" />
            </td>
        </tr>
        <tr class="rowsPost">
            <td>
                <asp:Localize ID="Localize3" runat="server" Text="<%$ Resources: StoreReviews_PageSize%>"></asp:Localize>
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtPageSize" Width="200px"></asp:TextBox>
            </td>
        </tr>
        <tr class="rowsPost">
            <td>
                <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources: StoreReviews_ActiveModerate%>"></asp:Localize>
            </td>
            <td>
                <asp:CheckBox runat="server" ID="ckbActiveModerate" />
            </td>
        </tr>
        <tr class="rowsPost">
            <td>
                <asp:Localize runat="server" Text="<%$ Resources: StoreReviews_AllowImageUploading %>"></asp:Localize>
            </td>
            <td>
                <asp:CheckBox runat="server" ID="ckbAllowImageUploading" />
            </td> 
        </tr>
        <tr class="rowsPost">
            <td>
                <asp:Localize ID="Localize8" runat="server" Text="<%$ Resources:StoreReviews_ImageDimensions%>"></asp:Localize>
            </td>
            <td>
                <asp:Localize runat="server" Text="<%$ Resources:StoreReviews_ImageWidth%>" />:
                <asp:TextBox ID="txtMaxImageWidth" runat="server" Width="35px" />&nbsp;px,&nbsp;
                <asp:Localize runat="server" Text="<%$ Resources:StoreReviews_Imageheight%>" />:
                <asp:TextBox ID="txtMaxImageHeight" runat="server" Width="35px" />&nbsp;px
            </td> 
        </tr>
        <tr class="rowsPost">
            <td>
                <asp:Localize ID="Localize5" runat="server" Text="<%$ Resources: StoreReviews_PageTitle %>"></asp:Localize>
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtPageTitle" Width="200px"></asp:TextBox>
            </td>
        </tr>
        <tr class="rowsPost">
            <td>
                <asp:Localize ID="Localize6" runat="server" Text="<%$ Resources: StoreReviews_MetaKeyWords %>"></asp:Localize>
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtMetaKeyWords" Width="200px"></asp:TextBox>
            </td>
        </tr>
        <tr class="rowsPost">
            <td>
                <asp:Localize ID="Localize7" runat="server" Text="<%$ Resources: StoreReviews_MetaDescription %>"></asp:Localize>
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtMetaDescription" Width="200px"></asp:TextBox>
            </td> 
        </tr>
        <tr class="rowsPost">
            <td>
                <asp:Localize ID="Localize4" runat="server" Text=""></asp:Localize>
            </td>
            <td>
                <asp:HyperLink href="../storereviews" runat="server" Text="<%$ Resources:StoreReviews_URL%>" Target="_blank" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="<%$ Resources:StoreReviews_Save %>" />
            </td>
        </tr>
    </table>
</div>