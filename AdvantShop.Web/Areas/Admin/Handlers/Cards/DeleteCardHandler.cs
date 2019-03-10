using System;
using AdvantShop.Core.Services.Bonuses.Service;
using AdvantShop.Web.Infrastructure.Handlers;

namespace AdvantShop.Web.Admin.Handlers.Cards
{
    public class DeleteCardHandler : AbstractCommandHandler
    {
        private Guid _cardId;
        public DeleteCardHandler(Guid cardId)
        {
            _cardId = cardId;
        }
        
        protected override void Handle()
        {
            CardService.Delete(_cardId);
        }        
    }
}
