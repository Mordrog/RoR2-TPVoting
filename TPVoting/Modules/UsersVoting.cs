using RoR2;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Mordrog
{
    public class UsersVoting
    {
        private Dictionary<NetworkUserId, bool> usersVotes = new Dictionary<NetworkUserId, bool>();
        public IReadOnlyDictionary<NetworkUserId, bool> UsersVotes => new ReadOnlyDictionary<NetworkUserId, bool>(usersVotes);

        public bool IsVotingStarted {get; private set;}

        public delegate void VotingUpdate();
        public event VotingUpdate OnVotingUpdate;

        public delegate void VotingStarted();
        public event VotingStarted OnVotingStarted;

        public delegate void VotingEnded();
        public event VotingEnded OnVotingEnded;

        public delegate void UserVoted(NetworkUser user);
        public event UserVoted OnUserVoted;

        public delegate void UserRemoved();
        public event UserRemoved OnUserRemoved;

        public void StartVoting(IReadOnlyCollection<NetworkUser> users)
        {
            usersVotes.Clear();

            foreach (var user in users)
            {
                usersVotes[user.id] = false;
            }

            IsVotingStarted = true;
            OnVotingStarted?.Invoke();
            OnVotingUpdate?.Invoke();
        }

        public void EndVoting()
        {
            if (!IsVotingStarted)
            {
                UnityEngine.Debug.LogWarning("UsersVoting::EndVoting: Trying to end voting while there is no voting started");
                return;
            }

            IsVotingStarted = false;
            OnVotingEnded?.Invoke();
            OnVotingUpdate?.Invoke();
        }

        public bool Vote(NetworkUser user)
        {
            if (!IsVotingStarted)
            {
                UnityEngine.Debug.LogWarning("UsersVoting::Vote: Trying to set vote while voting is not started");
                return false;
            }

            if (usersVotes.ContainsKey(user.id) && !usersVotes[user.id])
            {
                usersVotes[user.id] = true;

                OnUserVoted?.Invoke(user);
                OnVotingUpdate?.Invoke();
                return true;
            }

            UnityEngine.Debug.LogWarning($"UsersVoting::Vote: Failed to find user {user.userName} in voter list");
            return false;
        }

        public bool RemoveVoter(NetworkUser user)
        {
            if (!IsVotingStarted)
            {
                return false;
            }

            if (usersVotes.ContainsKey(user.id))
            {
                usersVotes.Remove(user.id);

                OnUserRemoved?.Invoke();
                OnVotingUpdate?.Invoke();
                return true;
            }

            return false;
        }

        public void AbandonVoting()
        {
            IsVotingStarted = false;
            usersVotes.Clear();
        }

        public bool CheckIfUserVoted(NetworkUser user)
        {
            return usersVotes.TryGetValue(user.id, out bool vote) && vote;
        }

        public bool CheckIfAllUsersVoted()
        {
            return usersVotes.All(kv => kv.Value == true);
        }

        public bool CheckIfOnlyOneUserLeft()
        {
            return usersVotes.Count == 1;
        }

        public bool CheckIfHalfOrMoreVoted()
        {
            return usersVotes.Count(kv => kv.Value == true) >= UnityEngine.Mathf.CeilToInt((float)usersVotes.Count / 2);
        }
    }
}
