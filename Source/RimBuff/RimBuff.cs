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
        public void RemoveBuff(Buff buff)
        {
            buff.OnDestroy();
            buffList.Remove(buff);
            buff.Owner = null;
        }
        public void RemoveBuff(string buffName)
        {
            try
            {
                if (buffName != null)
                {
                    RemoveBuff(buffList.Find(buff => buff.BuffName == buffName));
                }
                Log.Error(parent.ToString() + " RemoveBuff - Can't Find Buff: Cause buffName is Empty ");
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
                if(def!=null)
                {
                    RemoveBuff(buffList.Find(buff => buff.def == def));
                }
                Log.Error(parent.ToString() + " RemoveBuff - Can't Find Buff: Cause def is Empty ");
            }
            catch
            {
                Log.Error("Buff.Remove(" + def.defName + ") Error");
            }
        }
        public void RemoveBuff(ThingWithComps caster)
        {
            try
            {
                if (caster != null)
                {
                    RemoveBuff(buffList.Find(buff => buff.Caster == caster));
                }
                Log.Error(parent.ToString() + " RemoveBuff - Can't Find Buff: Cause target is Empty ");
            }
            catch
            {
                Log.Error("RemoveBuff Error");
            }
        }

        public void RemoveBuffAll(string buffName)
        {
            try
            {
                if(buffName !=null)
                {
                    List<Buff> removeList = FindBuffAll(buffName);
                    if (removeList != null)
                    {
                        for (int index = 0; index < removeList.Count; index++)
                        {
                            RemoveBuff(removeList[index]);
                        }
                    }
                }
                Log.Error(parent.ToString() + " RemoveBuffAll - Can't Find Buff: Cause buffName is Empty ");
            }
            catch
            {
                Log.Error("RemoveBuffAll Error");
            }
        }
        public void RemoveBuffAll(BuffDef def)
        {
            try
            {
                if (def != null)
                {
                    List<Buff> removeList = FindBuffAll(def);
                    if (removeList != null)
                    {
                        for (int index = 0; index < removeList.Count; index++)
                        {
                            RemoveBuff(removeList[index]);
                        }
                    }
                }
                Log.Error(parent.ToString() + " RemoveBuffAll - Can't Find Buff: Cause def is Empty ");
            }
            catch
            {
                Log.Error("RemoveBuffAll Error");
            }
        }
        public void RemoveBuffAll(ThingWithComps caster)
        {
            try
            {
                if (caster != null)
                {
                    List<Buff> removeList = FindBuffAll(caster);
                    if (removeList != null)
                    {
                        for (int index = 0; index < removeList.Count; index++)
                        {
                            RemoveBuff(removeList[index]);
                        }
                    }
                }
                Log.Error(parent.ToString() + " RemoveBuffAll - Can't Find Buff: Cause caster is Empty ");
            }
            catch
            {
                Log.Error("RemoveBuffAll Error");
            }

        }
        public void RemoveBuffAll()
        {
            try
            {

                for (int index = 0; index < buffList.Count; index++)
                {
                    RemoveBuff(buffList[index]);
                }
            }
            catch
            {
                Log.Error("RemoveBuffAll Error");
            }
        }

        //버프 확인
        public bool ContainBuff(Buff buff)
        {
            return buffList.Contains(buff);
        }
        public bool ContainDef(BuffDef def)
        {
            if(FindWithDef(def)!=null)
            {
                return true;
            }
            return false;
        }
        public bool ContainBuffName(string buffName)
        {
            if (FindWithName(buffName) != null)
            {
                return true;
            }
            return false;
        }
        public bool ContainUniqueID(string uniqueID)
        {
            if (FindWithUniqueID(uniqueID) != null)
            {
                return true;
            }
            return false;
        }
        public bool ContainCaster(ThingWithComps caster)
        {
            if (FindWithCaster(caster) != null)
            {
                return true;
            }
            return false;
        }

        //버프 검색
        public Buff FindBuff(Buff targetBuff)
        {
            try
            {
                if (targetBuff != null)
                {
                    return buffList.Find(buff => buff == targetBuff);
                }
                Log.Error(parent.ToString() + " Can't Find Buff: Cause buff is Empty ");
            }
            catch
            {
                Log.Error("FindBuff Error");
            }
            return default(Buff);
        }
        public Buff FindWithUniqueID(string id)
        {
            try
            {
                if (id != null)
                {
                    return buffList.Find(buff => buff.UniqueID == id);
                }
                Log.Error(parent.ToString() + " Can't Find Buff: Cause buffName is Empty ");
            }
            catch
            {
                Log.Error("FindWithUniqueID Error");
            }
            return default(Buff);
        }
        public Buff FindWithName(string buffName)
        {
            try
            {
                if (buffName != null)
                {
                    return buffList.Find(buff => buff.BuffName == buffName);
                }
                Log.Error(parent.ToString() + " Can't Find Buff: Cause buffName is Empty ");
            }
            catch
            {
                Log.Error("FindWithName Error");
            }
            return default(Buff);
        }
        public Buff FindWithDef(BuffDef def)
        {
            try
            {
                if (def != null)
                {
                    return buffList.Find(buff => buff.def == def);
                }
                Log.Error(parent.ToString() + " Can't Find Buff: Cause def is Empty ");
            }
            catch
            {
                Log.Error("FindWithDef Error");
            }
            return default(Buff);
        }
        public Buff FindWithCaster(ThingWithComps caster)
        {
            try
            {
                if (caster != null)
                {
                    return buffList.Find(buff => buff.Caster == caster);
                }
                Log.Error(parent.ToString() + " Can't Find Buff: Cause caster is Empty ");
            }
            catch
            {
                Log.Error("FindWithCaster Error");
            }
            return default(Buff);
        }

        public List<Buff> FindBuffAll(string buffName)
        {
            try
            {
                if (buffName != null)
                {
                    return buffList.FindAll(buff => buff.BuffName == buffName);
                }
                Log.Error(parent.ToString() + " Can't Find Buff: Cause buffName is Empty ");
            }
            catch
            {
                Log.Error("FindBuffAll Error");
            }
            return default(List<Buff>);
        }
        public List<Buff> FindBuffAll(BuffDef def)
        {
            try
            {
                if(def!=null)
                {
                    return buffList.FindAll(buff => buff.def == def);
                }
                Log.Error(parent.ToString() + " Can't Find Buff: Cause def is Empty ");
            }
            catch
            {
                Log.Error("FindBuffAll Error");
            }
            return default(List<Buff>);
        }
        public List<Buff> FindBuffAll(ThingWithComps caster)
        {
            try
            {
                if (caster != null)
                {
                    return buffList.FindAll(buff => buff.Caster == caster);
                }
                Log.Error(parent.ToString() + " Can't Find Buff: Cause target is Empty ");
            }
            catch
            {
                Log.Error("FindBuffAll Error");
            }
            return default(List<Buff>);
        }


        public override void PostExposeData()
        {
            try
            {
                base.PostExposeData();
                Scribe_Collections.Look<Buff>(ref buffList, true, "buffList", LookMode.Deep, new object[0]);
                if (Scribe.mode == LoadSaveMode.LoadingVars)
                {
                    if (buffList==null)
                    {
                        buffList = new List<Buff>();
                        Log.Message("BuffList is null. Auto Create New BuffList");
                    }
                    for(int i = 0; i < this.buffList.Count; i++)
                    {
                        if (buffList[i] != null)
                        {
                            buffList[i].Owner = this;
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
        protected ThingWithComps target;

        protected int maxLevel = 0;
        protected int duration = 0;
        protected int innerElapseTick = 0;

        protected int currentLevel = 0;
        protected int currentDuration = 0;
        protected int currentInnerElapseTick = 0;
        #endregion

        #region Constructors
        public Buff()
        {
            uniqueID = "needBuffName" + GetHashCode();
        }
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
        public int Duration
        {
            get
            {
                return duration;
            }
        }
        public int InnerElapseTick
        {
            get
            {
                return innerElapseTick;
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
        #endregion

        #region Public Methods
        public virtual void AddLevel(int level)
        {
            currentLevel += level;
            if (currentLevel >= maxLevel)
            {
                currentLevel = maxLevel;
            }
            OnRefresh();
        }
        public virtual void Tick(int interval)
        {
            if(currentDuration>=duration)
            {
                OnDurationExpire();
            }
            else
            {
                currentDuration+=interval;
            }
            if(currentInnerElapseTick>=innerElapseTick)
            {
                OnIterate();
                currentInnerElapseTick = 0;
            }
            else
            {
                currentInnerElapseTick += interval;
            }
        }
        public virtual void OnCreate()
        {

        }
        public virtual void OnRefresh()
        {

        }
        public virtual void OnDestroy()
        {
        }

        public virtual void ExposeData()
        {
            try
            {
                Scribe_Defs.Look<BuffDef>(ref def, "buffDef");

                Scribe_Values.Look<string>(ref buffName, "buffName");
                Scribe_Values.Look<string>(ref uniqueID, "uniqueID");
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