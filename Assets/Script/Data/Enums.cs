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
    Dummy,
    Gold,
    AddTile,
    Hint,
    Hammer
}

public enum UpgradePhase
{
    Cats = 0,
    AttackSpeed = 1,
    Multishot = 2
}

public enum UpgradeType
{
    AddCats,
    AttackSpeedMul,
    AddMultishot
}

public struct UpgradeOption
{
    public UpgradeType type;
    public int intValue;     // +3, +5, +10 / +3,+5,+7
    public float floatValue; // 공속 배율 (0.85, 0.7, 0.55 등)

    public string Title;
    public string Desc;
}


