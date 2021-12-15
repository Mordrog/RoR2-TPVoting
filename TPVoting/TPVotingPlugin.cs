using BepInEx;
using R2API.Utils;
using RoR2;

namespace Mordrog
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin(ModGuid, ModName, ModVer)]
    public class TPVotingPlugin : BaseUnityPlugin
    {
        public const string ModVer = "1.2.0";
        public const string ModName = "TPVoting";
        public const string ModGuid = "com.Mordrog.TPVoting";

        public TPLockerController TPLockerController { get; private set; }

        public TPVotingPlugin()
        {
            InitConfig();
        }

        public void Awake()
        {
            On.RoR2.Run.Awake += Run_Awake;
            On.RoR2.Run.OnDestroy += Run_OnDestroy;
        }

        private void Run_Awake(On.RoR2.Run.orig_Awake orig, Run self)
        {
            TPLockerController = base.gameObject.AddComponent<TPLockerController>();

            orig(self);
        }

        private void Run_OnDestroy(On.RoR2.Run.orig_OnDestroy orig, Run self)
        {
            Destroy(TPLockerController);

            orig(self);
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
