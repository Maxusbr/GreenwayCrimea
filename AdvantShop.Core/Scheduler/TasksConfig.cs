//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using System.Configuration;
using System.Xml;

namespace AdvantShop.Core
{
    public class TasksConfig : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            var scheduleTasks = new List<XmlNode>();

            foreach (XmlNode child in section.ChildNodes)
                scheduleTasks.Add(child);

            return scheduleTasks;
        }
    }
}