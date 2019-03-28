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

        private bool moveThisUpdate = false;
        private float mousePosLastClickX = 0f;
        private float mousePosLastClickY = 0f;

        // Adds event reader to match player position to server position
        private void OnEnable()
        {
            Debug.LogError("Init");
            positionReader.CoordsUpdated += OnPositionUpdate;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                mousePosLastClickX = Input.mousePosition.x;
                mousePosLastClickY = Input.mousePosition.y;
                //Debug.Log("Set: x: " + mousePosLastClickX + ", y: " + mousePosLastClickY);
                moveThisUpdate = true;
            }
            if (lastUpdate <= updateTime)
            {
                lastUpdate += Time.deltaTime;
                return;
            }

            lastUpdate = 0;

            if (moveThisUpdate)
            {
                moveThisUpdate = false;

                var pos = new Vector3(Mathf.RoundToInt(mousePosLastClickX), Mathf.RoundToInt(mousePosLastClickY), 0);
                pos = Camera.main.ScreenToWorldPoint(pos);

                //Instantiate(particle, transform.position, transform.rotation);

                float x = Mathf.RoundToInt(pos.x); //Input.GetAxis("Horizontal");
                float y = Mathf.RoundToInt(pos.y); // Input.GetAxis("Vertical");

                inputWriter.SendMovementUpdate(new MovementUpdate(x, y));

                Debug.Log("Sent: x: " + x + ", y: " + y);
            }
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