using AdvantShop.Configuration;
using AdvantShop.Core.Services.InplaceEditor;
using AdvantShop.Repository;

namespace AdvantShop.Handlers.Inplace
{
    public class InplacePhoneHandler
    {
        public bool Execute(int id, string content, PhoneInplaceField field)
        {
            if (field == PhoneInplaceField.Number)
            {
                if (id == 0)
                    SettingsMain.Phone = content;
                else
                    CityService.UpdatePhone(id, content);
                
                return true;
            }

            return false;
        }
    }
}