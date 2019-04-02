
// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using System.Collections.Generic;
using Unity.Entities;
using Improbable.Gdk.Core;
using Improbable.Gdk.GameObjectRepresentation;
using Entity = Unity.Entities.Entity;

namespace Improbable.Transform
{
    public partial class TransformInternal
    {
        public partial class Requirable
        {
            [InjectableId(InjectableType.ReaderWriter, 11000)]
            internal class ReaderWriterCreator : IInjectableCreator
            {
                public IInjectable CreateInjectable(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                {
                    return new ReaderWriterImpl(entity, entityManager, logDispatcher);
                }
            }

            [InjectableId(InjectableType.ReaderWriter, 11000)]
            [InjectionCondition(InjectionCondition.RequireComponentPresent)]
            public interface Reader : IReader<Improbable.Transform.TransformInternal.Component, Improbable.Transform.TransformInternal.Update>
            {
                EntityId EntityId { get; }

                event Action<global::Improbable.Transform.Location> LocationUpdated;
                event Action<global::Improbable.Transform.Quaternion> RotationUpdated;
                event Action<global::Improbable.Transform.Velocity> VelocityUpdated;
                event Action<uint> PhysicsTickUpdated;
                event Action<float> TicksPerSecondUpdated;
            }

            [InjectableId(InjectableType.ReaderWriter, 11000)]
            [InjectionCondition(InjectionCondition.RequireComponentWithAuthority)]
            public interface Writer : Reader, IWriter<Improbable.Transform.TransformInternal.Component, Improbable.Transform.TransformInternal.Update>
            {
            }

            internal class ReaderWriterImpl :
                ReaderWriterBase<Improbable.Transform.TransformInternal.Component, Improbable.Transform.TransformInternal.Update>, Reader, Writer
            {
                public new EntityId EntityId => base.EntityId;

                public ReaderWriterImpl(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                    : base(entity, entityManager, logDispatcher)
                {
                }

                private readonly List<Action<global::Improbable.Transform.Location>> locationDelegates = new List<Action<global::Improbable.Transform.Location>>();

                public event Action<global::Improbable.Transform.Location> LocationUpdated
                {
                    add
                    {
                        if (!IsValid())
                        {
                            return;
                        }

                        locationDelegates.Add(value);
                    }
                    remove
                    {
                        if (!IsValid())
                        {
                            return;
                        }

                        locationDelegates.Remove(value);
                    }
                }

                private readonly List<Action<global::Improbable.Transform.Quaternion>> rotationDelegates = new List<Action<global::Improbable.Transform.Quaternion>>();

                public event Action<global::Improbable.Transform.Quaternion> RotationUpdated
                {
                    add
                    {
                        if (!IsValid())
                        {
                            return;
                        }

                        rotationDelegates.Add(value);
                    }
                    remove
                    {
                        if (!IsValid())
                        {
                            return;
                        }

                        rotationDelegates.Remove(value);
                    }
                }

                private readonly List<Action<global::Improbable.Transform.Velocity>> velocityDelegates = new List<Action<global::Improbable.Transform.Velocity>>();

                public event Action<global::Improbable.Transform.Velocity> VelocityUpdated
                {
                    add
                    {
                        if (!IsValid())
                        {
                            return;
                        }

                        velocityDelegates.Add(value);
                    }
                    remove
                    {
                        if (!IsValid())
                        {
                            return;
                        }

                        velocityDelegates.Remove(value);
                    }
                }

                private readonly List<Action<uint>> physicsTickDelegates = new List<Action<uint>>();

                public event Action<uint> PhysicsTickUpdated
                {
                    add
                    {
                        if (!IsValid())
                        {
                            return;
                        }

                        physicsTickDelegates.Add(value);
                    }
                    remove
                    {
                        if (!IsValid())
                        {
                            return;
                        }

                        physicsTickDelegates.Remove(value);
                    }
                }

                private readonly List<Action<float>> ticksPerSecondDelegates = new List<Action<float>>();

                public event Action<float> TicksPerSecondUpdated
                {
                    add
                    {
                        if (!IsValid())
                        {
                            return;
                        }

                        ticksPerSecondDelegates.Add(value);
                    }
                    remove
                    {
                        if (!IsValid())
                        {
                            return;
                        }

                        ticksPerSecondDelegates.Remove(value);
                    }
                }

                protected override void TriggerFieldCallbacks(Improbable.Transform.TransformInternal.Update update)
                {
                    DispatchWithErrorHandling(update.Location, locationDelegates);
                    DispatchWithErrorHandling(update.Rotation, rotationDelegates);
                    DispatchWithErrorHandling(update.Velocity, velocityDelegates);
                    DispatchWithErrorHandling(update.PhysicsTick, physicsTickDelegates);
                    DispatchWithErrorHandling(update.TicksPerSecond, ticksPerSecondDelegates);
                }

                protected override void ApplyUpdate(Improbable.Transform.TransformInternal.Update update, ref Improbable.Transform.TransformInternal.Component data)
                {
                    if (update.Location.HasValue)
                    {
                        data.Location = update.Location.Value;
                    }
                    if (update.Rotation.HasValue)
                    {
                        data.Rotation = update.Rotation.Value;
                    }
                    if (update.Velocity.HasValue)
                    {
                        data.Velocity = update.Velocity.Value;
                    }
                    if (update.PhysicsTick.HasValue)
                    {
                        data.PhysicsTick = update.PhysicsTick.Value;
                    }
                    if (update.TicksPerSecond.HasValue)
                    {
                        data.TicksPerSecond = update.TicksPerSecond.Value;
                    }
                }
            }
        }
    }
}
