/*
* @(#)VizFriendsService.cs
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
using Com.VizApp.VizApp.Arch;
using Com.VizApp.VizApp.Service.Api;
using Com.VizApp.VizApp.Service.Dao;
using Com.VizApp.VizApp.Service.Validator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.VizApp.VizApp.Service.Impl
{
    public class VizFriendsService : BaseService, IVizFriendsService
    {
        #region Fields/Variables/Constants

        VizFriendsDAO dao;

        #endregion

        #region Constructor

        public VizFriendsService()
        {
            this.dao = new VizFriendsDAO();
        }

        #endregion

        #region Methods

        public FBFriends GetFriends()
        {
            FBUser User = AppRequestContext.FBUser;
            return dao.GetFriends(User);
        }

        public bool UpdateSelectedFriends(FBFriends fbFriends)
        {
            new VizFriendsValidator().ValidateSelectedFriends(fbFriends);
            return dao.UpdateSelectedFriends(fbFriends);
        }

        #endregion
    }
}
