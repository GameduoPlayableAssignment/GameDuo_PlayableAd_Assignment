using I2.Loc;
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Helper
{
    public class CommonHelper
    {
        public static PlatformType GetPlatform()
        {
#if UNITY_EDITOR
            return PlatformType.UNITY_EDITOR;
#elif !UNITY_EDITOR && UNITY_STANDALONE_OSX
            return PlatformType.MAC;
#elif !UNITY_EDITOR && UNITY_STANDALONE_WIN
            return PlatformType.PC;
#elif !UNITY_EDITOR && UNITY_IOS
            return PlatformType.IOS;
#elif !UNITY_EDITOR && UNITY_ANDROID
            return PlatformType.AOS;
#else
            return PlatformType.UNKNOWN;
#endif
        }

        public LanguageType GetCurrentLanguage()
        {
            return (LanguageType)Enum.Parse(typeof(LanguageType), LocalizationManager.CurrentLanguage);
        }

        public void SetCurrentLanguage(LanguageType type)
        {
            if (type == LanguageType.Max)
            {
                return;
            }

            LocalStorage.Language = (int)type;
            LocalizationManager.CurrentLanguage = type.ToString();
        }
        
        public static string Translation(string term)
        {
            term = term.Trim();
            return LocalizationManager.GetTranslation(term) != null ? LocalizationManager.GetTranslation(term) : term;
        }
    
        public static string GetPath(string subPath)
        {
            return Path.Combine(Application.streamingAssetsPath, subPath);
        }
    
        public static void ApplicationQuit()
        {
            Application.Quit();
#if UNITY_EDITOR            
            EditorApplication.ExitPlaymode();
#endif
        }
        
        public static int[] StringToIntArray(string str)
        {
            return str.Split(",").Select(n => Convert.ToInt32(n)).ToArray();
        }
        
        public static bool IsOfflineMode()
        {
            if (LocalStorage.IsOfflineModeForTesting)
                return true;
        
            return Application.internetReachability == NetworkReachability.NotReachable;
        }
    }
}