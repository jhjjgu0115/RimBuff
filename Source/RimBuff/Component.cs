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
        public void RemoveWithUniqueName(string uniqueName)
        {
            try
            {
                if (uniqueName != null)
                {
                    RemoveBuff(buffList.Find(buff => buff.UniqueName == uniqueName));
                }
                Log.Error(parent.ToString() + " RemoveBuff - Can't Find Buff: Cause uniqueName is Empty ");
            }
            catch
            {
                Log.Error(parent.ToString() + " - " + uniqueName + " can't find.");
            }
        }
        public void RemoveWithUniqueID(string id)
        {
            try
            {
                if (id != null)
                {
                    RemoveBuff(buffList.Find(buff => buff.UniqueID == id));
                }
                Log.Error(parent.ToString() + " RemoveBuff - Can't Find Buff: Cause id is Empty ");
            }
            catch
            {
                Log.Error(parent.ToString() + " - " + id + " can't find.");
            }
        }
        public void RemoveWithDef(BuffDef def)
        {
            try
            {
                if (def != null)
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
        public void RemoveWithCaster(ThingWithComps caster)
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

        public void RemoveBuffAll(string uniqueName)
        {
            try
            {
                if (uniqueName != null)
                {
                    List<Buff> removeList = FindBuffAll(uniqueName);
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
            if (FindWithDef(def) != null)
            {
                return true;
            }
            return false;
        }
        public bool ContainBuffName(string uniqueName)
        {
            if (FindWithName(uniqueName) != null)
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
        public Buff FindWithName(string uniqueName)
        {
            try
            {
                if (uniqueName != null)
                {
                    return buffList.Find(buff => buff.UniqueName == uniqueName);
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

        public List<Buff> FindBuffAll(string uniqueName)
        {
            try
            {
                if (uniqueName != null)
                {
                    return buffList.FindAll(buff => buff.UniqueName == uniqueName);
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
                if (def != null)
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
                    if (buffList == null)
                    {
                        buffList = new List<Buff>();
                        Log.Message("BuffList is null. Auto Create New BuffList");
                    }
                    for (int i = 0; i < this.buffList.Count; i++)
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
}
