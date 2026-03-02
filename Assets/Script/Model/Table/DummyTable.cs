using Model.Dao;
using System.Collections.Generic;

namespace Model.Table
{
    public class DummyTable
    {
        public class Dummy
        {
            public Dummy(DummyDao rawData) { _rawData = rawData; }
            
            private readonly DummyDao _rawData;
            public int Idx => _rawData.Idx;
            public TierType Tier => _rawData.TierType;
            public string Name => _rawData.Name;
            public string Description => _rawData.Description;
            // public int Level => _rawData.Level;
            // public int Duration => _rawData.Duration;
        }

        public static Dictionary<int, Dummy> Data { get; private set; }
        public static Dictionary<int, int> MaxLevelData { get; private set; }

        // Reflection을 통해서 자동으로 실행되는 부분임 (Parse라는 명칭을 바꾸민 안됨)
        public static void Parse(List<DummyDao> daoList)
        {
            Data = new Dictionary<int, Dummy>();
            MaxLevelData = new Dictionary<int, int>();

            foreach (DummyDao dao in daoList)
            {
                Data[dao.Idx] = new Dummy(dao);
                // MaxLevelData[dao.Idx] = dao.Level;
            }
        }

        public static Dummy GetDataByIdx(int idx)
        {
            return Data[idx];
        }

        public static int GetMaxLevel(int idx)
        {
            return MaxLevelData[idx];
        }
    }
}