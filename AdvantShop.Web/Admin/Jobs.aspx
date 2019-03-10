<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" CodeBehind="Jobs.aspx.cs" Inherits="Admin.Jobs" %>

<%@ Import Namespace="AdvantShop.Core.Scheduler" %>
<%@ Import Namespace="Quartz" %>
<%@ Import Namespace="Newtonsoft.Json" %>
<%@ Import Namespace="Quartz.Impl.Matchers" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="Server">
    Current date:<%= DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")  %>
    <br />
    <% var scheduler = TaskManager.TaskManagerInstance().Sched;
       IList<string> jobGroups = scheduler.GetJobGroupNames();
       foreach (string group in jobGroups)
       {
           var groupMatcher = GroupMatcher<JobKey>.GroupContains(group);
           var jobKeys = scheduler.GetJobKeys(groupMatcher);
           foreach (var jobKey in jobKeys)
           {
               var detail = scheduler.GetJobDetail(jobKey);
               var triggers = scheduler.GetTriggersOfJob(jobKey);
               foreach (ITrigger trigger in triggers)
               {
    %>

  group: <% = group %><br />
    jobName: <%= jobKey.Name %><br />
    jobDescription: <%= detail.Description %><br />
    jobDataMap: <%= JsonConvert.SerializeObject(detail.JobDataMap) %><br />
    triggerName: <%= trigger.Key.Name %><br />
    triggerGroup: <%= trigger.Key.Group %><br />
    triggertype: <%= trigger.GetType().Name %><br />
    triggerState: <%= scheduler.GetTriggerState(trigger.Key) %><br />
    <%DateTimeOffset? nextFireTime = trigger.GetNextFireTimeUtc();
      if (nextFireTime.HasValue)
      {%>
  nextFireTime: <%= nextFireTime.Value.LocalDateTime.ToString()%><br />
    <% } %>

    <% DateTimeOffset? previousFireTime = trigger.GetPreviousFireTimeUtc();
       if (previousFireTime.HasValue)
       { %>
    previousFireTime: <%= previousFireTime.Value.LocalDateTime.ToString() %>
    <% }
       else
       { %>
    previousFireTime: not start
    <% } %>
    <br/><br />
    <span>________________________________________________________________________________</span><br />
    <% }
           }
       }
    %>
</asp:Content>
