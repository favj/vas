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
    public class VizSecurityDAO : BaseDAO
    {
        public Settings Login(Credentials creds)
        {
            IDataReader reader = null;
            long userId = 0;

            try
            {
                string query = string.Format(VizSecurityQueries.LOGIN,
                    creds.Email, creds.Password);

                reader = ExecuteReader(query);

                using (reader)
                {
                    while (reader.Read())
                    {
                        userId = Convert.ToInt64(reader["ID"]);
                    }
                }
                return GetSettings(userId);
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                throw new AppException(ErrorCode.SQL_GENERAL_ERROR);
            }
        }

        class VizSecurityQueries
        {
            public const string LOGIN = "SELECT * FROM FB_USER " +
                                            "WHERE EMAIL='{0}' AND PASSWORD='{1}'";
        }
    }
}
