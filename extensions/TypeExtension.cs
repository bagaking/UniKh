/** TypeExtension.cs
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/22 18:25:42
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

namespace UniKh.extensions {
    public static class TypeExtension {
        
        public static bool IsInherits(this System.Type tSelf, System.Type tBase) {
            if (tBase.IsAssignableFrom(tSelf))
                return true;
            if (tBase.IsInterface)
                return tSelf.GetInterfaces().Find(t => t == tBase) != null;
            if (tSelf.IsInterface) // caz tBase is not interface
                return false;
            var tTemp = tSelf;
            while (null != (tTemp = tTemp.BaseType)) {
                if (tTemp == tBase)
                    return true;
                if (tBase.IsGenericTypeDefinition 
                    && tTemp.IsGenericType 
                    && tBase == tTemp.GetGenericTypeDefinition()) {
                    return true;
                }
            }

            return false;
        }
    }
}