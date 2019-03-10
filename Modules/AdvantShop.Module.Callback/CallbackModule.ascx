<%@ Control Language="C#" AutoEventWireup="true" Inherits="AdvantShop.Module.Callback.Admin_CallbackModule" Codebehind="CallbackModule.ascx.cs" %>
<div style="text-align: center;">
    <table border="0" cellpadding="2" cellspacing="0">
        <tr class="rowsPost">
            <td colspan="2" style="height: 34px;">
                <span class="spanSettCategory">
                    <asp:Localize ID="Localize9" runat="server" Text="<%$ Resources: Callback_Header%>" /></span>
                <asp:Label ID="lblMessage" runat="server" Visible="False" Style="float: right;"></asp:Label>
                <hr color="#C2C2C4" size="1px" />
            </td>
        </tr>
         <tr class="rowsPost" runat="server" id="tdCreateLead">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="lclCreateLead" runat="server" Text="<%$ Resources: Callback_CreateLead%>"></asp:Localize>
            </td>
            <td>
                <asp:CheckBox ID="ckbCreateLead" runat="server"/>
                
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize7" runat="server" Text="<%$ Resources: Callback_ShowCommentField%>"></asp:Localize>
            </td>
            <td>
                <asp:CheckBox ID="ckbShowCommentField" runat="server"/>
                
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources: Callback_Email%>"></asp:Localize>
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtEmail" Width="200px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtEmail" Text="*" ForeColor="Red"/>
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize3" runat="server" Text="<%$ Resources: Callback_MailSubject%>"></asp:Localize>
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtMailSubject" Width="200px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtMailSubject" Text="*" ForeColor="Red"/>
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize2" runat="server" Text="<%$ Resources: Callback_MailFormat%>"></asp:Localize><br />
                <asp:Localize ID="Localize6" runat="server" Text="<%$ Resources: Callback_UseVariables%>"></asp:Localize>
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtMailFormat" TextMode="MultiLine" Width="200px" Height="100px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtMailFormat" Text="*" ForeColor="Red"/>
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize4" runat="server" Text="<%$ Resources: Callback_WindowTitle%>"></asp:Localize>
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtWindowTitle" Width="200px"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtWindowTitle" Text="*" ForeColor="Red"/>
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
                <asp:Localize ID="Localize5" runat="server" Text="<%$ Resources: Callback_WindowText%>"></asp:Localize>
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtWindowText" TextMode="MultiLine" Width="200px" Height="100px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtWindowText" Text="*" ForeColor="Red"/>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="<%$ Resources: Callback_Save%>" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label runat="server" ID="lblErr" Visible="False"></asp:Label>
            </td>
        </tr>
    </table>
</div>