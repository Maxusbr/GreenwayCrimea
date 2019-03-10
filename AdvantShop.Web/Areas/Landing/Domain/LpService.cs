using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using AdvantShop.App.Landing.Handlers.Pictures;
using AdvantShop.Core.SQL;
using AdvantShop.Helpers;

namespace AdvantShop.App.Landing.Domain
{
    public class LpService
    {
        public const string TepmlateFolder = "~/Areas/Landing/Templates";
        public const string ViewsFolder = "~/Areas/Landing/Views";

        public static Lp CurrentLanding
        {
            get
            {
                if (HttpContext.Current != null)
                    return HttpContext.Current.Items["LandingPageModule_LandingPage"] as Lp;

                return null;
            }
            set { HttpContext.Current.Items["LandingPageModule_LandingPage"] = value; }
        }

        public static bool Inplace
        {
            get
            {
                if (HttpContext.Current != null)
                    return SQLDataHelper.GetBoolean(HttpContext.Current.Items["LandingPageModule_Inplace"]);

                return false;
            }
            set { HttpContext.Current.Items["LandingPageModule_Inplace"] = value.ToString(); }
        }


        #region CRUD

        public int Add(Lp lp)
        {
            return Convert.ToInt32(SQLDataAccess.ExecuteScalar(
                "INSERT INTO [CMS].[Landing] ([Url],[Enabled],[Name],[Template],[CreatedDate]) VALUES (@Url,@Enabled,@Name,@Template,GetDate()); Select scope_identity(); ",
                CommandType.Text,
                new SqlParameter("@Url", lp.Url),
                new SqlParameter("@Name", lp.Name ?? string.Empty),
                new SqlParameter("@Template", lp.Template),
                new SqlParameter("@Enabled", lp.Enabled)
                ));
        }

        public void Update(Lp lp)
        {
            SQLDataAccess.ExecuteNonQuery(
                "Update [CMS].[Landing] Set [Url] = @Url, [Enabled] = @Enabled, [Name] = @Name, [Template] = @Template Where Id = @Id",
                CommandType.Text,
                new SqlParameter("@Url", lp.Url),
                new SqlParameter("@Enabled", lp.Enabled),
                new SqlParameter("@Name", lp.Name ?? string.Empty),
                new SqlParameter("@Template", lp.Template),
                new SqlParameter("@Id", lp.Id)
                );
        }

        public void Delete(int id)
        {
            SQLDataAccess.ExecuteNonQuery(
                "Delete From [CMS].[Landing] Where Id = @Id",
                CommandType.Text,
                new SqlParameter("@Id", id));

            new RemoveLandingPicturesHandler(id).Execute();
        }

        public List<Lp> GetList()
        {
            return SQLDataAccess.Query<Lp>("Select * From [CMS].[Landing] ").ToList();
        }

        public Lp Get(int id)
        {
            return SQLDataAccess.Query<Lp>("Select * From [CMS].[Landing] Where Id = @id", new {id}).FirstOrDefault();
        }

        public Lp Get(string url)
        {
            return SQLDataAccess.Query<Lp>("Select * From [CMS].[Landing] Where Url = @url", new { url }).FirstOrDefault();
        }

        #endregion
    }
}
