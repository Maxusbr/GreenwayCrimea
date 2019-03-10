<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="AdvantShop.Module.JivoSite.Admin_JivoSiteModule" CodeBehind="JivoSiteModule.ascx.cs" %>
<div style="text-align: center;">
    <asp:UpdatePanel ID="upd" runat="server" ChildrenAsTriggers="true">
        <contenttemplate>
    <table border="0" cellpadding="2" cellspacing="0" style="width:800px">
        <tr class="rowsPost">
            <td colspan="2" style="height: 34px;">
                <span class="spanSettCategory">JivoSite</span>
                <asp:Label ID="lblMessage" runat="server" Visible="False" Style="float: right;"></asp:Label>
                <asp:Label runat="server" ID="lblErr" Visible="False" Style="float: right; color:red"></asp:Label>
                <hr color="#C2C2C4" size="1px" />
            </td>
        </tr>
    </table>
    
    <table runat="server" id="tblSelect">
        <tr class="rowsPost account-register">
            <td style="text-align: left; width: 250px;">Аккаунт
            </td>
            <td>
                <asp:RadioButtonList runat="server" ID="rbl" AutoPostBack="true" OnSelectedIndexChanged="rbl_SelectedIndexChanged">
                    <asp:ListItem Text="Зарегистрироваться" Value="reg" />
                    <asp:ListItem Text="Уже есть аккаунт" Value="account" />
                </asp:RadioButtonList>
                <br/>
                <br/>
            </td>
        </tr>
    </table>
    <table runat="server" id="tblReg">
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">E-mail для регистрации
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtEmail"></asp:TextBox>
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">Пароль
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtPassword"></asp:TextBox>
            </td>
        </tr>
        <tr class="rowsPost">
            <td colspan="2">
                Запомните или запишите эти данные. Они потребуются для входа в кабинет JivoSite и авторизации в приложении оператора.
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">Имя оператора в чате
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtName"></asp:TextBox>
            </td>
        </tr>

        <tr>
            <td colspan="2">
                <asp:Button ID="btnSave" runat="server" OnClick="btnRegister_Click" Text="Зарегистрироваться" />
            </td>
        </tr>
    </table>
    <table runat="server" id="tblAccount">
        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">Идентификатор виджета
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtWidgetID"></asp:TextBox>
            </td>
        </tr>

        <tr class="rowsPost">
            <td colspan="2">
               Для того, чтобы принимать чаты от посетителей на сайте, необходимо скачать и установить приложение оператора <br/> на свой компьютер или мобильный телефон, либо авторизоваться в веб-версии.<br/>
                <a href="http://www.jivosite.ru/apps" target="_blank">http://www.jivosite.ru/apps</a>
                <br/>
                <br/>
            </td>
        </tr>

       <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">URL адрес для webhooks<br/> необходимо прописать в кабинете JivoSite
            </td>
            <td>
                <asp:Label runat="server" ID="lblWebHook" />
            </td>
        </tr>

        <tr class="rowsPost">
            <td style="text-align: left; width: 250px;">
            </td>
            <td>
                <a href="https://admin.jivosite.com" target="_blank">Войти в кабинет JivoSite.ru</a>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="Button1" runat="server" OnClick="btnSave_Click" Text="Сохранить" />
                <asp:LinkButton  runat="server" ID="lbDeleteAcc" Text="Зарегистрировать новый аккаунт" OnClick="lbDeleteAcc_Click" />
            </td>
        </tr>
    </table>
        </contenttemplate>
    </asp:UpdatePanel>
</div>
