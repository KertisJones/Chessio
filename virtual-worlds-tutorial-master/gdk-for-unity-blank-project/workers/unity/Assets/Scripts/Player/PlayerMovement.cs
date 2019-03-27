using Improbable.Gdk.GameObjectRepresentation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Improbable.Player;
using Improbable;
using BlankProject;

namespace Assets.Scripts.Player
{
    [WorkerType(WorkerUtils.UnityGameLogic)]
    public class PlayerMovement : MonoBehaviour
    {
        [Require] private PlayerControls.Requirable.Reader inputReader;
        [Require] private Position.Requirable.Writer positionWriter;

        float speed = .5f;

        // Adds event reader to match player position to server position
        private void OnEnable()
        {
            Debug.LogWarning("Launch?");
            inputReader.OnMovementUpdate += OnControlUpdate;
        }

        // Takes the position update, and applies it directly to the object
        void OnControlUpdate(MovementUpdate update)
        {
            Debug.LogWarning("Update recieved");
            Vector2 control = new Vector2(update.X, update.Y);
            control.Normalize();
            control = control * speed;

            Vector3 newPos = this.transform.position;
            newPos.x = newPos.x + control.x;
            newPos.z = newPos.z + control.y;
            this.transform.position = newPos;

            Coordinates serverPos = new Coordinates((double)newPos.x, (double)newPos.y, (double)newPos.z);

            positionWriter.Send(new Position.Update
            {
                Coords = serverPos
            });
        }
    }
}