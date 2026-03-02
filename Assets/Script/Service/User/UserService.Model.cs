using Model;
using Service.Game;
using Service.Timer;
using System;
using System.Collections.Generic;
using Service.Dummy;

namespace Service.User
{
    /// <summary>
    /// 로컬/서버 저장을 위한 json 변환이 필요한 유저 데이터
    /// </summary>
    [Serializable]
    public class UserData
    {
        public long DataTimestamp { get; set; }
        public uint Scroll { get; set; }
        public bool IsAutoMode { get; set; }
        public bool IsPurchase { get; set; }
        public TimerCursor TimerCursor { get; set; }
        public GameCursor GameCursor { get; set; }
        public List<Slot> DummySlots { get; set; }
        public DummyCursor DummyCursor { get; set; }
    }
}