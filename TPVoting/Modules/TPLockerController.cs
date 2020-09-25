using RoR2;
using UnityEngine.Networking;

namespace Mordrog
{
    class TPLockerController : NetworkBehaviour
    {
        private UsersTPVotingController usersTPVotingController;

        public bool IsTPUnlocked = false;
        public bool WasTPInteractedOnceBeforeUnlock = false;

        public void Awake()
        {
            usersTPVotingController = gameObject.AddComponent<UsersTPVotingController>();

            usersTPVotingController.OnTPVotingRestart += UsersTPVotingController_OnTPVotingRestart;
            usersTPVotingController.OnTPVotingFinish += UsersTPVotingController_OnTPVotingEnd;

            On.RoR2.TeleporterInteraction.OnInteractionBegin += TeleporterInteraction_OnInteractionBegin;
            On.RoR2.TeleporterInteraction.GetInteractability += TeleporterInteraction_GetInteractability;
        }

        private void UsersTPVotingController_OnTPVotingRestart()
        {
            LockTP();
        }

        private void UsersTPVotingController_OnTPVotingEnd()
        {
            UnlockTP();
        }

        public void UnlockTP()
        {
            IsTPUnlocked = true;
            ChatHelper.TPUnlocked();
        }

        public void LockTP()
        {
            IsTPUnlocked = false;
            WasTPInteractedOnceBeforeUnlock = false;
        }

        private Interactability TeleporterInteraction_GetInteractability(On.RoR2.TeleporterInteraction.orig_GetInteractability orig, TeleporterInteraction self, Interactor activator)
        {
            if (!WasTPInteractedOnceBeforeUnlock || IsTPUnlocked)
            {
                return orig(self, activator);
            }
            else
            {
                return Interactability.ConditionsNotMet;
            }
        }

        private void TeleporterInteraction_OnInteractionBegin(On.RoR2.TeleporterInteraction.orig_OnInteractionBegin orig, RoR2.TeleporterInteraction self, RoR2.Interactor activator)
        {
            if (IsTPUnlocked)
            {
                orig(self, activator);
            }
            else
            {
                WasTPInteractedOnceBeforeUnlock = true;
                ChatHelper.PlayersNotReady();
            }
        }
    }
}
