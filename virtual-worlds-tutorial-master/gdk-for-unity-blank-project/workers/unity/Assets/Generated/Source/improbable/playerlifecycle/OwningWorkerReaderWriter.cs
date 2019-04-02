
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
    public partial class OwningWorker
    {
        public partial class Requirable
        {
            [InjectableId(InjectableType.ReaderWriter, 13003)]
            internal class ReaderWriterCreator : IInjectableCreator
            {
                public IInjectable CreateInjectable(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                {
                    return new ReaderWriterImpl(entity, entityManager, logDispatcher);
                }
            }

            [InjectableId(InjectableType.ReaderWriter, 13003)]
            [InjectionCondition(InjectionCondition.RequireComponentPresent)]
            public interface Reader : IReader<Improbable.PlayerLifecycle.OwningWorker.Component, Improbable.PlayerLifecycle.OwningWorker.Update>
            {
                EntityId EntityId { get; }

                event Action<string> WorkerIdUpdated;
            }

            [InjectableId(InjectableType.ReaderWriter, 13003)]
            [InjectionCondition(InjectionCondition.RequireComponentWithAuthority)]
            public interface Writer : Reader, IWriter<Improbable.PlayerLifecycle.OwningWorker.Component, Improbable.PlayerLifecycle.OwningWorker.Update>
            {
            }

            internal class ReaderWriterImpl :
                ReaderWriterBase<Improbable.PlayerLifecycle.OwningWorker.Component, Improbable.PlayerLifecycle.OwningWorker.Update>, Reader, Writer
            {
                public new EntityId EntityId => base.EntityId;

                public ReaderWriterImpl(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                    : base(entity, entityManager, logDispatcher)
                {
                }

                private readonly List<Action<string>> workerIdDelegates = new List<Action<string>>();

                public event Action<string> WorkerIdUpdated
                {
                    add
                    {
                        if (!IsValid())
                        {
                            return;
                        }

                        workerIdDelegates.Add(value);
                    }
                    remove
                    {
                        if (!IsValid())
                        {
                            return;
                        }

                        workerIdDelegates.Remove(value);
                    }
                }

                protected override void TriggerFieldCallbacks(Improbable.PlayerLifecycle.OwningWorker.Update update)
                {
                    DispatchWithErrorHandling(update.WorkerId, workerIdDelegates);
                }

                protected override void ApplyUpdate(Improbable.PlayerLifecycle.OwningWorker.Update update, ref Improbable.PlayerLifecycle.OwningWorker.Component data)
                {
                    if (update.WorkerId.HasValue)
                    {
                        data.WorkerId = update.WorkerId.Value;
                    }
                }
            }
        }
    }
}
