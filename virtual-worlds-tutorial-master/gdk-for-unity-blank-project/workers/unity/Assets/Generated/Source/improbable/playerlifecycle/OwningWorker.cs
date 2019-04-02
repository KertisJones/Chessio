// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using Improbable.Gdk.Core;
using Improbable.Worker.CInterop;
using System;
using System.Collections.Generic;
using Unity.Entities;

namespace Improbable.PlayerLifecycle
{
    public partial class OwningWorker
    {
        public const uint ComponentId = 13003;

        public struct Component : IComponentData, ISpatialComponentData, ISnapshottable<Snapshot>
        {
            public uint ComponentId => 13003;

            // Bit masks for tracking which component properties were changed locally and need to be synced.
            // Each byte tracks 8 component properties.
            private byte dirtyBits0;

            public bool IsDataDirty()
            {
                var isDataDirty = false;
                isDataDirty |= (dirtyBits0 != 0x0);
                return isDataDirty;
            }

            /*
            The propertyIndex argument counts up from 0 in the order defined in your schema component.
            It is not the schema field number itself. For example:
            component MyComponent
            {
                id = 1337;
                bool val_a = 1;
                bool val_b = 3;
            }
            In that case, val_a corresponds to propertyIndex 0 and val_b corresponds to propertyIndex 1 in this method.
            This method throws an InvalidOperationException in case your component doesn't contain properties.
            */
            public bool IsDataDirty(int propertyIndex)
            {
                if (propertyIndex < 0 || propertyIndex >= 1)
                {
                    throw new ArgumentException("\"propertyIndex\" argument out of range. Valid range is [0, 0]. " +
                        "Unless you are using custom component replication code, this is most likely caused by a code generation bug. " +
                        "Please contact SpatialOS support if you encounter this issue.");
                }

                // Retrieve the dirtyBits[0-n] field that tracks this property.
                var dirtyBitsByteIndex = propertyIndex / 8;
                switch (dirtyBitsByteIndex)
                {
                    case 0:
                        return (dirtyBits0 & (0x1 << propertyIndex % 8)) != 0x0;
                }

                return false;
            }

            // Like the IsDataDirty() method above, the propertyIndex arguments starts counting from 0.
            // This method throws an InvalidOperationException in case your component doesn't contain properties.
            public void MarkDataDirty(int propertyIndex)
            {
                if (propertyIndex < 0 || propertyIndex >= 1)
                {
                    throw new ArgumentException("\"propertyIndex\" argument out of range. Valid range is [0, 0]. " +
                        "Unless you are using custom component replication code, this is most likely caused by a code generation bug. " +
                        "Please contact SpatialOS support if you encounter this issue.");
                }

                // Retrieve the dirtyBits[0-n] field that tracks this property.
                var dirtyBitsByteIndex = propertyIndex / 8;
                switch (dirtyBitsByteIndex)
                {
                    case 0:
                        dirtyBits0 |= (byte) (0x1 << propertyIndex % 8);
                        break;
                }
            }

            public void MarkDataClean()
            {
                dirtyBits0 = 0x0;
            }

            public Snapshot ToComponentSnapshot(global::Unity.Entities.World world)
            {
                var componentDataSchema = new ComponentData(new SchemaComponentData(13003));
                Serialization.SerializeComponent(this, componentDataSchema.SchemaData.Value.GetFields(), world);
                var snapshot = Serialization.DeserializeSnapshot(componentDataSchema.SchemaData.Value.GetFields(), world);

                componentDataSchema.SchemaData?.Destroy();
                componentDataSchema.SchemaData = null;

                return snapshot;
            }

            internal uint workerIdHandle;

            public string WorkerId
            {
                get => Improbable.PlayerLifecycle.OwningWorker.ReferenceTypeProviders.WorkerIdProvider.Get(workerIdHandle);
                set
                {
                    MarkDataDirty(0);
                    Improbable.PlayerLifecycle.OwningWorker.ReferenceTypeProviders.WorkerIdProvider.Set(workerIdHandle, value);
                }
            }
        }

        public struct Snapshot : ISpatialComponentSnapshot
        {
            public uint ComponentId => 13003;

            public string WorkerId;
        }

        public static class Serialization
        {
            public static void SerializeComponent(Improbable.PlayerLifecycle.OwningWorker.Component component, global::Improbable.Worker.CInterop.SchemaObject obj, global::Unity.Entities.World world)
            {
                {
                    obj.AddString(1, component.WorkerId);
                }
            }

            public static void SerializeUpdate(Improbable.PlayerLifecycle.OwningWorker.Component component, global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj)
            {
                var obj = updateObj.GetFields();
                {
                    if (component.IsDataDirty(0))
                    {
                        obj.AddString(1, component.WorkerId);
                    }

                }
            }

            public static void SerializeSnapshot(Improbable.PlayerLifecycle.OwningWorker.Snapshot snapshot, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    obj.AddString(1, snapshot.WorkerId);
                }
            }

            public static Improbable.PlayerLifecycle.OwningWorker.Component Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj, global::Unity.Entities.World world)
            {
                var component = new Improbable.PlayerLifecycle.OwningWorker.Component();

                component.workerIdHandle = Improbable.PlayerLifecycle.OwningWorker.ReferenceTypeProviders.WorkerIdProvider.Allocate(world);
                {
                    component.WorkerId = obj.GetString(1);
                }
                return component;
            }

            public static Improbable.PlayerLifecycle.OwningWorker.Update DeserializeUpdate(global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj)
            {
                var update = new Improbable.PlayerLifecycle.OwningWorker.Update();
                var obj = updateObj.GetFields();

                {
                    if (obj.GetStringCount(1) == 1)
                    {
                        var value = obj.GetString(1);
                        update.WorkerId = new global::Improbable.Gdk.Core.Option<string>(value);
                    }
                    
                }
                return update;
            }

            public static Improbable.PlayerLifecycle.OwningWorker.Snapshot DeserializeSnapshot(global::Improbable.Worker.CInterop.SchemaObject obj, global::Unity.Entities.World world)
            {
                var component = new Improbable.PlayerLifecycle.OwningWorker.Snapshot();

                {
                    component.WorkerId = obj.GetString(1);
                }

                return component;
            }

            public static void ApplyUpdate(global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj, ref Improbable.PlayerLifecycle.OwningWorker.Component component)
            {
                var obj = updateObj.GetFields();

                {
                    if (obj.GetStringCount(1) == 1)
                    {
                        var value = obj.GetString(1);
                        component.WorkerId = value;
                    }
                    
                }
            }
        }

        public struct Update : ISpatialComponentUpdate
        {
            internal static Stack<List<Update>> Pool = new Stack<List<Update>>();

            public Option<string> WorkerId;
        }

        public struct ReceivedUpdates : IComponentData
        {
            internal uint handle;
            public global::System.Collections.Generic.List<Update> Updates
            {
                get => Improbable.PlayerLifecycle.OwningWorker.ReferenceTypeProviders.UpdatesProvider.Get(handle);
            }
        }

        internal class OwningWorkerDynamic : IDynamicInvokable
        {
            public uint ComponentId => OwningWorker.ComponentId;

            private static Component DeserializeData(ComponentData data, World world)
            {
                var schemaDataOpt = data.SchemaData;
                if (!schemaDataOpt.HasValue)
                {
                    throw new ArgumentException($"Can not deserialize an empty {nameof(ComponentData)}");
                }

                return Serialization.Deserialize(schemaDataOpt.Value.GetFields(), world);
            }

            private static Update DeserializeUpdate(ComponentUpdate update, World world)
            {
                var schemaDataOpt = update.SchemaData;
                if (!schemaDataOpt.HasValue)
                {
                    throw new ArgumentException($"Can not deserialize an empty {nameof(ComponentUpdate)}");
                }

                return Serialization.DeserializeUpdate(schemaDataOpt.Value);
            }

            private static Snapshot DeserializeSnapshot(ComponentData snapshot, World world)
            {
                var schemaDataOpt = snapshot.SchemaData;
                if (!schemaDataOpt.HasValue)
                {
                    throw new ArgumentException($"Can not deserialize an empty {nameof(ComponentData)}");
                }

                return Serialization.DeserializeSnapshot(schemaDataOpt.Value.GetFields(), world);
            }

            private static void SerializeSnapshot(Snapshot snapshot, ComponentData data)
            {
                var schemaDataOpt = data.SchemaData;
                if (!schemaDataOpt.HasValue)
                {
                    throw new ArgumentException($"Can not serialise an empty {nameof(ComponentData)}");
                }

                Serialization.SerializeSnapshot(snapshot, data.SchemaData.Value.GetFields());
            }

            public void InvokeHandler(Dynamic.IHandler handler)
            {
                handler.Accept<Component, Update>(OwningWorker.ComponentId, DeserializeData, DeserializeUpdate);
            }

            public void InvokeSnapshotHandler(DynamicSnapshot.ISnapshotHandler handler)
            {
                handler.Accept<Snapshot>(OwningWorker.ComponentId, DeserializeSnapshot, SerializeSnapshot);
            }
        }
    }
}
