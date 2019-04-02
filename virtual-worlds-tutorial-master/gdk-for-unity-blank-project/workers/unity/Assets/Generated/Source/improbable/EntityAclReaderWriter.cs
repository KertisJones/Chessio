
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
    public partial class EntityAcl
    {
        public partial class Requirable
        {
            [InjectableId(InjectableType.ReaderWriter, 50)]
            internal class ReaderWriterCreator : IInjectableCreator
            {
                public IInjectable CreateInjectable(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                {
                    return new ReaderWriterImpl(entity, entityManager, logDispatcher);
                }
            }

            [InjectableId(InjectableType.ReaderWriter, 50)]
            [InjectionCondition(InjectionCondition.RequireComponentPresent)]
            public interface Reader : IReader<Improbable.EntityAcl.Component, Improbable.EntityAcl.Update>
            {
                EntityId EntityId { get; }

                event Action<global::Improbable.WorkerRequirementSet> ReadAclUpdated;
                event Action<global::System.Collections.Generic.Dictionary<uint,global::Improbable.WorkerRequirementSet>> ComponentWriteAclUpdated;
            }

            [InjectableId(InjectableType.ReaderWriter, 50)]
            [InjectionCondition(InjectionCondition.RequireComponentWithAuthority)]
            public interface Writer : Reader, IWriter<Improbable.EntityAcl.Component, Improbable.EntityAcl.Update>
            {
            }

            internal class ReaderWriterImpl :
                ReaderWriterBase<Improbable.EntityAcl.Component, Improbable.EntityAcl.Update>, Reader, Writer
            {
                public new EntityId EntityId => base.EntityId;

                public ReaderWriterImpl(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                    : base(entity, entityManager, logDispatcher)
                {
                }

                private readonly List<Action<global::Improbable.WorkerRequirementSet>> readAclDelegates = new List<Action<global::Improbable.WorkerRequirementSet>>();

                public event Action<global::Improbable.WorkerRequirementSet> ReadAclUpdated
                {
                    add
                    {
                        if (!IsValid())
                        {
                            return;
                        }

                        readAclDelegates.Add(value);
                    }
                    remove
                    {
                        if (!IsValid())
                        {
                            return;
                        }

                        readAclDelegates.Remove(value);
                    }
                }

                private readonly List<Action<global::System.Collections.Generic.Dictionary<uint,global::Improbable.WorkerRequirementSet>>> componentWriteAclDelegates = new List<Action<global::System.Collections.Generic.Dictionary<uint,global::Improbable.WorkerRequirementSet>>>();

                public event Action<global::System.Collections.Generic.Dictionary<uint,global::Improbable.WorkerRequirementSet>> ComponentWriteAclUpdated
                {
                    add
                    {
                        if (!IsValid())
                        {
                            return;
                        }

                        componentWriteAclDelegates.Add(value);
                    }
                    remove
                    {
                        if (!IsValid())
                        {
                            return;
                        }

                        componentWriteAclDelegates.Remove(value);
                    }
                }

                protected override void TriggerFieldCallbacks(Improbable.EntityAcl.Update update)
                {
                    DispatchWithErrorHandling(update.ReadAcl, readAclDelegates);
                    DispatchWithErrorHandling(update.ComponentWriteAcl, componentWriteAclDelegates);
                }

                protected override void ApplyUpdate(Improbable.EntityAcl.Update update, ref Improbable.EntityAcl.Component data)
                {
                    if (update.ReadAcl.HasValue)
                    {
                        data.ReadAcl = update.ReadAcl.Value;
                    }
                    if (update.ComponentWriteAcl.HasValue)
                    {
                        data.ComponentWriteAcl = update.ComponentWriteAcl.Value;
                    }
                }
            }
        }
    }
}
