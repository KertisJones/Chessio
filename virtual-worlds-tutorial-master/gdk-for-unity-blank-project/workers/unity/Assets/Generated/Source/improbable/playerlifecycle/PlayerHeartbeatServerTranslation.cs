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
    public partial class PlayerHeartbeatServer
    {
        internal class DispatcherHandler : ComponentDispatcherHandler
        {
            public override uint ComponentId => 13002;

            private readonly EntityManager entityManager;

            private const string LoggerName = "PlayerHeartbeatServer.DispatcherHandler";


            public DispatcherHandler(WorkerSystem worker, World world) : base(worker, world)
            {
                entityManager = world.GetOrCreateManager<EntityManager>();
                var bookkeepingSystem = world.GetOrCreateManager<CommandRequestTrackerSystem>();
            }

            public override void Dispose()
            {
                PlayerHeartbeatServer.ReferenceTypeProviders.UpdatesProvider.CleanDataInWorld(World);
            }

            public override void OnAddComponent(AddComponentOp op)
            {
                var entity = TryGetEntityFromEntityId(new EntityId(op.EntityId));

                Profiler.BeginSample("PlayerHeartbeatServer");
                var data = Improbable.PlayerLifecycle.PlayerHeartbeatServer.Serialization.Deserialize(op.Data.SchemaData.Value.GetFields(), World);
                data.MarkDataClean();
                entityManager.AddComponentData(entity, data);
                entityManager.AddComponent(entity, ComponentType.Create<NotAuthoritative<Improbable.PlayerLifecycle.PlayerHeartbeatServer.Component>>());

                var update = new Improbable.PlayerLifecycle.PlayerHeartbeatServer.Update
                {
                };

                var updates = new List<Improbable.PlayerLifecycle.PlayerHeartbeatServer.Update>
                {
                    update
                };

                var updatesComponent = new Improbable.PlayerLifecycle.PlayerHeartbeatServer.ReceivedUpdates
                {
                    handle = ReferenceTypeProviders.UpdatesProvider.Allocate(World)
                };

                ReferenceTypeProviders.UpdatesProvider.Set(updatesComponent.handle, updates);
                entityManager.AddComponentData(entity, updatesComponent);

                if (entityManager.HasComponent<ComponentRemoved<Improbable.PlayerLifecycle.PlayerHeartbeatServer.Component>>(entity))
                {
                    entityManager.RemoveComponent<ComponentRemoved<Improbable.PlayerLifecycle.PlayerHeartbeatServer.Component>>(entity);
                }
                else if (!entityManager.HasComponent<ComponentAdded<Improbable.PlayerLifecycle.PlayerHeartbeatServer.Component>>(entity))
                {
                    entityManager.AddComponent(entity, ComponentType.Create<ComponentAdded<Improbable.PlayerLifecycle.PlayerHeartbeatServer.Component>>());
                }
                else
                {
                    LogDispatcher.HandleLog(LogType.Error, new LogEvent(ReceivedDuplicateComponentAdded)
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField(LoggingUtils.EntityId, op.EntityId)
                        .WithField("Component", "Improbable.PlayerLifecycle.PlayerHeartbeatServer")
                    );
                }

                Profiler.EndSample();
            }

            public override void OnRemoveComponent(RemoveComponentOp op)
            {
                var entity = TryGetEntityFromEntityId(new EntityId(op.EntityId));

                Profiler.BeginSample("PlayerHeartbeatServer");

                entityManager.RemoveComponent<Improbable.PlayerLifecycle.PlayerHeartbeatServer.Component>(entity);

                if (entityManager.HasComponent<ComponentAdded<Improbable.PlayerLifecycle.PlayerHeartbeatServer.Component>>(entity))
                {
                    entityManager.RemoveComponent<ComponentAdded<Improbable.PlayerLifecycle.PlayerHeartbeatServer.Component>>(entity);
                }
                else if (!entityManager.HasComponent<ComponentRemoved<Improbable.PlayerLifecycle.PlayerHeartbeatServer.Component>>(entity))
                {
                    entityManager.AddComponent(entity, ComponentType.Create<ComponentRemoved<Improbable.PlayerLifecycle.PlayerHeartbeatServer.Component>>());
                }
                else
                {
                    LogDispatcher.HandleLog(LogType.Error, new LogEvent(ReceivedDuplicateComponentRemoved)
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField(LoggingUtils.EntityId, op.EntityId)
                        .WithField("Component", "Improbable.PlayerLifecycle.PlayerHeartbeatServer")
                    );
                }

                Profiler.EndSample();
            }

            public override void OnComponentUpdate(ComponentUpdateOp op)
            {
                var entity = TryGetEntityFromEntityId(new EntityId(op.EntityId));

                Profiler.BeginSample("PlayerHeartbeatServer");
                if (entityManager.HasComponent<NotAuthoritative<Improbable.PlayerLifecycle.PlayerHeartbeatServer.Component>>(entity))
                {
                    var data = entityManager.GetComponentData<Improbable.PlayerLifecycle.PlayerHeartbeatServer.Component>(entity);
                    Improbable.PlayerLifecycle.PlayerHeartbeatServer.Serialization.ApplyUpdate(op.Update.SchemaData.Value, ref data);
                    data.MarkDataClean();
                    entityManager.SetComponentData(entity, data);
                }

                var update = Improbable.PlayerLifecycle.PlayerHeartbeatServer.Serialization.DeserializeUpdate(op.Update.SchemaData.Value);

                List<Improbable.PlayerLifecycle.PlayerHeartbeatServer.Update> updates;
                if (entityManager.HasComponent<Improbable.PlayerLifecycle.PlayerHeartbeatServer.ReceivedUpdates>(entity))
                {
                    updates = entityManager.GetComponentData<Improbable.PlayerLifecycle.PlayerHeartbeatServer.ReceivedUpdates>(entity).Updates;
                }
                else
                {
                    updates = Improbable.PlayerLifecycle.PlayerHeartbeatServer.Update.Pool.Count > 0 ? Improbable.PlayerLifecycle.PlayerHeartbeatServer.Update.Pool.Pop() : new List<Improbable.PlayerLifecycle.PlayerHeartbeatServer.Update>();
                    var updatesComponent = new Improbable.PlayerLifecycle.PlayerHeartbeatServer.ReceivedUpdates
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

                Profiler.BeginSample("PlayerHeartbeatServer");
                ApplyAuthorityChange(entity, op.Authority, entityId);
                Profiler.EndSample();
            }

            public override void OnCommandRequest(CommandRequestOp op)
            {
                var commandIndex = op.Request.SchemaData.Value.GetCommandIndex();
                throw new UnknownCommandIndexException(commandIndex, "PlayerHeartbeatServer");
            }

            public override void OnCommandResponse(CommandResponseOp op)
            {
                var commandIndex = op.CommandIndex;
                throw new UnknownCommandIndexException(commandIndex, "PlayerHeartbeatServer");
            }

            public override void AddCommandComponents(Unity.Entities.Entity entity)
            {
            }

            private void ApplyAuthorityChange(Unity.Entities.Entity entity, Authority authority, global::Improbable.Gdk.Core.EntityId entityId)
            {
                switch (authority)
                {
                    case Authority.Authoritative:
                        if (!entityManager.HasComponent<NotAuthoritative<Improbable.PlayerLifecycle.PlayerHeartbeatServer.Component>>(entity))
                        {
                            LogInvalidAuthorityTransition(Authority.Authoritative, Authority.NotAuthoritative, entityId);
                            return;
                        }

                        entityManager.RemoveComponent<NotAuthoritative<Improbable.PlayerLifecycle.PlayerHeartbeatServer.Component>>(entity);
                        entityManager.AddComponent(entity, ComponentType.Create<Authoritative<Improbable.PlayerLifecycle.PlayerHeartbeatServer.Component>>());

                        // Add event senders
                        break;
                    case Authority.AuthorityLossImminent:
                        if (!entityManager.HasComponent<Authoritative<Improbable.PlayerLifecycle.PlayerHeartbeatServer.Component>>(entity))
                        {
                            LogInvalidAuthorityTransition(Authority.AuthorityLossImminent, Authority.Authoritative, entityId);
                            return;
                        }

                        entityManager.AddComponent(entity, ComponentType.Create<AuthorityLossImminent<Improbable.PlayerLifecycle.PlayerHeartbeatServer.Component>>());
                        break;
                    case Authority.NotAuthoritative:
                        if (!entityManager.HasComponent<Authoritative<Improbable.PlayerLifecycle.PlayerHeartbeatServer.Component>>(entity))
                        {
                            LogInvalidAuthorityTransition(Authority.NotAuthoritative, Authority.Authoritative, entityId);
                            return;
                        }

                        if (entityManager.HasComponent<AuthorityLossImminent<Improbable.PlayerLifecycle.PlayerHeartbeatServer.Component>>(entity))
                        {
                            entityManager.RemoveComponent<AuthorityLossImminent<Improbable.PlayerLifecycle.PlayerHeartbeatServer.Component>>(entity);
                        }

                        entityManager.RemoveComponent<Authoritative<Improbable.PlayerLifecycle.PlayerHeartbeatServer.Component>>(entity);
                        entityManager.AddComponent(entity, ComponentType.Create<NotAuthoritative<Improbable.PlayerLifecycle.PlayerHeartbeatServer.Component>>());

                        // Remove event senders
                        break;
                }

                List<Authority> authorityChanges;
                if (entityManager.HasComponent<AuthorityChanges<Improbable.PlayerLifecycle.PlayerHeartbeatServer.Component>>(entity))
                {
                    authorityChanges = entityManager.GetComponentData<AuthorityChanges<Improbable.PlayerLifecycle.PlayerHeartbeatServer.Component>>(entity).Changes;

                }
                else
                {
                    var changes = new AuthorityChanges<Improbable.PlayerLifecycle.PlayerHeartbeatServer.Component>
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
                    .WithField("Component", "Improbable.PlayerLifecycle.PlayerHeartbeatServer")
                );
            }
        }

        internal class ComponentReplicator : ComponentReplicationHandler
        {
            public override uint ComponentId => 13002;

            public override EntityArchetypeQuery ComponentUpdateQuery => new EntityArchetypeQuery
            {
                All = new[]
                {
                    ComponentType.Create<Improbable.PlayerLifecycle.PlayerHeartbeatServer.Component>(),
                    ComponentType.ReadOnly<Authoritative<Improbable.PlayerLifecycle.PlayerHeartbeatServer.Component>>(),
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
                Profiler.BeginSample("PlayerHeartbeatServer");

                var chunkArray = replicationGroup.CreateArchetypeChunkArray(Allocator.TempJob);
                var spatialOSEntityType = system.GetArchetypeChunkComponentType<SpatialEntityId>(true);
                var componentType = system.GetArchetypeChunkComponentType<Improbable.PlayerLifecycle.PlayerHeartbeatServer.Component>();
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
                            var update = new global::Improbable.Worker.CInterop.SchemaComponentUpdate(13002);
                            Improbable.PlayerLifecycle.PlayerHeartbeatServer.Serialization.SerializeUpdate(data, update);

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
                    ComponentType.Create<ComponentAdded<Improbable.PlayerLifecycle.PlayerHeartbeatServer.Component>>(),
                    ComponentType.Create<ComponentRemoved<Improbable.PlayerLifecycle.PlayerHeartbeatServer.Component>>(),
                    ComponentType.Create<Improbable.PlayerLifecycle.PlayerHeartbeatServer.ReceivedUpdates>(),
                    ComponentType.Create<AuthorityChanges<Improbable.PlayerLifecycle.PlayerHeartbeatServer.Component>>(),
                },
                None = Array.Empty<ComponentType>(),
            };

            public override void CleanComponents(ComponentGroup group, ComponentSystemBase system,
                EntityCommandBuffer buffer)
            {
                var entityType = system.GetArchetypeChunkEntityType();
                var componentAddedType = system.GetArchetypeChunkComponentType<ComponentAdded<Improbable.PlayerLifecycle.PlayerHeartbeatServer.Component>>();
                var componentRemovedType = system.GetArchetypeChunkComponentType<ComponentRemoved<Improbable.PlayerLifecycle.PlayerHeartbeatServer.Component>>();
                var receivedUpdateType = system.GetArchetypeChunkComponentType<Improbable.PlayerLifecycle.PlayerHeartbeatServer.ReceivedUpdates>();
                var authorityChangeType = system.GetArchetypeChunkComponentType<AuthorityChanges<Improbable.PlayerLifecycle.PlayerHeartbeatServer.Component>>();

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
                            buffer.RemoveComponent<Improbable.PlayerLifecycle.PlayerHeartbeatServer.ReceivedUpdates>(entities[i]);
                            var updateList = updateArray[i].Updates;

                            // Pool update lists to avoid excessive allocation
                            updateList.Clear();
                            Improbable.PlayerLifecycle.PlayerHeartbeatServer.Update.Pool.Push(updateList);

                            ReferenceTypeProviders.UpdatesProvider.Free(updateArray[i].handle);
                        }
                    }

                    // Component Added
                    if (chunk.Has(componentAddedType))
                    {
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<ComponentAdded<Improbable.PlayerLifecycle.PlayerHeartbeatServer.Component>>(entities[i]);
                        }
                    }

                    // Component Removed
                    if (chunk.Has(componentRemovedType))
                    {
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<ComponentRemoved<Improbable.PlayerLifecycle.PlayerHeartbeatServer.Component>>(entities[i]);
                        }
                    }

                    // Authority
                    if (chunk.Has(authorityChangeType))
                    {
                        var authorityChangeArray = chunk.GetNativeArray(authorityChangeType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<AuthorityChanges<Improbable.PlayerLifecycle.PlayerHeartbeatServer.Component>>(entities[i]);
                            AuthorityChangesProvider.Free(authorityChangeArray[i].Handle);
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
                    ComponentType.ReadOnly<AuthorityLossImminent<Improbable.PlayerLifecycle.PlayerHeartbeatServer.Component>>(),
                    ComponentType.ReadOnly<SpatialEntityId>()
                },
                Any = Array.Empty<ComponentType>(),
                None = Array.Empty<ComponentType>()
            };

            public override void AcknowledgeAuthorityLoss(ComponentGroup group, ComponentSystemBase system,
                Improbable.Worker.CInterop.Connection connection)
            {
                var authorityLossType = system.GetArchetypeChunkComponentType<AuthorityLossImminent<Improbable.PlayerLifecycle.PlayerHeartbeatServer.Component>>();
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
                                13002);
                        }
                    }
                }

                chunkArray.Dispose();
            }
        }
    }
}
