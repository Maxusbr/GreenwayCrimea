//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using AdvantShop.Core.Services.Crm;

namespace AdvantShop.Core.Modules.Interfaces
{
    public interface ILeadChanged
    {
        void LeadAdded(Lead lead);
        
        void LeadUpdated(Lead lead);

        void LeadDeleted(int leadId);
    }
}