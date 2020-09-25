using RoR2;
using System.Collections.Generic;

namespace Mordrog
{
    public static class PluginGlobals
    {
        //Lol is there better way to store/check stages?!?
        public static List<string> IgnoredStages = new List<string>
        {
            "bazaar",
            "arena", //void
            "goldshores",
            "moon",
            "artifactworld",
            "mysteryspace",
            "limbo",
        };
    }
}
