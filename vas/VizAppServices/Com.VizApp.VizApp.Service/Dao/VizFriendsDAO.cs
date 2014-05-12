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
using Com.VizApp.Arch.Base;
using Com.VizApp.Arch.Logging;
using Com.VizApp.VizApp.Arch;
using Com.VizApp.VizApp.Service.Api;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Com.VizApp.VizApp.Service.Dao
{
    public class VizFriendsDAO : BaseDAO
    {
        public FBFriends GetFriends(FBUser User)
        {
            try
            {
                long userId = CheckFBUser(User);

                if (userId <= 0)
                {
                    throw new AppException(ErrorCode.INVALID_FB_USER);
                }

                List<long> friendsIds = GetFriendsIds(userId);

                List<FBFriend> friends = new List<FBFriend>();
                foreach (long fId in friendsIds)
                {
                    friends.Add(GetFriend(fId));
                }

                return new FBFriends() { Friends = friends };
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                throw new AppException(ErrorCode.SQL_GENERAL_ERROR);
            }
        }

        public bool UpdateSelectedFriends(FBFriends fbFriends)
        {
            try
            {
                Dictionary<int, string> settings = DefaultSettings;
                Dictionary<string, object> queryparams = null;
                foreach (FBFriend friend in fbFriends.Friends)
                {
                    queryparams = new Dictionary<string, object>();
                    queryparams.Add("@selected", friend.Selected);
                    queryparams.Add("@fb_id", friend.id);

                    ExecuteNonQuery(VizFriendsQueries.UPDATE_SELECTED_FRIEND,
                        ConvertToDBParameters(queryparams));
                }
                return true;
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                throw new AppException(ErrorCode.SQL_GENERAL_ERROR);
            }
        }

        private List<long> GetFriendsIds(long userId)
        {
            IDataReader reader = null;
            try
            {
                string query = string.Format(VizFriendsQueries.GET_USER_FRIENDS, userId);

                reader = ExecuteReader(query);

                List<long> fIds = new List<long>();
                using (reader)
                {
                    while (reader.Read())
                    {
                         fIds.Add(Convert.ToInt64(reader["id"]));
                    }
                }
                return fIds;
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                throw new AppException(ErrorCode.SQL_GENERAL_ERROR);
            }
        }

        private FBFriend GetFriend(long fId)
        {
            IDataReader reader = null;
            try
            {
                string query = string.Format(VizFriendsQueries.GET_FRIEND, fId);

                reader = ExecuteReader(query);

                FBFriend friend = new FBFriend();
                using (reader)
                {
                    while (reader.Read())
                    {
                        friend.id = Convert.ToString(reader["FB_ID"]);
                        friend.ImageURL = Convert.ToString(reader["URL_IMAGE"]);
                        friend.Name = Convert.ToString(reader["NAME"]);
                        friend.Email = Convert.ToString(reader["EMAIL"]);
                        friend.PhoneNumber = Convert.ToString(reader["PHONE"]);
                        friend.ProfileURL = Convert.ToString(reader["FB_PROFILE_URL"]);
                        friend.Lat = Convert.ToString(reader["LAT"]);
                        friend.Lng = Convert.ToString(reader["LONG"]);
                        friend.Selected = Convert.ToBoolean(reader["SELECTED"]);
                    }
                }
                return friend;
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                throw new AppException(ErrorCode.SQL_GENERAL_ERROR);
            }
        }

        class VizFriendsQueries
        {
            public const string GET_USER_FRIENDS = "SELECT * FROM USER_FRIEND " +
                                            "WHERE U_ID={0}";

            public const string GET_FRIEND = "SELECT * FROM FB_FRIEND " +
                                            "WHERE ID={0}";

            public const string UPDATE_SELECTED_FRIEND = "UPDATE FB_FRIEND SET " +
                                                        "SELECTED=@selected " +
                                                        "WHERE FB_ID=@fb_id";
        }
    }
}
