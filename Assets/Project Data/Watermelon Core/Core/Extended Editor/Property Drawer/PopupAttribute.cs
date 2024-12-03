using System;
using UnityEngine;

namespace Watermelon
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class PopupAttribute : PropertyAttribute
    {
        private object[] m_ArrayParams;
        public object[] arrayParams
        {
            get { return m_ArrayParams; }
        }

        public PopupAttribute(params object[] arrayParams)
        {
            m_ArrayParams = arrayParams;
        }
    }
}
