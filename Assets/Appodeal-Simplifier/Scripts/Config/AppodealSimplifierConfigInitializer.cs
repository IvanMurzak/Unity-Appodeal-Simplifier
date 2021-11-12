using System;
using UnityEngine;

namespace AppodealSimplifier
{
    public static class AppodealSimplifierConfigInitializer
    {
        public static AppodealSimplifierConfig settings { get; private set; } = CreateSettingsConfig();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
#pragma warning disable IDE0051 // Remove unused private members
#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
#endif
        internal static void Init()
#pragma warning restore IDE0051 // Remove unused private members
        {
            RefreshSettingsFromConfig();
        }

        public static void RefreshSettingsFromConfig()
        {
            settings ??= CreateSettingsConfig();
        }

        internal static AppodealSimplifierConfig GetExistingDefaultUnitySettings() => settings;

        private static AppodealSimplifierConfig CreateSettingsConfig()
        {
            try
            {
                var config = Resources.Load<AppodealSimplifierConfig>(AppodealSimplifierConfig.PATH_FOR_RESOURCES_LOAD);
                if (!config)
                {
                    Debug.Log($"Creating new Appodeal Simplifier Settings file");
                    config = ScriptableObject.CreateInstance<AppodealSimplifierConfig>();
                }
                return config;
            } 
            catch (Exception e)
			{
                Debug.LogException(e);
			}

            return null;
        }
    }
}