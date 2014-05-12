/*
* @(#)VizFBService.cs
*
* Copyright (c) 2014, VizitSolutions.
* All rights reserved.
*
* Use is subject to license terms. This software is protected by
* copyright law and international treaties. Unauthorized reproduction or
* distribution of this program, or any portion of it, may result in severe
* civil and criminal penalties, and will be prosecuted to the maximum extent.
*/

using Com.VizApp.Arch.Api;
using Com.VizApp.Arch.Base;
using Com.VizApp.Arch.Logging;
using Com.VizApp.Arch.Util;
using Com.VizApp.VizApp.Service.Api;
using Com.VizApp.VizApp.Service.Dao;
using Com.VizApp.VizApp.Service.Validator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.VizApp.VizApp.Service.Impl
{
    public class VizFBService : BaseService, IVizFBService
    {
        #region Fields/Variables/Constants

        VizFBDAO dao;

        #endregion

        #region Constructor

        public VizFBService()
        {
            this.dao = new VizFBDAO();
        }

        #endregion

        #region Methods

        public Settings SaveFBDetails(FBData fbData)
        {
            new VizFBValidator().ValidateSaveFBDetails(fbData);
            return dao.SaveFBDetails(fbData);
        }

        public bool SaveFBFriends(FBFriends fbFriends)
        {
            new VizFBValidator().ValidateSaveFBFriends(fbFriends);
            return dao.SaveFBFriends(fbFriends);
        }

        #endregion
    }
}
