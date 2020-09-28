using RoR2;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace Mordrog
{
    public class PortalInteractorWatcher : NetworkBehaviour
    {
        private List<GenericInteraction> portalsInteractors = new List<GenericInteraction>();

        public delegate void FoundPortal(GenericInteraction portalInteraction);
        public event FoundPortal OnFoundPortal;

        public void Awake()
        {
            On.RoR2.Stage.BeginAdvanceStage += Stage_BeginAdvanceStage;
            On.RoR2.Run.OnDestroy += Run_OnDestroy;
            On.RoR2.GenericInteraction.OnEnable += GenericInteraction_OnEnable;
        }

        private void Stage_BeginAdvanceStage(On.RoR2.Stage.orig_BeginAdvanceStage orig, Stage self, SceneDef destinationStage)
        {
            orig(self, destinationStage);

            portalsInteractors.Clear();
        }

        private void Run_OnDestroy(On.RoR2.Run.orig_OnDestroy orig, Run self)
        {
            orig(self);

            portalsInteractors.Clear();
        }

        private void GenericInteraction_OnEnable(On.RoR2.GenericInteraction.orig_OnEnable orig, GenericInteraction self)
        {
            orig(self);

            if (self.name.ToLower().Contains("portal"))
            {
                portalsInteractors.Add(self);
                OnFoundPortal?.Invoke(self);
            }
        }

        public void LockAllFoundPortals()
        {
            portalsInteractors.ForEach(i => i.SetInteractabilityConditionsNotMet());
        }

        public void UnlockAllFoundPortals()
        {
            portalsInteractors.ForEach(i => i.SetInteractabilityAvailable());
        }
    }
}
