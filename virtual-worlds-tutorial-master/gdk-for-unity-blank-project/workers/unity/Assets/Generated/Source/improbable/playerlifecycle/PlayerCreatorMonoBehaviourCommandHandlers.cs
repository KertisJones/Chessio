
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
    public partial class PlayerCreator
    {
        public partial class CreatePlayer
        {
            public struct RequestResponder
            {
                private readonly EntityManager entityManager;
                private readonly Entity entity;
                public CreatePlayer.ReceivedRequest Request { get; }

                internal RequestResponder(EntityManager entityManager, Entity entity, CreatePlayer.ReceivedRequest request)
                {
                    this.entity = entity;
                    this.entityManager = entityManager;
                    Request = request;
                }

                public void SendResponse(global::Improbable.PlayerLifecycle.CreatePlayerResponseType payload)
                {
                    entityManager.GetComponentData<CommandResponders.CreatePlayer>(entity).ResponsesToSend
                        .Add(CreatePlayer.CreateResponse(Request, payload));
                }

                public void SendResponseFailure(string message)
                {
                    entityManager.GetComponentData<CommandResponders.CreatePlayer>(entity).ResponsesToSend
                        .Add(CreatePlayer.CreateResponseFailure(Request, message));
                }
            }
        }

        public partial class Requirable
        {
            [InjectableId(InjectableType.CommandRequestSender, 13000)]
            internal class CommandRequestSenderCreator : IInjectableCreator
            {
                public IInjectable CreateInjectable(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                {
                    return new CommandRequestSender(entity, entityManager, logDispatcher);
                }
            }

            [InjectableId(InjectableType.CommandRequestSender, 13000)]
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

                public long SendCreatePlayerRequest(EntityId entityId, global::Improbable.PlayerLifecycle.CreatePlayerRequestType payload,
                    uint? timeoutMillis = null, bool allowShortCircuiting = false, object context = null)
                {
                    if (!IsValid())
                    {
                        return -1;
                    }

                    var ecsCommandRequestSender = entityManager.GetComponentData<CommandSenders.CreatePlayer>(entity);
                    var request = CreatePlayer.CreateRequest(entityId, payload, timeoutMillis, allowShortCircuiting, context);
                    ecsCommandRequestSender.RequestsToSend.Add(request);
                    return request.RequestId;
                }

                public long SendCreatePlayerRequest(EntityId entityId, global::Improbable.PlayerLifecycle.CreatePlayerRequestType payload,
                    Action<CreatePlayer.ReceivedResponse> callback, uint? timeoutMillis = null, bool allowShortCircuiting = false)
                {
                    if (!IsValid())
                    {
                        return -1;
                    }

                    var ecsCommandRequestSender = entityManager.GetComponentData<CommandSenders.CreatePlayer>(entity);
                    var request = CreatePlayer.CreateRequest(entityId, payload, timeoutMillis, allowShortCircuiting, callback);
                    ecsCommandRequestSender.RequestsToSend.Add(request);
                    return request.RequestId;
                }

            }

            [InjectableId(InjectableType.CommandRequestHandler, 13000)]
            internal class CommandRequestHandlerCreator : IInjectableCreator
            {
                public IInjectable CreateInjectable(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                {
                    return new CommandRequestHandler(entity, entityManager, logDispatcher);
                }
            }

            [InjectableId(InjectableType.CommandRequestHandler, 13000)]
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
                private readonly List<Action<CreatePlayer.RequestResponder>> createPlayerDelegates = new List<Action<CreatePlayer.RequestResponder>>();
                public event Action<CreatePlayer.RequestResponder> OnCreatePlayerRequest
                {
                    add
                    {
                        if (!IsValid())
                        {
                            return;
                        }

                        createPlayerDelegates.Add(value);
                    }
                    remove
                    {
                        if (!IsValid())
                        {
                            return;
                        }

                        createPlayerDelegates.Remove(value);
                    }
                }

                internal void OnCreatePlayerRequestInternal(CreatePlayer.ReceivedRequest request)
                {
                    GameObjectDelegates.DispatchWithErrorHandling(new CreatePlayer.RequestResponder(entityManager, entity, request), createPlayerDelegates, logger);
                }
            }

            [InjectableId(InjectableType.CommandResponseHandler, 13000)]
            internal class CommandResponseHandlerCreator : IInjectableCreator
            {
                public IInjectable CreateInjectable(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                {
                    return new CommandResponseHandler(entity, entityManager, logDispatcher);
                }
            }

            [InjectableId(InjectableType.CommandResponseHandler, 13000)]
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

                private readonly List<Action<CreatePlayer.ReceivedResponse>> createPlayerDelegates = new List<Action<CreatePlayer.ReceivedResponse>>();
                public event Action<CreatePlayer.ReceivedResponse> OnCreatePlayerResponse
                {
                    add
                    {
                        if (!IsValid())
                        {
                            return;
                        }

                        createPlayerDelegates.Add(value);
                    }
                    remove
                    {
                        if (!IsValid())
                        {
                            return;
                        }

                        createPlayerDelegates.Remove(value);
                    }
                }

                internal void OnCreatePlayerResponseInternal(CreatePlayer.ReceivedResponse response)
                {
                    GameObjectDelegates.DispatchWithErrorHandling(response, createPlayerDelegates, logger);
                }
            }
        }
    }
}
