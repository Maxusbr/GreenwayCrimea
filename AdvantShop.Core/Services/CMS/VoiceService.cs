//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AdvantShop.Core.Caching;
using AdvantShop.Core.SQL;
using AdvantShop.Helpers;

namespace AdvantShop.CMS
{
    public static class VoiceService
    {
        #region Answers
        
        private static Answer GetAnswerFromReader(SqlDataReader reader)
        {
            return new Answer
            {
                AnswerId = SQLDataHelper.GetInt(reader, "AnswerID"),
                FkidTheme = SQLDataHelper.GetInt(reader, "FKIDTheme"),
                Name = SQLDataHelper.GetString(reader, "Name"),
                CountVoice = SQLDataHelper.GetInt(reader, "CountVoice"),
                Sort = SQLDataHelper.GetInt(reader, "Sort"),
                IsVisible = SQLDataHelper.GetBoolean(reader, "IsVisible"),
                DateAdded = SQLDataHelper.GetDateTime(reader, "DateAdded"),
                DateModify = SQLDataHelper.GetDateTime(reader, "DateModify")
            };
        }

        public static List<Answer> GetAllAnswers(int voiceThemeId)
        {
            List<Answer> answers =
                SQLDataAccess.ExecuteReadList<Answer>(
                    "SELECT * FROM [Voice].[Answer] WHERE [FKIDTheme] = @VoiceThemeID ORDER BY [Sort]",
                    CommandType.Text, GetAnswerFromReader, new SqlParameter("@VoiceThemeID", voiceThemeId));
            return answers;
        }

        public static void InsertAnswer(Answer answer)
        {
            answer.AnswerId =
                SQLDataAccess.ExecuteScalar<int>(
                    "INSERT INTO [Voice].[Answer] ([FKIDTheme], [Name], [CountVoice], [Sort], [IsVisible], [DateAdded], [DateModify]) VALUES (@FKIDTheme,  @Name,  @CountVoice,  @Sort,  @IsVisible,  @DateAdded,  @DateModify); SELECT scope_identity();",
                    CommandType.Text,
                    new SqlParameter("@FKIDTheme", answer.FkidTheme),
                    new SqlParameter("@Name", answer.Name),
                    new SqlParameter("@CountVoice", answer.CountVoice),
                    new SqlParameter("@Sort", answer.Sort),
                    new SqlParameter("@IsVisible", answer.IsVisible),
                    new SqlParameter("@DateAdded", DateTime.Now),
                    new SqlParameter("@DateModify", DateTime.Now));

            CacheManager.RemoveByPattern(CacheNames.Voiting);
        }

        public static void AddVote(int answerId)
        {
            SQLDataAccess.ExecuteNonQuery(
                "UPDATE [Voice].[Answer] SET [CountVoice] = [CountVoice] + 1 WHERE [AnswerID] = @AnswerID",
                CommandType.Text, new SqlParameter("@AnswerID", answerId));

            CacheManager.RemoveByPattern(CacheNames.Voiting);
        }

        public static void UpdateAnswer(Answer answer)
        {
            SQLDataAccess.ExecuteNonQuery(
                @"UPDATE [Voice].[Answer] SET [FKIDTheme] = @FKIDTheme,[Name] = @Name,[CountVoice] = @CountVoice, [Sort] = @Sort, [IsVisible] = @IsVisible, [DateModify] = @DateModify WHERE [AnswerID] = @AnswerID",
                CommandType.Text,
                new SqlParameter("@AnswerID", answer.AnswerId),
                new SqlParameter("@FKIDTheme", answer.FkidTheme),
                new SqlParameter("@Name", answer.Name),
                new SqlParameter("@CountVoice", answer.CountVoice),
                new SqlParameter("@Sort", answer.Sort),
                new SqlParameter("@IsVisible", answer.IsVisible),
                new SqlParameter("@DateModify", DateTime.Now));

            CacheManager.RemoveByPattern(CacheNames.Voiting);
        }

        public static void DeleteAnswer(int answerId)
        {
            SQLDataAccess.ExecuteNonQuery("DELETE FROM [Voice].[Answer] WHERE [AnswerID] = @AnswerID", CommandType.Text,
                new SqlParameter("@AnswerID", answerId));

            CacheManager.RemoveByPattern(CacheNames.Voiting);
        }

        #endregion Answers

        #region VoiceTheme
        
        private static VoiceTheme GetVoiceThemeFromReader(SqlDataReader reader)
        {
            return new VoiceTheme
            {
                VoiceThemeId = SQLDataHelper.GetInt(reader["VoiceThemeID"]),
                PsyId = SQLDataHelper.GetInt(reader["PSYID"]),
                Name = SQLDataHelper.GetString(reader["Name"]).Trim(),
                IsHaveNullVoice = SQLDataHelper.GetBoolean(reader["IsHaveNullVoice"]),
                IsDefault = SQLDataHelper.GetBoolean(reader["IsDefault"]),
                IsClose = SQLDataHelper.GetBoolean(reader["IsClose"]),
                DateAdded = SQLDataHelper.GetDateTime(reader["DateAdded"]),
                DateModify = SQLDataHelper.GetDateTime(reader["DateModify"]),
                CountVoice = SQLDataHelper.GetInt(reader["CountVoice"])
            };
        }

        public static VoiceTheme GetTopTheme()
        {
            return CacheManager.Get(CacheNames.Voiting,
                () => SQLDataAccess.ExecuteReadOne(
                    "SELECT TOP (1) [VoiceThemeID], [PSYID], [Name], [IsHaveNullVoice], [IsDefault], [IsClose], [DateAdded], [DateModify], (SELECT SUM([CountVoice]) FROM [Voice].[Answer] WHERE (FKIDTheme = [Voice].[VoiceTheme].[VoiceThemeID]) AND (IsVisible = 1)) AS [CountVoice] FROM [Voice].[VoiceTheme] ORDER BY [IsDefault] DESC, [PSYID] ASC",
                    CommandType.Text,
                    GetVoiceThemeFromReader));
        }

        public static List<int> GetThemeIDs()
        {
            return SQLDataAccess.ExecuteReadColumn<int>("SELECT [VoiceThemeID] FROM [Voice].[VoiceTheme]",
                CommandType.Text,
                "VoiceThemeID");
        }

        public static List<VoiceTheme> GetAllVoiceThemes()
        {
            return SQLDataAccess.ExecuteReadList<VoiceTheme>(
                "SELECT [VoiceThemeID], [PSYID], [Name], [IsHaveNullVoice], [IsDefault], [IsClose], [DateAdded], [DateModify], (SELECT SUM([CountVoice]) FROM [Voice].[Answer] WHERE (FKIDTheme = [Voice].[VoiceTheme].[VoiceThemeID]) AND (IsVisible = 1)) AS [CountVoice] FROM [Voice].[VoiceTheme] ORDER BY [IsDefault] DESC, [PSYID] ASC",
                CommandType.Text,
                GetVoiceThemeFromReader);
        }

        public static void AddTheme(VoiceTheme voiceTheme)
        {
            if (voiceTheme.IsDefault)
            {
                ResetIsDefault();
            }
            SQLDataAccess.ExecuteNonQuery(
                "INSERT INTO [Voice].[VoiceTheme] ([PsyID], [Name], [IsDefault], [IsHaveNullVoice], [IsClose], [DateAdded], [DateModify]) VALUES ( @PsyID, @Name, @IsDefault, @IsHaveNullVoice, @IsClose, GETDATE(), GETDATE())",
                CommandType.Text,
                new SqlParameter("@Name", voiceTheme.Name),
                new SqlParameter("@PsyID", voiceTheme.PsyId),
                new SqlParameter("@IsDefault", voiceTheme.IsDefault),
                new SqlParameter("@IsHaveNullVoice", voiceTheme.IsHaveNullVoice),
                new SqlParameter("@IsClose", voiceTheme.IsClose)
                );

            CacheManager.RemoveByPattern(CacheNames.Voiting);
        }

        public static void UpdateTheme(VoiceTheme voiceTheme)
        {
            if (voiceTheme.IsDefault)
            {
                ResetIsDefault();
            }
            SQLDataAccess.ExecuteNonQuery(
                "Update [Voice].[VoiceTheme] set [PsyID]=@PsyID, [Name]=@Name, [IsDefault]=@IsDefault, [IsHaveNullVoice]=@IsHaveNullVoice, [IsClose]=@IsClose, [DateModify]=GetDate() where VoiceThemeId = @VoiceThemeId",
                CommandType.Text,
                new SqlParameter("@PsyID", voiceTheme.PsyId),
                new SqlParameter("@Name", voiceTheme.Name),
                new SqlParameter("@IsDefault", voiceTheme.IsDefault),
                new SqlParameter("@IsHaveNullVoice", voiceTheme.IsHaveNullVoice),
                new SqlParameter("@IsClose", voiceTheme.IsClose),
                new SqlParameter("@VoiceThemeId", voiceTheme.VoiceThemeId)
                );

            CacheManager.RemoveByPattern(CacheNames.Voiting);
        }

        public static void DeleteTheme(int id)
        {
            SQLDataAccess.ExecuteNonQuery(
                "DELETE FROM [Voice].[VoiceTheme] WHERE [VoiceThemeID] = @VoiceThemeID",
                CommandType.Text,
                new SqlParameter("@VoiceThemeID", id));

            CacheManager.RemoveByPattern(CacheNames.Voiting);
        }

        public static string GetVotingName(int themeId)
        {
            return SQLDataAccess.ExecuteScalar<string>(
                "SELECT [Name] FROM [Voice].[VoiceTheme] WHERE [VoiceThemeID] = @Theme",
                CommandType.Text,
                new SqlParameter("@Theme", themeId));
        }

        public static string ResetIsDefault()
        {
            return SQLDataAccess.ExecuteScalar<string>(
                "Update [Voice].[VoiceTheme] SET [IsDefault] = 0 ",
                CommandType.Text);
        }

        #endregion VoiceTheme
    }
}