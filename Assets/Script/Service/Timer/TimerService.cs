using Newtonsoft.Json;
using Service.User;
using System.Collections.Generic;

namespace Service.Timer
{
    public class Timer
    {
        public string Key { get; set; } 
        public float MaxDuration { get; set; }
        public float RemainDuration { get; set; }
        [JsonIgnore] public int TotalTicketDuration { get; set; }
        [JsonIgnore] public float RemainDurationRatio => RemainDuration / MaxDuration;
    }
    
    public class TimerCursor
    {
        [JsonIgnore]
        
        public List<ScaledTimer> ScaledTimers { get; set; }
        public List<UnscaledTimer> UnscaledTimers { get; set; }
        
        public static TimerCursor Create()
        {
            return new TimerCursor
            {
                ScaledTimers = new List<ScaledTimer>(),
                UnscaledTimers = new List<UnscaledTimer>(),
            };
        }
    }

    public partial class TimerService : Singleton<TimerService>
    {
        public bool IsDirtyPackage { get; set; } = true;
        public bool IsDirtyServerTimer { get; set; } = true;
        
        public TimerCursor Cursor { get; set; }

        public void Init()
        {
            Cursor = UserService.Instance.GetUserDataForInitializing().TimerCursor ?? TimerCursor.Create();
            _RunScaledTimers();
            _RunUnscaledTimers();
        }

        public void Run()
        {
            
        }
    }
}