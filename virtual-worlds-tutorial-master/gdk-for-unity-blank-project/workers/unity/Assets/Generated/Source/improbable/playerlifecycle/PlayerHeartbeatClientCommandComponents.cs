// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Collections.Generic;
using Unity.Entities;

namespace Improbable.PlayerLifecycle
{
    public partial class PlayerHeartbeatClient
    {
        public class CommandSenders
        {
            public struct PlayerHeartbeat : IComponentData
            {
                internal uint CommandListHandle;
                public List<Improbable.PlayerLifecycle.PlayerHeartbeatClient.PlayerHeartbeat.Request> RequestsToSend
                {
                    get => Improbable.PlayerLifecycle.PlayerHeartbeatClient.ReferenceTypeProviders.PlayerHeartbeatSenderProvider.Get(CommandListHandle);
                    set => Improbable.PlayerLifecycle.PlayerHeartbeatClient.ReferenceTypeProviders.PlayerHeartbeatSenderProvider.Set(CommandListHandle, value);
                }
            }
        }

        public class CommandRequests
        {
            public struct PlayerHeartbeat : IComponentData
            {
                internal uint CommandListHandle;
                public List<Improbable.PlayerLifecycle.PlayerHeartbeatClient.PlayerHeartbeat.ReceivedRequest> Requests
                {
                    get => Improbable.PlayerLifecycle.PlayerHeartbeatClient.ReferenceTypeProviders.PlayerHeartbeatRequestsProvider.Get(CommandListHandle);
                    set => Improbable.PlayerLifecycle.PlayerHeartbeatClient.ReferenceTypeProviders.PlayerHeartbeatRequestsProvider.Set(CommandListHandle, value);
                }
            }
        }

        public class CommandResponders
        {
            public struct PlayerHeartbeat : IComponentData
            {
                internal uint CommandListHandle;
                public List<Improbable.PlayerLifecycle.PlayerHeartbeatClient.PlayerHeartbeat.Response> ResponsesToSend
                {
                    get => Improbable.PlayerLifecycle.PlayerHeartbeatClient.ReferenceTypeProviders.PlayerHeartbeatResponderProvider.Get(CommandListHandle);
                    set => Improbable.PlayerLifecycle.PlayerHeartbeatClient.ReferenceTypeProviders.PlayerHeartbeatResponderProvider.Set(CommandListHandle, value);
                }
            }
        }

        public class CommandResponses
        {
            public struct PlayerHeartbeat : IComponentData
            {
                internal uint CommandListHandle;
                public List<Improbable.PlayerLifecycle.PlayerHeartbeatClient.PlayerHeartbeat.ReceivedResponse> Responses
                {
                    get => Improbable.PlayerLifecycle.PlayerHeartbeatClient.ReferenceTypeProviders.PlayerHeartbeatResponsesProvider.Get(CommandListHandle);
                    set => Improbable.PlayerLifecycle.PlayerHeartbeatClient.ReferenceTypeProviders.PlayerHeartbeatResponsesProvider.Set(CommandListHandle, value);
                }
            }
        }
    }
}
