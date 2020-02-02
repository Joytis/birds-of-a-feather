namespace LWG {
    public class ExpirationTimer {
        public float CurrentTime { get; private set; } = 0f;
        public float MaxTime { get; private set; } = 0f;

        public ExpirationTimer(float maxTime) => MaxTime = maxTime;

        public void AddTime(float time) => CurrentTime += time;
        public void DelayTimer(float time) => MaxTime += time;
        
        public bool IsExpired => CurrentTime > MaxTime;
        public float TimeToExpired => MaxTime - CurrentTime;
    }
}