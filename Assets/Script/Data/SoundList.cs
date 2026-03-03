
public static class SoundList
{
    /// <summary>
    /// BGM 1
    /// </summary>
    public const string sound_dummy = "sound_dummy";
    
    /// <summary>
    /// UI Button
    /// </summary>
    public const string sound_common_btn_default1 = "sound_common_btn_default1"; //공통 - 기본 선택 버튼음 (확인, 취소 및 각종 선택버튼 등)
    public const string sound_common_btn_default2 = "sound_common_btn_default2"; //공통 - 상세 설정 선택 버튼음 (세팅, 작은 아이콘 등)
    public const string sound_common_btn_menu_bottom_menu = "sound_common_btn_menu_bottom_menu"; //하단 1depth 메인 메뉴 버튼음
    public const string sound_common_btn_menu_bottom_menu_sub = "sound_common_btn_menu_bottom_menu_sub"; //하단 2depth 서브 메뉴 버튼음
    public const string sound_common_btn_close = "sound_common_btn_close"; //공통 - 팝업 창 메뉴 등 클로즈 버튼
    public const string sound_common_btn_upgrade = "sound_common_btn_upgrade"; //업그레이드 버튼
    public const string sound_common_btn_upgrade_fail = "sound_common_btn_upgrade_fail"; //코인 부족으로 인한 강화 실패

    public static string GetButtonSoundName(ButtonSoundType buttonSoundType)
    {
        switch (buttonSoundType)
        {
            case ButtonSoundType.Default1: return sound_common_btn_default1;
            case ButtonSoundType.Default2: return sound_common_btn_default2;
            case ButtonSoundType.BottomDepth1: return sound_common_btn_menu_bottom_menu;
            case ButtonSoundType.BottomDepth2: return sound_common_btn_menu_bottom_menu_sub;
            case ButtonSoundType.Close: return sound_common_btn_close;
        }
    
        return string.Empty;
    }

}