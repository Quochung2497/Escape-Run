using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Watermelon.Outline
{
    public class PathCombiner
    {
        List<TreeNode> children = new List<TreeNode>();
        
        string currentValue;
        char defaultSeparator;

        public void AddElement(string element, char defaultSeparator = '/')
        {
            string[] values = element.Split(defaultSeparator);
            this.defaultSeparator = defaultSeparator;

            AssignNode(values, children, 0, false);
        }

        private void AssignNode(string[] values, List<TreeNode> children, int depth, bool unique)
        {
            currentValue = values[depth];

            if (unique)
            {
                TreeNode node = new TreeNode(currentValue);
                children.Add(node);

                if(depth + 1 < values.Length)
                {
                    AssignNode(values, node.children, depth + 1, true);
                }
            }
            else
            {
                for (int i = 0; i < children.Count; i++)
                {
                    if (children[i].value.Equals(currentValue))
                    {
                        if (depth + 1 < values.Length)
                        {
                            AssignNode(values, children[i].children, depth + 1, false);
                            return;
                        }
                    }
                }

                AssignNode(values, children, depth, true);
            }
        }

        public KeyValuePair<string,string>[] GetResult(char separator = '.')
        {
            List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();

            for (int i = 0; i < children.Count; i++)
            {
                CollectResult(result, separator, string.Empty, string.Empty, children[i]);
            }

            return result.ToArray();
        }

        private void CollectResult(List<KeyValuePair<string, string>> result, char separator, string key, string value, TreeNode treeNode)
        {
            if(treeNode.children.Count == 0) //output
            {
                KeyValuePair<string, string> pair = new KeyValuePair<string, string>(key + treeNode.value, value + treeNode.value);
                result.Add(pair);
            }
            else if(treeNode.children.Count < 4) //group
            {
                for (int i = 0; i < treeNode.children.Count; i++)
                {
                    CollectResult(result, separator, key + treeNode.value + defaultSeparator, value + treeNode.value + separator, treeNode.children[i]);
                }
            }
            else // do nothing
            {
                for (int i = 0; i < treeNode.children.Count; i++)
                {
                    CollectResult(result, separator, key + treeNode.value + defaultSeparator, value + treeNode.value + defaultSeparator, treeNode.children[i]);
                }
            }
        }

        private class TreeNode
        {
            public string value;
            public List<TreeNode> children;

            public TreeNode(string value)
            {
                this.value = value;
                children = new List<TreeNode>();
            }
        }
    }
}
