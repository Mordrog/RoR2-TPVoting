using RoR2;
using System.Collections.Generic;
using System.Linq;

namespace Mordrog
{
    class UsersVoting
    {
        private Dictionary<NetworkUserId, bool> usersVotes = new Dictionary<NetworkUserId, bool>();

        public delegate void VotingUpdate(IReadOnlyDictionary<NetworkUserId, bool> usersVotes);
        public event VotingUpdate OnVotingUpdate;

        public delegate void UserVoted(NetworkUser user, IReadOnlyDictionary<NetworkUserId, bool> usersVotes);
        public event UserVoted OnUserVoted;

        public bool Vote(NetworkUser user)
        {
            if (usersVotes.ContainsKey(user.id) && !usersVotes[user.id])
            {
                usersVotes[user.id] = true;

                OnUserVoted?.Invoke(user, usersVotes);
                OnVotingUpdate?.Invoke(usersVotes);
                return true;
            }

            return false;
        }

        public void SetAllUsersVote()
        {
            foreach (var userID in new List<NetworkUserId>(usersVotes.Keys))
            {
                usersVotes[userID] = true;
            }

            OnVotingUpdate?.Invoke(usersVotes);
        }

        public bool RemoveVoter(NetworkUser user)
        {
            if (usersVotes.ContainsKey(user.id))
            {
                usersVotes.Remove(user.id);

                OnVotingUpdate?.Invoke(usersVotes);
                return true;
            }

            return false;
        }

        public void ResetVoting(IReadOnlyCollection<NetworkUser> users)
        {
            usersVotes.Clear();

            foreach (var user in users)
            {
                usersVotes[user.id] = false;
            }
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
