using log4net;
using UnityEngine;

namespace GameBrains.Extensions.ScriptableObjects
{
    public abstract class ExtendedScriptableObject : ScriptableObject
    {
        #region VerbosityStates

        [System.Flags] public enum VerbosityStates { None, Debug, Log, }
        
        [Tooltip("verbosity selects whether Debug, Log, etc. states are active.")]
        public VerbosityStates verbosity;
        public bool VerbosityDebug => VerbosityHasAny(VerbosityStates.Debug);
        public bool VerbosityLog => VerbosityHasAny(VerbosityStates.Log);

        public bool VerbosityDebugOrLog =>
            VerbosityHasAny(VerbosityStates.Debug | VerbosityStates.Log);

        public bool VerbosityHasAll(VerbosityStates stateFlags)
        {
            return (verbosity & stateFlags) == stateFlags;
        }

        public bool VerbosityHasAny(VerbosityStates stateFlags)
        {
            return (verbosity & stateFlags) != 0;
        }

        #endregion VerbosityStates

        #region Log
        
        protected ILog Log
        {
            get
            {
                log ??= LogManager.GetLogger(GetType());
                return log;
            }
        }
        ILog log;
        
        #endregion Log

        //TODO-2: Add shortcuts and extensions.

        #region ScriptableObject Callback Events

        //Note: Explicitly including unused event functions may negatively impact performance.
        //Note: Unity will disable a script if it throws an exception in Awake().
        public virtual void Awake() { }

        public virtual void OnEnable()
        {
            Application.logMessageReceived += HandleLog;
        }

        public virtual void OnDisable()
        {
            Application.logMessageReceived += HandleLog;
        }
        public virtual void OnDestroy() { }
        public virtual void OnValidate() { }

        #endregion ScriptableObject Callback Events

        #region Log Handler

        void HandleLog (string message, string stackTrace, LogType type)
        {
            // TODO: Create custom editor window to display custom log
        }

        #endregion Log Handler
    }
}