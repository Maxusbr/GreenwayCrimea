<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Settings.ascx.cs"
    Inherits="AdvantShop.Module.RetailCRM.Settings" %>
<%@ Import Namespace="AdvantShop.Modules" %>
<div style="text-align: center;">
    <table border="0" cellpadding="2" cellspacing="0">
        <tr class="rowsPost">
            <td colspan="2" style="height: 34px;">
                <span class="spanSettCategory">Подключение</span>
                <asp:Label ID="lblMessage" runat="server" Visible="False" Style="float: right;"></asp:Label>
                <hr color="#C2C2C4" size="1px" />
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="width: 250px; text-align: left;">
                <asp:Localize ID="Localize1" runat="server" Text="Домен"></asp:Localize>
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtSubdomain" Width="300px" placeholder="ваша-компания.retailcrm.ru"></asp:TextBox>
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="width: 250px; text-align: left;">
                <asp:Localize ID="Localize2" runat="server" Text="Api Key"></asp:Localize>
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtApiKey" Width="300px"></asp:TextBox>
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="width: 250px; text-align: left;">
                <asp:Localize ID="Localize5" runat="server" Text="Ключ интеграции с Collector"></asp:Localize>
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtCollectorKey" Width="300px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="btnApply" runat="server" OnClick="btnApply_Click" Text="Применить" />
                <br />
                <br />
            </td>
        </tr>
    </table>
    <div runat="server" id="tblSettings">
        <table>
            <tr class="rowsPost">
                <td colspan="2" style="height: 34px;">
                    <span class="spanSettCategory">Сайт для синхронизации</span>
                    <hr color="#C2C2C4" size="1px" />
                </td>
            </tr>
            <tr class="rowsPost">
                <td style="width: 250px; text-align: left;">
                    <asp:Localize ID="Localize3" runat="server" Text="Сайт"></asp:Localize>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlSites" Width="300px">
                        <Items>
                            <asp:ListItem Text="Выберите сайт для синхронизации" Value="0"></asp:ListItem>
                        </Items>
                    </asp:DropDownList>
                </td>
            </tr>

            <tr class="rowsPost">
                <td style="width: 250px; text-align: left;">Режим отправки заказов в retailCRM
                </td>
                <td>
                    <asp:RadioButtonList ID="rblOrderSendingMode" runat="server" CssClass="retailRadio">
                        <asp:ListItem Value="Always" Selected="True">При создании и при изменении заказа</asp:ListItem>
                        <asp:ListItem Value="OnCreating">Только при создании заказа</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>

            <tr class="rowsPost">
                <td style="width: 250px; text-align: left;">Валюта
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlCurrency" />
                </td>
            </tr>

            <tr class="rowsPost">
                <td colspan="2" style="height: 34px;">
                    <span class="spanSettCategory">Выгрузка каталога</span>
                    <asp:Label ID="Label2" runat="server" Visible="False" Style="float: right;"></asp:Label>
                    <hr color="#C2C2C4" size="1px" />
                </td>
            </tr>

            <tr class="rowsPost">
                <td style="width: 250px; text-align: left;">Файл для экспорта каталога
                </td>
                <td>
                    <asp:HyperLink runat="server" ID="lblFile" Target="_blank"></asp:HyperLink>
                    (<asp:Label runat="server" ID="lblDate"></asp:Label>)
                </td>
            </tr>
            <tr class="rowsPost">
                <td style="width: 250px; text-align: left;">
                    Выгружать в тег Id, артикул или id товара
                </td>
                <td>
                    <asp:RadioButtonList ID="rblArtNoType" runat="server">
                                    <asp:ListItem Value="ID" Selected="True">Выгружать id модификации</asp:ListItem>
                                    <asp:ListItem Value="ArtNo">Выгружать артикул модификации</asp:ListItem>
                                </asp:RadioButtonList>
                </td>
            </tr>
            <tr class="rowsPost">
                <td style="width: 250px; text-align: left;">
                    <asp:Button ID="btnExport" runat="server" OnClick="btnExport_Click" Text="Обновить файл" />
                    <br />
                    <br />
                </td>
                <td></td>
            </tr>

            <tr class="rowsPost">
                <td colspan="2" style="height: 34px;">
                    <span class="spanSettCategory">Новые статусы</span>
                    <asp:Label ID="Label1" runat="server" Visible="False" Style="float: right;"></asp:Label>
                    <hr color="#C2C2C4" size="1px" />
                </td>
            </tr>

            <tr class="rowsPost">
                <td style="width: 250px; text-align: left;">Группа статусов по умолчанию (в нее будут добавляться новые статусы из магазина)<br />
                    <br />
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlDefaultStatusCategory" Width="300px" DataTextField="name" DataValueField="code" /><br />
                </td>
            </tr>

            <tr class="rowsPost">
                <td colspan="2" style="height: 34px;">
                    <span class="spanSettCategory">Соответствие статусов</span>
                    <asp:Label ID="Label3" runat="server" Visible="False" Style="float: right;"></asp:Label>
                    <hr color="#C2C2C4" size="1px" />
                </td>
            </tr>

            <tr class="rowsPost">
                <td style="width: 250px; text-align: left;">Статус с интернет-магазине
                </td>
                <td>Cтатус в retailCRM
                </td>
            </tr>
            <asp:ListView runat="server" OnItemDataBound="OnItemDataBound" ID="lvStatuses">
                <itemtemplate>
                <tr class="rowsPost">
                    <td style="width: 250px; text-align: left;">
                        <asp:Localize ID="Localize4" runat="server" Text='<%# Eval("StatusName") %>'></asp:Localize>
                        <asp:HiddenField runat="server" ID="hfStatusID" Value='<%#Eval("StatusID") %>' />
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlStatuses" Width="300px" DataTextField="name" DataValueField="code" />
                    </td>
                </tr>
            </itemtemplate>
            </asp:ListView>
            <tr>
                <td colspan="2">
                    <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Сохранить" />
                    <br />
                    <br />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label runat="server" ID="lblErr" Visible="False"></asp:Label>
                </td>
            </tr>


            <tr class="rowsPost">

                <td colspan="2" style="height: 34px;">
                    <span class="spanSettCategory">Синхронизация старых заказов</span>
                    <asp:Label ID="Label4" runat="server" Visible="False" Style="float: right;"></asp:Label>
                    <hr color="#C2C2C4" size="1px" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:LinkButton ID="btnSync" runat="server" OnClick="btnSync_Click" Text="Отправить старые заказы в retailCRM" Width="300px" />
                    <asp:Button ID="Button1" runat="server" OnClick="Button1_OnClick" Text="Получить данные о заказах за неделю" Width="300px" Style="display: none" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <span id="log"></span>
                    <br />
                </td>
            </tr>
            <tr class="rowsPost">
                <td colspan="2" style="height: 34px;">
                    <span class="spanSettCategory">Логи событий</span>
                    <hr color="#C2C2C4" size="1px" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:ListView runat="server" ID="lvLogs" ItemPlaceholderID="itemPlaceHolder">
                        <layouttemplate>
                        <ul style="list-style-type: none; padding: 0; margin: 0;">
                            <li id="itemPlaceHolder" runat="server"></li>
                        </ul>
                    </layouttemplate>
                        <itemtemplate>
                        <li>
                            <%# Container.DataItemIndex + 1 %>. 
                            <a href="<%# "../Modules/" + RetailCRMModule.ModuleStringId + "/log/" + Container.DataItem %>" target="_blank">
                                <%# Container.DataItem %>
                            </a>
                        </li>
                    </itemtemplate>
                        <emptydatatemplate>
                        Нет данных
                    </emptydatatemplate>
                    </asp:ListView>
                </td>
            </tr>
        </table>
    </div>
    <% if (Request["Ping"] == "true")
       { %>
    <script>
        setInterval(function () {
            $.ajax({
                url: "<%= AdvantShop.Core.UrlRewriter.UrlService.GenerateBaseUrl()  + Request.RawUrl + "&message=true"%>",
                cache: false,
                success: function (data) {
                    $("#log").html(data);
                }
            });
        }, 3000);
    </script>
    <% } %>
</div>
