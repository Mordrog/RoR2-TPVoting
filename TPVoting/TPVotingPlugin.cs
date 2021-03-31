using BepInEx;

namespace Mordrog
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin(ModGuid, ModName, ModVer)]
    public class TPVotingPlugin : BaseUnityPlugin
    {
        public const string ModVer = "1.2.0";
        public const string ModName = "TPVoting";
        public const string ModGuid = "com.Mordrog.TPVoting";

        public TPVotingPlugin()
        {
            InitConfig();
        }

        public void Awake()
        {
            base.gameObject.AddComponent<TPLockerController>();
        }

        private void InitConfig()
        {
            PluginConfig.PlayerIsReadyMessages = Config.Bind<string>(
                "Settings",
                "PlayerIsReadyMessages",
                "r,rdy,ready",
                "The message the player has to write in the chat to confirm they are ready. Values must be separated by comma."
            );

            PluginConfig.MajorityVotesCountdownTime = Config.Bind<uint>(
                "Settings",
                "MajorityVotesCountdownTime",
                30,
                "Countdown in seconds to unlock the teleporter when half or most of the players are ready."
            );
        }
    }
}
