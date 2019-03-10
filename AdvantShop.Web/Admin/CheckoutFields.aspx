<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" Inherits="Admin.CheckoutFields" Codebehind="CheckoutFields.aspx.cs" %>
<%@ Import Namespace="Resources" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="Server">
    <div id="inprogress" style="display: none;">
        <div id="curtain" class="opacitybackground">
            &nbsp;</div>
        <div class="loader">
            <table width="100%" style="font-weight: bold; text-align: center;">
                <tbody>
                    <tr>
                        <td style="text-align: center;">
                            <img src="images/ajax-loader.gif" alt="" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center; color: #0D76B8;">
                            <asp:Localize ID="Localize_Admin_Properties_PleaseWait" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_PleaseWait %>"></asp:Localize>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
        <div class="content-top">
        <menu class="neighbor-menu neighbor-catalog">
            <li class="neighbor-menu-item"><a href="CommonSettings.aspx">
                <%= Resource.Admin_MasterPageAdmin_Settings%></a></li>
            <li class="neighbor-menu-item selected"><a href="CheckoutFields.aspx">
                <%= Resource.Admin_MasterPageAdmin_CheckoutFields%></a></li>
            <li class="neighbor-menu-item"><a href="PaymentMethod.aspx">
                <%= Resource.Admin_MasterPageAdmin_PaymentMethod%></a></li>
            <li class="neighbor-menu-item"><a href="ShippingMethod.aspx">
                <%= Resource.Admin_MasterPageAdmin_ShippingMethod%></a></li>
            <li class="neighbor-menu-item"><a href="Country.aspx">
                <%= Resource.Admin_MasterPageAdmin_Countries%></a></li>
            <li class="neighbor-menu-item"><a href="Currencies.aspx">
                <%= Resource.Admin_MasterPageAdmin_Currency%></a></li>
            <li class="neighbor-menu-item"><a href="Taxes.aspx">
                <%= Resource.Admin_MasterPageAdmin_Taxes%></a></li>
            <li class="neighbor-menu-item"><a href="MailFormat.aspx">
                <%= Resource.Admin_MasterPageAdmin_MailFormat%></a></li>
            <li class="neighbor-menu-item"><a href="301Redirects.aspx">
                <%= Resource.Admin_MasterPageAdmin_301Redirects%></a></li>
            <li class="neighbor-menu-item"><a href="LogError404.aspx">
                <%= Resource.Admin_MasterPageAdmin_LogError404%></a></li>
            <li class="neighbor-menu-item"><a href="Localizations.aspx">
                <%= Resource.Admin_Localizations_Header%></a></li>
        </menu>
    </div>
    <div class="content-own">
        <div>
            <table cellpadding="0" cellspacing="0" width="100%">
                <tbody>
                    <tr>
                        <td style="width: 72px;">
                            <img src="images/orders_ico.gif" alt="" />
                        </td>
                        <td>
                            <asp:Label ID="lblHead" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Admin_ContactFields_Header %>"></asp:Label><br />
                            <asp:Label ID="lblSubHead" CssClass="AdminSubHead" runat="server" Text="<%$ Resources:Resource, Admin_ContactFields_SubHeader %>"></asp:Label>
                        </td>
                        <td style="width: 200px">
                            <asp:Button CssClass="btn btn-middle btn-add" ID="btnSave" runat="server" Text="<%$ Resources:Resource, Admin_Save %>" OnClick="btnSave_OnClick" />
                        </td>
                    </tr>
                </tbody>
            </table>
            <div style="width: 100%">
                <div style="margin: 20px 0 10px 0;font-weight: bold;">
                    <%= Resource.Admin_ContactFields_CustomerFieds %>
                </div>

                <table class="table-ui" style="width: auto">
                    <tr>
                        <th style="width: 350px"><%= Resource.Admin_ContactFields_Name %></th>
                        <th style="width: 200px"><%= Resource.Admin_ContactFields_Show %></th>
                        <th style="width: 200px"><%= Resource.Admin_ContactFields_Required %></th>
                    </tr>
                    <tr>
                        <td><asp:TextBox  runat="server" ID="txtFirstName" Width="300px"/></td>
                        <td><input type="checkbox" checked="checked" disabled="disabled"/></td>
                        <td><input type="checkbox" checked="checked" disabled="disabled"/></td>
                    </tr>
                    <tr>
                        <td><%= Resource.Admin_ContactFields_LastName %></td>
                        <td><asp:CheckBox runat="server" ID="chkIsShowLastName" /></td>
                        <td><asp:CheckBox runat="server" ID="chkIsReqLastName" /></td>
                    </tr>
                    <tr>
                        <td><%= Resource.Admin_ContactFields_Patronymic%></td>
                        <td><asp:CheckBox runat="server" ID="chkIsShowPatronymic" /></td>
                        <td><asp:CheckBox runat="server" ID="chkIsReqPatronymic" /></td>
                    </tr>
                    <tr>
                        <td><asp:TextBox  runat="server" ID="txtPhone" Width="300px"/></td>
                        <td><asp:CheckBox runat="server" ID="chkIsShowPhone" /></td>
                        <td><asp:CheckBox runat="server" ID="chkIsReqPhone" /></td>
                    </tr>
                </table>
                
                <div style="margin: 20px 0 10px 0;font-weight: bold;">
                    <%= Resource.Admin_ContactFields_ShippingFields%>
                </div>
                
                <table class="table-ui" style="width: auto">
                    <tr>
                        <th style="width: 350px"><%= Resource.Admin_ContactFields_Name %></th>
                        <th style="width: 200px"><%= Resource.Admin_ContactFields_Show %></th>
                        <th style="width: 200px"><%= Resource.Admin_ContactFields_Required %></th>
                    </tr>
                    <tr>
                        <td><%= Resource.Admin_ContactFields_Country %></td>
                        <td><asp:CheckBox runat="server" ID="chkIsShowCountry" /></td>
                        <td><asp:CheckBox runat="server" ID="chkIsReqCountry" /></td>
                    </tr>
                    <tr>
                        <td><%= Resource.Admin_ContactFields_State %></td>
                        <td><asp:CheckBox runat="server" ID="chkIsShowState" /></td>
                        <td><asp:CheckBox runat="server" ID="chkIsReqState" /></td>
                    </tr>
                    <tr>
                        <td><%= Resource.Admin_ContactFields_City %></td>
                        <td><asp:CheckBox runat="server" ID="chkIsShowCity" /></td>
                        <td><asp:CheckBox runat="server" ID="chkIsReqCity" /></td>
                    </tr>
                    <tr>
                        <td><%= Resource.Admin_ContactFields_Zip %></td>
                        <td><asp:CheckBox runat="server" ID="chkIsShowZip" /></td>
                        <td><asp:CheckBox runat="server" ID="chkIsReqZip" /></td>
                    </tr>
                    <tr>
                        <td><%= Resource.Admin_ContactFields_Address %></td>
                        <td><asp:CheckBox runat="server" ID="chkIsShowAddress" /></td>
                        <td><asp:CheckBox runat="server" ID="chkIsReqAddress" /></td>
                    </tr>
                    <tr>
                        <td><%= Resource.Admin_ContactFields_UserComment %></td>
                        <td><asp:CheckBox runat="server" ID="chkIsShowUserComment" /></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td><asp:TextBox  runat="server" ID="txtCustomShippingField1" Width="300px" /></td>
                        <td><asp:CheckBox runat="server" ID="chkIsShowCustomShippingField1" /></td>
                        <td><asp:CheckBox runat="server" ID="chkIsReqCustomShippingField1" /></td>
                    </tr>
                    <tr>
                        <td><asp:TextBox  runat="server" ID="txtCustomShippingField2" Width="300px"/></td>
                        <td><asp:CheckBox runat="server" ID="chkIsShowCustomShippingField2" /></td>
                        <td><asp:CheckBox runat="server" ID="chkIsReqCustomShippingField2" /></td>
                    </tr>
                    <tr>
                        <td><asp:TextBox  runat="server" ID="txtCustomShippingField3" Width="300px"/></td>
                        <td><asp:CheckBox runat="server" ID="chkIsShowCustomShippingField3" /></td>
                        <td><asp:CheckBox runat="server" ID="chkIsReqCustomShippingField3" /></td>
                    </tr>
                </table>
                
                <table cellpadding="5" style="margin:25px 0 0 0;">
                    <tr>
                        <td style="vertical-align: top; padding: 10px 5px 10px 0;">
                            Показывать полный адрес (улица, дом, кв и тд.)
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="chkIsShowFullAddress" />
                        </td>
                    </tr>
                </table>
                
                <div style="margin: 20px 0 10px 0;font-weight: bold;">
                    <%= Resource.Admin_ContactFields_BuyOneClick%>
                </div>
                
                <table class="table-ui" style="width: auto">
                    <tr>
                        <th style="width: 350px"><%= Resource.Admin_ContactFields_Name %></th>
                        <th style="width: 200px"><%= Resource.Admin_ContactFields_Show %></th>
                        <th style="width: 200px"><%= Resource.Admin_ContactFields_Required %></th>
                    </tr>
                    <tr>
                        <td><asp:TextBox  runat="server" ID="txtBuyInOneClickName" Width="300px" /></td>
                        <td><asp:CheckBox runat="server" ID="chkIsShowBuyInOneClickName" /></td>
                        <td><asp:CheckBox runat="server" ID="chkIsRequiredBuyInOneClickName" /></td>
                    </tr>
                    <tr>
                        <td><asp:TextBox  runat="server" ID="txtBuyInOneClickEmail" Width="300px" /></td>
                        <td><asp:CheckBox runat="server" ID="chkIsShowBuyInOneClickEmail" /></td>
                        <td><asp:CheckBox runat="server" ID="chkIsRequiredBuyInOneClickEmail" /></td>
                    </tr>
                    <tr>
                        <td><asp:TextBox  runat="server" ID="txtBuyInOneClickPhone" Width="300px" /></td>
                        <td><asp:CheckBox runat="server" ID="chkIsShowBuyInOneClickPhone" /></td>
                        <td><asp:CheckBox runat="server" ID="chkIsRequiredBuyInOneClickPhone" /></td>
                    </tr>
                    <tr>
                        <td><asp:TextBox  runat="server" ID="txtBuyInOneClickComment" Width="300px" /></td>
                        <td><asp:CheckBox runat="server" ID="chkIsShowBuyInOneClickComment" /></td>
                        <td><asp:CheckBox runat="server" ID="chkIsRequiredBuyInOneClickComment" /></td>
                    </tr>
                </table>
                
                <br/>
                <br/>
                <br/>

            </div>
        </div>
    </div>
</asp:Content>
