
// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using System.Collections.Generic;
using Unity.Entities;
using Improbable.Gdk.Core;
using Improbable.Gdk.GameObjectRepresentation;
using Entity = Unity.Entities.Entity;

namespace Improbable.PlayerLifecycle
{
    public partial class PlayerCreator
    {
        public partial class Requirable
        {
            [InjectableId(InjectableType.ReaderWriter, 13000)]
            internal class ReaderWriterCreator : IInjectableCreator
            {
                public IInjectable CreateInjectable(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                {
                    return new ReaderWriterImpl(entity, entityManager, logDispatcher);
                }
            }

            [InjectableId(InjectableType.ReaderWriter, 13000)]
            [InjectionCondition(InjectionCondition.RequireComponentPresent)]
            public interface Reader : IReader<Improbable.PlayerLifecycle.PlayerCreator.Component, Improbable.PlayerLifecycle.PlayerCreator.Update>
            {
                EntityId EntityId { get; }

            }

            [InjectableId(InjectableType.ReaderWriter, 13000)]
            [InjectionCondition(InjectionCondition.RequireComponentWithAuthority)]
            public interface Writer : Reader, IWriter<Improbable.PlayerLifecycle.PlayerCreator.Component, Improbable.PlayerLifecycle.PlayerCreator.Update>
            {
            }

            internal class ReaderWriterImpl :
                ReaderWriterBase<Improbable.PlayerLifecycle.PlayerCreator.Component, Improbable.PlayerLifecycle.PlayerCreator.Update>, Reader, Writer
            {
                public new EntityId EntityId => base.EntityId;

                public ReaderWriterImpl(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                    : base(entity, entityManager, logDispatcher)
                {
                }

                protected override void TriggerFieldCallbacks(Improbable.PlayerLifecycle.PlayerCreator.Update update)
                {
                }

                protected override void ApplyUpdate(Improbable.PlayerLifecycle.PlayerCreator.Update update, ref Improbable.PlayerLifecycle.PlayerCreator.Component data)
                {
                }
            }
        }
    }
}
