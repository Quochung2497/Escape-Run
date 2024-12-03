using UnityEditor;

namespace Watermelon
{
    public class FilterMenuItem
    {
        public bool isActive;
        public string title;

        public SerializedPropertyFilter serializedPropertyFilter;

        public FilterMenuItem(string title, SerializedPropertyFilter serializedPropertyFilter)
        {
            isActive = true;

            this.title = title;
            this.serializedPropertyFilter = serializedPropertyFilter;
        }
    }

    public abstract class SerializedPropertyFilter
    {
        public abstract bool Validate(SerializedObject serializedObject);
    }
}
