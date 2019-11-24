
using System.Reflection;

using Harmony;

using Verse;

namespace SirRandoo.WheresMyBed
{
    public class WMB : Mod
    {
        internal static HarmonyInstance Harmony;

        public WMB(ModContentPack content) : base(content)
        {
            Log.Message("Where's My Bed :: Have you seen my bed?");

            Harmony = HarmonyInstance.Create("sirrandoo.wmb");
            Harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
