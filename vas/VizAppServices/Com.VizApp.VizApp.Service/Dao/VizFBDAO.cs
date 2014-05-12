/*
* @(#)VizFBDAO.cs
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
using Com.VizApp.Arch.Util;
using Com.VizApp.VizApp.Arch;
using Com.VizApp.VizApp.Service.Api;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Com.VizApp.VizApp.Service.Dao
{
    public class VizFBDAO : BaseDAO
    {
        public Settings SaveFBDetails(FBData fbData)
        {
            try
            {
                long userId = SaveFBUser(fbData.User);

                if (userId > 0)
                {
                    bool hasAllFriendsSaved = SaveFBFriends(fbData.Friends, userId);
                    if (hasAllFriendsSaved)
                    {
                        SaveDefaultSettings(userId);
                        return GetSettings(userId);
                    }
                    else
                    {
                        throw new AppException(ErrorCode.FBFRIENDS_SAVE_FAILED);
                    }
                }
                else
                {
                    throw new AppException(ErrorCode.FBUSER_SAVE_FAILED);
                }
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                throw new AppException(ErrorCode.SQL_GENERAL_ERROR);
            }
        }

        private long SaveFBUser(FBUser User)
        {
            try
            {

                long userId = CheckFBUser(User);
                bool userExists = (userId > 0);

                string query = userExists ?
                    VizFBQueries.UPDATE_FB_USER_DETAILS : VizFBQueries.SAVE_FB_USER_DETAILS;
                Dictionary<string, object> queryparams = new Dictionary<string, object>();
                queryparams.Add("@fb_id", User.id);
                queryparams.Add("@first_name", User.FirstName);
                queryparams.Add("@username", User.UserName);
                queryparams.Add("@timezone", User.TimeZone);
                queryparams.Add("@verified", User.Verified);
                queryparams.Add("@locale", User.Locale);
                queryparams.Add("@link", User.Link);
                queryparams.Add("@name", User.Name);
                queryparams.Add("@last_name", User.LastName);
                queryparams.Add("@gender", User.Gender);
                int str = User.UpdatedTime.Value.Year;
                DateTime date;
                if (str <= 1753)
                {
                    date = DateTime.Now;
                }
                else
                {
                    date = User.UpdatedTime.Value;
                }
                queryparams.Add("@updated_time", date);
                queryparams.Add("@profile_url", User.ProfileUrl);

                if (!userExists)
                {
                    return ExecuteNonQueryWithOutputParam(query, ConvertToDBParameters(queryparams));
                }
                else
                {
                    ExecuteNonQuery(query, ConvertToDBParameters(queryparams));
                    return userId;
                }
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                throw new AppException(ErrorCode.SQL_GENERAL_ERROR);
            }
        }

        public bool SaveFBFriends(FBFriends fbFriends)
        {
            FBUser User = AppRequestContext.FBUser;

            long userId = CheckFBUser(User);

            return SaveFBFriends(fbFriends, userId);
        }

        private bool SaveFBFriends(FBFriends fbFriends, long userId)
        {
            try
            {
                bool isFriendAdded = false;
                string query = string.Empty;
                Dictionary<string, object> queryparams = null;
                long friendId = 0;

                foreach (FBFriend friend in fbFriends.Friends)
                {
                    friendId = CheckFriend(friend);
                    isFriendAdded = (friendId > 0);
                    query = isFriendAdded ?
                        VizFBQueries.UPDATE_FB_FRIEND_DETAILS : VizFBQueries.SAVE_FB_FRIEND_DETAILS;
                    queryparams = new Dictionary<string, object>();
                    queryparams.Add("@fb_id", friend.id);
                    queryparams.Add("@image_url", friend.ImageURL);
                    queryparams.Add("@name", friend.Name);
                    queryparams.Add("@email", friend.Email);
                    queryparams.Add("@phone", friend.PhoneNumber);
                    queryparams.Add("@profile_url", friend.ProfileURL);
                    queryparams.Add("@lat", friend.Lat);
                    queryparams.Add("@lng", friend.Lng);
                    queryparams.Add("@selected", friend.Selected);

                    if (!isFriendAdded)
                    {
                        friendId = ExecuteNonQueryWithOutputParam(query, ConvertToDBParameters(queryparams));
                    }
                    else
                    {
                        ExecuteNonQuery(query, ConvertToDBParameters(queryparams));
                    }

                    if (!isFriendAdded)
                    {
                        MapUserFriend(userId, friendId);
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                throw new AppException(ErrorCode.SQL_GENERAL_ERROR);
            }
        }

        private long CheckFriend(FBFriend Friend)
        {
            IDataReader reader = null;
            try
            {
                string query = string.Format(VizFBQueries.CHECK_FRIEND, Friend.id);

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

        private long MapUserFriend(long UserId, long FriendId)
        {
            try
            {
                Dictionary<string, object> queryparams = new Dictionary<string, object>();
                queryparams.Add("@user_id", UserId);
                queryparams.Add("@friend_id", FriendId);

                return ExecuteNonQueryWithOutputParam(VizFBQueries.MAP_USER_FRIEND,
                    ConvertToDBParameters(queryparams));
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                throw new AppException(ErrorCode.SQL_GENERAL_ERROR);
            }
        }
    }

    class VizFBQueries
    {
        public const string SAVE_FB_USER_DETAILS = "INSERT INTO FB_USER" +
                                        "(FB_ID, FIRST_NAME, USERNAME, TIME_ZONE, " +
                                        "VERIFIED, LOCALE, LINK, NAME, LAST_NAME, GENDER, " +
                                        "UPDATED_TIME, PROFILE_URL)" +
                                        "VALUES" +
                                        "(@fb_id, @first_name, @username, @timezone," +
                                        "@verified, @locale, @link, @name, @last_name," + 
                                        "@gender,@updated_time,@profile_url); " +
                                        " SET @outputid=SCOPE_IDENTITY()";

        public const string UPDATE_FB_USER_DETAILS = "UPDATE FB_USER " +
                                        "SET FIRST_NAME=@first_name, USERNAME=@username, " +
                                        "TIME_ZONE=@timezone, VERIFIED=@verified, " +
                                        "LOCALE=@locale, LINK=@link, NAME=@name, " +
                                        "LAST_NAME=@last_name, GENDER=@gender, " +
                                        "UPDATED_TIME=@updated_time, PROFILE_URL=@profile_url " +
                                        "WHERE FB_ID=@fb_id";

        public const string CHECK_FRIEND = "SELECT * FROM FB_FRIEND " +
                                        "WHERE FB_ID='{0}'";

        public const string SAVE_FB_FRIEND_DETAILS = "INSERT INTO FB_FRIEND" +
                                        "(FB_ID, URL_IMAGE, NAME, EMAIL, " +
                                        "PHONE, FB_PROFILE_URL, LAT, LONG, SELECTED)" +
                                        "VALUES" +
                                        "(@fb_id, @image_url, @name, @email," +
                                        "@phone, @profile_url, @lat, @lng, @selected); " +
                                        " SET @outputid=SCOPE_IDENTITY()";

        public const string UPDATE_FB_FRIEND_DETAILS = "UPDATE FB_FRIEND " +
                                        "SET URL_IMAGE=@image_url, NAME=@name, " +
                                        "EMAIL=@email, PHONE=@phone, " +
                                        "FB_PROFILE_URL=@profile_url, LAT=@lat, LONG=@lng, " +
                                        "SELECTED=@selected WHERE FB_ID=@fb_id";

        public const string MAP_USER_FRIEND = "INSERT INTO USER_FRIEND" +
                                        "(U_ID, F_ID)" +
                                        "VALUES" +
                                        "(@user_id, @friend_id); " +
                                        " SET @outputid=SCOPE_IDENTITY()";
    }
}
