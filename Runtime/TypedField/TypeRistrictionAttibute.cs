using System;
using UnityEngine;

namespace Toolbox.Inspector
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true/*, AllowMultiple = true*/ )]
    public class TypeRistrictionAttribute : PropertyAttribute
    {
        public Type[] RistrictionTypes;

        public TypeRistrictionAttribute(params Type[] ristrictionTypes)
        {
            RistrictionTypes = ristrictionTypes;
        }
    }
}
