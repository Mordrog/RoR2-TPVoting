using RoR2;
using UnityEngine.Networking;

namespace Mordrog
{
    class GameIsStartedWatcher : NetworkBehaviour
    {
        private Timer showInstructionMessageTimer;

        public bool GameIsStarted { get; private set; }

        public void Awake()
        {
            showInstructionMessageTimer = base.gameObject.AddComponent<Timer>();

            showInstructionMessageTimer.OnTimerEnd += ShowInstructionMessageTimer_OnTimerEnd;

            On.RoR2.Run.Start += Run_Start;
            On.RoR2.Run.OnDestroy += Run_OnDestroy; ;
        }

        private void ShowInstructionMessageTimer_OnTimerEnd()
        {
            ChatHelper.VotingInstruction();
        }

        private void Run_Start(On.RoR2.Run.orig_Start orig, Run self)
        {
            orig(self);

            GameIsStarted = true;
            showInstructionMessageTimer.StartTimer(5);
        }

        private void Run_OnDestroy(On.RoR2.Run.orig_OnDestroy orig, Run self)
        {
            orig(self);

            GameIsStarted = false;
        }
    }
}
