using GameBrains.Extensions.DictionaryExtensions;
using GameBrains.Extensions.MonoBehaviours;
using UnityEngine;

namespace Testing
{
    [ExecuteAlways]
    public sealed class W23DTestSerializableDictionary : ExtendedMonoBehaviour
    {
        #region Members and Properties
        
        public bool testClearSerializableDictionary;
        public bool testSerializableDictionary;
        public bool testSerializableDictionaryPlaymode;
        public IntIntSerializableDictionary intIntSD
            = new IntIntSerializableDictionary();
        public IntFloatSerializableDictionary intFloatSD
            = new IntFloatSerializableDictionary();
        public IntStringSerializableDictionary intStringSD
            = new IntStringSerializableDictionary();

        #endregion Members and Properties

        #region Update
        public override void Update()
        {
            base.Update();
            
            if (testClearSerializableDictionary)
            {
                testClearSerializableDictionary = false;
                intIntSD.Clear();
                intFloatSD.Clear();
                intStringSD.Clear();
            }

            if (testSerializableDictionary)
            {
                testSerializableDictionary = false;

                intIntSD[0] = 10;
                intIntSD[2] = 12;

                if (intIntSD.ContainsKey(0))
                {
                    Log.Debug("intIntSD contains 0 as it should.");
                }
                else
                {
                    Log.Debug("intIntSD does not contain 0, but it should.");
                }

                intFloatSD[0] = 1.0f;
                intFloatSD[2] = 1.2f;

                if (intFloatSD.ContainsValue(1.2f))
                {
                    Log.Debug("intFloatSD contains 1.2 as it should.");
                }
                else
                {
                    Log.Debug("intFloatSD does not contain 1.2, but it should.");
                }

                intStringSD[1] = "a";
                intStringSD[3] = "c";

                if (intStringSD.ContainsValue("c"))
                {
                    Log.Debug("intStringSD contains c as it should.");
                }
                else
                {
                    Log.Debug("intStringSD does not contain c, but it should.");
                }
            }

            if (testSerializableDictionaryPlaymode)
            {
                testSerializableDictionaryPlaymode = false;

                if (!Application.IsPlaying(this))
                {
                    Log.Debug("Application must be playing for this test, but it is not.");
                }
                else
                {
                    intIntSD[1] = 11;
                    intIntSD[3] = 13;

                    if (intIntSD.ContainsKey(1))
                    {
                        Log.Debug("intIntSD contains 1 as it should.");
                    }
                    else
                    {
                        Log.Debug("intIntSD does not contain 1, but it should.");
                    }

                    intFloatSD[1] = 1.1f;
                    intFloatSD[3] = 1.3f;

                    if (intFloatSD.ContainsValue(1.3f))
                    {
                        Log.Debug("intFloatSD contains 1.3 as it should.");
                    }
                    else
                    {
                        Log.Debug("intFloatSD does not contain 1.3, but it should.");
                    }

                    intStringSD[2] = "b";
                    intStringSD[4] = "d";

                    if (intStringSD.ContainsValue("d"))
                    {
                        Log.Debug("intStringSD contains d as it should.");
                    }
                    else
                    {
                        Log.Debug("intStringSD does not contains d, but it should.");
                    }
                }
            }
        }
        
        #endregion Update
    }
}