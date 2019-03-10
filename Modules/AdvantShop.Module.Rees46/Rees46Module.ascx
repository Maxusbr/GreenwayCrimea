<%@ Control Language="C#" AutoEventWireup="true" Inherits="AdvantShop.Module.Rees46.Admin_Rees46Module" CodeBehind="Rees46Module.ascx.cs" %>
<div style="text-align: center;">
    <asp:UpdatePanel runat="server" ID="updPanel" UpdateMode="Conditional">
        <contenttemplate>
        <asp:Panel runat="server" ID="settingsRees46">
            <table class="info-tb">
                <tr class="rowsPost">
                    <td colspan="2" style="height: 34px;">
                        <span class="spanSettCategory">
                            <asp:Localize ID="Localize9" runat="server" Text="<%$ Resources: Rees46_Header%>" /></span>
                        <asp:Label ID="lblMessage" runat="server" Visible="False" Style="float: right;"></asp:Label>
                        <hr color="#C2C2C4" size="1px" />
                    </td>
                </tr>
                <tr class="rowsPost">
                    <td style="width: 215px; text-align: left;">
                        <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources: Rees46_ShopKey%>"></asp:Localize>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtShopKey" Width="300px" />
                    </td>
                </tr>
                <tr class="rowsPost">
                    <td style="width: 215px; text-align: left; padding: 15px 0" colspan="2">
                        <asp:Literal runat="server" ID="lblRecommenderNotice" Mode="PassThrough" />
                    </td>
                </tr>
                <tr class="rowsPost">
                    <td style="width: 215px; text-align: left;">
                        <asp:Localize ID="locRelatedProduct" runat="server" />
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlRelatedProduct" Width="200px" DataTextField="Text" DataValueField="Value" />
                    </td>
                </tr>
                <tr class="rowsPost">
                    <td style="width: 215px; text-align: left;">
                        <asp:Localize ID="locAlternativeProduct" runat="server" />
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlAlternativeProduct" Width="200px" DataTextField="Text" DataValueField="Value" />
                    </td>
                </tr>

                <tr class="rowsPost">
                    <td style="width: 215px; text-align: left;">
                        <asp:Localize ID="loc1" Text="<%$ Resources: Rees46_MainPage%>" runat="server" />
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlMainPage" Width="200px" DataTextField="Text" DataValueField="Value" />
                    </td>
                </tr>
                <tr class="rowsPost">
                    <td style="width: 215px; text-align: left;">
                        <asp:Localize ID="Localize4" runat="server" Text="<%$ Resources: Rees46_CatalogTop%>" />
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlCatalogTopBlock" Width="200px" DataTextField="Text" DataValueField="Value" />
                    </td>
                </tr>
                <tr class="rowsPost">
                    <td style="width: 215px; text-align: left;">
                        <asp:Localize ID="Localize5" runat="server" Text="<%$ Resources: Rees46_CatalogBottom%>" />
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlCatalogBottomBlock" Width="200px" DataTextField="Text" DataValueField="Value" />
                    </td>
                </tr>
                <tr class="rowsPost">
                    <td style="width: 215px; text-align: left;">
                        <asp:Localize ID="Localize3" runat="server" Text="<%$ Resources: Rees46_DisplayInShoppingCart%>" />
                    </td>
                    <td>
                        <asp:CheckBox runat="server" ID="chkDisplayInShoppingCart" />
                    </td>
                </tr>

                <tr class="rowsPost">
                    <td style="width: 215px; text-align: left;">
                        <asp:Localize ID="Localize2" runat="server" Text="<%$ Resources: Rees46_Limit%>" />
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtLimit" Text="8" Width="50px" />
                    </td>
                </tr>
                <tr class="rowsPost">
                    <td style="width: 215px; text-align: left;">
                        <asp:Localize ID="Localize6" runat="server" Text="<%$ Resources: Rees46_FilePathPushSw%>" />
                    </td>
                    <td>
                        <asp:TextBox runat="server" ReadOnly="True" ID="txtUrlPushSw" Text="8" Width="300px" />
                    </td>
                </tr>
                <tr class="rowsPost">
                    <td style="width: 215px; text-align: left;">
                        <asp:Localize ID="Localize7" runat="server" Text="<%$ Resources: Ress46_ManifestJson%>" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtManifest" runat="server" TextMode="MultiLine" class="niceTextBox textBoxClass" Style="width: 300px; height: 500px" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="<%$ Resources: Rees46_Save%>" />
                        <asp:Button ID="btnMoveReg" runat="server" OnClick="btnMoveReg_Click" Text="Перейти на форму регистрации" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel runat="server" ID="autorizeRees46">
            <table class="info-tb">
                <tr class="rowsPost">
                    <td colspan="2" style="height: 34px;">
                        <span class="spanSettCategory">
                            <asp:Localize ID="Localize11" runat="server" Text="<%$ Resources: Rees46_Header%>" /></span>
                        <asp:Label ID="Label1" runat="server" Visible="False" Style="float: right;"></asp:Label>
                        <hr color="#C2C2C4" size="1px" />
                    </td>
                </tr>
                <tr class="rowsPost">
                    <td style="width: 215px; text-align: left;">
                        <asp:Localize ID="Localize8" runat="server" Text="<%$ Resources: Ress46_Registered%>" />
                    </td>
                    <td>
                        <asp:RadioButtonList runat="server" ID="dblRegisteredShop" OnClick="changeTypeReg()" class="niceTextBox textBoxClass dblRegisteredShop" Style="width: 300px; border: 0px solid gray;"></asp:RadioButtonList>
                    </td>
                </tr>
                <tr class="rowsPost regs" style="display: none;">
                    <td style="width: 215px; text-align: left;">
                        <asp:Localize ID="Localize10" runat="server" Text="Почтовый адрес"></asp:Localize>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtEmail" Width="300px" />
                    </td>
                </tr>
                <tr class="rowsPost regs" style="display: none;">
                    <td style="width: 215px; text-align: left;">
                        <asp:Localize ID="Localize12" runat="server" Text="Телефон(11 знаков)"></asp:Localize>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtPhone" Width="300px" />
                    </td>
                </tr>
                <tr class="rowsPost regs" style="display: none;">
                    <td style="width: 215px; text-align: left;">
                        <asp:Localize ID="Localize13" runat="server" Text="Ваше имя"></asp:Localize>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtFirstName" Width="300px" />
                    </td>
                </tr>
                <tr class="rowsPost regs" style="display: none;">
                    <td style="width: 215px; text-align: left;">
                        <asp:Localize ID="Localize14" runat="server" Text="Ваша фамилия"></asp:Localize>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtlastName" Width="300px" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="btnRegister" runat="server" OnClick="btnReg_Click" Text="<%$ Resources: Rees46_Confirm%>" />
                        <div data-plugin="help" class="help-block regs" style="display: none;">
                                <div class="help-icon js-help-icon"></div>
                                <article class="bubble help js-help">
                                    <header class="help-header">
                                        Уведомление
                                    </header>
                                    <div class="help-content">
                                        Процесс регистрации займет около минуты,<br>
                                        пожалуйста ожидайте.
                                    </div>
                                </article>
                        </div>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Label runat="server" ID="lblErr" Visible="False"></asp:Label>
            </contenttemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">
        var count = 0
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            if (count > 0) {
                Advantshop.ScriptsManager.Help.prototype.InitTotal();
            }
            count = 1;
        });

        function changeTypeReg() {
            var table = document.getElementsByClassName('dblRegisteredShop');
            var inp = table[0].querySelector('input[type="radio"]:checked');
            if (inp == null)
                return;
            var select = inp.value;
            var change = document.getElementsByClassName('regs');
            var i = 0;
            for (i; i < change.length; i++) {
                change[i].style.display = select === "No" || select === "Нет" ? "" : "none";
            }
        }

    </script>
</div>
