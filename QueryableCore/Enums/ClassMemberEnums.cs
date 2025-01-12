using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryableCore.Enums
{
    public enum AccessModifier
    {
        Public,
        Private,
        Protected,
        Internal,
        Static
    }

    public enum MemberType
    {
        Field,
        Property,
        Method,
        Event,
        Constructor
    }
}
