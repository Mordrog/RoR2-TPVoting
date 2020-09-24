using RoR2;
using UnityEngine.Networking;

namespace Mordrog
{
    class TPLockerController : NetworkBehaviour
    {
        private UsersTPVotingController usersTPVotingController;

        public bool IsTPUnlocked = false;

        public void Awake()
        {
            usersTPVotingController = gameObject.AddComponent<UsersTPVotingController>();

            usersTPVotingController.OnTPVotingRestart += UsersTPVotingController_OnTPVotingRestart;
            usersTPVotingController.OnTPVotingFinish += UsersTPVotingController_OnTPVotingEnd;

            On.RoR2.TeleporterInteraction.OnInteractionBegin += TeleporterInteraction_OnInteractionBegin;
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
        }

        private void TeleporterInteraction_OnInteractionBegin(On.RoR2.TeleporterInteraction.orig_OnInteractionBegin orig, RoR2.TeleporterInteraction self, RoR2.Interactor activator)
        {
            if (IsTPUnlocked)
            {
                orig(self, activator);
            }
            else
            {
                ChatHelper.PlayersNotReady();
            }
        }
    }
}
