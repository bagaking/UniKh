using System;
using UnityEngine;

namespace UniKh.utils.Inspector {
    [System.AttributeUsage(System.AttributeTargets.Method)]
    public class BtnAttribute : PropertyAttribute {
        public string Name;
    }

    [System.AttributeUsage(System.AttributeTargets.Method)]
    public class CompAttribute : PropertyAttribute
    {
    }
}