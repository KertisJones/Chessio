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

namespace Improbable.Transform
{
    public partial class TransformInternal
    {
        internal class DispatcherHandler : ComponentDispatcherHandler
        {
            public override uint ComponentId => 11000;

            private readonly EntityManager entityManager;

            private const string LoggerName = "TransformInternal.DispatcherHandler";


            public DispatcherHandler(WorkerSystem worker, World world) : base(worker, world)
            {
                entityManager = world.GetOrCreateManager<EntityManager>();
                var bookkeepingSystem = world.GetOrCreateManager<CommandRequestTrackerSystem>();
            }

            public override void Dispose()
            {
                TransformInternal.ReferenceTypeProviders.UpdatesProvider.CleanDataInWorld(World);
            }

            public override void OnAddComponent(AddComponentOp op)
            {
                var entity = TryGetEntityFromEntityId(new EntityId(op.EntityId));

                Profiler.BeginSample("TransformInternal");
                var data = Improbable.Transform.TransformInternal.Serialization.Deserialize(op.Data.SchemaData.Value.GetFields(), World);
                data.MarkDataClean();
                entityManager.AddComponentData(entity, data);
                entityManager.AddComponent(entity, ComponentType.Create<NotAuthoritative<Improbable.Transform.TransformInternal.Component>>());

                var update = new Improbable.Transform.TransformInternal.Update
                {
                    Location = data.Location,
                    Rotation = data.Rotation,
                    Velocity = data.Velocity,
                    PhysicsTick = data.PhysicsTick,
                    TicksPerSecond = data.TicksPerSecond,
                };

                var updates = new List<Improbable.Transform.TransformInternal.Update>
                {
                    update
                };

                var updatesComponent = new Improbable.Transform.TransformInternal.ReceivedUpdates
                {
                    handle = ReferenceTypeProviders.UpdatesProvider.Allocate(World)
                };

                ReferenceTypeProviders.UpdatesProvider.Set(updatesComponent.handle, updates);
                entityManager.AddComponentData(entity, updatesComponent);

                if (entityManager.HasComponent<ComponentRemoved<Improbable.Transform.TransformInternal.Component>>(entity))
                {
                    entityManager.RemoveComponent<ComponentRemoved<Improbable.Transform.TransformInternal.Component>>(entity);
                }
                else if (!entityManager.HasComponent<ComponentAdded<Improbable.Transform.TransformInternal.Component>>(entity))
                {
                    entityManager.AddComponent(entity, ComponentType.Create<ComponentAdded<Improbable.Transform.TransformInternal.Component>>());
                }
                else
                {
                    LogDispatcher.HandleLog(LogType.Error, new LogEvent(ReceivedDuplicateComponentAdded)
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField(LoggingUtils.EntityId, op.EntityId)
                        .WithField("Component", "Improbable.Transform.TransformInternal")
                    );
                }

                Profiler.EndSample();
            }

            public override void OnRemoveComponent(RemoveComponentOp op)
            {
                var entity = TryGetEntityFromEntityId(new EntityId(op.EntityId));

                Profiler.BeginSample("TransformInternal");

                entityManager.RemoveComponent<Improbable.Transform.TransformInternal.Component>(entity);

                if (entityManager.HasComponent<ComponentAdded<Improbable.Transform.TransformInternal.Component>>(entity))
                {
                    entityManager.RemoveComponent<ComponentAdded<Improbable.Transform.TransformInternal.Component>>(entity);
                }
                else if (!entityManager.HasComponent<ComponentRemoved<Improbable.Transform.TransformInternal.Component>>(entity))
                {
                    entityManager.AddComponent(entity, ComponentType.Create<ComponentRemoved<Improbable.Transform.TransformInternal.Component>>());
                }
                else
                {
                    LogDispatcher.HandleLog(LogType.Error, new LogEvent(ReceivedDuplicateComponentRemoved)
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField(LoggingUtils.EntityId, op.EntityId)
                        .WithField("Component", "Improbable.Transform.TransformInternal")
                    );
                }

                Profiler.EndSample();
            }

            public override void OnComponentUpdate(ComponentUpdateOp op)
            {
                var entity = TryGetEntityFromEntityId(new EntityId(op.EntityId));

                Profiler.BeginSample("TransformInternal");
                if (entityManager.HasComponent<NotAuthoritative<Improbable.Transform.TransformInternal.Component>>(entity))
                {
                    var data = entityManager.GetComponentData<Improbable.Transform.TransformInternal.Component>(entity);
                    Improbable.Transform.TransformInternal.Serialization.ApplyUpdate(op.Update.SchemaData.Value, ref data);
                    data.MarkDataClean();
                    entityManager.SetComponentData(entity, data);
                }

                var update = Improbable.Transform.TransformInternal.Serialization.DeserializeUpdate(op.Update.SchemaData.Value);

                List<Improbable.Transform.TransformInternal.Update> updates;
                if (entityManager.HasComponent<Improbable.Transform.TransformInternal.ReceivedUpdates>(entity))
                {
                    updates = entityManager.GetComponentData<Improbable.Transform.TransformInternal.ReceivedUpdates>(entity).Updates;
                }
                else
                {
                    updates = Improbable.Transform.TransformInternal.Update.Pool.Count > 0 ? Improbable.Transform.TransformInternal.Update.Pool.Pop() : new List<Improbable.Transform.TransformInternal.Update>();
                    var updatesComponent = new Improbable.Transform.TransformInternal.ReceivedUpdates
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

                Profiler.BeginSample("TransformInternal");
                ApplyAuthorityChange(entity, op.Authority, entityId);
                Profiler.EndSample();
            }

            public override void OnCommandRequest(CommandRequestOp op)
            {
                var commandIndex = op.Request.SchemaData.Value.GetCommandIndex();
                throw new UnknownCommandIndexException(commandIndex, "TransformInternal");
            }

            public override void OnCommandResponse(CommandResponseOp op)
            {
                var commandIndex = op.CommandIndex;
                throw new UnknownCommandIndexException(commandIndex, "TransformInternal");
            }

            public override void AddCommandComponents(Unity.Entities.Entity entity)
            {
            }

            private void ApplyAuthorityChange(Unity.Entities.Entity entity, Authority authority, global::Improbable.Gdk.Core.EntityId entityId)
            {
                switch (authority)
                {
                    case Authority.Authoritative:
                        if (!entityManager.HasComponent<NotAuthoritative<Improbable.Transform.TransformInternal.Component>>(entity))
                        {
                            LogInvalidAuthorityTransition(Authority.Authoritative, Authority.NotAuthoritative, entityId);
                            return;
                        }

                        entityManager.RemoveComponent<NotAuthoritative<Improbable.Transform.TransformInternal.Component>>(entity);
                        entityManager.AddComponent(entity, ComponentType.Create<Authoritative<Improbable.Transform.TransformInternal.Component>>());

                        // Add event senders
                        break;
                    case Authority.AuthorityLossImminent:
                        if (!entityManager.HasComponent<Authoritative<Improbable.Transform.TransformInternal.Component>>(entity))
                        {
                            LogInvalidAuthorityTransition(Authority.AuthorityLossImminent, Authority.Authoritative, entityId);
                            return;
                        }

                        entityManager.AddComponent(entity, ComponentType.Create<AuthorityLossImminent<Improbable.Transform.TransformInternal.Component>>());
                        break;
                    case Authority.NotAuthoritative:
                        if (!entityManager.HasComponent<Authoritative<Improbable.Transform.TransformInternal.Component>>(entity))
                        {
                            LogInvalidAuthorityTransition(Authority.NotAuthoritative, Authority.Authoritative, entityId);
                            return;
                        }

                        if (entityManager.HasComponent<AuthorityLossImminent<Improbable.Transform.TransformInternal.Component>>(entity))
                        {
                            entityManager.RemoveComponent<AuthorityLossImminent<Improbable.Transform.TransformInternal.Component>>(entity);
                        }

                        entityManager.RemoveComponent<Authoritative<Improbable.Transform.TransformInternal.Component>>(entity);
                        entityManager.AddComponent(entity, ComponentType.Create<NotAuthoritative<Improbable.Transform.TransformInternal.Component>>());

                        // Remove event senders
                        break;
                }

                List<Authority> authorityChanges;
                if (entityManager.HasComponent<AuthorityChanges<Improbable.Transform.TransformInternal.Component>>(entity))
                {
                    authorityChanges = entityManager.GetComponentData<AuthorityChanges<Improbable.Transform.TransformInternal.Component>>(entity).Changes;

                }
                else
                {
                    var changes = new AuthorityChanges<Improbable.Transform.TransformInternal.Component>
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
                    .WithField("Component", "Improbable.Transform.TransformInternal")
                );
            }
        }

        internal class ComponentReplicator : ComponentReplicationHandler
        {
            public override uint ComponentId => 11000;

            public override EntityArchetypeQuery ComponentUpdateQuery => new EntityArchetypeQuery
            {
                All = new[]
                {
                    ComponentType.Create<Improbable.Transform.TransformInternal.Component>(),
                    ComponentType.ReadOnly<Authoritative<Improbable.Transform.TransformInternal.Component>>(),
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
                Profiler.BeginSample("TransformInternal");

                var chunkArray = replicationGroup.CreateArchetypeChunkArray(Allocator.TempJob);
                var spatialOSEntityType = system.GetArchetypeChunkComponentType<SpatialEntityId>(true);
                var componentType = system.GetArchetypeChunkComponentType<Improbable.Transform.TransformInternal.Component>();
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
                            var update = new global::Improbable.Worker.CInterop.SchemaComponentUpdate(11000);
                            Improbable.Transform.TransformInternal.Serialization.SerializeUpdate(data, update);

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
                    ComponentType.Create<ComponentAdded<Improbable.Transform.TransformInternal.Component>>(),
                    ComponentType.Create<ComponentRemoved<Improbable.Transform.TransformInternal.Component>>(),
                    ComponentType.Create<Improbable.Transform.TransformInternal.ReceivedUpdates>(),
                    ComponentType.Create<AuthorityChanges<Improbable.Transform.TransformInternal.Component>>(),
                },
                None = Array.Empty<ComponentType>(),
            };

            public override void CleanComponents(ComponentGroup group, ComponentSystemBase system,
                EntityCommandBuffer buffer)
            {
                var entityType = system.GetArchetypeChunkEntityType();
                var componentAddedType = system.GetArchetypeChunkComponentType<ComponentAdded<Improbable.Transform.TransformInternal.Component>>();
                var componentRemovedType = system.GetArchetypeChunkComponentType<ComponentRemoved<Improbable.Transform.TransformInternal.Component>>();
                var receivedUpdateType = system.GetArchetypeChunkComponentType<Improbable.Transform.TransformInternal.ReceivedUpdates>();
                var authorityChangeType = system.GetArchetypeChunkComponentType<AuthorityChanges<Improbable.Transform.TransformInternal.Component>>();

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
                            buffer.RemoveComponent<Improbable.Transform.TransformInternal.ReceivedUpdates>(entities[i]);
                            var updateList = updateArray[i].Updates;

                            // Pool update lists to avoid excessive allocation
                            updateList.Clear();
                            Improbable.Transform.TransformInternal.Update.Pool.Push(updateList);

                            ReferenceTypeProviders.UpdatesProvider.Free(updateArray[i].handle);
                        }
                    }

                    // Component Added
                    if (chunk.Has(componentAddedType))
                    {
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<ComponentAdded<Improbable.Transform.TransformInternal.Component>>(entities[i]);
                        }
                    }

                    // Component Removed
                    if (chunk.Has(componentRemovedType))
                    {
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<ComponentRemoved<Improbable.Transform.TransformInternal.Component>>(entities[i]);
                        }
                    }

                    // Authority
                    if (chunk.Has(authorityChangeType))
                    {
                        var authorityChangeArray = chunk.GetNativeArray(authorityChangeType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<AuthorityChanges<Improbable.Transform.TransformInternal.Component>>(entities[i]);
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
                    ComponentType.ReadOnly<AuthorityLossImminent<Improbable.Transform.TransformInternal.Component>>(),
                    ComponentType.ReadOnly<SpatialEntityId>()
                },
                Any = Array.Empty<ComponentType>(),
                None = Array.Empty<ComponentType>()
            };

            public override void AcknowledgeAuthorityLoss(ComponentGroup group, ComponentSystemBase system,
                Improbable.Worker.CInterop.Connection connection)
            {
                var authorityLossType = system.GetArchetypeChunkComponentType<AuthorityLossImminent<Improbable.Transform.TransformInternal.Component>>();
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
                                11000);
                        }
                    }
                }

                chunkArray.Dispose();
            }
        }
    }
}
