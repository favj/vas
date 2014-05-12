/*
* @(#)VizFriendsDAO.cs
*
* Copyright (c) 2014, VizitSolutions.
* All rights reserved.
*
* Use is subject to license terms. This software is protected by
* copyright law and international treaties. Unauthorized reproduction or
* distribution of this program, or any portion of it, may result in severe
* civil and criminal penalties, and will be prosecuted to the maximum extent.
*/

using Com.VizApp.Arch;
using Com.VizApp.Arch.Api;
using Com.VizApp.Arch.Base;
using Com.VizApp.Arch.Logging;
using Com.VizApp.VizApp.Arch;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Com.VizApp.VizApp.Service.Dao
{
    public class VizDAO : BaseDAO
    {
        public Settings GetUserSettings(FBUser user)
        {
            long userId = CheckFBUser(AppRequestContext.FBUser);
            return GetSettings(userId);
        }

        public Settings UpdateSettings(Settings setting)
        {
            long userId = CheckFBUser(AppRequestContext.FBUser);

            try
            {
                Dictionary<string, object> queryparams = null;
                foreach (int key in DefaultSettings.Keys)
                {
                    string value = string.Empty;
                    switch (key)
                    {
                        case 1:
                            value = Convert.ToString(setting.LocationUpdate);
                            break;
                        case 2:
                            value = setting.AutoLogin ? "1" : "0";
                            break;
                        case 3:
                            value = setting.Sound ? "1" : "0";
                            break;
                        case 4:
                            value = setting.Vibrate ? "1" : "0";
                            break;
                    }
                    queryparams = new Dictionary<string, object>();
                    queryparams.Add("@u_id", userId);
                    queryparams.Add("@s_id", key);
                    queryparams.Add("@value", value);

                    ExecuteNonQuery(VizQueries.UPDATE_USER_SETTINGS,
                    ConvertToDBParameters(queryparams));
                }

                return GetSettings(userId);
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                throw new AppException(ErrorCode.SQL_GENERAL_ERROR);
            }
        }

        public bool UpdateUserLocation(Location location)
        {
            try
            {
                long userId = CheckFBUser(AppRequestContext.FBUser);

                bool isLocationAdded = CheckLocation(userId);

                string query = isLocationAdded ?
                    VizQueries.UPDATE_LOCATION : VizQueries.ADD_LOCATION;
                Dictionary<string, object> queryparams = new Dictionary<string, object>();
                queryparams.Add("@u_id", userId);
                queryparams.Add("@lat", location.Latitude);
                queryparams.Add("@lng", location.Longitude);

                ExecuteNonQuery(query, ConvertToDBParameters(queryparams));
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                throw new AppException(ErrorCode.SQL_GENERAL_ERROR);
            }

            return true;
        }

        public bool CheckLocation(long userId)
        {
            IDataReader reader = null;
            try
            {
                string query = string.Format(VizQueries.CHECK_USER_LOCATION, userId);

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

        public FBUser Register(FBUser user)
        {
            try
            {
                long userId = CheckFBUser(user);

                if (userId > 0)
                {
                    throw new AppException(ErrorCode.USER_ALREADY_EXISTS);
                }

                Dictionary<string, object> queryparams = new Dictionary<string, object>();
                queryparams.Add("@first_name", user.FirstName);
                queryparams.Add("@last_name", user.LastName);
                queryparams.Add("@email", user.Email);
                queryparams.Add("@password", user.Password);

                userId = ExecuteNonQueryWithOutputParam(VizQueries.REGISTER_USER, ConvertToDBParameters(queryparams));

                SaveDefaultSettings(userId);
                return GetUser(userId);
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                throw new AppException(ErrorCode.SQL_GENERAL_ERROR);
            }
        }

        private FBUser GetUser(long userId)
        {
            IDataReader reader = null;
            try
            {
                string query = string.Format(VizQueries.GET_USER, userId);

                reader = ExecuteReader(query);

                FBUser user = new FBUser();
                using (reader)
                {
                    while (reader.Read())
                    {
                        user.FirstName = Convert.ToString(reader["FIRST_NAME"]);
                        user.LastName = Convert.ToString(reader["LAST_NAME"]);
                        user.Email = Convert.ToString(reader["EMAIL"]);
                    }
                }
                return user;
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                throw new AppException(ErrorCode.SQL_GENERAL_ERROR);
            }
        }

        class VizQueries
        {
            public const string UPDATE_USER_SETTINGS = "UPDATE USER_SETTINGS " +
                                            "SET VALUE=@value " +
                                            "WHERE U_ID=@u_id AND S_ID=@s_id";
            public const string CHECK_USER_LOCATION = "SELECT * FROM LOCATION " +
                                            "WHERE U_ID={0}";
            public const string ADD_LOCATION = "INSERT INTO LOCATION " +
                                            "(U_ID,LAT,LONG) VALUES " +
                                            "(@u_id,@lat,@lng)";
            public const string UPDATE_LOCATION = "UPDATE LOCATION " +
                                            "SET LAT=@lat,LONG=@lng " +
                                            "WHERE U_ID=@u_id";
            public const string REGISTER_USER = "INSERT INTO FB_USER " +
                                            "(FIRST_NAME,LAST_NAME,EMAIL,PASSWORD) VALUES " +
                                            "(@first_name,@last_name,@email,@password); " +
                                            " SET @outputid=SCOPE_IDENTITY()";
            public const string GET_USER = "SELECT * FROM FB_USER " +
                                            "WHERE ID={0}";
        }
    }
}
