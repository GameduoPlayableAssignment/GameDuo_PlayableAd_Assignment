using UnityEngine;

public static class LocalStorage
{
    public static bool IsGuestMode
    {
        get => PlayerPrefsExt.GetBool("IsGuestMode", false);
        set
        {
            PlayerPrefsExt.SetBool("IsGuestMode", value);
        }
    }

    public static bool IsSynobsis
    {
        get => PlayerPrefsExt.GetBool("IsSynobsis", true);
        set => PlayerPrefsExt.SetBool("IsSynobsis", value);
    }

    public static bool UseTutorial
    {
        get => PlayerPrefsExt.GetBool("UseTutorial", false);
        set => PlayerPrefsExt.SetBool("UseTutorial", value);
    }

    public static bool IsResetUserData
    {
        get => PlayerPrefsExt.GetBool("IsResetUserData", false);
        set => PlayerPrefsExt.SetBool("IsResetUserData", value);
    }

    public static string SerializedUserData
    {
        get => PlayerPrefs.GetString("SerializedUserData", string.Empty);
        set => PlayerPrefs.SetString("SerializedUserData", value);
    }

    public static bool IsAppleAuthurized
    {
        get => PlayerPrefsExt.GetBool("IsAppleAuthurized", false);
        set
        {
            PlayerPrefsExt.SetBool("IsAppleAuthurized", value);
        }
    }

    public static string Uid
    {
        get => PlayerPrefs.GetString("Uid", string.Empty);
        set => PlayerPrefs.SetString("Uid", value);
    }
    
    public static string BearerToken
    {
        get => PlayerPrefs.GetString("BearerToken", string.Empty);
        set
        {
            PlayerPrefs.SetString("BearerToken", value);
        }
    }

    public static bool IsOfflineModeForTesting
    {
        get => PlayerPrefsExt.GetBool("IsOfflineModeForTesting", false);
        set => PlayerPrefsExt.SetBool("IsOfflineModeForTesting", value);
    }

    public static bool IsShare
    {
        get => PlayerPrefsExt.GetBool("IsShare", false);
        set => PlayerPrefsExt.SetBool("IsShare", value);
    }


    #region GeneralSetting
    public static int Language
    {
        get => PlayerPrefs.GetInt("Language", (int)LanguageType.English);
        set
        {
            PlayerPrefs.SetInt("Language", value);
            PlayerPrefs.Save();
        }
    }
    #endregion


    public static bool IsSetLanguage
    {
        get => PlayerPrefsExt.GetBool("IsSetLanguage", false);
        set => PlayerPrefsExt.SetBool("IsSetLanguage", value);
    }
    
    #region Data
    public static float BgmAudio
    {
        get => PlayerPrefs.GetFloat("BgmAudio", 1);
        set => PlayerPrefs.SetFloat("BgmAudio", value);
    }

    public static float SFXAudio
    {
        get => PlayerPrefs.GetFloat("SFXAudio", 1);
        set => PlayerPrefs.SetFloat("SFXAudio", value);
    }
    
    public static bool IsVibration
    {
        get => PlayerPrefsExt.GetBool("IsVibration", false);
        set => PlayerPrefsExt.SetBool("IsVibration", value);
    }

    public static bool IsNotification
    {
        get => PlayerPrefsExt.GetBool("IsNotification", false);
        set => PlayerPrefsExt.SetBool("IsNotification", value);
    }
    
    public static bool IsPersonaliaedAds
    {
        get => PlayerPrefsExt.GetBool("IsPersonaliaedAds", false);
        set => PlayerPrefsExt.SetBool("IsPersonaliaedAds", value);
    }
    #endregion
    
    
    public static bool IsSpeedUp
    {
        get => PlayerPrefsExt.GetBool("IsSpeedUp", false);
        set => PlayerPrefsExt.SetBool("IsSpeedUp", value);
    }

    public static bool HasAgreedToTerms
    {
        get => PlayerPrefsExt.GetBool("HasAgreedToTerms", false);
        set => PlayerPrefsExt.SetBool("HasAgreedToTerms", value);
    }
}