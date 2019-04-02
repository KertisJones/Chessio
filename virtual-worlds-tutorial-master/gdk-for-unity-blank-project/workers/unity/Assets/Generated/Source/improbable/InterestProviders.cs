// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core;

namespace Improbable
{
    public partial class Interest
    {
        internal static class ReferenceTypeProviders
        {
            public static class UpdatesProvider 
{
    private static readonly Dictionary<uint, List<Improbable.Interest.Update>> Storage = new Dictionary<uint, List<Improbable.Interest.Update>>();
    private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();

    private static uint nextHandle = 0;

    public static uint Allocate(global::Unity.Entities.World world)
    {
        var handle = GetNextHandle();

        Storage.Add(handle, default(List<Improbable.Interest.Update>));
        WorldMapping.Add(handle, world);

        return handle;
    }

    public static List<Improbable.Interest.Update> Get(uint handle)
    {
        if (!Storage.TryGetValue(handle, out var value))
        {
            throw new ArgumentException($"UpdatesProvider does not contain handle {handle}");
        }

        return value;
    }

    public static void Set(uint handle, List<Improbable.Interest.Update> value)
    {
        if (!Storage.ContainsKey(handle))
        {
            throw new ArgumentException($"UpdatesProvider does not contain handle {handle}");
        }

        Storage[handle] = value;
    }

    public static void Free(uint handle)
    {
        Storage.Remove(handle);
        WorldMapping.Remove(handle);
    }

    public static void CleanDataInWorld(global::Unity.Entities.World world)
    {
        var handles = WorldMapping.Where(pair => pair.Value == world).Select(pair => pair.Key).ToList();

        foreach (var handle in handles)
        {
            Free(handle);
        }
    }

    private static uint GetNextHandle() 
    {
        nextHandle++;
        
        while (Storage.ContainsKey(nextHandle))
        {
            nextHandle++;
        }

        return nextHandle;
    }
}


            public static class ComponentInterestProvider 
{
    private static readonly Dictionary<uint, global::System.Collections.Generic.Dictionary<uint,global::Improbable.ComponentInterest>> Storage = new Dictionary<uint, global::System.Collections.Generic.Dictionary<uint,global::Improbable.ComponentInterest>>();
    private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();

    private static uint nextHandle = 0;

    public static uint Allocate(global::Unity.Entities.World world)
    {
        var handle = GetNextHandle();

        Storage.Add(handle, default(global::System.Collections.Generic.Dictionary<uint,global::Improbable.ComponentInterest>));
        WorldMapping.Add(handle, world);

        return handle;
    }

    public static global::System.Collections.Generic.Dictionary<uint,global::Improbable.ComponentInterest> Get(uint handle)
    {
        if (!Storage.TryGetValue(handle, out var value))
        {
            throw new ArgumentException($"ComponentInterestProvider does not contain handle {handle}");
        }

        return value;
    }

    public static void Set(uint handle, global::System.Collections.Generic.Dictionary<uint,global::Improbable.ComponentInterest> value)
    {
        if (!Storage.ContainsKey(handle))
        {
            throw new ArgumentException($"ComponentInterestProvider does not contain handle {handle}");
        }

        Storage[handle] = value;
    }

    public static void Free(uint handle)
    {
        Storage.Remove(handle);
        WorldMapping.Remove(handle);
    }

    public static void CleanDataInWorld(global::Unity.Entities.World world)
    {
        var handles = WorldMapping.Where(pair => pair.Value == world).Select(pair => pair.Key).ToList();

        foreach (var handle in handles)
        {
            Free(handle);
        }
    }

    private static uint GetNextHandle() 
    {
        nextHandle++;
        
        while (Storage.ContainsKey(nextHandle))
        {
            nextHandle++;
        }

        return nextHandle;
    }
}


        }
    }
}
