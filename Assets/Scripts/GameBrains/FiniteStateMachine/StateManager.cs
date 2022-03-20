using System.Collections.Generic;
using UnityEngine;

namespace GameBrains.FiniteStateMachine
{
    // Used to lookup states by type.
    public static class StateManager
    {
        static readonly Dictionary<System.Type, State> RegisteredStates;

        static StateManager() { RegisteredStates = new Dictionary<System.Type, State>(); }

        public static void Register(System.Type stateType, State stateToAdd)
        {
            RegisteredStates[stateType] = stateToAdd;
        }

        public static void Deregister(System.Type stateType)
        {
            RegisteredStates.Remove(stateType);
        }

        public static State Lookup(System.Type stateType)
        {
            return RegisteredStates.ContainsKey(stateType) ? RegisteredStates[stateType] : Create(stateType);
        }

        // If a state is referenced, but not registered, create and register it.
        static State Create(System.Type stateType)
        {
            ScriptableObject.CreateInstance(stateType);
            return RegisteredStates[stateType];
        }
    }
}