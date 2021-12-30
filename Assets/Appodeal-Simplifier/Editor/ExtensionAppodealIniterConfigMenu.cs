using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace AppodealSimplifier
{
    public static class ExtensionAppodealIniterConfigMenu
    {
        [InitializeOnLoadMethod]
        public static IEnumerator Init()
        {
            yield return null; // let's Unity initialize itself and project resources first
            GetOrCreateConfig();
        }

        [MenuItem("Edit/Appodeal-Simplifier Settings...", false, 250)]
        public static void OpenOrCreateConfig()
        {
            var config = GetOrCreateConfig();

            EditorUtility.FocusProjectWindow();
            EditorWindow inspectorWindow = EditorWindow.GetWindow(typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.InspectorWindow"));
            inspectorWindow.Focus();

            Selection.activeObject = config;
        }

        public static AppodealSimplifierConfig GetOrCreateConfig()
        {
            var config = Resources.Load<AppodealSimplifierConfig>(AppodealSimplifierConfig.PATH_FOR_RESOURCES_LOAD);
            if (config == null)
            {
                Debug.Log($"<color=orange><b>Creating AppodealSimplifierConfig file</b> at <i>{AppodealSimplifierConfig.PATH}</i></color>");
                config = ScriptableObject.CreateInstance<AppodealSimplifierConfig>();

                string directory = Path.GetDirectoryName(AppodealSimplifierConfig.PATH);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                AssetDatabase.CreateAsset(config, AppodealSimplifierConfig.PATH);
                AssetDatabase.SaveAssets();
            }
            return config;
        }
    }
}