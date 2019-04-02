// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Collections.Generic;
using Unity.Entities;

namespace Improbable.Player
{
    public partial class PlayerControls
    {
        public static class ReceivedEvents
        {
            public struct MovementUpdate : IComponentData
            {
                internal uint handle;

                public List<global::Improbable.Player.MovementUpdate> Events
                {
                    get => Improbable.Player.PlayerControls.ReferenceTypeProviders.MovementUpdateProvider.Get(handle);
                    internal set => Improbable.Player.PlayerControls.ReferenceTypeProviders.MovementUpdateProvider.Set(handle, value);
                }
            }

        }

        public static class EventSender
        {
            public struct MovementUpdate : IComponentData
            {
                internal uint handle;

                public List<global::Improbable.Player.MovementUpdate> Events
                {
                    get => Improbable.Player.PlayerControls.ReferenceTypeProviders.MovementUpdateProvider.Get(handle);
                    internal set => Improbable.Player.PlayerControls.ReferenceTypeProviders.MovementUpdateProvider.Set(handle, value);
                }
            }

        }
    }
}
