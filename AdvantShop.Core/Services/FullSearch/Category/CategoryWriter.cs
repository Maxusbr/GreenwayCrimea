using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AdvantShop.Catalog;
using AdvantShop.Core.Services.FullSearch.Core;
using AdvantShop.Helpers;

namespace AdvantShop.Core.Services.FullSearch
{
    public class CategoryWriter : BaseWriter<CategoryDocument>
    {
          public CategoryWriter()
            : base(string.Empty)
        {
        }
          public CategoryWriter(string path)
            : base(path)
        {
        }

        public void AddUpdateToIndex(Category model)
        {
            if(model.ID != 0)
                AddUpdateItemsToIndex(new List<CategoryDocument> { (CategoryDocument)model });
        }


        public void AddUpdateToIndex(List<Category> model)
        {
                AddUpdateItemsToIndex(model.Where(cat=> cat.ID != 0).Select(p => (CategoryDocument)p).ToList());
        }


        public void DeleteFromIndex(Category model)
        {
            DeleteItemsFromIndex(new List<CategoryDocument> { (CategoryDocument)model });
        }


        public void DeleteFromIndex(int id)
        {
            DeleteItemsFromIndex(new List<CategoryDocument> { new CategoryDocument { Id = id } });
        }
        
        //static 
        public static void AddUpdate(Category model)
        {
            using (var writer = new CategoryWriter())
            {
                writer.AddUpdateToIndex(model);
            }
        }


        public static void AddUpdate(List<Category> model)
        {
            using (var writer = new CategoryWriter())
            {
                writer.AddUpdateToIndex(model);
            }
        }


        public static void Delete(Category model)
        {
            using (var writer = new CategoryWriter())
            {
                writer.DeleteFromIndex(model);
            }
        }

        public static void Delete(int id)
        {
            using (var writer = new CategoryWriter())
            {
                writer.DeleteFromIndex(id);
            }
        }

        public static void CreateIndexFromDbInTask()
        {
            Task.Factory.StartNew(CreateIndexFromDb, TaskCreationOptions.LongRunning);
        }

        public static void CreateIndexFromDb()
        {
            var basePath = BasePath(typeof(CategoryDocument).Name);
            var tempPath = basePath + "_temp";
            var mergePath = basePath + "_temp2";
            var cats = CategoryService.GetCategories();

            using (var writer = new CategoryWriter(tempPath))
            {
                foreach (var item in cats)
                {
                    //if (item.Enabled)
                    writer.AddUpdateToIndex(item);
                }
                writer.Optimize();
            }
            FileHelpers.CreateDirectory(basePath);

            if (Directory.Exists(mergePath))
            {
                Directory.Delete(mergePath, true);
            }

            Directory.Move(basePath, mergePath);
            Directory.Move(tempPath, basePath);
            Directory.Delete(mergePath, true);
        }
    }
}