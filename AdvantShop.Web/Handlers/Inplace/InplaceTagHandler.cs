using AdvantShop.Core.Services.InplaceEditor;
using AdvantShop.Core.Services.Catalog;

namespace AdvantShop.Handlers.Inplace
{
	public class InplaceTagHandler
	{
		public bool Execute(int id, string content, TagInplaceField field)
		{
			var tag = TagService.Get(id);
			if (tag == null)
				return false;

			switch (field)
			{
				case TagInplaceField.Description:
					tag.Description = content;
					break;
				case TagInplaceField.BriefDescription:
					tag.BriefDescription = content;
					break;
			}

			TagService.Update(tag);

			return true;
		}
	}
}