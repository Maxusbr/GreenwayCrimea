<%@ Control Language="C#" CodeBehind="CurrentProcess.ascx.cs" Inherits="AdvantShop.Module.SimaLand.CurrentProcess" %>
<%@ Import Namespace="AdvantShop.Module.SimaLand.Service" %>

<script runat="server">
    int v = PSLModuleSettings.Version;
</script>

<link rel="stylesheet" href="../Modules/SimaLand/Content/Styles/styles.css?<%=v %>" />
<link rel="stylesheet" href="../Modules/SimaLand/Content/Styles/current-process.css?<%=v %>" />

<div id="currentProcess-wrap" style="display: none">
    <div>Текущий процесс:<span id="currentProcess"></span></div>
    <div>Всего:<span id="totalCount"></span></div>
    <div>Обработано:<span id="totalHandled"></span></div>
    <div>Времени затрачено:<span id="timeSpent"></span></div>
</div>
<div id="not-process">
    Процесс не запущен
</div>
<script type="text/javascript" src="../Modules/SimaLand/Content/Scripts/current-process.js?<%=v %>"></script>
