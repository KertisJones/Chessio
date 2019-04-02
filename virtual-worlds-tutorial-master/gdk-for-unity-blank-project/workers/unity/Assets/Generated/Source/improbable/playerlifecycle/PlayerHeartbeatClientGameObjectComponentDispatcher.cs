// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using UnityEngine.Profiling;
using Improbable.Gdk.Core;
using Improbable.Gdk.GameObjectRepresentation;
using Improbable.Worker.CInterop;

namespace Improbable.PlayerLifecycle
{
    public partial class PlayerHeartbeatClient
    {
        internal class GameObjectComponentDispatcher : GameObjectComponentDispatcherBase
        {
            public override ComponentType[] ComponentAddedComponentTypes => new ComponentType[]
            {
                ComponentType.ReadOnly<ComponentAdded<Improbable.PlayerLifecycle.PlayerHeartbeatClient.Component>>(), ComponentType.ReadOnly<GameObjectReference>()
            };

            public override ComponentType[] ComponentRemovedComponentTypes => new ComponentType[]
            {
                ComponentType.ReadOnly<ComponentRemoved<Improbable.PlayerLifecycle.PlayerHeartbeatClient.Component>>(), ComponentType.ReadOnly<GameObjectReference>()
            };

            public override ComponentType[] AuthorityGainedComponentTypes => new ComponentType[]
            {
                ComponentType.ReadOnly<AuthorityChanges<Improbable.PlayerLifecycle.PlayerHeartbeatClient.Component>>(), ComponentType.ReadOnly<GameObjectReference>(),
                ComponentType.ReadOnly<Authoritative<Improbable.PlayerLifecycle.PlayerHeartbeatClient.Component>>()
            };

            public override ComponentType[] AuthorityLostComponentTypes => new ComponentType[]
            {
                ComponentType.ReadOnly<AuthorityChanges<Improbable.PlayerLifecycle.PlayerHeartbeatClient.Component>>(), ComponentType.ReadOnly<GameObjectReference>(),
                ComponentType.ReadOnly<NotAuthoritative<Improbable.PlayerLifecycle.PlayerHeartbeatClient.Component>>()
            };

            public override ComponentType[] AuthorityLossImminentComponentTypes => new ComponentType[]
            {
                ComponentType.ReadOnly<AuthorityChanges<Improbable.PlayerLifecycle.PlayerHeartbeatClient.Component>>(), ComponentType.ReadOnly<GameObjectReference>(),
                ComponentType.ReadOnly<AuthorityLossImminent<Improbable.PlayerLifecycle.PlayerHeartbeatClient.Component>>()
            };

            public override ComponentType[] ComponentsUpdatedComponentTypes => new ComponentType[]
            {
            };

            public override ComponentType[][] EventsReceivedComponentTypeArrays => new ComponentType[][]
            {
            };

            public override ComponentType[][] CommandRequestsComponentTypeArrays => new ComponentType[][]
            {
                new ComponentType[] { ComponentType.ReadOnly<CommandRequests.PlayerHeartbeat>(), ComponentType.ReadOnly<GameObjectReference>() },
            };

            public override ComponentType[][] CommandResponsesComponentTypeArrays => new ComponentType[][]
            {
                new ComponentType[] { ComponentType.ReadOnly<CommandResponses.PlayerHeartbeat>(), ComponentType.ReadOnly<GameObjectReference>() },
            };

            private const uint componentId = 13001;
            private static readonly InjectableId readerWriterInjectableId = new InjectableId(InjectableType.ReaderWriter, componentId);
            private static readonly InjectableId commandRequestHandlerInjectableId = new InjectableId(InjectableType.CommandRequestHandler, componentId);
            private static readonly InjectableId commandResponseHandlerInjectableId = new InjectableId(InjectableType.CommandResponseHandler, componentId);

            public override void MarkComponentsAddedForActivation(Dictionary<Unity.Entities.Entity, MonoBehaviourActivationManager> entityToManagers)
            {
                if (ComponentAddedComponentGroup.IsEmptyIgnoreFilter)
                {
                    return;
                }

                Profiler.BeginSample("PlayerHeartbeatClient");
                var entities = ComponentAddedComponentGroup.GetEntityArray();
                for (var i = 0; i < entities.Length; i++)
                {
                    var activationManager = entityToManagers[entities[i]];
                    activationManager.AddComponent(componentId);
                }

                Profiler.EndSample();
            }

            public override void MarkComponentsRemovedForDeactivation(Dictionary<Unity.Entities.Entity, MonoBehaviourActivationManager> entityToManagers)
            {
                if (ComponentRemovedComponentGroup.IsEmptyIgnoreFilter)
                {
                    return;
                }

                Profiler.BeginSample("PlayerHeartbeatClient");
                var entities = ComponentRemovedComponentGroup.GetEntityArray();
                for (var i = 0; i < entities.Length; i++)
                {
                    var activationManager = entityToManagers[entities[i]];
                    activationManager.RemoveComponent(componentId);
                }

                Profiler.EndSample();
            }

            public override void MarkAuthorityGainedForActivation(Dictionary<Unity.Entities.Entity, MonoBehaviourActivationManager> entityToManagers)
            {
                if (AuthorityGainedComponentGroup.IsEmptyIgnoreFilter)
                {
                    return;
                }

                Profiler.BeginSample("PlayerHeartbeatClient");
                var authoritiesChangedTags = AuthorityGainedComponentGroup.GetComponentDataArray<AuthorityChanges<Improbable.PlayerLifecycle.PlayerHeartbeatClient.Component>>();
                var entities = AuthorityGainedComponentGroup.GetEntityArray();
                for (var i = 0; i < entities.Length; i++)
                {
                    var activationManager = entityToManagers[entities[i]];
                    // Call once except if flip-flopped back to starting state
                    if (IsFirstAuthChange(Authority.Authoritative, authoritiesChangedTags[i]))
                    {
                        activationManager.ChangeAuthority(componentId, Authority.Authoritative);
                    }
                }

                Profiler.EndSample();
            }

            public override void MarkAuthorityLostForDeactivation(Dictionary<Unity.Entities.Entity, MonoBehaviourActivationManager> entityToManagers)
            {
                if (AuthorityLostComponentGroup.IsEmptyIgnoreFilter)
                {
                    return;
                }

                Profiler.BeginSample("PlayerHeartbeatClient");
                var authoritiesChangedTags = AuthorityLostComponentGroup.GetComponentDataArray<AuthorityChanges<Improbable.PlayerLifecycle.PlayerHeartbeatClient.Component>>();
                var entities = AuthorityLostComponentGroup.GetEntityArray();
                for (var i = 0; i < entities.Length; i++)
                {
                    var activationManager = entityToManagers[entities[i]];
                    // Call once except if flip-flopped back to starting state
                    if (IsFirstAuthChange(Authority.NotAuthoritative, authoritiesChangedTags[i]))
                    {
                        activationManager.ChangeAuthority(componentId, Authority.NotAuthoritative);
                    }
                }

                Profiler.EndSample();
            }

            public override void InvokeOnComponentUpdateCallbacks(Dictionary<Unity.Entities.Entity, InjectableStore> entityToInjectableStore)
            {
            }

            public override void InvokeOnEventCallbacks(Dictionary<Unity.Entities.Entity, InjectableStore> entityToInjectableStore)
            {
            }

            public override void InvokeOnCommandRequestCallbacks(Dictionary<Unity.Entities.Entity, InjectableStore> entityToInjectableStore)
            {
                Profiler.BeginSample("PlayerHeartbeatClient");
                if (!CommandRequestsComponentGroups[0].IsEmptyIgnoreFilter)
                {
                    var entities = CommandRequestsComponentGroups[0].GetEntityArray();
                    var commandRequestLists = CommandRequestsComponentGroups[0].GetComponentDataArray<CommandRequests.PlayerHeartbeat>();
                    for (var i = 0; i < entities.Length; i++)
                    {
                        var injectableStore = entityToInjectableStore[entities[i]];
                        if (!injectableStore.TryGetInjectablesForComponent(commandRequestHandlerInjectableId, out var commandRequestHandlers))
                        {
                            continue;
                        }

                        var commandRequestList = commandRequestLists[i];
                        foreach (Requirable.CommandRequestHandler commandRequestHandler in commandRequestHandlers)
                        {
                            foreach (var commandRequest in commandRequestList.Requests)
                            {
                                commandRequestHandler.OnPlayerHeartbeatRequestInternal(commandRequest);
                            }
                        }
                    }
                }

                Profiler.EndSample();
            }

            public override void InvokeOnCommandResponseCallbacks(Dictionary<Unity.Entities.Entity, InjectableStore> entityToInjectableStore)
            {
                Profiler.BeginSample("PlayerHeartbeatClient");

                if (!CommandResponsesComponentGroups[0].IsEmptyIgnoreFilter)
                {
                    var entities = CommandResponsesComponentGroups[0].GetEntityArray();
                    var commandResponseLists = CommandResponsesComponentGroups[0].GetComponentDataArray<CommandResponses.PlayerHeartbeat>();
                    for (var i = 0; i < entities.Length; i++)
                    {
                        var injectableStore = entityToInjectableStore[entities[i]];
                        injectableStore.TryGetInjectablesForComponent(commandResponseHandlerInjectableId,
                            out var commandResponseHandlers);

                        var commandResponseList = commandResponseLists[i];
                        foreach (var commandResponse in commandResponseList.Responses)
                        {
                            if (commandResponseHandlers != null)
                            {
                                foreach (Requirable.CommandResponseHandler commandResponseHandler in
                                    commandResponseHandlers)
                                {
                                    commandResponseHandler.OnPlayerHeartbeatResponseInternal(commandResponse);
                                }
                            }

                            var callback = commandResponse.Context as Action<PlayerHeartbeat.ReceivedResponse>;
                            callback?.Invoke(commandResponse);
                        }
                    }
                }

                Profiler.EndSample();
            }

            public override void InvokeOnAuthorityGainedCallbacks(Dictionary<Unity.Entities.Entity, InjectableStore> entityToInjectableStore)
            {
                if (AuthorityGainedComponentGroup.IsEmptyIgnoreFilter)
                {
                    return;
                }

                Profiler.BeginSample("PlayerHeartbeatClient");
                var entities = AuthorityGainedComponentGroup.GetEntityArray();
                var changeOpsLists = AuthorityGainedComponentGroup.GetComponentDataArray<AuthorityChanges<Improbable.PlayerLifecycle.PlayerHeartbeatClient.Component>>();

                // Call once on all entities unless they flip-flopped back into the state they started in
                for (var i = 0; i < entities.Length; i++)
                {
                    var injectableStore = entityToInjectableStore[entities[i]];
                    if (!injectableStore.TryGetInjectablesForComponent(readerWriterInjectableId, out var readersWriters))
                    {
                        continue;
                    }

                    if (IsFirstAuthChange(Authority.Authoritative, changeOpsLists[i]))
                    {
                        foreach (Requirable.ReaderWriterImpl readerWriter in readersWriters)
                        {
                            readerWriter.OnAuthorityChange(Authority.Authoritative);
                        }
                    }
                }

                Profiler.EndSample();
            }

            public override void InvokeOnAuthorityLostCallbacks(Dictionary<Unity.Entities.Entity, InjectableStore> entityToInjectableStore)
            {
                if (AuthorityLostComponentGroup.IsEmptyIgnoreFilter)
                {
                    return;
                }

                Profiler.BeginSample("PlayerHeartbeatClient");
                var entities = AuthorityLostComponentGroup.GetEntityArray();
                var changeOpsLists = AuthorityLostComponentGroup.GetComponentDataArray<AuthorityChanges<Improbable.PlayerLifecycle.PlayerHeartbeatClient.Component>>();

                // Call once on all entities unless they flip-flopped back into the state they started in
                for (var i = 0; i < entities.Length; i++)
                {
                    var injectableStore = entityToInjectableStore[entities[i]];
                    if (!injectableStore.TryGetInjectablesForComponent(readerWriterInjectableId, out var readersWriters))
                    {
                        continue;
                    }

                    if (IsFirstAuthChange(Authority.NotAuthoritative, changeOpsLists[i]))
                    {
                        foreach (Requirable.ReaderWriterImpl readerWriter in readersWriters)
                        {
                            readerWriter.OnAuthorityChange(Authority.NotAuthoritative);
                        }
                    }
                }

                Profiler.EndSample();
            }

            private bool IsFirstAuthChange(Authority authToMatch, AuthorityChanges<Improbable.PlayerLifecycle.PlayerHeartbeatClient.Component> changeOps)
            {
                foreach (var auth in changeOps.Changes)
                {
                    if (auth != Authority.AuthorityLossImminent) // not relevant
                    {
                        return auth == authToMatch;
                    }
                }

                return false;
            }

            public override void InvokeOnAuthorityLossImminentCallbacks(Dictionary<Unity.Entities.Entity, InjectableStore> entityToInjectableStore)
            {
                if (AuthorityLossImminentComponentGroup.IsEmptyIgnoreFilter)
                {
                    return;
                }

                Profiler.BeginSample("PlayerHeartbeatClient");
                var entities = AuthorityLossImminentComponentGroup.GetEntityArray();

                // Call once on all entities
                for (var i = 0; i < entities.Length; i++)
                {
                    var injectableStore = entityToInjectableStore[entities[i]];
                    if (!injectableStore.TryGetInjectablesForComponent(readerWriterInjectableId, out var readersWriters))
                    {
                        continue;
                    }

                    foreach (Requirable.ReaderWriterImpl readerWriter in readersWriters)
                    {
                        readerWriter.OnAuthorityChange(Authority.AuthorityLossImminent);
                    }
                }

                Profiler.EndSample();
            }
        }
    }
}
