<%@ Control Language="C#" AutoEventWireup="true" Inherits="Admin.UserControls.ShippingMethods.FreeShippingControl" CodeBehind="FreeShipping.ascx.cs" %>
<table border="0" cellpadding="2" cellspacing="0" style="width: 99%; margin-left: 5px; margin-top: 5px;">
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
