using System;
using UnityEngine;

namespace Toolbox.Inspector
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
