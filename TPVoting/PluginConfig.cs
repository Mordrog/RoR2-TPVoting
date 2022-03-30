using BepInEx.Configuration;
using System.Collections.Generic;

namespace Mordrog
{
    class PluginConfig
    {
        public static ConfigEntry<string>
            PlayerIsReadyMessages,
            IgnoredGameModes;

        public static ConfigEntry<uint>
            MajorityVotesCountdownTime;

        public static ConfigEntry<bool>
            UserAutoVoteOnDeath;
    }
}
