// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;
using Unity.Mathematics;
using Unity.Entities;
using Unity.Collections;
using Improbable.Worker.CInterop;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.CodegenAdapters;
using Improbable.Gdk.Core.Commands;

namespace Improbable.PlayerLifecycle
{
    public partial class PlayerCreator
    {
        internal class DispatcherHandler : ComponentDispatcherHandler
        {
            public override uint ComponentId => 13000;

            private readonly EntityManager entityManager;

            private const string LoggerName = "PlayerCreator.DispatcherHandler";

            private CommandStorages.CreatePlayer createPlayerStorage;

            public DispatcherHandler(WorkerSystem worker, World world) : base(worker, world)
            {
                entityManager = world.GetOrCreateManager<EntityManager>();
                var bookkeepingSystem = world.GetOrCreateManager<CommandRequestTrackerSystem>();
                createPlayerStorage = bookkeepingSystem.GetCommandStorageForType<CommandStorages.CreatePlayer>();
            }

            public override void Dispose()
            {
                PlayerCreator.ReferenceTypeProviders.UpdatesProvider.CleanDataInWorld(World);
                PlayerCreator.ReferenceTypeProviders.CreatePlayerSenderProvider.CleanDataInWorld(World);
                PlayerCreator.ReferenceTypeProviders.CreatePlayerRequestsProvider.CleanDataInWorld(World);
                PlayerCreator.ReferenceTypeProviders.CreatePlayerResponderProvider.CleanDataInWorld(World);
                PlayerCreator.ReferenceTypeProviders.CreatePlayerResponsesProvider.CleanDataInWorld(World);
            }

            public override void OnAddComponent(AddComponentOp op)
            {
                var entity = TryGetEntityFromEntityId(new EntityId(op.EntityId));

                Profiler.BeginSample("PlayerCreator");
                var data = Improbable.PlayerLifecycle.PlayerCreator.Serialization.Deserialize(op.Data.SchemaData.Value.GetFields(), World);
                data.MarkDataClean();
                entityManager.AddComponentData(entity, data);
                entityManager.AddComponent(entity, ComponentType.Create<NotAuthoritative<Improbable.PlayerLifecycle.PlayerCreator.Component>>());

                var update = new Improbable.PlayerLifecycle.PlayerCreator.Update
                {
                };

                var updates = new List<Improbable.PlayerLifecycle.PlayerCreator.Update>
                {
                    update
                };

                var updatesComponent = new Improbable.PlayerLifecycle.PlayerCreator.ReceivedUpdates
                {
                    handle = ReferenceTypeProviders.UpdatesProvider.Allocate(World)
                };

                ReferenceTypeProviders.UpdatesProvider.Set(updatesComponent.handle, updates);
                entityManager.AddComponentData(entity, updatesComponent);

                if (entityManager.HasComponent<ComponentRemoved<Improbable.PlayerLifecycle.PlayerCreator.Component>>(entity))
                {
                    entityManager.RemoveComponent<ComponentRemoved<Improbable.PlayerLifecycle.PlayerCreator.Component>>(entity);
                }
                else if (!entityManager.HasComponent<ComponentAdded<Improbable.PlayerLifecycle.PlayerCreator.Component>>(entity))
                {
                    entityManager.AddComponent(entity, ComponentType.Create<ComponentAdded<Improbable.PlayerLifecycle.PlayerCreator.Component>>());
                }
                else
                {
                    LogDispatcher.HandleLog(LogType.Error, new LogEvent(ReceivedDuplicateComponentAdded)
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField(LoggingUtils.EntityId, op.EntityId)
                        .WithField("Component", "Improbable.PlayerLifecycle.PlayerCreator")
                    );
                }

                Profiler.EndSample();
            }

            public override void OnRemoveComponent(RemoveComponentOp op)
            {
                var entity = TryGetEntityFromEntityId(new EntityId(op.EntityId));

                Profiler.BeginSample("PlayerCreator");

                entityManager.RemoveComponent<Improbable.PlayerLifecycle.PlayerCreator.Component>(entity);

                if (entityManager.HasComponent<ComponentAdded<Improbable.PlayerLifecycle.PlayerCreator.Component>>(entity))
                {
                    entityManager.RemoveComponent<ComponentAdded<Improbable.PlayerLifecycle.PlayerCreator.Component>>(entity);
                }
                else if (!entityManager.HasComponent<ComponentRemoved<Improbable.PlayerLifecycle.PlayerCreator.Component>>(entity))
                {
                    entityManager.AddComponent(entity, ComponentType.Create<ComponentRemoved<Improbable.PlayerLifecycle.PlayerCreator.Component>>());
                }
                else
                {
                    LogDispatcher.HandleLog(LogType.Error, new LogEvent(ReceivedDuplicateComponentRemoved)
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField(LoggingUtils.EntityId, op.EntityId)
                        .WithField("Component", "Improbable.PlayerLifecycle.PlayerCreator")
                    );
                }

                Profiler.EndSample();
            }

            public override void OnComponentUpdate(ComponentUpdateOp op)
            {
                var entity = TryGetEntityFromEntityId(new EntityId(op.EntityId));

                Profiler.BeginSample("PlayerCreator");
                if (entityManager.HasComponent<NotAuthoritative<Improbable.PlayerLifecycle.PlayerCreator.Component>>(entity))
                {
                    var data = entityManager.GetComponentData<Improbable.PlayerLifecycle.PlayerCreator.Component>(entity);
                    Improbable.PlayerLifecycle.PlayerCreator.Serialization.ApplyUpdate(op.Update.SchemaData.Value, ref data);
                    data.MarkDataClean();
                    entityManager.SetComponentData(entity, data);
                }

                var update = Improbable.PlayerLifecycle.PlayerCreator.Serialization.DeserializeUpdate(op.Update.SchemaData.Value);

                List<Improbable.PlayerLifecycle.PlayerCreator.Update> updates;
                if (entityManager.HasComponent<Improbable.PlayerLifecycle.PlayerCreator.ReceivedUpdates>(entity))
                {
                    updates = entityManager.GetComponentData<Improbable.PlayerLifecycle.PlayerCreator.ReceivedUpdates>(entity).Updates;
                }
                else
                {
                    updates = Improbable.PlayerLifecycle.PlayerCreator.Update.Pool.Count > 0 ? Improbable.PlayerLifecycle.PlayerCreator.Update.Pool.Pop() : new List<Improbable.PlayerLifecycle.PlayerCreator.Update>();
                    var updatesComponent = new Improbable.PlayerLifecycle.PlayerCreator.ReceivedUpdates
                    {
                        handle = ReferenceTypeProviders.UpdatesProvider.Allocate(World)
                    };
                    ReferenceTypeProviders.UpdatesProvider.Set(updatesComponent.handle, updates);
                    entityManager.AddComponentData(entity, updatesComponent);
                }

                updates.Add(update);

                Profiler.EndSample();
            }

            public override void OnAuthorityChange(AuthorityChangeOp op)
            {
                var entityId = new EntityId(op.EntityId);
                var entity = TryGetEntityFromEntityId(entityId);

                Profiler.BeginSample("PlayerCreator");
                ApplyAuthorityChange(entity, op.Authority, entityId);
                Profiler.EndSample();
            }

            public override void OnCommandRequest(CommandRequestOp op)
            {
                var commandIndex = op.Request.SchemaData.Value.GetCommandIndex();

                Profiler.BeginSample("PlayerCreator");
                switch (commandIndex)
                {
                    case 1:
                        OnCreatePlayerRequest(op);
                        break;
                    default:
                        throw new UnknownCommandIndexException(commandIndex, "PlayerCreator");
                }

                Profiler.EndSample();
            }

            public override void OnCommandResponse(CommandResponseOp op)
            {
                var commandIndex = op.CommandIndex;

                Profiler.BeginSample("PlayerCreator");
                switch (commandIndex)
                {
                    case 1:
                        OnCreatePlayerResponse(op);
                        break;
                    default:
                        throw new UnknownCommandIndexException(commandIndex, "PlayerCreator");
                }

                Profiler.EndSample();
            }

            public override void AddCommandComponents(Unity.Entities.Entity entity)
            {
                {
                    var commandSender = new Improbable.PlayerLifecycle.PlayerCreator.CommandSenders.CreatePlayer();
                    commandSender.CommandListHandle = Improbable.PlayerLifecycle.PlayerCreator.ReferenceTypeProviders.CreatePlayerSenderProvider.Allocate(World);
                    commandSender.RequestsToSend = new List<Improbable.PlayerLifecycle.PlayerCreator.CreatePlayer.Request>();

                    entityManager.AddComponentData(entity, commandSender);

                    var commandResponder = new Improbable.PlayerLifecycle.PlayerCreator.CommandResponders.CreatePlayer();
                    commandResponder.CommandListHandle = Improbable.PlayerLifecycle.PlayerCreator.ReferenceTypeProviders.CreatePlayerResponderProvider.Allocate(World);
                    commandResponder.ResponsesToSend = new List<Improbable.PlayerLifecycle.PlayerCreator.CreatePlayer.Response>();

                    entityManager.AddComponentData(entity, commandResponder);
                }
            }

            private void ApplyAuthorityChange(Unity.Entities.Entity entity, Authority authority, global::Improbable.Gdk.Core.EntityId entityId)
            {
                switch (authority)
                {
                    case Authority.Authoritative:
                        if (!entityManager.HasComponent<NotAuthoritative<Improbable.PlayerLifecycle.PlayerCreator.Component>>(entity))
                        {
                            LogInvalidAuthorityTransition(Authority.Authoritative, Authority.NotAuthoritative, entityId);
                            return;
                        }

                        entityManager.RemoveComponent<NotAuthoritative<Improbable.PlayerLifecycle.PlayerCreator.Component>>(entity);
                        entityManager.AddComponent(entity, ComponentType.Create<Authoritative<Improbable.PlayerLifecycle.PlayerCreator.Component>>());

                        // Add event senders
                        break;
                    case Authority.AuthorityLossImminent:
                        if (!entityManager.HasComponent<Authoritative<Improbable.PlayerLifecycle.PlayerCreator.Component>>(entity))
                        {
                            LogInvalidAuthorityTransition(Authority.AuthorityLossImminent, Authority.Authoritative, entityId);
                            return;
                        }

                        entityManager.AddComponent(entity, ComponentType.Create<AuthorityLossImminent<Improbable.PlayerLifecycle.PlayerCreator.Component>>());
                        break;
                    case Authority.NotAuthoritative:
                        if (!entityManager.HasComponent<Authoritative<Improbable.PlayerLifecycle.PlayerCreator.Component>>(entity))
                        {
                            LogInvalidAuthorityTransition(Authority.NotAuthoritative, Authority.Authoritative, entityId);
                            return;
                        }

                        if (entityManager.HasComponent<AuthorityLossImminent<Improbable.PlayerLifecycle.PlayerCreator.Component>>(entity))
                        {
                            entityManager.RemoveComponent<AuthorityLossImminent<Improbable.PlayerLifecycle.PlayerCreator.Component>>(entity);
                        }

                        entityManager.RemoveComponent<Authoritative<Improbable.PlayerLifecycle.PlayerCreator.Component>>(entity);
                        entityManager.AddComponent(entity, ComponentType.Create<NotAuthoritative<Improbable.PlayerLifecycle.PlayerCreator.Component>>());

                        // Remove event senders
                        break;
                }

                List<Authority> authorityChanges;
                if (entityManager.HasComponent<AuthorityChanges<Improbable.PlayerLifecycle.PlayerCreator.Component>>(entity))
                {
                    authorityChanges = entityManager.GetComponentData<AuthorityChanges<Improbable.PlayerLifecycle.PlayerCreator.Component>>(entity).Changes;

                }
                else
                {
                    var changes = new AuthorityChanges<Improbable.PlayerLifecycle.PlayerCreator.Component>
                    {
                        Handle = AuthorityChangesProvider.Allocate(World)
                    };
                    AuthorityChangesProvider.Set(changes.Handle, new List<Authority>());
                    authorityChanges = changes.Changes;
                    entityManager.AddComponentData(entity, changes);
                }

                authorityChanges.Add(authority);
            }

            private void LogInvalidAuthorityTransition(Authority newAuthority, Authority expectedOldAuthority, global::Improbable.Gdk.Core.EntityId entityId)
            {
                LogDispatcher.HandleLog(LogType.Error, new LogEvent(InvalidAuthorityChange)
                    .WithField(LoggingUtils.LoggerName, LoggerName)
                    .WithField(LoggingUtils.EntityId, entityId.Id)
                    .WithField("New Authority", newAuthority)
                    .WithField("Expected Old Authority", expectedOldAuthority)
                    .WithField("Component", "Improbable.PlayerLifecycle.PlayerCreator")
                );
            }

            private void OnCreatePlayerRequest(CommandRequestOp op)
            {
                var entity = TryGetEntityFromEntityId(new EntityId(op.EntityId));

                var deserializedRequest = global::Improbable.PlayerLifecycle.CreatePlayerRequestType.Serialization.Deserialize(op.Request.SchemaData.Value.GetObject());

                List<Improbable.PlayerLifecycle.PlayerCreator.CreatePlayer.ReceivedRequest> requests;
                if (entityManager.HasComponent<Improbable.PlayerLifecycle.PlayerCreator.CommandRequests.CreatePlayer>(entity))
                {
                    requests = entityManager.GetComponentData<Improbable.PlayerLifecycle.PlayerCreator.CommandRequests.CreatePlayer>(entity).Requests;
                }
                else
                {
                    var data = new Improbable.PlayerLifecycle.PlayerCreator.CommandRequests.CreatePlayer
                    {
                        CommandListHandle = Improbable.PlayerLifecycle.PlayerCreator.ReferenceTypeProviders.CreatePlayerRequestsProvider.Allocate(World)
                    };
                    requests = data.Requests = new List<Improbable.PlayerLifecycle.PlayerCreator.CreatePlayer.ReceivedRequest>();
                    entityManager.AddComponentData(entity, data);
                }

                requests.Add(new Improbable.PlayerLifecycle.PlayerCreator.CreatePlayer.ReceivedRequest(op.RequestId,
                    op.CallerWorkerId,
                    op.CallerAttributeSet,
                    deserializedRequest));
            }

            private void OnCreatePlayerResponse(CommandResponseOp op)
            {
                if (!createPlayerStorage.CommandRequestsInFlight.TryGetValue(op.RequestId, out var requestBundle))
                {
                    throw new InvalidOperationException($"Could not find corresponding request for RequestId {op.RequestId} and command CreatePlayer.");
                }

                var entity = requestBundle.Entity;
                createPlayerStorage.CommandRequestsInFlight.Remove(op.RequestId);
                if (!entityManager.Exists(entity))
                {
                    LogDispatcher.HandleLog(LogType.Log, new LogEvent(EntityNotFound)
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField("Op", "CommandResponseOp - CreatePlayer")
                        .WithField("Component", "Improbable.PlayerLifecycle.PlayerCreator")
                    );
                    return;
                }

                global::Improbable.PlayerLifecycle.CreatePlayerResponseType? response = null;
                if (op.StatusCode == StatusCode.Success)
                {
                    response = global::Improbable.PlayerLifecycle.CreatePlayerResponseType.Serialization.Deserialize(op.Response.SchemaData.Value.GetObject());
                }

                List<Improbable.PlayerLifecycle.PlayerCreator.CreatePlayer.ReceivedResponse> responses;
                if (entityManager.HasComponent<Improbable.PlayerLifecycle.PlayerCreator.CommandResponses.CreatePlayer>(entity))
                {
                    responses = entityManager.GetComponentData<Improbable.PlayerLifecycle.PlayerCreator.CommandResponses.CreatePlayer>(entity).Responses;
                }
                else
                {
                    var data = new Improbable.PlayerLifecycle.PlayerCreator.CommandResponses.CreatePlayer
                    {
                        CommandListHandle = Improbable.PlayerLifecycle.PlayerCreator.ReferenceTypeProviders.CreatePlayerResponsesProvider.Allocate(World)
                    };
                    responses = data.Responses = new List<Improbable.PlayerLifecycle.PlayerCreator.CreatePlayer.ReceivedResponse>();
                    entityManager.AddComponentData(entity, data);
                }

                responses.Add(new Improbable.PlayerLifecycle.PlayerCreator.CreatePlayer.ReceivedResponse(new EntityId(op.EntityId),
                    op.Message,
                    op.StatusCode,
                    response,
                    requestBundle.Request,
                    requestBundle.Context,
                    requestBundle.RequestId));
            }
        }

        internal class ComponentReplicator : ComponentReplicationHandler
        {
            public override uint ComponentId => 13000;

            public override EntityArchetypeQuery ComponentUpdateQuery => new EntityArchetypeQuery
            {
                All = new[]
                {
                    ComponentType.Create<Improbable.PlayerLifecycle.PlayerCreator.Component>(),
                    ComponentType.ReadOnly<Authoritative<Improbable.PlayerLifecycle.PlayerCreator.Component>>(),
                    ComponentType.ReadOnly<SpatialEntityId>()
                },
                Any = Array.Empty<ComponentType>(),
                None = Array.Empty<ComponentType>(),
            };

            public override EntityArchetypeQuery[] CommandQueries => new EntityArchetypeQuery[]
            {
                new EntityArchetypeQuery()
                {
                    All = new[]
                    {
                        ComponentType.Create<Improbable.PlayerLifecycle.PlayerCreator.CommandSenders.CreatePlayer>(),
                        ComponentType.Create<Improbable.PlayerLifecycle.PlayerCreator.CommandResponders.CreatePlayer>(),
                    },
                    Any = Array.Empty<ComponentType>(),
                    None = Array.Empty<ComponentType>(),
                },
            };

            private CommandStorages.CreatePlayer createPlayerStorage;

            public ComponentReplicator(EntityManager entityManager, Unity.Entities.World world) : base(entityManager)
            {
                var bookkeepingSystem = world.GetOrCreateManager<CommandRequestTrackerSystem>();
                createPlayerStorage = bookkeepingSystem.GetCommandStorageForType<CommandStorages.CreatePlayer>();
            }

            public override void ExecuteReplication(ComponentGroup replicationGroup, ComponentSystemBase system, global::Improbable.Worker.CInterop.Connection connection)
            {
                Profiler.BeginSample("PlayerCreator");

                var chunkArray = replicationGroup.CreateArchetypeChunkArray(Allocator.TempJob);
                var spatialOSEntityType = system.GetArchetypeChunkComponentType<SpatialEntityId>(true);
                var componentType = system.GetArchetypeChunkComponentType<Improbable.PlayerLifecycle.PlayerCreator.Component>();
                foreach (var chunk in chunkArray)
                {
                    var entityIdArray = chunk.GetNativeArray(spatialOSEntityType);
                    var componentArray = chunk.GetNativeArray(componentType);
                    for (var i = 0; i < componentArray.Length; i++)
                    {
                        var data = componentArray[i];
                        var eventsToSend = 0;

                        if (data.IsDataDirty() || eventsToSend > 0)
                        {
                            var update = new global::Improbable.Worker.CInterop.SchemaComponentUpdate(13000);
                            Improbable.PlayerLifecycle.PlayerCreator.Serialization.SerializeUpdate(data, update);

                            // Send serialized update over the wire
                            connection.SendComponentUpdate(entityIdArray[i].EntityId.Id, new global::Improbable.Worker.CInterop.ComponentUpdate(update), UpdateParameters);

                            data.MarkDataClean();
                            componentArray[i] = data;
                        }
                    }
                }

                chunkArray.Dispose();
                Profiler.EndSample();
            }

            public override void SendCommands(ComponentGroup commandGroup, ComponentSystemBase system, global::Improbable.Worker.CInterop.Connection connection)
            {
                Profiler.BeginSample("PlayerCreator");
                var entityType = system.GetArchetypeChunkEntityType();
                {
                    var senderType = system.GetArchetypeChunkComponentType<Improbable.PlayerLifecycle.PlayerCreator.CommandSenders.CreatePlayer>(true);
                    var responderType = system.GetArchetypeChunkComponentType<Improbable.PlayerLifecycle.PlayerCreator.CommandResponders.CreatePlayer>(true);

                    var chunks = commandGroup.CreateArchetypeChunkArray(Allocator.TempJob);
                    foreach (var chunk in chunks)
                    {
                        var entities = chunk.GetNativeArray(entityType);
                        var senders = chunk.GetNativeArray(senderType);
                        var responders = chunk.GetNativeArray(responderType);
                        for (var i = 0; i < senders.Length; i++)
                        {
                            var requests = senders[i].RequestsToSend;
                            var responses = responders[i].ResponsesToSend;
                            if (requests.Count > 0)
                            {
                                foreach (var request in requests)
                                {
                                    var schemaCommandRequest = new global::Improbable.Worker.CInterop.SchemaCommandRequest(ComponentId, 1);
                                    global::Improbable.PlayerLifecycle.CreatePlayerRequestType.Serialization.Serialize(request.Payload, schemaCommandRequest.GetObject());

                                    var requestId = connection.SendCommandRequest(request.TargetEntityId.Id,
                                        new global::Improbable.Worker.CInterop.CommandRequest(schemaCommandRequest),
                                        1,
                                        request.TimeoutMillis,
                                        request.AllowShortCircuiting ? ShortCircuitParameters : null);

                                    createPlayerStorage.CommandRequestsInFlight[requestId] =
                                        new CommandRequestStore<global::Improbable.PlayerLifecycle.CreatePlayerRequestType>(entities[i], request.Payload, request.Context, request.RequestId);
                                }

                                requests.Clear();
                            }

                            if (responses.Count > 0)
                            {
                                foreach (var response in responses)
                                {
                                    var requestId = response.RequestId;

                                    if (response.FailureMessage != null)
                                    {
                                        // Send a command failure if the string is non-null.
                                        connection.SendCommandFailure((uint) requestId, response.FailureMessage);
                                        continue;
                                    }

                                    var schemaCommandResponse = new global::Improbable.Worker.CInterop.SchemaCommandResponse(ComponentId, 1);
                                    global::Improbable.PlayerLifecycle.CreatePlayerResponseType.Serialization.Serialize(response.Payload.Value, schemaCommandResponse.GetObject());

                                    connection.SendCommandResponse((uint) requestId, new global::Improbable.Worker.CInterop.CommandResponse(schemaCommandResponse));
                                }

                                responses.Clear();
                            }
                        }
                    }

                    chunks.Dispose();
                }

                Profiler.EndSample();
            }
        }

        internal class ComponentCleanup : ComponentCleanupHandler
        {
            public override EntityArchetypeQuery CleanupArchetypeQuery => new EntityArchetypeQuery
            {
                All = Array.Empty<ComponentType>(),
                Any = new ComponentType[]
                {
                    ComponentType.Create<ComponentAdded<Improbable.PlayerLifecycle.PlayerCreator.Component>>(),
                    ComponentType.Create<ComponentRemoved<Improbable.PlayerLifecycle.PlayerCreator.Component>>(),
                    ComponentType.Create<Improbable.PlayerLifecycle.PlayerCreator.ReceivedUpdates>(),
                    ComponentType.Create<AuthorityChanges<Improbable.PlayerLifecycle.PlayerCreator.Component>>(),
                    ComponentType.Create<CommandRequests.CreatePlayer>(),
                    ComponentType.Create<CommandResponses.CreatePlayer>(),
                },
                None = Array.Empty<ComponentType>(),
            };

            public override void CleanComponents(ComponentGroup group, ComponentSystemBase system,
                EntityCommandBuffer buffer)
            {
                var entityType = system.GetArchetypeChunkEntityType();
                var componentAddedType = system.GetArchetypeChunkComponentType<ComponentAdded<Improbable.PlayerLifecycle.PlayerCreator.Component>>();
                var componentRemovedType = system.GetArchetypeChunkComponentType<ComponentRemoved<Improbable.PlayerLifecycle.PlayerCreator.Component>>();
                var receivedUpdateType = system.GetArchetypeChunkComponentType<Improbable.PlayerLifecycle.PlayerCreator.ReceivedUpdates>();
                var authorityChangeType = system.GetArchetypeChunkComponentType<AuthorityChanges<Improbable.PlayerLifecycle.PlayerCreator.Component>>();

                var createPlayerRequestType = system.GetArchetypeChunkComponentType<CommandRequests.CreatePlayer>();
                var createPlayerResponseType = system.GetArchetypeChunkComponentType<CommandResponses.CreatePlayer>();

                var chunkArray = group.CreateArchetypeChunkArray(Allocator.TempJob);

                foreach (var chunk in chunkArray)
                {
                    var entities = chunk.GetNativeArray(entityType);

                    // Updates
                    if (chunk.Has(receivedUpdateType))
                    {
                        var updateArray = chunk.GetNativeArray(receivedUpdateType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<Improbable.PlayerLifecycle.PlayerCreator.ReceivedUpdates>(entities[i]);
                            var updateList = updateArray[i].Updates;

                            // Pool update lists to avoid excessive allocation
                            updateList.Clear();
                            Improbable.PlayerLifecycle.PlayerCreator.Update.Pool.Push(updateList);

                            ReferenceTypeProviders.UpdatesProvider.Free(updateArray[i].handle);
                        }
                    }

                    // Component Added
                    if (chunk.Has(componentAddedType))
                    {
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<ComponentAdded<Improbable.PlayerLifecycle.PlayerCreator.Component>>(entities[i]);
                        }
                    }

                    // Component Removed
                    if (chunk.Has(componentRemovedType))
                    {
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<ComponentRemoved<Improbable.PlayerLifecycle.PlayerCreator.Component>>(entities[i]);
                        }
                    }

                    // Authority
                    if (chunk.Has(authorityChangeType))
                    {
                        var authorityChangeArray = chunk.GetNativeArray(authorityChangeType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<AuthorityChanges<Improbable.PlayerLifecycle.PlayerCreator.Component>>(entities[i]);
                            AuthorityChangesProvider.Free(authorityChangeArray[i].Handle);
                        }
                    }

                    // CreatePlayer Command
                    if (chunk.Has(createPlayerRequestType))
                    {
                            var createPlayerRequestArray = chunk.GetNativeArray(createPlayerRequestType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<CommandRequests.CreatePlayer>(entities[i]);
                            ReferenceTypeProviders.CreatePlayerRequestsProvider.Free(createPlayerRequestArray[i].CommandListHandle);
                        }
                    }

                    if (chunk.Has(createPlayerResponseType))
                    {
                        var createPlayerResponseArray = chunk.GetNativeArray(createPlayerResponseType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<CommandResponses.CreatePlayer>(entities[i]);
                            ReferenceTypeProviders.CreatePlayerResponsesProvider.Free(createPlayerResponseArray[i].CommandListHandle);
                        }
                    }
                }

                chunkArray.Dispose();
            }
        }

        internal class AcknowledgeAuthorityLossHandler : AbstractAcknowledgeAuthorityLossHandler
        {
            public override EntityArchetypeQuery Query => new EntityArchetypeQuery
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<AuthorityLossImminent<Improbable.PlayerLifecycle.PlayerCreator.Component>>(),
                    ComponentType.ReadOnly<SpatialEntityId>()
                },
                Any = Array.Empty<ComponentType>(),
                None = Array.Empty<ComponentType>()
            };

            public override void AcknowledgeAuthorityLoss(ComponentGroup group, ComponentSystemBase system,
                Improbable.Worker.CInterop.Connection connection)
            {
                var authorityLossType = system.GetArchetypeChunkComponentType<AuthorityLossImminent<Improbable.PlayerLifecycle.PlayerCreator.Component>>();
                var spatialEntityType = system.GetArchetypeChunkComponentType<SpatialEntityId>();

                var chunkArray = group.CreateArchetypeChunkArray(Allocator.TempJob);

                foreach (var chunk in chunkArray)
                {
                    var authorityArray = chunk.GetNativeArray(authorityLossType);
                    var spatialEntityIdArray = chunk.GetNativeArray(spatialEntityType);

                    for (int i = 0; i < authorityArray.Length; ++i)
                    {
                        if (authorityArray[i].AcknowledgeAuthorityLoss)
                        {
                            connection.SendAuthorityLossImminentAcknowledgement(spatialEntityIdArray[i].EntityId.Id,
                                13000);
                        }
                    }
                }

                chunkArray.Dispose();
            }
        }
    }
}
