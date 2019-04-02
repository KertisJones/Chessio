// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using System.Collections.Generic;
using Improbable.Gdk.Core.Commands;

namespace Improbable.PlayerLifecycle
{
    public partial class PlayerHeartbeatClient
    {
        internal class CommandStorages
        {
            public class PlayerHeartbeat : CommandStorage
            {
                public Dictionary<long, CommandRequestStore<global::Improbable.Common.Empty>> CommandRequestsInFlight =
                    new Dictionary<long, CommandRequestStore<global::Improbable.Common.Empty>>();
            }
        }
    }
}
