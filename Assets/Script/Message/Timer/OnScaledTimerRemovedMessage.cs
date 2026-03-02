namespace Message.Timer
{
    public class OnScaledTimerRemovedMessage
    {
        public string TimerItemKey { get; set; }
        public float MaxDuration { get; set; }
        public float CurrentDuration { get; set; }
        
    }
}