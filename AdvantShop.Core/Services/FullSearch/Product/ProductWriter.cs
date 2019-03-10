using AdvantShop.Catalog;
using AdvantShop.Core.Services.FullSearch.Core;
using AdvantShop.Helpers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdvantShop.Core.Services.FullSearch
{
    public class ProductWriter : BaseWriter<ProductDocument>
    {
        public ProductWriter()
            : base(string.Empty)
        {
        }
        public ProductWriter(string path)
            : base(path)
        {
        }

        public void AddUpdateToIndex(Product model)
        {
            AddUpdateItemsToIndex(new List<ProductDocument> { (ProductDocument)model });
        }

        public void AddUpdateToIndex(List<Product> model)
        {
            AddUpdateItemsToIndex(model.Select(p => (ProductDocument)p).ToList());
        }

        public void DeleteFromIndex(Product model)
        {
            DeleteItemsFromIndex(new List<ProductDocument> { (ProductDocument)model });
        }

        public void DeleteFromIndex(int id)
        {
            DeleteItemsFromIndex(new List<ProductDocument> { new ProductDocument { Id = id } });
        }

        //static 
        public static void AddUpdate(Product model)
        {
            using (var writer = new ProductWriter())
            {
                writer.AddUpdateToIndex(model);
            }
        }


        public static void AddUpdate(List<Product> model)
        {
            using (var writer = new ProductWriter())
            {
                writer.AddUpdateToIndex(model);
            }
        }


        public static void Delete(Product model)
        {
            using (var writer = new ProductWriter())
            {
                writer.DeleteFromIndex(model);
            }
        }

        public static void Delete(int id)
        {
            using (var writer = new ProductWriter())
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
            var basePath = BasePath(typeof(ProductDocument).Name);
            var tempPath = basePath + "_temp";
            var mergePath = basePath + "_temp2";

            var ids = ProductService.GetAllProductIDs();
            using (var writer = new ProductWriter(tempPath))
            {
                foreach (var item in ids)
                {
                    var product = ProductService.GetProduct(item);
                    //if (product.Enabled && product.CategoryEnabled)
                    writer.AddUpdateToIndex(product);
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