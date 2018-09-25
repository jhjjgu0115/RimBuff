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
    public enum TargetType
    {
        Caster=1,
        Target=2
    }

    public class CompBuffManager : ThingComp
    {
        #region Fields
        private List<Buff> buffList;
        #endregion

        #region Constructors
        public CompBuffManager()
        {
            if (buffList == null)
            {
                buffList = new List<Buff>();
            }
            BuffController.CompList.Add(this);
        }
        ~CompBuffManager()
        {
            BuffController.CompList.Remove(this);
        }
        #endregion

        #region Properties
        public List<Buff> BuffList
        {
            get
            {
                if (buffList == null)
                {
                    buffList = new List<Buff>();
                }
                return buffList;
            }
        }
        #endregion

        #region Public Methods
        public void Tick()
        {
            try
            {
                if (buffList.Count > 0)
                {
                    for (int index = 0; index < buffList.Count; index++)
                    {
                        buffList[index].Tick(1);
                    }
                }
            }
            catch
            {
                if (buffList == null)
                {
                    Log.Error("BuffList is Null");
                }
            }
        }
      
        //버프 생성
        public Buff CreateBuff()
        {
            Buff buff = new Buff();
            BuffList.Add(buff);
            return buff;
        }
        //버프 추가
        public void AddBuff(Buff buff)
        {
            try
            {
                buffList.Add(buff);
                buff.OnCreate();
                buff.Owner = this;
            }
            catch
            {
                Log.Error("Buff.AddBuff(" + buff + ") Error");
            }
        }
        //버프 삭제
        public void RemoveBuff(string buffName)
        {
            try
            {
                buffList.Remove(buffList.Find(buff => buff.BuffName == buffName));
            }
            catch
            {
                Log.Error(parent.ToString() + " - " + buffName + " can't find.");
            }
        }
        public void RemoveBuff(BuffDef def)
        {
            try
            {
                buffList.Remove(buffList.Find(buff => buff.def == def));
            }
            catch
            {
                Log.Error("Buff.Remove(" + def.defName + ") Error");
            }
        }
        public void RemoveBuff(ThingWithComps target, TargetType targetType)
        {
            if (target == null)
            {
                Log.Error(parent.ToString() + " RemoveBuff - Can't Find Buff: Cause target is Empty ");
            }
            else
            {
                try
                {
                    if (targetType == TargetType.Caster)
                    {
                        buffList.Remove(buffList.Find(buff => buff.Caster == target));
                    }
                    else if (targetType == TargetType.Target)
                    {
                        buffList.Remove(buffList.Find(buff => buff.Target == target));
                    }
                }
                catch
                {
                    Log.Error("RemoveBuff With target Error");
                }
            }

        }
        public void RemoveBuffAll(string buffName)
        {
            try
            {
                buffList.RemoveAll(buff => buff.BuffName == buffName);
            }
            catch
            {
                Log.Error("Buff.RemoveBuffAll(" + buffName + ") Error");
            }
        }
        public void RemoveBuffAll(BuffDef def)
        {
            try
            {
                buffList.RemoveAll(buff => buff.def == def);
            }
            catch
            {
                Log.Error("Buff.RemoveBuffAll(" + def.defName + ") Error");
            }
        }
        public void RemoveBuffAll(ThingWithComps target, TargetType targetType)
        {
            if (target == null)
            {
                Log.Error(parent.ToString() + " RemoveBuff - Can't Find Buff: Cause target is Empty ");
            }
            else
            {
                try
                {
                    if (targetType == TargetType.Caster)
                    {
                        buffList.RemoveAll(buff => buff.Caster == target);
                    }
                    else if (targetType == TargetType.Target)
                    {
                        buffList.RemoveAll(buff => buff.Target == target);
                    }
                }
                catch
                {
                    Log.Error("RemoveBuffAll With target Error");
                }
            }

        }
        public void RemoveBuffAll()
        {
            try
            {
                buffList.Clear();
            }
            catch
            {
                Log.Error("Buff.RemoveBuffAll() Error");
            }
        }

        //버프 확인
        public bool ContainBuff(Buff buff)
        {
            return buffList.Contains(buff);
        }
        public bool ContainBuffName(string buffName)
        {
            if (buffList.Find(buff => buff.BuffName == buffName) != null)
            {
                return true;
            }
            return false;
        }
        public bool ContainUniqueID(string uniqueID)
        {
            if (buffList.Find(buff => buff.UniqueID == uniqueID) != null)
            {
                return true;
            }
            return false;
        }

        //버프 검색
        public Buff FindBuff(Buff targetBuff)
        {
            if (targetBuff == null)
            {
                Log.Error(parent.ToString() + " Can't Find Buff: Cause buff is Empty ");
                return default(Buff);
            }
            return buffList.Find(buff => buff == targetBuff);
        }
        public Buff FindBuff(string buffName)
        {
            try
            {
                if (buffName == null)
                {
                    Log.Error(parent.ToString() + " Can't Find Buff: Cause buffName is Empty ");
                }
                return buffList.Find(buff => buff.BuffName == buffName);
            }
            catch
            {
                Log.Error("FindBuff With buffName Error");
                return default(Buff);
            }
        }
        public Buff FindBuff(BuffDef def)
        {
            try
            {
                if (def == null)
                {
                    Log.Error(parent.ToString() + " Can't Find Buff: Cause def is Empty ");
                }
                if (buffList.Count > 0)
                {
                    foreach (Buff buff in buffList)
                    {
                        if (buff == null)
                        {
                            Log.Message("This buff is Null");
                        }
                    }
                }
                try
                {
                    return buffList.Find(buff => buff.def == def);
                }
                catch
                {
                    return default(Buff);
                }

            }
            catch
            {
                Log.Error("FindBuff With Def Error");
                return default(Buff);
            }
        }
        public Buff FindBuff(ThingWithComps target, TargetType targetType)
        {
            if (target == null)
            {
                Log.Error(parent.ToString() + " Can't Find Buff: Cause target is Empty ");
            }
            else
            {
                try
                {
                    if (targetType == TargetType.Caster)
                    {
                        return buffList.Find(buff => buff.Caster == target);
                    }
                    else if (targetType == TargetType.Target)
                    {
                        return buffList.Find(buff => buff.target == target);
                    }
                    return default(Buff);
                }
                catch
                {
                    Log.Error("FindBuff With target Error");
                }
            }
            return default(Buff);
        }

        public List<Buff> FindBuffAll(string buffName)
        {
            try
            {
                if (buffName == null)
                {
                    Log.Error(parent.ToString() + " Can't Find Buff: Cause buffName is Empty ");
                }
                return buffList.FindAll(buff => buff.BuffName == buffName);
            }
            catch
            {
                Log.Error("FindBuff With buffName Error");
                return default(List<Buff>);
            }
        }
        public List<Buff> FindBuffAll(BuffDef def)
        {
            try
            {
                if (def == null)
                {
                    Log.Error(parent.ToString() + " Can't Find Buff: Cause def is Empty ");
                }
                if (buffList.Count > 0)
                {
                    foreach (Buff buff in buffList)
                    {
                        if (buff == null)
                        {
                            Log.Message("This buff is Null");
                        }
                    }
                }
                try
                {
                    return buffList.FindAll(buff => buff.def == def);
                }
                catch
                {
                    return default(List<Buff>);
                }

            }
            catch
            {
                Log.Error("FindBuff With Def Error");
                return default(List<Buff>);
            }
        }
        public List<Buff> FindBuffAll(ThingWithComps target, TargetType targetType)
        {
            if (target == null)
            {
                Log.Error(parent.ToString() + " Can't Find Buff: Cause target is Empty ");
            }
            else
            {
                try
                {
                    if (targetType == TargetType.Caster)
                    {
                        return buffList.FindAll(buff => buff.Caster == target);
                    }
                    else if (targetType == TargetType.Target)
                    {
                        return buffList.FindAll(buff => buff.target == target);
                    }
                    return default(List<Buff>);
                }
                catch
                {
                    Log.Error("FindBuff With target Error");
                }
            }
            return default(List<Buff>);
        }


        public override void PostExposeData()
        {       
            try
            {
                base.PostExposeData();
                Scribe_Collections.Look<Buff>(ref buffList, true, "buffList", LookMode.Deep, new object[0]);
                if (Scribe.mode == LoadSaveMode.LoadingVars || Scribe.mode == LoadSaveMode.PostLoadInit)
                {
                    if(buffList==null)
                    {
                        buffList = new List<Buff>();
                        Log.Message("BuffList is null. Auto Create New BuffList");
                    }
                    for(int i = 0; i < this.buffList.Count; i++)
                    {
                        if (this.buffList[i] != null)
                        {
                            this.buffList[i].Owner = this;
                        }
                    }
                }
            }
            catch
            {
                Log.Error("CompBuffManager.PostExposeData() Error");
            }
        }
        #endregion

        #region Interface Methods
        #endregion

        #region Private Methods
        #endregion
    }

    public class Buff : IExposable
    {
        #region Fields
        public BuffDef def;
        protected string buffName = string.Empty;
        protected string uniqueID = string.Empty;
        protected ThingWithComps caster = null;
        protected CompBuffManager owner = null;
        public ThingWithComps target;

        protected int maxLevel = 0;
        protected int duration = 0;
        protected int innerElapseTick = 0;

        protected int currentLevel = 0;
        protected int currentDuration = 0;
        protected int currentInnerElapseTick = 0;
        #endregion

        #region Constructors
        #endregion

        #region Properties
        public string BuffName
        {
            get
            {
                return buffName;
            }
        }
        public string UniqueID
        {
            get
            {
                return uniqueID;
            }
        }
        public ThingWithComps Caster
        {
            get
            {
                return caster;
            }
            set
            {
                caster = value;
            }
        }
        public ThingWithComps Target
        {
            get
            {
                return target;
            }
            set
            {
                target = value;
            }
        }
        public CompBuffManager Owner
        {
            get
            {
                return owner;
            }
            set
            {
                owner = value;
            }
        }
        public int MaxLevel
        {
            get
            {
                return maxLevel;
            }
        }
        public int CurrentLevel
        {
            get
            {
                return currentLevel;

            }
            set
            {
                currentLevel = value;
                if (currentLevel >= maxLevel)
                {
                    currentLevel = maxLevel;
                }
            }
        }
        public int CurrentDuration
        {
            get
            {
                return currentDuration;

            }
        }

        public int InnerElapseTick
        {
            get
            {
                return innerElapseTick;

            }
        }
        public int CurrentInnerElapseTick
        {
            get
            {
                return currentInnerElapseTick;

            }
        }
        #endregion

        #region protected Methods

        protected virtual void OnIterate()
        {

        }

        protected virtual void OnDurationExpire()
        {

        }
        public virtual void OnDestroy()
        {

        }
        #endregion

        #region Public Methods
        public virtual void AddLevel(int level)
        {
            currentLevel += level;
            if (currentLevel >= maxLevel)
            {
                currentLevel = maxLevel;
            }
        }
        public virtual void Tick(int interval)
        {
            currentDuration++;
            currentInnerElapseTick++;
        }
        public virtual void OnCreate()
        {

        }
        public virtual void OnRefresh()
        {

        }

        public virtual void ExposeData()
        {
            try
            {
                Scribe_Defs.Look<BuffDef>(ref def, "buffDef");
                Scribe_Values.Look<string>(ref buffName, "buffName");
                Scribe_References.Look<ThingWithComps>(ref caster, "caster");
                Scribe_References.Look<ThingWithComps>(ref target, "target");

                Scribe_Values.Look<int>(ref maxLevel, "maxLevel");
                Scribe_Values.Look<int>(ref duration, "duration");
                Scribe_Values.Look<int>(ref innerElapseTick, "innerElapseTick");

                Scribe_Values.Look<int>(ref currentLevel, "currentLevel");
                Scribe_Values.Look<int>(ref currentDuration, "currentDuration");
                Scribe_Values.Look<int>(ref currentInnerElapseTick, "currentInnerElapseTick");
            }
            catch
            {
                Log.Error("Buff.ExposeData() Error");
            }


        }
        #endregion
    }

    public class BuffDef : Def
    {
        public int maxLevel = 0;
        public int duration = 0;
        public int innerElapseTick = 0;
    }

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