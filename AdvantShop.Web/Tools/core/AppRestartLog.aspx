<%@ Page Language="C#" AutoEventWireup="true" Inherits="Tools.core.AppRestartLog"
    MasterPageFile="MasterPage.master" Codebehind="AppRestartLog.aspx.cs" %>

<asp:Content runat="server" ID="cntHead" ContentPlaceHolderID="head">
    <title>AdvantShop.NET Core Tools</title>
</asp:Content>
<asp:Content runat="server" ID="cntMain" ContentPlaceHolderID="main">
    <fieldset>
        <asp:Button ID="ShowLogButton" runat="server" OnClick="ShowLogButton_Click" Text="Show application restart log" />
        <asp:Button ID="DeleteLogButton" runat="server" OnClick="DeleteLogButton_Click" Text="DELETE all entries" />
    </fieldset>
    <fieldset id="dataBlock" runat="server" visible="false">
        <asp:Repeater ID="LogRepeater" runat="server">
            <HeaderTemplate>
                <table style="background-color: White; border: 1px none #CCCCCC; width: 99%; border-collapse: collapse;">
                    <tr style="color: White; background-color: #006699; font-weight: bold;">
                        <th style="text-align: left;">
                            ID
                        </th>
                        <th style="text-align: left;">
                            Date
                        </th>
                        <th style="text-align: left;">
                            Server IP
                        </th>
                        <th style="text-align: left;">
                            ServerName
                        </th>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr style="color: #000066;">
                    <td style="border-style: solid; border-width: 1px;">
                        <%# Eval("EntryId")%>
                    </td>
                    <td style="border-style: solid; border-width: 1px;">
                        <%# Eval("EntryDate")%>
                    </td>
                    <td style="border-style: solid; border-width: 1px;">
                        <%# Eval("ServerIp") %>
                    </td>
                    <td style="border-style: solid; border-width: 1px;">
                        <%# Eval("ServerName")%>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
    </fieldset>
</asp:Content>
