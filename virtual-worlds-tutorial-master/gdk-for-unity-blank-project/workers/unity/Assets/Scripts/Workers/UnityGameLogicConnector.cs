using Improbable;
using Improbable.Gdk.Core;
using Improbable.Gdk.GameObjectCreation;
using Improbable.Gdk.GameObjectRepresentation;
using Improbable.Gdk.PlayerLifecycle;
using Improbable.Gdk.TransformSynchronization;
using Improbable.Player;

namespace BlankProject
{
    public class UnityGameLogicConnector : DefaultWorkerConnector
    {
        public const string WorkerType = WorkerUtils.UnityGameLogic;

        private async void Start()
        {
            PlayerLifecycleConfig.CreatePlayerEntityTemplate = CreatePlayerEntityTemplate;
            await Connect(WorkerType, new ForwardingDispatcher()).ConfigureAwait(false);
        }

        protected override void HandleWorkerConnectionEstablished()
        {
            Worker.World.GetOrCreateManager<MetricSendSystem>();
            PlayerLifecycleHelper.AddServerSystems(Worker.World);
            GameObjectRepresentationHelper.AddSystems(Worker.World);
            GameObjectCreationHelper.EnableStandardGameObjectCreation(Worker.World);
        }

        private static EntityTemplate CreatePlayerEntityTemplate(string workerId, Improbable.Vector3f position)
        {
            var clientAttribute = $"workerId:{workerId}";
            var serverAttribute = WorkerType;

            var template = new EntityTemplate();
            template.AddComponent(new Position.Snapshot(), serverAttribute);
            template.AddComponent(new PlayerControls.Snapshot(), clientAttribute);
            template.AddComponent(new Metadata.Snapshot { EntityType = "Player" }, serverAttribute);
            TransformSynchronizationHelper.AddTransformSynchronizationComponents(template, serverAttribute);
            PlayerLifecycleHelper.AddPlayerLifecycleComponents(template, workerId, clientAttribute, serverAttribute);

            template.SetReadAccess(UnityClientConnector.WorkerType, AndroidClientWorkerConnector.WorkerType, iOSClientWorkerConnector.WorkerType, serverAttribute);
            template.SetComponentWriteAccess(EntityAcl.ComponentId, serverAttribute);

            return template;
        }
    }
}
