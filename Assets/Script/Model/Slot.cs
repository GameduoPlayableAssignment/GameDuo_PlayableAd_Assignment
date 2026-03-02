using System;

namespace Model
{
    [Serializable]
    public class Slot
    {
        public const int NO_ITEM_IDX = -1;
        
        public const int FIRST_SLOT_IDX = 0;
        public const int SECOND_SLOT_IDX = 1;
        public const int THIRD_SLOT_IDX = 2;
        public const int FOURTH_SLOT_IDX = 3;
        public const int FIFTH_SLOT_IDX = 4;
        
        public const int NOTE_SLOT_IDX = 0;
        public const int ARMOR_SLOT_IDX = 1;
        public const int BOOTS_SLOT_IDX = 2;
        
        public enum SlotState
        {
            Unlock,
            Equip,
            Lock
        }
        
        public int Idx { get; set; }
        public int ItemIdx { get; set; }
        public SlotState CurrentState { get; set; }
    }
}