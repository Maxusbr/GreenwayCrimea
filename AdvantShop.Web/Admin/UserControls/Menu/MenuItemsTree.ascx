<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MenuItemsTree.ascx.cs" Inherits="AdvantShop.Admin.UserControls.Menu.MenuItemsTree" %>
<%@ Import Namespace="AdvantShop.Core.Common.Extensions" %>
<%@ Import Namespace="Resources" %>
<div id="tree-<%=MenuType.ToString() %>">
    <div class="justify">
        <h2 class="justify-item products-header"><%=MenuType.Localize() %></h2>
        <div class="justify-item products-header-controls">
            <asp:ImageButton ID="ibtnAddInRoot" runat="server" ImageUrl="~/admin/images/gplus.gif" onmouseover="this.src='images/bplus.gif'"
                onmouseout="this.src='images/gplus.gif';" ToolTip='<%$ Resources:Resource,Admin_MenuManager_CreateItem %>'
                OnClientClick="open_window('m_Menu.aspx', 750, 700);return false;" />
        </div>
    </div>
    <div class="treeview">
        <adv:CommandTreeView ID="tree" runat="server" NodeWrap="True" ShowLines="true"
            OnTreeNodeCommand="tree_TreeNodeCommand" OnTreeNodePopulate="tree_TreeNodePopulate">
            <ParentNodeStyle Font-Bold="False" />
            <SelectedNodeStyle ForeColor="#027dc2" Font-Bold="true" HorizontalPadding="5px" VerticalPadding="0px" />
            <NodeStyle Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" HorizontalPadding="5px"
                NodeSpacing="0px" VerticalPadding="0px" />
        </adv:CommandTreeView>
    </div>
</div>
<script type="text/javascript">
    function setup<%=MenuType.ToString() %>HoverPanel() {
        $("#tree-<%=MenuType.ToString() %> .newToolTip").each(function () {
            if ($(this).data('qtip')) {
                return true;
            }
            var MenuId = $(this).attr("menuId");
            var MenuName = $(this).attr("menuName");
            var MenuType = $(this).attr("menuType");

            var cnt = "<div class='hoverPanel'><div style='margin-bottom:5px;width:190px'><a onmouseover='this.style.color = \"#FF7F27\"' onmouseout='this.style.color = \"#000000\"' onclick=\"open_window('m_Menu.aspx?ParentId=" + MenuId + "&type=" + MenuType + "',750,700); return false;\"> <div class=\"hoverPanel_img_container\"><img onmouseout=\"this.src = 'images/glplus.gif';\" onmouseover=\"this.src = 'images/blplus.gif';\" src=\"images/glplus.gif\" alt=\"\"/></div>";
            cnt += "<span><%= Resource.Admin_MasterPageAdminCatalog_FPanel_AddMenu %></span></a></div>";
            if (MenuId != 0) {
                cnt += "<div style='margin-bottom:5px;'><a onmouseover='this.style.color = \"#FF7F27\"' onmouseout='this.style.color = \"#000000\"' onclick=\"open_window('m_Menu.aspx?MenuID=" + MenuId + "',750,700); return false;\"> <div class=\"hoverPanel_img_container\"><img onmouseout=\"this.src = 'images/gpencil.gif';\" onmouseover=\"this.src = 'images/bpencil.gif';\" src=\"images/gpencil.gif\" alt=\"\"/></div>";
                cnt += "<span><%= Resource.Admin_MasterPageAdminCatalog_FPanel_EditMenu %></span></a></div>";
                cnt += "<div><a onmouseover='this.style.color = \"#FF7F27\"' onmouseout='this.style.color = \"#000000\"' onclick=\"if(confirm('" + MenuName + "')){__doPostBack('<%= tree.UniqueID %>','c$DeleteMenuItem#" + MenuId + "')} return false;\"> <div class=\"hoverPanel_img_container\"><img onmouseout=\"this.src = 'images/gcross.gif';\" onmouseover=\"this.src = 'images/bcross.gif';\" src=\"images/gcross.gif\" alt=\"\"/></div>";
                cnt += "<span><%= Resource.Admin_MasterPageAdminCatalog_FPanel_DeleteMenu %></span></a></div></div>";
            }

            $(this).qtip({
                content: cnt,
                position: { corner: { target: 'bottomLeft', tooltip: "topLeft" }, adjust: { screen: true} },
                //раскомментировать в случае падения производительности скриптов на странице
                hide: { fixed: true, delay: 100 /*,effect: function () { $(this).stop(true, true).hide(); }*/ },
                show: { solo: true, delay: 600 /*,effect: function () { $(this).stop(true, true).show(); }*/ }
            });

            $(this).mouseover(function () {
                $(this).addClass("AdminTree_HoverNodeStyle");
            });

            $(this).mouseout(function () {
                $(this).removeClass("AdminTree_HoverNodeStyle");
            });
            return true;
        });
    }

    Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function() {
        setup<%=MenuType.ToString() %>HoverPanel();
    });

</script>