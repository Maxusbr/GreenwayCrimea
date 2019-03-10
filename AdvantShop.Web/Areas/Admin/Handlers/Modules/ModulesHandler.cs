using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Modules;
using AdvantShop.Web.Admin.Models.Modules;
using System.Collections.Generic;
using System.Linq;

namespace AdvantShop.Web.Admin.Handlers.Modules
{
    public class ModulesHandler
    {
        public List<Module> GetLocalModules(ModulesFilterModel filter)
        {
            var modules = ModulesService.GetModules().Items.Where(x => x.Active && x.IsInstall).ToList();
            return GetFiltered(modules, filter);
        }

        public List<Module> GetMarketModules(ModulesFilterModel filter)
        {
            var modules = ModulesService.GetModules().Items.Where(x => !x.Active || !x.IsInstall).ToList();
            return GetFiltered(modules, filter);
        }

        public Module GetModule(string stringId)
        {
            return ModulesService.GetModules().Items.FirstOrDefault(x => x.StringId.ToLower() == stringId.ToLower());
        }

        private List<Module> GetFiltered(List<Module> modules, ModulesFilterModel filter)
        {
            if (filter == null)
                return modules;

            var result = modules;

            if (filter.Name.IsNotEmpty())
            {
                result = result.Where(x => x.Name.Contains(filter.Name) || (x.BriefDescription !=null && x.BriefDescription.Contains(filter.Name)) || x.StringId.Contains(filter.Name)).ToList();
            }
            
            if (filter.FilterBy != ModulesPreFilterType.None)
            {
                switch (filter.FilterBy)
                {
                    case ModulesPreFilterType.Bestsellers:
                        result = result.Where(x => x.Popular).ToList();
                        break;
                    case ModulesPreFilterType.Free:
                        result = result.Where(x => x.Price == 0).ToList();
                        break;
                    case ModulesPreFilterType.New:
                        result = result.Where(x => x.New).ToList();
                        break;
                    case ModulesPreFilterType.Paid:
                        result = result.Where(x => x.Price !=0).ToList();
                        break;
                }
                
            }


            return result;
        }


    }
}
