<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" CodeBehind="BonusSystemAdmin.aspx.cs" Inherits="Admin.BonusSystemAdmin" %>

<%@ Import Namespace="Resources" %>

<%@ Register Src="~/Admin/UserControls/Menu/MarketingNeighborMenu.ascx" TagName="MarketingNeighborMenu" TagPrefix="adv" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="Server">
    <div class="content-top">
        <adv:MarketingNeighborMenu ID="MarketingNeighborMenu" runat="server" />
    </div>
    <div class="content-own">
        <div>
            <table cellpadding="0" cellspacing="0" width="100%">
                <tbody>
                    <tr>
                        <td style="width: 72px;">
                            <img src="images/orders_ico.gif" alt="" />
                        </td>
                        <td style="vertical-align: top;">
                            <asp:Label ID="lblHead" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Admin_SaaS_BonusSystem %>"></asp:Label><br />

                        </td>
                        <td style="text-align: right; vertical-align: bottom;">
                            <asp:Button CssClass="btn btn-middle btn-add" ID="btnSave" runat="server" Text="<%$ Resources:Resource, Admin_Save %>" OnClick="btnSave_Click" />
                        </td>
                    </tr>
                </tbody>
            </table>
            <asp:Label ID="lblMessage" runat="server" Visible="False"></asp:Label>
            <asp:Label runat="server" ID="lblErr" Visible="False"></asp:Label>
            <div style="width: 800px; margin: auto; margin-top: 10px;">

                <table class="info-tb" style="width: 100%;">

                    <tr class="rowsPost ">
                        <td style="width: 250px;">
                            <asp:Label runat="server" Text="<%$ Resources:Resource, BonusSystem_Active%>" /></td>
                        <td>
                            <asp:CheckBox ID="cbEnabled" runat="server" /></td>
                    </tr>

                    <tr class="rowsPost ">
                        <td style="width: 250px;">
                            <asp:Label runat="server" AssociatedControlID="txtApiKey" Text="ApiKey" />
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtApiKey" Width="500px" />
                            <div id="divTrial" runat="server">
                                <asp:Label runat="server" ID="txtApiKeyDemo" Text="**************************************************" />
                                <br />
                                <a href="javascript:void(0);" onclick=" $('.newKey').show() ">Изменить ключ</a>
                                <br />
                                <asp:TextBox runat="server" ID="txtNewKey" CssClass="newKey" Style="display: none; margin: 10px 0" Width="500px" />
                            </div>
                        </td>
                    </tr>


                    <tr class="rowsPost ">
                        <td style="width: 250px;">
                            <asp:Label ID="Localize2" AssociatedControlID="ddlBonusType" runat="server" Text="<%$ Resources:Resource, BonusSystem_ChargeBonuses%>" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlBonusType" runat="server">
                                <asp:ListItem Text="<%$ Resources:Resource, BonusSystem_OrderCost %>" Value="0" />
                                <asp:ListItem Text="<%$ Resources:Resource, BonusSystem_ProductsCost %>" Value="1" />
                            </asp:DropDownList>
                        </td>
                    </tr>


                    <tr class="rowsPost ">
                        <td style="width: 250px;">
                            <asp:Label ID="Label1" AssociatedControlID="txtMaxOrderPercent" runat="server" Text="<%$ Resources:Resource, BonusSystem_MaxOrderPercent%>" />
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtMaxOrderPercent" />
                            %
                        </td>
                    </tr>

                    <tr class="rowsPost ">
                        <td style="width: 250px;">
                            <asp:Label ID="Localize3" AssociatedControlID="lblBonusFirstPercent" runat="server" Text="<%$ Resources:Resource, BonusSystem_DefaultPercent%>" />
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblBonusFirstPercent" />
                            %
                        </td>
                    </tr>

                    <tr class="rowsPost ">
                        <td style="width: 250px;">
                            <asp:Label ID="Label2" AssociatedControlID="txtBonusesForNewCard" runat="server" Text="<%$ Resources:Resource, BonusSystem_BonusesForNewCard%>" />
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtBonusesForNewCard" Text="0" />
                            <div style="padding: 5px 0">
                                <asp:Localize runat="server" Text="<%$ Resources:Resource, BonusSystem_BonusesForNewCard_Hint%>" />
                            </div>
                        </td>
                    </tr>

                    <tr class="rowsPost ">
                        <td style="width: 250px;">
                            <asp:Label runat="server" Text="<%$ Resources:Resource, BonusSystem_BonusTextBlock%>" /></td>
                        <td>
                            <asp:TextBox ID="txtBonusTextBlock" TextMode="MultiLine" runat="server" CssClass="js-wysiwyg" Height="500px" Width="100%" /></td>
                    </tr>
                    <tr class="rowsPost ">
                        <td style="width: 250px;">
                            <asp:Label runat="server" Text="<%$ Resources:Resource, BonusSystem_RightBonusTextBlock%>" /></td>
                        <td>
                            <asp:TextBox ID="txtRightBonusTextBlock" TextMode="MultiLine" runat="server" CssClass="js-wysiwyg" Height="500px" Width="100%" /></td>
                    </tr>

                    <tr class="rowsPost ">
                        <td style="width: 250px;">
                            <asp:Label runat="server" Text="<%$ Resources:Resource, BonusSystem_ShowGrades%>" /></td>
                        <td>
                            <asp:CheckBox runat="server" ID="chkShowGrades" Checked="True" /></td>
                    </tr>

                    <tr class="rowsPost ">
                        <td style="width: 250px;">
                            <asp:Label runat="server" Text="<%$ Resources:Resource, BonusSystem_UseOrderId%>" /></td>
                        <td>
                            <asp:CheckBox ID="ckbUseOrderId" runat="server" /></td>
                    </tr>

                    <tr class="rowsPost ">
                        <td style="width: 250px;"></td>
                        <td>
                            <asp:HyperLink runat="server" ID="hlGetBonusCard" Target="_blank" Text="<%$ Resources:Resource, BonusSystem_GetBonusCardLink%>" /></td>
                    </tr>

                </table>

            </div>
        </div>
    </div>
    


</asp:Content>
