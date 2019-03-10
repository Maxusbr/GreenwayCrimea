<%@ Page Language="C#" AutoEventWireup="true" Inherits="Admin.m_Menu"
    ValidateRequest="false" MasterPageFile="m_MasterPage.master" Codebehind="m_Menu.aspx.cs" %>

<%@ Register Src="~/Admin/UserControls/PopupGridProduct.ascx" TagName="PopupGridProduct" TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/PopupGridNews.ascx" TagName="PopupGridNews" TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/PopupGridAux.ascx" TagName="PopupGridAux" TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/PopupGridCategory.ascx" TagName="PopupGridCategory" TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/PopupGridBrand.ascx" TagName="PopupGridBrand" TagPrefix="adv" %>
<asp:Content runat="server" ContentPlaceHolderID="cphCenter">
    <asp:UpdatePanel ID="UpdatePanelTree" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="tree" EventName="TreeNodePopulate" />
        </Triggers>
        <ContentTemplate>
            <ajaxToolkit:ModalPopupExtender ID="mpeTree" runat="server" PopupControlID="pTree"
                TargetControlID="hhl" BackgroundCssClass="blackopacitybackground" CancelControlID="btnCancelParent"
                BehaviorID="ModalBehaviour">
            </ajaxToolkit:ModalPopupExtender>
            <asp:HyperLink ID="hhl" runat="server" Style="display: none;" />
            <asp:Panel runat="server" ID="pTree" CssClass="modal-admin" Style="display: none;">
                <div style="text-align: center;">
                    <table>
                        <tbody>
                            <tr>
                                <td>
                                    <span class="title">
                                        <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources:Resource, Admin_Menu_ParentMunuItem %>"></asp:Localize></span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div style="height: 360px; width: 450px; overflow: scroll; background-color: White;
                                        text-align: left">
                                        <asp:TreeView ID="tree" ForeColor="Black" PopulateNodesFromClient="true" runat="server"
                                            ShowLines="True" ExpandImageUrl="images/loading.gif" BackColor="White" OnTreeNodePopulate="PopulateNode"
                                            AutoPostBack="false" OnSelectedNodeChanged="tree_SelectedNodeChange" SelectedNodeStyle-BackColor="Yellow" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td valign="bottom" style="height: 36px; text-align: right;">
                                    <asp:Button ID="btnUpdateParent" runat="server" Text="<%$ Resources:Resource, Admin_Menu_Select %>"
                                        OnClick="btnUpdateParent_Click" />
                                    <asp:Button ID="btnCancelParent" runat="server" Text="<%$ Resources: Resource, Admin_Cancel %>"
                                        Width="67" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <adv:PopupGridProduct ID="gridProduct" runat="server" />
    <adv:PopupGridCategory ID="gridCategory" runat="server" />
    <adv:PopupGridNews ID="gridNews" runat="server" />
    <adv:PopupGridAux ID="gridAux" runat="server" />
    <adv:PopupGridBrand ID="gridBrand" runat="server" />
    <div style="padding-top: 5px;">
        <div style="text-align: center;">
            <asp:Label ID="lblBigHead" runat="server" CssClass="AdminHead"></asp:Label><br />
            <asp:Label ID="lblSubHead" runat="server" CssClass="AdminSubHead"></asp:Label><br />
        </div>
        <br />
        <asp:UpdatePanel ID="updPanel" runat="server">
            <ContentTemplate>
                <table class="info-center-tb catalog_link">
                    <tr>
                        <td class="lpart">
                            <asp:Label ID="Label2" runat="server" Text="<%$  Resources:Resource,Admin_mMenu_MenuName %>"></asp:Label><span
                                style="color: red;">&nbsp;*</span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtName" runat="server" Width="230px" Text="" ValidationGroup="vGroup" MaxLength="100"> </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtName"
                                ValidationGroup="vGroup" ErrorMessage="<%$ Resources:Resource, Admin_FillChar %>"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="lpart">
                            <% = Resources.Resource .Admin_mMenu_MenuType  %>
                        </td>
                        <td onclick="showButtonChoose();">
                            <asp:RadioButtonList ID="rblLinkType" runat="server">
                                <asp:ListItem Text="<%$ Resources:Resource,Admin_mMenu_None %>" Value="5" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="<%$ Resources:Resource,Admin_mMenu_StaticPage %>" Value="2"></asp:ListItem>
                                <asp:ListItem Text="<%$ Resources:Resource,Admin_mMenu_Product %>" Value="0"></asp:ListItem>
                                <asp:ListItem Text="<%$ Resources:Resource,Admin_mMenu_Category %>" Value="1"></asp:ListItem>
                                <asp:ListItem Text="<%$ Resources:Resource,Admin_mMenu_News %>" Value="3"></asp:ListItem>
                                <asp:ListItem Text="<%$ Resources:Resource,Admin_mMenu_Brand %>" Value="4"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <%--ссылка--%>
                    <tr>
                        <td class="lpart">URL</td>
                        <td>
                            <asp:TextBox ID="txtUrl" runat="server" Width="230px"></asp:TextBox>
                            <asp:LinkButton ID="lbChooseUrl" runat="server" Text="<%$ Resources:Resource,Admin_mMenu_MenubtnChose %>"
                                OnClientClick="GetRadioButtonValue(); return false;"></asp:LinkButton>
                            <asp:HiddenField ID="hfParamId" runat="server" />
                        </td>
                    </tr>
                    <%--родитель--%>
                    <tr>
                        <td class="lpart">
                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Resource,Admin_mMenu_MenuLocatedIn %>"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lParent" runat="server" Text=""></asp:Label>
                            <asp:HiddenField ID="hParent" runat="server" />
                            <asp:LinkButton ID="lbParentChange" runat="server" Text="<%$ Resources:Resource, Admin_m_Category_ChangeParent %>"
                                OnClick="lbParentChange_Click"></asp:LinkButton>
                        </td>
                    </tr>
                    <%--в новом окне--%>
                    <tr>
                        <td class="lpart">
                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource,Admin_mMenu_MenuOpenNewWindow %>"></asp:Label>
                        </td>
                        <td>
                            <asp:CheckBox ID="ckbBlank" runat="server" />
                        </td>
                    </tr>
                    <%--активность--%>
                    <tr>
                        <td class="lpart">
                            <asp:Label ID="Label5" runat="server" Text="<%$ Resources: Resource, Admin_mMenu_Active %>"></asp:Label>
                        </td>
                        <td>
                            <asp:CheckBox ID="ckbEnabled" runat="server" Checked="True" />
                        </td>
                    </tr>
                    
                    <tr>
                        <td class="lpart">
                            <asp:Label ID="Label8" runat="server" Text="<%$ Resources: Resource, Admin_mMenu_NoFollow %>"></asp:Label>
                        </td>
                        <td>
                            <asp:CheckBox ID="ckbNofollow" runat="server" />
                        </td>
                    </tr>

                    <%--кому показывать--%>
                    <tr>
                        <td class="lpart">
                            <asp:Label ID="Label7" runat="server" Text="<%$ Resources: Resource, Admin_mMenu_ShowMode %>"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlShowMode" runat="server">
                                <asp:ListItem Text="<%$ Resources: Resource, Admin_mMenu_ShowMode_All %>" Value="0"></asp:ListItem>
                                <asp:ListItem Text="<%$ Resources: Resource, Admin_mMenu_ShowMode_Reg %>" Value="1"></asp:ListItem>
                                <asp:ListItem Text="<%$ Resources: Resource, Admin_mMenu_ShowMode_UnReg %>" Value="2"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <%--сортировка--%>
                    <tr>
                        <td class="lpart">
                            <asp:Label ID="Label4" runat="server" Text="<%$  Resources:Resource,Admin_mMenu_MenuSorted %>"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSortOrder" runat="server" Width="230px" Text="0"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="lpart">
                            <asp:Label ID="lblicon" runat="server" Text="<%$ Resources: Resource, Admin_mMenu_Icon %>" />
                        </td>
                        <td>
                            <asp:Panel ID="pnlIcon" runat="server" Width="100%">
                                &nbsp;<asp:Label ID="Label6" runat="server" Text="<%$ Resources: Resource, Admin_mMenu_Icon %>"></asp:Label>
                                <br />
                                &nbsp;<asp:Image ID="imgIcon" runat="server" />
                                <br />
                                <asp:Button ID="btnDeleteIcon" runat="server" Text="<%$  Resources:Resource, Admin_Delete%>"
                                    Style="height: 25px" OnClick="btnDeleteIcon_Click" />
                                <br />
                            </asp:Panel>
                            <asp:FileUpload ID="IconFileUpload" runat="server" Height="20px" Width="308px" />
                            <asp:Label ID="lblIconFileName" runat="server" Text="Label" Visible="False"></asp:Label>
                            <br />
                            <asp:Label ID="lblIconInfo" runat="server" Font-Bold="False" Font-Size="Smaller"
                                ForeColor="Gray"></asp:Label>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        <br />
        <div style="text-align: center;">
            <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="False"></asp:Label>
            <asp:HiddenField ID="hfMetaId" runat="server" />
        </div>
        <div style="text-align: center;">
            <br />
            <asp:Button ID="btnAdd" runat="server" Width="103px" ValidationGroup="vGroup" OnClick="btnAdd_Click" Text="<$ Resources:Resource, Admin_m_Category_Save" />&nbsp;
        </div>
        <br />
        <br />
    </div>
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphScript">
    <script type="text/javascript">
        function removeunloadhandler() {
            window.onbeforeunload = null;
        }

        var base$TreeView_ProcessNodeData;
        var base$TreeView_PopulateNodeDoCallBack;

        function updatetree() {
            var win = document.parentWindow || document.defaultView;
            if (win.TreeView_ProcessNodeData != ProcessNodeData) {
                base$TreeView_ProcessNodeData = win.TreeView_ProcessNodeData;
                win.TreeView_ProcessNodeData = ProcessNodeData;
            }
            if (win.TreeView_PopulateNodeDoCallBack != PopulateNodeDoCallBack) {
                base$TreeView_PopulateNodeDoCallBack = win.TreeView_PopulateNodeDoCallBack;
                win.TreeView_PopulateNodeDoCallBack = PopulateNodeDoCallBack;
            }
        }

        function ProcessNodeData(result, context) {
            hide_wait_for_node(context.node);
            return base$TreeView_ProcessNodeData(result, context);
        }

        function PopulateNodeDoCallBack(context, param) {
            show_wait_for_node(context.node);
            return base$TreeView_PopulateNodeDoCallBack(context, param);
        }

        function hide_wait_for_node(node) {
            if (node.wait_img) {
                node.removeChild(node.wait_img);
            }
        }

        function show_wait_for_node(node) {
            var wait_img = document.createElement("IMG");
            wait_img.src = "images/loading.gif";
            wait_img.border = 0;
            node.wait_img = wait_img;
            node.appendChild(wait_img);
        }

        var _TreePostBack = false;

        function endRequest() {
            if (_TreePostBack) {
                updatetree();
                //document.getElementById('mpeBehavior_backgroundElement').onclick = function () { $find('mpeBehavior').hide(); };
            }
            else {
                window.onbeforeunload = beforeunload;
                $(".photoinput").val("");
            }
        }

        function beforeunload(e) {
            if ($("img.floppy:visible, img.exclamation:visible").length > 0) {
                var evt = window.event || e;
                evt.returnValue = "<%= Resources.Resource.Admin_m_Category_ChangeWillBeLost %>";
            }
        }

        function addbeforeunloadhandler() {
            window.onbeforeunload = beforeunload;
        }

        $(document).ready(function () {
            // edithook();
            window.onbeforeunload = beforeunload;
            document.forms[0].onsubmit = removeunloadhandler;

            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_pageLoaded(function () { showButtonChoose(); });
            prm.add_endRequest(endRequest);
        });

        function ATreeView_Select(sender, arg) {
            $("a.selectedtreenode").removeClass("selectedtreenode");
            $(sender).addClass("selectedtreenode");
            document.getElementById("TreeView_SelectedValue").value = arg;
            document.getElementById("TreeView_SelectedNodeText").value = sender.innerHTML;
            return false;
        }

        function esc_press(D) {
            D = D || window.event;
            var A = D.keyCode;
            if (A == 27) {
                HideModalPopupProduct();
                HideModalPopupCategory();
                HideModalPopupAux();
                HideModalPopupNews();
                HideModalPopupBrand();
            }
        }
        document.onkeydown = esc_press;

        function GetRadioButtonValue() {
            var name = ('<%=rblLinkType.ClientID%>').toString().replace(/[_]+/g, '$');
            var radio = document.getElementsByName(name);
            for (var j = 0; j < radio.length; j++) {
                if (radio[j].checked) {

                    switch (radio[j].value) {
                        case "0":
                            ShowModalPopupProduct();
                            break;
                        case "1":
                            ShowModalPopupCategory();
                            break;
                        case "3":
                            ShowModalPopupNews();
                            break;
                        case "2":
                            ShowModalPopupAux();
                            break;
                        case "4":
                            ShowModalPopupBrand();
                            break;
                    }
                    break;
                }
            }
        }

        function showButtonChoose() {
            var name = ('<%=rblLinkType.ClientID%>').toString().replace(/[_]+/g, '$');
            var radio = document.getElementsByName(name);
            $("#<%= lbChooseUrl.ClientID%>").css("display", "inline");
            for (var j = 0; j < radio.length; j++) {
                if (radio[j].checked) {
                    if (radio[j].value == "5")
                        $("#<%= lbChooseUrl.ClientID%>").css("display", "none");
                }
            }
        }
      
    </script>
</asp:Content>
