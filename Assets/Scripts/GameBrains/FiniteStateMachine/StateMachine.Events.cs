#region Events

using System.ComponentModel;
using GameBrains.Entities;
using GameBrains.FiniteStateMachine;

// ReSharper disable once CheckNamespace
namespace GameBrains.EventSystem // NOTE: Don't change this namespace
{
    public static partial class Events
    {
        [Description("Change State Request.")]
        public static readonly EventType ChangeStateRequest = (EventType) Count++;
    }

    public struct ChangeStateRequestEventPayload
    {
        public Entity entity;
        public State state;

        public ChangeStateRequestEventPayload(
            Entity entity,
            State state)
        {
            this.entity = entity;
            this.state = state;
        }
    }
}

#endregion Events