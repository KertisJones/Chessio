
// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using System.Collections.Generic;
using Improbable;
using Improbable.Gdk.Core;
using Improbable.Gdk.GameObjectRepresentation;
using Improbable.Worker.CInterop;
using Unity.Entities;
using UnityEngine;
using Entity = Unity.Entities.Entity;

namespace Improbable.PlayerLifecycle
{
    public partial class PlayerHeartbeatClient
    {
        public partial class PlayerHeartbeat
        {
            public struct RequestResponder
            {
                private readonly EntityManager entityManager;
                private readonly Entity entity;
                public PlayerHeartbeat.ReceivedRequest Request { get; }

                internal RequestResponder(EntityManager entityManager, Entity entity, PlayerHeartbeat.ReceivedRequest request)
                {
                    this.entity = entity;
                    this.entityManager = entityManager;
                    Request = request;
                }

                public void SendResponse(global::Improbable.Common.Empty payload)
                {
                    entityManager.GetComponentData<CommandResponders.PlayerHeartbeat>(entity).ResponsesToSend
                        .Add(PlayerHeartbeat.CreateResponse(Request, payload));
                }

                public void SendResponseFailure(string message)
                {
                    entityManager.GetComponentData<CommandResponders.PlayerHeartbeat>(entity).ResponsesToSend
                        .Add(PlayerHeartbeat.CreateResponseFailure(Request, message));
                }
            }
        }

        public partial class Requirable
        {
            [InjectableId(InjectableType.CommandRequestSender, 13001)]
            internal class CommandRequestSenderCreator : IInjectableCreator
            {
                public IInjectable CreateInjectable(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                {
                    return new CommandRequestSender(entity, entityManager, logDispatcher);
                }
            }

            [InjectableId(InjectableType.CommandRequestSender, 13001)]
            [InjectionCondition(InjectionCondition.RequireNothing)]
            public class CommandRequestSender : RequirableBase
            {
                private Entity entity;
                private readonly EntityManager entityManager;
                private readonly ILogDispatcher logger;

                public CommandRequestSender(Entity entity, EntityManager entityManager, ILogDispatcher logger) : base(logger)
                {
                    this.entity = entity;
                    this.entityManager = entityManager;
                    this.logger = logger;
                }

                public long SendPlayerHeartbeatRequest(EntityId entityId, global::Improbable.Common.Empty payload,
                    uint? timeoutMillis = null, bool allowShortCircuiting = false, object context = null)
                {
                    if (!IsValid())
                    {
                        return -1;
                    }

                    var ecsCommandRequestSender = entityManager.GetComponentData<CommandSenders.PlayerHeartbeat>(entity);
                    var request = PlayerHeartbeat.CreateRequest(entityId, payload, timeoutMillis, allowShortCircuiting, context);
                    ecsCommandRequestSender.RequestsToSend.Add(request);
                    return request.RequestId;
                }

                public long SendPlayerHeartbeatRequest(EntityId entityId, global::Improbable.Common.Empty payload,
                    Action<PlayerHeartbeat.ReceivedResponse> callback, uint? timeoutMillis = null, bool allowShortCircuiting = false)
                {
                    if (!IsValid())
                    {
                        return -1;
                    }

                    var ecsCommandRequestSender = entityManager.GetComponentData<CommandSenders.PlayerHeartbeat>(entity);
                    var request = PlayerHeartbeat.CreateRequest(entityId, payload, timeoutMillis, allowShortCircuiting, callback);
                    ecsCommandRequestSender.RequestsToSend.Add(request);
                    return request.RequestId;
                }

            }

            [InjectableId(InjectableType.CommandRequestHandler, 13001)]
            internal class CommandRequestHandlerCreator : IInjectableCreator
            {
                public IInjectable CreateInjectable(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                {
                    return new CommandRequestHandler(entity, entityManager, logDispatcher);
                }
            }

            [InjectableId(InjectableType.CommandRequestHandler, 13001)]
            [InjectionCondition(InjectionCondition.RequireComponentWithAuthority)]
            public class CommandRequestHandler : RequirableBase
            {
                private Entity entity;
                private readonly EntityManager entityManager;
                private readonly ILogDispatcher logger;

                public CommandRequestHandler(Entity entity, EntityManager entityManager, ILogDispatcher logger) : base(logger)
                {
                    this.entity = entity;
                    this.entityManager = entityManager;
                    this.logger = logger;
                }
                private readonly List<Action<PlayerHeartbeat.RequestResponder>> playerHeartbeatDelegates = new List<Action<PlayerHeartbeat.RequestResponder>>();
                public event Action<PlayerHeartbeat.RequestResponder> OnPlayerHeartbeatRequest
                {
                    add
                    {
                        if (!IsValid())
                        {
                            return;
                        }

                        playerHeartbeatDelegates.Add(value);
                    }
                    remove
                    {
                        if (!IsValid())
                        {
                            return;
                        }

                        playerHeartbeatDelegates.Remove(value);
                    }
                }

                internal void OnPlayerHeartbeatRequestInternal(PlayerHeartbeat.ReceivedRequest request)
                {
                    GameObjectDelegates.DispatchWithErrorHandling(new PlayerHeartbeat.RequestResponder(entityManager, entity, request), playerHeartbeatDelegates, logger);
                }
            }

            [InjectableId(InjectableType.CommandResponseHandler, 13001)]
            internal class CommandResponseHandlerCreator : IInjectableCreator
            {
                public IInjectable CreateInjectable(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                {
                    return new CommandResponseHandler(entity, entityManager, logDispatcher);
                }
            }

            [InjectableId(InjectableType.CommandResponseHandler, 13001)]
            [InjectionCondition(InjectionCondition.RequireNothing)]
            public class CommandResponseHandler : RequirableBase
            {
                private Entity entity;
                private readonly EntityManager entityManager;
                private readonly ILogDispatcher logger;

                public CommandResponseHandler(Entity entity, EntityManager entityManager, ILogDispatcher logger) : base(logger)
                {
                    this.entity = entity;
                    this.entityManager = entityManager;
                    this.logger = logger;
                }

                private readonly List<Action<PlayerHeartbeat.ReceivedResponse>> playerHeartbeatDelegates = new List<Action<PlayerHeartbeat.ReceivedResponse>>();
                public event Action<PlayerHeartbeat.ReceivedResponse> OnPlayerHeartbeatResponse
                {
                    add
                    {
                        if (!IsValid())
                        {
                            return;
                        }

                        playerHeartbeatDelegates.Add(value);
                    }
                    remove
                    {
                        if (!IsValid())
                        {
                            return;
                        }

                        playerHeartbeatDelegates.Remove(value);
                    }
                }

                internal void OnPlayerHeartbeatResponseInternal(PlayerHeartbeat.ReceivedResponse response)
                {
                    GameObjectDelegates.DispatchWithErrorHandling(response, playerHeartbeatDelegates, logger);
                }
            }
        }
    }
}
