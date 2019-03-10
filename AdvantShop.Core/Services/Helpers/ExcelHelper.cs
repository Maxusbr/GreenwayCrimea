//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.Odbc;
using System.IO;
using System.Text;

namespace AdvantShop.Helpers
{
    public enum ExcelHelperReadMode
    {
        ReadWorkSheet,
        ReadNamedRange
    }

    public interface IExcelHelper
    {
        string ConnectionString { get; }        
        OleDbConnection Connection { get; }
        bool Header { get; set; }
        bool Intermixed { get; set; }
        DataTable GetSchema();
        List<string> GetTablesNamesList();
        OleDbDataReader ReadTable(string tableName);
        OleDbDataReader ReadTable(string tableName, ExcelHelperReadMode mode);
        OleDbDataReader ReadTable(string tableName, ExcelHelperReadMode mode, string criteria);
        int GetCountRows(string tableName, ExcelHelperReadMode mode, string criteria);
        void CreateTable(string tableName, Dictionary<string, OleDbType> tableDefinition);
        void AddNewRow(string tableName, Dictionary<string, string> valueDict, Dictionary<string, OleDbType> tableDefinition);
        void ExecuteCommand(string command);
    }

    public class ExcelHelper : IDisposable, IExcelHelper
    {
        public const string ExcelConnectionString =
            "Provider=Microsoft.{0}.OLEDB.{1};Data Source={2};Extended Properties=\"Excel {3};HDR={4};IMEX={5}\";";

        private const string ExcelOdbcConnectionString =
            "Driver=Microsoft Excel Driver ({0});DriverId=790;DBQ={1};HDR={2};Format={3}; ReadOnly=0;";

        private OleDbConnection _con;        

        private string _filepath = string.Empty;

        //"HDR=Yes;" indicates that the first row contains columnnames, not data. "HDR=No;" indicates the opposite.
        //"IMEX=1;" tells the driver to always read "intermixed" (numbers, dates, strings etc) data columns as text. Note that this option might affect excel sheet write access negative.
        private string _hdr = "NO";
        private string _imex = "0";
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filepath">file name</param>
        public ExcelHelper(string filepath)
        {
            _filepath = filepath;
        }

        /// <summary>
        /// get oleDb connection string
        /// </summary>
        public string ConnectionString
        {
            get
            {
                string result = string.Empty;
                if (String.IsNullOrEmpty(_filepath))
                    return result;

                //Check for File Format
                var finfo = new FileInfo(_filepath);
                if (finfo.Extension.Equals(".xls"))
                {
                    result = string.Format(ExcelConnectionString, "Jet", "4.0", _filepath, "8.0", _hdr, _imex);                
                }
                if(finfo.Extension.Equals(".xlsx"))                
                {
                    result = string.Format(ExcelConnectionString, "Ace", "12.0", _filepath, "12.0", _hdr, _imex);
                }
                return result;
            }
        }

        public OleDbConnection Connection
        {
            get
            {
                if (_con == null)
                {
                    _con = new OleDbConnection {ConnectionString = ConnectionString};                    
                }
                return _con;
            }
        }

        /// <summary>
        /// "TRUE" indicates that the first row contains columnnames, not data. "FALSE" indicates the opposite.
        /// Default "false"
        /// </summary>
        public bool Header
        {
            get { 
                if(_hdr == "yes")
                    return true;
                else
                return false; }
            set
            {
                if (value == true)
                    _hdr = "YES";
                else
                    _hdr = "NO";
            }
        }
        
        /// <summary>
        ///  "TRUE" tells the driver to always read "intermixed", only read! 
        ///  Default "false"
        /// </summary>
        public bool Intermixed
        {
            get
            {
                if (_imex == "1")
                    return true;
                else
                    return false;
            }
            set
            {
                if (value == true)
                    _imex = "1";
                else
                    _imex = "0";
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (_con != null && _con.State == ConnectionState.Open)
                _con.Close();
            if (_con != null)
                _con.Dispose();
            _con = null;        
            _filepath = string.Empty;
        }

        #endregion
                
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable GetSchema()
        {
            DataTable dtSchema = null;
            if (this.Connection.State != ConnectionState.Open) this.Connection.Open();
            dtSchema = this.Connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
            return dtSchema;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<string> GetTablesNamesList()
        {
            DataTable dt = GetSchema();

            List<string> excelsheets = new List<string>();
            
            foreach (DataRow row in dt.Rows)
            {
                excelsheets.Add(row["TABLE_NAME"].ToString().Substring(0,row["TABLE_NAME"].ToString().Length));                
            }
            return excelsheets;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, OleDbDataReader> ReadTables()
        {
            if (Connection.State != ConnectionState.Open)
            {
                Connection.Open();
            }
            string cmdText = "Select * from [{0}]";
            
            DataTable dt = GetSchema();

            Dictionary<string, OleDbDataReader> readers = new Dictionary<string, OleDbDataReader>();
                        
            foreach (DataRow row in dt.Rows)
            {
                var cmd = new OleDbCommand(string.Format(cmdText, row["TABLE_NAME"].ToString())) { Connection = Connection };
                readers.Add(row["TABLE_NAME"].ToString().Substring(0, row["TABLE_NAME"].ToString().Length - 1), cmd.ExecuteReader());               
            }
            return readers;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public OleDbDataReader ReadTable(string tableName)
        {
            return ReadTable(tableName, ExcelHelperReadMode.ReadWorkSheet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public OleDbDataReader ReadTable(string tableName, ExcelHelperReadMode mode)
        {
            return ReadTable(tableName, mode, "");
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="mode"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public OleDbDataReader ReadTable(string tableName, ExcelHelperReadMode mode, string criteria)
        {
            if (Connection.State != ConnectionState.Open)
            {
                Connection.Open();
            }
            string cmdText = "Select * from [{0}]";
            if (!string.IsNullOrEmpty(criteria))
            {
                cmdText += " Where " + criteria;
            }

            string tableNameSuffix = string.Empty;
            if (mode == ExcelHelperReadMode.ReadWorkSheet)
                tableNameSuffix = "$";

            var cmd = new OleDbCommand(string.Format(cmdText, tableName + tableNameSuffix)) { Connection = Connection };
            OleDbDataReader read = cmd.ExecuteReader();

            //OleDbDataAdapter adpt = new OleDbDataAdapter(cmd);

            //DataSet ds = new DataSet();

            //adpt.Fill(ds, tableName);

            //if (ds.Tables.Count >= 1)
            //{
            //    return ds.Tables[0];
            //}
            //else
            //{
            //    return null;
            //}
            return read;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="mode"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public int GetCountRows(string tableName, ExcelHelperReadMode mode, string criteria)
        {
            if (Connection.State != ConnectionState.Open)
            {
                Connection.Open();
            }
            string cmdText = "Select Count(*) from [{0}]";
            if (!string.IsNullOrEmpty(criteria))
            {
                cmdText += " Where " + criteria;
            }
            string tableNameSuffix = string.Empty;
            if (mode == ExcelHelperReadMode.ReadWorkSheet)
                tableNameSuffix = "$";

            var cmd = new OleDbCommand(string.Format(cmdText, tableName + tableNameSuffix)) { Connection = Connection };
            var count = (int) cmd.ExecuteScalar();            
            return count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        public void DropTable(string tableName)
        {
            if (this.Connection.State != ConnectionState.Open)
            {
                this.Connection.Open();
            }
            string cmdText = "Drop Table [{0}]";
            using (OleDbCommand cmd = new OleDbCommand(string.Format(cmdText, tableName), this.Connection))
            {
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (OleDbException ex)
                {
                    this.Connection.Close();
                    throw new Exception(ex.Message);
                }              
            }
            this.Connection.Close();
        }

        /// <summary>
        /// Create table "tableName"
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="tableDefinition"></param>
        public void CreateTable(string tableName, Dictionary<string, OleDbType> tableDefinition)
        {
            if (this.Connection.State != ConnectionState.Open)
            {
                this.Connection.Open();
            }

            List<string> tables = GetTablesNamesList();
            foreach (string table in tables)
            {
                if (table == tableName)
                {
                    return;
                }
            }
            using (var cmd = new OleDbCommand(GenerateTable(tableName, tableDefinition), Connection))
            {
                cmd.ExecuteNonQuery();
            }
            this.Connection.Close();
        }

         public void AddNewRow(string tableName, Dictionary<string, string> valueDict, Dictionary<string, OleDbType> tableDefinition)
        {            
            var read = ReadTable(tableName);
            read.GetDataTypeName(0);
            string command = InsertRowString(tableName, valueDict, read);
            
            using (var cmd = new OleDbCommand(command, Connection))
            {                
                for (int i = 0; i < read.FieldCount; ++i)
                {                    
                    cmd.Parameters.Add("@" + read.GetName(i), tableDefinition[read.GetName(i)]).Value = valueDict[read.GetName(i)];
                }
                  
                if (Connection.State != ConnectionState.Open) Connection.Open();
                cmd.ExecuteNonQuery();
                read.Close();
            }
        }

        public void ExecuteCommand(string command)
        {
            using (var cmd = new OleDbCommand(command, Connection))
            {                
                if (Connection.State != ConnectionState.Open) Connection.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private static string GenerateTable(string tableName, Dictionary<string, OleDbType> tableDefinition)
        {
            var sb = new StringBuilder();
            bool firstcol = true;
            sb.AppendFormat("CREATE TABLE [{0}](", tableName);
            foreach (var keyvalue in tableDefinition)
            {
                if (!firstcol)
                {
                    sb.Append(",");
                }
                firstcol = false;
                sb.AppendFormat("{0} {1}", keyvalue.Key, OleDbTypeToString(keyvalue.Value));
            }

            sb.Append(")");
            return sb.ToString();
        }

        private static string InsertRowString(string tableName, Dictionary<string, string> valueDict, OleDbDataReader read)
        {
            var sb = new StringBuilder();
            //bool firstcol = true;            
            sb.AppendFormat("INSERT INTO [{0}](", tableName);

            for (int i = 0; i < read.FieldCount; ++i)
            {
                if (i != 0)
                {
                    sb.Append(",");
                }                
                sb.Append(read.GetName(i));
            }

            sb.Append(") VALUES(");
            for (int i = 0; i < read.FieldCount; ++i)
            {
                sb.Append("@" + read.GetName(i));
                if (i != read.FieldCount - 1)
                {
                    sb.Append(",");
                }
            }
            sb.Append(")");
            return sb.ToString();
        }

        private static string OleDbTypeToString(OleDbType type)
        {
            switch (type)
            {
                case OleDbType.Char | OleDbType.LongVarChar | OleDbType.LongVarWChar | 
                OleDbType.VarChar | OleDbType.WChar| OleDbType.VarWChar:
                    return "TEXT";                
                case OleDbType.Currency:
                    return "MONEY";
                case OleDbType.Numeric:
                    return "DECIMAL";
                case OleDbType.Integer:
                    return "INTEGER";
                case OleDbType.Binary:
                    return "TINYINT";
                case OleDbType.Double:
                    return "FLOAT";
                case OleDbType.DBDate:
                    return "DATETIME";
                case OleDbType.Guid:
                    return "UNIQUEIDENTIFIER";
                case OleDbType.Boolean:
                    return "BIT";
            }
            return "TEXT";
        }
    }
}