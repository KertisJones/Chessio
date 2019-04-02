// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Improbable.PlayerLifecycle
{
    
public struct CreatePlayerResponseType
{
    public global::Improbable.Gdk.Core.EntityId CreatedEntityId;

    public CreatePlayerResponseType(global::Improbable.Gdk.Core.EntityId createdEntityId)
    {
        CreatedEntityId = createdEntityId;
    }
    public static class Serialization
    {
        public static void Serialize(CreatePlayerResponseType instance, global::Improbable.Worker.CInterop.SchemaObject obj)
        {
            {
                obj.AddEntityId(3, instance.CreatedEntityId);
            }
        }

        public static CreatePlayerResponseType Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
        {
            var instance = new CreatePlayerResponseType();
            {
                instance.CreatedEntityId = obj.GetEntityIdStruct(3);
            }
            return instance;
        }
    }
}

}
