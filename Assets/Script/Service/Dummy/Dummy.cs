using Manager.Game;
using Newtonsoft.Json;
using System.Numerics;
using Model;
using UnityEngine;

namespace Service.Dummy
{
    public class Dummy : BaseItem
    {
        public static Dummy Create(int idx)
        {
            return new Dummy
            {
                Idx = idx,
                // CurrentState = State.DontHave,
                CurrentShards = 0,
                Level = 1,
            };
        }
    }
}