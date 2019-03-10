<%@ Control Language="C#" CodeBehind="CurrentProcess.ascx.cs" Inherits="AdvantShop.Module.SimaLand.CurrentProcess" %>
<%@ Import Namespace="AdvantShop.Module.SimaLand.Service" %>

<script runat="server">
    int v = PSLModuleSettings.Version;
</script>

<link rel="stylesheet" href="../Modules/SimaLand/Content/Styles/styles.css?<%=v %>" />
<div class="info-labels">
    <label id="errorMessageLabel"></label>
</div>
<div class="product-attributes-wrap">
    <div>
        <label><input type="checkbox" id="notUpdateName" value="false" /> Не обновлять наименование товара</label>
    </div>
    <div>
        <label><input type="checkbox" id="notUpdateDescr" value="false" /> Не обновлять описание товара</label>
    </div>
    <div>
        <label><input type="checkbox" id="notUpdateUrl" value="false" /> Не обновлять url товара</label>
    </div>
    <div>
        <label><input type="checkbox" id="notUpdateProperty" value="false" /> Не обновлять свойства товара, если товар есть в магазине</label>
    </div>
    <br/>
    <div>
        <input type="button" id="savePAttributes" value="Сохранить" class="btn btn-add btn-middle" />
    </div>
</div>

<script type="text/javascript" src="../Modules/SimaLand/Content/Scripts/product-attributes.js?<%=v %>"></script>
