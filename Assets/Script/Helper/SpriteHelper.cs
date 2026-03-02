using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace Helper
{
    public enum SpriteType
    {
        All,
        Common,
    }

    public class SpriteHelper : MonoBehaviour
    {
        #region 아이템 Sprite가져오기

        private static SpriteAtlas _atlasAll;
        private static SpriteAtlas _atlasCommon;
        private static Dictionary<string, Sprite> spriteAll = new();

        public static void InitSprite()
        {
            spriteAll.Clear(); // Dictionary 초기화
        }

        /// <summary>
        /// 이름으로 스프라이트 가져오기
        /// </summary>
        public static Sprite GetSpriteAtlasByName(SpriteType spriteType, string name)
        {
            switch (spriteType)
            {
                case SpriteType.All:
                    try
                    {
                        if (_atlasAll == null) _atlasAll = Resources.Load<SpriteAtlas>(Constant.PathSpriteDynamic);
                        return _atlasAll.GetSprite(name.ToLower());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }

                case SpriteType.Common:
                    if (_atlasCommon == null)
                        _atlasCommon = Resources.Load<SpriteAtlas>(Constant.PathSpriteDynamicCommon);
                    return _atlasCommon.GetSprite(name.ToLower());
                default:
                    throw new ArgumentOutOfRangeException(nameof(spriteType), spriteType, null);
            }
        }

        #endregion

    }
}
