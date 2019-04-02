// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Improbable.Transform
{
    
public struct Quaternion
{
    public float W;
    public float X;
    public float Y;
    public float Z;

    public Quaternion(float w, float x, float y, float z)
    {
        W = w;
        X = x;
        Y = y;
        Z = z;
    }
    public static class Serialization
    {
        public static void Serialize(Quaternion instance, global::Improbable.Worker.CInterop.SchemaObject obj)
        {
            {
                obj.AddFloat(1, instance.W);
            }
            {
                obj.AddFloat(2, instance.X);
            }
            {
                obj.AddFloat(3, instance.Y);
            }
            {
                obj.AddFloat(4, instance.Z);
            }
        }

        public static Quaternion Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
        {
            var instance = new Quaternion();
            {
                instance.W = obj.GetFloat(1);
            }
            {
                instance.X = obj.GetFloat(2);
            }
            {
                instance.Y = obj.GetFloat(3);
            }
            {
                instance.Z = obj.GetFloat(4);
            }
            return instance;
        }
    }
}

}
