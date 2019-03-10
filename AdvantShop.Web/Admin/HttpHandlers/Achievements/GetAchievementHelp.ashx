<%@ WebHandler Language="C#" Class="Admin.HttpHandlers.AchievementSave" %>

using System;
using System.Web;
using AdvantShop;
using AdvantShop.Trial;
using Newtonsoft.Json;

namespace Admin.HttpHandlers
{
    public class AchievementSave : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            int achievementId;
            bool isNumberAchievementId = int.TryParse((string)context.Request["achievementId"], out achievementId);


            if (isNumberAchievementId)
            {
                context.Response.Write(JsonConvert.SerializeObject(new
                {
                    Result = TrialService.GetAchievementHelp(achievementId),
                    Ok = true
                }));
            }
            else
            {
                context.Response.Write(JsonConvert.SerializeObject(new {
                    Ok = false
                }));
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

    }
}