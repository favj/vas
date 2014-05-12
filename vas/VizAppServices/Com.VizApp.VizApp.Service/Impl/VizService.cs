/*
* @(#)VizService.cs
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
    public class VizService : BaseService, IVizService
    {
        #region Fields/Variables/Constants

        VizDAO dao;

        #endregion

        #region Constructor

        public VizService()
        {
            this.dao = new VizDAO();
        }

        #endregion

        #region Methods

        public Settings GetSettings()
        {
            FBUser user = AppRequestContext.FBUser;
            return dao.GetUserSettings(user);
        }

        public Settings UpdateSettings(Settings setting)
        {
            new VizValidator().ValidateUpdateSettings(setting);
            return dao.UpdateSettings(setting);
        }

        public bool UpdateUserLocation(Location location)
        {
            new VizValidator().ValidateUpdateUserLocation(location);
            return dao.UpdateUserLocation(location);
        }

        public FBUser Register(FBUser user)
        {
            new VizValidator().ValidateRegisterUser(user);
            return dao.Register(user);
        }

        #endregion
    }
}
