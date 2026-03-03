namespace Model.Vo
{
    public class CommonDataVo
    {
        public static int StartGold { get; set; } = 2000; // 시작 골드
        public static int ColumnCount { get; set; } = 9; // 열 갯수 
        public static int MergeCondition { get; set; } = 10; // 머지 조건 (합)
        public static int SkillAddTileCount { get; set; } = 5; // 타일 추가 스킬 기본 갯수
        public static int SkillCount { get; set; } = 3; // 스킬 기본 갯수
        public static int TileHeight { get; set; } = 120; // 타일 세로 높이
        public static int ClearGoldReward { get; set; } = 400; // 클리어 골드 보상
        public static int SkillUnlock_AddTile { get; set; } = 1; // 타일추가 스킬 잠금해제 스테이지
        public static int SkillUnlock_Hint { get; set; } = 1; // 힌트 스킬 잠금해제 스테이지
        public static int SkillUnlock_Hammer { get; set; } = 2; // 해머 스킬 잠금해제 스테이지
         
    }
}
