<%@ Control Language="C#" AutoEventWireup="true" Inherits="Admin.UserControls.ShippingMethods.ShippingByRangeWeightAndDistanceControl" Codebehind="ShippingByRangeWeightAndDistance.ascx.cs" %>
<%@ Import Namespace="AdvantShop.Catalog" %>
<%@ Import Namespace="AdvantShop.Core.Services.Catalog" %>
<table border="0" cellpadding="2" cellspacing="0" style="width: 99%; margin-left: 5px; margin-top: 5px;">
    <tr class="rowPost">
        <td colspan="3" style="height: 34px;">
            <h4 style="display: inline; font-size: 12pt;">
                <asp:Localize ID="Localize13" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethods_HeadSettings %>"></asp:Localize></h4>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <span><%= Resources.Resource.Admin_ShippingMethod_ShippingByRangeWeightAndDistance_BasePrice %></span>
        </td>
        <td class="columnVal" style="width: 500px">
            <table border="0" cellpadding="2" cellspacing="0">
                <tr>
                    <td style="width:120px;">
                        <%= Resources.Resource. Admin_ShippingMethod_ShippingByRangeWeightAndDistance_UpTo %>:
                        <asp:TextBox runat="server" ID="txtAmount" Width="60" ValidationGroup="byPrice" />
                        <%= Resources.Resource.Admin_ShippingMethod_ShippingByRangeWeightAndDistance_Kg %>
                    </td>
                    <td style="width:150px;">
                        <%= Resources.Resource.Admin_ShippingMethod_ShippingByRangeWeightAndDistance_Price %>:
                        <asp:TextBox runat="server" ID="txtPrice" Width="60" ValidationGroup="byPrice" />
                    </td>
                    <td style="width:60px;">
                        <%= Resources.Resource.Admin_ShippingMethod_ShippingByRangeWeightAndDistance_WeightPerUnit %>
                        <asp:CheckBox runat="server" ID="chbShippingPerUnit" />
                    </td>
                    <td style="text-align:center;">
                        <asp:Button runat="server" ID="btnAdd" OnClick="btnAdd_Click" Text="<%$ Resources:Resource, Admin_ShippingMethod_ShippingByOrderPrice_Add %>"
                            CssClass="btn btn-middle btn-action" ValidationGroup="byPrice" />
                    </td>
                </tr>
                <asp:Repeater runat="server" ID="rWeightLimits" OnItemCommand="rWeightLimits_Delete">
                    <ItemTemplate>
                        <tr style="height:25px;">
                            <td style="border-right-style:solid; border-right-width:1px; border-right-color:black; width:120px;">
                                <%= Resources.Resource.Admin_ShippingMethod_ShippingByRangeWeightAndDistance_UpTo %>: <%#Eval("Amount") %> <%= Resources.Resource.Admin_ShippingMethod_ShippingByRangeWeightAndDistance_Kg %>
                            </td>
                            <td style="border-right-style:solid; border-right-width:1px; border-right-color:black; text-align:center; width:150px;">
                                <%# PriceFormatService.FormatPrice((float)Eval("Price")) %>
                            </td>
                            <td style="border-right-style:solid; border-right-width:1px; border-right-color:black; text-align:center; width:60px;">
                                <%# !(bool)Eval("PerUnit") ? "" : Resources.Resource.Admin_ShippingMethod_ShippingByRangeWeightAndDistance_WeightPerUnit %>
                            </td>
                            <td style="text-align:center;">
                                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="../../images/remove.jpg" ToolTip="<%$ Resources:Resource, Admin_ShippingMethod_ShippingByOrderPrice_Delete %>"
                                    CommandName="DeleteWeightLimits" CommandArgument='<%# Container.ItemIndex %>' CausesValidation="false" />
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </td>
        <td class="columnDescr">&nbsp;</td>
    </tr>
    <tr>
        <td class="columnName"><%= Resources.Resource.Admin_ShippingMethod_ShippingByRangeWeightAndDistance_ConsideringTheDistance %>
        </td>
        <td class="columnName">
            <asp:CheckBox runat="server" ID="chbUseDistance" />
        </td>
        <td class="columnDescr">&nbsp;</td>
    </tr>
        <tr>
        <td class="columnName"><%= Resources.Resource.Admin_ShippingMethod_ShippingByRangeWeightAndDistance_MaxDistance %>
        </td>
        <td class="columnName">
            <asp:TextBox runat="server" ID="txtMaxDistance" Width="60" ValidationGroup="byPrice2"/>
        </td>
        <td class="columnDescr">&nbsp;</td>
    </tr>
    <tr>
        <td class="columnName"><%= Resources.Resource.Admin_ShippingMethod_ShippingByRangeWeightAndDistance_DepensOnDistance %>
        </td>
        <td class="columnVal" style="width: 500px">
            <table border="0" cellpadding="2" cellspacing="0">
                <tr>
                    <td style="width:120px;">
                        <%= Resources.Resource.Admin_ShippingMethod_ShippingByRangeWeightAndDistance_UpTo %>:
                        <asp:TextBox runat="server" ID="txtDistanse" Width="60" ValidationGroup="byPrice2" />
                        <%= Resources.Resource.Admin_ShippingMethod_ShippingByRangeWeightAndDistance_Km %>
                    </td>
                    <td style="width:150px;">
                        <%= Resources.Resource.Admin_ShippingMethod_ShippingByRangeWeightAndDistance_Price %>:
                        <asp:TextBox runat="server" ID="txtDistansePrice" Width="60" ValidationGroup="byPrice2" />
                    </td>
                    <td style="width:60px;">
                        <%= Resources.Resource.Admin_ShippingMethod_ShippingByRangeWeightAndDistance_PerUnit %>
                        <asp:CheckBox runat="server" ID="chbPerUnit" />
                    </td>
                    <td style="text-align:center;">
                        <asp:Button runat="server" ID="btnAddD" OnClick="btnAddD_Click" Text="<%$ Resources:Resource, Admin_ShippingMethod_ShippingByOrderPrice_Add %>"
                            CssClass="btn btn-middle btn-action" ValidationGroup="byPrice2" />
                    </td>
                </tr>
                <asp:Repeater runat="server" ID="rDistanceLimits" OnItemCommand="rDistanceLimits_Delete">
                    <ItemTemplate>
                        <tr style="height:25px;">
                            <td style="border-right-style:solid; border-right-width:1px; border-right-color:black; width:120px;">
                                <%= Resources.Resource.Admin_ShippingMethod_ShippingByRangeWeightAndDistance_UpTo %>: <%#Eval("Amount") %> <%= Resources.Resource.Admin_ShippingMethod_ShippingByRangeWeightAndDistance_Km %>
                            </td>
                            <td style="border-right-style:solid; border-right-width:1px; border-right-color:black; text-align:center; width:150px;">
                                <%# PriceFormatService.FormatPrice((float)Eval("Price")) %>
                            </td>
                            <td style="border-right-style:solid; border-right-width:1px; border-right-color:black; text-align:center; width:60px;">
                                <%# !(bool)Eval("PerUnit") ? "" : Resources.Resource.Admin_ShippingMethod_ShippingByRangeWeightAndDistance_PerUnit %>
                            </td>
                            <td style="text-align:center;">
                                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="../../images/remove.jpg" ToolTip="<%$ Resources:Resource, Admin_ShippingMethod_ShippingByOrderPrice_Delete %>"
                                    CommandName="DeleteWeightLimits" CommandArgument='<%# Container.ItemIndex %>' CausesValidation="false" />
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </td>
        <td class="columnDescr">&nbsp;</td>
    </tr>
</table>
<table border="0" cellpadding="2" cellspacing="0" style="width: 99%; margin-left: 5px; margin-top: 10px">
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources:Resource, Admin_ShippingMethod_ShippingTerm %>"></asp:Localize><span
                class="required">&nbsp;*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtDeliveryTime" Width="250" ValidationGroup="5"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        <%= Resources.Resource.Admin_ShippingMethod_ShippingTerm%>
                    </header>
                    <div class="help-content">
                        Срок доставки, как отрезок времени, в который данный метод доставки сможет доставить товар.<br />
                        <br />
                        Например: 1-2 дня.
                    </div>
                </article>
            </div>
        </td>
    </tr>
</table>
<table border="0" cellpadding="2" cellspacing="0" style="width: 99%; margin-left: 5px; margin-top: 10px">
    <tr class="rowPost">
        <td colspan="3" style="height: 34px">
            <h4 style="display: inline; font-size: 12pt">
                <asp:Localize ID="Localize7" runat="server" Text="<%$ Resources:Resource, Admin_ShippingMethod_CargoParams %>"></asp:Localize></h4>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize3" runat="server" Text="<%$ Resources:Resource, Admin_ShippingMethod_CargoParams_Weight %>"></asp:Localize><span
                class="required">*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtWeight" Width="250" Text="1" ValidationGroup="5"></asp:TextBox>
            <asp:RangeValidator ID="RangeValidator1" Type="Double" MinimumValue="1" MaximumValue="1000000"
                runat="server" ControlToValidate="txtWeight" EnableClientScript="True" Display="Static"></asp:RangeValidator>
        </td>
        <td class="columnDescr">
            <div data-plugin="help" class="help-block">
                <div class="help-icon js-help-icon"></div>
                <article class="bubble help js-help">
                    <header class="help-header">
                        Вес товара
                    </header>
                    <div class="help-content">
                        Вес товара примет указанное значение, если у товара данный параметр не был задан.<br />
                        <br />
                        Значение указывается в кг, возможно указать дробное значение.<br />
                        <br />
                        Например: 1 или 0.2 (кг.)
                    </div>
                </article>
            </div>
            <asp:Label runat="server" ID="Label1" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
</table>
<br />