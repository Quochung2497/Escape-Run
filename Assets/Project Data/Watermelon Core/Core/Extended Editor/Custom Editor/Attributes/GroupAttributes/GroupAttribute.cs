using System;

namespace Watermelon
{
    public class GroupAttribute : ExtendedEditorAttribute
    {
        private string name;
        public string Name => name;

        public GroupAttribute(string name)
        {
            this.name = name;
        }
    }
}