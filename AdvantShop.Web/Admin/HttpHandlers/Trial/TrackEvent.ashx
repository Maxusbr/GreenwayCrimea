<%@ WebHandler Language="C#" Class="Admin.HttpHandlers.Trial.TrackEvent" %>

using System;
using System.Web;
using AdvantShop.Core.HttpHandlers;
using AdvantShop.Saas;
using AdvantShop.Trial;

namespace Admin.HttpHandlers.Trial
{
    public class TrackEvent : AdminHandler, IHttpHandler
    {
        public new void ProcessRequest(HttpContext context)
        {
            if (!TrialService.IsTrialEnabled && !SaasDataService.IsSaasEnabled)
                return;

            TrialEvents events;
            if(Enum.TryParse(context.Request["trialevent"], out events))
            {
                TrialService.TrackEvent(events, context.Request["trialparams"]);
            }
        }
    }
}