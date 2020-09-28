using System.Linq;

namespace Mordrog
{
    public static class UsersHelper
    {
        public static RoR2.NetworkUser GetUser(RoR2.CharacterMaster player)
        {
            return RoR2.NetworkUser.readOnlyInstancesList.FirstOrDefault(u => u.master == player);
        }

        public static bool IsOneUserOnly()
        {
            return RoR2.NetworkUser.readOnlyInstancesList.Count <= 1;
        }
    }
}
