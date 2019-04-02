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

namespace Improbable.Player
{
    public partial class PlayerControls
    {
        internal class DispatcherHandler : ComponentDispatcherHandler
        {
            public override uint ComponentId => 1001;

            private readonly EntityManager entityManager;

            private const string LoggerName = "PlayerControls.DispatcherHandler";


            public DispatcherHandler(WorkerSystem worker, World world) : base(worker, world)
            {
                entityManager = world.GetOrCreateManager<EntityManager>();
                var bookkeepingSystem = world.GetOrCreateManager<CommandRequestTrackerSystem>();
            }

            public override void Dispose()
            {
                PlayerControls.ReferenceTypeProviders.UpdatesProvider.CleanDataInWorld(World);
                PlayerControls.ReferenceTypeProviders.MovementUpdateProvider.CleanDataInWorld(World);
            }

            public override void OnAddComponent(AddComponentOp op)
            {
                var entity = TryGetEntityFromEntityId(new EntityId(op.EntityId));

                Profiler.BeginSample("PlayerControls");
                var data = Improbable.Player.PlayerControls.Serialization.Deserialize(op.Data.SchemaData.Value.GetFields(), World);
                data.MarkDataClean();
                entityManager.AddComponentData(entity, data);
                entityManager.AddComponent(entity, ComponentType.Create<NotAuthoritative<Improbable.Player.PlayerControls.Component>>());

                var update = new Improbable.Player.PlayerControls.Update
                {
                };

                var updates = new List<Improbable.Player.PlayerControls.Update>
                {
                    update
                };

                var updatesComponent = new Improbable.Player.PlayerControls.ReceivedUpdates
                {
                    handle = ReferenceTypeProviders.UpdatesProvider.Allocate(World)
                };

                ReferenceTypeProviders.UpdatesProvider.Set(updatesComponent.handle, updates);
                entityManager.AddComponentData(entity, updatesComponent);

                if (entityManager.HasComponent<ComponentRemoved<Improbable.Player.PlayerControls.Component>>(entity))
                {
                    entityManager.RemoveComponent<ComponentRemoved<Improbable.Player.PlayerControls.Component>>(entity);
                }
                else if (!entityManager.HasComponent<ComponentAdded<Improbable.Player.PlayerControls.Component>>(entity))
                {
                    entityManager.AddComponent(entity, ComponentType.Create<ComponentAdded<Improbable.Player.PlayerControls.Component>>());
                }
                else
                {
                    LogDispatcher.HandleLog(LogType.Error, new LogEvent(ReceivedDuplicateComponentAdded)
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField(LoggingUtils.EntityId, op.EntityId)
                        .WithField("Component", "Improbable.Player.PlayerControls")
                    );
                }

                Profiler.EndSample();
            }

            public override void OnRemoveComponent(RemoveComponentOp op)
            {
                var entity = TryGetEntityFromEntityId(new EntityId(op.EntityId));

                Profiler.BeginSample("PlayerControls");

                entityManager.RemoveComponent<Improbable.Player.PlayerControls.Component>(entity);

                if (entityManager.HasComponent<ComponentAdded<Improbable.Player.PlayerControls.Component>>(entity))
                {
                    entityManager.RemoveComponent<ComponentAdded<Improbable.Player.PlayerControls.Component>>(entity);
                }
                else if (!entityManager.HasComponent<ComponentRemoved<Improbable.Player.PlayerControls.Component>>(entity))
                {
                    entityManager.AddComponent(entity, ComponentType.Create<ComponentRemoved<Improbable.Player.PlayerControls.Component>>());
                }
                else
                {
                    LogDispatcher.HandleLog(LogType.Error, new LogEvent(ReceivedDuplicateComponentRemoved)
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField(LoggingUtils.EntityId, op.EntityId)
                        .WithField("Component", "Improbable.Player.PlayerControls")
                    );
                }

                Profiler.EndSample();
            }

            public override void OnComponentUpdate(ComponentUpdateOp op)
            {
                var entity = TryGetEntityFromEntityId(new EntityId(op.EntityId));

                Profiler.BeginSample("PlayerControls");
                if (entityManager.HasComponent<NotAuthoritative<Improbable.Player.PlayerControls.Component>>(entity))
                {
                    var data = entityManager.GetComponentData<Improbable.Player.PlayerControls.Component>(entity);
                    Improbable.Player.PlayerControls.Serialization.ApplyUpdate(op.Update.SchemaData.Value, ref data);
                    data.MarkDataClean();
                    entityManager.SetComponentData(entity, data);
                }

                var update = Improbable.Player.PlayerControls.Serialization.DeserializeUpdate(op.Update.SchemaData.Value);

                List<Improbable.Player.PlayerControls.Update> updates;
                if (entityManager.HasComponent<Improbable.Player.PlayerControls.ReceivedUpdates>(entity))
                {
                    updates = entityManager.GetComponentData<Improbable.Player.PlayerControls.ReceivedUpdates>(entity).Updates;
                }
                else
                {
                    updates = Improbable.Player.PlayerControls.Update.Pool.Count > 0 ? Improbable.Player.PlayerControls.Update.Pool.Pop() : new List<Improbable.Player.PlayerControls.Update>();
                    var updatesComponent = new Improbable.Player.PlayerControls.ReceivedUpdates
                    {
                        handle = ReferenceTypeProviders.UpdatesProvider.Allocate(World)
                    };
                    ReferenceTypeProviders.UpdatesProvider.Set(updatesComponent.handle, updates);
                    entityManager.AddComponentData(entity, updatesComponent);
                }

                updates.Add(update);

                var eventsObject = op.Update.SchemaData.Value.GetEvents();
                {
                    var eventCount = eventsObject.GetObjectCount(1);
                    if (eventCount > 0)
                    {
                        // Create component to hold received events
                        ReceivedEvents.MovementUpdate eventsReceived;
                        List<global::Improbable.Player.MovementUpdate> eventList;
                        if (!entityManager.HasComponent<ReceivedEvents.MovementUpdate>(entity))
                        {
                            eventsReceived = new ReceivedEvents.MovementUpdate() {
                                handle = ReferenceTypeProviders.MovementUpdateProvider.Allocate(World)
                            };
                            eventList = new List<global::Improbable.Player.MovementUpdate>((int) eventCount);
                            ReferenceTypeProviders.MovementUpdateProvider.Set(eventsReceived.handle, eventList);
                            entityManager.AddComponentData(entity, eventsReceived);
                        }
                        else
                        {
                            eventsReceived = entityManager.GetComponentData<ReceivedEvents.MovementUpdate>(entity);
                            eventList = eventsReceived.Events;
                        }

                        // Deserialize events onto component
                        for (uint i = 0; i < eventCount; i++)
                        {
                            var e = global::Improbable.Player.MovementUpdate.Serialization.Deserialize(eventsObject.IndexObject(1, i));
                            eventList.Add(e);
                        }
                    }
                }

                Profiler.EndSample();
            }

            public override void OnAuthorityChange(AuthorityChangeOp op)
            {
                var entityId = new EntityId(op.EntityId);
                var entity = TryGetEntityFromEntityId(entityId);

                Profiler.BeginSample("PlayerControls");
                ApplyAuthorityChange(entity, op.Authority, entityId);
                Profiler.EndSample();
            }

            public override void OnCommandRequest(CommandRequestOp op)
            {
                var commandIndex = op.Request.SchemaData.Value.GetCommandIndex();
                throw new UnknownCommandIndexException(commandIndex, "PlayerControls");
            }

            public override void OnCommandResponse(CommandResponseOp op)
            {
                var commandIndex = op.CommandIndex;
                throw new UnknownCommandIndexException(commandIndex, "PlayerControls");
            }

            public override void AddCommandComponents(Unity.Entities.Entity entity)
            {
            }

            private void ApplyAuthorityChange(Unity.Entities.Entity entity, Authority authority, global::Improbable.Gdk.Core.EntityId entityId)
            {
                switch (authority)
                {
                    case Authority.Authoritative:
                        if (!entityManager.HasComponent<NotAuthoritative<Improbable.Player.PlayerControls.Component>>(entity))
                        {
                            LogInvalidAuthorityTransition(Authority.Authoritative, Authority.NotAuthoritative, entityId);
                            return;
                        }

                        entityManager.RemoveComponent<NotAuthoritative<Improbable.Player.PlayerControls.Component>>(entity);
                        entityManager.AddComponent(entity, ComponentType.Create<Authoritative<Improbable.Player.PlayerControls.Component>>());

                        // Add event senders
                        {
                            var eventSender = new EventSender.MovementUpdate()
                            {
                                handle = ReferenceTypeProviders.MovementUpdateProvider.Allocate(World)
                            };
                            ReferenceTypeProviders.MovementUpdateProvider.Set(eventSender.handle, new List<global::Improbable.Player.MovementUpdate>());
                            entityManager.AddComponentData(entity, eventSender);
                        }
                        break;
                    case Authority.AuthorityLossImminent:
                        if (!entityManager.HasComponent<Authoritative<Improbable.Player.PlayerControls.Component>>(entity))
                        {
                            LogInvalidAuthorityTransition(Authority.AuthorityLossImminent, Authority.Authoritative, entityId);
                            return;
                        }

                        entityManager.AddComponent(entity, ComponentType.Create<AuthorityLossImminent<Improbable.Player.PlayerControls.Component>>());
                        break;
                    case Authority.NotAuthoritative:
                        if (!entityManager.HasComponent<Authoritative<Improbable.Player.PlayerControls.Component>>(entity))
                        {
                            LogInvalidAuthorityTransition(Authority.NotAuthoritative, Authority.Authoritative, entityId);
                            return;
                        }

                        if (entityManager.HasComponent<AuthorityLossImminent<Improbable.Player.PlayerControls.Component>>(entity))
                        {
                            entityManager.RemoveComponent<AuthorityLossImminent<Improbable.Player.PlayerControls.Component>>(entity);
                        }

                        entityManager.RemoveComponent<Authoritative<Improbable.Player.PlayerControls.Component>>(entity);
                        entityManager.AddComponent(entity, ComponentType.Create<NotAuthoritative<Improbable.Player.PlayerControls.Component>>());

                        // Remove event senders
                        {
                            var eventSender = entityManager.GetComponentData<EventSender.MovementUpdate>(entity);
                            ReferenceTypeProviders.MovementUpdateProvider.Free(eventSender.handle);
                            entityManager.RemoveComponent<EventSender.MovementUpdate>(entity);
                        }
                        break;
                }

                List<Authority> authorityChanges;
                if (entityManager.HasComponent<AuthorityChanges<Improbable.Player.PlayerControls.Component>>(entity))
                {
                    authorityChanges = entityManager.GetComponentData<AuthorityChanges<Improbable.Player.PlayerControls.Component>>(entity).Changes;

                }
                else
                {
                    var changes = new AuthorityChanges<Improbable.Player.PlayerControls.Component>
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
                    .WithField("Component", "Improbable.Player.PlayerControls")
                );
            }
        }

        internal class ComponentReplicator : ComponentReplicationHandler
        {
            public override uint ComponentId => 1001;

            public override EntityArchetypeQuery ComponentUpdateQuery => new EntityArchetypeQuery
            {
                All = new[]
                {
                    ComponentType.Create<EventSender.MovementUpdate>(),
                    ComponentType.Create<Improbable.Player.PlayerControls.Component>(),
                    ComponentType.ReadOnly<Authoritative<Improbable.Player.PlayerControls.Component>>(),
                    ComponentType.ReadOnly<SpatialEntityId>()
                },
                Any = Array.Empty<ComponentType>(),
                None = Array.Empty<ComponentType>(),
            };

            public override EntityArchetypeQuery[] CommandQueries => new EntityArchetypeQuery[]
            {
            };


            public ComponentReplicator(EntityManager entityManager, Unity.Entities.World world) : base(entityManager)
            {
                var bookkeepingSystem = world.GetOrCreateManager<CommandRequestTrackerSystem>();
            }

            public override void ExecuteReplication(ComponentGroup replicationGroup, ComponentSystemBase system, global::Improbable.Worker.CInterop.Connection connection)
            {
                Profiler.BeginSample("PlayerControls");

                var chunkArray = replicationGroup.CreateArchetypeChunkArray(Allocator.TempJob);
                var spatialOSEntityType = system.GetArchetypeChunkComponentType<SpatialEntityId>(true);
                var componentType = system.GetArchetypeChunkComponentType<Improbable.Player.PlayerControls.Component>();
                var eventMovementUpdateType = system.GetArchetypeChunkComponentType<EventSender.MovementUpdate>(true);
                foreach (var chunk in chunkArray)
                {
                    var entityIdArray = chunk.GetNativeArray(spatialOSEntityType);
                    var componentArray = chunk.GetNativeArray(componentType);
                    var eventMovementUpdateArray = chunk.GetNativeArray(eventMovementUpdateType);
                    for (var i = 0; i < componentArray.Length; i++)
                    {
                        var data = componentArray[i];
                        var eventsToSend = 0;
                        var eventsMovementUpdate = eventMovementUpdateArray[i].Events;
                        eventsToSend += eventsMovementUpdate.Count;

                        if (data.IsDataDirty() || eventsToSend > 0)
                        {
                            var update = new global::Improbable.Worker.CInterop.SchemaComponentUpdate(1001);
                            Improbable.Player.PlayerControls.Serialization.SerializeUpdate(data, update);

                            // Serialize events
                            var eventsObject = update.GetEvents();
                            if (eventsMovementUpdate.Count > 0)
                            {
                                foreach (var e in eventsMovementUpdate)
                                {
                                    var obj = eventsObject.AddObject(1);
                                    global::Improbable.Player.MovementUpdate.Serialization.Serialize(e, obj);
                                }

                                eventsMovementUpdate.Clear();
                            }

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
            }
        }

        internal class ComponentCleanup : ComponentCleanupHandler
        {
            public override EntityArchetypeQuery CleanupArchetypeQuery => new EntityArchetypeQuery
            {
                All = Array.Empty<ComponentType>(),
                Any = new ComponentType[]
                {
                    ComponentType.Create<ComponentAdded<Improbable.Player.PlayerControls.Component>>(),
                    ComponentType.Create<ComponentRemoved<Improbable.Player.PlayerControls.Component>>(),
                    ComponentType.Create<Improbable.Player.PlayerControls.ReceivedUpdates>(),
                    ComponentType.Create<AuthorityChanges<Improbable.Player.PlayerControls.Component>>(),
                    ComponentType.Create<ReceivedEvents.MovementUpdate>(),
                },
                None = Array.Empty<ComponentType>(),
            };

            public override void CleanComponents(ComponentGroup group, ComponentSystemBase system,
                EntityCommandBuffer buffer)
            {
                var entityType = system.GetArchetypeChunkEntityType();
                var componentAddedType = system.GetArchetypeChunkComponentType<ComponentAdded<Improbable.Player.PlayerControls.Component>>();
                var componentRemovedType = system.GetArchetypeChunkComponentType<ComponentRemoved<Improbable.Player.PlayerControls.Component>>();
                var receivedUpdateType = system.GetArchetypeChunkComponentType<Improbable.Player.PlayerControls.ReceivedUpdates>();
                var authorityChangeType = system.GetArchetypeChunkComponentType<AuthorityChanges<Improbable.Player.PlayerControls.Component>>();
                var movementUpdateEventType = system.GetArchetypeChunkComponentType<ReceivedEvents.MovementUpdate>();

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
                            buffer.RemoveComponent<Improbable.Player.PlayerControls.ReceivedUpdates>(entities[i]);
                            var updateList = updateArray[i].Updates;

                            // Pool update lists to avoid excessive allocation
                            updateList.Clear();
                            Improbable.Player.PlayerControls.Update.Pool.Push(updateList);

                            ReferenceTypeProviders.UpdatesProvider.Free(updateArray[i].handle);
                        }
                    }

                    // Component Added
                    if (chunk.Has(componentAddedType))
                    {
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<ComponentAdded<Improbable.Player.PlayerControls.Component>>(entities[i]);
                        }
                    }

                    // Component Removed
                    if (chunk.Has(componentRemovedType))
                    {
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<ComponentRemoved<Improbable.Player.PlayerControls.Component>>(entities[i]);
                        }
                    }

                    // Authority
                    if (chunk.Has(authorityChangeType))
                    {
                        var authorityChangeArray = chunk.GetNativeArray(authorityChangeType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<AuthorityChanges<Improbable.Player.PlayerControls.Component>>(entities[i]);
                            AuthorityChangesProvider.Free(authorityChangeArray[i].Handle);
                        }
                    }

                    // MovementUpdate Event
                    if (chunk.Has(movementUpdateEventType))
                    {
                        var movementUpdateEventArray = chunk.GetNativeArray(movementUpdateEventType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<ReceivedEvents.MovementUpdate>(entities[i]);
                            ReferenceTypeProviders.MovementUpdateProvider.Free(movementUpdateEventArray[i].handle);
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
                    ComponentType.ReadOnly<AuthorityLossImminent<Improbable.Player.PlayerControls.Component>>(),
                    ComponentType.ReadOnly<SpatialEntityId>()
                },
                Any = Array.Empty<ComponentType>(),
                None = Array.Empty<ComponentType>()
            };

            public override void AcknowledgeAuthorityLoss(ComponentGroup group, ComponentSystemBase system,
                Improbable.Worker.CInterop.Connection connection)
            {
                var authorityLossType = system.GetArchetypeChunkComponentType<AuthorityLossImminent<Improbable.Player.PlayerControls.Component>>();
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
                                1001);
                        }
                    }
                }

                chunkArray.Dispose();
            }
        }
    }
}
