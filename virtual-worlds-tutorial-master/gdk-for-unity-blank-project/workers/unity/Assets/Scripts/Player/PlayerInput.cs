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

        private bool selected = false;
        [SerializeField] private Sprite unselectedSprite;
        [SerializeField] private Sprite selectedSprite;

        // Adds event reader to match player position to server position
        private void OnEnable()
        {
            Debug.LogError("Init");
            positionReader.CoordsUpdated += OnPositionUpdate;
            GetComponentInChildren<SpriteRenderer>().color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                mousePosLastClickX = Input.mousePosition.x;
                mousePosLastClickY = Input.mousePosition.y;
                //Debug.Log("Set: x: " + mousePosLastClickX + ", y: " + mousePosLastClickY);

                if (selected)
                {
                    moveThisUpdate = true;
                }
                else
                {
                    var currentServerPos = positionReader.Data.Coords;
                    Vector3 currentServerPosVector = new Vector3((float)currentServerPos.X, (float)currentServerPos.Y, (float)currentServerPos.Z);
                    //Debug.Log(currentServerPosVector);
                    //Debug.Log(mousePosLastClickX + ", " + mousePosLastClickY);

                    var pos = new Vector3(Mathf.RoundToInt(mousePosLastClickX), Mathf.RoundToInt(mousePosLastClickY), 0);
                    pos = Camera.main.ScreenToWorldPoint(pos);
                    pos = new Vector3(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), 0);

                    //Debug.Log(pos);
                    if (pos.x == currentServerPosVector.x && pos.y == currentServerPosVector.y)
                    {
                        selected = true;
                    }
                    else
                    {
                        selected = false;
                    }
                }
            }

            if (selected)
            {
                if (selectedSprite != null)
                    GetComponentInChildren<SpriteRenderer>().sprite = selectedSprite;
            }
            else
            {
                if (unselectedSprite != null)
                    GetComponentInChildren<SpriteRenderer>().sprite = unselectedSprite;
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
                pos = new Vector3(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), 0);

                //Instantiate(particle, transform.position, transform.rotation);

                float x = Mathf.RoundToInt(pos.x); //Input.GetAxis("Horizontal");
                float y = Mathf.RoundToInt(pos.y); // Input.GetAxis("Vertical");

                inputWriter.SendMovementUpdate(new MovementUpdate(x, y));

                //Handle Deselect
                var currentServerPos = positionReader.Data.Coords;
                Vector3 currentServerPosVector = new Vector3((float)currentServerPos.X, (float)currentServerPos.Y, (float)currentServerPos.Z);
                if (!((Mathf.Abs(currentServerPosVector.x - pos.x) <= 1) && (Mathf.Abs(currentServerPosVector.y - pos.y) <= 1)))
                {
                    selected = false;
                }

                //Debug.Log("Sent: x: " + x + ", y: " + y);
            }
        }

        // Takes the position update, and applies it directly to the object
        void OnPositionUpdate(Coordinates update)
        {
            Vector3 newPos = new Vector3((float)update.X, (float)update.Y, (float)update.Z);
            gameObject.transform.position = newPos;
            //Debug.LogWarning("Position Updated");
            selected = false;
        }
    }
}