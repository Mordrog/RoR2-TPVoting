using RoR2;
using UnityEngine.Networking;

namespace Mordrog
{
    public class CurrentStageWatcher : NetworkBehaviour
    {
        public string currentStage = "";

        public delegate void CurrentStageChanged();
        public event CurrentStageChanged OnCurrentStageChanged;

        public void Awake()
        {
            On.RoR2.Run.OnServerSceneChanged += Run_OnServerSceneChanged;
        }

        private void Run_OnServerSceneChanged(On.RoR2.Run.orig_OnServerSceneChanged orig, Run self, string sceneName)
        {
            orig(self, sceneName);
            currentStage = sceneName;
            OnCurrentStageChanged?.Invoke();
        }
    }
}
