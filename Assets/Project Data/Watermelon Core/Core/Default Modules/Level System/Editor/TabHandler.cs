#pragma warning disable 649

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Watermelon
{
    public class TabHandler
    {
        private string[] tabNames;
        private List<Tab> tabs;
        private int previousTabIndex;
        private int selectedTabIndex;

        private GUIStyle toolBarStyle;
        private bool useToolBarStyle;
        private bool toolBarStyleSet;
        public bool toolBarDisabled;

        public TabHandler(bool useToolBarStyle = true)
        {
            this.useToolBarStyle = useToolBarStyle;
            toolBarDisabled = false;
            tabs = new List<Tab>();
        }

        public void AddTab(Tab tab)
        {
            tabs.Add(tab);
            tabNames = new string[tabs.Count];

            for (int i = 0; i < tabNames.Length; i++)
            {
                tabNames[i] = tabs[i].name;
            }
        }

        public void SetToolBarStyle(GUIStyle style)
        {
            toolBarStyle = style;
            toolBarStyleSet = true;
        }

        public void SetTabIndex(int index)
        {
            previousTabIndex = index;
            selectedTabIndex = index;
        }

        public void DisplayTab()
        {
            EditorGUI.BeginDisabledGroup(toolBarDisabled);

            if (toolBarStyleSet && useToolBarStyle)
            {
                selectedTabIndex = GUILayout.Toolbar(previousTabIndex, tabNames, toolBarStyle);
            }
            else
            {
                selectedTabIndex = GUILayout.Toolbar(previousTabIndex, tabNames);
            }

            EditorGUI.EndDisabledGroup();

            if (selectedTabIndex != previousTabIndex)
            {
                tabs[selectedTabIndex].openTabFunction?.Invoke();
            }

            previousTabIndex = selectedTabIndex;
            tabs[selectedTabIndex].displayFunction?.Invoke();
        }

        public void SetDefaultToolbarStyle()
        {
            toolBarStyle = new GUIStyle(GUI.skin.button);
            toolBarStyle.fontSize = 28;
            toolBarStyle.fixedHeight = 30;
            toolBarStyle.margin = new RectOffset(6, 6, 8, 16);
            toolBarStyleSet = true;
        }

        public class Tab
        {
            public string name;
            public Action displayFunction;
            public Action openTabFunction;

            public Tab(string name, Action displayFunction)
            {
                this.name = name;
                this.displayFunction = displayFunction;
            }

            public Tab(string name, Action displayFunction, Action openTabFunction) : this(name, displayFunction)
            {
                this.openTabFunction = openTabFunction;
            }
        }
    }
}
