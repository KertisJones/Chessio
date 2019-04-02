// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using System.Collections.Generic;
using Improbable.Gdk.Core.Commands;

namespace Improbable.PlayerLifecycle
{
    public partial class PlayerCreator
    {
        internal class CommandStorages
        {
            public class CreatePlayer : CommandStorage
            {
                public Dictionary<long, CommandRequestStore<global::Improbable.PlayerLifecycle.CreatePlayerRequestType>> CommandRequestsInFlight =
                    new Dictionary<long, CommandRequestStore<global::Improbable.PlayerLifecycle.CreatePlayerRequestType>>();
            }
        }
    }
}
