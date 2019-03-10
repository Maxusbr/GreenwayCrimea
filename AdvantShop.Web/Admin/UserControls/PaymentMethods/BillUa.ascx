<%@ Control Language="C#" AutoEventWireup="true" Inherits="Admin.UserControls.PaymentMethods.BillUaControl" Codebehind="BillUa.ascx.cs" %>
<table border="0" cellpadding="2" cellspacing="0" style="width: 99%; margin-left: 5px;
    margin-top: 5px;">
    <tr class="rowPost">
        <td colspan="3" style="height: 34px;">
            <h4 style="display: inline; font-size: 12pt;">
                <asp:Localize ID="Localize13" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethods_HeadSettings %>"></asp:Localize></h4>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize runat="server" Text="Название компании"></asp:Localize><span class="required">&nbsp;*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtCompanyName" Width="250" ></asp:TextBox>
        </td>
        <td class="columnDescr">
            <asp:Label runat="server" ID="msgCompanyName" Visible="false" ForeColor="Red"></asp:Label>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize1" runat="server" Text="Код"></asp:Localize><span class="required">&nbsp;*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtCompanyCode" Width="250" ></asp:TextBox>
        </td>
        <td class="columnDescr">
            <asp:Label runat="server" ID="msgCompanyCode" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize2" runat="server" Text="Название банка"></asp:Localize><span class="required">&nbsp;*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtBankName" Width="250" ></asp:TextBox>
        </td>
        <td class="columnDescr">
            <asp:Label runat="server" ID="msgBankName" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize5" runat="server" Text="МФО банка"></asp:Localize><span class="required">&nbsp;*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtBankCode" Width="250" ></asp:TextBox>
        </td>
        <td class="columnDescr">
            <asp:Label runat="server" ID="msgBankCode" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize3" runat="server" Text="Кредитный номер"></asp:Localize><span class="required">&nbsp;*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtCredit" Width="250" ></asp:TextBox>
        </td>
        <td class="columnDescr">
            <asp:Label runat="server" ID="msgCredit" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize4" runat="server" Text="Номер счёта"></asp:Localize><span class="required">&nbsp;*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtCompanyEssentials" Width="250" ></asp:TextBox>
        </td>
        <td class="columnDescr">
            <asp:Label runat="server" ID="msgCompanyEssentials" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>    
</table>
