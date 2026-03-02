public enum PlatformType
{
    IOS,
    AOS,
    UNITY_EDITOR,
    MAC,
    PC,
    UNKNOWN,
}

public enum SceneType
{
    S0_Splash,
    S1_Intro, 
    S1_Synobsis,
    S2_Play,
}

public enum LanguageType
{
    English,
    Korean,
    Japanese,
    Max
}

public enum State
{
    Equipped,
    Unequipped,
    DontHave,
}

public enum TierType
{
    COMMON = 1, 
    UNCOMMON = 2, 
    RARE = 3, 
    EPIC = 4, 
    LEGENDARY = 5, 
    MYTHIC = 6, 
    EXOTIC = 7, 
}

public enum BuildingState
{
    Lock,
    Nothing,
    Upgradable,
    Upgrading,
    MaxLevel
}

public enum ItemType
{
    Dummy = 1,
}