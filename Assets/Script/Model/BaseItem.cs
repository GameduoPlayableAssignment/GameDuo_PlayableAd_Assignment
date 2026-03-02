using Helper;
using Newtonsoft.Json;
using UnityEngine;

namespace Model
{
    public class BaseItem
    {
        public int Idx { get; set; }
        public int Level { get; set; }
        public int CurrentShards { get; set; }
        public State CurrentState { get; set; }
        [JsonIgnore] public string Name => BaseItemHelper.GetNameByIdx(Idx);
        [JsonIgnore] public TierType Tier => BaseItemHelper.GetTierByIdx(Idx);
        [JsonIgnore] public Sprite Icon => SpriteHelper.GetSpriteAtlasByName(SpriteType.All, Idx.ToString());
        [JsonIgnore] public Sprite Bg => SpriteHelper.GetSpriteAtlasByName(SpriteType.All, Idx.ToString());
        [JsonIgnore] public bool IsMaxLevel => Level >= BaseItemHelper.GetMaxLevel(Idx);
    }
}