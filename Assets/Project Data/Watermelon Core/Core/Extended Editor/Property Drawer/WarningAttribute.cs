using System;

namespace Watermelon
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class WarningAttribute : HelpBoxAttribute
    {
        public WarningAttribute(string title) : base(title)
        {

        }
    }
}
