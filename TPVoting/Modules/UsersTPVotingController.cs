using UnityEngine.Networking;
using RoR2;
using System.Collections.Generic;
using System.Linq;

namespace Mordrog
{
    public class UsersTPVotingController : NetworkBehaviour
    {
        private UsersVoting usersTPVoting = new UsersVoting();
        private Timer majorityTPVotingTimer;
        private GameIsStartedWatcher gameIsStartedWatcher;
        private CurrentStageWatcher currentStageWatcher;

        public delegate void TPVotingRestart();
        public event TPVotingRestart OnTPVotingRestart;

        public delegate void TPVotingEnd();
        public event TPVotingEnd OnTPVotingFinish;

        public void Awake()
        {
            majorityTPVotingTimer = base.gameObject.AddComponent<Timer>();
            gameIsStartedWatcher = base.gameObject.AddComponent<GameIsStartedWatcher>();
            currentStageWatcher = base.gameObject.AddComponent<CurrentStageWatcher>();

            usersTPVoting.OnUserVoted += UsersTPVoting_OnUserVoted;
            usersTPVoting.OnVotingUpdate += UsersTPVoting_OnVotingUpdate;

            majorityTPVotingTimer.OnTimerEnd += MajorityTPVotingTimer_OnTimerEnd;

            currentStageWatcher.OnCurrentStageChanged += CurrentStageWatcher_OnCurrentStageChanged;

            On.RoR2.CharacterMaster.OnBodyDeath += CharacterMaster_OnBodyDeath;
            On.RoR2.Run.OnUserRemoved += Run_OnUserRemoved;

            On.RoR2.Chat.SendBroadcastChat_ChatMessageBase += Chat_SendBroadcastChat_ChatMessageBase;
        }

        public bool HasUserVoted(NetworkUser user)
        {
            return user && usersTPVoting.CheckIfUserVoted(user);
        }

        public bool Vote(NetworkUser user)
        {
            return user && usersTPVoting.Vote(user);
        }

        private void UsersTPVoting_OnUserVoted(NetworkUser user, IReadOnlyDictionary<NetworkUserId, bool> usersVotes)
        {
            ChatHelper.UserIsReady(user.userName, usersVotes.Count(ks => ks.Value == true), usersVotes.Count);
        }

        private void UsersTPVoting_OnVotingUpdate(IReadOnlyDictionary<NetworkUserId, bool> usersVotes)
        {
            if (usersTPVoting.CheckIfAllUsersVoted() || usersTPVoting.CheckIfOnlyOneUserLeft())
            {
                majorityTPVotingTimer.Reset();
                OnTPVotingFinish?.Invoke();
            }
            else if (usersTPVoting.CheckIfHalfOrMoreVoted())
            {
                if (!majorityTPVotingTimer.IsRunning)
                {
                    var unlockTime = PluginConfig.MajorityVotesCountdownTime.Value;

                    ChatHelper.TPCountdown(unlockTime);
                    majorityTPVotingTimer.StartTimer(unlockTime);
                }
            }
        }

        private void MajorityTPVotingTimer_OnTimerEnd()
        {
            usersTPVoting.SetAllUsersVote();
        }

        private void CurrentStageWatcher_OnCurrentStageChanged()
        {
            majorityTPVotingTimer.Reset();
            usersTPVoting.ResetVoting(NetworkUser.readOnlyInstancesList);

            if (usersTPVoting.CheckIfOnlyOneUserLeft() || !CheckIfCurrentStageQualifyForTPVoting())
                usersTPVoting.SetAllUsersVote();
            else
                OnTPVotingRestart?.Invoke();
        }

        private void CharacterMaster_OnBodyDeath(On.RoR2.CharacterMaster.orig_OnBodyDeath orig, CharacterMaster self, CharacterBody body)
        {
            orig(self, body);

            if (self.IsDeadAndOutOfLivesServer() && CheckIfCurrentStageQualifyForTPVoting())
            {
                var user = UsersHelper.GetUser(self);

                if (user)
                {
                    usersTPVoting.Vote(user);
                }
            }
        }

        private void Run_OnUserRemoved(On.RoR2.Run.orig_OnUserRemoved orig, RoR2.Run self, RoR2.NetworkUser user)
        {
            orig(self, user);

            usersTPVoting.RemoveVoter(user);
        }

        private void Chat_SendBroadcastChat_ChatMessageBase(On.RoR2.Chat.orig_SendBroadcastChat_ChatMessageBase orig, RoR2.Chat.ChatMessageBase message)
        {
            if (!gameIsStartedWatcher.GameIsStarted)
            {
                orig(message);
                return;
            }

            if (message is Chat.UserChatMessage userChatMessage)
            {
                var user = userChatMessage.sender.GetComponent<NetworkUser>();

                if (user)
                {
                    var preparedMessage = userChatMessage.text.ToLower().Trim();

                    if (CheckIfReadyMessage(preparedMessage))
                    {
                        if (usersTPVoting.Vote(user))
                            return;
                    }
                }
            }

            orig(message);
        }

        private bool CheckIfReadyMessage(string message)
        {
            return PluginConfig.PlayerIsReadyMessages.Value.Split(',').Contains(message);
        }

        private bool CheckIfCurrentStageQualifyForTPVoting()
        {
            return !PluginGlobals.IgnoredStages.Contains(currentStageWatcher.currentStage);
        }
    }
}
