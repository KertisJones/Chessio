
// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using System.Collections.Generic;
using Unity.Entities;
using Improbable.Gdk.Core;
using Improbable.Gdk.GameObjectRepresentation;
using Entity = Unity.Entities.Entity;

namespace Improbable
{
    public partial class Position
    {
        public partial class Requirable
        {
            [InjectableId(InjectableType.ReaderWriter, 54)]
            internal class ReaderWriterCreator : IInjectableCreator
            {
                public IInjectable CreateInjectable(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                {
                    return new ReaderWriterImpl(entity, entityManager, logDispatcher);
                }
            }

            [InjectableId(InjectableType.ReaderWriter, 54)]
            [InjectionCondition(InjectionCondition.RequireComponentPresent)]
            public interface Reader : IReader<Improbable.Position.Component, Improbable.Position.Update>
            {
                EntityId EntityId { get; }

                event Action<global::Improbable.Coordinates> CoordsUpdated;
            }

            [InjectableId(InjectableType.ReaderWriter, 54)]
            [InjectionCondition(InjectionCondition.RequireComponentWithAuthority)]
            public interface Writer : Reader, IWriter<Improbable.Position.Component, Improbable.Position.Update>
            {
            }

            internal class ReaderWriterImpl :
                ReaderWriterBase<Improbable.Position.Component, Improbable.Position.Update>, Reader, Writer
            {
                public new EntityId EntityId => base.EntityId;

                public ReaderWriterImpl(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                    : base(entity, entityManager, logDispatcher)
                {
                }

                private readonly List<Action<global::Improbable.Coordinates>> coordsDelegates = new List<Action<global::Improbable.Coordinates>>();

                public event Action<global::Improbable.Coordinates> CoordsUpdated
                {
                    add
                    {
                        if (!IsValid())
                        {
                            return;
                        }

                        coordsDelegates.Add(value);
                    }
                    remove
                    {
                        if (!IsValid())
                        {
                            return;
                        }

                        coordsDelegates.Remove(value);
                    }
                }

                protected override void TriggerFieldCallbacks(Improbable.Position.Update update)
                {
                    DispatchWithErrorHandling(update.Coords, coordsDelegates);
                }

                protected override void ApplyUpdate(Improbable.Position.Update update, ref Improbable.Position.Component data)
                {
                    if (update.Coords.HasValue)
                    {
                        data.Coords = update.Coords.Value;
                    }
                }
            }
        }
    }
}
