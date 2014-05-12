/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System.Reflection;

namespace TyMetrix360.Core.Container
{
    public class TypeInformation
    {
        public TypeInformation(ConstructorInfo constructorInfo, Lifetime lifetime = Lifetime.Transient)
        {
            ConstructorInfo = constructorInfo;
            Lifetime = lifetime;
        }
        public ConstructorInfo ConstructorInfo { get; private set; }
        public Lifetime Lifetime { get; private set; }
        private object _instance;
        public object GetInstance()
        {
            if(Lifetime == Lifetime.ContainerControlled)
            {
                if(_instance == null)
                {
                    _instance = ConstructorInfo.Invoke(null);
                }
                return _instance;
            }
            else
            {
                return ConstructorInfo.Invoke(null);    
            }
        }
    }
}
