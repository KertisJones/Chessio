using Improbable.Gdk.GameObjectRepresentation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Improbable.Player;
using Improbable;
using BlankProject;

namespace Assets.Scripts.Player
{
    [WorkerType(WorkerUtils.UnityClient)]
    public class PlayerInput : MonoBehaviour
    {
        [Require] private PlayerControls.Requirable.Writer inputWriter;
        [Require] private Position.Requirable.Reader positionReader;

        float updateTime = .1f;
        float lastUpdate = 0f;

        // Adds event reader to match player position to server position
        private void OnEnable()
        {
            Debug.LogError("Init");
            positionReader.CoordsUpdated += OnPositionUpdate;
        }

        // Update is called once per frame
        void Update()
        {
            if (lastUpdate <= updateTime)
            {
                lastUpdate += Time.deltaTime;
                return;
            }

            lastUpdate = 0;

            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");

            inputWriter.SendMovementUpdate(new MovementUpdate(x, y));
            Debug.LogError("Sent");
        }

        // Takes the position update, and applies it directly to the object
        void OnPositionUpdate(Coordinates update)
        {
            Vector3 newPos = new Vector3((float)update.X, (float)update.Y, (float)update.Z);
            gameObject.transform.position = newPos;
            //Debug.LogWarning("Position Updated");
        }
    }
}