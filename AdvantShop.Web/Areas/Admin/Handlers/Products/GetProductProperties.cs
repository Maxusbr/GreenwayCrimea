using System.Collections.Generic;
using System.Linq;
using AdvantShop.Catalog;
using AdvantShop.Web.Admin.Models;
using AdvantShop.Web.Admin.Models.Products;

namespace AdvantShop.Web.Admin.Handlers.Products
{
    public class GetProductProperties
    {
        private readonly int _productId;

        public GetProductProperties(int productId)
        {
            _productId = productId;
        }

        public ProductProperiesModel Execute()
        {
            var model = new List<ProductPropertyGroupModel>();

            var propertyValues = PropertyService.GetPropertyValuesByProductId(_productId);
            var categoryId = ProductService.GetFirstCategoryIdByProductId(_productId);
            var category = CategoryService.GetCategory(categoryId);

            // Свойства, у которых группы привязанны к категории

            var propertyInGroups = new List<int>();
            var propertyGroups = PropertyGroupService.GetGroupsByCategory(categoryId).Where(x => x.PropertyId != 0).ToList();

            if (propertyGroups.Count > 0 && categoryId != 0)
                model.Add(new ProductPropertyGroupModel()
                {
                    Title = string.Format("Группы, привязанные к категории \"{0}\"", category.Name),
                });

            var groups = propertyGroups.Select(g => new UsedPropertyGroupView()
            {
                PropertyGroupId = g.PropertyGroupId,
                GroupName = g.GroupName,
                Properties =
                    propertyGroups.Where(p => p.PropertyGroupId == g.PropertyGroupId).Select(p => new Property
                    {
                        PropertyId = p.PropertyId,
                        Name = p.PropertyName,
                        Type = p.Type
                    }).ToList()
            }).Distinct(new PropertyGroupComparer()).ToList();

            foreach (var group in groups)
            {
                var modelGroup = new ProductPropertyGroupModel()
                {
                    GroupId = group.PropertyGroupId,
                    Name = group.GroupName,
                };

                foreach (var property in group.Properties)
                {
                    modelGroup.Properties.Add(new ProductPropertyModel(property)
                    {
                        SelectedPropertyValues =
                            propertyValues.Where(x => x.PropertyId == property.PropertyId)
                                .Select(x => new PropertyValueModel(x))
                                .ToList()
                    });
                    propertyInGroups.Add(property.PropertyId);
                }
                model.Add(modelGroup);
            }

            // Свойства, не привязанные к категории (сначала у кого есть группа, потом остальные)

            var propsWithoutCategory = propertyValues.Where(p => !propertyInGroups.Contains(p.PropertyId)).ToList();
            if (propsWithoutCategory.Count > 0)
            {
                if (propertyGroups.Count > 0 && categoryId != 0)
                    model.Add(new ProductPropertyGroupModel()
                    {
                        Title = string.Format("Группы, не привязанные к категории \"{0}\"", category.Name)
                    });

                var propsWithGroup = propsWithoutCategory.Where(p => p.Property.GroupId != null).ToList();
                foreach (var propertyValue in propsWithGroup)
                {
                    var group = model.FirstOrDefault(x => x.GroupId == propertyValue.Property.GroupId) ??
                                new ProductPropertyGroupModel()
                                {
                                    Name = propertyValue.Property.Group.Name,
                                    GroupId = propertyValue.Property.GroupId,
                                };

                    if (group.Properties.Find(x => x.PropertyId == propertyValue.PropertyId) == null)
                        group.Properties.Add(new ProductPropertyModel(propertyValue.Property)
                        {
                            SelectedPropertyValues =
                                propsWithGroup.Where(x => x.PropertyId == propertyValue.PropertyId)
                                    .Select(x => new PropertyValueModel(x))
                                    .ToList()
                        });

                    if (model.Find(x => x.GroupId == group.GroupId) == null)
                        model.Add(group);
                }

                var propsWithoutGroup = propsWithoutCategory.Where(p => p.Property.GroupId == null).ToList();
                if (propsWithoutGroup.Count > 0)
                {
                    var modelGroup = new ProductPropertyGroupModel()
                    {
                        Name = "Прочее",
                        Properties = new List<ProductPropertyModel>()
                    };

                    foreach (var propertyWithoutGroup in propsWithoutGroup)
                    {
                        if (modelGroup.Properties.Find(x => x.PropertyId == propertyWithoutGroup.PropertyId) == null)
                            modelGroup.Properties.Add(new ProductPropertyModel(propertyWithoutGroup.Property)
                            {
                                SelectedPropertyValues =
                                    propsWithoutGroup.Where(x => x.PropertyId == propertyWithoutGroup.PropertyId)
                                        .Select(x => new PropertyValueModel(x))
                                        .ToList()
                            });
                    }

                    model.Add(modelGroup);
                }
            }

            var result = new ProductProperiesModel()
            {
                Groups = model,
                Properties = PropertyService.GetAllProperties().Select(x => new SelectItemModel(x.Name, x.PropertyId.ToString())).ToList() 
                //.Select(x => new ProductPropertyModel(x)).ToList()
            };

            return result;
        }

    }
}
