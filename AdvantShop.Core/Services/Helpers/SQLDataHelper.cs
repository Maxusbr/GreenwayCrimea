//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Data;

namespace AdvantShop.Helpers
{
    public class SQLDataHelper
    {
        /// <summary>
        /// Gets a boolean value of a data reader by a column name
        /// </summary>
        /// <param name="rdr">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns>A boolean value</returns>
        public static bool GetBoolean(IDataReader rdr, string columnName)
        {
            return GetBoolean(rdr, columnName, false);
        }

        /// <summary>
        /// Gets a boolean value of a data reader by a column name
        /// </summary>
        /// <param name="rdr">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <param name="defaultValue"></param>
        /// <returns>A boolean value</returns>
        public static bool GetBoolean(IDataReader rdr, string columnName, bool defaultValue)
        {
            int index = rdr.GetOrdinal(columnName);
            if (rdr.IsDBNull(index))
            {
                return defaultValue;
            }
            return Convert.ToBoolean(rdr.GetValue(index));
        }

        /// <summary>
        /// Gets a byte array of a data reader by a column name
        /// </summary>
        /// <param name="rdr">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns>A byte array</returns>
        public static byte[] GetBytes(IDataReader rdr, string columnName)
        {
            int index = rdr.GetOrdinal(columnName);
            if (rdr.IsDBNull(index))
            {
                return null;
            }
            return (byte[])rdr[index];
        }

        /// <summary>
        /// Gets a datetime value of a data reader by a column name
        /// </summary>
        /// <param name="rdr">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns>A date time</returns>
        public static DateTime GetDateTime(IDataReader rdr, string columnName)
        {
            return GetDateTime(rdr, columnName, DateTime.MinValue);
        }

        /// <summary>
        /// Gets a datetime value of a data reader by a column name
        /// </summary>
        /// <param name="rdr">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <param name="defaultValue"></param>
        /// <returns>A date time</returns>
        public static DateTime GetDateTime(IDataReader rdr, string columnName, DateTime defaultValue)
        {
            var index = rdr.GetOrdinal(columnName);
            if (rdr.IsDBNull(index))
            {
                return defaultValue;
            }
            return Convert.ToDateTime(rdr.GetValue(index));
        }

        /// <summary>
        /// Gets an UTC datetime value of a data reader by a column name
        /// </summary>
        /// <param name="rdr">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns>A date time</returns>
        public static DateTime GetUtcDateTime(IDataReader rdr, string columnName)
        {
            return GetUtcDateTime(rdr, columnName, DateTime.MinValue);
        }

        /// <summary>
        /// Gets an UTC datetime value of a data reader by a column name
        /// </summary>
        /// <param name="rdr">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <param name="defaultValue"></param>
        /// <returns>A date time if exists; otherwise, defaultValue</returns>
        public static DateTime GetUtcDateTime(IDataReader rdr, string columnName, DateTime defaultValue)
        {
            int index = rdr.GetOrdinal(columnName);
            if (rdr.IsDBNull(index))
            {
                return defaultValue;
            }
            return DateTime.SpecifyKind(Convert.ToDateTime(rdr[index]), DateTimeKind.Utc);
        }

        /// <summary>
        /// Gets a nullable datetime value of a data reader by a column name
        /// </summary>
        /// <param name="rdr">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns>A date time if exists; otherwise, null</returns>
        public static DateTime? GetNullableDateTime(IDataReader rdr, string columnName)
        {
            int index = rdr.GetOrdinal(columnName);
            if (rdr.IsDBNull(index))
            {
                return null;
            }
            return Convert.ToDateTime(rdr[index]);
        }

        /// <summary>
        /// Gets a nullable UTC datetime value of a data reader by a column name
        /// </summary>
        /// <param name="rdr">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns>A date time if exists; otherwise, null</returns>
        public static DateTime? GetNullableUtcDateTime(IDataReader rdr, string columnName)
        {
            int index = rdr.GetOrdinal(columnName);
            if (rdr.IsDBNull(index))
            {
                return null;
            }
            return DateTime.SpecifyKind(Convert.ToDateTime(rdr[index]), DateTimeKind.Utc);
        }

        /// <summary>
        /// Gets a decimal value of a data reader by a column name
        /// </summary>
        /// <param name="rdr">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns>A decimal value</returns>
        public static decimal GetDecimal(IDataReader rdr, string columnName)
        {
            return GetDecimal(rdr, columnName, decimal.Zero);
        }

        /// <summary>
        /// Gets a decimal value of a data reader by a column name
        /// </summary>
        /// <param name="rdr">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <param name="defaultValue"></param>
        /// <returns>A decimal value</returns>
        public static decimal GetDecimal(IDataReader rdr, string columnName, decimal defaultValue)
        {
            int index = rdr.GetOrdinal(columnName);
            if (rdr.IsDBNull(index))
            {
                return defaultValue;
            }
            return Convert.ToDecimal(rdr.GetValue(index));
        }

        /// <summary>
        /// Gets a double value of a data reader by a column name
        /// </summary>
        /// <param name="rdr">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns>A double value</returns>
        public static double GetDouble(IDataReader rdr, string columnName)
        {
            return GetDouble(rdr, columnName, 0.0);
        }

        /// <summary>
        /// Gets a double value of a data reader by a column name
        /// </summary>
        /// <param name="rdr">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <param name="defaultValue"></param>
        /// <returns>A double value</returns>
        public static double GetDouble(IDataReader rdr, string columnName, double defaultValue)
        {
            int index = rdr.GetOrdinal(columnName);
            if (rdr.IsDBNull(index))
            {
                return defaultValue;
            }
            return Convert.ToDouble(rdr.GetValue(index));
        }


        /// <summary>
        /// Gets a float value of a data reader by a column name
        /// </summary>
        /// <param name="rdr">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns>A float value</returns>
        public static float GetFloat(IDataReader rdr, string columnName)
        {
            return GetFloat(rdr, columnName, 0.0F);
        }

        /// <summary>
        /// Gets a float value of a data reader by a column name
        /// </summary>
        /// <param name="rdr">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <param name="defaultValue"></param>
        /// <returns>A float value</returns>
        public static float GetFloat(IDataReader rdr, string columnName, float defaultValue)
        {
            int index = rdr.GetOrdinal(columnName);
            if (rdr.IsDBNull(index))
            {
                return defaultValue;
            }
            return Convert.ToSingle(rdr.GetValue(index));
        }

        /// <summary>
        /// Gets a GUID value of a data reader by a column name
        /// </summary>
        /// <param name="rdr">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns>A GUID value</returns>
        public static Guid GetGuid(IDataReader rdr, string columnName)
        {
            int index = rdr.GetOrdinal(columnName);
            if (rdr.IsDBNull(index))
            {
                return Guid.Empty;
            }
            return (Guid) rdr.GetValue(index);
        }

        /// <summary>
        /// Gets an integer value of a data reader by a column name
        /// </summary>
        /// <param name="rdr">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns>An integer value</returns>
        public static int GetInt(IDataReader rdr, string columnName)
        {
            return GetInt(rdr, columnName, 0);
        }

        /// <summary>
        /// Gets an integer value of a data reader by a column name
        /// </summary>
        /// <param name="rdr">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <param name="defaultValue"></param>
        /// <returns>An integer value</returns>
        public static int GetInt(IDataReader rdr, string columnName, int defaultValue)
        {
            int index = rdr.GetOrdinal(columnName);
            if (rdr.IsDBNull(index))
            {
                return defaultValue;
            }
            return Convert.ToInt32(rdr.GetValue(index));
        }

        public static long GetLong(IDataReader rdr, string columnName)
        {
            return GetLong(rdr, columnName, 0);
        }

        public static long GetLong(IDataReader rdr, string columnName, int defaultValue)
        {
            int index = rdr.GetOrdinal(columnName);
            if (rdr.IsDBNull(index))
            {
                return defaultValue;
            }
            return Convert.ToInt64(rdr.GetValue(index));
        }

        /// <summary>
        /// Gets an long value of a object
        /// </summary>
        /// <param name="obj">object</param>
        /// <returns>An integer value</returns>
        public static long GetLong(object obj)
        {
            return GetLong(obj, 0);
        }

        /// <summary>
        /// Gets an long value of a object
        /// </summary>
        /// <param name="obj">object</param>        
        /// <param name="defaultValue"></param>
        /// <returns>An integer value</returns>
        public static long GetLong(object obj, int defaultValue)
        {
            if (obj == DBNull.Value || obj == null)
            {
                return defaultValue;
            }
            return Convert.ToInt64(obj);
        }


        /// <summary>
        /// Gets a nullable integer value of a data reader by a column name
        /// </summary>
        /// <param name="rdr">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns>A nullable integer value</returns>
        public static int? GetNullableInt(IDataReader rdr, string columnName)
        {
            int index = rdr.GetOrdinal(columnName);
            if (rdr.IsDBNull(index))
            {
                return null;
            }
            return rdr.GetInt32(index);//Convert.ToInt32(rdr[index]);
        }

        public static long? GetNullableLong(IDataReader rdr, string columnName)
        {
            int index = rdr.GetOrdinal(columnName);
            if (rdr.IsDBNull(index))
            {
                return null;
            }
            return rdr.GetInt64(index);//Convert.ToInt64(rdr[index]);
        }

        /// <summary>
        /// Gets a string of a data reader by a column name
        /// </summary>
        /// <param name="rdr">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns>A string value</returns>
        public static string GetString(IDataReader rdr, string columnName)
        {
            return GetString(rdr, columnName, string.Empty);
        }

        /// <summary>
        /// Gets a string of a data reader by a column name
        /// </summary>
        /// <param name="rdr">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <param name="defaultValue"></param>
        /// <returns>A string value</returns>
        public static string GetString(IDataReader rdr, string columnName, string defaultValue)
        {
            int index = rdr.GetOrdinal(columnName);
            if (rdr.IsDBNull(index))
            {
                return defaultValue;
            }
            return Convert.ToString(rdr.GetValue(index));
        }
        

        ////////****** convert Evals 

        /// <summary>
        /// Gets a string of a object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>A string value</returns>
        public static string GetString(object obj)
        {
            return GetString(obj, string.Empty);
        }

        public static object GetObject(IDataReader rdr, string columnName)
        {
            int index = rdr.GetOrdinal(columnName);
            if (rdr.IsDBNull(index))
            {
                return null;
            }
            return rdr.GetValue(index);// rdr[index];
        }

        /// <summary>
        /// Gets a string of a object
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns>A string value</returns>
        public static string GetString(object obj, string defaultValue)
        {
            if (obj == DBNull.Value || obj == null)
            {
                return defaultValue;
            }
            return Convert.ToString(obj);
        }

        /// <summary>
        /// Gets an integer value of a object
        /// </summary>
        /// <param name="obj">object</param>
        /// <returns>An integer value</returns>
        public static int GetInt(object obj)
        {
            return GetInt(obj, 0);
        }

        /// <summary>
        /// Gets an integer value of a object
        /// </summary>
        /// <param name="obj">object</param>        
        /// <param name="defaultValue"></param>
        /// <returns>An integer value</returns>
        public static int GetInt(object obj, int defaultValue)
        {
            if (obj == DBNull.Value || obj == null)
            {
                return defaultValue;
            }
            return Convert.ToInt32(obj);
        }

        /// <summary>
        /// Gets a nullable integer value of a object
        /// </summary>
        /// <param name="obj">Object</param>        
        /// <returns>A nullable integer value</returns>
        public static int? GetNullableInt(object obj)
        {
            if (obj == DBNull.Value || obj == null)
            {
                return null;
            }
            return Convert.ToInt32(obj);
        }

        /// <summary>
        /// Gets a GUID value of a object
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="columnName"></param>
        /// <returns>A GUID value</returns>
        public static Guid GetGuid(object obj, string columnName)
        {
            if (obj == DBNull.Value || obj == null)
            {
                return Guid.Empty;
            }
            return (Guid)obj;
        }

        /// <summary>
        /// Gets a GUID value of a object
        /// </summary>
        /// <param name="rdr">Data reader</param>
        /// <param name="columnName"></param>
        /// <returns>A GUID value</returns>
        public static Guid? GetNullableGuid(IDataReader rdr, string columnName)
        {
            int index = rdr.GetOrdinal(columnName);
            if (rdr.IsDBNull(index))
            {
                return null;
            }
            return (Guid)(rdr.GetValue(index));
        }

        /// <summary>
        /// Gets a GUID value of a object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>A GUID value</returns>
        public static Guid? GetNullableGuid(object obj)
        {
            if (obj == DBNull.Value || obj == null)
            {
                return null;
            }
            return (Guid)obj;
        }

        /// <summary>
        /// Gets a float value of a object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>A float value</returns>
        public static decimal GetDecimal(object obj)
        {
            return GetDecimal(obj, decimal.Zero);
        }

        /// <summary>
        /// Gets a float value of a object
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns>A float value</returns>
        public static decimal GetDecimal(object obj, decimal defaultValue)
        {
            if (obj == DBNull.Value || obj == null)
            {
                return defaultValue;
            }
            return Convert.ToDecimal(obj);
        }

        /// <summary>
        /// Gets a double value of a object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>A double value</returns>
        public static double GetDouble(object obj)
        {
            return GetDouble(obj, 0.0);
        }

        /// <summary>
        /// Gets a double value of a object
        /// </summary>
        /// <param name="obj">Object</param>        
        /// <param name="defaultValue"></param>
        /// <returns>A double value</returns>
        public static double GetDouble(object obj, double defaultValue)
        {
            if (obj == DBNull.Value || obj == null)
            {
                return defaultValue;
            }
            return Convert.ToDouble(obj);
        }


        /// <summary>
        /// Gets a double value of a object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>A float value</returns>
        public static float GetFloat(object obj)
        {
            return GetFloat(obj, 0F);
        }

        /// <summary>
        /// Gets a double value of a object
        /// </summary>
        /// <param name="obj">Object</param>        
        /// <param name="defaultValue"></param>
        /// <returns>A double value</returns>
        public static float GetFloat(object obj, float defaultValue)
        {
            if (obj == DBNull.Value || obj == null)
            {
                return defaultValue;
            }
            return Convert.ToSingle(obj);
        }

        /// <summary>
        /// Gets a nullable float value of a object
        /// </summary>
        /// <param name="obj">Object</param>        
        /// <returns>A nullable integer value</returns>
        public static float? GetNullableFloat(object obj)
        {
            if (obj == DBNull.Value || obj == null)
            {
                return null;
            }
            return Convert.ToSingle(obj);
        }

        /// <summary>
        /// Gets a nullable float value of a data reader by a column name
        /// </summary>
        /// <param name="rdr">Data reader</param>
        /// <param name="columnName">Column name</param>
        /// <returns>A nullable float value</returns>
        public static float? GetNullableFloat(IDataReader rdr, string columnName)
        {
            int index = rdr.GetOrdinal(columnName);
            if (rdr.IsDBNull(index))
            {
                return null;
            }
            return Convert.ToSingle(rdr.GetValue(index));
        }

        /// <summary>
        /// Return a boolean value of DBNull object or not
        /// </summary>
        /// <param name="obj"></param>        
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsDbNull(object obj)
        {
            if (obj == DBNull.Value || obj == null)
                return true;
            return false;
        }

        /// <summary>
        /// Gets a boolean value of a data reader by a column name
        /// </summary>
        /// <param name="obj">Object</param>        
        /// <returns>A boolean value</returns>
        public static bool GetBoolean(object obj)
        {
            return GetBoolean(obj, false);
        }

        /// <summary>
        /// Gets a guid value of a data reader by a column name
        /// </summary>
        /// <param name="obj">Object</param>        
        /// <param name="defaultValue"></param>
        /// <returns>A guid value</returns>
        public static Guid GetGuid(object obj, Guid defaultValue)
        {
            if (obj == DBNull.Value || obj == null)
            {
                return defaultValue;
            }
            return (Guid)obj;
        }


        /// <summary>
        /// Gets a guid value of a data reader by a column name
        /// </summary>
        /// <param name="obj">Object</param>        
        /// <returns>A guid value</returns>
        public static Guid GetGuid(object obj)
        {
            return GetGuid(obj, Guid.Empty);
        }

        /// <summary>
        /// Gets a boolean value of a data reader by a column name
        /// </summary>
        /// <param name="obj">Object</param>        
        /// <param name="defaultValue"></param>
        /// <returns>A boolean value</returns>
        public static bool GetBoolean(object obj, bool defaultValue)
        {
            if (obj == DBNull.Value || obj == null)
            {
                return defaultValue;
            }
            return Convert.ToBoolean(obj);
        }

        public static bool GetBoolean(string obj)
        {
            if (String.IsNullOrEmpty(obj))
                return false;

            return Convert.ToBoolean(obj);
        }

        public static bool? GetNullableBoolean(IDataReader rdr, string columnName)
        {
            int index = rdr.GetOrdinal(columnName);
            if (rdr.IsDBNull(index))
            {
                return null;
            }
            return Convert.ToBoolean(rdr.GetValue(index));
        }

        public static bool? GetNullableBoolean(object obj)
        {
            if (obj == DBNull.Value || obj == null)
            {
                return null;
            }
            return Convert.ToBoolean(obj);
        }

        /// <summary>
        /// Gets a byte array of a object
        /// </summary>
        /// <param name="obj">Object</param>        
        /// <returns>A byte array</returns>
        public static byte[] GetBytes(object obj)
        {
            if (obj == DBNull.Value || obj == null)
            {
                return null;
            }
            return (byte[])obj;
        }

        /// <summary>
        /// Gets a datetime value of a object
        /// </summary>
        /// <param name="obj">Object</param>        
        /// <returns>A date time</returns>
        public static DateTime GetDateTime(object obj)
        {
            return GetDateTime(obj, DateTime.MinValue);
        }

        /// <summary>
        /// Gets a datetime value of a object
        /// </summary>
        /// <param name="obj">Object</param>        
        /// <param name="defaultValue"></param>
        /// <returns>A date time</returns>
        public static DateTime GetDateTime(object obj, DateTime defaultValue)
        {

            if (obj == DBNull.Value || obj == null)
            {
                return defaultValue;
            }
            return Convert.ToDateTime(obj);
        }

        /// <summary>
        /// Gets an UTC datetime value of a object
        /// </summary>
        /// <param name="obj">Object</param>        
        /// <returns>A date time</returns>
        public static DateTime GetUtcDateTime(object obj)
        {
            return GetUtcDateTime(obj, DateTime.MinValue);
        }

        /// <summary>
        /// Gets an UTC datetime value of a object
        /// </summary>
        /// <param name="obj">object</param>        
        /// <param name="defaultValue"></param>
        /// <returns>A date time if exists; otherwise, defaultValue</returns>
        public static DateTime GetUtcDateTime(object obj, DateTime defaultValue)
        {
            if (obj == DBNull.Value || obj == null)
            {
                return defaultValue;
            }
            return DateTime.SpecifyKind(Convert.ToDateTime(obj), DateTimeKind.Utc);
        }

        /// <summary>
        /// Gets a nullable datetime value of a object
        /// </summary>
        /// <param name="obj">Object</param>        
        /// <returns>A date time if exists; otherwise, null</returns>
        public static DateTime? GetNullableDateTime(object obj)
        {
            if (obj == DBNull.Value || obj == null)
            {
                return null;
            }
            return Convert.ToDateTime(obj);
        }

        /// <summary>
        /// Gets a nullable UTC datetime value of a object
        /// </summary>
        /// <param name="obj">Object</param>        
        /// <returns>A date time if exists; otherwise, null</returns>
        public static DateTime? GetNullableUtcDateTime(object obj)
        {
            if (obj == DBNull.Value || obj == null)
            {
                return null;
            }
            return DateTime.SpecifyKind(Convert.ToDateTime(obj), DateTimeKind.Utc);
        }

        public static TResult GetValue<TResult>(IDataReader rdr, string columnName) where TResult : IConvertible
        {
            return (TResult)Convert.ChangeType(rdr[columnName], typeof(TResult));
        }

        public static TResult GetValue<TResult>(IDataReader rdr, string columnName, TResult defaultValue) where TResult : IConvertible
        {
            int index = rdr.GetOrdinal(columnName);
            if (rdr.IsDBNull(index))
            {
                return defaultValue;
            }
            return (TResult)Convert.ChangeType(rdr.GetValue(index), typeof(TResult));
        }
    }
}