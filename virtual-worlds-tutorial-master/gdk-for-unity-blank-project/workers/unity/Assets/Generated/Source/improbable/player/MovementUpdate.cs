// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Improbable.Player
{
    
public struct MovementUpdate
{
    public float X;
    public float Y;

    public MovementUpdate(float x, float y)
    {
        X = x;
        Y = y;
    }
    public static class Serialization
    {
        public static void Serialize(MovementUpdate instance, global::Improbable.Worker.CInterop.SchemaObject obj)
        {
            {
                obj.AddFloat(1, instance.X);
            }
            {
                obj.AddFloat(2, instance.Y);
            }
        }

        public static MovementUpdate Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
        {
            var instance = new MovementUpdate();
            {
                instance.X = obj.GetFloat(1);
            }
            {
                instance.Y = obj.GetFloat(2);
            }
            return instance;
        }
    }
}

}
