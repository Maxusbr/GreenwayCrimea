//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using AdvantShop.Core.Modules;

namespace AdvantShop.Module.StoreReviews.Domain
{
    public class StoreReview
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string ReviewerEmail { get; set; }
        public string ReviewerName { get; set; }
        public string Review { get; set; }
        public int Rate { get; set; }
        public bool Moderated { get; set; }
        public int ChildsCount { get; set; }
        public bool HasChild {
            get { return ChildsCount > 0; }
        }
        public DateTime DateAdded { get; set; }
        public string ReviewerImage { get; set; }

        private List<StoreReview> _childrenReviews;
        public List<StoreReview> ChildrenReviews
        {
            get
            {
                return _childrenReviews
                    ?? (_childrenReviews = StoreReviewRepository.GetStoreReviewsByParentId(Id, ModuleSettingsProvider.GetSettingValue<bool>("ActiveModerateStoreReviews", StoreReviews.ModuleID), ++Level));
            }
            set { _childrenReviews = value; }
        }

        public int Level { get; set; }
    }
}