#pragma warning disable 219

using System.IO;
using UnityEngine;

namespace Watermelon
{
    public static class IOUtils
    {
        /// <summary>
        /// Creating all folders in the path if they don't exist
        /// </summary>
        public static void CreatePath(string path, char separator = '/')
        {
            if (Directory.Exists(path))
                return;

            bool pathCreated = false;

            string[] pathFolders = path.Split(separator);
            for (int i = 0; i < pathFolders.Length; i++)
            {
                string tempPath = "";

                for (int j = 0; j < i; j++)
                {
                    tempPath += pathFolders[j] + "/";
                }

                if (!Directory.Exists(tempPath + pathFolders[i]))
                {
                    Directory.CreateDirectory(tempPath + pathFolders[i]);

                    pathCreated = true;
                }
            }

#if UNITY_EDITOR
            if (!Application.isPlaying && pathCreated)
                UnityEditor.AssetDatabase.Refresh();
#endif
        }
    }
}