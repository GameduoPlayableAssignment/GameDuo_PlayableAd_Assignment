using Model.Table;
using System;

namespace Helper
{
    public class BaseItemHelper
    {
        public static int GetMaxLevel(int idx)
        {
            return GetItemTypeByIdx(idx) switch
            {
                ItemType.Dummy => DummyTable.GetMaxLevel(idx),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        public static TierType GetTierByIdx(int idx)
        {
            return GetItemTypeByIdx(idx) switch
            {
                ItemType.Dummy => DummyTable.GetDataByIdx(idx).Tier,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        public static string GetNameByIdx(int idx)
        {
            return GetItemTypeByIdx(idx) switch
            {
                ItemType.Dummy => DummyTable.GetDataByIdx(idx).Name,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        public static ItemType GetItemTypeByIdx(int idx)
        {
            return idx switch
            {
                _ => int.Parse(idx.ToString()[..3]) switch
                {
                    101 => ItemType.Dummy,
                    _ => throw new ArgumentOutOfRangeException()
                }
            };
        }
    }
}