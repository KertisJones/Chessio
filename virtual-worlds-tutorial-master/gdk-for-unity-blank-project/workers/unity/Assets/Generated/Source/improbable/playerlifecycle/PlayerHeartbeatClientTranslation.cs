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
    public partial class PlayerHeartbeatClient
    {
        internal class DispatcherHandler : ComponentDispatcherHandler
        {
            public override uint ComponentId => 13001;

            private readonly EntityManager entityManager;

            private const string LoggerName = "PlayerHeartbeatClient.DispatcherHandler";

            private CommandStorages.PlayerHeartbeat playerHeartbeatStorage;

            public DispatcherHandler(WorkerSystem worker, World world) : base(worker, world)
            {
                entityManager = world.GetOrCreateManager<EntityManager>();
                var bookkeepingSystem = world.GetOrCreateManager<CommandRequestTrackerSystem>();
                playerHeartbeatStorage = bookkeepingSystem.GetCommandStorageForType<CommandStorages.PlayerHeartbeat>();
            }

            public override void Dispose()
            {
                PlayerHeartbeatClient.ReferenceTypeProviders.UpdatesProvider.CleanDataInWorld(World);
                PlayerHeartbeatClient.ReferenceTypeProviders.PlayerHeartbeatSenderProvider.CleanDataInWorld(World);
                PlayerHeartbeatClient.ReferenceTypeProviders.PlayerHeartbeatRequestsProvider.CleanDataInWorld(World);
                PlayerHeartbeatClient.ReferenceTypeProviders.PlayerHeartbeatResponderProvider.CleanDataInWorld(World);
                PlayerHeartbeatClient.ReferenceTypeProviders.PlayerHeartbeatResponsesProvider.CleanDataInWorld(World);
            }

            public override void OnAddComponent(AddComponentOp op)
            {
                var entity = TryGetEntityFromEntityId(new EntityId(op.EntityId));

                Profiler.BeginSample("PlayerHeartbeatClient");
                var data = Improbable.PlayerLifecycle.PlayerHeartbeatClient.Serialization.Deserialize(op.Data.SchemaData.Value.GetFields(), World);
                data.MarkDataClean();
                entityManager.AddComponentData(entity, data);
                entityManager.AddComponent(entity, ComponentType.Create<NotAuthoritative<Improbable.PlayerLifecycle.PlayerHeartbeatClient.Component>>());

                var update = new Improbable.PlayerLifecycle.PlayerHeartbeatClient.Update
                {
                };

                var updates = new List<Improbable.PlayerLifecycle.PlayerHeartbeatClient.Update>
                {
                    update
                };

                var updatesComponent = new Improbable.PlayerLifecycle.PlayerHeartbeatClient.ReceivedUpdates
                {
                    handle = ReferenceTypeProviders.UpdatesProvider.Allocate(World)
                };

                ReferenceTypeProviders.UpdatesProvider.Set(updatesComponent.handle, updates);
                entityManager.AddComponentData(entity, updatesComponent);

                if (entityManager.HasComponent<ComponentRemoved<Improbable.PlayerLifecycle.PlayerHeartbeatClient.Component>>(entity))
                {
                    entityManager.RemoveComponent<ComponentRemoved<Improbable.PlayerLifecycle.PlayerHeartbeatClient.Component>>(entity);
                }
                else if (!entityManager.HasComponent<ComponentAdded<Improbable.PlayerLifecycle.PlayerHeartbeatClient.Component>>(entity))
                {
                    entityManager.AddComponent(entity, ComponentType.Create<ComponentAdded<Improbable.PlayerLifecycle.PlayerHeartbeatClient.Component>>());
                }
                else
                {
                    LogDispatcher.HandleLog(LogType.Error, new LogEvent(ReceivedDuplicateComponentAdded)
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField(LoggingUtils.EntityId, op.EntityId)
                        .WithField("Component", "Improbable.PlayerLifecycle.PlayerHeartbeatClient")
                    );
                }

                Profiler.EndSample();
            }

            public override void OnRemoveComponent(RemoveComponentOp op)
            {
                var entity = TryGetEntityFromEntityId(new EntityId(op.EntityId));

                Profiler.BeginSample("PlayerHeartbeatClient");

                entityManager.RemoveComponent<Improbable.PlayerLifecycle.PlayerHeartbeatClient.Component>(entity);

                if (entityManager.HasComponent<ComponentAdded<Improbable.PlayerLifecycle.PlayerHeartbeatClient.Component>>(entity))
                {
                    entityManager.RemoveComponent<ComponentAdded<Improbable.PlayerLifecycle.PlayerHeartbeatClient.Component>>(entity);
                }
                else if (!entityManager.HasComponent<ComponentRemoved<Improbable.PlayerLifecycle.PlayerHeartbeatClient.Component>>(entity))
                {
                    entityManager.AddComponent(entity, ComponentType.Create<ComponentRemoved<Improbable.PlayerLifecycle.PlayerHeartbeatClient.Component>>());
                }
                else
                {
                    LogDispatcher.HandleLog(LogType.Error, new LogEvent(ReceivedDuplicateComponentRemoved)
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField(LoggingUtils.EntityId, op.EntityId)
                        .WithField("Component", "Improbable.PlayerLifecycle.PlayerHeartbeatClient")
                    );
                }

                Profiler.EndSample();
            }

            public override void OnComponentUpdate(ComponentUpdateOp op)
            {
                var entity = TryGetEntityFromEntityId(new EntityId(op.EntityId));

                Profiler.BeginSample("PlayerHeartbeatClient");
                if (entityManager.HasComponent<NotAuthoritative<Improbable.PlayerLifecycle.PlayerHeartbeatClient.Component>>(entity))
                {
                    var data = entityManager.GetComponentData<Improbable.PlayerLifecycle.PlayerHeartbeatClient.Component>(entity);
                    Improbable.PlayerLifecycle.PlayerHeartbeatClient.Serialization.ApplyUpdate(op.Update.SchemaData.Value, ref data);
                    data.MarkDataClean();
                    entityManager.SetComponentData(entity, data);
                }

                var update = Improbable.PlayerLifecycle.PlayerHeartbeatClient.Serialization.DeserializeUpdate(op.Update.SchemaData.Value);

                List<Improbable.PlayerLifecycle.PlayerHeartbeatClient.Update> updates;
                if (entityManager.HasComponent<Improbable.PlayerLifecycle.PlayerHeartbeatClient.ReceivedUpdates>(entity))
                {
                    updates = entityManager.GetComponentData<Improbable.PlayerLifecycle.PlayerHeartbeatClient.ReceivedUpdates>(entity).Updates;
                }
                else
                {
                    updates = Improbable.PlayerLifecycle.PlayerHeartbeatClient.Update.Pool.Count > 0 ? Improbable.PlayerLifecycle.PlayerHeartbeatClient.Update.Pool.Pop() : new List<Improbable.PlayerLifecycle.PlayerHeartbeatClient.Update>();
                    var updatesComponent = new Improbable.PlayerLifecycle.PlayerHeartbeatClient.ReceivedUpdates
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

                Profiler.BeginSample("PlayerHeartbeatClient");
                ApplyAuthorityChange(entity, op.Authority, entityId);
                Profiler.EndSample();
            }

            public override void OnCommandRequest(CommandRequestOp op)
            {
                var commandIndex = op.Request.SchemaData.Value.GetCommandIndex();

                Profiler.BeginSample("PlayerHeartbeatClient");
                switch (commandIndex)
                {
                    case 1:
                        OnPlayerHeartbeatRequest(op);
                        break;
                    default:
                        throw new UnknownCommandIndexException(commandIndex, "PlayerHeartbeatClient");
                }

                Profiler.EndSample();
            }

            public override void OnCommandResponse(CommandResponseOp op)
            {
                var commandIndex = op.CommandIndex;

                Profiler.BeginSample("PlayerHeartbeatClient");
                switch (commandIndex)
                {
                    case 1:
                        OnPlayerHeartbeatResponse(op);
                        break;
                    default:
                        throw new UnknownCommandIndexException(commandIndex, "PlayerHeartbeatClient");
                }

                Profiler.EndSample();
            }

            public override void AddCommandComponents(Unity.Entities.Entity entity)
            {
                {
                    var commandSender = new Improbable.PlayerLifecycle.PlayerHeartbeatClient.CommandSenders.PlayerHeartbeat();
                    commandSender.CommandListHandle = Improbable.PlayerLifecycle.PlayerHeartbeatClient.ReferenceTypeProviders.PlayerHeartbeatSenderProvider.Allocate(World);
                    commandSender.RequestsToSend = new List<Improbable.PlayerLifecycle.PlayerHeartbeatClient.PlayerHeartbeat.Request>();

                    entityManager.AddComponentData(entity, commandSender);

                    var commandResponder = new Improbable.PlayerLifecycle.PlayerHeartbeatClient.CommandResponders.PlayerHeartbeat();
                    commandResponder.CommandListHandle = Improbable.PlayerLifecycle.PlayerHeartbeatClient.ReferenceTypeProviders.PlayerHeartbeatResponderProvider.Allocate(World);
                    commandResponder.ResponsesToSend = new List<Improbable.PlayerLifecycle.PlayerHeartbeatClient.PlayerHeartbeat.Response>();

                    entityManager.AddComponentData(entity, commandResponder);
                }
            }

            private void ApplyAuthorityChange(Unity.Entities.Entity entity, Authority authority, global::Improbable.Gdk.Core.EntityId entityId)
            {
                switch (authority)
                {
                    case Authority.Authoritative:
                        if (!entityManager.HasComponent<NotAuthoritative<Improbable.PlayerLifecycle.PlayerHeartbeatClient.Component>>(entity))
                        {
                            LogInvalidAuthorityTransition(Authority.Authoritative, Authority.NotAuthoritative, entityId);
                            return;
                        }

                        entityManager.RemoveComponent<NotAuthoritative<Improbable.PlayerLifecycle.PlayerHeartbeatClient.Component>>(entity);
                        entityManager.AddComponent(entity, ComponentType.Create<Authoritative<Improbable.PlayerLifecycle.PlayerHeartbeatClient.Component>>());

                        // Add event senders
                        break;
                    case Authority.AuthorityLossImminent:
                        if (!entityManager.HasComponent<Authoritative<Improbable.PlayerLifecycle.PlayerHeartbeatClient.Component>>(entity))
                        {
                            LogInvalidAuthorityTransition(Authority.AuthorityLossImminent, Authority.Authoritative, entityId);
                            return;
                        }

                        entityManager.AddComponent(entity, ComponentType.Create<AuthorityLossImminent<Improbable.PlayerLifecycle.PlayerHeartbeatClient.Component>>());
                        break;
                    case Authority.NotAuthoritative:
                        if (!entityManager.HasComponent<Authoritative<Improbable.PlayerLifecycle.PlayerHeartbeatClient.Component>>(entity))
                        {
                            LogInvalidAuthorityTransition(Authority.NotAuthoritative, Authority.Authoritative, entityId);
                            return;
                        }

                        if (entityManager.HasComponent<AuthorityLossImminent<Improbable.PlayerLifecycle.PlayerHeartbeatClient.Component>>(entity))
                        {
                            entityManager.RemoveComponent<AuthorityLossImminent<Improbable.PlayerLifecycle.PlayerHeartbeatClient.Component>>(entity);
                        }

                        entityManager.RemoveComponent<Authoritative<Improbable.PlayerLifecycle.PlayerHeartbeatClient.Component>>(entity);
                        entityManager.AddComponent(entity, ComponentType.Create<NotAuthoritative<Improbable.PlayerLifecycle.PlayerHeartbeatClient.Component>>());

                        // Remove event senders
                        break;
                }

                List<Authority> authorityChanges;
                if (entityManager.HasComponent<AuthorityChanges<Improbable.PlayerLifecycle.PlayerHeartbeatClient.Component>>(entity))
                {
                    authorityChanges = entityManager.GetComponentData<AuthorityChanges<Improbable.PlayerLifecycle.PlayerHeartbeatClient.Component>>(entity).Changes;

                }
                else
                {
                    var changes = new AuthorityChanges<Improbable.PlayerLifecycle.PlayerHeartbeatClient.Component>
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
                    .WithField("Component", "Improbable.PlayerLifecycle.PlayerHeartbeatClient")
                );
            }

            private void OnPlayerHeartbeatRequest(CommandRequestOp op)
            {
                var entity = TryGetEntityFromEntityId(new EntityId(op.EntityId));

                var deserializedRequest = global::Improbable.Common.Empty.Serialization.Deserialize(op.Request.SchemaData.Value.GetObject());

                List<Improbable.PlayerLifecycle.PlayerHeartbeatClient.PlayerHeartbeat.ReceivedRequest> requests;
                if (entityManager.HasComponent<Improbable.PlayerLifecycle.PlayerHeartbeatClient.CommandRequests.PlayerHeartbeat>(entity))
                {
                    requests = entityManager.GetComponentData<Improbable.PlayerLifecycle.PlayerHeartbeatClient.CommandRequests.PlayerHeartbeat>(entity).Requests;
                }
                else
                {
                    var data = new Improbable.PlayerLifecycle.PlayerHeartbeatClient.CommandRequests.PlayerHeartbeat
                    {
                        CommandListHandle = Improbable.PlayerLifecycle.PlayerHeartbeatClient.ReferenceTypeProviders.PlayerHeartbeatRequestsProvider.Allocate(World)
                    };
                    requests = data.Requests = new List<Improbable.PlayerLifecycle.PlayerHeartbeatClient.PlayerHeartbeat.ReceivedRequest>();
                    entityManager.AddComponentData(entity, data);
                }

                requests.Add(new Improbable.PlayerLifecycle.PlayerHeartbeatClient.PlayerHeartbeat.ReceivedRequest(op.RequestId,
                    op.CallerWorkerId,
                    op.CallerAttributeSet,
                    deserializedRequest));
            }

            private void OnPlayerHeartbeatResponse(CommandResponseOp op)
            {
                if (!playerHeartbeatStorage.CommandRequestsInFlight.TryGetValue(op.RequestId, out var requestBundle))
                {
                    throw new InvalidOperationException($"Could not find corresponding request for RequestId {op.RequestId} and command PlayerHeartbeat.");
                }

                var entity = requestBundle.Entity;
                playerHeartbeatStorage.CommandRequestsInFlight.Remove(op.RequestId);
                if (!entityManager.Exists(entity))
                {
                    LogDispatcher.HandleLog(LogType.Log, new LogEvent(EntityNotFound)
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField("Op", "CommandResponseOp - PlayerHeartbeat")
                        .WithField("Component", "Improbable.PlayerLifecycle.PlayerHeartbeatClient")
                    );
                    return;
                }

                global::Improbable.Common.Empty? response = null;
                if (op.StatusCode == StatusCode.Success)
                {
                    response = global::Improbable.Common.Empty.Serialization.Deserialize(op.Response.SchemaData.Value.GetObject());
                }

                List<Improbable.PlayerLifecycle.PlayerHeartbeatClient.PlayerHeartbeat.ReceivedResponse> responses;
                if (entityManager.HasComponent<Improbable.PlayerLifecycle.PlayerHeartbeatClient.CommandResponses.PlayerHeartbeat>(entity))
                {
                    responses = entityManager.GetComponentData<Improbable.PlayerLifecycle.PlayerHeartbeatClient.CommandResponses.PlayerHeartbeat>(entity).Responses;
                }
                else
                {
                    var data = new Improbable.PlayerLifecycle.PlayerHeartbeatClient.CommandResponses.PlayerHeartbeat
                    {
                        CommandListHandle = Improbable.PlayerLifecycle.PlayerHeartbeatClient.ReferenceTypeProviders.PlayerHeartbeatResponsesProvider.Allocate(World)
                    };
                    responses = data.Responses = new List<Improbable.PlayerLifecycle.PlayerHeartbeatClient.PlayerHeartbeat.ReceivedResponse>();
                    entityManager.AddComponentData(entity, data);
                }

                responses.Add(new Improbable.PlayerLifecycle.PlayerHeartbeatClient.PlayerHeartbeat.ReceivedResponse(new EntityId(op.EntityId),
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
            public override uint ComponentId => 13001;

            public override EntityArchetypeQuery ComponentUpdateQuery => new EntityArchetypeQuery
            {
                All = new[]
                {
                    ComponentType.Create<Improbable.PlayerLifecycle.PlayerHeartbeatClient.Component>(),
                    ComponentType.ReadOnly<Authoritative<Improbable.PlayerLifecycle.PlayerHeartbeatClient.Component>>(),
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
                        ComponentType.Create<Improbable.PlayerLifecycle.PlayerHeartbeatClient.CommandSenders.PlayerHeartbeat>(),
                        ComponentType.Create<Improbable.PlayerLifecycle.PlayerHeartbeatClient.CommandResponders.PlayerHeartbeat>(),
                    },
                    Any = Array.Empty<ComponentType>(),
                    None = Array.Empty<ComponentType>(),
                },
            };

            private CommandStorages.PlayerHeartbeat playerHeartbeatStorage;

            public ComponentReplicator(EntityManager entityManager, Unity.Entities.World world) : base(entityManager)
            {
                var bookkeepingSystem = world.GetOrCreateManager<CommandRequestTrackerSystem>();
                playerHeartbeatStorage = bookkeepingSystem.GetCommandStorageForType<CommandStorages.PlayerHeartbeat>();
            }

            public override void ExecuteReplication(ComponentGroup replicationGroup, ComponentSystemBase system, global::Improbable.Worker.CInterop.Connection connection)
            {
                Profiler.BeginSample("PlayerHeartbeatClient");

                var chunkArray = replicationGroup.CreateArchetypeChunkArray(Allocator.TempJob);
                var spatialOSEntityType = system.GetArchetypeChunkComponentType<SpatialEntityId>(true);
                var componentType = system.GetArchetypeChunkComponentType<Improbable.PlayerLifecycle.PlayerHeartbeatClient.Component>();
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
                            var update = new global::Improbable.Worker.CInterop.SchemaComponentUpdate(13001);
                            Improbable.PlayerLifecycle.PlayerHeartbeatClient.Serialization.SerializeUpdate(data, update);

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
                Profiler.BeginSample("PlayerHeartbeatClient");
                var entityType = system.GetArchetypeChunkEntityType();
                {
                    var senderType = system.GetArchetypeChunkComponentType<Improbable.PlayerLifecycle.PlayerHeartbeatClient.CommandSenders.PlayerHeartbeat>(true);
                    var responderType = system.GetArchetypeChunkComponentType<Improbable.PlayerLifecycle.PlayerHeartbeatClient.CommandResponders.PlayerHeartbeat>(true);

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
                                    global::Improbable.Common.Empty.Serialization.Serialize(request.Payload, schemaCommandRequest.GetObject());

                                    var requestId = connection.SendCommandRequest(request.TargetEntityId.Id,
                                        new global::Improbable.Worker.CInterop.CommandRequest(schemaCommandRequest),
                                        1,
                                        request.TimeoutMillis,
                                        request.AllowShortCircuiting ? ShortCircuitParameters : null);

                                    playerHeartbeatStorage.CommandRequestsInFlight[requestId] =
                                        new CommandRequestStore<global::Improbable.Common.Empty>(entities[i], request.Payload, request.Context, request.RequestId);
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
                                    global::Improbable.Common.Empty.Serialization.Serialize(response.Payload.Value, schemaCommandResponse.GetObject());

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
                    ComponentType.Create<ComponentAdded<Improbable.PlayerLifecycle.PlayerHeartbeatClient.Component>>(),
                    ComponentType.Create<ComponentRemoved<Improbable.PlayerLifecycle.PlayerHeartbeatClient.Component>>(),
                    ComponentType.Create<Improbable.PlayerLifecycle.PlayerHeartbeatClient.ReceivedUpdates>(),
                    ComponentType.Create<AuthorityChanges<Improbable.PlayerLifecycle.PlayerHeartbeatClient.Component>>(),
                    ComponentType.Create<CommandRequests.PlayerHeartbeat>(),
                    ComponentType.Create<CommandResponses.PlayerHeartbeat>(),
                },
                None = Array.Empty<ComponentType>(),
            };

            public override void CleanComponents(ComponentGroup group, ComponentSystemBase system,
                EntityCommandBuffer buffer)
            {
                var entityType = system.GetArchetypeChunkEntityType();
                var componentAddedType = system.GetArchetypeChunkComponentType<ComponentAdded<Improbable.PlayerLifecycle.PlayerHeartbeatClient.Component>>();
                var componentRemovedType = system.GetArchetypeChunkComponentType<ComponentRemoved<Improbable.PlayerLifecycle.PlayerHeartbeatClient.Component>>();
                var receivedUpdateType = system.GetArchetypeChunkComponentType<Improbable.PlayerLifecycle.PlayerHeartbeatClient.ReceivedUpdates>();
                var authorityChangeType = system.GetArchetypeChunkComponentType<AuthorityChanges<Improbable.PlayerLifecycle.PlayerHeartbeatClient.Component>>();

                var playerHeartbeatRequestType = system.GetArchetypeChunkComponentType<CommandRequests.PlayerHeartbeat>();
                var playerHeartbeatResponseType = system.GetArchetypeChunkComponentType<CommandResponses.PlayerHeartbeat>();

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
                            buffer.RemoveComponent<Improbable.PlayerLifecycle.PlayerHeartbeatClient.ReceivedUpdates>(entities[i]);
                            var updateList = updateArray[i].Updates;

                            // Pool update lists to avoid excessive allocation
                            updateList.Clear();
                            Improbable.PlayerLifecycle.PlayerHeartbeatClient.Update.Pool.Push(updateList);

                            ReferenceTypeProviders.UpdatesProvider.Free(updateArray[i].handle);
                        }
                    }

                    // Component Added
                    if (chunk.Has(componentAddedType))
                    {
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<ComponentAdded<Improbable.PlayerLifecycle.PlayerHeartbeatClient.Component>>(entities[i]);
                        }
                    }

                    // Component Removed
                    if (chunk.Has(componentRemovedType))
                    {
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<ComponentRemoved<Improbable.PlayerLifecycle.PlayerHeartbeatClient.Component>>(entities[i]);
                        }
                    }

                    // Authority
                    if (chunk.Has(authorityChangeType))
                    {
                        var authorityChangeArray = chunk.GetNativeArray(authorityChangeType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<AuthorityChanges<Improbable.PlayerLifecycle.PlayerHeartbeatClient.Component>>(entities[i]);
                            AuthorityChangesProvider.Free(authorityChangeArray[i].Handle);
                        }
                    }

                    // PlayerHeartbeat Command
                    if (chunk.Has(playerHeartbeatRequestType))
                    {
                            var playerHeartbeatRequestArray = chunk.GetNativeArray(playerHeartbeatRequestType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<CommandRequests.PlayerHeartbeat>(entities[i]);
                            ReferenceTypeProviders.PlayerHeartbeatRequestsProvider.Free(playerHeartbeatRequestArray[i].CommandListHandle);
                        }
                    }

                    if (chunk.Has(playerHeartbeatResponseType))
                    {
                        var playerHeartbeatResponseArray = chunk.GetNativeArray(playerHeartbeatResponseType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<CommandResponses.PlayerHeartbeat>(entities[i]);
                            ReferenceTypeProviders.PlayerHeartbeatResponsesProvider.Free(playerHeartbeatResponseArray[i].CommandListHandle);
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
                    ComponentType.ReadOnly<AuthorityLossImminent<Improbable.PlayerLifecycle.PlayerHeartbeatClient.Component>>(),
                    ComponentType.ReadOnly<SpatialEntityId>()
                },
                Any = Array.Empty<ComponentType>(),
                None = Array.Empty<ComponentType>()
            };

            public override void AcknowledgeAuthorityLoss(ComponentGroup group, ComponentSystemBase system,
                Improbable.Worker.CInterop.Connection connection)
            {
                var authorityLossType = system.GetArchetypeChunkComponentType<AuthorityLossImminent<Improbable.PlayerLifecycle.PlayerHeartbeatClient.Component>>();
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
                                13001);
                        }
                    }
                }

                chunkArray.Dispose();
            }
        }
    }
}
