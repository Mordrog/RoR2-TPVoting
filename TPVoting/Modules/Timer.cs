using UnityEngine.Networking;

namespace Mordrog
{
    class Timer : NetworkBehaviour
    {
        private float timeRemaining = 0;

        public delegate void TimerEnd();
        public event TimerEnd OnTimerEnd;

        public bool IsRunning { get; private set; } = false;

        public void StartTimer(float time)
        {
            timeRemaining = time;
            IsRunning = true;
        }

        public void Reset()
        {
            IsRunning = false;
            timeRemaining = 0;
        }

        public void Update()
        {
            if (IsRunning)
            {
                if (timeRemaining > 0)
                {
                    timeRemaining -= UnityEngine.Time.deltaTime;
                }
                else
                {
                    Reset();
                    OnTimerEnd?.Invoke();
                }
            }
        }
    }
}
