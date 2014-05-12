/*
* @(#)BaseDAO.cs
*
* Copyright (c) 2014, VizitSolutions.
* All rights reserved.
*
* Use is subject to license terms. This software is protected by
* copyright law and international treaties. Unauthorized reproduction or
* distribution of this program, or any portion of it, may result in severe
* civil and criminal penalties, and will be prosecuted to the maximum extent.
*/

using System;
using System.Collections;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using Com.VizApp.VizApp.Arch;
using Com.VizApp.Arch.Logging;
using Com.VizApp.Arch.Api;

namespace Com.VizApp.Arch.Base
{
    public class BaseDAO
    {
        protected string CaseSensitiveSearchKey()
        {
            return "COLLATE Latin1_General_CS_AS";
        }

        private Database GetDataBase()
        {
            return EnterpriseLibraryContainer.Current.GetInstance<Database>();
        }

        public IDataReader ExecuteReader(string query)
        {
            var database = GetDataBase();
            IDataReader reader = database.ExecuteReader(CommandType.Text, query);
            return reader;
        }

        public IDataReader ExecuteReader(string query, object[] inputParams)
        {
            var database = GetDataBase();
            IDataReader reader = null;

            using (var sqlCmd = database.GetSqlStringCommand(query))
            {
                foreach (DbParameter parm in inputParams)
                {
                    database.AddInParameter(sqlCmd, parm.ParameterName, parm.DbType, parm.Value);
                }
                reader = database.ExecuteReader(sqlCmd);
            }
            return reader;
        }

        public DataSet ExecuteDataSet(string storedProcedureName, System.Data.SqlClient.SqlParameter[] inputParams)
        {
            var database = GetDataBase();
            DbCommand dbcommand = database.GetStoredProcCommand(storedProcedureName);
            dbcommand.Parameters.AddRange(inputParams);
            return database.ExecuteDataSet(dbcommand);
        }

        public object ExecuteScalar(string query)
        {
            var database = GetDataBase();
            return database.ExecuteScalar(CommandType.Text, query);
        }

        public object ExecuteScalar(string query, object[] inputParams)
        {
            var database = GetDataBase();
            object output = null;

            using (var sqlCmd = database.GetSqlStringCommand(query))
            {
                foreach (DbParameter parm in inputParams)
                {
                    database.AddInParameter(sqlCmd, parm.ParameterName, parm.DbType, parm.Value);
                }
                output = database.ExecuteScalar(sqlCmd);
            }
            return output;
        }

        public int ExecuteNonQuery(string query)
        {
            var database = GetDataBase();
            return database.ExecuteNonQuery(CommandType.Text, query);
        }

        public int ExecuteNonQuery(string query, object[] inputParams)
        {
            var database = GetDataBase();
            int output;

            using (var sqlCmd = database.GetSqlStringCommand(query))
            {
                foreach (DbParameter parm in inputParams)
                {
                    database.AddInParameter(sqlCmd, parm.ParameterName, parm.DbType, parm.Value);
                }
                output = database.ExecuteNonQuery(sqlCmd);
            }
            return output;
        }

        public long ExecuteNonQueryWithOutputParam(string query, object[] inputParams)
        {
            var database = GetDataBase();
            int result;
            long identity;

            using (var sqlCmd = database.GetSqlStringCommand(query))
            {
                foreach (DbParameter parm in inputParams)
                {
                    database.AddInParameter(sqlCmd, parm.ParameterName, parm.DbType, parm.Value);
                }

                database.AddOutParameter(sqlCmd, "@outputid", DbType.Int32, 4);
                result = database.ExecuteNonQuery(sqlCmd);
                identity = Convert.ToInt64(database.GetParameterValue(sqlCmd, "@outputid"));
            }
            return identity;
        }

        //public Dictionary<string, int> ExecuteNonQueryWithOutputParam(string query, object[] inputParams)
        //{
        //    var database = GetDataBase();
        //    Dictionary<string, int> output = new Dictionary<string, int>();
        //    int result;
        //    int identity;

        //    using (var sqlCmd = database.GetSqlStringCommand(query))
        //    {
        //        foreach (DbParameter parm in inputParams)
        //        {
        //            database.AddInParameter(sqlCmd, parm.ParameterName, parm.DbType, parm.Value);
        //        }

        //        database.AddOutParameter(sqlCmd, "@outputid", DbType.Int32, 4);
        //        result = database.ExecuteNonQuery(sqlCmd);
        //        identity = Convert.ToInt32(database.GetParameterValue(sqlCmd, "@outputid"));
        //        output.Add("result", result);
        //        output.Add("identity", identity);
        //    }
        //    return output;
        //}

        public DbParameter[] ConvertToDBParameters(Dictionary<string, object> inputParams)
        {
            List<System.Data.SqlClient.SqlParameter> sqlParams = new List<System.Data.SqlClient.SqlParameter>();
            foreach (string key in inputParams.Keys)
            {
                System.Data.SqlClient.SqlParameter param = new System.Data.SqlClient.SqlParameter(key, inputParams[key]);
                sqlParams.Add(param);
            }
            return sqlParams.ToArray();
        }
        
        public bool IsDBNull(object inputObject)
        {
            return System.Convert.IsDBNull(inputObject);
        }

        public void release(IDataReader reader)
        {
            if (reader != null)
            {
                reader.Close();
            }
        }

        public string asString(IEnumerable keys)
        {
            StringBuilder value = new StringBuilder();

            foreach (var key in keys) // Loop through all strings
            {
                value.Append(key).Append(",");
            }

            string result = value.ToString();
            return result.Substring(0, value.Length - 1);
        }

        public string asCommentedString(IEnumerable keys)
        {
            StringBuilder value = new StringBuilder();

            foreach (var key in keys) // Loop through all strings
            {
                value.Append(key).Append("','");
            }

            string result = value.ToString();
            return result.Substring(0, value.Length - 3);
        }

        public Dictionary<int, string> DefaultSettings
        {
            get
            {
                Dictionary<int, string> defaults = new Dictionary<int, string>();
                defaults.Add(1, "5");
                defaults.Add(2, "0");
                defaults.Add(3, "0");
                defaults.Add(4, "0");
                return defaults;
            }
        }

        public long CheckFBUser(FBUser User)
        {

            IDataReader reader = null;
            try
            {
                bool isFBUser = !string.IsNullOrEmpty(User.id);
                string query = string.Format(
                    isFBUser ? BaseQueries.CHECK_FB_USER : BaseQueries.CHECK_USER,
                    isFBUser ? User.id : User.Email);

                reader = ExecuteReader(query);

                using (reader)
                {
                    while (reader.Read())
                    {
                        return Convert.ToInt64(reader["id"]);
                    }
                }
                return 0;
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                throw new AppException(ErrorCode.SQL_GENERAL_ERROR);
            }
        }

        public Settings GetSettings(long UserId)
        {
            IDataReader reader = null;
            try
            {
                string query = string.Format(BaseQueries.USER_SETTINGS, UserId);

                reader = ExecuteReader(query);

                Settings settings = new Settings();
                int settingsId = 0;
                int value = 0;
                using (reader)
                {
                    while (reader.Read())
                    {
                        settingsId = Convert.ToInt32(Convert.ToString(reader["S_ID"]));
                        value = Convert.ToInt32(reader["VALUE"]);
                        switch (settingsId)
                        {
                            case 1:
                                settings.LocationUpdate = value;
                                break;
                            case 2:
                                settings.AutoLogin = (value == 1);
                                break;
                            case 3:
                                settings.Sound = (value == 1);
                                break;
                            case 4:
                                settings.Vibrate = (value == 1);
                                break;
                        }
                    }
                }
                return settings;
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                throw new AppException(ErrorCode.SQL_GENERAL_ERROR);
            }
        }

        protected void SaveDefaultSettings(long userId)
        {
            try
            {
                bool userExists = CheckUserInSettings(userId);

                if (userExists) return;

                Dictionary<int, string> settings = DefaultSettings;
                Dictionary<string, object> queryparams = null;
                foreach (int key in settings.Keys)
                {
                    queryparams = new Dictionary<string, object>();
                    queryparams.Add("@user_id", userId);
                    queryparams.Add("@settings_id", key);
                    queryparams.Add("@value", settings[key]);

                    ExecuteNonQueryWithOutputParam(BaseQueries.SAVE_SETTINGS,
                        ConvertToDBParameters(queryparams));
                }
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                throw new AppException(ErrorCode.SQL_GENERAL_ERROR);
            }
        }

        private bool CheckUserInSettings(long userId)
        {
            IDataReader reader = null;
            try
            {
                string query = string.Format(BaseQueries.CHECK_USER_SETTINGS, userId);

                reader = ExecuteReader(query);

                using (reader)
                {
                    while (reader.Read())
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                throw new AppException(ErrorCode.SQL_GENERAL_ERROR);
            }
        }

        class BaseQueries
        {
            public const string CHECK_FB_USER = "SELECT * FROM FB_USER " +
                                            "WHERE FB_ID='{0}'";

            public const string USER_SETTINGS = "SELECT * FROM USER_SETTINGS " +
                                            "WHERE U_ID={0}";

            public const string CHECK_USER = "SELECT * FROM FB_USER " +
                                            "WHERE EMAIL='{0}'";

            public const string SAVE_SETTINGS = "INSERT INTO USER_SETTINGS" +
                                            "(U_ID, S_ID, VALUE)" +
                                            "VALUES" +
                                            "(@user_id, @settings_id, @value); " +
                                            " SET @outputid=SCOPE_IDENTITY()";

            public const string CHECK_USER_SETTINGS = "SELECT * FROM USER_SETTINGS " +
                                            "WHERE U_ID={0}";
        }
    }
}
