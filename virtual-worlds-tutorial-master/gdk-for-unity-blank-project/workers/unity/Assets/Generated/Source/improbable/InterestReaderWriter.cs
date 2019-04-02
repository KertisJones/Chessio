
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
    public partial class Interest
    {
        public partial class Requirable
        {
            [InjectableId(InjectableType.ReaderWriter, 58)]
            internal class ReaderWriterCreator : IInjectableCreator
            {
                public IInjectable CreateInjectable(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                {
                    return new ReaderWriterImpl(entity, entityManager, logDispatcher);
                }
            }

            [InjectableId(InjectableType.ReaderWriter, 58)]
            [InjectionCondition(InjectionCondition.RequireComponentPresent)]
            public interface Reader : IReader<Improbable.Interest.Component, Improbable.Interest.Update>
            {
                EntityId EntityId { get; }

                event Action<global::System.Collections.Generic.Dictionary<uint,global::Improbable.ComponentInterest>> ComponentInterestUpdated;
            }

            [InjectableId(InjectableType.ReaderWriter, 58)]
            [InjectionCondition(InjectionCondition.RequireComponentWithAuthority)]
            public interface Writer : Reader, IWriter<Improbable.Interest.Component, Improbable.Interest.Update>
            {
            }

            internal class ReaderWriterImpl :
                ReaderWriterBase<Improbable.Interest.Component, Improbable.Interest.Update>, Reader, Writer
            {
                public new EntityId EntityId => base.EntityId;

                public ReaderWriterImpl(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                    : base(entity, entityManager, logDispatcher)
                {
                }

                private readonly List<Action<global::System.Collections.Generic.Dictionary<uint,global::Improbable.ComponentInterest>>> componentInterestDelegates = new List<Action<global::System.Collections.Generic.Dictionary<uint,global::Improbable.ComponentInterest>>>();

                public event Action<global::System.Collections.Generic.Dictionary<uint,global::Improbable.ComponentInterest>> ComponentInterestUpdated
                {
                    add
                    {
                        if (!IsValid())
                        {
                            return;
                        }

                        componentInterestDelegates.Add(value);
                    }
                    remove
                    {
                        if (!IsValid())
                        {
                            return;
                        }

                        componentInterestDelegates.Remove(value);
                    }
                }

                protected override void TriggerFieldCallbacks(Improbable.Interest.Update update)
                {
                    DispatchWithErrorHandling(update.ComponentInterest, componentInterestDelegates);
                }

                protected override void ApplyUpdate(Improbable.Interest.Update update, ref Improbable.Interest.Component data)
                {
                    if (update.ComponentInterest.HasValue)
                    {
                        data.ComponentInterest = update.ComponentInterest.Value;
                    }
                }
            }
        }
    }
}
