<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Tools.fm.FileManagerControl.FileManagerControl" Codebehind="FileManagerControl.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../DynamicTreeControl/DynamicTreeControl.ascx" TagName="dt" TagPrefix="dt" %>
<div id="FileManagerBlock">
    <asp:UpdatePanel ID="FileManagerUpdatePanel" runat="Server">
        <ContentTemplate>
            <div id="fileManagerControlBlock" style="width: 100%">
                <div id="fileManagerMenu">
                    <div class="menuCell" onmouseover="onMenuCellMouseOver(this)" onmouseout="onMenuCellMouseLeft(this)">
                        <asp:LinkButton ID="FileManagerUpdateButton" OnClick="FileManagerUpdateButton_ClickHandler"
                            runat="Server">Обновить</asp:LinkButton>
                    </div>
                    <div class="verticalSeparator">
                        &nbsp;</div>
                    <div class="menuCell" onmouseover="onMenuCellMouseOver(this)" onmouseout="onMenuCellMouseLeft(this)">
                        <asp:LinkButton ID="FileManagerUploadButton" OnClick="FileManagerUploadButton_ClickHandler"
                            runat="Server">Загрузить</asp:LinkButton>
                    </div>
                    <div class="verticalSeparator">
                        &nbsp;</div>
                    <div class="menuCell" onmouseover="onMenuCellMouseOver(this)" onmouseout="onMenuCellMouseLeft(this)">
                        <asp:LinkButton ID="FileManagerRenameButton" OnClick="FileManagerRenameButton_ClickHandler"
                            runat="Server">Переименовать файл</asp:LinkButton>
                    </div>
                    <div class="verticalSeparator">
                        &nbsp;</div>
                    <div class="menuCell" onmouseover="onMenuCellMouseOver(this)" onmouseout="onMenuCellMouseLeft(this)">
                        <asp:LinkButton ID="FileManagerDeleteButton" OnClick="FileManagerDeleteButton_ClickHandler" CssClass="valid-confirm"
                            data-confirm="Вы действительно хотите удалить эти файли и папки?"
                            runat="Server">Удалить</asp:LinkButton>
                    </div>
                    <%--<div class="verticalSeparator">&nbsp;</div>--%>
                    <div class="menuCell" onmouseover="onMenuCellMouseOver(this)" onmouseout="onMenuCellMouseLeft(this)">
                        <asp:LinkButton ID="fileManagerCopyButton" OnClick="FileManagerCopyButton_ClickHandler"
                            runat="Server">Копировать</asp:LinkButton>
                    </div>
                    <%--<div class="verticalSeparator">&nbsp;</div>--%>
                    <div class="menuCell" onmouseover="onMenuCellMouseOver(this)" onmouseout="onMenuCellMouseLeft(this)">
                        <asp:LinkButton ID="FileManagerMoveButton" OnClick="FileManagerMoveButton_ClickHandler"
                            runat="Server">Переместить</asp:LinkButton>
                    </div>
                    <div class="verticalSeparator">
                        &nbsp;</div>
                    <div class="menuCell" onmouseover="onMenuCellMouseOver(this)" onmouseout="onMenuCellMouseLeft(this)">
                        <asp:LinkButton ID="FileManagerCreateDirectoryButton" OnClick="FileManagerCreateDirectoryButton_ClockHandler"
                            runat="Server">Создать папку</asp:LinkButton>
                    </div>
                    <div class="verticalSeparator">
                        &nbsp;</div>
                    <div class="menuCell" onmouseover="onMenuCellMouseOver(this)" onmouseout="onMenuCellMouseLeft(this)">
                        <asp:LinkButton ID="FileManagerExtractZipArchiveButton" OnClick="FileManagerExtractZipArchiveButton_ClickHandler"
                            runat="server">Распаковать архив</asp:LinkButton>
                    </div>
                    <%--<div class="verticalSeparator">&nbsp;</div>--%>
                    <div class="menuCell" onmouseover="onMenuCellMouseOver(this)" onmouseout="onMenuCellMouseLeft(this)">
                        <asp:LinkButton ID="FileManagerAddItemsToZipButton" OnClick="FileManagerAddFoldersToZipButton_ClickHandler"
                            runat="Server">Добавить в архив</asp:LinkButton>
                    </div>
                    <div class="loadingBlock menuCell">
                        <asp:UpdateProgress AssociatedUpdatePanelID="FileManagerUpdatePanel" DisplayAfter="500"
                            DynamicLayout="false" runat="Server">
                            <ProgressTemplate>
                                <div class="loadingCell">
                                    Loading
                                </div>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </div>
                </div>
            </div>
            <div id="FileManagerControlHeader" class="fileManagerControlHeader" runat="Server">
            </div>
            <div id="fileManagerMainBlock">
                <table style="width: 100%">
                    <tr>
                        <td colspan="2">
                            <div style="height: 1px; width: 950px;">
                                &nbsp;</div>
                        </td>
                    </tr>
                    <tr style="vertical-align: top">
                        <td style="width: 250px; border-right: solid 1px black;">
                            <div id="fileManagerTree">
                                <asp:TreeView ID="FilemanagerTreeControl" EnableViewState="false" PathSeparator="\"
                                    NodeStyle-ForeColor="Black" SelectedNodeStyle-BackColor="GrayText" runat="Server"
                                    OnSelectedNodeChanged="FileManagerFilemanagerTreeControl_SelectedNodeChanged_Handler" />
                            </div>
                        </td>
                        <td>
                            <div id="fileManagerFolderContent">
                                <asp:MultiView ID="ActionPanel" runat="Server" ActiveViewIndex="0">
                                    <asp:View ID="FolderContentView" runat="Server">
                                        <table id="fileManagerFolderContentTable" enableviewstate="false" class="fileManagerFolderContentTable"
                                            runat="Server">
                                            <tr class="fileManagerFolderContentTableHeader">
                                                <td class="fileManagerFolderContentTableSelectCell" style="border-bottom: solid 1px #a5a5a5;">
                                                    <input id="selectAllCheckBox" type="checkbox" onclick="SelectAll();" />
                                                </td>
                                                <td class="fileManagerFolderContentTableNameCell" style="border-bottom: solid 1px #a5a5a5;">
                                                    Имя
                                                </td>
                                                <td class="fileManagerFolderContentTableLengthCell" style="border-bottom: solid 1px #a5a5a5;">
                                                    Размер
                                                </td>
                                                <td class="fileManagerFolderContentTableLastModificationCell" style="border-bottom: solid 1px #a5a5a5;">
                                                    Дата изменения
                                                </td>
                                                <td class="fileManagerFolderContentTableDownloadCell" style="border-bottom: solid 1px #a5a5a5;">
                                                    &nbsp;
                                                </td>
                                                <td class="fileManagerFolderContentTableDeleteCell" style="border-bottom: solid 1px #a5a5a5;">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:View>
                                    <asp:View ID="UploadView" runat="Server">
                                        <div id="fileManagerUploadPanel" runat="Server" class="uploadPanel">
                                            <div style="border-bottom: solid 1px #000000;">
                                                Загрузка файла
                                            </div>
                                            <div style="margin-top: 5px;">
                                                <asp:FileUpload ID="FileManagerUpload1" runat="Server" /><br />
                                                <asp:FileUpload ID="FileManagerUpload2" runat="Server" /><br />
                                                <asp:FileUpload ID="FileManagerUpload3" runat="Server" /><br />
                                                <asp:FileUpload ID="FileManagerUpload4" runat="Server" /><br />
                                            </div>
                                            <div class="buttonsBlock">
                                                <asp:Button ID="UploadButton" Text="Загрузить" OnClick="UploadButton_ClickHandler"
                                                    runat="server" />&nbsp;
                                                <asp:Button ID="CancelUploadButton" Text="Отменить" OnClick="CancelUploadButton_ClickHandler"
                                                    runat="Server" />
                                            </div>
                                        </div>
                                    </asp:View>
                                    <asp:View ID="MoveView" runat="Server">
                                        <asp:UpdatePanel ID="FileManagerMoveUpdatePanel" runat="Server">
                                            <ContentTemplate>
                                                <div id="fileManagerMovePanel" runat="Server" class="movePanel">
                                                    <div style="border-bottom: solid 1px #000000;">
                                                        Перемещение</div>
                                                    <div id="fileManagerMoveBlock" runat="server">
                                                        <dt:dt ID="moveTree" runat="server" />
                                                    </div>
                                                    <div class="buttonsBlock">
                                                        <asp:Button ID="MoveButton" Text="Переместить" OnClick="MoveButton_ClickHandler"
                                                            runat="server" />&nbsp;
                                                        <asp:Button ID="CancelMoveButton" Text="Отменить" OnClick="CancelMoveButton_ClickHandler"
                                                            runat="Server" />
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </asp:View>
                                    <asp:View ID="CopyView" runat="Server">
                                        <div id="fileManagerCopyPanel" runat="Server" class="movePanel">
                                            <div style="border-bottom: solid 1px #000000;">
                                                Копирование</div>
                                            <div style="margin-top: 5px;" id="fileManagerCopyBlock" runat="server">
                                                <%--<asp:TreeView ID="FileManagerCopyTree" runat="server" PathSeparator="\" SelectedNodeStyle-BackColor="GrayText" />--%>
                                                <dt:dt ID="copyTree" runat="server" />
                                            </div>
                                            <div class="buttonsBlock">
                                                <asp:Button ID="CopyButton" Text="Копировать" OnClick="CopyButton_ClickHandler" runat="server" />&nbsp;
                                                <asp:Button ID="CancelCopyButton" Text="Отменить" OnClick="CancelCopyButton_ClickHandler"
                                                    runat="Server" />
                                            </div>
                                        </div>
                                    </asp:View>
                                    <asp:View ID="CreateDirectoryView" runat="Server">
                                        <div id="fileManagerCreateDirectoryPanel" runat="Server" class="movePanel">
                                            <div style="border-bottom: solid 1px #000000;">
                                                Создание папки</div>
                                            <div>
                                                <asp:TextBox ID="FileManagerDirectoryNameTextBox" EnableViewState="false" runat="Server" />
                                            </div>
                                            <div class="buttonsBlock">
                                                <asp:Button ID="CreateDirectoryButton" Text="Создать" OnClick="CreateDirectoryButton_ClickHandler"
                                                    runat="server" />
                                                <asp:Button ID="CancelCreateDirectoryButton" Text="Отменить" OnClick="CancelCreateDirectoryButton_ClickHandler"
                                                    runat="Server" />
                                            </div>
                                        </div>
                                    </asp:View>
                                    <asp:View ID="AddToZipView" runat="Server">
                                        <div style="border-bottom: solid 1px #000000;">
                                            Добавление в архив
                                        </div>
                                        <div>
                                            <asp:TextBox ID="FileManagerZipNameTextBox" EnableViewState="false" runat="Server" />
                                        </div>
                                        <div class="buttonsBlock">
                                            <asp:Button ID="AddToZipButton" Text="Создать" OnClick="AddToZipButton_ClickHandler"
                                                runat="server" />
                                            <asp:Button ID="CancelAddToZipButton" Text="Отменить" OnClick="CancelAddToZipButton_ClickHandler"
                                                runat="Server" />
                                        </div>
                                    </asp:View>
                                    <asp:View ID="RenameView" runat="Server">
                                        <div style="border-bottom: solid 1px #000000;">
                                            Переименовать файл
                                        </div>
                                        <div>
                                            Старое имя файла<br />
                                            <asp:TextBox ID="FileManagerOldNameTextBox" EnableViewState="false" runat="Server"
                                                ReadOnly="true" />
                                            <br />
                                            <br />
                                            Новое название файла<br />
                                            <asp:TextBox ID="FileManagerNewNameTextBox" EnableViewState="false" runat="Server" />
                                        </div>
                                        <div class="buttonsBlock">
                                            <asp:Button ID="RenameButton" Text="Переименовать" OnClick="RenameButton_ClickHandler"
                                                runat="server" />
                                            <asp:Button ID="CancelRenameButton" Text="Отменить" OnClick="CancelRenameButton_ClickHandler"
                                                runat="Server" />
                                        </div>
                                    </asp:View>
                                </asp:MultiView>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="fileManagerFooter">
                &nbsp;<span id="FileManagerFooterBlock" runat="Server"></span>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="UploadButton" />
        </Triggers>
    </asp:UpdatePanel>
    <script type="text/javascript">
        var UploadControlId = "<%# FileManagerUpload1.ClientID%>";
        function CancelUpload() {
            var layer = document.getElementById(UploadControlId);
            layer.innerHTML = layer.innerHTML;
        }
        function SelectAll(value) {
            var checkBoxes = document.getElementById("fileManagerFolderContent").getElementsByTagName("input");
            for (var i = 0; i < checkBoxes.length; ++i) {
                if (checkBoxes[i].type == "checkbox")
                    checkBoxes[i].checked = document.getElementById("selectAllCheckBox").checked;
            }
        }
        //Обход бага в Opera для работы Asp.Net Ajax History 
        function AddDummyQueryString() {
            var browser = navigator.appName;
            if (browser == "Opera") {
                if (!(window.location.href.indexOf("?x=y") != -1))
                    window.location.href = window.location.href + "?x=y";
            }
        }

        function onTableRowMouseOver(item) {
            item.className = "tableLineHover";
        }

        function onTableRowMouseLeft(item) {
            item.className = "";
        }

        function onTableRowClick(item, event) {
            if (event.target) {
                if (event.target.tagName == "INPUT" || event.target.tagName == "A") {
                    return;
                }
            }
            if (event.srcElement) {
                if (event.srcElement.tagName == "INPUT" || event.srcElement.tagName == "A") {
                    return;
                }
            }

            var inputs = item.getElementsByTagName("input");
            for (i = 0; i < inputs.length; ++i) {
                if (inputs[i].type == "checkbox") {
                    inputs[i].checked = !inputs[i].checked;
                }
            }
            return false;
        }

        function onMenuCellMouseOver(item) {
            item.className = "menuCellHover";
        }

        function onMenuCellMouseLeft(item) {
            item.className = "menuCell";
        }

        function stopBubble(event) {
            if (event.stopPropagation) {
                event.stopPropagation();
            } else {
                window.event.cancelBubble = true;
            }
        }

        function downloadFile(event, file) {
            window.location = "?path=" + file;
        }
        
    </script>
</div>
