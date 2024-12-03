using System;

namespace Watermelon
{
    public interface IAttribute
    {
        Type TargetAttributeType { get; }
    }
}
