using System;
using UnityEngine;

namespace AppodealSimplifier
{
    public static class AppodealSimplifierConfigInitializer
    {
        private static AppodealSimplifierConfig _config = null;
        public static AppodealSimplifierConfig Config => _config ??= LoadOrCreateConfig();

        private static AppodealSimplifierConfig LoadOrCreateConfig()
        {
            var config = Resources.Load<AppodealSimplifierConfig>(AppodealSimplifierConfig.PATH_FOR_RESOURCES_LOAD);
            if (!config) throw new NullReferenceException("AppodealSimplifier config was not found");
            return config;
        }
    }
}