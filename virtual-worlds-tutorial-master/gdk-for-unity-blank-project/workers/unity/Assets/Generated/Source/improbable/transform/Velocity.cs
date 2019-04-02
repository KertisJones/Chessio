// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Improbable.Transform
{
    
public struct Velocity
{
    public float X;
    public float Y;
    public float Z;

    public Velocity(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }
    public static class Serialization
    {
        public static void Serialize(Velocity instance, global::Improbable.Worker.CInterop.SchemaObject obj)
        {
            {
                obj.AddFloat(1, instance.X);
            }
            {
                obj.AddFloat(2, instance.Y);
            }
            {
                obj.AddFloat(3, instance.Z);
            }
        }

        public static Velocity Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
        {
            var instance = new Velocity();
            {
                instance.X = obj.GetFloat(1);
            }
            {
                instance.Y = obj.GetFloat(2);
            }
            {
                instance.Z = obj.GetFloat(3);
            }
            return instance;
        }
    }
}

}
