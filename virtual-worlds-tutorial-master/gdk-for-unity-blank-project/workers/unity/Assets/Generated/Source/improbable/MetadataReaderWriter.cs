
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
    public partial class Metadata
    {
        public partial class Requirable
        {
            [InjectableId(InjectableType.ReaderWriter, 53)]
            internal class ReaderWriterCreator : IInjectableCreator
            {
                public IInjectable CreateInjectable(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                {
                    return new ReaderWriterImpl(entity, entityManager, logDispatcher);
                }
            }

            [InjectableId(InjectableType.ReaderWriter, 53)]
            [InjectionCondition(InjectionCondition.RequireComponentPresent)]
            public interface Reader : IReader<Improbable.Metadata.Component, Improbable.Metadata.Update>
            {
                EntityId EntityId { get; }

                event Action<string> EntityTypeUpdated;
            }

            [InjectableId(InjectableType.ReaderWriter, 53)]
            [InjectionCondition(InjectionCondition.RequireComponentWithAuthority)]
            public interface Writer : Reader, IWriter<Improbable.Metadata.Component, Improbable.Metadata.Update>
            {
            }

            internal class ReaderWriterImpl :
                ReaderWriterBase<Improbable.Metadata.Component, Improbable.Metadata.Update>, Reader, Writer
            {
                public new EntityId EntityId => base.EntityId;

                public ReaderWriterImpl(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                    : base(entity, entityManager, logDispatcher)
                {
                }

                private readonly List<Action<string>> entityTypeDelegates = new List<Action<string>>();

                public event Action<string> EntityTypeUpdated
                {
                    add
                    {
                        if (!IsValid())
                        {
                            return;
                        }

                        entityTypeDelegates.Add(value);
                    }
                    remove
                    {
                        if (!IsValid())
                        {
                            return;
                        }

                        entityTypeDelegates.Remove(value);
                    }
                }

                protected override void TriggerFieldCallbacks(Improbable.Metadata.Update update)
                {
                    DispatchWithErrorHandling(update.EntityType, entityTypeDelegates);
                }

                protected override void ApplyUpdate(Improbable.Metadata.Update update, ref Improbable.Metadata.Component data)
                {
                    if (update.EntityType.HasValue)
                    {
                        data.EntityType = update.EntityType.Value;
                    }
                }
            }
        }
    }
}
