
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
    public partial class Persistence
    {
        public partial class Requirable
        {
            [InjectableId(InjectableType.ReaderWriter, 55)]
            internal class ReaderWriterCreator : IInjectableCreator
            {
                public IInjectable CreateInjectable(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                {
                    return new ReaderWriterImpl(entity, entityManager, logDispatcher);
                }
            }

            [InjectableId(InjectableType.ReaderWriter, 55)]
            [InjectionCondition(InjectionCondition.RequireComponentPresent)]
            public interface Reader : IReader<Improbable.Persistence.Component, Improbable.Persistence.Update>
            {
                EntityId EntityId { get; }

            }

            [InjectableId(InjectableType.ReaderWriter, 55)]
            [InjectionCondition(InjectionCondition.RequireComponentWithAuthority)]
            public interface Writer : Reader, IWriter<Improbable.Persistence.Component, Improbable.Persistence.Update>
            {
            }

            internal class ReaderWriterImpl :
                ReaderWriterBase<Improbable.Persistence.Component, Improbable.Persistence.Update>, Reader, Writer
            {
                public new EntityId EntityId => base.EntityId;

                public ReaderWriterImpl(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                    : base(entity, entityManager, logDispatcher)
                {
                }

                protected override void TriggerFieldCallbacks(Improbable.Persistence.Update update)
                {
                }

                protected override void ApplyUpdate(Improbable.Persistence.Update update, ref Improbable.Persistence.Component data)
                {
                }
            }
        }
    }
}
