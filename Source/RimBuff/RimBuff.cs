using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;
using Harmony;
using System.Reflection;


namespace RimBuff
{
    [StaticConstructorOnStartup]
    public static class RimBuffPatch
    {
        static RimBuffPatch()
        {
            HarmonyInstance harmonyInstance = HarmonyInstance.Create("com.RimBuffPatch.rimworld.mod");
            harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
    [HarmonyPatch(typeof(TickManager)), HarmonyPatch("DoSingleTick")]
    internal class BuffControllerPatch
    {
        static bool Prefix()
        {
            BuffController.Tick();
            return true;
        }
    }

    public static class BuffController
    {
        private static List<CompBuffManager> compList = new List<CompBuffManager>();
        public static List<CompBuffManager> CompList
        {
            get
            {
                if (compList == null)
                {
                    compList = new List<CompBuffManager>();
                }
                return compList;
            }
        }

        public static void Tick()
        {
            for(int index=0;index<compList.Count;index++)
            {
                compList[index].Tick();
            }
        }

    }
}