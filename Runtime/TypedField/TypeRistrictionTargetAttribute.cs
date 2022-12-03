using System;
using UnityEngine;

namespace TeamZero.Inspector
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = true/*, AllowMultiple = true*/ )]
    public class TypeRistrictionTargetAttribute : PropertyAttribute
    {
        public string FieldName;

        public TypeRistrictionTargetAttribute(string serializeFieldName)
        {
            FieldName = serializeFieldName;
        }
    }
}
