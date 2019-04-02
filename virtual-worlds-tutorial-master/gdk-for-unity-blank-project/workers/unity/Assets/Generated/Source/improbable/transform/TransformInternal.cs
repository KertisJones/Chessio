// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using Improbable.Gdk.Core;
using Improbable.Worker.CInterop;
using System;
using System.Collections.Generic;
using Unity.Entities;

namespace Improbable.Transform
{
    public partial class TransformInternal
    {
        public const uint ComponentId = 11000;

        public struct Component : IComponentData, ISpatialComponentData, ISnapshottable<Snapshot>
        {
            public uint ComponentId => 11000;

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
                if (propertyIndex < 0 || propertyIndex >= 5)
                {
                    throw new ArgumentException("\"propertyIndex\" argument out of range. Valid range is [0, 4]. " +
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
                if (propertyIndex < 0 || propertyIndex >= 5)
                {
                    throw new ArgumentException("\"propertyIndex\" argument out of range. Valid range is [0, 4]. " +
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
                var componentDataSchema = new ComponentData(new SchemaComponentData(11000));
                Serialization.SerializeComponent(this, componentDataSchema.SchemaData.Value.GetFields(), world);
                var snapshot = Serialization.DeserializeSnapshot(componentDataSchema.SchemaData.Value.GetFields(), world);

                componentDataSchema.SchemaData?.Destroy();
                componentDataSchema.SchemaData = null;

                return snapshot;
            }

            private global::Improbable.Transform.Location location;

            public global::Improbable.Transform.Location Location
            {
                get => location;
                set
                {
                    MarkDataDirty(0);
                    this.location = value;
                }
            }

            private global::Improbable.Transform.Quaternion rotation;

            public global::Improbable.Transform.Quaternion Rotation
            {
                get => rotation;
                set
                {
                    MarkDataDirty(1);
                    this.rotation = value;
                }
            }

            private global::Improbable.Transform.Velocity velocity;

            public global::Improbable.Transform.Velocity Velocity
            {
                get => velocity;
                set
                {
                    MarkDataDirty(2);
                    this.velocity = value;
                }
            }

            private uint physicsTick;

            public uint PhysicsTick
            {
                get => physicsTick;
                set
                {
                    MarkDataDirty(3);
                    this.physicsTick = value;
                }
            }

            private float ticksPerSecond;

            public float TicksPerSecond
            {
                get => ticksPerSecond;
                set
                {
                    MarkDataDirty(4);
                    this.ticksPerSecond = value;
                }
            }
        }

        public struct Snapshot : ISpatialComponentSnapshot
        {
            public uint ComponentId => 11000;

            public global::Improbable.Transform.Location Location;
            public global::Improbable.Transform.Quaternion Rotation;
            public global::Improbable.Transform.Velocity Velocity;
            public uint PhysicsTick;
            public float TicksPerSecond;
        }

        public static class Serialization
        {
            public static void SerializeComponent(Improbable.Transform.TransformInternal.Component component, global::Improbable.Worker.CInterop.SchemaObject obj, global::Unity.Entities.World world)
            {
                {
                    global::Improbable.Transform.Location.Serialization.Serialize(component.Location, obj.AddObject(1));
                }
                {
                    global::Improbable.Transform.Quaternion.Serialization.Serialize(component.Rotation, obj.AddObject(2));
                }
                {
                    global::Improbable.Transform.Velocity.Serialization.Serialize(component.Velocity, obj.AddObject(3));
                }
                {
                    obj.AddUint32(4, component.PhysicsTick);
                }
                {
                    obj.AddFloat(5, component.TicksPerSecond);
                }
            }

            public static void SerializeUpdate(Improbable.Transform.TransformInternal.Component component, global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj)
            {
                var obj = updateObj.GetFields();
                {
                    if (component.IsDataDirty(0))
                    {
                        global::Improbable.Transform.Location.Serialization.Serialize(component.Location, obj.AddObject(1));
                    }

                }
                {
                    if (component.IsDataDirty(1))
                    {
                        global::Improbable.Transform.Quaternion.Serialization.Serialize(component.Rotation, obj.AddObject(2));
                    }

                }
                {
                    if (component.IsDataDirty(2))
                    {
                        global::Improbable.Transform.Velocity.Serialization.Serialize(component.Velocity, obj.AddObject(3));
                    }

                }
                {
                    if (component.IsDataDirty(3))
                    {
                        obj.AddUint32(4, component.PhysicsTick);
                    }

                }
                {
                    if (component.IsDataDirty(4))
                    {
                        obj.AddFloat(5, component.TicksPerSecond);
                    }

                }
            }

            public static void SerializeSnapshot(Improbable.Transform.TransformInternal.Snapshot snapshot, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    global::Improbable.Transform.Location.Serialization.Serialize(snapshot.Location, obj.AddObject(1));
                }
                {
                    global::Improbable.Transform.Quaternion.Serialization.Serialize(snapshot.Rotation, obj.AddObject(2));
                }
                {
                    global::Improbable.Transform.Velocity.Serialization.Serialize(snapshot.Velocity, obj.AddObject(3));
                }
                {
                    obj.AddUint32(4, snapshot.PhysicsTick);
                }
                {
                    obj.AddFloat(5, snapshot.TicksPerSecond);
                }
            }

            public static Improbable.Transform.TransformInternal.Component Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj, global::Unity.Entities.World world)
            {
                var component = new Improbable.Transform.TransformInternal.Component();

                {
                    component.Location = global::Improbable.Transform.Location.Serialization.Deserialize(obj.GetObject(1));
                }
                {
                    component.Rotation = global::Improbable.Transform.Quaternion.Serialization.Deserialize(obj.GetObject(2));
                }
                {
                    component.Velocity = global::Improbable.Transform.Velocity.Serialization.Deserialize(obj.GetObject(3));
                }
                {
                    component.PhysicsTick = obj.GetUint32(4);
                }
                {
                    component.TicksPerSecond = obj.GetFloat(5);
                }
                return component;
            }

            public static Improbable.Transform.TransformInternal.Update DeserializeUpdate(global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj)
            {
                var update = new Improbable.Transform.TransformInternal.Update();
                var obj = updateObj.GetFields();

                {
                    if (obj.GetObjectCount(1) == 1)
                    {
                        var value = global::Improbable.Transform.Location.Serialization.Deserialize(obj.GetObject(1));
                        update.Location = new global::Improbable.Gdk.Core.Option<global::Improbable.Transform.Location>(value);
                    }
                    
                }
                {
                    if (obj.GetObjectCount(2) == 1)
                    {
                        var value = global::Improbable.Transform.Quaternion.Serialization.Deserialize(obj.GetObject(2));
                        update.Rotation = new global::Improbable.Gdk.Core.Option<global::Improbable.Transform.Quaternion>(value);
                    }
                    
                }
                {
                    if (obj.GetObjectCount(3) == 1)
                    {
                        var value = global::Improbable.Transform.Velocity.Serialization.Deserialize(obj.GetObject(3));
                        update.Velocity = new global::Improbable.Gdk.Core.Option<global::Improbable.Transform.Velocity>(value);
                    }
                    
                }
                {
                    if (obj.GetUint32Count(4) == 1)
                    {
                        var value = obj.GetUint32(4);
                        update.PhysicsTick = new global::Improbable.Gdk.Core.Option<uint>(value);
                    }
                    
                }
                {
                    if (obj.GetFloatCount(5) == 1)
                    {
                        var value = obj.GetFloat(5);
                        update.TicksPerSecond = new global::Improbable.Gdk.Core.Option<float>(value);
                    }
                    
                }
                return update;
            }

            public static Improbable.Transform.TransformInternal.Snapshot DeserializeSnapshot(global::Improbable.Worker.CInterop.SchemaObject obj, global::Unity.Entities.World world)
            {
                var component = new Improbable.Transform.TransformInternal.Snapshot();

                {
                    component.Location = global::Improbable.Transform.Location.Serialization.Deserialize(obj.GetObject(1));
                }

                {
                    component.Rotation = global::Improbable.Transform.Quaternion.Serialization.Deserialize(obj.GetObject(2));
                }

                {
                    component.Velocity = global::Improbable.Transform.Velocity.Serialization.Deserialize(obj.GetObject(3));
                }

                {
                    component.PhysicsTick = obj.GetUint32(4);
                }

                {
                    component.TicksPerSecond = obj.GetFloat(5);
                }

                return component;
            }

            public static void ApplyUpdate(global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj, ref Improbable.Transform.TransformInternal.Component component)
            {
                var obj = updateObj.GetFields();

                {
                    if (obj.GetObjectCount(1) == 1)
                    {
                        var value = global::Improbable.Transform.Location.Serialization.Deserialize(obj.GetObject(1));
                        component.Location = value;
                    }
                    
                }
                {
                    if (obj.GetObjectCount(2) == 1)
                    {
                        var value = global::Improbable.Transform.Quaternion.Serialization.Deserialize(obj.GetObject(2));
                        component.Rotation = value;
                    }
                    
                }
                {
                    if (obj.GetObjectCount(3) == 1)
                    {
                        var value = global::Improbable.Transform.Velocity.Serialization.Deserialize(obj.GetObject(3));
                        component.Velocity = value;
                    }
                    
                }
                {
                    if (obj.GetUint32Count(4) == 1)
                    {
                        var value = obj.GetUint32(4);
                        component.PhysicsTick = value;
                    }
                    
                }
                {
                    if (obj.GetFloatCount(5) == 1)
                    {
                        var value = obj.GetFloat(5);
                        component.TicksPerSecond = value;
                    }
                    
                }
            }
        }

        public struct Update : ISpatialComponentUpdate
        {
            internal static Stack<List<Update>> Pool = new Stack<List<Update>>();

            public Option<global::Improbable.Transform.Location> Location;
            public Option<global::Improbable.Transform.Quaternion> Rotation;
            public Option<global::Improbable.Transform.Velocity> Velocity;
            public Option<uint> PhysicsTick;
            public Option<float> TicksPerSecond;
        }

        public struct ReceivedUpdates : IComponentData
        {
            internal uint handle;
            public global::System.Collections.Generic.List<Update> Updates
            {
                get => Improbable.Transform.TransformInternal.ReferenceTypeProviders.UpdatesProvider.Get(handle);
            }
        }

        internal class TransformInternalDynamic : IDynamicInvokable
        {
            public uint ComponentId => TransformInternal.ComponentId;

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
                handler.Accept<Component, Update>(TransformInternal.ComponentId, DeserializeData, DeserializeUpdate);
            }

            public void InvokeSnapshotHandler(DynamicSnapshot.ISnapshotHandler handler)
            {
                handler.Accept<Snapshot>(TransformInternal.ComponentId, DeserializeSnapshot, SerializeSnapshot);
            }
        }
    }
}
