using Helper;
using Manager.Game;
using Model;
using Service.User;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Model.Table;
using UniRx;

namespace Service.Dummy
{
    public class DummyCursor
    {
        public List<Slot> Slots { get; set; }
        public List<Dummy> Items { get; set; }
        public static DummyCursor Create()
        {
            return new DummyCursor
            {
                Slots = new List<Slot>
                {
                    new() { Idx = Slot.FIRST_SLOT_IDX, ItemIdx = Slot.NO_ITEM_IDX, CurrentState = Slot.SlotState.Lock },
                    new() { Idx = Slot.SECOND_SLOT_IDX, ItemIdx = Slot.NO_ITEM_IDX, CurrentState = Slot.SlotState.Lock },
                    new() { Idx = Slot.THIRD_SLOT_IDX, ItemIdx = Slot.NO_ITEM_IDX, CurrentState = Slot.SlotState.Lock },
                    new() { Idx = Slot.FOURTH_SLOT_IDX, ItemIdx = Slot.NO_ITEM_IDX, CurrentState = Slot.SlotState.Lock },
                    new() { Idx = Slot.FIFTH_SLOT_IDX, ItemIdx = Slot.NO_ITEM_IDX, CurrentState = Slot.SlotState.Lock },
                },
                Items = DummyTable.Data.Keys.Select(Dummy.Create).ToList(),
            };
        }
    }
    
    public partial class DummyService : Singleton<DummyService>
    {
        public DummyCursor Cursor { get; set; }
        public List<Slot> Slots => Cursor.Slots;
        public List<Dummy> Items => Cursor.Items;

        public void Init()
        {
            Cursor = UserService.Instance.GetUserDataForInitializing().DummyCursor ?? DummyCursor.Create();
        }
    }
}