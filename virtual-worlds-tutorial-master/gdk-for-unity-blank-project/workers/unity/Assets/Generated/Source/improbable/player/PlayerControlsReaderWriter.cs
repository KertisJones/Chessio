
// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using System.Collections.Generic;
using Unity.Entities;
using Improbable.Gdk.Core;
using Improbable.Gdk.GameObjectRepresentation;
using Entity = Unity.Entities.Entity;

namespace Improbable.Player
{
    public partial class PlayerControls
    {
        public partial class Requirable
        {
            [InjectableId(InjectableType.ReaderWriter, 1001)]
            internal class ReaderWriterCreator : IInjectableCreator
            {
                public IInjectable CreateInjectable(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                {
                    return new ReaderWriterImpl(entity, entityManager, logDispatcher);
                }
            }

            [InjectableId(InjectableType.ReaderWriter, 1001)]
            [InjectionCondition(InjectionCondition.RequireComponentPresent)]
            public interface Reader : IReader<Improbable.Player.PlayerControls.Component, Improbable.Player.PlayerControls.Update>
            {
                EntityId EntityId { get; }

                event Action<global::Improbable.Player.MovementUpdate> OnMovementUpdate;
            }

            [InjectableId(InjectableType.ReaderWriter, 1001)]
            [InjectionCondition(InjectionCondition.RequireComponentWithAuthority)]
            public interface Writer : Reader, IWriter<Improbable.Player.PlayerControls.Component, Improbable.Player.PlayerControls.Update>
            {
                void SendMovementUpdate( global::Improbable.Player.MovementUpdate payload);
            }

            internal class ReaderWriterImpl :
                ReaderWriterBase<Improbable.Player.PlayerControls.Component, Improbable.Player.PlayerControls.Update>, Reader, Writer
            {
                public new EntityId EntityId => base.EntityId;

                public ReaderWriterImpl(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                    : base(entity, entityManager, logDispatcher)
                {
                }

                protected override void TriggerFieldCallbacks(Improbable.Player.PlayerControls.Update update)
                {
                }

                protected override void ApplyUpdate(Improbable.Player.PlayerControls.Update update, ref Improbable.Player.PlayerControls.Component data)
                {
                }

                private readonly List<Action<global::Improbable.Player.MovementUpdate>> MovementUpdateDelegates = new List<Action<global::Improbable.Player.MovementUpdate>>();

                public event Action<global::Improbable.Player.MovementUpdate> OnMovementUpdate
                {
                    add
                    {
                        if (!IsValid())
                        {
                            return;
                        }

                        MovementUpdateDelegates.Add(value);
                    }
                    remove
                    {
                        if (!IsValid())
                        {
                            return;
                        }

                        MovementUpdateDelegates.Remove(value);
                    }
                }

                public void OnMovementUpdateEvent(global::Improbable.Player.MovementUpdate payload)
                {
                    GameObjectDelegates.DispatchWithErrorHandling(payload, MovementUpdateDelegates, LogDispatcher);
                }

                public void SendMovementUpdate(global::Improbable.Player.MovementUpdate payload)
                {
                    if (!IsValid())
                    {
                        return;
                    }

                    var sender = EntityManager.GetComponentData<EventSender.MovementUpdate>(Entity);
                    sender.Events.Add(payload);
                }
            }
        }
    }
}
