using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using EntityStates.LunarTeleporter;
using MonoMod.RuntimeDetour;
using RoR2;
using UnityEngine.Networking;

namespace Mordrog
{
    public class TPLockerController : NetworkBehaviour
    {
        private UsersTPVotingController usersTPVotingController;

        public bool IsTPUnlocked = true;

        public delegate void orig_OnInteractionBegin(GenericInteraction self, Interactor activator);
        public Hook hook_OnInteractionBegin;

        public delegate Interactability orig_GetInteractability(GenericInteraction self, Interactor activator);
        public Hook hook_GetInteractability;

        public void Awake()
        {
            usersTPVotingController = gameObject.AddComponent<UsersTPVotingController>();

            usersTPVotingController.OnTPVotingStarted += UsersTPVotingController_OnTPVotingStarted;
            usersTPVotingController.OnTPVotingEnded += UsersTPVotingController_OnTPVotingEnded;

            On.RoR2.TeleporterInteraction.GetInteractability += TeleporterInteraction_GetInteractability;
            On.RoR2.TeleporterInteraction.OnInteractionBegin += TeleporterInteraction_OnInteractionBegin;

            hook_GetInteractability = new Hook(typeof(GenericInteraction).GetMethod("RoR2.IInteractable.GetInteractability", BindingFlags.NonPublic | BindingFlags.Instance), typeof(TPLockerController).GetMethod("GenericInteraction_GetInteractability"), this, new HookConfig());
            hook_OnInteractionBegin = new Hook(typeof(GenericInteraction).GetMethod("RoR2.IInteractable.OnInteractionBegin", BindingFlags.NonPublic | BindingFlags.Instance), typeof(TPLockerController).GetMethod("GenericInteraction_OnInteractionBegin"), this, new HookConfig());
        }

        public void OnDestroy()
        {
            usersTPVotingController.OnTPVotingStarted -= UsersTPVotingController_OnTPVotingStarted;
            usersTPVotingController.OnTPVotingEnded -= UsersTPVotingController_OnTPVotingEnded;

            On.RoR2.TeleporterInteraction.GetInteractability -= TeleporterInteraction_GetInteractability;
            On.RoR2.TeleporterInteraction.OnInteractionBegin -= TeleporterInteraction_OnInteractionBegin;

            hook_GetInteractability.Dispose();
            hook_OnInteractionBegin.Dispose();

            Destroy(usersTPVotingController);
        }

        private void UsersTPVotingController_OnTPVotingStarted()
        {
            IsTPUnlocked = false;

            StartCoroutine(Show());

            IEnumerator Show()
            {
                yield return new UnityEngine.WaitForSeconds(3);
                ChatHelper.VotingInstruction();
            }
        }

        private void UsersTPVotingController_OnTPVotingEnded()
        {
            IsTPUnlocked = true;
            ChatHelper.TPUnlocked();
        }

        private Interactability TeleporterInteraction_GetInteractability(On.RoR2.TeleporterInteraction.orig_GetInteractability orig, TeleporterInteraction self, Interactor activator)
        {
            var user = UsersHelper.GetUser(activator);

            if (!usersTPVotingController.HasUserVoted(user) || IsTPUnlocked)
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
                var user = UsersHelper.GetUser(activator);
                if (usersTPVotingController.Vote(user))
                    return;

                ChatHelper.PlayersNotReady();
            }
        }

        public Interactability GenericInteraction_GetInteractability(orig_GetInteractability orig, GenericInteraction self, Interactor activator)
        {
            if (!self.name.ToLower().Contains("portal"))
                orig(self, activator);

            var user = UsersHelper.GetUser(activator);

            if (!usersTPVotingController.HasUserVoted(user) || IsTPUnlocked)
            {
                return orig(self, activator);
            }
            else
            {
                return Interactability.ConditionsNotMet;
            }
        }

        public void GenericInteraction_OnInteractionBegin(orig_OnInteractionBegin orig, GenericInteraction self, Interactor activator)
        {
            if (!self.name.ToLower().Contains("portal"))
                orig(self, activator);

            if (IsTPUnlocked)
            {
                orig(self, activator);
            }
            else
            {
                var user = UsersHelper.GetUser(activator);
                if (usersTPVotingController.Vote(user))
                    return;

                ChatHelper.PlayersNotReady();
            }
        }
    }
}
