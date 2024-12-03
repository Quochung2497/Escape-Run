using System;

namespace Watermelon
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class ErrorAttribute : HelpBoxAttribute
    {
        public ErrorAttribute(string title) : base(title)
        {

        }
    }
}