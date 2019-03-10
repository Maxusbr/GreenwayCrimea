using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Web.DynamicData;
using AdvantShop.Module.SimaLand.Service;
using System.Threading;
using AdvantShop.Core.Modules;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Web.Script.Serialization;

namespace AdvantShop.Module.SimaLand
{
    public partial class ModuleSettings : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //if (ModulesRepository.IsActiveModule(SimaLand.ModuleStringId))
                //{
                    //var lastUpdate = Convert.ToDateTime(PSLModuleSettings.LastUpdateCategory);
                    //var comparer = DateTime.Compare(lastUpdate, DateTime.Now.Date);
                    //if (comparer < 0)
                    //{
                    if (!ModuleService.ExistRows() && !SimalandImportStatistic.Process)
                        Task.Factory.StartNew(() => SimalandCategoryService.pc());
                    //}
                //}
            }
        }                  
    }
}
