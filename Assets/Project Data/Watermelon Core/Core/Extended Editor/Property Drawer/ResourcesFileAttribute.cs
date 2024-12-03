using System;
using UnityEngine;

namespace Watermelon
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ResourcesFileAttribute : PropertyAttribute
    {
        private string m_Path;
        public string path
        {
            get { return m_Path; }
        }

        private Type m_Type;
        public Type type
        {
            get { return m_Type; }
        }

        public ResourcesFileAttribute(string folderPath, Type type)
        {
            m_Path = folderPath;
            m_Type = type;
        }
    }
}