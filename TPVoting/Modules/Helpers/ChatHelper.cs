namespace Mordrog
{
    public static class ChatHelper
    {
        private const string GrayColor = "7e91af";
        private const string RedColor = "ff0000";
        private const string YellowColor = "ffff00";
        private const string GreenColor = "32cd32";
        private const string SilverColor = "c0c0c0";


        public static void UserIsReady(string userName, int numberOfUsersWhoVoted, int numberOfVotingUsers)
        {
            if (!UsersHelper.IsOneUserOnly())
            {
                var message = $"<color=#{GreenColor}>{userName}</color> is ready! <color=#{GrayColor}>({numberOfUsersWhoVoted}/{numberOfVotingUsers})</color>";
                RoR2.Chat.SendBroadcastChat(new RoR2.Chat.SimpleChatMessage { baseToken = message });
            }
        }

        public static void TPUnlocked()
        {
            if (!UsersHelper.IsOneUserOnly())
            {
                var message = $"<color=#{RedColor}>Teleporter</color> is unlocked! <color=#{GreenColor}>Players are free to start it.</color>";
                RoR2.Chat.SendBroadcastChat(new RoR2.Chat.SimpleChatMessage { baseToken = message });
            }
        }

        public static void PlayersNotReady()
        {
            if (!UsersHelper.IsOneUserOnly())
            {
                var message = $"<color=#{RedColor}>Players are not ready.</color> Vote by writting <color=#{SilverColor}>\"r\"</color> in chat.";
                RoR2.Chat.SendBroadcastChat(new RoR2.Chat.SimpleChatMessage { baseToken = message });
            }
        }

        public static void VotingInstruction()
        {
            if (!UsersHelper.IsOneUserOnly())
            {
                var message = $"Vote <color=#{RedColor}>Teleporter</color> by writting <color=#{SilverColor}>\"r\"</color> in chat.";
                RoR2.Chat.SendBroadcastChat(new RoR2.Chat.SimpleChatMessage { baseToken = message });
            }
        }

        public static void TPCountdown(uint unlockTime)
        {
            if (!UsersHelper.IsOneUserOnly())
            {
                var message = $"Starting <color=#{RedColor}>Teleporter</color> unlock <color=#{YellowColor}>countdown!</color> <color=#{GrayColor}>({unlockTime} seconds left)</color>";
                RoR2.Chat.SendBroadcastChat(new RoR2.Chat.SimpleChatMessage { baseToken = message });
            }
        }
    }
}
