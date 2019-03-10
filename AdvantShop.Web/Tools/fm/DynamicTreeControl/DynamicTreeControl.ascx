<%@ Control Language="C#" AutoEventWireup="true" Inherits="Tools.fm.DynamicTreeControl.DynamicTreeControl" Codebehind="DynamicTreeControl.ascx.cs" %>
<div id="container" class="AspNet-TreeView" runat="server">
    <input type="hidden" id="SelectedNodeInput" runat="server" />
    <input type="hidden" id="selectedNodeInputID" value="<%# SelectedNodeInput.ClientID.ToString() %>"></input>
</div>
<script type="text/javascript">
    function Expand(o) {
        o.className = "Net-TreeView-Collapse";
        o.parentNode.className = "AspNet-TreeView-Parent-Open";
        o.onclick = function onclick() { Collapse(this); };
    }

    function Collapse(o) {
        o.className = "Net-TreeView-Expand";
        o.parentNode.className = "AspNet-TreeView-Parent-Closed";
        o.onclick = function onclick() { Expand(this); };
    }

    var previousSelectedItem = null;

    function SelectListItem(item, nodeValue) {
        if (item.tagName != "A") {
            return;
        }
        var selectedNodeFieldId = document.getElementById("selectedNodeInputID").value;
        var selectedNode = document.getElementById(selectedNodeFieldId);

        if (previousSelectedItem != null) {
            previousSelectedItem.style.backgroundColor = "";
        }
        previousSelectedItem = item;
        item.style.backgroundColor = "#aaaaaa";
        selectedNode.value = nodeValue;
    }
</script>