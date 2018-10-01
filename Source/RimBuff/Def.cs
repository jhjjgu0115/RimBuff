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
    public class BuffDef : Def
    {
        public int maxLevel = 0;
        public int duration = 0;
        public int innerElapseTick = 0;
    }
}
