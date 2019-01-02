using UnityEngine;

namespace UniKh.utils.Inspector {
    [System.AttributeUsage(System.AttributeTargets.Method)]
    public class BtnAttribute : PropertyAttribute
    {
    }

    [System.AttributeUsage(System.AttributeTargets.Method)]
    public class CompAttribute : PropertyAttribute
    {
    }
}