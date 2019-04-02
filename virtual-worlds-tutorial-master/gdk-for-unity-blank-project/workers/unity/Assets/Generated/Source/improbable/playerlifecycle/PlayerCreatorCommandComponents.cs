// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Collections.Generic;
using Unity.Entities;

namespace Improbable.PlayerLifecycle
{
    public partial class PlayerCreator
    {
        public class CommandSenders
        {
            public struct CreatePlayer : IComponentData
            {
                internal uint CommandListHandle;
                public List<Improbable.PlayerLifecycle.PlayerCreator.CreatePlayer.Request> RequestsToSend
                {
                    get => Improbable.PlayerLifecycle.PlayerCreator.ReferenceTypeProviders.CreatePlayerSenderProvider.Get(CommandListHandle);
                    set => Improbable.PlayerLifecycle.PlayerCreator.ReferenceTypeProviders.CreatePlayerSenderProvider.Set(CommandListHandle, value);
                }
            }
        }

        public class CommandRequests
        {
            public struct CreatePlayer : IComponentData
            {
                internal uint CommandListHandle;
                public List<Improbable.PlayerLifecycle.PlayerCreator.CreatePlayer.ReceivedRequest> Requests
                {
                    get => Improbable.PlayerLifecycle.PlayerCreator.ReferenceTypeProviders.CreatePlayerRequestsProvider.Get(CommandListHandle);
                    set => Improbable.PlayerLifecycle.PlayerCreator.ReferenceTypeProviders.CreatePlayerRequestsProvider.Set(CommandListHandle, value);
                }
            }
        }

        public class CommandResponders
        {
            public struct CreatePlayer : IComponentData
            {
                internal uint CommandListHandle;
                public List<Improbable.PlayerLifecycle.PlayerCreator.CreatePlayer.Response> ResponsesToSend
                {
                    get => Improbable.PlayerLifecycle.PlayerCreator.ReferenceTypeProviders.CreatePlayerResponderProvider.Get(CommandListHandle);
                    set => Improbable.PlayerLifecycle.PlayerCreator.ReferenceTypeProviders.CreatePlayerResponderProvider.Set(CommandListHandle, value);
                }
            }
        }

        public class CommandResponses
        {
            public struct CreatePlayer : IComponentData
            {
                internal uint CommandListHandle;
                public List<Improbable.PlayerLifecycle.PlayerCreator.CreatePlayer.ReceivedResponse> Responses
                {
                    get => Improbable.PlayerLifecycle.PlayerCreator.ReferenceTypeProviders.CreatePlayerResponsesProvider.Get(CommandListHandle);
                    set => Improbable.PlayerLifecycle.PlayerCreator.ReferenceTypeProviders.CreatePlayerResponsesProvider.Set(CommandListHandle, value);
                }
            }
        }
    }
}
