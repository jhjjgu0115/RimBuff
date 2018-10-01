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
    public class Buff : IExposable
    {
        #region Fields
        public BuffDef def;
        protected string name = string.Empty;
        protected string uniqueName = string.Empty;
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
        public string UniqueName
        {
            get
            {
                return uniqueName;
            }
        }
        public string Name
        {
            get
            {
                return name;
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
            if (currentDuration >= duration)
            {
                OnDurationExpire();
            }
            else
            {
                currentDuration += interval;
            }
            if (currentInnerElapseTick >= innerElapseTick)
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

        public virtual void OnIterate()
        {

        }
        public virtual void OnDurationExpire()
        {

        }

        public virtual void ExposeData()
        {
            try
            {
                Scribe_Defs.Look<BuffDef>(ref def, "buffDef");

                Scribe_Values.Look<string>(ref name, "name");
                Scribe_Values.Look<string>(ref uniqueName, "uniqueName");
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
}
