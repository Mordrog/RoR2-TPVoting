using RoR2;
using UnityEngine.Networking;

namespace Mordrog
{
    class GameIsStartedWatcher : NetworkBehaviour
    {
        private Timer showStartMessageTimer;

        public bool GameIsStarted { get; private set; }

        public void Awake()
        {
            showStartMessageTimer = base.gameObject.AddComponent<Timer>();

            showStartMessageTimer.OnTimerEnd += ShowStartMessageTimer_OnTimerEnd;

            On.RoR2.Run.Start += Run_Start;
            On.RoR2.Run.OnDestroy += Run_OnDestroy; ;
        }

        private void Run_OnDestroy(On.RoR2.Run.orig_OnDestroy orig, Run self)
        {
            orig(self);

            GameIsStarted = false;
        }

        private void ShowStartMessageTimer_OnTimerEnd()
        {
            ChatHelper.StartMessage();
        }

        private void Run_Start(On.RoR2.Run.orig_Start orig, Run self)
        {
            orig(self);

            GameIsStarted = true;
            showStartMessageTimer.StartTimer(5);
        }
    }
}
