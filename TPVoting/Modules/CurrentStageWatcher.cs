using RoR2;
using UnityEngine.Networking;

namespace Mordrog
{
    public class CurrentStageWatcher : NetworkBehaviour
    {
        private Timer showInstructionMessageTimer;

        public string currentStage = "";

        public delegate void CurrentStageChanged();
        public event CurrentStageChanged OnCurrentStageChanged;

        public void Awake()
        {
            showInstructionMessageTimer = base.gameObject.AddComponent<Timer>();

            showInstructionMessageTimer.OnTimerEnd += ShowInstructionMessageTimer_OnTimerEnd;

            On.RoR2.Run.OnServerSceneChanged += Run_OnServerSceneChanged;
        }

        private void ShowInstructionMessageTimer_OnTimerEnd()
        {
            ChatHelper.VotingInstruction();
        }

        private void Run_OnServerSceneChanged(On.RoR2.Run.orig_OnServerSceneChanged orig, Run self, string sceneName)
        {
            orig(self, sceneName);

            currentStage = sceneName;
            if (CheckIfCurrentStageIsPortalOnly())
                showInstructionMessageTimer.StartTimer(3);


            OnCurrentStageChanged?.Invoke();
        }

        private bool CheckIfCurrentStageIsPortalOnly()
        {
            return PluginGlobals.PortalOnlyStages.Contains(currentStage);
        }
    }
}
