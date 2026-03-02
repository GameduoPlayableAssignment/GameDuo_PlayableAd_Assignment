namespace Message.Timer
{
    public class OnUnscaledTimerRemovedMessage
    {
        public string TimerKey { get; set; }
        public float MaxDuration { get; set; }
        public float CurrentDuration { get; set; }
        
    }
}