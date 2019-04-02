// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Improbable.PlayerLifecycle
{
    
public struct CreatePlayerRequestType
{
    public global::Improbable.Vector3f Position;

    public CreatePlayerRequestType(global::Improbable.Vector3f position)
    {
        Position = position;
    }
    public static class Serialization
    {
        public static void Serialize(CreatePlayerRequestType instance, global::Improbable.Worker.CInterop.SchemaObject obj)
        {
            {
                global::Improbable.Vector3f.Serialization.Serialize(instance.Position, obj.AddObject(1));
            }
        }

        public static CreatePlayerRequestType Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
        {
            var instance = new CreatePlayerRequestType();
            {
                instance.Position = global::Improbable.Vector3f.Serialization.Deserialize(obj.GetObject(1));
            }
            return instance;
        }
    }
}

}
